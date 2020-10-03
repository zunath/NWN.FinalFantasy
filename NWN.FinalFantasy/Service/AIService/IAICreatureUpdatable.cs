namespace NWN.FinalFantasy.Service.AIService
{
    public interface IAICreatureUpdatable
    {
        bool WasUpdated { get; set; }
        void CreatureRemovedMainThread(uint creature);
        void CreatureAddedMainThread(uint creature);
    }
}
