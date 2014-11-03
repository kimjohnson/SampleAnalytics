
namespace SampleAnalytics {

  public interface ILogger {
    void TraceInformation(string message, string category = null, string subcategory = null);
    void TraceWarning(string message, string category = null, string subcategory = null);
    void TraceError(string message, string category = null, string subcategory = null);
  }
}
