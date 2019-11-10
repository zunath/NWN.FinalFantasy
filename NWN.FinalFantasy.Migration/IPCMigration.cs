namespace NWN.FinalFantasy.Migration
{
    internal interface IPCMigration
    {
        int Version { get; }
        void RunMigration(NWGameObject player);
    }
}
