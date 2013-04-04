using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Shell.Gadgets;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using log4net;
using log4net.Core;
using System.Collections.Specialized;
using EPiServer.Shell.Navigation;
using DeveloperTools.Models;
using DeveloperTools.Core;

namespace DeveloperTools.Controllers
{
    public class LogViewerController : DeveloperToolsController
    {
        private  RollingMemoryAppender _memoryAppender = null;
        private Hierarchy _hierarchy = (Hierarchy)LogManager.GetRepository();

        public LogViewerController()
        {
            InitMessageLogger();
        }

        [HttpGet]
        public ActionResult Index()
        {
            LogSettingAndEvents logSettingAndEvents = new LogSettingAndEvents();
            logSettingAndEvents.IsStarted = _memoryAppender != null ? _memoryAppender.IsStarted : false;
            logSettingAndEvents.LoggingEvents = _memoryAppender == null ? Enumerable.Empty<LoggingEvent>() : _memoryAppender.GetEvents();

            return View("Show", logSettingAndEvents);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult Filter(LoggerSettings loggerSetting)
        {
            LogSettingAndEvents logSettingAndEvents = new LogSettingAndEvents();
            logSettingAndEvents.IsStarted = _memoryAppender != null ? _memoryAppender.IsStarted : false;
            logSettingAndEvents.LoggingEvents = GetFilteredEvents(loggerSetting);
            logSettingAndEvents.LoggerSetting = loggerSetting;
            return View("Show", logSettingAndEvents);
        }

        public ActionResult Start()
        {
            if (_memoryAppender == null)
            {
                CreateDefaultMemoryAppender();
            }
            if (_memoryAppender != null)
            {
                _memoryAppender.IsStarted = true;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Stop()
        {
            if (_memoryAppender != null)
            {
                _memoryAppender.IsStarted = false;
            }
            return RedirectToAction("Index");
        }


        private void InitMessageLogger()
        {
            AppenderCollection appenderCollection;
            appenderCollection = ((Logger)_hierarchy.Root).Appenders;

            foreach (IAppender item in appenderCollection)
            {
                var ma = item as RollingMemoryAppender;
                if (ma != null)
                {
                    _memoryAppender = (RollingMemoryAppender)item;
                    _memoryAppender.ActivateOptions();
                    return;
                }
            }
        }

        private void CreateDefaultMemoryAppender()
        {
            _memoryAppender = new RollingMemoryAppender();
            _memoryAppender.Name = "DeveloperToolsLogViewer";
            _memoryAppender.ActivateOptions();
            log4net.Repository.Hierarchy.Hierarchy repository = LogManager.GetRepository() as Hierarchy;
            repository.Root.AddAppender(_memoryAppender);
            repository.Root.Level = Level.All;
            repository.Configured = true;
            repository.RaiseConfigurationChanged(EventArgs.Empty); 
        }

        private IEnumerable<LoggingEvent> GetFilteredEvents(LoggerSettings loggerSetting)
        {
            if (_memoryAppender == null)
            {
                return Enumerable.Empty<LoggingEvent>();
            }

            IEnumerable<LoggingEvent> res = _memoryAppender.GetEvents().Distinct().Where(l => l.Level >= loggerSetting.Level).OrderBy(l => l.TimeStamp);

            if (res.Count() > 0 && !String.IsNullOrEmpty(loggerSetting.LoggerName))
            {
                res = res.Where<LoggingEvent>(l => l.LoggerName.Contains(loggerSetting.LoggerName));
            }

            if (res.Count() > 0 && !String.IsNullOrEmpty(loggerSetting.ThreadName))
            {
                res = res.Where(l => l.ThreadName == loggerSetting.ThreadName);
            }

            if (res.Count() > 0 && !String.IsNullOrEmpty(loggerSetting.UserName))
            {
                res = res.Where(l => l.UserName == loggerSetting.UserName);
            }

            if (res.Count() > 0)
            {
                res = res.Where(l => ((l.TimeStamp.Ticks >= loggerSetting.StartDate.Ticks) && (l.TimeStamp.Ticks <= loggerSetting.EndDate.Ticks)));
            }

            return res;
        }
    }
}
