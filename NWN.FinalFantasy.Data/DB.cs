using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data
{
    public static class DB
    {
        /// <summary>
        /// Builds a key for the standard get/set entity operations.
        /// </summary>
        /// <param name="guid">The ID of the entity</param>
        /// <param name="type">The type of entity we're working with.</param>
        /// <returns>A key used for setting/getting objects</returns>
        private static string BuildKey(Guid guid, Type type)
        {
            // If we've got a list, get the underlying type of list
            // for use when building the key.
            if (type.GetGenericArguments().Length > 0)
            {
                type = type.GetGenericArguments()[0];
            }

            var id = guid.ToString();
            var @namespace = type.Namespace + "." + type.Name;
            var key = @namespace + ":" + id;

            return key;
        }

        /// <summary>
        /// Stores a specific object in the database by its ID.
        /// </summary>
        /// <typeparam name="T">The type of data to store.</typeparam>
        /// <param name="entity">The data to store</param>
        public static void Set<T>(T entity)
            where T: EntityBase
        {
            var key = BuildKey(entity.ID, typeof(T));
            var data = JsonConvert.SerializeObject(entity);

            NWNXRedis.Set(key, data);
        }

        /// <summary>
        /// Stores a specific object in the database by an arbitrary key.
        /// </summary>
        /// <typeparam name="T">The type of data to store</typeparam>
        /// <param name="entity">The data to store.</param>
        /// <param name="key">The arbitrary key to set this object under.</param>
        public static void Set<T>(string key, T entity)
            where T: EntityBase
        {
            var data = JsonConvert.SerializeObject(entity);
            NWNXRedis.Set(key, data);
        }

        /// <summary>
        /// Retrieves a specific object in the database by its ID.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="id">The ID of the entity</param>
        /// <returns>An entity matching that ID</returns>
        public static T Get<T>(Guid id)
            where T: EntityBase
        {
            var key = BuildKey(id, typeof(T));
            var json = NWNXRedis.Get(key);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Retrieves a specific object in the database by an arbitrary key.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="key">The arbitrary key the data is stored under</param>
        /// <returns>The object stored in the database under the specified key</returns>
        public static T Get<T>(string key)
        {
            var json = NWNXRedis.Get(key);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Retrieves a list of objects stored under the same ID from the database.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="id">The ID of the entity list</param>
        /// <returns>An entity list matching that ID</returns>
        public static EntityList<T> GetList<T>(Guid id)
            where T: EntityBase
        {
            var key = BuildKey(id, typeof(T));
            var json = NWNXRedis.Get(key);
            return JsonConvert.DeserializeObject<EntityList<T>>(json);
        }

        /// <summary>
        /// Retrieves a list of objects stored under the specified key from the database. 
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        /// <param name="key">The arbitrary key the data is stored under.</param>
        /// <returns></returns>
        public static EntityList<T> GetList<T>(string key)
            where T: EntityBase
        {
            var json = NWNXRedis.Get(key);
            return JsonConvert.DeserializeObject<EntityList<T>>(json);
        }

        /// <summary>
        /// Returns true if an entry with the specified ID exists.
        /// Returns false if not.
        /// </summary>
        /// <typeparam name="T">The type of data to check the existence of</typeparam>
        /// <param name="id">The ID of the entity</param>
        /// <returns>true if found, false otherwise</returns>
        public static bool Exists<T>(Guid id)
        {
            var key = BuildKey(id, typeof(T));
            return NWNXRedis.Exists(key);
        }

        /// <summary>
        /// Returns true if an entry with the specified key exists.
        /// Returns false if not.
        /// </summary>
        /// <param name="key">The key of the entity.</param>
        /// <returns>true if found, false otherwise.</returns>
        public static bool Exists(string key)
        {
            return NWNXRedis.Exists(key);
        }
    }
}
