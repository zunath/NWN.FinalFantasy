using System;

namespace NWN.FinalFantasy.Data
{
    public abstract class EntityBase
    {
        public Guid ID { get; set; }

        protected EntityBase()
        {
            ID = Guid.NewGuid();
        }
    }
}
