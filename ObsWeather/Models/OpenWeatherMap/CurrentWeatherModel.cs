using Newtonsoft.Json;

namespace ObsWeather.Models.OpenWeatherMap
{

    public class CurrentWeatherModel
    {
        [JsonProperty("coord")]
        public Coord? Coord { get; set; }
        [JsonProperty("weather")]
        public Weather[]? Weather { get; set; }
        [JsonProperty("base")]
        public string? Base { get; set; }
        [JsonProperty("main")]
        public Main? Main { get; set; }
        [JsonProperty("wind")]
        public Wind? Wind { get; set; }
        [JsonProperty("clouds")]
        public Clouds? Clouds { get; set; }
        [JsonProperty("dt")]
        public int Dt { get; set; }
        [JsonProperty("sys")]
        public Sys? Sys { get; set; }
        [JsonProperty("timezone")]
        public int TimeZone { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("cod")]
        public int Cod { get; set; }
    }

    public class Coord
    {
        [JsonProperty("lon")]
        public float Lon { get; set; }
        [JsonProperty("lat")]
        public float Lat { get; set; }
    }

    public class Main
    {
        [JsonProperty("temp")]
        public float Temp { get; set; }
        [JsonProperty("feels_like")]
        public float FeelsLike { get; set; }
        [JsonProperty("temp_min")]
        public float TempMin { get; set; }
        [JsonProperty("temp_max")]
        public float TempMax { get; set; }
        [JsonProperty("pressure")]
        public float Pressure { get; set; }
        [JsonProperty("humidity")]
        public float Humidity { get; set; }
    }

    public class Wind
    {
        [JsonProperty("speed")]
        public float Speed { get; set; }
        [JsonProperty("deg")]
        public int Deg { get; set; }
    }

    public class Clouds
    {
        [JsonProperty("all")]
        public int All { get; set; }
    }

    public class Sys
    {
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("country")]
        public string? Country { get; set; }
        [JsonProperty("sunrise")]
        public int Sunrise { get; set; }
        [JsonProperty("sunset")]
        public int Sunset { get; set; }
    }

    public class Weather
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("main")]
        public string? Main { get; set; }
        [JsonProperty("description")]
        public string? Description { get; set; }
        [JsonProperty("icon")]
        public string? Icon { get; set; }
    }

}
