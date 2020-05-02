namespace NWN.FinalFantasy.Service.PerkService
{
    public interface IPerkRequirement
    {
        string CheckRequirements(uint player);
        string RequirementText { get; }
    }

}
