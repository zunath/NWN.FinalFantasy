using System.Collections.Generic;
using NWN.FinalFantasy.Core.Event.Prefix;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using NWN.FinalFantasy.Core.Utility;

namespace NWN.FinalFantasy.Application
{
    public static class AreaScriptRegistration
    {
        private static readonly List<string> _onEnterScripts = new List<string>();
        private static readonly List<string> _onExitScripts = new List<string>();
        private static readonly List<string> _onHeartbeatScripts = new List<string>();
        private static readonly List<string> _onUserDefinedScripts = new List<string>();

        /// <summary>
        /// Registers this registry and subscribes to associated events.
        /// This should only be called one time within the preload script.
        /// </summary>
        internal static void Register()
        {
            SubscribeEvents();
            LoadAreaScripts();
            AssignScripts();
        }

        /// <summary>
        /// Whenever a new area is created, add the registered scripts to the area.
        /// </summary>
        private static void SubscribeEvents()
        {
            MessageHub.Instance.Subscribe<AreaCreated>((msg) => AssignScripts(msg.Area));
        }

        private static void LoadAreaScripts()
        {
            NWGameObject module = _.GetModule();
            _onEnterScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"AREA_{AreaPrefix.OnEnter}"));
            _onExitScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"AREA_{AreaPrefix.OnExit}"));
            _onHeartbeatScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"AREA_{AreaPrefix.OnHeartbeat}"));
            _onUserDefinedScripts.AddRange(LocalVariableTool.FindByPrefix(module, $"AREA_{AreaPrefix.OnUserDefined}"));
        }

        /// <summary>
        /// Iterates over all areas in the module and assigns registered scripts to them.
        /// These scripts may come from various sources, such as Startup scripts in other projects.
        /// </summary>
        private static void AssignScripts()
        {
            NWGameObject area = _.GetFirstArea();

            while(_.GetIsObjectValid(area))
            {
                AssignScripts(area);

                area = _.GetNextArea();
            }
        }

        /// <summary>
        /// Assigns registered scripts to a specific area.
        /// These scripts may come from various sources, such as Startup scripts in other projects.
        /// </summary>
        /// <param name="area">The area to assign scripts to.</param>
        internal static void AssignScripts(NWGameObject area)
        {
            foreach (var onEnter in _onEnterScripts)
            {
                var varName = AreaPrefix.OnEnter + LocalVariableTool.GetOpenScriptID(area, AreaPrefix.OnEnter);
                _.SetLocalString(area, varName, onEnter);
            }

            foreach (var onExit in _onExitScripts)
            {
                var varName = AreaPrefix.OnExit + LocalVariableTool.GetOpenScriptID(area, AreaPrefix.OnExit);
                _.SetLocalString(area, varName, onExit);
            }

            foreach (var onHeartbeat in _onHeartbeatScripts)
            {
                var varName = AreaPrefix.OnHeartbeat + LocalVariableTool.GetOpenScriptID(area, AreaPrefix.OnHeartbeat);
                _.SetLocalString(area, varName, onHeartbeat);
            }

            foreach (var onUserDefined in _onUserDefinedScripts)
            {
                var varName = AreaPrefix.OnUserDefined + LocalVariableTool.GetOpenScriptID(area, AreaPrefix.OnUserDefined);
                _.SetLocalString(area, varName, onUserDefined);
            }

        }

    }
}
