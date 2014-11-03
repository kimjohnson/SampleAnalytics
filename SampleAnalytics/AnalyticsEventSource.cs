using Microsoft.Diagnostics.Tracing;
using System;
using System.Runtime.CompilerServices;

namespace SampleAnalytics {

  [EventSource(Name = "Samples-Analytics")]
  public sealed class AnalyticsEventSource : EventSource, ILogger {

    public static readonly AnalyticsEventSource Log = new AnalyticsEventSource();

    public class Tasks {
      public const EventTask TimedAction = (EventTask)0x1;
      public const EventTask Action = (EventTask)0x2;
    }

    private const int TraceActionTimedStartEventId = 1;
    private const int TraceActionTimedStopEventId = 2;
    private const int TraceActionStartEventId = 3;
    private const int TraceActionStopEventId = 4;
    private const int TraceInformationEventId = 5;
    private const int TraceWarningEventId = 6;
    private const int TraceErrorEventId = 7;
    private const int TraceExceptionEventId = 99;

    [Event(TraceActionTimedStartEventId, Level = EventLevel.Verbose, Task = Tasks.TimedAction, Opcode = EventOpcode.Start)]
    public void TraceActionTimedStart(string category, string actionName) {
      WriteEvent(TraceActionTimedStartEventId, category, actionName);
    }

    [Event(TraceActionTimedStopEventId, Message = "Category '{0}' - Action '{1}' took {2} ms", Level = EventLevel.Verbose, Task = Tasks.TimedAction, Opcode = EventOpcode.Stop)]
    public void TraceActionTimedStop(string category, string actionName, long elapsedMilliseconds) {
      WriteEvent(TraceActionTimedStopEventId, category, actionName, elapsedMilliseconds);
    }

    [Event(TraceActionStartEventId, Level = EventLevel.Verbose, Task = Tasks.Action, Opcode = EventOpcode.Start)]
    public void TraceActionStart(string category, string actionName) {
      WriteEvent(TraceActionStartEventId, category, actionName);
    }

    [Event(TraceActionStopEventId, Level = EventLevel.Verbose, Task = Tasks.Action, Opcode = EventOpcode.Stop)]
    public void TraceActionStop(string category, string actionName) {
      WriteEvent(TraceActionStopEventId, category, actionName);
    }

    [Event(TraceInformationEventId, Level = EventLevel.Informational)]
    public void TraceInformation(string message, string category = "", string subcategory = "") {
      WriteEvent(TraceInformationEventId, message, category, subcategory);
    }

    [Event(TraceWarningEventId, Level = EventLevel.Warning)]
    public void TraceWarning(string message, string category = "", string subcategory = "") {
      WriteEvent(TraceWarningEventId, message, category, subcategory);
    }

    [Event(TraceErrorEventId, Level = EventLevel.Error)]
    public void TraceError(string message, string category = "", string subcategory = "") {
      WriteEvent(TraceErrorEventId, message, category, subcategory);
    }

    [Event(TraceExceptionEventId, Message = "Exception in '{0}': {1}", Level = EventLevel.Error)]
    public void TraceException(string methodName, string message) {
      WriteEvent(TraceExceptionEventId, methodName, message);
    }


    /// <summary>
    /// Provides a strongly-typed WriteEvent overload instead of defaulting to params object[] in the TraceActionTimeStop event.
    /// </summary>
    [NonEvent]
    private unsafe void WriteEvent(int eventId, string arg1, string arg2, long arg3) {
      if (!IsEnabled()) return;
      if (arg1 == null) arg1 = "";
      if (arg2 == null) arg2 = "";

      fixed (char* str = arg1) {
        char* chPtr = str;
        fixed (char* str2 = arg2) {
          char* chPtr2 = str2;

          EventData* data = stackalloc EventSource.EventData[3];
          data[0].DataPointer = (IntPtr)chPtr;
          data[0].Size = (arg1.Length + 1) * 2;
          data[1].DataPointer = (IntPtr)chPtr2;
          data[1].Size = (arg2.Length + 1) * 2;
          data[2].DataPointer = (IntPtr)(&arg3);
          data[2].Size = 8;
          this.WriteEventCore(eventId, 3, data);
        }
      }
    }

  }
}
