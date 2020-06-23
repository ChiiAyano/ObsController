using Newtonsoft.Json.Linq;
using OBS.WebSocket.NET;
using OBS.WebSocket.NET.Types;
using ObsClock.Generator;
using ObsClock.Models;
using ObsController.Core;
using ObsController.Core.Objects;
using Reactive.Bindings;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ObsClock
{
    class Program
    {
        private static ObsWebSocket ObsWebSocket { get; } = new ObsWebSocket();
        private static ReactiveTimer ClockTimer { get; } = new ReactiveTimer(TimeSpan.FromSeconds(0.5));
        private static SceneItem? _textItem;
        private static DateTimeOffset _nextShowTime;
        private static SettingModel? Settings { get; set; }

        private static async Task Main(string[] args)
        {
            Settings = SettingLoader.Load<SettingModel>();

            SetTimer();

            ObsWebSocket.Connected += ObsWebSocket_Connected;
            ObsWebSocket.Disconnected += ObsWebSocket_Disconnected;

            ObsWebSocket.Connect(Settings.Url, Settings.Password);

            await ConsoleHost.WaitAsync();

            ObsWebSocket.Disconnect();

            static void SetTimer()
            {
                ClockTimer
                    .Where(w => _textItem != default)
                    .Where(w => DateTimeOffset.Now >= _nextShowTime)
                    .Subscribe(_ =>
                    {
                        if (IsAlwaysShowClockTime)
                        {
                            // 常に表示
                            ShowClock();

                            _nextShowTime = NextShowClockTime(1);
                        }
                        else
                        {
                            ShowClock(TimeSpan.FromSeconds(Settings!.ShowDuration));

                            _nextShowTime = NextShowClockTime(Settings!.ShowInterval);
                        }


                        Log.WriteLogLine($"次の送出時刻: {_nextShowTime:MM/dd HH:mm:ss}");
                    });
            }
        }

        private static void ObsWebSocket_Disconnected(object? sender, EventArgs e)
        {
            Log.WriteLogLine($"{Settings?.Url} から切断しました");
            ClockTimer.Stop();
        }

        private static void ObsWebSocket_Connected(object? sender, EventArgs e)
        {
            Log.WriteLogLine($"{Settings?.Url} に接続しました");

            _textItem = GetTextItem(Settings?.TextSourceName);
            if (_textItem == default)
            {
                Log.WriteLogLine($"オブジェクト \'{Settings?.TextSourceName}\' が見つかりませんでした", Log.LogType.Error);
                return;
            }

            ClockTimer.Start();
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

            if (!ObsWebSocket.IsConnected)
            {
                return default;
            }

            return ObsWebSocket.Api.GetCurrentScene().Items.FirstOrDefault(f => f.SourceName == itemName);
        }

        /// <summary>
        /// テキスト書き換えを OBS Studio へ通知
        /// </summary>
        /// <param name="item"></param>
        /// <param name="message"></param>
        private static void SetText(string message)
        {
            if (_textItem == null)
            {
                return;
            }

            if (!ObsWebSocket.IsConnected)
            {
                return;
            }

            var prop = new TextProperty { Source = _textItem.SourceName, Text = message };
            var jObject = JObject.FromObject(prop);
            ObsWebSocket.Api.SetSourceSettings(_textItem.SourceName, jObject);
        }

        /// <summary>
        /// 表示する時報を生成
        /// </summary>
        /// <returns></returns>
        private static string GetClockText()
        {
            return TimeGenerator.Generate(DateTimeOffset.Now, Settings!.HoursFormat, Settings!.NoonAppearance, Settings!.FillCharacter.FirstOrDefault());
        }

        /// <summary>
        /// 時報を表示 (永続的)
        /// </summary>
        /// <param name="item"></param>
        private static void ShowClock()
        {
            // 一度消す
            SetText(string.Empty);

            // 新しいのを作る
            _textItem = GetTextItem(Settings?.TextSourceName);

            var clockText = GetClockText();
            SetText(clockText);

            Log.WriteLogLine($"時刻送出: {clockText}");
        }

        /// <summary>
        /// 時報を表示
        /// </summary>
        /// <param name="item"></param>
        /// <param name="duration"></param>
        private static async void ShowClock(TimeSpan duration)
        {
            // 一度消す
            SetText(string.Empty);

            // 新しいのを作る
            _textItem = GetTextItem(Settings?.TextSourceName);

            var clockText = GetClockText();

            Log.WriteLogLine($"時刻送出: {clockText} 消去時刻: {DateTimeOffset.Now + duration:HH:mm:ss}");

            SetText(clockText);

            await Task.Delay(duration);

            SetText(string.Empty);

            Log.WriteLogLine($"{DateTimeOffset.Now:HH:mm:ss} 時刻消去");
        }

        /// <summary>
        /// 次に時報を送出する時刻を算出する
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        private static DateTimeOffset NextShowClockTime(int interval)
        {
            // 一番近い分まで持って行く
            var now = DateTimeOffset.Now;

            // 計算で次の間隔にする
            // 例えば 15 分間隔で、今が 10 時 40 分だった場合、 40 + (15 - (40 % 15)) = 40 + 5 = 45
            var n = now.Minute + (interval - (now.Minute % interval));
            var hourOffset = 0;

            if (n >= 60)
            {
                n -= 60;
                hourOffset = 1;
            }

            return new DateTimeOffset(now.Year, now.Month, now.Day, now.Hour, n, 0, now.Offset).AddHours(hourOffset);
        }

        private static bool IsAlwaysShowClockTime
        {
            get
            {
                if (!Settings!.AlwaysShowClockTimes.Any())
                {
                    // 設定がない場合は常に表示する
                    return true;
                }

                var now = DateTimeOffset.Now;
                var result = Settings!.AlwaysShowClockTimes.Any(a => a.Start <= now.TimeOfDay && now.TimeOfDay <= a.End);

                return result;
            }
        }
    }
}
