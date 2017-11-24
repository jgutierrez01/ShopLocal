using System.IO;
using Mimo.Framework.Common;

namespace SAM.BusinessLogic.Ingenieria
{
    /// <summary>
    /// Wrapper para un stream y el nombre del archivo subido para ingenieria
    /// </summary>
    public class ArchivoSimple
    {
        public string[] Lineas;

        public string Nombre { get; set; }

        private Stream _stream;
        public Stream Stream
        {
            get
            {
                return _stream;
            }
            set
            {
                _stream = value;
                Lineas = IOUtils.GetLinesFromTextStream(_stream);
            }
        }
    }
}