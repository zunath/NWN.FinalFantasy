using System.Collections.Generic;
using NWN.FinalFantasy.Core.Event.Area;
using NWN.FinalFantasy.Core.Message;
using NWN.FinalFantasy.Core.Messaging;
using static NWN._;

namespace NWN.FinalFantasy.Core.Startup
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
            AssignScripts();
        }

        /// <summary>
        /// Whenever a new area is created, add the registered scripts to the area.
        /// </summary>
        private static void SubscribeEvents()
        {
            MessageHub.Instance.Subscribe<AreaCreated>((msg) => AssignScripts(msg.Area));
        }

        /// <summary>
        /// Iterates over all areas in the module and assigns registered scripts to them.
        /// These scripts may come from various sources, such as Startup scripts in other projects.
        /// </summary>
        private static void AssignScripts()
        {
            NWGameObject area = GetFirstArea();

            while(GetIsObjectValid(area))
            {
                AssignScripts(area);

                area = GetNextArea();
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
                var varName = AreaScriptPrefix.OnEnter + GetOpenID(area, AreaScriptPrefix.OnEnter);
                SetLocalString(area, varName, onEnter);
            }

            foreach (var onExit in _onExitScripts)
            {
                var varName = AreaScriptPrefix.OnExit + GetOpenID(area, AreaScriptPrefix.OnExit);
                SetLocalString(area, varName, onExit);
            }

            foreach (var onHeartbeat in _onHeartbeatScripts)
            {
                var varName = AreaScriptPrefix.OnHeartbeat + GetOpenID(area, AreaScriptPrefix.OnHeartbeat);
                SetLocalString(area, varName, onHeartbeat);
            }

            foreach (var onUserDefined in _onUserDefinedScripts)
            {
                var varName = AreaScriptPrefix.OnUserDefined + GetOpenID(area, AreaScriptPrefix.OnUserDefined);
                SetLocalString(area, varName, onUserDefined);
            }

        }

        /// <summary>
        /// Finds an open ID number for a given script prefix.
        /// </summary>
        /// <param name="area">The area to check variables on</param>
        /// <param name="prefix">The prefix to look for</param>
        /// <returns>An open ID number</returns>
        private static int GetOpenID(NWGameObject area, string prefix)
        {
            int id = 1;
            
            while(!string.IsNullOrWhiteSpace(GetLocalString(area, prefix + id)))
            {
                id++;
            }

            return id;
        }

        /// <summary>
        /// Registers a new OnEnter script to all areas in the module.
        /// </summary>
        /// <param name="script">The name of the C# script, excluding the base namespace.</param>
        public static void RegisterOnEnterScript(string script)
        {
            _onEnterScripts.Add(script);
        }

        /// <summary>
        /// Registers a new OnExit script to all areas in the module.
        /// </summary>
        /// <param name="script">The name of the C# script, excluding the base namespace.</param>
        public static void RegisterOnExitScript(string script)
        {
            _onExitScripts.Add(script);
        }

        /// <summary>
        /// Registers a new OnHeartbeat script to all areas in the module.
        /// </summary>
        /// <param name="script">The name of the C# script, excluding the base namespace.</param>
        public static void RegisterOnHeartbeatScript(string script)
        {
            _onHeartbeatScripts.Add(script);
        }

        /// <summary>
        /// Registers a new OnUserDefined script to all areas in the module.
        /// </summary>
        /// <param name="script">The name of the C# script, excluding the base namespace.</param>
        public static void RegisterOnUserDefinedScript(string script)
        {
            _onUserDefinedScripts.Add(script);
        }

    }
}
