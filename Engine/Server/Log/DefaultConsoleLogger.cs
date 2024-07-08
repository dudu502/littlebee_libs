using Engine.Common.Log;
using System;

namespace Engine.Server.Log
{
    public class DefaultConsoleLogger : ILogger
    {
        private string _tag;
        private readonly string _TimeFormatPatten = "yyyy/MM/dd HH:mm:ss.fff";

        public bool IsDebugEnabled { get; set; } = true;

        public DefaultConsoleLogger(string tag)
        {
            _tag = tag;
        }
        public void Error(string message)
        {
            if (IsDebugEnabled)
                Console.WriteLine($"[{DateTime.Now.ToString(_TimeFormatPatten)}]E[{_tag}]\t{message}");
        }

        public void Info(string message)
        {
            if (IsDebugEnabled)
                Console.WriteLine($"[{DateTime.Now.ToString(_TimeFormatPatten)}]I[{_tag}]\t{message}");
        }

        public void Warn(string message)
        {
            if (IsDebugEnabled)
                Console.WriteLine($"[{DateTime.Now.ToString(_TimeFormatPatten)}]W[{_tag}]\t{message}");
        }

        public void Info(object message)
        {
            Info(message.ToString());
        }

        public void Warn(object message)
        {
            Warn(message.ToString());
        }

        public void Error(object message)
        {
            Error(message.ToString());
        }
    }
}
