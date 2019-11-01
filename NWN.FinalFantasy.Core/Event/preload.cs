using System;
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
                        case ObjectType.Item:
                            RegisterItemEvents(obj);
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
                        case ObjectType.Waypoint:
                            RegisterWaypointEvents(obj);
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
        /// Registers events for items
        /// </summary>
        private static void RegisterItemEvents(NWGameObject item)
        {
        }

        /// <summary>
        /// Registers events for triggers
        /// </summary>
        private static void RegisterTriggerEvents(NWGameObject trigger)
        {

        }

        /// <summary>
        /// Registers events for doors
        /// </summary>
        private static void RegisterDoorEvents(NWGameObject door)
        {

        }

        /// <summary>
        /// Registers events for areas of effect
        /// </summary>
        private static void RegisterAreaOfEffectEvents(NWGameObject aoe)
        {

        }

        /// <summary>
        /// Registers events for waypoints
        /// </summary>
        private static void RegisterWaypointEvents(NWGameObject waypoint)
        {

        }

        /// <summary>
        /// Registers events for placeables
        /// </summary>
        private static void RegisterPlaceableEvents(NWGameObject placeable)
        {

        }

        /// <summary>
        /// Registers events for stores
        /// </summary>
        private static void RegisterStoreEvents(NWGameObject store)
        {

        }

        /// <summary>
        /// Registers events for encounters
        /// </summary>
        private static void RegisterEncounterEvents(NWGameObject encounter)
        {

        }
    }
}
