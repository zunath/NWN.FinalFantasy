namespace NWN.FinalFantasy.Service.AIService
{
    public interface IAIDataUpdatable
    {
        bool WasUpdated { get; set; }
        void CaptureDataMainThread();
        void ProcessDataAIThread();
    }
}
