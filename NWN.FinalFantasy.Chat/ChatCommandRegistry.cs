using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWN.FinalFantasy.Chat.Command;
using NWN.FinalFantasy.Core.Utility;

namespace NWN.FinalFantasy.Chat
{
    internal class ChatCommandRegistry
    {
        private static readonly Dictionary<string, IChatCommand> _chatCommands = new Dictionary<string, IChatCommand>();
        public static string HelpTextPlayer { get; private set; }
        public static string HelpTextDM { get; private set; }
        public static string HelpTextAdmin { get; private set; }

        public static void Initialize()
        {
            Register();
            BuildHelpText();
        }

        private static void Register()
        {
            var types = TypeFinder.GetTypesImplementingInterface<IChatCommand>();
            foreach (var classType in types)
            {
                IChatCommand instance = Activator.CreateInstance(classType) as IChatCommand;

                if (instance == null)
                {
                    throw new NullReferenceException("Unable to activate instance of type: " + classType);
                }
                // We use the lower-case class name as the key because later on we do a lookup based on text entered by the player.
                string key = classType.Name.ToLower();
                _chatCommands.Add(key, instance);
            }
        }

        private static void BuildHelpText()
        {
            foreach (var command in _chatCommands.Values)
            {
                var type = command.GetType();
                var attribute = type.GetCustomAttribute<CommandDetailsAttribute>();
                if (attribute == null) continue;

                if (attribute.Permissions.HasFlag(CommandPermissionType.Player))
                {
                    HelpTextPlayer += ColorToken.Green("/" + type.Name.ToLower()) + ColorToken.White(": " + attribute.Description) + "\n";
                }

                if (attribute.Permissions.HasFlag(CommandPermissionType.DM))
                {
                    HelpTextDM += ColorToken.Green("/" + type.Name.ToLower()) + ColorToken.White(": " + attribute.Description) + "\n";
                }

                if (attribute.Permissions.HasFlag(CommandPermissionType.Admin))
                {
                    HelpTextAdmin += ColorToken.Green("/" + type.Name.ToLower() + ColorToken.White(": " + attribute.Description) + "\n");
                }
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
