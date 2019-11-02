using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace NWN.FinalFantasy.Data
{
    public static class DB
    {
        private static ConnectionMultiplexer Connection { get; }

        static DB()
        {
            var settings = new ApplicationSettings();
            var uri = settings.Host + ":" + settings.Port;
            Connection = ConnectionMultiplexer.Connect(uri);
        }

        /// <summary>
        /// Subscribes to a Redis event. Whenever an event matching this object type is raised, the specified action will be executed.
        /// These events may come internally from the server OR external sources like the website or Discord.
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <param name="action">The action to run when the event is raised.</param>
        public static void Subscribe<T>(Action<T> action)
            where T: class
        {
            var sub = Connection.GetSubscriber();
            var channel = typeof(T).FullName;

            sub.Subscribe(channel, (c, message) =>
            {
                T payload = JsonConvert.DeserializeObject<T>(message);
                action(payload);
            });
        }

        /// <summary>
        /// Publishes a new Redis event. All subscribers currently listening to this event will execute their actions.
        /// </summary>
        /// <typeparam name="T">The event type</typeparam>
        /// <param name="payload">The data to transfer to subscribers listening for this event.</param>
        public static void Publish<T>(T payload)
        {
            var sub = Connection.GetSubscriber();
            var channel = typeof(T).FullName;
            var data = JsonConvert.SerializeObject(payload);

            sub.Publish(channel, data);
        }

        /// <summary>
        /// Stores a specific object in the database by its ID.
        /// </summary>
        /// <typeparam name="T">The type of data to store.</typeparam>
        /// <param name="entity">The data to store</param>
        public static void Set<T>(T entity)
            where T: Entity
        {
            var @namespace = typeof(T).FullName;
            var id = entity.ID.ToString();
            var key = @namespace + "::" + id;
            var data = JsonConvert.SerializeObject(entity);

            var db = Connection.GetDatabase();
            var success = db.StringSet(key, data);

            if(!success)
                throw new Exception($"Failed to set value for key '{key}");
        }

        /// <summary>
        /// Retrieves a specific object in the database by its ID.
        /// </summary>
        /// <typeparam name="T">The type of data to retrieve</typeparam>
        /// <param name="id">The ID of the entity</param>
        /// <returns></returns>
        public static T Get<T>(Guid id)
            where T: Entity
        {
            var @namespace = typeof(T).FullName;
            var key = @namespace + "::" + id;
            var db = Connection.GetDatabase();

            var json = db.StringGet(key);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
