using Newtonsoft.Json;
using ObsController.Core.Models;
using System;

namespace ObsClock.Models
{
    public class SettingModel : SettingModelBase
    {
        /// <summary>
        /// 時報を表示するテキストソースの名前 (既定値: Clock)
        /// </summary>
        [JsonProperty("sourceName")]
        public string TextSourceName { get; set; } = "Clock";

        /// <summary>
        /// "時" の表示フォーマット (既定値: h)
        /// </summary>
        [JsonProperty("hoursFormat")]
        public string HoursFormat { get; set; } = "h";

        /// <summary>
        /// 正午のときの時表示。"時"の表示フォーマットが "H" を含む場合、この設定は無視される (既定値: 0 (ゼロ時とする))
        /// </summary>
        [JsonProperty("noon")]
        public NoonAppearance NoonAppearance { get; set; } = NoonAppearance.Zero;

        /// <summary>
        /// "時" の表示フォーマットによって穴埋めする文字 (既定値: 埋めなし)
        /// </summary>
        [JsonProperty("fillCharacter")]
        public string FillCharacter { get; set; } = string.Empty;

        /// <summary>
        /// 時報表示の長さ。常に表示する時間帯では無効 (既定値: 5秒)
        /// </summary>
        [JsonProperty("showDuration")]
        public int ShowDuration { get; set; } = 5;

        /// <summary>
        /// 時報を表示する間隔。常に表示する時間帯は無効 (既定値: 30分)
        /// </summary>
        [JsonProperty("showInterval")]
        public int ShowInterval { get; set; } = 30;

        /// <summary>
        /// 常に時刻表示をする時間帯の指定 (既定値: 午前6時～午前10時)
        /// </summary>
        [JsonProperty("alwaysShowClockTimes")]
        public TimeRange[] AlwaysShowClockTimes { get; set; } = new[] { new TimeRange { Start = new TimeSpan(6, 0, 0), End = new TimeSpan(10, 0, 0) } };
    }
}
