using Newtonsoft.Json;
using ObsController.Core.Models;

namespace ObsUserLock.Models
{
    public class SettingModel : SettingModelBase
    {
        /// <summary>
        /// セッションがロック状態のときに遷移するシーン名 (既定値: Locked)
        /// </summary>
        [JsonProperty("session_locked_scene")]
        public string SessionLockingSceneName { get; set; } = "Locked";

        /// <summary>
        /// セッションがアンロック状態になった場合には前のシーンを復帰させるかどうか (既定値: true)
        /// </summary>
        [JsonProperty("resume_before_scene_unlocked")]
        public bool ResumeScene { get; set; } = true;
    }
}
