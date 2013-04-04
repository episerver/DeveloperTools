using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeveloperTools.Models
{
    public class LogSettingAndEvents
    {
        public LogSettingAndEvents()
        {
            LoggingEvents = Enumerable.Empty<LoggingEvent>();
            LoggerSetting = new LoggerSettings();

        }
        public bool IsStarted { get; set; }
        public IEnumerable<LoggingEvent> LoggingEvents { get; set; }
        public LoggerSettings LoggerSetting { get; set; }
    }

}