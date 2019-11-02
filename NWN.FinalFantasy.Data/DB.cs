using System;
using Newtonsoft.Json;
using NWN.FinalFantasy.Core.NWNX;

namespace NWN.FinalFantasy.Data
{
    public static class DB
    {
        /// <summary>
        /// Stores a specific object in the database by its ID.
        /// </summary>
        /// <typeparam name="T">The type of data to store.</typeparam>
        /// <param name="entity">The data to store</param>
        public static void Set<T>(T entity)
            where T: EntityBase
        {
            var @namespace = typeof(T).FullName;
            var id = entity.ID.ToString();
            var key = @namespace + ":" + id;
            var data = JsonConvert.SerializeObject(entity);

            NWNXRedis.Set(key, data);
        }

        /// <summary>
        /// Retrieves a specific object in the database by its ID.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="id">The ID of the entity</param>
        /// <returns></returns>
        public static T Get<T>(Guid id)
            where T: EntityBase
        {
            var @namespace = typeof(T).FullName;
            var key = @namespace + ":" + id;

            var json = NWNXRedis.Get(key);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
