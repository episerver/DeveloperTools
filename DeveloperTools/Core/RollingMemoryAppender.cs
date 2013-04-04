using log4net.Appender;
using log4net.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeveloperTools.Core
{
    /// <summary>
    /// Simple appender to avoid filling up memory 
    /// </summary>
    public class RollingMemoryAppender : AppenderSkeleton
    {
        ConcurrentQueue<LoggingEvent> _q = new ConcurrentQueue<LoggingEvent>();
        private bool _isStarted = true;

        private const int MAX_EVENTS = 10000;

        public RollingMemoryAppender()
            : base()
        {
            _q = new ConcurrentQueue<LoggingEvent>();
        }

        virtual public LoggingEvent[] GetEvents()
        {
            return _q.ToArray();
        }

        public bool IsStarted
        {
            get
            {
                return _isStarted;
            }
            set
            {
                _isStarted = value;
            }
        }

        override protected void Append(LoggingEvent loggingEvent)
        {
            if (!_isStarted)
            {
                return;
            }

            loggingEvent.Fix = FixFlags.All;
            _q.Enqueue(loggingEvent);

            if (_q.Count > MAX_EVENTS)
            {
                //Just restart with a few events from the previous queue
                _q = new ConcurrentQueue<LoggingEvent>(_q.Take(100));

            }
        }
    }
}