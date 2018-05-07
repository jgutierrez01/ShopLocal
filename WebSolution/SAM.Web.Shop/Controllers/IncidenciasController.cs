using Newtonsoft.Json;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Busqueda;
using SAM.Entities.Personalizadas.Shop;
using SAM.Web.Common;
using SAM.Web.Shop.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SAM.Web.Shop.Controllers
{
    public class IncidenciasController : AuthenticatedController
    {
        public IncidenciasController(INavigationContext navContext) : base(navContext) { }
        // GET: Incidencias
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ObtenerSpools(int ProyectoID, int CuadranteID)
        {
            List<ListaIncidencia> ListaSpools = OrdenTrabajoSpoolBO.Instance.ObtenerSpoolsNoResueltos(ProyectoID, CuadranteID);
            string resultado = "";
            if (ListaSpools != null && ListaSpools.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(ListaSpools);
            }
            else
            {
                resultado = "NODATA";
            }
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ValidateInput(false)]
        public JsonResult GuardarIncidencia(int SpoolID, int TipoIncidenciaID, int MaterialSpoolID, int JuntaSpoolID, int ErrorIncidenciaID, string Observacion, string SI)
        {
            string Usuario = SessionFacade.NombreCompleto;
            /*
             * Tipo de Usuario = 1 -----> Inspector
             * Tipo de Usuario = 2 -----> Cliente
             */
            string respuesta = OrdenTrabajoSpoolBO.Instance.GuardarIncidencia(SpoolID, TipoIncidenciaID, MaterialSpoolID, JuntaSpoolID, ErrorIncidenciaID, Observacion, Usuario, SI, 1);
            var myData = new[] { new { result = respuesta } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerSpoolAgregarSI(int SpoolID)
        {
            List<AgregarSI> Spool = OrdenTrabajoSpoolBO.Instance.ObtenerSpoolAgregarSI(SpoolID);
            int spoolID = 0;
            foreach(var item in Spool)
            {
                spoolID = item.SpoolID;
            }           
            //Inserto Spool A Cache
            List<AgregarSI> SpoolsResueltos = JsonConvert.DeserializeObject<List<AgregarSI>>(NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos"));
            AgregarSI SpoolBuscar = new AgregarSI();
            if (SpoolsResueltos != null)
            {
                //Se busca el nuevo spool si existe ya no lo agrega si no se inserta              
                if(spoolID != 0)
                {
                    SpoolBuscar = SpoolsResueltos.Where(x => x.SpoolID == spoolID).FirstOrDefault();
                    if (SpoolBuscar == null)
                    {
                        SpoolsResueltos.InsertRange(0, Spool);
                    }
                }             
            }
            else
            {
                //Se agrega a la lista de spools resueltos
                SpoolsResueltos = new List<AgregarSI>();
                SpoolsResueltos.InsertRange(0, Spool);
            }
            
            //Actualizo la cache con los spools resueltos
            NavContext.SetDataToSession<string>(Session, "ListaSpoolsResueltos", JsonConvert.SerializeObject(SpoolsResueltos));
            string resultado = "";
            if (Spool != null && Spool.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(Spool);
            }
            else
            {
                resultado = "NODATA";
            }                  
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerSpoolsResueltos()
        {
            string spoolsResueltos = NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos");
            if(spoolsResueltos == null)
            {
                spoolsResueltos = "NODATA";
            }
            var myData = new[] { new { result = spoolsResueltos } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult EliminarSpoolDeSession(int SpoolID)
        {
            string resultado = "";            
            List<AgregarSI> SpoolsResueltos = JsonConvert.DeserializeObject<List<AgregarSI>>(NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos"));
            AgregarSI SpoolBuscar = new AgregarSI();
            SpoolBuscar = SpoolsResueltos.Where(x => x.SpoolID == SpoolID).FirstOrDefault();
            if(SpoolBuscar != null)
            {
                SpoolsResueltos.Remove(SpoolBuscar);
                //Actualizo la cache con los spools resueltos
                NavContext.SetDataToSession<string>(Session, "ListaSpoolsResueltos", JsonConvert.SerializeObject(SpoolsResueltos));
                resultado = "OK";
            }
            else
            {
                resultado = "ERROR";
            }            
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GenerarSI(string Data)
        {
            string Inspector = SessionFacade.NombreCompleto; //Tanto para Usuario e Inspector           
            List<DetalleGenerarSI> Lista = JsonConvert.DeserializeObject<List<DetalleGenerarSI>>(Data == null ? "" : Data);
            string respuesta = OrdenTrabajoSpoolBO.Instance.GenerarSI(ToDataTable.Instance.toDataTable(Lista), SessionFacade.UserId, Inspector);
            //Limpio lista de spools generados                        
            List<AgregarSI> SpoolsResueltos = JsonConvert.DeserializeObject<List<AgregarSI>>(NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos"));
            List<DetalleGenerarSI> otra = new List<DetalleGenerarSI>();            
            otra = (from a in Lista join b in SpoolsResueltos on a.SpoolID equals b.SpoolID select a).ToList();
            SpoolsResueltos.RemoveAll(a => Lista.Any(b => a.SpoolID == b.SpoolID));
            /*if (otra.Count > 0)
            {
                for(int i = 0; i < otra.Count; i++)
                {
                    SpoolsResueltos.RemoveAt(i);
                }
                
            }*/
            NavContext.SetDataToSession<string>(Session, "ListaSpoolsResueltos", "");
            NavContext.SetDataToSession<string>(Session, "ListaSpoolsResueltos", JsonConvert.SerializeObject(SpoolsResueltos));
            //int valor;
            //if(int.TryParse(respuesta, out valor))
            //{
            //    //Limpio lista de spools resueltos
            //    Session["ListaSpoolsResueltos"] = null;
            //}
            var myData = new[] { new { result = respuesta } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ActualizarCacheProyectoNuevo(string ProyectoID)
        {
            HttpNavigationContext nav = new HttpNavigationContext();
            nav.SetDataToSession<string>(Session, "ProyectoIdNuevo", ProyectoID);
            var myData = new[] { new { result = "OK" } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerProyectoNuevo()
        {
            HttpNavigationContext nav = new HttpNavigationContext();
            string ProyectoID = "0";
            if (nav.GetDataFromSession<string>(Session, "ProyectoIdNuevo") != null)
            {
                ProyectoID = nav.GetDataFromSession<string>(Session, "ProyectoIdNuevo");
            }
            var myData = new[] { new { result = ProyectoID.ToString() } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SetVistaForm(int opc)
        {
            HttpNavigationContext nav = new HttpNavigationContext();
            nav.SetDataToSession<string>(Session, "VistaIncidencia", opc);
            var myData = new[] { new { result = "OK" } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetVistaForm()
        {
            HttpNavigationContext nav = new HttpNavigationContext();
            string Vista = "0";
            try
            {
                if (nav.GetDataFromSession<string>(Session, "VistaIncidencia") != null)
                {
                    Vista = nav.GetDataFromSession<string>(Session, "VistaIncidencia");
                }
            }
            catch (System.Exception e)
            {
                Vista = "0";
            }            
            var myData = new[] { new { result = Vista } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNumeroControl(string Prefijo, int ProyectoID, int ODT, int NumControl)
        {
            string Respuesta = "";
            int numDigitosODT = OrdenTrabajoSpoolBO.Instance.ObtenerDigitosODT(ProyectoID);
            List<string> ListaNumControl = new List<string>();
            for (int i = 1; i <= numDigitosODT; i++)
            {
                ListaNumControl.Add(Prefijo + ODT.ToString().PadLeft(i, '0') + "-" + NumControl.ToString().PadLeft(3, '0'));
            }                        
            ObjectoSpool Spool = OrdenTrabajoSpoolBO.Instance.ObtenerDatosSpool(ListaNumControl, ProyectoID);
            if(Spool.NumeroControl != null)
            {
                Respuesta = JsonConvert.SerializeObject(Spool);
            }
            else
            {
                Respuesta = "ERROR";
            }           
            var myData = new[] { new { result = Respuesta } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerDatosPorNumeroControl(int ProyectoID, string NumeroControl)
        {
            List<ListaIncidencia> ListaSpools = OrdenTrabajoSpoolBO.Instance.ObtenerSpoolsPorNumeroControl(ProyectoID, NumeroControl);
            string resultado = "";
            if (ListaSpools != null && ListaSpools.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(ListaSpools);
            }
            else
            {
                resultado = "NODATA";
            }
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

    }
}