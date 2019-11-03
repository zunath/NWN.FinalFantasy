using System.Collections.Generic;

namespace NWN.FinalFantasy.Data
{
    public abstract class EntityListBase<T>: EntityBase
        where T: EntityBase
    {
        public List<T> Entities { get; set; }

        protected EntityListBase()
        {
            Entities = new List<T>();
        }
    }
}
