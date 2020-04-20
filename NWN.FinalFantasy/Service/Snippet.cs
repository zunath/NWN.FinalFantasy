using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWN.FinalFantasy.Core;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service
{
    public class SnippetAttribute : Attribute
    {
        public string Name { get; }

        public SnippetAttribute(string name)
        {
            Name = name;
        }
    }

    public static class Snippet
    {
        /// <summary>
        /// This dictionary tracks the original text of a node for a specific player.
        /// Note that this doesn't ever clean up because there's not a reliable way to know when conversations created by builders end.
        /// In theory this shouldn't cause too many memory issues because conversations are only so big.
        /// </summary>
        private static readonly Dictionary<uint, Dictionary<int, string>> _playerNodeOriginalText = new Dictionary<uint, Dictionary<int, string>>();

        private delegate bool AppearsWhenDelegate(uint player, string[] args);
        private delegate void ActionsTakenDelegate(uint player, string[] args);

        private static readonly Dictionary<string, AppearsWhenDelegate> _appearsWhenCommands = new Dictionary<string, AppearsWhenDelegate>();
        private static readonly Dictionary<string, ActionsTakenDelegate> _actionsTakenCommands = new Dictionary<string, ActionsTakenDelegate>();

        /// <summary>
        /// When the module loads, all available conversation commands are loaded into the cache.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void RegisterConversationCommands()
        {
            var methods = Assembly.GetExecutingAssembly()
                .GetTypes()
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(SnippetAttribute), false).Length > 0)
                .ToArray();

            foreach (var mi in methods)
            {
                foreach (var attr in mi.GetCustomAttributes(typeof(SnippetAttribute), false))
                {
                    var name = ((SnippetAttribute)attr).Name.ToLower();
                    if (name.StartsWith("action"))
                    {
                        Console.WriteLine();
                        _actionsTakenCommands[name] = (ActionsTakenDelegate)mi.CreateDelegate(typeof(ActionsTakenDelegate));
                    }
                    else if (name.StartsWith("condition"))
                    {
                        _appearsWhenCommands[name] = (AppearsWhenDelegate)mi.CreateDelegate(typeof(AppearsWhenDelegate));
                    }
                }
            }
        }
        /// <summary>
        /// When a conversation node with this script assigned in the "Appears When" event is run,
        /// check for any conversation conditions and process them.
        /// </summary>
        /// <returns></returns>
        [NWNEventHandler("appears")]
        public static bool ConversationAppearsWhen()
        {
            var player = GetPCSpeaker();
            var text = Core.NWNX.Dialog.GetCurrentNodeText();
            var nodeId = Core.NWNX.Dialog.GetCurrentNodeID();
            if(!_playerNodeOriginalText.ContainsKey(player))
                _playerNodeOriginalText[player] = new Dictionary<int, string>();

            _playerNodeOriginalText[player][nodeId] = text;
            return ProcessConditions(player, text);
        }

        /// <summary>
        /// When a conversation node with this script assigned in the "Actions Taken" event is run,
        /// check for any conversation actions and process them.
        /// </summary>
        [NWNEventHandler("action")]
        public static void ConversationAction()
        {
            var player = GetPCSpeaker();
            var nodeId = Core.NWNX.Dialog.GetCurrentNodeID();
            ProcessActions(player, nodeId);
        }

        /// <summary>
        /// Handles processing condition commands.
        /// If any of the conditions fail, false will be returned.
        /// </summary>
        /// <param name="player">The player running the conditions.</param>
        /// <param name="text">The conversation message text.</param>
        /// <returns>true if all commands passed successfully, false otherwise</returns>
        private static bool ProcessConditions(uint player, string text)
        {
            var (commands, newText) = ParseCommand(text);
            if (commands.Count <= 0) return true;

            Core.NWNX.Dialog.SetCurrentNodeText(newText);

            // Iterate over each command. If the command doesn't exist or is on the wrong node, print out a warning to the log.
            foreach (var command in commands)
            {
                if (!command.ToLower().StartsWith("condition"))
                {
                    continue;
                }

                var args = command.Split(' ').ToList();
                var commandText = args[0];
                args.RemoveAt(0);
                if (!_appearsWhenCommands.ContainsKey(commandText))
                {
                    var error = $"Conversation snippet '{commandText}' is not valid. Make sure you typed the command correctly.";
                    SendMessageToPC(player, error);
                    Log.Write(LogGroup.Error, error);
                    continue;
                }

                // The first command that fails will result in failure.
                var commandResult = _appearsWhenCommands[commandText](player, args.ToArray());
                if (!commandResult) return false;
            }

            // By this point, all commands have run and they've all returned true.
            return true;
        }

        /// <summary>
        /// Handles processing action commands.
        /// </summary>
        /// <param name="player">The player to run the commands against</param>
        /// <param name="nodeId">The conversation node to execute.</param>
        private static void ProcessActions(uint player, int nodeId)
        {
            if (!_playerNodeOriginalText.ContainsKey(player) || !_playerNodeOriginalText[player].ContainsKey(nodeId))
                return;

            var originalText = _playerNodeOriginalText[player][nodeId];
            var (commands,_)= ParseCommand(originalText);
            if (commands.Count <= 0) return;


            foreach (var command in commands)
            {
                if (!command.ToLower().StartsWith("action"))
                {
                    continue;
                }

                var args = command.Split(' ').ToList();
                var commandText = args[0];
                args.RemoveAt(0);
                if (!_actionsTakenCommands.ContainsKey(commandText))
                {
                    var error = $"Conversation snippet '{commandText}' is not valid. Make sure you typed the command correctly.";
                    SendMessageToPC(player, error);
                    Log.Write(LogGroup.Error, error);
                    continue;
                }

                // The first command that fails will result in failure.
                _actionsTakenCommands[commandText](player, args.ToArray());
            }
        }


        /// <summary>
        /// Converts a node's text to lower, then parses out any commands wrapped with "{{" and "}}" brackets.
        /// Commands returned exclude the brackets.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>A list of commands, or an empty list if none can be found.</returns>
        private static (List<string>, string) ParseCommand(string text)
        {
            static (string, string) GetCommandString(string commandText)
            {
                // Commands are wrapped in {{ and }} brackets. 
                const string OpeningBracket = "{{";
                const string ClosingBracket = "}}";

                // If a complete command isn't wrapped, we exit early.
                if (!commandText.Contains(OpeningBracket) || !commandText.Contains(ClosingBracket)) return (string.Empty, commandText);

                // Parse out the command, excluding the brackets.
                var startIndex = commandText.IndexOf(OpeningBracket, StringComparison.Ordinal) + OpeningBracket.Length;
                var length = commandText.IndexOf(ClosingBracket, StringComparison.Ordinal) - commandText.IndexOf(OpeningBracket, StringComparison.Ordinal) - OpeningBracket.Length;
                var parsedCommand = commandText.Substring(startIndex, length).Trim();

                // Modify the full text so we don't pick up this command on the next iteration.
                var modifiedText = commandText.Remove(startIndex - OpeningBracket.Length, length + OpeningBracket.Length + ClosingBracket.Length);

                return (parsedCommand, modifiedText);
            }

            // Parse out the text and look for commands.
            var commands = new List<string>();
            var (command, newText) = GetCommandString(text);
            while (!string.IsNullOrWhiteSpace(command))
            {
                commands.Add(command);
                text = newText;
                (command, newText) = GetCommandString(text);
            }

            return (commands, newText);
        }

    }
}
