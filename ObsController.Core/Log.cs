using System;
using System.Collections.Generic;
using System.Text;

namespace ObsController.Core
{
    public static class Log
    {
        public static void WriteLogLine(string message, LogType type = LogType.Info)
        {
            WriteLog(message + "\r\n", type);
        }

        public static void WriteLog(string message, LogType type = LogType.Info)
        {
            var color = GetColor();

            if (color.HasValue)
            {
                Console.ForegroundColor = color.Value;
            }

            Console.Write($"[{DateTimeOffset.Now:HH:mm:ss}] {message}");
            Console.ResetColor();

            ConsoleColor? GetColor()
            {
                return type switch
                {
                    LogType.Info => null,
                    LogType.Warning => ConsoleColor.Yellow,
                    LogType.Error => ConsoleColor.Red,
                    _ => null
                };
            }
        }

        public enum LogType
        {
            Info,
            Warning,
            Error
        }
    }
}
