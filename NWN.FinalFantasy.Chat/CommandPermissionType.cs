using System;

namespace NWN.FinalFantasy.Chat
{
    [Flags]
    internal enum CommandPermissionType
    {
        Player = 1,
        DM = 2,
        Admin = 4
    }
}
