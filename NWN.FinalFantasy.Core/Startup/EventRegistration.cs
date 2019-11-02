using System.Linq;
using NWN.FinalFantasy.Core.Event.AreaOfEffect;
using NWN.FinalFantasy.Core.Event.Door;
using NWN.FinalFantasy.Core.Event.Encounter;
using NWN.FinalFantasy.Core.Event.Placeable;
using NWN.FinalFantasy.Core.Event.Store;
using NWN.FinalFantasy.Core.Event.Trigger;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWScript.Enumerations;

namespace NWN.FinalFantasy.Core.Startup
{
    internal static class EventRegistration
    {
        internal static void Register()
        {
            RegisterModuleEvents();
            RegisterAreaEvents();
            RegisterObjectEvents();

            SubscribeEvents();
        }

        /// <summary>
        /// Subscribes to events published by the message hub.
        /// </summary>
        private static void SubscribeEvents()
        {
            MessageHub.Instance.Subscribe<ObjectCreated>(@event => RegisterObjectEvent(@event.GameObject));
            MessageHub.Instance.Subscribe<AreaCreated>(@event => RegisterAreaEvent(@event.Area));
        }

        /// <summary>
        /// Registers all of the module event scripts
        /// </summary>
        private static void RegisterModuleEvents()
        {
            _.SetEventScript(_.GetModule(), EventScriptModule.OnAcquireItem, "mod_on_acquire");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnActivateItem, "mod_on_activate");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnClientEnter, "mod_on_enter");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnClientExit, "mod_on_leave");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnPlayerCancelCutscene, "mod_on_csabort");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnHeartbeat, "mod_on_heartbeat");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnPlayerChat, "mod_on_chat");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnPlayerDeath, "mod_on_death");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnPlayerDying, "mod_on_dying");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnEquipItem, "mod_on_equip");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnPlayerLevelUp, "mod_on_levelup");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnRespawnButtonPressed, "mod_on_respawn");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnPlayerRest, "mod_on_rest");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnUnequipItem, "mod_on_unequip");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnLoseItem, "mod_on_unacquire");
            _.SetEventScript(_.GetModule(), EventScriptModule.OnUserDefinedEvent, "mod_on_user");
        }

        /// <summary>
        /// Registers all of the area event scripts
        /// </summary>
        private static void RegisterAreaEvents()
        {
            var area = _.GetFirstArea();
            while (_.GetIsObjectValid(area))
            {
                RegisterAreaEvent(area);
                area = _.GetNextArea();
            }
        }

        /// <summary>
        /// Registers the events of all objects in the module which have been created at boot time
        /// </summary>
        private static void RegisterObjectEvents()
        {
            var area = _.GetFirstArea();
            while (_.GetIsObjectValid(area))
            {
                var obj = _.GetFirstObjectInArea(area);
                while (_.GetIsObjectValid(obj))
                {
                    RegisterObjectEvent(obj);
                    obj = _.GetNextObjectInArea(area);
                }

                area = _.GetNextArea();
            }
        }

        /// <summary>
        /// Registers the events of a specific object.
        /// </summary>
        /// <param name="obj">The objects whose scripts are being adjusted</param>
        private static void RegisterObjectEvent(NWGameObject obj)
        {
            var type = _.GetObjectType(obj);

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
            _.SetEventScript(area, EventScriptArea.OnEnter, "area_on_enter");
            _.SetEventScript(area, EventScriptArea.OnExit, "area_on_exit");
            _.SetEventScript(area, EventScriptArea.OnHeartbeat, "area_on_hb");
            _.SetEventScript(area, EventScriptArea.OnUserDefinedEvent, "area_on_user");
        }

        /// <summary>
        /// Registers events for creatures.
        /// </summary>
        private static void RegisterCreatureEvents(NWGameObject creature)
        {
            _.SetEventScript(creature, EventScriptCreature.OnMeleeAttacked, "crea_on_attacked");
            _.SetEventScript(creature, EventScriptCreature.OnBlockedByDoor, "crea_on_blocked");
            _.SetEventScript(creature, EventScriptCreature.OnDialogue, "crea_on_convo");
            _.SetEventScript(creature, EventScriptCreature.OnDamaged, "crea_on_damaged");
            _.SetEventScript(creature, EventScriptCreature.OnDeath, "crea_on_death");
            _.SetEventScript(creature, EventScriptCreature.OnDisturbed, "crea_on_disturb");
            _.SetEventScript(creature, EventScriptCreature.OnHeartbeat, "crea_on_hb");
            _.SetEventScript(creature, EventScriptCreature.OnNotice, "crea_on_percept");
            _.SetEventScript(creature, EventScriptCreature.OnRested, "crea_on_rested");
            _.SetEventScript(creature, EventScriptCreature.OnEndCombatRound, "crea_on_roundend");
            _.SetEventScript(creature, EventScriptCreature.OnSpawnIn, "crea_on_spawn");
            _.SetEventScript(creature, EventScriptCreature.OnSpellCastAt, "crea_on_splcast");
            _.SetEventScript(creature, EventScriptCreature.OnUserDefinedEvent, "crea_on_userdef");
        }

        /// <summary>
        /// Registers events for triggers
        /// </summary>
        private static void RegisterTriggerEvents(NWGameObject trigger)
        {
            _.SetEventScript(trigger, EventScriptTrigger.OnHeartbeat, string.Empty);
            _.SetEventScript(trigger, EventScriptTrigger.OnClicked, string.Empty);
            _.SetEventScript(trigger, EventScriptTrigger.OnDisarmed, string.Empty);
            _.SetEventScript(trigger, EventScriptTrigger.OnObjectEnter, string.Empty);
            _.SetEventScript(trigger, EventScriptTrigger.OnObjectExit, string.Empty);
            _.SetEventScript(trigger, EventScriptTrigger.OnTrapTriggered, string.Empty);
            _.SetEventScript(trigger, EventScriptTrigger.OnUserDefinedEvent, string.Empty);

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnHeartbeat).Any())
                _.SetEventScript(trigger, EventScriptTrigger.OnHeartbeat, "trig_on_hb");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnClicked).Any())
                _.SetEventScript(trigger, EventScriptTrigger.OnClicked, "trig_on_click");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnDisarmed).Any())
                _.SetEventScript(trigger, EventScriptTrigger.OnDisarmed, "trig_on_disarm");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnEnter).Any())
                _.SetEventScript(trigger, EventScriptTrigger.OnObjectEnter, "trig_on_enter");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnExit).Any())
                _.SetEventScript(trigger, EventScriptTrigger.OnObjectExit, "trig_on_exit");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnTriggerTrap).Any())
                _.SetEventScript(trigger, EventScriptTrigger.OnTrapTriggered, "trig_on_trap");

            if (ScriptRunner.GetMatchingVariables(trigger, TriggerPrefix.OnUserDefined).Any())
                _.SetEventScript(trigger, EventScriptTrigger.OnUserDefinedEvent, "trig_on_userdef");
        }

        /// <summary>
        /// Registers events for doors
        /// </summary>
        private static void RegisterDoorEvents(NWGameObject door)
        {
            _.SetEventScript(door, EventScriptDoor.OnMeleeAttacked, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnClicked, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnClose, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnDialogue, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnDamage, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnDeath, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnDisarm, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnFailToOpen, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnHeartbeat, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnLock, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnOpen, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnSpellCastAt, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnTrapTriggered, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnUnlock, string.Empty);
            _.SetEventScript(door, EventScriptDoor.OnUserDefined, string.Empty);

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnAttacked).Any())
                _.SetEventScript(door, EventScriptDoor.OnMeleeAttacked, "door_on_attacked");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnClicked).Any())
                _.SetEventScript(door, EventScriptDoor.OnClicked, "door_on_click");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnClosed).Any())
                _.SetEventScript(door, EventScriptDoor.OnClose, "door_on_close");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnConversation).Any())
                _.SetEventScript(door, EventScriptDoor.OnDialogue, "door_on_convo");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnDamaged).Any())
                _.SetEventScript(door, EventScriptDoor.OnDamage, "door_on_damage");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnDeath).Any())
                _.SetEventScript(door, EventScriptDoor.OnDeath, "door_on_death");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnDisarmed).Any())
                _.SetEventScript(door, EventScriptDoor.OnDisarm, "door_on_disarm");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnFailToOpen).Any())
                _.SetEventScript(door, EventScriptDoor.OnFailToOpen, "door_on_fail");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnHeartbeat).Any())
                _.SetEventScript(door, EventScriptDoor.OnHeartbeat, "door_on_hb");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnLocked).Any())
                _.SetEventScript(door, EventScriptDoor.OnLock, "door_on_lock");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnOpened).Any())
                _.SetEventScript(door, EventScriptDoor.OnOpen, "door_on_open");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnSpellCastAt).Any())
                _.SetEventScript(door, EventScriptDoor.OnSpellCastAt, "door_on_splcast");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnTriggerTrap).Any())
                _.SetEventScript(door, EventScriptDoor.OnTrapTriggered, "door_on_trap");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnUnlock).Any())
                _.SetEventScript(door, EventScriptDoor.OnUnlock, "door_on_unlock");

            if (ScriptRunner.GetMatchingVariables(door, DoorPrefix.OnUserDefined).Any())
                _.SetEventScript(door, EventScriptDoor.OnUserDefined, "door_on_userdef");
        }

        /// <summary>
        /// Registers events for areas of effect
        /// </summary>
        private static void RegisterAreaOfEffectEvents(NWGameObject aoe)
        {
            _.SetEventScript(aoe, EventScriptAreaOfEffect.OnHeartbeat, string.Empty);
            _.SetEventScript(aoe, EventScriptAreaOfEffect.OnUserDefinedEvent, string.Empty);
            _.SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectEnter, string.Empty);
            _.SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectExit, string.Empty);

            if (ScriptRunner.GetMatchingVariables(aoe, AreaOfEffectPrefix.OnHeartbeat).Any())
                _.SetEventScript(aoe, EventScriptAreaOfEffect.OnHeartbeat, "aoe_on_hb");

            if (ScriptRunner.GetMatchingVariables(aoe, AreaOfEffectPrefix.OnUserDefined).Any())
                _.SetEventScript(aoe, EventScriptAreaOfEffect.OnUserDefinedEvent, "aoe_on_userdef");

            if (ScriptRunner.GetMatchingVariables(aoe, AreaOfEffectPrefix.OnEnter).Any())
                _.SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectEnter, "aoe_on_enter");

            if (ScriptRunner.GetMatchingVariables(aoe, AreaOfEffectPrefix.OnExit).Any())
                _.SetEventScript(aoe, EventScriptAreaOfEffect.OnObjectExit, "aoe_on_exit");
        }

        /// <summary>
        /// Registers events for placeables
        /// </summary>
        private static void RegisterPlaceableEvents(NWGameObject placeable)
        {
            _.SetEventScript(placeable, EventScriptPlaceable.OnMeleeAttacked, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnLeftClick, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnClosed, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnDialogue, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnDamaged, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnDeath, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnDisarm, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnInventoryDisturbed, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnHeartbeat, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnLock, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnOpen, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnSpellCastAt, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnTrapTriggered, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnUnlock, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnUsed, string.Empty);
            _.SetEventScript(placeable, EventScriptPlaceable.OnUserDefinedEvent, string.Empty);

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnAttacked).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnMeleeAttacked, "plc_on_attack");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnClicked).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnLeftClick, "plc_on_click");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnClosed).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnClosed, "plc_on_close");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnConversation).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnDialogue, "plc_on_convo");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnDamaged).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnDamaged, "plc_on_damage");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnDeath).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnDeath, "plc_on_death");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnDisarmed).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnDisarm, "plc_on_disarm");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnDisturbed).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnInventoryDisturbed, "plc_on_disturb");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnHeartbeat).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnHeartbeat, "plc_on_hb");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnLocked).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnLock, "plc_on_lock");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnOpened).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnOpen, "plc_on_open");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnSpellCastAt).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnSpellCastAt, "plc_on_splcast");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnTriggerTrap).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnTrapTriggered, "plc_on_trap");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnUnlocked).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnUnlock, "plc_on_unlock");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnUsed).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnUsed, "plc_on_used");

            if (ScriptRunner.GetMatchingVariables(placeable, PlaceablePrefix.OnUserDefined).Any())
                _.SetEventScript(placeable, EventScriptPlaceable.OnUserDefinedEvent, "plc_on_userdef");
        }

        /// <summary>
        /// Registers events for stores
        /// </summary>
        private static void RegisterStoreEvents(NWGameObject store)
        {
            _.SetEventScript(store, EventScriptStore.OnOpen, string.Empty);
            _.SetEventScript(store, EventScriptStore.OnClose, string.Empty);

            if (ScriptRunner.GetMatchingVariables(store, StorePrefix.OnOpen).Any())
                _.SetEventScript(store, EventScriptStore.OnOpen, "store_on_open");

            if (ScriptRunner.GetMatchingVariables(store, StorePrefix.OnClose).Any())
                _.SetEventScript(store, EventScriptStore.OnClose, "store_on_close");
        }

        /// <summary>
        /// Registers events for encounters
        /// </summary>
        private static void RegisterEncounterEvents(NWGameObject encounter)
        {
            _.SetEventScript(encounter, EventScriptEncounter.OnObjectEnter, string.Empty);
            _.SetEventScript(encounter, EventScriptEncounter.OnObjectExit, string.Empty);
            _.SetEventScript(encounter, EventScriptEncounter.OnHeartbeat, string.Empty);
            _.SetEventScript(encounter, EventScriptEncounter.OnEncounterExhausted, string.Empty);
            _.SetEventScript(encounter, EventScriptEncounter.OnUserDefinedEvent, string.Empty);

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnEnter).Any())
                _.SetEventScript(encounter, EventScriptEncounter.OnObjectEnter, "enc_on_enter");

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnExit).Any())
                _.SetEventScript(encounter, EventScriptEncounter.OnObjectExit, "enc_on_exit");

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnHeartbeat).Any())
                _.SetEventScript(encounter, EventScriptEncounter.OnHeartbeat, "enc_on_hb");

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnExhausted).Any())
                _.SetEventScript(encounter, EventScriptEncounter.OnEncounterExhausted, "enc_on_exhaust");

            if (ScriptRunner.GetMatchingVariables(encounter, EncounterPrefix.OnUserDefined).Any())
                _.SetEventScript(encounter, EventScriptEncounter.OnUserDefinedEvent, "enc_on_userdef");
        }
    }
}
