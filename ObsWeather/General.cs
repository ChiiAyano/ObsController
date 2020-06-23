using System.IO;

namespace ObsWeather
{
    public static class General
    {
        public static string StartupPath => ObsController.Core.General.StartupPath;
        public static string SettingPath => ObsController.Core.General.SettingPath;
        public static string WeatherIconsPath => Path.Combine(StartupPath, "weathericons.xml");
    }
}
