using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Data.Entity
{
    public class EntityList<T>: EntityBase
        where T: EntityBase
    {
        public List<T> Entities { get; set; }

        public EntityList(Guid id)
        {
            ID = id;
            Entities = new List<T>();
        }
    }
}
