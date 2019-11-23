using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Dialog;
using NWN.FinalFantasy.Core.Event.Prefix;
using NWN.FinalFantasy.Core.Logging;

// ReSharper disable once CheckNamespace
namespace NWN.FinalFantasy.Application
{
    public class Entrypoints
    {
        public const int SCRIPT_HANDLED = 0;
        public const int SCRIPT_NOT_HANDLED = -1;

        private static readonly Dictionary<string, Action> _eventRegistrations = new Dictionary<string, Action>();
        private static readonly Dictionary<string, Func<int>> _dialogEventRegistrations = new Dictionary<string, Func<int>>();

        /// <summary>
        /// This is called once every main loop frame, outside of object context 
        /// </summary>
        /// <param name="frame">The current frame</param>
        public static void OnMainLoop(ulong frame)
        {

        }

        //
        // This is called every time a named script is scheduled to run.
        // oidSelf is the object running the script (OBJECT_SELF), and script
        // is the name given to the event handler (e.g. via SetEventScript).
        // If the script is not handled in the managed code, and needs to be
        // forwarded to the original NWScript VM, return SCRIPT_NOT_HANDLED.
        // Otherwise, return either 0/SCRIPT_HANDLED for void main() scripts,
        // or an int (0 or 1) for StartingConditionals
        //
        public static int OnRunScript(string script, uint oidSelf)
        {
            // Is this one of the scripts we've registered in the OnStart() method below?
            if(_eventRegistrations.ContainsKey(script))
            {
                try
                {
                    _eventRegistrations[script].Invoke();
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
                    return _dialogEventRegistrations[script].Invoke();
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

        /// <summary>
        /// This is called once when the internal structures have been initialized
        /// The module is not yet loaded, so most NWScript functions will fail if
        /// called here.
        /// </summary>
        public static void OnStart()
        {
            // Bootstrapping
            _eventRegistrations.Add("startup", Startup.Main);
            
            // Area events
            _eventRegistrations.Add("area_on_enter", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaPrefix.OnEnter));
            _eventRegistrations.Add("area_on_exit", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaPrefix.OnExit));
            _eventRegistrations.Add("area_on_hb", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaPrefix.OnHeartbeat));
            _eventRegistrations.Add("area_on_user", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaPrefix.OnUserDefined));

