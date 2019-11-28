namespace NWN.FinalFantasy.Core.Contracts
{
    public interface IApplication
    {
        void OnStart();
        void OnNWNContextReady();
        void OnMainLoop(ulong frame);
        int OnRunScript(string script, uint oidSelf);
    }
}
