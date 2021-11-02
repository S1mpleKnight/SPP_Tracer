namespace TracerLib
{
    public interface IWriter
    {
        void Write(TraceResult traceResult, IConverter converter);
    }
}
