using NWN.FinalFantasy.Core.NWScript.Enumerations;
using static NWN._;

namespace NWN.FinalFantasy.Core
{
    internal static class EventRegistration
    {
        internal static void Register()
        {
            RegisterModuleEvents();
            RegisterAreaEvents();
            RegisterObjectEvents();
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
        /// Registers all of the area event scripts
        /// </summary>
        private static void RegisterAreaEvents()
        {
            var area = GetFirstArea();
            while (GetIsObjectValid(area))
            {
                SetEventScript(area, EventScriptArea.OnEnter, "area_on_enter");
                SetEventScript(area, EventScriptArea.OnEnter, "area_on_exit");
                SetEventScript(area, EventScriptArea.OnEnter, "area_on_hb");
                SetEventScript(area, EventScriptArea.OnEnter, "area_on_user");

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

                    obj = GetNextObjectInArea(area);
                }

                area = GetNextArea();
            }
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
            SetEventScript(trigger, EventScriptTrigger.OnHeartbeat, "trig_on_hb");
            SetEventScript(trigger, EventScriptTrigger.OnClicked, "trig_on_click");
            SetEventScript(trigger, EventScriptTrigger.OnDisarmed, "trig_on_disarm");
            SetEventScript(trigger, EventScriptTrigger.OnObjectEnter, "trig_on_enter");
            SetEventScript(trigger, EventScriptTrigger.OnObjectExit, "trig_on_exit");
            SetEventScript(trigger, EventScriptTrigger.OnTrapTriggered, "trig_on_trap");
            SetEventScript(trigger, EventScriptTrigger.OnUserDefinedEvent, "trig_on_userdef");
        }

        /// <summary>
        /// Registers events for doors
        /// </summary>
        private static void RegisterDoorEvents(NWGameObject door)
        {
            SetEventScript(door, EventScriptDoor.OnMeleeAttacked, "door_on_attacked");
            SetEventScript(door, EventScriptDoor.OnClicked, "door_on_click");
            SetEventScript(door, EventScriptDoor.OnClose, "door_on_close");
            SetEventScript(door, EventScriptDoor.OnDialogue, "door_on_convo");
            SetEventScript(door, EventScriptDoor.OnDamage, "door_on_damage");
            SetEventScript(door, EventScriptDoor.OnDeath, "door_on_death");
            SetEventScript(door, EventScriptDoor.OnDisarm, "door_on_disarm");
            SetEventScript(door, EventScriptDoor.OnFailToOpen, "door_on_fail");
            SetEventScript(door, EventScriptDoor.OnHeartbeat, "door_on_hb");
            SetEventScript(door, EventScriptDoor.OnLock, "door_on_lock");
            SetEventScript(door, EventScriptDoor.OnOpen, "door_on_open");
            SetEventScript(door, EventScriptDoor.OnSpellCastAt, "door_on_splcast");
            SetEventScript(door, EventScriptDoor.OnTrapTriggered, "door_on_trap");
            SetEventScript(door, EventScriptDoor.OnUnlock, "door_on_unlock");
            SetEventScript(door, EventScriptDoor.OnUserDefined, "door_on_userdef");
        }

        /// <summary>
        /// Registers events for areas of effect
        /// </summary>
        private static void RegisterAreaOfEffectEvents(NWGameObject aoe)
        {
            SetEventScript(aoe, EventScriptAreaOfEffect.OnHeartbeat, "aoe_on_hb");
            SetEventScript(aoe, EventScriptAreaOfEffect.OnUserDefinedEvent, "aoe_on_userdef");
            SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectEnter, "aoe_on_enter");
            SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectExit, "aoe_on_exit");
        }

        /// <summary>
        /// Registers events for placeables
        /// </summary>
        private static void RegisterPlaceableEvents(NWGameObject placeable)
        {
            SetEventScript(placeable, EventScriptPlaceable.OnMeleeAttacked, "plc_on_attack");
            SetEventScript(placeable, EventScriptPlaceable.OnLeftClick, "plc_on_click");
            SetEventScript(placeable, EventScriptPlaceable.OnClosed, "plc_on_close");
            SetEventScript(placeable, EventScriptPlaceable.OnDialogue, "plc_on_convo");
            SetEventScript(placeable, EventScriptPlaceable.OnDamaged, "plc_on_damage");
            SetEventScript(placeable, EventScriptPlaceable.OnDeath, "plc_on_death");
            SetEventScript(placeable, EventScriptPlaceable.OnDisarm, "plc_on_disarm");
            SetEventScript(placeable, EventScriptPlaceable.OnInventoryDisturbed, "plc_on_disturb");
            SetEventScript(placeable, EventScriptPlaceable.OnHeartbeat, "plc_on_hb");
            SetEventScript(placeable, EventScriptPlaceable.OnLock, "plc_on_lock");
            SetEventScript(placeable, EventScriptPlaceable.OnOpen, "plc_on_open");
            SetEventScript(placeable, EventScriptPlaceable.OnSpellCastAt, "plc_on_splcast");
            SetEventScript(placeable, EventScriptPlaceable.OnTrapTriggered, "plc_on_trap");
            SetEventScript(placeable, EventScriptPlaceable.OnUnlock, "plc_on_unlock");
            SetEventScript(placeable, EventScriptPlaceable.OnUsed, "plc_on_used");
            SetEventScript(placeable, EventScriptPlaceable.OnUserDefinedEvent, "plc_on_userdef");
        }

        /// <summary>
        /// Registers events for stores
        /// </summary>
        private static void RegisterStoreEvents(NWGameObject store)
        {
            SetEventScript(store, EventScriptStore.OnOpen, "store_on_open");
            SetEventScript(store, EventScriptStore.OnClose, "store_on_close");
        }

        /// <summary>
        /// Registers events for encounters
        /// </summary>
        private static void RegisterEncounterEvents(NWGameObject encounter)
        {
            SetEventScript(encounter, EventScriptEncounter.OnObjectEnter, "enc_on_enter");
            SetEventScript(encounter, EventScriptEncounter.OnObjectExit, "enc_on_exit");
            SetEventScript(encounter, EventScriptEncounter.OnHeartbeat, "enc_on_hb");
            SetEventScript(encounter, EventScriptEncounter.OnEncounterExhausted, "enc_on_exhaust");
            SetEventScript(encounter, EventScriptEncounter.OnUserDefinedEvent, "enc_on_userdef");
        }
    }
}
