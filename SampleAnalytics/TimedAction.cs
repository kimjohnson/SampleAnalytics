using System;
using System.Diagnostics;

namespace SampleAnalytics {

  /// <summary>
  /// Wraps a timed method or block of code.
  /// </summary>
  public class TimedAction : ITraceAction, IDisposable {

    private readonly string _category;
    private readonly string _actionName;
    private Stopwatch _stopwatch;

    internal TimedAction(string category, string actionName) {
      _category = category;
      _actionName = actionName;
    }

    public void Start() {
      if (_stopwatch != null) return;
      AnalyticsEventSource.Log.TraceActionTimedStart(_category, _actionName);
      _stopwatch = Stopwatch.StartNew();
    }

    public void Stop() {
      if (_stopwatch == null || !_stopwatch.IsRunning) return;
      _stopwatch.Stop();
      AnalyticsEventSource.Log.TraceActionTimedStop(_category, _actionName, _stopwatch.ElapsedMilliseconds);
    }

    public void Cancel() {
      _stopwatch.Reset();
    }

    public void Pause() {
      _stopwatch.Stop();
    }

    public void Resume() {
      _stopwatch.Start();
    }

    public void Dispose() {
      Stop();
    }
  }

}
