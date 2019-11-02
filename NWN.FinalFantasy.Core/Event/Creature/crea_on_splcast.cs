using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.Event.Creature;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
    public class crea_on_splcast
    {
        internal static void Main()
        {
            ScriptRunner.RunScriptEvents(NWGameObject.OBJECT_SELF, CreaturePrefix.OnSpellCastAt);
        }
    }
}