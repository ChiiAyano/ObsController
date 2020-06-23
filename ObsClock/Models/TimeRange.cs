using Newtonsoft.Json;
using System;

namespace ObsClock.Models
{
    public class TimeRange
    {
        /// <summary>
        /// 範囲開始時刻
        /// </summary>
        [JsonProperty("start")]
        public TimeSpan Start { get; set; }
        /// <summary>
        /// 範囲終了時刻
        /// </summary>
        [JsonProperty("end")]
        public TimeSpan End { get; set; }
    }
}
