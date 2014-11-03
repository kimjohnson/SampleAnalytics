using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;
using System.Text;

namespace SampleAnalytics {

  /// <summary>
  /// PostSharp aspect which generates ETW trace messages on method entry, success and failure (exception).
  /// </summary>
  [PSerializable]
  public class ETWTraceAttribute : OnMethodBoundaryAspect {

    private string _methodName;
    private string _category;
    private bool _addTiming;

    public ETWTraceAttribute(string category = "default", bool addTiming = false) {
      ApplyToStateMachine = true;
      _category = category;
      _addTiming = addTiming;
    }

    public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo) {
      _methodName = method.DeclaringType.FullName + "." + method.Name;
    }

    public override void OnEntry(MethodExecutionArgs args) {
      var action = _addTiming ? Analytics.TrackTime(_methodName, _category) : Analytics.TrackUntimed(_methodName, _category);
      args.MethodExecutionTag = action;
    }

    public override void OnSuccess(MethodExecutionArgs args) {
      var action = args.MethodExecutionTag as ITraceAction;
      action.Stop();
    }

    public override void OnYield(MethodExecutionArgs args) {
      var action = args.MethodExecutionTag as ITraceAction;
      action.Pause();
    }

    public override void OnResume(MethodExecutionArgs args) {
      var action = args.MethodExecutionTag as ITraceAction;
      action.Resume();
    }

    public override void OnException(MethodExecutionArgs args) {
      var action = args.MethodExecutionTag as ITraceAction;
      action.Cancel();

      StringBuilder stringBuilder = new StringBuilder(1024);
      stringBuilder.Append(_methodName);
      stringBuilder.Append('(');

      object instance = args.Instance;
      if (instance != null) {
        stringBuilder.Append("this=");
        stringBuilder.Append(instance);
        stringBuilder.Append("; ");
      }

      if (args.Arguments.Count > 0) {
        stringBuilder.Append(string.Join(", ", args.Arguments.ToArray()));
      }

      stringBuilder.AppendFormat("): Exception ");
      stringBuilder.Append(args.Exception.GetType().Name);
      stringBuilder.Append(": ");
      stringBuilder.Append(args.Exception.Message);

      AnalyticsEventSource.Log.TraceException(_methodName, stringBuilder.ToString());
    }
 
  }
}
