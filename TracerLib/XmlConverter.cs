using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace TracerLib
{
    public class XMLConverter : IConverter
    {
        private DataContractSerializer xmlConverter;
        private XmlWriterSettings xmlWriterSettings;

        public XMLConverter()
        {
            xmlConverter = new DataContractSerializer(typeof(TraceResult));
            xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "     "
            };
        }

        public void Convert(TraceResult traceResult, Stream stream)
        {
            using (XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
            {
                xmlConverter.WriteObject(xmlWriter, traceResult);
            }
        }

    }
}
