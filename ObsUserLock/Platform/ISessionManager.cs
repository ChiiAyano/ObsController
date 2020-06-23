using System;
using System.Collections.Generic;
using System.Text;

namespace ObsUserLock.Platform
{
    public interface ISessionManager
    {
        /// <summary>
        /// セッション状態が変化したとき
        /// </summary>
        event EventHandler<SessionChangedEventArgs> SessionChanged;
        /// <summary>
        /// セッション状態の監視を開始
        /// </summary>
        void Listen();
        /// <summary>
        /// セッション状態の監視を終了
        /// </summary>
        void Stop();
    }
}
