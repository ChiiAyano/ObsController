using System;
using System.Collections.Generic;
using System.Text;

namespace ObsUserLock.Platform
{
    public class SessionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 現在のセッション状態
        /// </summary>
        public SessionState State { get; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="state"></param>
        public SessionChangedEventArgs(SessionState state)
        {
            this.State = state;
        }
    }
}
