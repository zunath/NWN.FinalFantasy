using System;

namespace NWN.FinalFantasy.Entity
{
    public abstract class EntityBase
    {
        public Guid ID { get; set; }
        public DateTime DateCreated { get; set; }

        protected EntityBase()
        {
            ID = Guid.NewGuid();
            DateCreated = DateTime.UtcNow;
        }
    }
}