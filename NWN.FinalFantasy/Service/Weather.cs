using System;
using System.Collections.Generic;
using NWN.FinalFantasy.Core;
using NWN.FinalFantasy.Core.NWScript.Enum;
using NWN.FinalFantasy.Service.WeatherService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using WeatherType = NWN.FinalFantasy.Core.NWScript.Enum.Area.WeatherType;

namespace NWN.FinalFantasy.Service
{
    public static class Weather
    {
        private static readonly Dictionary<WeatherRegionType, WeatherRegion> _weatherRegions = new Dictionary<WeatherRegionType, WeatherRegion>();
        private static readonly Dictionary<uint, WeatherArea> _weatherAreas = new Dictionary<uint, WeatherArea>();
        private static readonly Dictionary<WeatherRegionType, List<uint>> _areaWeatherRegions = new Dictionary<WeatherRegionType, List<uint>>();
        private static int _nextWeatherChangeHour = 0;
        private static bool _hasInitialized = false;

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
                    _areaWeatherRegions[regionType] = new List<uint>();
                }

                _weatherAreas[area] = new WeatherArea(area, regionType);
                _areaWeatherRegions[regionType].Add(area);
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
            if (!_hasInitialized || hour != lastHour && _nextWeatherChangeHour == hour)
            {
                _hasInitialized = true;

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

        private static void AdjustWeather(WeatherRegionType regionType, uint area)
        {
            var weatherArea = _weatherAreas[area];
            var weather = _weatherRegions[regionType];
            void CalculateHeat()
            {
                var calculatedHeat = weather.HeatModifier;

                // Heat is affected by time of year.
                calculatedHeat += (GetIsNight() ? -2 : 2) + Random.Next(0, 4) + (6 - Math.Abs(GetCalendarMonth() - 6));
                if (calculatedHeat < 1) calculatedHeat = 1;
                else if (calculatedHeat > 10) calculatedHeat = 10;

                weather.CurrentHeat = calculatedHeat;
            }

            void CalculateWind()
            {
                var calculatedWind = weather.CurrentWind + weather.WindModifier + Random.D10(1);

                if (GetIsAreaNatural(area))
                    calculatedWind--;

                if (calculatedWind < 1) calculatedWind = 1;
                else if (calculatedWind > 10) calculatedWind = 10;

                weather.CurrentWind = calculatedWind;
            }

            void CalculateHumidity()
            {
                var calculatedHumidity = weather.CurrentHumidity + weather.HumidityModifier;

                // Humidity is random by moves slowly.
                calculatedHumidity += Random.Next(2 * weather.CurrentWind + 1) - weather.CurrentWind;
                if (calculatedHumidity > 10) calculatedHumidity = 20 - calculatedHumidity;
                else if (calculatedHumidity < 1) calculatedHumidity = 1 - calculatedHumidity;

                weather.CurrentHumidity = calculatedHumidity;
            }

            CalculateHeat();
            CalculateHumidity();
            CalculateWind();

            _nextWeatherChangeHour = GetTimeHour() + (11 - weather.CurrentWind);
            if (_nextWeatherChangeHour > 23) _nextWeatherChangeHour -= 24;

            Console.WriteLine($"Heat = {weather.CurrentHeat}");
            Console.WriteLine($"Wind = {weather.CurrentWind}");
            Console.WriteLine($"Humidity = {weather.CurrentHumidity}");
            Console.WriteLine($"Next Change Hour = {_nextWeatherChangeHour}");


            // Process weather rules for this specific area.
            if (weather.CurrentHumidity > 7 && weather.CurrentHeat > 3)
            {
                if (weather.CurrentHeat < 6 && weather.CurrentWind < 3)
                {
                    SetWeather(area, WeatherType.Clear);
                }
                else
                {
                    SetWeather(area, WeatherType.Rain);
                }
            }
            else if (weather.CurrentHumidity > 7)
            {
                SetWeather(area, WeatherType.Snow);
            }
            else
            {
                SetWeather(area, WeatherType.Clear);
            }

            if (weather.CurrentHeat > 4 &&
                weather.CurrentHumidity > 7 &&
                (
                    weatherArea.IsStorming && Random.D20(1) - weather.CurrentWind < 1) ||
                    weatherArea.IsStorming && Random.D3(1) == 1
                )
            {
                SetSkyBox(Skybox.GrassStorm, area);
                // todo: Thunderstorm function 
            }
            else
            {
                SetSkyBox(weatherArea.OriginalSkybox, area);
                weatherArea.IsStorming = false;
            }
        }

        private static void ApplyWeatherEffects(uint player)
        {
            if (!GetIsPC(player)) return;
            var area = GetArea(player);
            if (!_weatherAreas.ContainsKey(area)) return;
            var weatherArea = _weatherAreas[area];
            var weatherRegion = _weatherRegions[weatherArea.Region];
            string message;
            var humidity = weatherRegion.CurrentHumidity;
            var heat = weatherRegion.CurrentHeat;
            var wind = weatherRegion.CurrentWind;

            // Stormy weather
            if (weatherArea.IsStorming)
                message = weatherRegion.StormText;
            // Rain or mist
            else if (humidity > 7 && heat > 3)
            {
                // Mist
                if (heat < 6 && wind < 3)
                {
                    message = weatherRegion.MistText;
                }
                // Humid
                else if (heat > 7)
                {
                    message = weatherRegion.RainWarmText;
                }
                else
                {
                    message = weatherRegion.RainNormalText;
                }
            }
            // Snow
            else if (humidity > 7)
            {
                message = weatherRegion.SnowText;
            }
            // Freezing
            else if (heat < 3)
            {
                message = weatherRegion.FreezingText;
            }
            // Boiling
            else if (heat > 8)
            {
                message = weatherRegion.ScorchingText;
            }
            // Cold
            else if (heat < 5)
            {
                if (wind < 5) message = weatherRegion.ColdMildText;
                else if (wind < 8) message = weatherRegion.ColdCloudyText;
                else message = weatherRegion.ColdWindyText;
            }
            // Hot
            else if (heat > 6)
            {
                if (wind < 5) message = weatherRegion.WarmMildText;
                else if (wind < 8) message = weatherRegion.WarmCloudyText;
                else message = weatherRegion.WarmWindyText;
            }
            // Mild
            else if (wind < 5)
            {
                message = GetIsNight() ? weatherRegion.MildNightText: weatherRegion.MildText;
            }
            // Cloudy
            else if (wind < 8)
            {
                message = weatherRegion.CloudyText;
            }
            // Windy
            else
            {
                message = weatherRegion.WindyText;
            }

            SendMessageToPC(player, message);
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
