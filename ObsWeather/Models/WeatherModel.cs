using ObsWeather.Extensions;
using ObsWeather.Models.OpenWeatherMap;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ObsWeather.Models
{
    public class WeatherModel
    {
        private static ReadOnlyDictionary<string, string>? _iconConvertData;

        public DateTimeOffset AnnouncedAt { get; }
        public double CurrentTemperature { get; }
        public double CurrentTemperatureKelvin { get; }
        public string CurrentWeatherText { get; }
        public string CityName { get; }

        public int? CurrentWeatherId { get; }
        public string? CurrentWeatherIcon { get; }


        public WeatherModel(CurrentWeatherModel model)
        {
            var weather = model.Weather?.FirstOrDefault();

            this.AnnouncedAt = DateTimeOffset.FromUnixTimeSeconds(model.Dt).ToLocalTime();
            this.CurrentTemperatureKelvin = model.Main?.Temp ?? -99;
            this.CurrentTemperature = this.CurrentTemperatureKelvin.ConvertToCelsius();
            this.CurrentWeatherText = weather?.Main ?? string.Empty;
            this.CityName = model.Name ?? string.Empty;
            this.CurrentWeatherId = weather?.Id;

            if (_iconConvertData == null)
            {
                return;
            }

            this.CurrentWeatherIcon = _iconConvertData[$"wi_owm_day_{this.CurrentWeatherId}"];
        }

        public static void CacheWeatherIcons(string filePath)
        {
            using var file = new StreamReader(filePath);

            var xml = XDocument.Load(file);
            var data = xml.Descendants("string")
                .ToDictionary(x => x.Attribute("name")?.Value ?? string.Empty, y => y.Value);

            _iconConvertData = new ReadOnlyDictionary<string, string>(data);
        }
    }
}
