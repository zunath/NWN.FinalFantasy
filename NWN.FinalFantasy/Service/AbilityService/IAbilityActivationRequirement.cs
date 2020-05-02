namespace NWN.FinalFantasy.Service.AbilityService
{
    public interface IAbilityActivationRequirement
    {
        string CheckRequirements(uint player);
        void AfterActivationAction(uint player);
    }
}
