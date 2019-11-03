using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data
{
    public static class DB
    {
        private static string BuildKey(Guid guid, Type type)
        {
            // If we've got a list, get the underlying type of list
            // for use when building the key.
            if (type.GetGenericArguments().Length > 0)
            {
                Console.WriteLine("got a list");
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
    }
}
