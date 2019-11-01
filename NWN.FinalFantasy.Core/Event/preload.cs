using System;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public static class preload
    {
        public static void Main()
        {
            RegisterModuleEvents();
        }

        private static void RegisterModuleEvents()
        {
            SetEventScript(GetModule(), EventScriptModule.OnAcquireItem, "mod_on_acquire");
            SetEventScript(GetModule(), EventScriptModule.OnActivateItem, "mod_on_activate");
            SetEventScript(GetModule(), EventScriptModule.OnClientEnter, "mod_on_enter");
            SetEventScript(GetModule(), EventScriptModule.OnClientExit, "mod_on_leave");
            SetEventScript(GetModule(), EventScriptModule.OnPlayerCancelCutscene, "mod_on_csabort");
            SetEventScript(GetModule(), EventScriptModule.OnHeartbeat, "mod_on_heartbeat");
            SetEventScript(GetModule(), EventScriptModule.OnPlayerChat, "mod_on_chat");
            SetEventScript(GetModule(), EventScriptModule.OnPlayerDeath, "mod_on_death");
            SetEventScript(GetModule(), EventScriptModule.OnPlayerDying, "mod_on_dying");
            SetEventScript(GetModule(), EventScriptModule.OnEquipItem, "mod_on_equip");
            SetEventScript(GetModule(), EventScriptModule.OnPlayerLevelUp, "mod_on_levelup");
            SetEventScript(GetModule(), EventScriptModule.OnRespawnButtonPressed, "mod_on_respawn");
            SetEventScript(GetModule(), EventScriptModule.OnPlayerRest, "mod_on_rest");
            SetEventScript(GetModule(), EventScriptModule.OnUnequipItem, "mod_on_unequip");
            SetEventScript(GetModule(), EventScriptModule.OnLoseItem, "mod_on_unacquire");
            SetEventScript(GetModule(), EventScriptModule.OnUserDefinedEvent, "mod_on_user");
        }
    }
}
