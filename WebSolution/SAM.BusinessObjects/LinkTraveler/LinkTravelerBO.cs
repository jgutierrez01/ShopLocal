using SAM.BusinessObjects.Sql;
using SAM.Entities.LinkTraveler;
using System;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAM.BusinessObjects.Utilities;

namespace SAM.BusinessObjects.LinkTraveler
{
    public class LinkTravelerBO
    {
        private static readonly object _mutex = new object();
        private static LinkTravelerBO _instance;
        private FileStream DocumentoActual = null;
        public PdfCopy pdfCopy { get; set; }
        public Document document = new Document();
        public bool AgregoPaginas { get; set; }
        public static int CantidadTravelers = 0;
        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private LinkTravelerBO() { }
        public static LinkTravelerBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new LinkTravelerBO();
                    }
                }
                return _instance;
            }
        }

        public bool ExisteNumeroOrdenPath(string pathBusqueda, string numeroOrden)
        {
            string buscarArchivoEnPath = System.IO.Path.Combine(pathBusqueda, "ODT " + numeroOrden + ".pdf");
            string usuario = ConfigurationManager.AppSettings["usuario"];
            string pass = ConfigurationManager.AppSettings["pass"];
            bool existeArchivo = false;
            using (new NetworkConnection(pathBusqueda, new NetworkCredential(usuario, pass)))
            {
                if (File.Exists(buscarArchivoEnPath))
                {
                    existeArchivo = true;
                }
                else
                {
                    existeArchivo = false;
                }
            }
            return existeArchivo;
        }

        public DataTable ObtieneSpools(string NumeroControl, int proyectoID)
        {
            ObjetosSQL _SQL = new ObjetosSQL();
            string[,] parametro = { { "@NumeroControl", NumeroControl }, { "@ProyectoID", proyectoID.ToString() } };

            return _SQL.EjecutaDataAdapter(Stords.LinkTravelerObtenerSpool, parametro);
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



        public ProyectoModel GetInfoProyecto(int ProyectoID)
        {            
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = " SELECT " +
                                    " A.ProyectoID, " +
                                    " Nombre NombreProyecto, " +
                                    " B.pathCarpetaCompartida " +
                                " FROM " +
                                    " Proyecto A WITH(NOLOCK) " +
                                    " INNER JOIN ProyectoConfiguracion B WITH(NOLOCK) ON A.ProyectoID = B.ProyectoID " +
                                " WHERE " +
                                    " A.ProyectoID = " + ProyectoID + " AND A.Activo = 1 AND A.ActivoReporte = 1 ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    ProyectoModel Proyecto = new ProyectoModel();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Proyecto = new ProyectoModel
                        {
                            ProyectoID = int.Parse(ds.Tables[0].Rows[0]["ProyectoID"].ToString()),
                            Nombre = ds.Tables[0].Rows[0]["NombreProyecto"].ToString(),
                            pathPDF = ds.Tables[0].Rows[0]["pathCarpetaCompartida"].ToString()
                        };
                    }                                                               
                    return Proyecto;
                }
            }
        }

        public int CantidadDePaginas(string pdfFilePath)
        {
            using (PdfReader reader = new PdfReader(pdfFilePath))
            {
                return reader.NumberOfPages;
            }
        }          
        
        public string ValidarGranel(string NumeroControl, int ProyectoID)
        {
            string campo7 = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = " SELECT " +
                                    " ISNULL(S.Campo7, '') Campo7 " +
                                " FROM " +
                                    " OrdenTrabajoSpool OT WITH(NOLOCK) " +
                                    " INNER JOIN Spool S WITH(NOLOCK) ON OT.SpoolID = S.SpoolID " +
                                " WHERE " +
                                    " OT.NumeroControl = '" + NumeroControl + "' AND S.ProyectoID = " + ProyectoID;

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        campo7 = ds.Tables[0].Rows[0]["Campo7"].ToString();                       
                    }
                }
            }
            return campo7;
        }  
    }
}
