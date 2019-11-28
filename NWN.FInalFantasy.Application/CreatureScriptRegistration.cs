using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Chat.Command;
using NWN.FinalFantasy.Core.Event.Prefix;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Core.Utility;
using static NWN._;

namespace NWN.FinalFantasy.Application
{
    public static class CreatureScriptRegistration
    {
        private static readonly List<string> _onBlockedScripts = new List<string>();
        private static readonly List<string> _onCombatRoundEndScripts = new List<string>();
        private static readonly List<string> _onConversationScripts = new List<string>();
        private static readonly List<string> _onDamagedScripts = new List<string>();
        private static readonly List<string> _onDeathScripts = new List<string>();
        private static readonly List<string> _onDisturbedScripts = new List<string>();
        private static readonly List<string> _onHeartbeatScripts = new List<string>();
        private static readonly List<string> _onPerceptionScripts = new List<string>();
        private static readonly List<string> _onPhysicalAttackedScripts = new List<string>();
        private static readonly List<string> _onRestedScripts = new List<string>();
        private static readonly List<string> _onSpawnScripts = new List<string>();
        private static readonly List<string> _onSpellCastAtScripts = new List<string>();
        private static readonly List<string> _onUserDefinedScripts = new List<string>();

        internal static void Register()
        {
            SubscribeEvents();
            LoadCreatureScripts();
        }

        private static void SubscribeEvents()
        {
            MessageHub.Instance.Subscribe<ObjectCreated>((msg) => AssignScripts(msg.GameObject));
        }

        private static void LoadCreatureScripts()
        {
            var module = GetModule();
            _onBlockedScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnBlocked}"));
            _onCombatRoundEndScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnCombatRoundEnd}"));
            _onConversationScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnConversation}"));
            _onDamagedScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnDamaged}"));
            _onDeathScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnDeath}"));
            _onDisturbedScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnDisturbed}"));
            _onHeartbeatScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnHeartbeat}"));
            _onPerceptionScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnPerception}"));
            _onPhysicalAttackedScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnAttacked}"));
            _onRestedScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnRested}"));
            _onSpawnScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnSpawn}"));
            _onSpellCastAtScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnSpellCastAt}"));
            _onUserDefinedScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"CREATURE_{CreaturePrefix.OnUserDefined}"));
        }

        private static void AssignScripts(NWGameObject obj)
        {
            if (GetObjectType(obj) != ObjectType.Creature) return;

            foreach (var onBlocked in _onBlockedScripts)
            {
                var varName = CreaturePrefix.OnBlocked + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnBlocked);
                SetLocalString(obj, varName, onBlocked);
            }
            foreach (var onCombatRoundEnd in _onCombatRoundEndScripts)
            {
                var varName = CreaturePrefix.OnCombatRoundEnd + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnCombatRoundEnd);
                SetLocalString(obj, varName, onCombatRoundEnd);
            }
            foreach (var onConversation in _onConversationScripts)
            {
                var varName = CreaturePrefix.OnConversation + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnConversation);
                SetLocalString(obj, varName, onConversation);
            }
            foreach (var onDamaged in _onDamagedScripts)
            {
                var varName = CreaturePrefix.OnDamaged + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnDamaged);
                SetLocalString(obj, varName, onDamaged);
            }
            foreach (var onDeath in _onDeathScripts)
            {
                var varName = CreaturePrefix.OnDeath + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnDeath);
                SetLocalString(obj, varName, onDeath);
            }
            foreach (var onDisturbed in _onDisturbedScripts)
            {
                var varName = CreaturePrefix.OnDisturbed + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnDisturbed);
                SetLocalString(obj, varName, onDisturbed);
            }
            foreach (var onHeartbeat in _onHeartbeatScripts)
            {
                var varName = CreaturePrefix.OnHeartbeat + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnHeartbeat);
                SetLocalString(obj, varName, onHeartbeat);
            }
            foreach (var onPerception in _onPerceptionScripts)
            {
                var varName = CreaturePrefix.OnPerception + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnPerception);
                SetLocalString(obj, varName, onPerception);
            }
            foreach (var onPhysicalAttacked in _onPhysicalAttackedScripts)
            {
                var varName = CreaturePrefix.OnAttacked + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnAttacked);
                SetLocalString(obj, varName, onPhysicalAttacked);
            }
            foreach (var onRested in _onRestedScripts)
            {
                var varName = CreaturePrefix.OnRested + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnRested);
                SetLocalString(obj, varName, onRested);
            }
            foreach (var onSpawn in _onSpawnScripts)
            {
                var varName = CreaturePrefix.OnSpawn + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnSpawn);
                SetLocalString(obj, varName, onSpawn);
            }
            foreach (var onSpellCastAt in _onSpellCastAtScripts)
            {
                var varName = CreaturePrefix.OnSpellCastAt + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnSpellCastAt);
                SetLocalString(obj, varName, onSpellCastAt);
            }
            foreach (var onUserDefined in _onUserDefinedScripts)
            {
                var varName = CreaturePrefix.OnUserDefined + LocalVariableTool.GetOpenScriptID(obj, CreaturePrefix.OnUserDefined);
                SetLocalString(obj, varName, onUserDefined);
            }
        }
    }
}
