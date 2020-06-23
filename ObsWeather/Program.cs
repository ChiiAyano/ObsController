using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OBS.WebSocket.NET;
using OBS.WebSocket.NET.Types;
using ObsController.Core;
using ObsController.Core.Objects;
using ObsWeather.Models;
using ObsWeather.Models.OpenWeatherMap;
using Reactive.Bindings;
using System;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ObsWeather
{
    class Program
    {
        private static HttpClient HttpClient { get; } = new HttpClient();
        private static SettingModel? Settings { get; set; }

        private static ReactiveTimer WeatherTimer { get; } = new ReactiveTimer(TimeSpan.FromSeconds(1));

        private static SceneItem? _weatherIcon;
        private static SceneItem? _temperatureText;
        private static DateTimeOffset _nextRefreshTime;
        private static ObsWebSocket? _obsWebSocket;

        private static async Task Main(string[] args)
        {
            // キャッシュ
            WeatherModel.CacheWeatherIcons(General.WeatherIconsPath);
            // 設定読み込み
            Settings = SettingLoader.Load<SettingModel>();

            if (string.IsNullOrWhiteSpace(Settings?.OpenWeatherMap.ApiKey))
            {
                Log.WriteLogLine("OpenWeatherMap へ接続するための API Key が見つかりません", Log.LogType.Error);
                return;
            }

            SetTimer();

            Connect();

            await ConsoleHost.WaitAsync();

            Disconnect();
        }

        private static async void ObsWebSocket_Disconnected(object? sender, EventArgs e)
        {
            Log.WriteLogLine($"{Settings?.Url} から切断しました");

            Disconnect();
            WeatherTimer.Stop();

            await Task.Delay(TimeSpan.FromSeconds(3));

            // 再接続
            Connect();
        }

        private static void ObsWebSocket_Connected(object? sender, EventArgs e)
        {
            Log.WriteLogLine($"{Settings?.Url} に接続しました");

            _weatherIcon = GetTextItem(Settings?.WeatherIconTextSourceName);
            _temperatureText = GetTextItem(Settings?.TemperatureTextSourceName);

            if (WeatherTimer.IsEnabled)
            {
                return;
            }

            WeatherTimer.Start();
        }

        private static void Connect()
        {
            _obsWebSocket = new ObsWebSocket();
            _obsWebSocket.Connected += ObsWebSocket_Connected;
            _obsWebSocket.Disconnected += ObsWebSocket_Disconnected;
            _obsWebSocket.Connect(Settings!.Url, Settings.Password);
        }

        private static void Disconnect()
        {
            if (_obsWebSocket == null)
            {
                return;
            }

            if (_obsWebSocket.IsConnected)
            {
                _obsWebSocket.Disconnect();
            }

            _obsWebSocket.Connected -= ObsWebSocket_Connected;
            _obsWebSocket.Disconnected -= ObsWebSocket_Disconnected;

            _obsWebSocket = null;
        }

        private static bool ObsConnected() => _obsWebSocket?.IsConnected ?? false;

        private static async Task<string?> GetReportAsync(string url)
        {
            var response = await HttpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        private static void SetTimer()
        {
            WeatherTimer
                .Where(w => DateTimeOffset.Now >= _nextRefreshTime)
                .Subscribe(async _ =>
                {
                    var json = await GetReportAsync($"https://api.openweathermap.org/data/2.5/weather?lat={Settings?.OpenWeatherMap.Latitude}&lon={Settings?.OpenWeatherMap.Longitude}&appid={Settings?.OpenWeatherMap.ApiKey}&lang=ja");
                    if (json == null)
                    {
                        return;
                    }

                    var data = JsonConvert.DeserializeObject<CurrentWeatherModel>(json);
                    var weather = new WeatherModel(data);

                    SetText(_weatherIcon, weather.CurrentWeatherIcon ?? string.Empty);
                    SetText(_temperatureText, $"{weather.CurrentTemperature:0.0}℃");

                    _nextRefreshTime = GetNextRefreshTime();

                    Log.WriteLogLine($"現在の天気: {weather.CurrentWeatherText} ({weather.CurrentTemperature:0.00}℃)");
                    Log.WriteLogLine($"次の送出時刻: {_nextRefreshTime:MM/dd HH:mm:ss}");
                });
        }

        /// <summary>
        /// 書き換えるテキストオブジェクトを OBS Studio から取得
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        private static SceneItem? GetTextItem(string? itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                return default;
            }

            if (!ObsConnected())
            {
                return default;
            }

            return _obsWebSocket?.Api.GetCurrentScene().Items.FirstOrDefault(f => f.SourceName == itemName);
        }

        /// <summary>
        /// テキスト書き換えを OBS Studio へ通知
        /// </summary>
        /// <param name="textItem"></param>
        /// <param name="message"></param>
        private static void SetText(SceneItem? textItem, string message)
        {
            if (textItem == null)
            {
                return;
            }

            if (!ObsConnected())
            {
                return;
            }

            var prop = new TextProperty { Source = textItem.SourceName, Text = message };
            var jObject = JObject.FromObject(prop);
            _obsWebSocket?.Api.SetSourceSettings(textItem.SourceName, jObject);
        }

        /// <summary>
        /// 次の更新日時を設定
        /// </summary>
        /// <returns></returns>
        private static DateTimeOffset GetNextRefreshTime()
        {
            return DateTimeOffset.Now + TimeSpan.FromMinutes(15);
        }
    }
}
