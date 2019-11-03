namespace NWN.FinalFantasy.Migration
{
    internal interface IServerMigration
    {
        int Version { get; }
        void RunMigration();
    }
}
