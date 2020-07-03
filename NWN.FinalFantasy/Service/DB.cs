using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Entity;
using StackExchange.Redis;

namespace NWN.FinalFantasy.Service
{
    internal class DB
    {
        private static readonly Dictionary<Type, string> _keyPrefixByType = new Dictionary<Type, string>();
        private static ConnectionMultiplexer _multiplexer;

        [NWNEventHandler("mod_preload")]
        public static void Load()
        {
            var appSettings = ApplicationSettings.Get();
            _multiplexer = ConnectionMultiplexer.Connect(appSettings.RedisIPAddress);
            LoadKeyPrefixes();
        }

        /// <summary>
        /// When initialized, the assembly will be searched for all implementations of the EntityBase object.
        /// The KeyPrefix value of each of these will be stored into a dictionary for quick retrievals later.
        /// This is intended to abstract key building away from the consumer of this class.
        /// </summary>
        private static void LoadKeyPrefixes()
        {
            var entityInstances = typeof(EntityBase)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(EntityBase)) && !t.IsAbstract && !t.IsGenericType)
                .Select(t => (EntityBase)Activator.CreateInstance(t));

            foreach (var entity in entityInstances)
            {
                // Register the type by itself first.
                _keyPrefixByType[entity.GetType()] = entity.KeyPrefix;
                Console.WriteLine($"Registered type '{entity.GetType()}' using key prefix {entity.KeyPrefix}");
            }
        }

        /// <summary>
        /// Stores a specific object in the database by an arbitrary key.
        /// </summary>
        /// <typeparam name="T">The type of data to store</typeparam>
        /// <param name="entity">The data to store.</param>
        /// <param name="key">The arbitrary key to set this object under.</param>
        /// <param name="keyPrefixOverride">If null, the key prefix defined on the entity will be used. Otherwise, this value will be used as the prefix.</param>
        public static void Set<T>(string key, T entity, string keyPrefixOverride = null)
            where T : EntityBase
        {
            if (string.IsNullOrWhiteSpace(keyPrefixOverride))
            {
                keyPrefixOverride = _keyPrefixByType[typeof(T)];
            }

            var data = JsonConvert.SerializeObject(entity);
            _multiplexer.GetDatabase().StringSet($"{keyPrefixOverride}:{key}", data);
        }

        /// <summary>
        /// Stores a list of objects in the database by an arbitrary key.
        /// </summary>
        /// <typeparam name="T">The type of data to store.</typeparam>
        /// <param name="key">The arbitrary key to set this object under.</param>
        /// <param name="entities">The list of entities to store.</param>
        /// <param name="keyPrefix">The key prefix to store the data under. Must be specified or call will fail</param>
        public static void SetList<T>(string key, EntityList<T> entities, string keyPrefix)
            where T: EntityBase
        {
            if(string.IsNullOrWhiteSpace(keyPrefix))
                throw new ArgumentException($"{nameof(keyPrefix)} cannot be null or whitespace.");

            var data = JsonConvert.SerializeObject(entities);
            _multiplexer.GetDatabase().StringSet($"{keyPrefix}:{key}", data);
        }

        /// <summary>
        /// Retrieves a specific object in the database by an arbitrary key.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="key">The arbitrary key the data is stored under</param>
        /// <param name="keyPrefixOverride">If null, the key prefix defined on the entity will be used. Otherwise, this value will be used as the prefix.</param>
        /// <returns>The object stored in the database under the specified key</returns>
        public static T Get<T>(string key, string keyPrefixOverride = null)
        {
            if (string.IsNullOrWhiteSpace(keyPrefixOverride))
            {
                keyPrefixOverride = _keyPrefixByType[typeof(T)];
            }

            var json = _multiplexer.GetDatabase().StringGet($"{keyPrefixOverride}:{key}");
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Retrieves a list of objects stored under the specified key from the database. 
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        /// <param name="key">The arbitrary key the data is stored under.</param>
        /// <param name="keyPrefix">The key prefix to store the data under. Must be specified or call will fail</param>
        /// <returns>A list of entities.</returns>
        public static EntityList<T> GetList<T>(string key, string keyPrefix)
            where T : EntityBase
        {
            if(string.IsNullOrWhiteSpace(keyPrefix))
                throw new ArgumentException($"{nameof(keyPrefix)} cannot be null or whitespace.");

            var json = _multiplexer.GetDatabase().StringGet($"{keyPrefix}:{key}");
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonConvert.DeserializeObject<EntityList<T>>(json);
        }

        /// <summary>
        /// Returns true if an entry with the specified key exists.
        /// Returns false if not.
        /// </summary>
        /// <param name="key">The key of the entity.</param>
        /// <param name="keyPrefixOverride">If null, the key prefix defined on the entity will be used. Otherwise, this value will be used as the prefix.</param>
        /// <returns>true if found, false otherwise.</returns>
        public static bool Exists<T>(string key, string keyPrefixOverride = null)
        {
            if (keyPrefixOverride == null)
            {
                keyPrefixOverride = _keyPrefixByType[typeof(T)];
            }

            return _multiplexer.GetDatabase().KeyExists($"{keyPrefixOverride}:{key}");
        }

        /// <summary>
        /// Deletes an entry by a specified key.
        /// </summary>
        /// <typeparam name="T">The type of entity to delete.</typeparam>
        /// <param name="key">The key of the entity</param>
        /// <param name="keyPrefixOverride">If null, the key prefix defined on the entity will be used. Otherwise, this value will be used as the prefix.</param>
        public static void Delete<T>(string key, string keyPrefixOverride = null)
        {
            if (keyPrefixOverride == null)
            {
                keyPrefixOverride = _keyPrefixByType[typeof(T)];
            }

            _multiplexer.GetDatabase().KeyDelete($"{keyPrefixOverride}:{key}");
        }
    }
}
