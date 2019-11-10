using System;
using NWN.FinalFantasy.Authorization;
using NWN.FinalFantasy.Core.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Chat.Command
{
    [CommandDetails("Displays all chat commands available to you.", CommandPermissionType.DM | CommandPermissionType.Admin | CommandPermissionType.Player)]
    public class Help: IChatCommand
    {
        /// <summary>
        /// Displays all the chat commands available to a user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="target"></param>
        /// <param name="targetLocation"></param>
        /// <param name="args"></param>
        public void DoAction(NWGameObject user, NWGameObject target, Location targetLocation, params string[] args)
        {
            var authorization = AuthorizationRegistry.GetAuthorizationLevel(user);

            Console.WriteLine("Running: auth = " + authorization);

            if (authorization == AuthorizationLevel.DM)
            {
                SendMessageToPC(user, ChatCommandRegistry.HelpTextDM);
            }
            else if (authorization == AuthorizationLevel.Admin)
            {
                SendMessageToPC(user, ChatCommandRegistry.HelpTextAdmin);
            }
            else
            {
                SendMessageToPC(user, ChatCommandRegistry.HelpTextPlayer);
            }
        }

        public string ValidateArguments(NWGameObject user, params string[] args)
        {
            return string.Empty;
        }

        public bool RequiresTarget => false;
    }
}
