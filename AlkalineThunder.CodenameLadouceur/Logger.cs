using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace AlkalineThunder.CodenameLadouceur
{
    public static class Logger
    {
        public static void Log(string message, LogLevel level = LogLevel.Info, [CallerMemberName] string member = "", [CallerFilePath] string file = "", [CallerLineNumber] int ln = 0)
        {
            switch(level)
            {
                case LogLevel.Debug:
#if DEBUG
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
#else
                    return;
#endif
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Message:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            string fname = Path.GetFileName(file);

            Console.WriteLine("[{0}] [{1}:{2}] <{3}/{4}> {5}", DateTime.Now.ToShortTimeString(), fname, ln, member, level.ToString().ToLower(), message);
        }
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Message,
        Error,
        Debug
    }
}
