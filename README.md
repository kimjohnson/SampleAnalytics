SampleAnalytics
===============

ETW tracing for methods and code blocks.  Generates start and stop ETW events
to trace code entry and exit.



Usage
-----

For code blocks:

```
   using (var action = Analytics.TrackTime("Save Student")) {
      db.SaveChanges();
   }
```


For method-level interception:

   Decorate a method, class or assembly with the ETWTraceAttribute.

```
     [ETWTrace(category: "Instructor")]
     public ActionResult Index(int? id, int? courseID) {..}
```

```
     [ETWTrace(category: "Course", addTiming: true)]
     public class CourseController : Controller {..}
```



See http://aroundtuitblog.wordpress.com/2014/11/03/a-look-at-etw-part-3/ for more information.