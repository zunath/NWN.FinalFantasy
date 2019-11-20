using System;
using System.Linq;
using System.Reflection;
using NWN.FinalFantasy.Authorization;
using NWN.FinalFantasy.Chat.Command;
using NWN.FinalFantasy.Core.Enumerations;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Core.Utility;
using static NWN._;

namespace NWN.FinalFantasy.Chat
{
    public class OnNWNXChat
    {
        public static void Main()
        {
            NWGameObject sender = NWGameObject.OBJECT_SELF;
            string originalMessage = NWNXChat.GetMessage().Trim();

            if (!CanHandleChat(sender, originalMessage))
            {
                return;
            }

            var split = originalMessage.Split(' ').ToList();

            // Commands with no arguments won't be split, so if we didn't split anything then add the command to the split list manually.
            if (split.Count <= 0)
                split.Add(originalMessage);

            split[0] = split[0].ToLower();
            string command = split[0].Substring(1, split[0].Length - 1);
            split.RemoveAt(0);

            NWNXChat.SkipMessage();

            if (!ChatCommandRegistry.IsRegistered(command))
            {
                SendMessageToPC(sender, ColorToken.Red("Invalid chat command. Use '/help' to get a list of available commands."));
                return;
            }

            IChatCommand chatCommand = ChatCommandRegistry.GetChatCommandHandler(command);
            string args = string.Join(" ", split);

            if (!chatCommand.RequiresTarget)
            {
                ProcessChatCommand(chatCommand, sender, null, null, args);
            }
            else
            {
                string error = chatCommand.ValidateArguments(sender, split.ToArray());
                if (!string.IsNullOrWhiteSpace(error))
                {
                    SendMessageToPC(sender, error);
                    return;
                }

                SetLocalString(sender, "CHAT_COMMAND", command);
                SetLocalString(sender, "CHAT_COMMAND_ARGS", command);
                SendMessageToPC(sender, "Please use your 'Chat Command Targeter' feat to select the target of this chat command.");

                if (!GetHasFeat(Feat.ChatCommandTargeter, sender)  || GetIsDungeonMaster(sender))
                {
                    NWNXCreature.AddFeatByLevel(sender, Feat.ChatCommandTargeter, 1);

                    if (GetIsDungeonMaster(sender))
                    {
                        var qbs = NWNXPlayer.GetQuickBarSlot(sender, 11);
                        if (qbs.ObjectType == QuickBarSlotType.Empty)
                        {
                            NWNXPlayer.SetQuickBarSlot(sender, 11, NWNXPlayerQuickBarSlot.UseFeat(Feat.ChatCommandTargeter));
                        }
                    }
                }
            }

        }

        private static bool CanHandleChat(NWGameObject sender, string message)
        {
            bool validTarget = GetIsPlayer(sender) || GetIsDungeonMaster(sender);
            bool validMessage = message.Length >= 2 && message[0] == '/' && message[1] != '/';
            return validTarget && validMessage;
        }


        private static void ProcessChatCommand(IChatCommand command, NWGameObject sender, NWGameObject target, Location targetLocation, string args)
        {
            if (target == null)
            {
                target = new NWGameObject();
            }

            if (targetLocation == null)
            {
                targetLocation = new Location(IntPtr.Zero);
            }

            CommandDetailsAttribute attribute = command.GetType().GetCustomAttribute<CommandDetailsAttribute>();
            var authorization = AuthorizationRegistry.GetAuthorizationLevel(sender);

            if (attribute != null &&
                (attribute.Permissions.HasFlag(CommandPermissionType.Player) && authorization == AuthorizationLevel.Player ||
                 attribute.Permissions.HasFlag(CommandPermissionType.DM) && authorization == AuthorizationLevel.DM ||
                 attribute.Permissions.HasFlag(CommandPermissionType.Admin) && authorization == AuthorizationLevel.Admin))
            {
                string[] argsArr = string.IsNullOrWhiteSpace(args) ? new string[0] : args.Split(' ').ToArray();
                string error = command.ValidateArguments(sender, argsArr);

                if (!string.IsNullOrWhiteSpace(error))
                {
                    SendMessageToPC(sender, error);
                }
                else
                {
                    command.DoAction(sender, target, targetLocation, argsArr);
                }
            }
            else
            {
                SendMessageToPC(sender, ColorToken.Red("Invalid chat command. Use '/help' to get a list of available commands."));
            }
        }
    }
}
