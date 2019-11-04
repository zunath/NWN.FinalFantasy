using System;
using NWN.FinalFantasy.Data.Entity;

namespace NWN.FinalFantasy.Data.Repository
{
    public static class BugReportRepo
    {
        private static string BuildKey(Guid id)
        {
            return $"BugReport:{id.ToString()}";
        }

        public static void Set(BugReport entity)
        {
            var key = BuildKey(entity.ID);
            DB.Set(key, entity);
        }
    }
}
