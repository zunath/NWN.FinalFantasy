using NWN.FinalFantasy.Core.NWScript.Enumerations;
using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.AbilityDefinition
{
    internal interface IAbilityDefinition
    {
        string Name { get; }
        Feat Feat { get; }
        AbilityCategory Category { get; }
        AbilityGroup Group { get; }
        bool IsEquippable { get; }
        int APRequired { get; }
        JobLevel[] JobRequirements { get; }

        int MP(NWGameObject user);
        string CanUse(NWGameObject user, NWGameObject target);
        float CastingTime(NWGameObject user);
        float CooldownTime(NWGameObject user);
        void Impact(NWGameObject user, NWGameObject target);
        

    }
}
