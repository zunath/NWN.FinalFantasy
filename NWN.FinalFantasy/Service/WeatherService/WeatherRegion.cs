using NWN.FinalFantasy.Core.NWScript.Enum;

namespace NWN.FinalFantasy.Service.WeatherService
{
    public class WeatherRegion
    {
        public int CurrentHeat { get; set; } = 0;
        public int CurrentHumidity { get; set; } = Random.Next(10) + 1;
        public int CurrentWind { get; set; } = 0;
        public WeatherType CurrentWeather { get; set; } = WeatherType.Clear;

        public int HeatModifier { get; set; } = 0;
        public int HumidityModifier { get; set; } = 0;
        public int WindModifier { get; set; } = 0;
        public bool HasSandStorms { get; set; } = false;
        public string CloudyText { get; set; } = WeatherMessages.Cloudy;
        public string ColdCloudyText { get; set; } = WeatherMessages.ColdCloudy;
        public string ColdMildText { get; set; } = WeatherMessages.ColdMild;
        public string ColdWindyText { get; set; } = WeatherMessages.ColdWindy;
        public string FreezingText { get; set; } = WeatherMessages.Freezing;
        public string MildText { get; set; } = WeatherMessages.Mild;
        public string MildNightText { get; set; } = WeatherMessages.MildNight;
        public string MistText { get; set; } = WeatherMessages.Mist;
        public string WarmCloudyText { get; set; } = WeatherMessages.WarmCloudy;
        public string WarmMildText { get; set; } = WeatherMessages.WarmMild;
        public string WarmWindyText { get; set; } = WeatherMessages.WarmWindy;
        public string RainNormalText { get; set; } = WeatherMessages.RainNormal;
        public string RainWarmText { get; set; } = WeatherMessages.RainWarm;
        public string ScorchingText { get; set; } = WeatherMessages.Scorching;
        public string SnowText { get; set; } = WeatherMessages.Snow;
        public string StormText { get; set; } = WeatherMessages.Storm;
        public string WindyText { get; set; } = WeatherMessages.Windy;
    }
}
