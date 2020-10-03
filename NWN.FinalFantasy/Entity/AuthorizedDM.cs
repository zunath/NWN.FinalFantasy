using MessagePack;

namespace NWN.FinalFantasy.Entity
{
    public enum AuthorizationLevel
    {
        Player = 1,
        DM = 2,
        Admin = 3
    }

    [MessagePackObject(true)]
    public class AuthorizedDM: EntityBase
    {
        public string Name { get; set; }
        public string CDKey { get; set; }
        public AuthorizationLevel Authorization { get; set; }
        public override string KeyPrefix => "AuthorizedDM";
    }
}
