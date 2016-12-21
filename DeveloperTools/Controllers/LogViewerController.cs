using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DeveloperTools.Core;
using DeveloperTools.Models;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace DeveloperTools.Controllers
{
    public class LogViewerController : DeveloperToolsController
    {
        private readonly Hierarchy _hierarchy = (Hierarchy) LogManager.GetRepository();
        private RollingMemoryAppender _memoryAppender;

        public LogViewerController()
        {
            InitMessageLogger();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var logSettingAndEvents = new LogSettingAndEvents
            {
                IsStarted = _memoryAppender?.IsStarted ?? false,
                LoggingEvents = _memoryAppender == null ? Enumerable.Empty<LoggingEvent>() : _memoryAppender.GetEvents()
            };

            return View("Show", logSettingAndEvents);
        }

        [HttpPost, ActionName("Index")]
        public ActionResult Filter(LoggerSettings loggerSetting)
        {
            var logSettingAndEvents = new LogSettingAndEvents
            {
                IsStarted = _memoryAppender?.IsStarted ?? false,
                LoggingEvents = GetFilteredEvents(loggerSetting),
                LoggerSetting = loggerSetting
            };

            return View("Show", logSettingAndEvents);
        }

        public ActionResult Start()
        {
            if(_memoryAppender == null)
                CreateDefaultMemoryAppender();

            if(_memoryAppender != null)
                _memoryAppender.IsStarted = true;

            return RedirectToAction("Index");
        }

        public ActionResult Stop()
        {
            if(_memoryAppender != null)
                _memoryAppender.IsStarted = false;

            return RedirectToAction("Index");
        }

        private void InitMessageLogger()
        {
            var appenderCollection = _hierarchy.Root.Appenders;

            foreach (var item in appenderCollection)
            {
                var ma = item as RollingMemoryAppender;
                if(ma != null)
                {
                    _memoryAppender = (RollingMemoryAppender) item;
                    _memoryAppender.ActivateOptions();
                    return;
                }
            }
        }

        private void CreateDefaultMemoryAppender()
        {
            _memoryAppender = new RollingMemoryAppender { Name = "DeveloperToolsLogViewer" };
            _memoryAppender.ActivateOptions();
            var repository = LogManager.GetRepository() as Hierarchy;

            if(repository != null)
            {
                repository.Root.AddAppender(_memoryAppender);
                repository.Root.Level = Level.All;
                repository.Configured = true;
                repository.RaiseConfigurationChanged(EventArgs.Empty);
            }
        }

        private IEnumerable<LoggingEvent> GetFilteredEvents(LoggerSettings loggerSetting)
        {
            if(_memoryAppender == null)
            {
                return Enumerable.Empty<LoggingEvent>();
            }

            IEnumerable<LoggingEvent> res = _memoryAppender.GetEvents().Distinct().Where(l => l.Level >= loggerSetting.Level).OrderBy(l => l.TimeStamp);

            if(res.Any() && !string.IsNullOrEmpty(loggerSetting.LoggerName))
            {
                res = res.Where(l => l.LoggerName.Contains(loggerSetting.LoggerName));
            }

            if(res.Any() && !string.IsNullOrEmpty(loggerSetting.ThreadName))
            {
                res = res.Where(l => l.ThreadName == loggerSetting.ThreadName);
            }

            if(res.Any() && !string.IsNullOrEmpty(loggerSetting.UserName))
            {
                res = res.Where(l => l.UserName == loggerSetting.UserName);
            }

            if(res.Any())
            {
                res = res.Where(l => ((l.TimeStamp.Ticks >= loggerSetting.StartDate.Ticks) && (l.TimeStamp.Ticks <= loggerSetting.EndDate.Ticks)));
            }

            return res;
        }
    }
}
