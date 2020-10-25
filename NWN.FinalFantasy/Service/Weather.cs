using System;
using System.Collections.Generic;
using System.Text;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript;
using NWN.FinalFantasy.Service.WeatherService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using WeatherType = NWN.FinalFantasy.Core.NWScript.Enum.Area.WeatherType;

namespace NWN.FinalFantasy.Service
{
    public static class Weather
    {
        private static readonly Dictionary<WeatherRegionType, WeatherRegion> _weatherRegions = new Dictionary<WeatherRegionType, WeatherRegion>();
        private static readonly Dictionary<WeatherRegionType, List<WeatherArea>> _areaWeatherRegions = new Dictionary<WeatherRegionType, List<WeatherArea>>();

        /// <summary>
        /// When the module loads, create all of the weather region details and store them into cache.
        /// </summary>
        [NWNEventHandler("mod_load")]
        public static void LoadData()
        {
            LoadRegions();
            MapAreaRegions();
        }

        /// <summary>
        /// Creates entries for each of the available regions.
        /// </summary>
        private static void LoadRegions()
        {
            _weatherRegions[WeatherRegionType.Balamb] = new WeatherRegion
            {
                HumidityModifier = 2,
                HeatModifier = -2
            };
        }

        /// <summary>
        /// Checks every area for which region they belong to and then caches the result.
        /// </summary>
        private static void MapAreaRegions()
        {
            for (var area = GetFirstArea(); GetIsObjectValid(area); area = GetNextArea())
            {
                if (GetIsAreaInterior(area) || !GetIsAreaAboveGround(area)) continue;

                var regionId = GetLocalInt(area, "WEATHER_REGION_ID");
                if (regionId <= 0) continue;
                if (!Enum.IsDefined(typeof(WeatherRegionType), regionId)) continue;

                var regionType = (WeatherRegionType) regionId;
                if (!_areaWeatherRegions.ContainsKey(regionType))
                {
                    _areaWeatherRegions[regionType] = new List<WeatherArea>();
                }

                var weatherArea = new WeatherArea(area);
                _areaWeatherRegions[regionType].Add(weatherArea);
            }
        }

        /// <summary>
        /// Every heartbeat, process weather for each region and then apply any effects associated with the weather onto each player within those areas.
        /// </summary>
        [NWNEventHandler("mod_heartbeat")]
        public static void ProcessWeather()
        {
            var hour = GetTimeHour();
            var lastHour = GetLocalInt(OBJECT_SELF, "WEATHER_LAST_HOUR");
            if (hour != lastHour)
            {
                // Adjust weather across all registered areas
                foreach (var (regionType, areas) in _areaWeatherRegions)
                {
                    foreach (var area in areas)
                    {
                        AdjustWeather(regionType, area);
                    }
                }

                // Iterate over all players and adjust their weather effects, if necessary.
                for (var player = GetFirstPC(); GetIsObjectValid(player); player = GetNextPC())
                {
                    ApplyWeatherEffects(player);
                }

                SetLocalInt(OBJECT_SELF, "WEATHER_LAST_HOUR", hour);
            }
        }

        private static void AdjustWeather(WeatherRegionType regionType, WeatherArea area)
        {
            var weather = _weatherRegions[regionType];
            void CalculateHeat()
            {
                var calculatedHeat = weather.HeatModifier;

                // Heat is affected by time of year.
                calculatedHeat += (GetIsNight() ? -2 : 2) + Random.Next(0, 4) + (6 - Math.Abs(GetCalendarMonth() - 6));
                if (calculatedHeat < 1) calculatedHeat = 1;

                weather.CurrentHeat = calculatedHeat;
            }

            void CalculateWind()
            {
                var calculatedWind = weather.WindModifier + Random.D10(1);

                if (GetIsAreaNatural(area.Area))
                    calculatedWind--;

                if (calculatedWind < 1) calculatedWind = 1;
                else if (calculatedWind > 10) calculatedWind = 10;

                weather.CurrentWind = calculatedWind;
            }

            void CalculateHumidity()
            {
                var calculatedHumidity = weather.HumidityModifier;

                // Humidity is random by moves slowly.
                calculatedHumidity += Random.Next(2 * weather.CurrentWind + 1) - weather.CurrentWind;
                if (calculatedHumidity > 10) calculatedHumidity = 20 - calculatedHumidity;
                else if (calculatedHumidity < 1) calculatedHumidity = 1 - calculatedHumidity;

                weather.CurrentHumidity = calculatedHumidity;
            }

            CalculateHeat();
            CalculateHumidity();
            CalculateWind();

            Console.WriteLine($"Heat = {weather.CurrentHeat}");
            Console.WriteLine($"Wind = {weather.CurrentWind}");
            Console.WriteLine($"Humidity = {weather.CurrentHumidity}");

            // Process weather rules for this specific area.
            if (weather.CurrentHumidity > 7 && weather.CurrentHeat > 3)
            {
                if (weather.CurrentHeat < 6 && weather.CurrentWind < 3)
                {
                    SetWeather(area.Area, WeatherType.Clear);
                }
                else
                {
                    SetWeather(area.Area, WeatherType.Rain);
                }
            }
            else if (weather.CurrentHumidity > 7)
            {
                SetWeather(area.Area, WeatherType.Snow);
            }
            else
            {
                SetWeather(area.Area, WeatherType.Clear);
            }

            if (weather.CurrentHeat > 4 &&
                weather.CurrentHumidity > 7 &&
                area.IsStorming)
            {

            }

        }

        private static void ApplyWeatherEffects(uint player)
        {

        }

        [NWNEventHandler("testweather")]
        public static void TestWeatherChanges()
        {
            var hour = GetTimeHour() + 1;
            if (hour > 23)
                hour = 0;

            SetTime(hour, 0, 0 ,0);
            Console.WriteLine($"New hour = {hour}");
        }
    }
}
