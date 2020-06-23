using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace ObsUserLock.Platform
{
    public class SessionManager : ISessionManager
    {
        public event EventHandler<SessionChangedEventArgs> SessionChanged;

        /// <inheritdoc/>
        public void Listen()
        {
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        /// <inheritdoc/>
        public void Stop()
        {
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
        }

        private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            var state = e.Reason switch
            {
                SessionSwitchReason.SessionLock => SessionState.SessionLock,
                SessionSwitchReason.SessionUnlock => SessionState.SessionUnlock,
                _ => SessionState.SessionUnlock
            };

            this.SessionChanged?.Invoke(this, new SessionChangedEventArgs(state));
        }
    }
}
