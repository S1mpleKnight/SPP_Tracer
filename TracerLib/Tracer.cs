using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace TracerLib
{
    public class Tracer : ITracer
    {
        private TraceResult traceResult;

        public Tracer()
        {
            traceResult = new TraceResult();
        }

        public void StartTrace()
        {
            MethodBase currentMethod = new StackTrace().GetFrame(1).GetMethod();
            traceResult.StartTrace(Thread.CurrentThread.ManagedThreadId, currentMethod);
        }

        public void StopTrace()
        {
            traceResult.StopTrace(Thread.CurrentThread.ManagedThreadId);
        }

        public TraceResult GetTraceResult()
        {
            return traceResult;
        }
    }
}
