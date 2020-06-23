using Newtonsoft.Json;
using ObsController.Core.Models;

namespace ObsWeather.Models
{
    public class SettingModel : SettingModelBase
    {
        /// <summary>
        /// 天気アイコンを表示するテキストソースの名前 (既定値: WeatherIcon)
        /// </summary>
        [JsonProperty("weatherIconSourceName")]
        public string WeatherIconTextSourceName { get; set; } = "WeatherIcon";

        /// <summary>
        /// 温度を表示するテキストソースの名前 (既定値: TemperatureText)
        /// </summary>
        [JsonProperty("temperatureTextSourceName")]
        public string TemperatureTextSourceName { get; set; } = "TemperatureText";

        /// <summary>
        /// OpenWeatherMap に関する設定
        /// </summary>
        [JsonProperty("openWeatherMap")]
        public OpenWeatherMapSetting OpenWeatherMap { get; set; } = new OpenWeatherMapSetting();
    }

    public class OpenWeatherMapSetting
    {
        /// <summary>
        /// OpenWeatherMap の API キー (既定値: 空白文字列)
        /// </summary>
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// 天気を取得する位置の緯度 (既定値: 35.685595 おおよそ皇居)
        /// </summary>
        [JsonProperty("latitude")]
        public double Latitude { get; set; } = 35.685595;

        /// <summary>
        /// 天気を取得する位置の経度 (既定値: 139.753026 おおよそ皇居)
        /// </summary>
        [JsonProperty("longitude")]
        public double Longitude { get; set; } = 139.753026;
    }
}
