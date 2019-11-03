using System;
using System.Linq;
using NWN.FinalFantasy.Core.Event.AreaOfEffect;
using NWN.FinalFantasy.Core.Event.Door;
using NWN.FinalFantasy.Core.Event.Encounter;
using NWN.FinalFantasy.Core.Event.Placeable;
using NWN.FinalFantasy.Core.Event.Store;
using NWN.FinalFantasy.Core.Event.Trigger;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWNX;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Core.Startup
{
    /// <summary>
    /// Responsible for registering NWN script events to objects on initialization as well as any time a new object is created.
    /// </summary>
    internal static class EventRegistration
    {
        internal static void Register()
        {
            SubscribeEvents();

            RegisterModuleEvents();
            RegisterNWNXEvents();
            RegisterAreaEvents();
            RegisterObjectEvents();
            RegisterDMEvents();
        }

        /// <summary>
        /// Subscribes to events published by the message hub.
        /// </summary>
        private static void SubscribeEvents()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
            {
                MessageHub.Instance.Publish(new ServerStopped());
            };

            MessageHub.Instance.Subscribe<ObjectCreated>(@event => RegisterObjectEvent(@event.GameObject));
            MessageHub.Instance.Subscribe<AreaCreated>(@event => RegisterAreaEvent(@event.Area));
        }

        /// <summary>
        /// Registers all of the module event scripts
        /// </summary>
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

        /// <summary>
        /// Registers all of the NWNX event scripts.
        /// </summary>
        private static void RegisterNWNXEvents()
        {
            NWNXEvents.SubscribeEvent(NWNXEventType.StartCombatRoundBefore, "mod_on_attack");
            NWNXEvents.SubscribeEvent(NWNXEventType.StartCombatRoundBefore, "mod_on_examine");
            NWNXEvents.SubscribeEvent(NWNXEventType.StartCombatRoundBefore, "mod_on_usefeat");
            NWNXEvents.SubscribeEvent(NWNXEventType.StartCombatRoundBefore, "mod_on_useitem");

            NWNXChat.RegisterChatScript("mod_on_nwnxchat");
        }

        /// <summary>
        /// Registers all of the area event scripts
        /// </summary>
        private static void RegisterAreaEvents()
        {
            var area = GetFirstArea();
            while (GetIsObjectValid(area))
            {
                RegisterAreaEvent(area);
                area = GetNextArea();
            }
        }

        /// <summary>
        /// Registers the events of all objects in the module which have been created at boot time
        /// </summary>
        private static void RegisterObjectEvents()
        {
            var area = GetFirstArea();
            while (GetIsObjectValid(area))
            {
                var obj = GetFirstObjectInArea(area);
                while (GetIsObjectValid(obj))
                {
                    RegisterObjectEvent(obj);
                    obj = GetNextObjectInArea(area);
                }

                area = GetNextArea();
            }
        }

        /// <summary>
        /// Registers the events of a specific object.
        /// </summary>
        /// <param name="obj">The objects whose scripts are being adjusted</param>
        private static void RegisterObjectEvent(NWGameObject obj)
        {
            var type = GetObjectType(obj);

            switch (type)
            {
                case ObjectType.Creature:
                    RegisterCreatureEvents(obj);
                    break;
                case ObjectType.Trigger:
                    RegisterTriggerEvents(obj);
                    break;
                case ObjectType.Door:
                    RegisterDoorEvents(obj);
                    break;
                case ObjectType.AreaOfEffect:
                    RegisterAreaOfEffectEvents(obj);
                    break;
                case ObjectType.Placeable:
                    RegisterPlaceableEvents(obj);
                    break;
                case ObjectType.Store:
                    RegisterStoreEvents(obj);
                    break;
                case ObjectType.Encounter:
                    RegisterEncounterEvents(obj);
                    break;
            }
        }

        /// <summary>
        /// Registers the events of an area.
        /// </summary>
        /// <param name="area"></param>
        private static void RegisterAreaEvent(NWGameObject area)
        {
            SetEventScript(area, EventScriptArea.OnEnter, "area_on_enter");
            SetEventScript(area, EventScriptArea.OnExit, "area_on_exit");
            SetEventScript(area, EventScriptArea.OnHeartbeat, "area_on_hb");
            SetEventScript(area, EventScriptArea.OnUserDefinedEvent, "area_on_user");
        }

        /// <summary>
        /// Registers events for creatures.
        /// </summary>
        private static void RegisterCreatureEvents(NWGameObject creature)
        {
            SetEventScript(creature, EventScriptCreature.OnMeleeAttacked, "crea_on_attacked");
            SetEventScript(creature, EventScriptCreature.OnBlockedByDoor, "crea_on_blocked");
            SetEventScript(creature, EventScriptCreature.OnDialogue, "crea_on_convo");
            SetEventScript(creature, EventScriptCreature.OnDamaged, "crea_on_damaged");
            SetEventScript(creature, EventScriptCreature.OnDeath, "crea_on_death");
            SetEventScript(creature, EventScriptCreature.OnDisturbed, "crea_on_disturb");
            SetEventScript(creature, EventScriptCreature.OnHeartbeat, "crea_on_hb");
            SetEventScript(creature, EventScriptCreature.OnNotice, "crea_on_percept");
            SetEventScript(creature, EventScriptCreature.OnRested, "crea_on_rested");
            SetEventScript(creature, EventScriptCreature.OnEndCombatRound, "crea_on_roundend");
            SetEventScript(creature, EventScriptCreature.OnSpawnIn, "crea_on_spawn");
            SetEventScript(creature, EventScriptCreature.OnSpellCastAt, "crea_on_splcast");
            SetEventScript(creature, EventScriptCreature.OnUserDefinedEvent, "crea_on_userdef");
        }

        /// <summary>
        /// Registers events for triggers
        /// </summary>
        private static void RegisterTriggerEvents(NWGameObject trigger)
        {
            SetEventScript(trigger, EventScriptTrigger.OnHeartbeat, string.Empty);
            SetEventScript(trigger, EventScriptTrigger.OnClicked, string.Empty);
            SetEventScript(trigger, EventScriptTrigger.OnDisarmed, string.Empty);
            SetEventScript(trigger, EventScriptTrigger.OnObjectEnter, string.Empty);
            SetEventScript(trigger, EventScriptTrigger.OnObjectExit, string.Empty);
            SetEventScript(trigger, EventScriptTrigger.OnTrapTriggered, string.Empty);
            SetEventScript(trigger, EventScriptTrigger.OnUserDefinedEvent, string.Empty);

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnHeartbeat).Any())
                SetEventScript(trigger, EventScriptTrigger.OnHeartbeat, "trig_on_hb");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnClicked).Any())
                SetEventScript(trigger, EventScriptTrigger.OnClicked, "trig_on_click");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnDisarmed).Any())
                SetEventScript(trigger, EventScriptTrigger.OnDisarmed, "trig_on_disarm");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnEnter).Any())
                SetEventScript(trigger, EventScriptTrigger.OnObjectEnter, "trig_on_enter");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnExit).Any())
                SetEventScript(trigger, EventScriptTrigger.OnObjectExit, "trig_on_exit");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnTriggerTrap).Any())
                SetEventScript(trigger, EventScriptTrigger.OnTrapTriggered, "trig_on_trap");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnUserDefined).Any())
                SetEventScript(trigger, EventScriptTrigger.OnUserDefinedEvent, "trig_on_userdef");
        }

        /// <summary>
        /// Registers events for doors
        /// </summary>
        private static void RegisterDoorEvents(NWGameObject door)
        {
            SetEventScript(door, EventScriptDoor.OnMeleeAttacked, string.Empty);
            SetEventScript(door, EventScriptDoor.OnClicked, string.Empty);
            SetEventScript(door, EventScriptDoor.OnClose, string.Empty);
            SetEventScript(door, EventScriptDoor.OnDialogue, string.Empty);
            SetEventScript(door, EventScriptDoor.OnDamage, string.Empty);
            SetEventScript(door, EventScriptDoor.OnDeath, string.Empty);
            SetEventScript(door, EventScriptDoor.OnDisarm, string.Empty);
            SetEventScript(door, EventScriptDoor.OnFailToOpen, string.Empty);
            SetEventScript(door, EventScriptDoor.OnHeartbeat, string.Empty);
            SetEventScript(door, EventScriptDoor.OnLock, string.Empty);
            SetEventScript(door, EventScriptDoor.OnOpen, string.Empty);
            SetEventScript(door, EventScriptDoor.OnSpellCastAt, string.Empty);
            SetEventScript(door, EventScriptDoor.OnTrapTriggered, string.Empty);
            SetEventScript(door, EventScriptDoor.OnUnlock, string.Empty);
            SetEventScript(door, EventScriptDoor.OnUserDefined, string.Empty);

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnAttacked).Any())
                SetEventScript(door, EventScriptDoor.OnMeleeAttacked, "door_on_attacked");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnClicked).Any())
                SetEventScript(door, EventScriptDoor.OnClicked, "door_on_click");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnClosed).Any())
                SetEventScript(door, EventScriptDoor.OnClose, "door_on_close");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnConversation).Any())
                SetEventScript(door, EventScriptDoor.OnDialogue, "door_on_convo");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnDamaged).Any())
                SetEventScript(door, EventScriptDoor.OnDamage, "door_on_damage");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnDeath).Any())
                SetEventScript(door, EventScriptDoor.OnDeath, "door_on_death");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnDisarmed).Any())
                SetEventScript(door, EventScriptDoor.OnDisarm, "door_on_disarm");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnFailToOpen).Any())
                SetEventScript(door, EventScriptDoor.OnFailToOpen, "door_on_fail");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnHeartbeat).Any())
                SetEventScript(door, EventScriptDoor.OnHeartbeat, "door_on_hb");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnLocked).Any())
                SetEventScript(door, EventScriptDoor.OnLock, "door_on_lock");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnOpened).Any())
                SetEventScript(door, EventScriptDoor.OnOpen, "door_on_open");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnSpellCastAt).Any())
                SetEventScript(door, EventScriptDoor.OnSpellCastAt, "door_on_splcast");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnTriggerTrap).Any())
                SetEventScript(door, EventScriptDoor.OnTrapTriggered, "door_on_trap");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnUnlock).Any())
                SetEventScript(door, EventScriptDoor.OnUnlock, "door_on_unlock");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnUserDefined).Any())
                SetEventScript(door, EventScriptDoor.OnUserDefined, "door_on_userdef");
        }

        /// <summary>
        /// Registers events for areas of effect
        /// </summary>
        private static void RegisterAreaOfEffectEvents(NWGameObject aoe)
        {
            SetEventScript(aoe, EventScriptAreaOfEffect.OnHeartbeat, string.Empty);
            SetEventScript(aoe, EventScriptAreaOfEffect.OnUserDefinedEvent, string.Empty);
            SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectEnter, string.Empty);
            SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectExit, string.Empty);

            if (ScriptRunner.GetMatchingVariables(aoe, AreaOfEffectPrefix.OnHeartbeat).Any())
                SetEventScript(aoe, EventScriptAreaOfEffect.OnHeartbeat, "aoe_on_hb");

            if (ScriptRunner.GetMatchingVariables(aoe, AreaOfEffectPrefix.OnUserDefined).Any())
                SetEventScript(aoe, EventScriptAreaOfEffect.OnUserDefinedEvent, "aoe_on_userdef");

            if (ScriptRunner.GetMatchingVariables(aoe, AreaOfEffectPrefix.OnEnter).Any())
                SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectEnter, "aoe_on_enter");

            if (ScriptRunner.GetMatchingVariables(aoe, AreaOfEffectPrefix.OnExit).Any())
                SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectExit, "aoe_on_exit");
        }

        /// <summary>
        /// Registers events for placeables
        /// </summary>
        private static void RegisterPlaceableEvents(NWGameObject placeable)
        {
            SetEventScript(placeable, EventScriptPlaceable.OnMeleeAttacked, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnLeftClick, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnClosed, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnDialogue, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnDamaged, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnDeath, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnDisarm, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnInventoryDisturbed, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnHeartbeat, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnLock, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnOpen, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnSpellCastAt, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnTrapTriggered, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnUnlock, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnUsed, string.Empty);
            SetEventScript(placeable, EventScriptPlaceable.OnUserDefinedEvent, string.Empty);

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnAttacked).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnMeleeAttacked, "plc_on_attack");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnClicked).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnLeftClick, "plc_on_click");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnClosed).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnClosed, "plc_on_close");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnConversation).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnDialogue, "plc_on_convo");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnDamaged).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnDamaged, "plc_on_damage");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnDeath).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnDeath, "plc_on_death");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnDisarmed).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnDisarm, "plc_on_disarm");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnDisturbed).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnInventoryDisturbed, "plc_on_disturb");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnHeartbeat).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnHeartbeat, "plc_on_hb");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnLocked).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnLock, "plc_on_lock");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnOpened).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnOpen, "plc_on_open");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnSpellCastAt).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnSpellCastAt, "plc_on_splcast");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnTriggerTrap).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnTrapTriggered, "plc_on_trap");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnUnlocked).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnUnlock, "plc_on_unlock");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnUsed).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnUsed, "plc_on_used");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnUserDefined).Any())
                SetEventScript(placeable, EventScriptPlaceable.OnUserDefinedEvent, "plc_on_userdef");
        }

        /// <summary>
        /// Registers events for stores
        /// </summary>
        private static void RegisterStoreEvents(NWGameObject store)
        {
            SetEventScript(store, EventScriptStore.OnOpen, string.Empty);
            SetEventScript(store, EventScriptStore.OnClose, string.Empty);

            if (ScriptRunner.GetMatchingVariables(store, StorePrefix.OnOpen).Any())
                SetEventScript(store, EventScriptStore.OnOpen, "store_on_open");

            if (ScriptRunner.GetMatchingVariables(store, StorePrefix.OnClose).Any())
                SetEventScript(store, EventScriptStore.OnClose, "store_on_close");
        }

        /// <summary>
        /// Registers events for encounters
        /// </summary>
        private static void RegisterEncounterEvents(NWGameObject encounter)
        {
            SetEventScript(encounter, EventScriptEncounter.OnObjectEnter, string.Empty);
            SetEventScript(encounter, EventScriptEncounter.OnObjectExit, string.Empty);
            SetEventScript(encounter, EventScriptEncounter.OnHeartbeat, string.Empty);
            SetEventScript(encounter, EventScriptEncounter.OnEncounterExhausted, string.Empty);
            SetEventScript(encounter, EventScriptEncounter.OnUserDefinedEvent, string.Empty);

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnEnter).Any())
                SetEventScript(encounter, EventScriptEncounter.OnObjectEnter, "enc_on_enter");

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnExit).Any())
                SetEventScript(encounter, EventScriptEncounter.OnObjectExit, "enc_on_exit");

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnHeartbeat).Any())
                SetEventScript(encounter, EventScriptEncounter.OnHeartbeat, "enc_on_hb");

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnExhausted).Any())
                SetEventScript(encounter, EventScriptEncounter.OnEncounterExhausted, "enc_on_exhaust");

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnUserDefined).Any())
                SetEventScript(encounter, EventScriptEncounter.OnUserDefinedEvent, "enc_on_userdef");
        }

        private static void RegisterDMEvents()
        {

            NWNXEvents.SubscribeEvent(NWNXEventType.DMAppearBefore, "dm_appear");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMChangeDifficultyBefore, "dm_change_diff");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMDisableTrapBefore, "dm_disab_trap");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMDisappearBefore, "dm_disappear");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMForceRestBefore, "dm_force_rest");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMGetVariableBefore, "dm_get_var");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMGiveGoldBefore, "dm_give_gold");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMGiveItemAfter, "dm_give_item");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMGiveLevelBefore, "dm_give_level");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMGiveXPBefore, "dm_give_xp");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMHealBefore, "dm_heal");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMJumpBefore, "dm_jump");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMJumpAllPlayersToPointBefore, "dm_jump_all");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMJumpTargetToPointBefore, "dm_jump_target");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMKillBefore, "dm_kill");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMLimboBefore, "dm_limbo");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMPossessBefore, "dm_possess");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMSetDateBefore, "dm_set_date");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMSetStatBefore, "dm_set_stat");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMSetTimeBefore, "dm_set_time");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMSetVariableBefore, "dm_set_var");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMSpawnObjectAfter, "dm_spawn");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMTakeItemBefore, "dm_take_item");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMToggleImmortalBefore, "dm_togg_immo");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMToggleInvulnerabilityBefore, "dm_togg_invuln");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMToggleAIBefore, "dm_toggle_ai");
            NWNXEvents.SubscribeEvent(NWNXEventType.DMToggleLockBefore, "dm_toggle_lock");
        }
    }
}
