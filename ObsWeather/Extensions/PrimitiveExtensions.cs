namespace ObsWeather.Extensions
{
    public static class PrimitiveExtensions
    {
        /// <summary>
        /// ケルビンからセルシウス度に変換
        /// </summary>
        /// <param name="kelvin"></param>
        /// <returns></returns>
        public static double ConvertToCelsius(this double kelvin)
        {
            return kelvin - 273.15d;
        }
    }
}
