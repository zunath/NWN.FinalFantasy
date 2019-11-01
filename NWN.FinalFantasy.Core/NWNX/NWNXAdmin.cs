namespace NWN.FinalFantasy.Core.NWNX
{
    public static class NWNXAdmin
    {
        /// <summary>
        /// Gets the current player password.
        /// </summary>
        /// <returns></returns>
        public static string GetPlayerPassword()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "GET_PLAYER_PASSWORD");
            return NWNXCore.NWNX_GetReturnValueString("NWNX_Administration", "GET_PLAYER_PASSWORD");
        }

        /// <summary>
        /// Sets the current player password.
        /// </summary>
        /// <param name="password"></param>
        public static void SetPlayerPassword(string password)
        {
            if (password == null) password = string.Empty;

            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "SET_PLAYER_PASSWORD", password);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "SET_PLAYER_PASSWORD");
        }

        /// <summary>
        /// Removes the current player password.
        /// </summary>
        public static void ClearPlayerPassword()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "CLEAR_PLAYER_PASSWORD");
        }

        /// <summary>
        /// Gets the current DM password.
        /// </summary>
        /// <returns></returns>
        public static string GetDMPassword()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "GET_DM_PASSWORD");
            return NWNXCore.NWNX_GetReturnValueString("NWNX_Administration", "GET_DM_PASSWORD");
        }

        /// <summary>
        /// Sets the current DM password. May be set to an empty string.
        /// </summary>
        /// <param name="password"></param>
        public static void SetDMPassword(string password)
        {
            if (password == null) password = string.Empty;

            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "SET_DM_PASSWORD", password);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "SET_DM_PASSWORD");
        }

        /// <summary>
        /// Signals the server to immediately shut down.
        /// </summary>
        public static void ShutdownServer()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "SHUTDOWN_SERVER");
        }

        /// <summary>
        /// Deletes the player character from the servervault
        /// The PC will be immediately booted from the game with a "Delete Character" message
        /// </summary>
        /// <param name="pc">The player character to boot.</param>
        /// <param name="bPreserveBackup">If true, it will leave the files on the server and append ".deleted0" to the bic file name.</param>
        public static void DeletePlayerCharacter(NWGameObject pc, bool bPreserveBackup)
        {
            NWNXCore.NWNX_PushArgumentInt("NWNX_Administration", "DELETE_PLAYER_CHARACTER", bPreserveBackup ? 1 : 0);
            NWNXCore.NWNX_PushArgumentObject("NWNX_Administration", "DELETE_PLAYER_CHARACTER", pc);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "DELETE_PLAYER_CHARACTER");
        }

        /// <summary>
        /// Ban a given IP - get via GetPCIPAddress()
        /// </summary>
        /// <param name="ip"></param>
        public static void AddBannedIP(string ip)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "ADD_BANNED_IP", ip);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "ADD_BANNED_IP");
        }

        /// <summary>
        /// Removes a banned IP address.
        /// </summary>
        /// <param name="ip"></param>
        public static void RemoveBannedIP(string ip)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "REMOVE_BANNED_IP", ip);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "REMOVE_BANNED_IP");
        }

        /// <summary>
        /// Adds a banned CD key. Get via GetPCPublicCDKey
        /// </summary>
        /// <param name="key"></param>
        public static void AddBannedCDKey(string key)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "ADD_BANNED_CDKEY", key);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "ADD_BANNED_CDKEY");
        }

        /// <summary>
        /// Removes a banned CD key.
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveBannedCDKey(string key)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "REMOVE_BANNED_CDKEY", key);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "REMOVE_BANNED_CDKEY");
        }

        /// <summary>
        /// Adds a banned player name - get via GetPCPlayerName.
        /// NOTE: Players can easily change their names.
        /// </summary>
        /// <param name="playername"></param>
        public static void AddBannedPlayerName(string playername)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "ADD_BANNED_PLAYER_NAME", playername);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "ADD_BANNED_PLAYER_NAME");
        }

        /// <summary>
        /// Removes a banned player name.
        /// </summary>
        /// <param name="playername"></param>
        public static void RemoveBannedPlayerName(string playername)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "REMOVE_BANNED_PLAYER_NAME", playername);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "REMOVE_BANNED_PLAYER_NAME");
        }

        /// <summary>
        /// Gets a list of all banned IPs, CD Keys, and player names as a string.
        /// </summary>
        /// <returns></returns>
        public static string GetBannedList()
        {
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "GET_BANNED_LIST");
            return NWNXCore.NWNX_GetReturnValueString("NWNX_Administration", "GET_BANNED_LIST");
        }


        /// <summary>
        /// Sets the module's name as shown in the server list.
        /// </summary>
        /// <param name="name"></param>
        public static void SetModuleName(string name)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "SET_MODULE_NAME", name);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "SET_MODULE_NAME");
        }

        /// <summary>
        /// Sets the server's name as shown in the server list.
        /// </summary>
        /// <param name="name"></param>
        public static void SetServerName(string name)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "SET_SERVER_NAME", name);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "SET_SERVER_NAME");
        }

        /// <summary>
        /// Get an AdministrationOption value
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static int GetPlayOption(AdministrationOption option)
        {
            NWNXCore.NWNX_PushArgumentInt("NWNX_Administration", "GET_PLAY_OPTION", (int)option);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "GET_PLAY_OPTION");

            return NWNXCore.NWNX_GetReturnValueInt("NWNX_Administration", "GET_PLAY_OPTION");
        }

        /// <summary>
        /// Set an AdministrationOption value
        /// </summary>
        /// <param name="option"></param>
        /// <param name="value"></param>
        public static void SetPlayOption(AdministrationOption option, int value)
        {
            NWNXCore.NWNX_PushArgumentInt("NWNX_Administration", "SET_PLAY_OPTION", value);
            NWNXCore.NWNX_PushArgumentInt("NWNX_Administration", "SET_PLAY_OPTION", (int)option);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "SET_PLAY_OPTION");
        }

        /// <summary>
        /// Delete the temporary user resource data (TURD) of a playerName + characterName
        /// </summary>
        /// <param name="playerName">Name of the player's user account</param>
        /// <param name="characterName">Name of the character</param>
        public static void DeleteTURD(string playerName, string characterName)
        {
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "DELETE_TURD", characterName);
            NWNXCore.NWNX_PushArgumentString("NWNX_Administration", "DELETE_TURD", playerName);
            NWNXCore.NWNX_CallFunction("NWNX_Administration", "DELETE_TURD");
        }
    }
}
