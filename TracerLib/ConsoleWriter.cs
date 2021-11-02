using System;
using System.IO;

namespace TracerLib
{
    public class ConsoleWriter : IWriter
    {
        public void Write(TraceResult traceResult, IConverter converter)
        {
            using (Stream consoleStream = Console.OpenStandardOutput())
            {
                converter.Convert(traceResult, consoleStream);
            }
        }
    }
}
