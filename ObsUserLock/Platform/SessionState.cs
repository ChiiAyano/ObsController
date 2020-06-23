using System;
using System.Collections.Generic;
using System.Text;

namespace ObsUserLock.Platform
{
    /// <summary>
    /// ユーザーセッション情報
    /// </summary>
    public enum SessionState
    {
        /// <summary>
        /// ロック状態
        /// </summary>
        SessionLock,
        /// <summary>
        /// アンロック状態
        /// </summary>
        /// <remarks>ユーザーが自由に画面を触っている状態</remarks>
        SessionUnlock
    }
}
