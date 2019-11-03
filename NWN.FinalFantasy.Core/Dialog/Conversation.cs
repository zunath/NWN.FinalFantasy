using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Core.Startup;

namespace NWN.FinalFantasy.Core.Dialog
{
    public static class Conversation
    {
        private static readonly Dictionary<Guid, PlayerDialog> _playerDialogs = new Dictionary<Guid, PlayerDialog>();

        /// <summary>
        /// Starts a new programmatic conversation between a player and another object.
        /// @class should exclude the root namespace of the solution. (I.E: NWN.FinalFantasy).
        /// For example, if you want to find the NWN.FinalFantasy.Menu.RestMenu class you would only pass in Menu.RestMenu for @class.
        /// If the class cannot be found, an exception will be thrown.
        /// </summary>
        /// <param name="player">The player involved in the conversation</param>
        /// <param name="talkTo">The target object the player is speaking to</param>
        /// <param name="class">The C# class to search for.</param>
        public static void Start(NWGameObject player, NWGameObject talkTo, string @class)
        {
            var playerID = _.GetGlobalID(player);

            if (!_playerDialogs.ContainsKey(playerID))
            {
                Load(player, talkTo, @class);
            }

            if (_.GetObjectType(talkTo) == ObjectType.Creature &&
                !_.GetIsPlayer(talkTo) &&
                !_.GetIsDungeonMaster(talkTo))
            {
                _.BeginConversation("dialog", new NWGameObject());
            }
            // Everything else
            else
            {
                _.AssignCommand(player, () =>
                {
                    _.ActionStartConversation(talkTo, "dialog", true, false);
                });
            }
        }

        /// <summary>
        /// Removes an active dialog from the cache. Ensure you call this whenever the conversation is closed.
        /// </summary>
        /// <param name="player">The player whose dialog we're removing</param>
        public static void End(NWGameObject player)
        {
            var playerID = _.GetGlobalID(player);
            if (!_playerDialogs.ContainsKey(playerID))
            {
                throw new Exception($"Cannot find player ID in active dialogs: {playerID} ({_.GetName(player)})");
            }

            _playerDialogs.Remove(playerID);
        }

        /// <summary>
        /// Loads and stores a player's active dialog into the cache.
        /// </summary>
        /// <param name="player">The player whose dialog file we're making and storing</param>
        /// <param name="talkTo">Who the player is talking to</param>
        /// <param name="class">The class name of the conversation</param>
        public static void Load(NWGameObject player, NWGameObject talkTo, string @class)
        {
            var playerID = _.GetGlobalID(player);
            var conversation = FindConversation(@class);
            var playerDialog = new PlayerDialog("MainPage");
            conversation.SetUp(player, playerDialog);

            playerDialog.ActiveDialogName = @class;
            playerDialog.DialogTarget = talkTo;

            _playerDialogs[playerID] = playerDialog;
        }

        /// <summary>
        /// Retrieves the active player dialog for a given player.
        /// If player is not currently involved in a dialog an exception will be thrown.
        /// </summary>
        /// <param name="playerID">The ID of the player currently involved in a dialog</param>
        /// <returns>The active player dialog</returns>
        public static PlayerDialog GetActivePlayerDialog(Guid playerID)
        {
            if (!_playerDialogs.ContainsKey(playerID))
                throw new Exception($"Player ID {playerID} does not have an active dialog.");

            return _playerDialogs[playerID];
        }

        /// <summary>
        /// Looks for a conversation class matching the @class argument.
        /// @class should exclude the root namespace of the solution. (I.E: NWN.FinalFantasy).
        /// For example, if you want to find the NWN.FinalFantasy.Menu.RestMenu class you would only pass in Menu.RestMenu for @class.
        /// If the class cannot be found, an exception will be thrown.
        /// </summary>
        /// <param name="class">The class to search for</param>
        /// <returns>The conversation found which matches the @class</returns>
        public static IConversation FindConversation(string @class)
        {
            var settings = ApplicationSettings.Get();
            var @namespace = settings.NamespaceRoot + "." + @class;
            var type = AssemblyLoader.FindType(@namespace);

            if(type == null)
                throw new Exception("Could not location conversation at path: " + @class);

            var conversation = (IConversation)Activator.CreateInstance(type);
            return conversation;
        }

    }
}
