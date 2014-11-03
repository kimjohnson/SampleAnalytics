using Microsoft.Diagnostics.Tracing;
using System;
using System.Diagnostics;
using System.Linq;

namespace SampleAnalytics {

  /// <summary>
  /// EventListener for debugging - writes to Trace.
  /// </summary>
  public class DebugEventListener : EventListener {

    protected override void OnEventSourceCreated(EventSource eventSource) {
      EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All);
      Trace.TraceInformation("Listening on " + eventSource.Name);
    }

    protected override void OnEventWritten(EventWrittenEventArgs eventData) {
      string msg1 = string.Format("Event {0} from {1} level={2} opcode={3} at {4:HH:mm:ss.fff}",
        eventData.EventId, eventData.EventSource.Name, eventData.Level, eventData.Opcode, DateTime.Now);

      string msg2 = null;
      if (eventData.Message != null) {
        msg2 = string.Format(eventData.Message, eventData.Payload.ToArray());
      } else {
        string[] sargs = eventData.Payload != null ? eventData.Payload.Select(o => o.ToString()).ToArray() : null;
        msg2 = string.Format("({0}).", sargs != null ? string.Join(", ", sargs) : "");
      }

      if (eventData.Level == EventLevel.Error || eventData.Level == EventLevel.Critical) {
        Trace.TraceError("{0}\n{1}", msg1, msg2);
      } else {
        Trace.TraceInformation("{0}\n{1}", msg1, msg2);
      }
    }

  }
}
