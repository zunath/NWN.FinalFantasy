using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWN.FinalFantasy.Chat.Command;

namespace NWN.FinalFantasy.Chat
{
    internal class ChatCommandRegistry
    {
        private static readonly Dictionary<string, IChatCommand> _chatCommands = new Dictionary<string, IChatCommand>();

        public static void Register()
        {
            // Use reflection to get all of IChatCommand handler implementations.
            var classes = Assembly.GetCallingAssembly().GetTypes()
                .Where(p => typeof(IChatCommand).IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).ToArray();

            foreach (var type in classes)
            {
                IChatCommand instance = Activator.CreateInstance(type) as IChatCommand;

                if (instance == null)
                {
                    throw new NullReferenceException("Unable to activate instance of type: " + type);
                }
                // We use the lower-case class name as the key because later on we do a lookup based on text entered by the player.
                string key = type.Name.ToLower();
                _chatCommands.Add(key, instance);
            }
        }

        public static bool IsRegistered(string commandName)
        {
            commandName = commandName.ToLower();
            return _chatCommands.ContainsKey(commandName);
        }

        public static IChatCommand GetChatCommandHandler(string commandName)
        {
            commandName = commandName.ToLower();
            if (!_chatCommands.ContainsKey(commandName))
            {
                throw new ArgumentException("Chat command handler '" + commandName + "' is not registered.");
            }

            return _chatCommands[commandName];
        }
    }
}
