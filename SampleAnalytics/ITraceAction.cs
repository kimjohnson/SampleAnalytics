using System;

namespace SampleAnalytics {
  
  public interface ITraceAction : IDisposable {
    void Start();
    void Stop();

    // To cancel a timed action
    void Cancel();    

    // For async actions
    void Pause();
    void Resume();
  }
}
