using ObsUserLock.Platform;
using System;
using OBS.WebSocket.NET;
using ObsUserLock.Models;
using ObsController.Core;
using System.Threading.Tasks;
using OBS.WebSocket.NET.Types;
using System.Linq;

namespace ObsUserLock
{
    internal class Program
    {
        private static ISessionManager? SessionManager { get; set; }
        private static SettingModel? Settings { get; set; }

        private static ObsWebSocket? _obsWebSocket;

        private static OBSScene? _LockingScene;
        private static OBSScene? _currentScene;

        private static async Task Main(string[] args)
        {
            Settings = SettingLoader.Load<SettingModel>();

            // TODO: プラットフォーム依存 (しかし Windows 以外がわからん)
            SessionManager = new SessionManager();

            Connect();

            await ConsoleHost.WaitAsync();
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

            SessionManager!.Stop();

            _obsWebSocket = null;
        }

        private static async void ObsWebSocket_Disconnected(object? sender, EventArgs e)
        {
            Log.WriteLogLine($"{Settings?.Url} から切断しました");

            SessionManager!.SessionChanged -= SessionManager_SessionChanged;

            Disconnect();

            //await Task.Delay(TimeSpan.FromSeconds(3));

            //// 再接続
            //Connect();
        }

        private static void ObsWebSocket_Connected(object? sender, EventArgs e)
        {
            Log.WriteLogLine($"{Settings?.Url} に接続しました");
            SessionManager!.SessionChanged += SessionManager_SessionChanged;

            _LockingScene = GetScene(Settings!.SessionLockingSceneName);

            SessionManager!.Listen();
        }

        private static OBSScene? GetScene(string? sceneName)
        {
            return _obsWebSocket?.Api.ListScenes().FirstOrDefault(f => f.Name == sceneName);
        }

        private static OBSScene? GetCurrentScene()
        {
            return _obsWebSocket?.Api.GetCurrentScene();
        }

        private static void SetScene(OBSScene scene)
        {
            _obsWebSocket?.Api.SetCurrentScene(scene.Name);
        }

        #region セッション確認

        private static void SessionManager_SessionChanged(object? sender, SessionChangedEventArgs e)
        {
            if (_LockingScene == null)
            {
                return;
            }

            OBSScene? transition = null;

            if (e.State == SessionState.SessionLock)
            {
                if (Settings!.ResumeScene)
                {
                    // 開いていたシーンを記憶する
                    _currentScene = GetCurrentScene();
                }

                transition = _LockingScene;
            }
            else if (e.State == SessionState.SessionUnlock)
            {
                if (_currentScene != null)
                {
                    transition = _currentScene;
                }
            }

            Log.WriteLogLine($"セッション状態が変化しました ({e.State}): 遷移先シーン情報: {transition?.Name ?? "(null)"}");

            if (transition == null)
            {
                // 遷移先シーンが指定されていないならスルー
                return;
            }

            SetScene(transition);
        }

        #endregion
    }
}
