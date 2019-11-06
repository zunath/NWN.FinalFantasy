namespace NWN.FinalFantasy.Job.Contracts
{
    internal interface IAbility
    {
        int MP(NWGameObject user);
        string CanUse(NWGameObject user, NWGameObject target);
        float CastingTime(NWGameObject user);
        float CooldownTime(NWGameObject user);
        void Impact(NWGameObject user, NWGameObject target);
        AbilityType Type { get; }

    }
}
