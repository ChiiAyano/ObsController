using Newtonsoft.Json;

namespace ObsController.Core.Models
{
    public abstract class SettingModelBase
    {
        /// <summary>
        /// 接続先アドレス (既定値: ws://127.0.0.1:4444/)
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; } = "ws://127.0.0.1:4444/";

        /// <summary>
        /// パスワード (既定値: [null])
        /// </summary>
        [JsonProperty("password")]
        public string? Password { get; set; }
    }
}
