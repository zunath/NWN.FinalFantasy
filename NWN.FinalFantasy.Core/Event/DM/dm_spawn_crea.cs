using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.DM;


// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    internal class dm_spawn_crea
#pragma warning restore IDE1006 // Naming Styles
    {
        // ReSharper disable once UnusedMember.Local
        private static void Main()
        {
            ScriptRunner.RunScriptEvents(_.GetModule(), DMScriptPrefix.OnSpawnCreature);
        }
    }
}