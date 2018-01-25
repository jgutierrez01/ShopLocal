using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessObjects.Utilerias
{

    public class SplitPDF
    {
        public PdfCopy pdfCopy { get; set; }
        public Document document = new Document();
        private static SplitPDF _Instance;
        public static int CantidadTravelers = 0;
        public bool AgregoPaginas { get; set; }
        public static SplitPDF Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SplitPDF();
                }
                return _Instance;
            }
            set
            {
                _Instance = value;
            }

        }

        public bool CrearObtenerPDF(string pathDestino, string nombreNuevoArchivoPDF, out string mensaje)
        {
            try
            {
                string directoryName = Path.GetRandomFileName();
                Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), directoryName));
                pathDestino = directoryName;
                nombreNuevoArchivoPDF = "PDF_" + nombreNuevoArchivoPDF;
                int contador = 0;
                string ruta = pathDestino + "\\" + nombreNuevoArchivoPDF + ".pdf";
                string rutaArchivo = pathDestino + "\\" + nombreNuevoArchivoPDF + ".csv";
                while (File.Exists(ruta))
                {
                    ruta = pathDestino + "\\" + nombreNuevoArchivoPDF + " " + (contador + 1) + ".pdf";
                    rutaArchivo = pathDestino + "\\" + nombreNuevoArchivoPDF + " " + (contador + 1) + ".csv";
                    contador++;
                }
                this.pdfCopy = new PdfCopy(document, new FileStream(pathDestino + "\\" + (nombreNuevoArchivoPDF + " " + (contador == 0 ? "" : contador.ToString())).Trim() + ".pdf", FileMode.Create));
                this.document.Open();
                //creo el archivo csv
                //CrearArchivo.Instance.EscribirMensajeDocumento("","","Se creo el archivo " + nombreNuevoArchivoPDF + " " + contador + DateTime.Now.ToString());
                mensaje = (nombreNuevoArchivoPDF + " " + (contador == 0 ? "" : contador.ToString())).Trim();
                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }
        }

        public bool CrearObtenerCSV(string pathDestino, string nombreNuevoArchivoPDF, out string mensaje)
        {
            try
            {
                nombreNuevoArchivoPDF = "PDF_" + nombreNuevoArchivoPDF;
                int contador = 0;

                string rutaArchivo = pathDestino + "\\" + nombreNuevoArchivoPDF + ".csv";
                while (File.Exists(rutaArchivo))
                {
                    rutaArchivo = pathDestino + "\\" + nombreNuevoArchivoPDF + " " + (contador + 1) + ".csv";
                    contador++;
                }
                //creo el archivo csv
                CrearArchivo.Instance.crearArchivo(rutaArchivo);
                CrearArchivo.Instance.EscribirMensajeDocumento("OrdenTrabajo", "Numero Control", "Comentario");
                mensaje = (nombreNuevoArchivoPDF + " " + (contador == 0 ? "" : contador.ToString())).Trim();
                return true;
            }
            catch (Exception ex)
            {
                mensaje = ex.Message;
                return false;
            }

        }




        public void SplitAndSaveInterval(string pdfFilePath, string outputPath, int startPage, int interval, string pdfFileName, string ordentrabajo, string numeroControl)
        {
            using (PdfReader reader = new PdfReader(pdfFilePath))
            {
                Document document = new Document();
                try
                {
                    if (reader.NumberOfPages >= startPage)
                    {
                        pdfCopy.AddPage(pdfCopy.GetImportedPage(reader, startPage));
                        this.AgregoPaginas = true;
                        CantidadTravelers++;
                        //CrearArchivo.Instance.EscribirMensajeDocumento(ordentrabajo, numeroControl , "se agrego al archivo correctamente");
                    }
                    else
                    {
                        //CrearArchivo.Instance.EscribirMensajeDocumento(ordentrabajo, numeroControl, "la pagina" + startPage + " no se encuentra en el documento " + pdfFilePath + " , el documento cuenta con " + reader.NumberOfPages + "paginas");
                    }
                }
                catch (Exception ex)
                {
                    //CrearArchivo.Instance.EscribirMensajeDocumento(ordentrabajo, numeroControl, ex.Message);
                }

            }
        }

        public void CerrarDocumentoCreado(string fileName)
        {
            try
            {
                this.document.Close();
                this.pdfCopy.Close();

            }
            catch (Exception ex)
            {


            }

        }

        public int CantidadDePaginas(string pdfFilePath)
        {
            using (PdfReader reader = new PdfReader(pdfFilePath))
            {
                return reader.NumberOfPages;
            }
        }


    }
}
