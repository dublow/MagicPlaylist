using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicPlaylist.Common.Loggers
{
    public class LoggerCreator
    {
        public static LogEventInfo Info(string loggerName, string message)
        {
            LogEventInfo evt = new LogEventInfo(LogLevel.Info, loggerName, message);
            evt.Properties["message"] = message;

            return evt;
        }
        public static LogEventInfo Info(string loggerName, string message, TimeSpan elapsed)
        {
            LogEventInfo evt = new LogEventInfo(LogLevel.Info, loggerName, message);
            evt.Properties["message"] = message;
            evt.Properties["elapsed"] = elapsed;

            return evt;
        }

        public static LogEventInfo Info(string loggerName, string message, int userId)
        {
            LogEventInfo evt = new LogEventInfo(LogLevel.Info, loggerName, message);
            evt.Properties["message"] = message;
            evt.Properties["userId"] = userId;

            return evt;
        }

        public static LogEventInfo Info(string loggerName, string message, int userId, TimeSpan elapsed)
        {
            LogEventInfo evt = new LogEventInfo(LogLevel.Info, loggerName, message);
            evt.Properties["message"] = message;
            evt.Properties["userId"] = userId;
            evt.Properties["elapsed"] = elapsed;

            return evt;
        }
    }
}
