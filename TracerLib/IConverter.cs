using System.IO;

namespace TracerLib
{
    public interface IConverter
    {
        void Convert(TraceResult traceResult, Stream stream);
    }
}
