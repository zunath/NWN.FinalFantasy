using NWN.FinalFantasy.Core.NWScript.Enum;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Service.WeatherService
{
    public class WeatherArea
    {
        public uint Area { get; set; }
        public Skybox OriginalSkybox { get; set; }
        public int OriginalSunFogAmount { get; set; }
        public int OriginalMoonFogAmount { get; set; }
        public FogColor OriginalSunFogColor { get; set; }
        public FogColor OriginalMoonFogColor { get; set; }
        public bool IsStorming { get; set; }

        public WeatherArea(uint area)
        {
            Area = area;
            OriginalSkybox = GetSkyBox(area);
            OriginalSunFogAmount = GetFogAmount(FogType.Sun, area);
            OriginalMoonFogAmount = GetFogAmount(FogType.Moon, area);
            OriginalSunFogColor = GetFogColor(FogType.Sun, area);
            OriginalMoonFogColor = GetFogColor(FogType.Moon, area);
            IsStorming = false;
        }
    }
}
