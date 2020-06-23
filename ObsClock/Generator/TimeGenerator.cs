using System;
using ObsClock.Models;

namespace ObsClock.Generator
{
    public static class TimeGenerator
    {
        public static string Generate(DateTimeOffset date, string hoursFormat, NoonAppearance appearance, char padChar)
        {
            if (hoursFormat.Contains("H"))
            {
                return PadLeft(date.ToString($"{hoursFormat}:mm"));
            }

            if (appearance == NoonAppearance.Twelve)
            {
                return PadLeft(date.ToString($"{hoursFormat}:mm"));
            }
            else if (appearance == NoonAppearance.Zero)
            {
                var hours = (date.Hour >= 12 ? date.Hour - 12 : date.Hour).ToString().PadLeft(hoursFormat.Length, '0');
                return PadLeft(hours + date.ToString(":mm"));
            }
            else
            {
                return PadLeft(date.ToString($"{hoursFormat}:mm"));
            }

            string PadLeft(string value)
            {
                var totalWidth = 5;

                if (padChar == '\0')
                {
                    totalWidth -= 1;
                }

                return value.PadLeft(totalWidth, padChar);
            }
        }
    }
}
