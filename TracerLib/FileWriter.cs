using System.IO;

namespace TracerLib
{
    public class FileWriter : IWriter
    {
        private string fileName;

        public FileWriter(string fileName)
        {
            this.fileName = fileName;
        }

        public void Write(TraceResult result, IConverter converter)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                converter.Convert(result, fs);
            }
        }
    }
}
