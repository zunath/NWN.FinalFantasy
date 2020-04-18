using System;
using System.Collections.Generic;

namespace NWN.FinalFantasy.Entity
{
    public class EntityList<T> : List<T>
        where T : EntityBase
    {
    }
}