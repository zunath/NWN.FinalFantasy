using System;

namespace NWN.FinalFantasy.Data
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
