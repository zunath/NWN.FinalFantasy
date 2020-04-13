using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Entity
{
    public class EntityList<T> : EntityBase
        where T : EntityBase
    {
        public List<T> Entities { get; set; }

        public EntityList()
        {
            ID = Guid.NewGuid();
            Entities = new List<T>();
        }

        public EntityList(Guid id)
        {
            ID = id;
            Entities = new List<T>();
        }
    }
}