using System;

namespace NWN.FinalFantasy.Enumeration
{
    public enum NPCGroupType
    {
        [NPCGroup("Invalid")]
        Invalid = 0,
        [NPCGroup("GOBLINZZZ")]
        TestGroup = 1,
    }

    public class NPCGroupAttribute : Attribute
    {
        public string Name { get; set; }

        public NPCGroupAttribute(string name)
        {
            Name = name;
        }
    }
}
