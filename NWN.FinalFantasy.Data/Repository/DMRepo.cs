using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class DMRepo
    {
        public static EntityList<DM> Get()
        {
            return DB.GetList<DM>(Keys.DMList);
        }

        public static void Set(EntityList<DM> entities)
        {
            DB.Set(Keys.DMList, entities);
        }
    }
}
