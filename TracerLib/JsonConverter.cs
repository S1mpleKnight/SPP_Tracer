using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;

namespace TracerLib
{
    public class JsonConverter : IConverter
    {
        private DataContractJsonSerializer jsonFormatter;

        public JsonConverter()
        {
            jsonFormatter = new DataContractJsonSerializer(typeof(TraceResult));
        }

        public void Convert(TraceResult traceResult, Stream stream)
        {
            using (XmlDictionaryWriter jsonWriter = JsonReaderWriterFactory.CreateJsonWriter(stream, Encoding.UTF8, ownsStream: true, indent: true, indentChars: "     "))
            {
                jsonFormatter.WriteObject(jsonWriter, traceResult);
            }

        }
    }
}
