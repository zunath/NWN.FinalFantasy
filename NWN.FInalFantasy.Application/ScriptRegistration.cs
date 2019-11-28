using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Dialog;
using NWN.FinalFantasy.Core.Event.Prefix;
using NWN.FinalFantasy.Core.Logging;
using static NWN._;

namespace NWN.FinalFantasy.Application
{
    public static class ScriptRegistration
    {
        private class ScriptContext
        {
            public bool SourceIsModule { get; }
            public string Prefix { get; }

            public ScriptContext(bool sourceIsModule, string prefix)
            {
                SourceIsModule = sourceIsModule;
                Prefix = prefix;
            }
        }


        private const int SCRIPT_HANDLED = 0;
        private const int SCRIPT_NOT_HANDLED = -1;

        private static readonly Dictionary<string, ScriptContext> _eventRegistrations = new Dictionary<string, ScriptContext>();
        private static readonly Dictionary<string, Func<int>> _dialogEventRegistrations = new Dictionary<string, Func<int>>();
        private static Action _startupAction;

        public static void RegisterStartupAction(Action startupAction)
        {
            _startupAction = startupAction;
        }

        // This is called every time a named script is scheduled to run.
        // oidSelf is the object running the script (OBJECT_SELF), and script
        // is the name given to the event handler (e.g. via SetEventScript).
        // If the script is not handled in the managed code, and needs to be
        // forwarded to the original NWScript VM, return SCRIPT_NOT_HANDLED.
        // Otherwise, return either 0/SCRIPT_HANDLED for void main() scripts,
        // or an int (0 or 1) for StartingConditionals
        public static int OnRunScript(string script, uint oidSelf)
        {
            if (script == "startup")
            {
                _startupAction();
                return SCRIPT_HANDLED;
            }

            // Is this one of the scripts we've registered in the OnStart() method below?
            if (_eventRegistrations.ContainsKey(script))
            {
                try
                {
                    var context = _eventRegistrations[script];
                    var source = context.SourceIsModule ? GetModule() : oidSelf;

                    Console.WriteLine("Self = " + GetName(oidSelf) + ", script = " + script + ", source = " + GetName(source));
                    Script.RunScriptEvents(source, context.Prefix);
                    return SCRIPT_HANDLED;
                }
                catch (Exception ex)
                {
                    Audit.Write(AuditGroup.Error, ex.ToMessageAndCompleteStacktrace());
                    return SCRIPT_NOT_HANDLED;
                }
            }

            // Is this a conditional script (such as the dialog AppearsWhen event)?
            if (_dialogEventRegistrations.ContainsKey(script))
            {
                try
                {
                    return _dialogEventRegistrations[script]();
                }
                catch (Exception ex)
                {
                    Audit.Write(AuditGroup.Error, ex.ToMessageAndCompleteStacktrace());
                    return SCRIPT_NOT_HANDLED;
                }
            }

            // We don't have a registration, let the NWN script handle it, if it exists.
            return SCRIPT_NOT_HANDLED;
        }

