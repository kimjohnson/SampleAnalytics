using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleAnalytics {

  public static class Analytics {

    private static DebugEventListener _listener;

    public static ITraceAction TrackTime(string actionName, string category = "Trace") {
      var action = new TimedAction(category, actionName);
      action.Start();
      return action;
    }

    public static ITraceAction TrackUntimed(string actionName, string category = "Trace") {
      var action = new TraceAction(category, actionName);
      action.Start();
      return action;
    }

    public static ILogger Log {
      get { return AnalyticsEventSource.Log; }
    }

    // For debugging
    public static void StartListener() {
      _listener = new DebugEventListener();
    }
  }
}
