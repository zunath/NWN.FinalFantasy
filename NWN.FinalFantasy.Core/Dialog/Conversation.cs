using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Core.Dialog
{
    public static class Conversation
    {
        private const int NumberOfDialogs = 255;
        private static readonly Dictionary<Guid, PlayerDialog> _playerDialogs = new Dictionary<Guid, PlayerDialog>();
        private static readonly Dictionary<int, bool> _dialogsInUse = new Dictionary<int, bool>();

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
            var playerID = GetGlobalID(player);

            if (!_playerDialogs.ContainsKey(playerID))
            {
                Load(player, talkTo, @class);
            }

            var dialog = _playerDialogs[playerID];
            if (GetObjectType(talkTo) == ObjectType.Creature &&
                !GetIsPlayer(talkTo) &&
                !GetIsDungeonMaster(talkTo))
            {
                BeginConversation("dialog" + dialog.DialogID, talkTo);
            }
            // Everything else
            else
            {
                AssignCommand(player, () =>
                {
                    ActionStartConversation(talkTo, "dialog" + dialog.DialogID, true, false);
                });
            }
        }

        /// <summary>
        /// Removes an active dialog from the cache. Ensure you call this whenever the conversation is closed.
        /// </summary>
        /// <param name="player">The player whose dialog we're removing</param>
        public static void End(NWGameObject player)
        {
            var playerID = GetGlobalID(player);
            if (!_playerDialogs.ContainsKey(playerID))
            {
                throw new Exception($"Cannot find player ID in active dialogs: {playerID} ({GetName(player)})");
            }

            var dialog = _playerDialogs[playerID];
            _dialogsInUse[dialog.DialogID] = false;
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
            var playerID = GetGlobalID(player);
            var conversation = FindConversation(@class);
            var playerDialog = new PlayerDialog("MainPage");
            conversation.SetUp(player, playerDialog);

            playerDialog.ActiveDialogName = @class;
            playerDialog.DialogTarget = talkTo;

            var dialogID = RetrieveInactiveDialogID();
            playerDialog.DialogID = dialogID;

            _playerDialogs[playerID] = playerDialog;
            _dialogsInUse[dialogID] = true;
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
            var type = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies()
                .Select(Assembly.Load)
                .SelectMany(x => x.DefinedTypes)
                .SingleOrDefault(x => x.Namespace + "." + x.Name == @namespace);

            if (type == null)
                throw new Exception("Could not location conversation at path: " + @class);

            var conversation = (IConversation)Activator.CreateInstance(type);
            return conversation;
        }

        /// <summary>
        /// Retrieves the ID of the first inactive dialog.
        /// </summary>
        /// <returns>The ID of the first inactive dialog.</returns>
        private static int RetrieveInactiveDialogID()
        {
            for (int x = 1; x <= NumberOfDialogs; x++)
            {
                if (!_dialogsInUse[x])
                    return x;
            }

            throw new Exception("Unable to find an unused dialog. Add more dialog files and increase the constant in Conversation.cs");
        }
    }
}
