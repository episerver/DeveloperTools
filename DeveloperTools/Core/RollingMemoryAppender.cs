using System.Collections.Concurrent;
using System.Linq;

namespace DeveloperTools.Core
{
    /// <summary>
    ///     Simple appender to avoid filling up memory
    /// </summary>
    //public class RollingMemoryAppender : AppenderSkeleton
    //{
    //    private const int MAX_EVENTS = 10000;
    //    ConcurrentQueue<LoggingEvent> _q = new ConcurrentQueue<LoggingEvent>();

    //    public RollingMemoryAppender()
    //    {
    //        _q = new ConcurrentQueue<LoggingEvent>();
    //    }

    //    public bool IsStarted { get; set; } = true;

    //    public virtual LoggingEvent[] GetEvents()
    //    {
    //        return _q.ToArray();
    //    }

    //    protected override void Append(LoggingEvent loggingEvent)
    //    {
    //        if(!IsStarted)
    //        {
    //            return;
    //        }

    //        loggingEvent.Fix = FixFlags.All;
    //        _q.Enqueue(loggingEvent);

    //        if(_q.Count > MAX_EVENTS)
    //        {
    //            //Just restart with a few events from the previous queue
    //            _q = new ConcurrentQueue<LoggingEvent>(_q.Take(100));
    //        }
    //    }
    //}
}