            // AreaOfEffect events
            _eventRegistrations.Add("aoe_on_enter", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaOfEffectPrefix.OnEnter));
            _eventRegistrations.Add("aoe_on_exit", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaOfEffectPrefix.OnExit));
            _eventRegistrations.Add("aoe_on_hb", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaOfEffectPrefix.OnHeartbeat));
            _eventRegistrations.Add("aoe_on_userdef", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, AreaOfEffectPrefix.OnUserDefined));

            // Creature events
            _eventRegistrations.Add("crea_on_attacked", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnAttacked));
            _eventRegistrations.Add("crea_on_blocked", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnBlocked));
            _eventRegistrations.Add("crea_on_convo", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnConversation));
            _eventRegistrations.Add("crea_on_damaged", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnDamaged));
            _eventRegistrations.Add("crea_on_death", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnDeath));
            _eventRegistrations.Add("crea_on_disturb", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnDisturbed));
            _eventRegistrations.Add("crea_on_hb", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnHeartbeat));
            _eventRegistrations.Add("crea_on_percept", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnPerception));
            _eventRegistrations.Add("crea_on_rested", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnRested));
            _eventRegistrations.Add("crea_on_roundend", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnCombatRoundEnd));
            _eventRegistrations.Add("crea_on_spawn", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnSpawn));
            _eventRegistrations.Add("crea_on_splcast", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnSpellCastAt));
            _eventRegistrations.Add("crea_on_userdef", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnUserDefined));

            // Dialog events
            _eventRegistrations.Add("dialog_start", OnDialogStart.Main);
            _eventRegistrations.Add("dialog_end", OnDialogEnd.Main);
            _eventRegistrations.Add("dialog_action_0", () => OnDialogAction.Run(0));
            _eventRegistrations.Add("dialog_action_1", () => OnDialogAction.Run(1));
            _eventRegistrations.Add("dialog_action_2", () => OnDialogAction.Run(2));
            _eventRegistrations.Add("dialog_action_3", () => OnDialogAction.Run(3));
            _eventRegistrations.Add("dialog_action_4", () => OnDialogAction.Run(4));
            _eventRegistrations.Add("dialog_action_5", () => OnDialogAction.Run(5));
            _eventRegistrations.Add("dialog_action_6", () => OnDialogAction.Run(6));
            _eventRegistrations.Add("dialog_action_7", () => OnDialogAction.Run(7));
            _eventRegistrations.Add("dialog_action_8", () => OnDialogAction.Run(8));
            _eventRegistrations.Add("dialog_action_9", () => OnDialogAction.Run(9));
            _eventRegistrations.Add("dialog_action_10", () => OnDialogAction.Run(10));
            _eventRegistrations.Add("dialog_action_11", () => OnDialogAction.Run(11));
            _eventRegistrations.Add("dialog_action_n", () => OnDialogAction.Run(12));
            _eventRegistrations.Add("dialog_action_p", () => OnDialogAction.Run(13));
            _eventRegistrations.Add("dialog_action_b", () => OnDialogAction.Run(14));
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
            _eventRegistrations.Add("dm_appear", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnAppear));
            _eventRegistrations.Add("dm_change_diff", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnChangeDifficulty));
            _eventRegistrations.Add("dm_disab_trap", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnDisableTrap));
            _eventRegistrations.Add("dm_disappear", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnDisappear));
            _eventRegistrations.Add("dm_force_rest", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnForceRest));
            _eventRegistrations.Add("dm_get_var", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnGetVariable));
            _eventRegistrations.Add("dm_give_gold", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnGiveGold));
            _eventRegistrations.Add("dm_give_item", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnGiveItem));
            _eventRegistrations.Add("dm_give_level", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnGiveLevel));
            _eventRegistrations.Add("dm_give_xp", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnGiveXP));
            _eventRegistrations.Add("dm_heal", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnHeal));
            _eventRegistrations.Add("dm_jump", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnJump));
            _eventRegistrations.Add("dm_jump_all", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnJumpAll));
            _eventRegistrations.Add("dm_jump_target", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnJumpTarget));
            _eventRegistrations.Add("dm_kill", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnKill));
            _eventRegistrations.Add("dm_limbo", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnLimbo));
            _eventRegistrations.Add("dm_possess", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnPossess));
            _eventRegistrations.Add("dm_set_date", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnSetDate));
            _eventRegistrations.Add("dm_set_stat", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnSetStat));
            _eventRegistrations.Add("dm_set_time", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnSetTime));
            _eventRegistrations.Add("dm_set_var", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnSetVariable));
            _eventRegistrations.Add("dm_spawn", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnSpawnObject));
            _eventRegistrations.Add("dm_take_item", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnTakeItem));
            _eventRegistrations.Add("dm_togg_immo", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnToggleImmortality));
            _eventRegistrations.Add("dm_toggle_ai", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnToggleAI));
            _eventRegistrations.Add("dm_toggle_invuln", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnToggleInvulnerability));
            _eventRegistrations.Add("dm_toggle_lock", () => Script.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnToggleLock));

            // Door events
            _eventRegistrations.Add("door_on_attacked", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnAttacked));
            _eventRegistrations.Add("door_on_click", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnClicked));
            _eventRegistrations.Add("door_on_close", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnClosed));
            _eventRegistrations.Add("door_on_convo", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnConversation));
            _eventRegistrations.Add("door_on_damage", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnDamaged));
            _eventRegistrations.Add("door_on_death", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnDeath));
            _eventRegistrations.Add("door_on_disarm", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnDisarmed));
            _eventRegistrations.Add("door_on_fail", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnFailToOpen));
            _eventRegistrations.Add("door_on_hb", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnHeartbeat));
            _eventRegistrations.Add("door_on_lock", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnLocked));
            _eventRegistrations.Add("door_on_open", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnOpened));
            _eventRegistrations.Add("door_on_splcast", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnSpellCastAt));
            _eventRegistrations.Add("door_on_trap", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnTriggerTrap));
            _eventRegistrations.Add("door_on_unlock", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnUnlock));
            _eventRegistrations.Add("door_on_userdef", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, DoorPrefix.OnUserDefined));

            // Encounter events
            _eventRegistrations.Add("enc_on_enter", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, EncounterPrefix.OnEnter));
            _eventRegistrations.Add("enc_on_exhaust", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, EncounterPrefix.OnExhausted));
            _eventRegistrations.Add("enc_on_exit", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, EncounterPrefix.OnExit));
            _eventRegistrations.Add("enc_on_hb", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, EncounterPrefix.OnHeartbeat));
            _eventRegistrations.Add("enc_on_userdef", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, EncounterPrefix.OnUserDefined));

            // Inventory events
            _eventRegistrations.Add("inv_add_item", () =>
            {
                if (!_.GetIsObjectValid(NWGameObject.OBJECT_SELF))
                {
                    return;
                }

                Script.RunScriptEvents(NWGameObject.OBJECT_SELF, InventoryPrefix.OnAddItem);
            });

            _eventRegistrations.Add("inv_rem_item", () =>
            {
                if (!_.GetIsObjectValid(NWGameObject.OBJECT_SELF))
                {
                    return;
                }

                Script.RunScriptEvents(NWGameObject.OBJECT_SELF, InventoryPrefix.OnRemoveItem);
            });

            // Item events
            _eventRegistrations.Add("item_on_hit", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ItemEventPrefix.OnItemHitCastSpell, _.GetModule()));

            // Module events (NWNX)
            _eventRegistrations.Add("mod_nwnx_equip", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnNWNXEquipItem, _.GetModule()));
            _eventRegistrations.Add("mod_nwnx_unequip", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnNWNXUnequipItem, _.GetModule()));
            _eventRegistrations.Add("mod_on_attack", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnAttack, _.GetModule()));
            _eventRegistrations.Add("mod_on_examine", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnExamine, _.GetModule()));
            _eventRegistrations.Add("mod_on_nwnxchat", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnNWNXChat));
            _eventRegistrations.Add("mod_on_usefeat", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnUseFeat, _.GetModule()));
            _eventRegistrations.Add("mod_on_useitem", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnUseItem));

            // Module events (Non-NWNX)
            _eventRegistrations.Add("mod_on_acquire", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnAcquireItem));
            _eventRegistrations.Add("mod_on_activate", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnActivateItem));
            _eventRegistrations.Add("mod_on_chat", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerChat));
            _eventRegistrations.Add("mod_on_csabort", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnCutsceneAbort));
            _eventRegistrations.Add("mod_on_death", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerDeath));
            _eventRegistrations.Add("mod_on_dying", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerDying));
            _eventRegistrations.Add("mod_on_enter", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerEnter));
            _eventRegistrations.Add("mod_on_equip", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnEquipItem));
            _eventRegistrations.Add("mod_on_heartbeat", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnHeartbeat));
            _eventRegistrations.Add("mod_on_leave", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerLeave));
            _eventRegistrations.Add("mod_on_levelup", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerLevelUp));
            _eventRegistrations.Add("mod_on_respawn", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerRespawn));
            _eventRegistrations.Add("mod_on_rest", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnPlayerRest));
            _eventRegistrations.Add("mod_on_unacquire", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnUnacquireItem));
            _eventRegistrations.Add("mod_on_unequip", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnUnequipItem));
            _eventRegistrations.Add("mod_on_user", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ModulePrefix.OnUserDefined));

            // Placeable events
            _eventRegistrations.Add("plc_on_attack", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnAttacked));
            _eventRegistrations.Add("plc_on_click", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnClicked));
            _eventRegistrations.Add("plc_on_close", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnClosed));
            _eventRegistrations.Add("plc_on_convo", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnConversation));
            _eventRegistrations.Add("plc_on_damage", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnDamaged));
            _eventRegistrations.Add("plc_on_death", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnDeath));
            _eventRegistrations.Add("plc_on_disarm", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnDisarmed));
            _eventRegistrations.Add("plc_on_disturb", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnDisturbed));
            _eventRegistrations.Add("plc_on_hb", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnHeartbeat));
            _eventRegistrations.Add("plc_on_lock", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnLocked));
            _eventRegistrations.Add("plc_on_open", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnOpened));
            _eventRegistrations.Add("plc_on_splcast", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnSpellCastAt));
            _eventRegistrations.Add("plc_on_trap", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnTriggerTrap));
            _eventRegistrations.Add("plc_on_unlock", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnUnlocked));
            _eventRegistrations.Add("plc_on_used", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnUsed));
            _eventRegistrations.Add("plc_on_userdef", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, PlaceablePrefix.OnUserDefined));

            // Server events
            _eventRegistrations.Add("server_on_connec", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ServerEventPrefix.OnConnect, _.GetModule()));
            _eventRegistrations.Add("server_on_discon", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, ServerEventPrefix.OnDisconnect, _.GetModule()));

            // Store events
            _eventRegistrations.Add("store_on_close", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, StorePrefix.OnClose));
            _eventRegistrations.Add("store_on_open", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, StorePrefix.OnOpen));

            // Trigger events
            _eventRegistrations.Add("trig_on_click", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, TriggerPrefix.OnClicked));
            _eventRegistrations.Add("trig_on_disarm", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, TriggerPrefix.OnDisarmed));
            _eventRegistrations.Add("trig_on_enter", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, TriggerPrefix.OnEnter));
            _eventRegistrations.Add("trig_on_exit", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, TriggerPrefix.OnExit));
            _eventRegistrations.Add("trig_on_hb", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, TriggerPrefix.OnHeartbeat));
            _eventRegistrations.Add("trig_on_trap", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, TriggerPrefix.OnTriggerTrap));
            _eventRegistrations.Add("trig_on_userdef", () => Script.RunScriptEvents(NWGameObject.OBJECT_SELF, TriggerPrefix.OnUserDefined));
        }
    }
}