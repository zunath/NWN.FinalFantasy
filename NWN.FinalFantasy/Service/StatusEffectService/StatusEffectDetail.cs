using System;

namespace NWN.FinalFantasy.Service.StatusEffectService
{
    public class StatusEffectDetail
    {
        public string Name { get; set; }
        public int EffectIconId { get; set; }
        public Action<uint> GrantAction { get; set; }
        public Action<uint> RemoveAction { get; set; }
        public Action<uint> TickAction { get; set; }

        public StatusEffectDetail()
        {
            Name = string.Empty;
            EffectIconId = 0;
        }
    }
}
