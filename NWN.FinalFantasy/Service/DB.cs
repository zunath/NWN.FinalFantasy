using Newtonsoft.Json;
using NWN.FinalFantasy.Entity;
using StackExchange.Redis;

namespace NWN.FinalFantasy.Service
{
    internal static class DB
    {
        private static readonly ConnectionMultiplexer _multiplexer;

        static DB()
        {
            var appSettings = ApplicationSettings.Get();

            _multiplexer = ConnectionMultiplexer.Connect(appSettings.RedisIPAddress);
        }

        /// <summary>
        /// Stores a specific object in the database by an arbitrary key.
        /// </summary>
        /// <typeparam name="T">The type of data to store</typeparam>
        /// <param name="entity">The data to store.</param>
        /// <param name="key">The arbitrary key to set this object under.</param>
        public static void Set<T>(string key, T entity)
            where T : EntityBase
        {
            var data = JsonConvert.SerializeObject(entity);
            _multiplexer.GetDatabase().StringSet(key, data);
        }

        /// <summary>
        /// Retrieves a specific object in the database by an arbitrary key.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="key">The arbitrary key the data is stored under</param>
        /// <returns>The object stored in the database under the specified key</returns>
        public static T Get<T>(string key)
        {
            var json = _multiplexer.GetDatabase().StringGet(key);
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Retrieves a list of objects stored under the specified key from the database. 
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve.</typeparam>
        /// <param name="key">The arbitrary key the data is stored under.</param>
        /// <returns></returns>
        public static EntityList<T> GetList<T>(string key)
            where T : EntityBase
        {
            var json = _multiplexer.GetDatabase().StringGet(key);
            if (string.IsNullOrWhiteSpace(json))
                return default;

            return JsonConvert.DeserializeObject<EntityList<T>>(json);
        }

        /// <summary>
        /// Returns true if an entry with the specified key exists.
        /// Returns false if not.
        /// </summary>
        /// <param name="key">The key of the entity.</param>
        /// <returns>true if found, false otherwise.</returns>
        public static bool Exists(string key)
        {
            return _multiplexer.GetDatabase().KeyExists(key);
        }
    }
}
