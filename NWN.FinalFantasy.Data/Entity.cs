using System;

namespace NWN.FinalFantasy.Data
{
    public abstract class Entity
    {
        public Guid ID { get; set; }

        protected Entity()
        {
            ID = Guid.NewGuid();
        }
    }
}
