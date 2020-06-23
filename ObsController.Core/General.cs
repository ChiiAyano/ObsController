using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ObsController.Core
{
    public static class General
    {
        public static string StartupPath { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        public static string SettingPath => Path.Combine(StartupPath, "settings.json");
    }
}
