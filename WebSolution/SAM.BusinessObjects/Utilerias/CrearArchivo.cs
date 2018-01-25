using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessObjects.Utilerias
{
    public class CrearArchivo
    {
        private FileStream DocumentoActual = null;
        private static CrearArchivo _Instance;
        public static CrearArchivo Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CrearArchivo();
                }
                return _Instance;
            }
            set
            {
                _Instance = null;
            }
        }

        public bool crearArchivo(string path)
        {
            try
            {
                DocumentoActual = System.IO.File.Create(path);
                return true;
            }
            catch (Exception ex)
            {
                return false;

                throw;
            }
        }

        public bool EscribirMensajeDocumento(string ordenTrabajo, string numeroControl, string comentario)
        {
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    sw.WriteLine(ordenTrabajo + "," + numeroControl + "," + comentario);
                    byte[] info = new UTF8Encoding(true).GetBytes(sw.ToString());
                    this.DocumentoActual.Write(info, 0, info.Length);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CerrarDocumento()
        {
            try
            {
                this.DocumentoActual.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