        public static void HookEvents()
        {
            Console.WriteLine("Hooking events: " + NWGameObject.OBJECT_SELF.Self);

            // Area events
            _eventRegistrations.Add("area_on_enter", new ScriptContext(false, AreaPrefix.OnEnter));
            _eventRegistrations.Add("area_on_exit", new ScriptContext(false, AreaPrefix.OnExit));
            _eventRegistrations.Add("area_on_hb", new ScriptContext(false, AreaPrefix.OnHeartbeat));
            _eventRegistrations.Add("area_on_user", new ScriptContext(false, AreaPrefix.OnUserDefined));

            // AreaOfEffect events
            _eventRegistrations.Add("aoe_on_enter", new ScriptContext(false, AreaOfEffectPrefix.OnEnter));
            _eventRegistrations.Add("aoe_on_exit", new ScriptContext(false, AreaOfEffectPrefix.OnExit));
            _eventRegistrations.Add("aoe_on_hb", new ScriptContext(false, AreaOfEffectPrefix.OnHeartbeat));
            _eventRegistrations.Add("aoe_on_userdef", new ScriptContext(false, AreaOfEffectPrefix.OnUserDefined));

            // Creature events
            _eventRegistrations.Add("crea_on_attacked", new ScriptContext(false, CreaturePrefix.OnAttacked));
            _eventRegistrations.Add("crea_on_blocked", new ScriptContext(false, CreaturePrefix.OnBlocked));
            _eventRegistrations.Add("crea_on_convo", new ScriptContext(false, CreaturePrefix.OnConversation));
            _eventRegistrations.Add("crea_on_damaged", new ScriptContext(false, CreaturePrefix.OnDamaged));
            _eventRegistrations.Add("crea_on_death", new ScriptContext(false, CreaturePrefix.OnDeath));
            _eventRegistrations.Add("crea_on_disturb", new ScriptContext(false, CreaturePrefix.OnDisturbed));
            _eventRegistrations.Add("crea_on_hb", new ScriptContext(false, CreaturePrefix.OnHeartbeat));
            _eventRegistrations.Add("crea_on_percept", new ScriptContext(false, CreaturePrefix.OnPerception));
            _eventRegistrations.Add("crea_on_rested", new ScriptContext(false, CreaturePrefix.OnRested));
            _eventRegistrations.Add("crea_on_roundend", new ScriptContext(false, CreaturePrefix.OnCombatRoundEnd));
            _eventRegistrations.Add("crea_on_spawn", new ScriptContext(false, CreaturePrefix.OnSpawn));
            _eventRegistrations.Add("crea_on_splcast", new ScriptContext(false, CreaturePrefix.OnSpellCastAt));
            _eventRegistrations.Add("crea_on_userdef", new ScriptContext(false, CreaturePrefix.OnUserDefined));

            // Dialog events
            //_eventRegistrations.Add("dialog_start", OnDialogStart.Run);
            //_eventRegistrations.Add("dialog_end", OnDialogEnd.Run);
            //_eventRegistrations.Add("dialog_action_0", () => OnDialogAction.Run(0));
            //_eventRegistrations.Add("dialog_action_1", () => OnDialogAction.Run(1));
            //_eventRegistrations.Add("dialog_action_2", () => OnDialogAction.Run(2));
            //_eventRegistrations.Add("dialog_action_3", () => OnDialogAction.Run(3));
            //_eventRegistrations.Add("dialog_action_4", () => OnDialogAction.Run(4));
            //_eventRegistrations.Add("dialog_action_5", () => OnDialogAction.Run(5));
            //_eventRegistrations.Add("dialog_action_6", () => OnDialogAction.Run(6));
            //_eventRegistrations.Add("dialog_action_7", () => OnDialogAction.Run(7));
            //_eventRegistrations.Add("dialog_action_8", () => OnDialogAction.Run(8));
            //_eventRegistrations.Add("dialog_action_9", () => OnDialogAction.Run(9));
            //_eventRegistrations.Add("dialog_action_10", () => OnDialogAction.Run(10));
            //_eventRegistrations.Add("dialog_action_11", () => OnDialogAction.Run(11));
            //_eventRegistrations.Add("dialog_action_n", () => OnDialogAction.Run(12));
            //_eventRegistrations.Add("dialog_action_p", () => OnDialogAction.Run(13));
            //_eventRegistrations.Add("dialog_action_b", () => OnDialogAction.Run(14));
            _dialogEventRegistrations.Add("dialog_appears_0", () => OnDialogAppears.Run(2, 0));
            _dialogEventRegistrations.Add("dialog_appears_1", () => OnDialogAppears.Run(2, 1));
            _dialogEventRegistrations.Add("dialog_appears_2", () => OnDialogAppears.Run(2, 2));
            _dialogEventRegistrations.Add("dialog_appears_3", () => OnDialogAppears.Run(2, 3));
            _dialogEventRegistrations.Add("dialog_appears_4", () => OnDialogAppears.Run(2, 4));
            _dialogEventRegistrations.Add("dialog_appears_5", () => OnDialogAppears.Run(2, 5));
            _dialogEventRegistrations.Add("dialog_appears_6", () => OnDialogAppears.Run(2, 6));
            _dialogEventRegistrations.Add("dialog_appears_7", () => OnDialogAppears.Run(2, 7));
            _dialogEventRegistrations.Add("dialog_appears_8", () => OnDialogAppears.Run(2, 8));
            _dialogEventRegistrations.Add("dialog_appears_9", () => OnDialogAppears.Run(2, 9));
            _dialogEventRegistrations.Add("dialog_appears10", () => OnDialogAppears.Run(2, 10));
            _dialogEventRegistrations.Add("dialog_appears11", () => OnDialogAppears.Run(2, 11));
            _dialogEventRegistrations.Add("dialog_appears_h", () => OnDialogAppears.Run(1, 0));
            _dialogEventRegistrations.Add("dialog_appears_n", () => OnDialogAppears.Run(3, 12));
            _dialogEventRegistrations.Add("dialog_appears_p", () => OnDialogAppears.Run(4, 13));
            _dialogEventRegistrations.Add("dialog_appears_b", () => OnDialogAppears.Run(5, 14));

            // DM events
            _eventRegistrations.Add("dm_appear", new ScriptContext(true, DMScriptPrefix.OnAppear));
            _eventRegistrations.Add("dm_change_diff", new ScriptContext(true, DMScriptPrefix.OnChangeDifficulty));
            _eventRegistrations.Add("dm_disab_trap", new ScriptContext(true, DMScriptPrefix.OnDisableTrap));
            _eventRegistrations.Add("dm_disappear", new ScriptContext(true, DMScriptPrefix.OnDisappear));
            _eventRegistrations.Add("dm_force_rest", new ScriptContext(true, DMScriptPrefix.OnForceRest));
            _eventRegistrations.Add("dm_get_var", new ScriptContext(true, DMScriptPrefix.OnGetVariable));
            _eventRegistrations.Add("dm_give_gold", new ScriptContext(true, DMScriptPrefix.OnGiveGold));
            _eventRegistrations.Add("dm_give_item", new ScriptContext(true, DMScriptPrefix.OnGiveItem));
            _eventRegistrations.Add("dm_give_level", new ScriptContext(true, DMScriptPrefix.OnGiveLevel));
            _eventRegistrations.Add("dm_give_xp", new ScriptContext(true, DMScriptPrefix.OnGiveXP));
            _eventRegistrations.Add("dm_heal", new ScriptContext(true, DMScriptPrefix.OnHeal));
            _eventRegistrations.Add("dm_jump", new ScriptContext(true, DMScriptPrefix.OnJump));
            _eventRegistrations.Add("dm_jump_all", new ScriptContext(true, DMScriptPrefix.OnJumpAll));
            _eventRegistrations.Add("dm_jump_target", new ScriptContext(true, DMScriptPrefix.OnJumpTarget));
            _eventRegistrations.Add("dm_kill", new ScriptContext(true, DMScriptPrefix.OnKill));
            _eventRegistrations.Add("dm_limbo", new ScriptContext(true, DMScriptPrefix.OnLimbo));
            _eventRegistrations.Add("dm_possess", new ScriptContext(true, DMScriptPrefix.OnPossess));
            _eventRegistrations.Add("dm_set_date", new ScriptContext(true, DMScriptPrefix.OnSetDate));
            _eventRegistrations.Add("dm_set_stat", new ScriptContext(true, DMScriptPrefix.OnSetStat));
            _eventRegistrations.Add("dm_set_time", new ScriptContext(true, DMScriptPrefix.OnSetTime));
            _eventRegistrations.Add("dm_set_var", new ScriptContext(true, DMScriptPrefix.OnSetVariable));
            _eventRegistrations.Add("dm_spawn", new ScriptContext(true, DMScriptPrefix.OnSpawnObject));
            _eventRegistrations.Add("dm_take_item", new ScriptContext(true, DMScriptPrefix.OnTakeItem));
            _eventRegistrations.Add("dm_togg_immo", new ScriptContext(true, DMScriptPrefix.OnToggleImmortality));
            _eventRegistrations.Add("dm_toggle_ai", new ScriptContext(true, DMScriptPrefix.OnToggleAI));
            _eventRegistrations.Add("dm_toggle_invuln", new ScriptContext(true, DMScriptPrefix.OnToggleInvulnerability));
            _eventRegistrations.Add("dm_toggle_lock", new ScriptContext(true, DMScriptPrefix.OnToggleLock));

            // Door events
            _eventRegistrations.Add("door_on_attacked", new ScriptContext(false, DoorPrefix.OnAttacked));
            _eventRegistrations.Add("door_on_click", new ScriptContext(false, DoorPrefix.OnClicked));
            _eventRegistrations.Add("door_on_close", new ScriptContext(false, DoorPrefix.OnClosed));
            _eventRegistrations.Add("door_on_convo", new ScriptContext(false, DoorPrefix.OnConversation));
            _eventRegistrations.Add("door_on_damage", new ScriptContext(false, DoorPrefix.OnDamaged));
            _eventRegistrations.Add("door_on_death", new ScriptContext(false, DoorPrefix.OnDeath));
            _eventRegistrations.Add("door_on_disarm", new ScriptContext(false, DoorPrefix.OnDisarmed));
            _eventRegistrations.Add("door_on_fail", new ScriptContext(false, DoorPrefix.OnFailToOpen));
            _eventRegistrations.Add("door_on_hb", new ScriptContext(false, DoorPrefix.OnHeartbeat));
            _eventRegistrations.Add("door_on_lock", new ScriptContext(false, DoorPrefix.OnLocked));
            _eventRegistrations.Add("door_on_open", new ScriptContext(false, DoorPrefix.OnOpened));
            _eventRegistrations.Add("door_on_splcast", new ScriptContext(false, DoorPrefix.OnSpellCastAt));
            _eventRegistrations.Add("door_on_trap", new ScriptContext(false, DoorPrefix.OnTriggerTrap));
            _eventRegistrations.Add("door_on_unlock", new ScriptContext(false, DoorPrefix.OnUnlock));
            _eventRegistrations.Add("door_on_userdef", new ScriptContext(false, DoorPrefix.OnUserDefined));

            // Encounter events
            _eventRegistrations.Add("enc_on_enter", new ScriptContext(false, EncounterPrefix.OnEnter));
            _eventRegistrations.Add("enc_on_exhaust", new ScriptContext(false, EncounterPrefix.OnExhausted));
            _eventRegistrations.Add("enc_on_exit", new ScriptContext(false, EncounterPrefix.OnExit));
            _eventRegistrations.Add("enc_on_hb", new ScriptContext(false, EncounterPrefix.OnHeartbeat));
            _eventRegistrations.Add("enc_on_userdef", new ScriptContext(false, EncounterPrefix.OnUserDefined));

            // Inventory events
            _eventRegistrations.Add("inv_add_item", new ScriptContext(false, InventoryPrefix.OnAddItem));
            _eventRegistrations.Add("inv_rem_item", new ScriptContext(false, InventoryPrefix.OnRemoveItem));

            // Item events
            _eventRegistrations.Add("item_on_hit", new ScriptContext(true, ItemEventPrefix.OnItemHitCastSpell));

            // Module events (NWNX)
            _eventRegistrations.Add("mod_nwnx_equip", new ScriptContext(true, ModulePrefix.OnNWNXEquipItem));
            _eventRegistrations.Add("mod_nwnx_unequip", new ScriptContext(true, ModulePrefix.OnNWNXUnequipItem));
            _eventRegistrations.Add("mod_on_attack", new ScriptContext(true, ModulePrefix.OnAttack));
            _eventRegistrations.Add("mod_on_examine", new ScriptContext(true, ModulePrefix.OnExamine));
            _eventRegistrations.Add("mod_on_nwnxchat", new ScriptContext(true, ModulePrefix.OnNWNXChat));
            _eventRegistrations.Add("mod_on_usefeat", new ScriptContext(true, ModulePrefix.OnUseFeat));
            _eventRegistrations.Add("mod_on_useitem", new ScriptContext(true, ModulePrefix.OnUseItem));

            // Module events (Non-NWNX)
            _eventRegistrations.Add("mod_on_acquire", new ScriptContext(false, ModulePrefix.OnAcquireItem));
            _eventRegistrations.Add("mod_on_activate", new ScriptContext(false, ModulePrefix.OnActivateItem));
            _eventRegistrations.Add("mod_on_chat", new ScriptContext(false, ModulePrefix.OnPlayerChat));
            _eventRegistrations.Add("mod_on_csabort", new ScriptContext(false, ModulePrefix.OnCutsceneAbort));
            _eventRegistrations.Add("mod_on_death", new ScriptContext(false, ModulePrefix.OnPlayerDeath));
            _eventRegistrations.Add("mod_on_dying", new ScriptContext(false, ModulePrefix.OnPlayerDying));
            _eventRegistrations.Add("mod_on_enter", new ScriptContext(false, ModulePrefix.OnPlayerEnter));
            _eventRegistrations.Add("mod_on_equip", new ScriptContext(false, ModulePrefix.OnEquipItem));
            _eventRegistrations.Add("mod_on_heartbeat", new ScriptContext(false, ModulePrefix.OnHeartbeat));
            _eventRegistrations.Add("mod_on_leave", new ScriptContext(false, ModulePrefix.OnPlayerLeave));
            _eventRegistrations.Add("mod_on_levelup", new ScriptContext(false, ModulePrefix.OnPlayerLevelUp));
            _eventRegistrations.Add("mod_on_load", new ScriptContext(false, ModulePrefix.OnLoad));
            _eventRegistrations.Add("mod_on_respawn", new ScriptContext(false, ModulePrefix.OnPlayerRespawn));
            _eventRegistrations.Add("mod_on_rest", new ScriptContext(false, ModulePrefix.OnPlayerRest));
            _eventRegistrations.Add("mod_on_unacquire", new ScriptContext(false, ModulePrefix.OnUnacquireItem));
            _eventRegistrations.Add("mod_on_unequip", new ScriptContext(false, ModulePrefix.OnUnequipItem));
            _eventRegistrations.Add("mod_on_user", new ScriptContext(false, ModulePrefix.OnUserDefined));

            // Placeable events
            _eventRegistrations.Add("plc_on_attack", new ScriptContext(false, PlaceablePrefix.OnAttacked));
            _eventRegistrations.Add("plc_on_click", new ScriptContext(false, PlaceablePrefix.OnClicked));
            _eventRegistrations.Add("plc_on_close", new ScriptContext(false, PlaceablePrefix.OnClosed));
            _eventRegistrations.Add("plc_on_convo", new ScriptContext(false, PlaceablePrefix.OnConversation));
            _eventRegistrations.Add("plc_on_damage", new ScriptContext(false, PlaceablePrefix.OnDamaged));
            _eventRegistrations.Add("plc_on_death", new ScriptContext(false, PlaceablePrefix.OnDeath));
            _eventRegistrations.Add("plc_on_disarm", new ScriptContext(false, PlaceablePrefix.OnDisarmed));
            _eventRegistrations.Add("plc_on_disturb", new ScriptContext(false, PlaceablePrefix.OnDisturbed));
            _eventRegistrations.Add("plc_on_hb", new ScriptContext(false, PlaceablePrefix.OnHeartbeat));
            _eventRegistrations.Add("plc_on_lock", new ScriptContext(false, PlaceablePrefix.OnLocked));
            _eventRegistrations.Add("plc_on_open", new ScriptContext(false, PlaceablePrefix.OnOpened));
            _eventRegistrations.Add("plc_on_splcast", new ScriptContext(false, PlaceablePrefix.OnSpellCastAt));
            _eventRegistrations.Add("plc_on_trap", new ScriptContext(false, PlaceablePrefix.OnTriggerTrap));
            _eventRegistrations.Add("plc_on_unlock", new ScriptContext(false, PlaceablePrefix.OnUnlocked));
            _eventRegistrations.Add("plc_on_used", new ScriptContext(false, PlaceablePrefix.OnUsed));
            _eventRegistrations.Add("plc_on_userdef", new ScriptContext(false, PlaceablePrefix.OnUserDefined));

            // Server events
            _eventRegistrations.Add("server_on_connec", new ScriptContext(true, ServerEventPrefix.OnConnect));
            _eventRegistrations.Add("server_on_discon", new ScriptContext(true, ServerEventPrefix.OnDisconnect));

            // Store events
            _eventRegistrations.Add("store_on_close", new ScriptContext(false, StorePrefix.OnClose));
            _eventRegistrations.Add("store_on_open", new ScriptContext(false, StorePrefix.OnOpen));

            // Trigger events
            _eventRegistrations.Add("trig_on_click", new ScriptContext(false, TriggerPrefix.OnClicked));
            _eventRegistrations.Add("trig_on_disarm", new ScriptContext(false, TriggerPrefix.OnDisarmed));
            _eventRegistrations.Add("trig_on_enter", new ScriptContext(false, TriggerPrefix.OnEnter));
            _eventRegistrations.Add("trig_on_exit", new ScriptContext(false, TriggerPrefix.OnExit));
            _eventRegistrations.Add("trig_on_hb", new ScriptContext(false, TriggerPrefix.OnHeartbeat));
            _eventRegistrations.Add("trig_on_trap", new ScriptContext(false, TriggerPrefix.OnTriggerTrap));
            _eventRegistrations.Add("trig_on_userdef", new ScriptContext(false, TriggerPrefix.OnUserDefined));
        }
    }
}
