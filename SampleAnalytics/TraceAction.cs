
namespace SampleAnalytics {

  /// <summary>
  /// Wraps an untimed method or block of code.
  /// </summary>
  public class TraceAction : ITraceAction {

    private readonly string _category;
    private readonly string _actionName;

    internal TraceAction(string category, string actionName) {
      _category = category;
      _actionName = actionName;
    }

    public void Start() {
      AnalyticsEventSource.Log.TraceActionStart(_category, _actionName);
    }

    public void Stop() {
      AnalyticsEventSource.Log.TraceActionStop(_category, _actionName);
    }

    public void Cancel() {
    }

    public void Pause() {
    }

    public void Resume() {
    }

    public void Dispose() {
      Stop();
    }
  }
}
