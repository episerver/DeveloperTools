namespace EPiServer.DeveloperTools.Features.LogViewer
{
    public class LogSettingAndEvents
    {
        public LogSettingAndEvents()
        {
            //LoggingEvents = Enumerable.Empty<LoggingEvent>();
            LoggerSetting = new LoggerSettings();
        }

        public bool IsStarted { get; set; }
        //public IEnumerable<LoggingEvent> LoggingEvents { get; set; }
        public LoggerSettings LoggerSetting { get; set; }
    }
}
