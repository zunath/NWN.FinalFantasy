using NWN.FinalFantasy.Job.Enumeration;

namespace NWN.FinalFantasy.Job.Contracts
{
    internal interface IAbilityDefinition
    {
        int MP(NWGameObject user);
        string CanUse(NWGameObject user, NWGameObject target);
        float CastingTime(NWGameObject user);
        float CooldownTime(NWGameObject user);
        void Impact(NWGameObject user, NWGameObject target);
        AbilityCategory Category { get; }

    }
}
