using Newtonsoft.Json;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Busqueda;
using SAM.Entities.Personalizadas.Shop;
using SAM.Web.Common;
using SAM.Web.Shop.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAM.Web.Shop.Controllers
{
    public class AutorizarSIController : Controller
    {        
        // GET: AutorizarSI
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerSpools(string SI, int ProyectoID)        
        {
            List<AutorizarSI> ListaSpools = OrdenTrabajoSpoolBO.Instance.ObtenerSpoolsPorSQyProyecto(SI, ProyectoID);
            string resultado = "";            
            if(ListaSpools != null && ListaSpools.Count > 0)
            {                
                resultado = JsonConvert.SerializeObject(ListaSpools);
                
            }else
            {
                resultado = "NODATA";
            }            
            var myData = new[] { new { result = resultado } };            
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ActualizarCacheProyecto(string ProyectoID)
        {
            HttpNavigationContext nav = new HttpNavigationContext();
            nav.SetDataToSession<string>(Session, "ProyectoID", ProyectoID);
            var myData = new[] { new { result = "OK" } };
            return Json(myData, JsonRequestBehavior.AllowGet);           
        }

        [HttpGet]
        public JsonResult ObtenerProyecto()
        {
            HttpNavigationContext nav = new HttpNavigationContext();
            string ProyectoID = "0";
            if(nav.GetDataFromSession<string>(Session, "ProyectoID") != null)
            {
                ProyectoID = nav.GetDataFromSession<string>(Session, "ProyectoID");
            }
            var myData = new[] { new { result = ProyectoID.ToString() } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerTipoIncidencias()
        {
            List<TipoIncidencia> ListaIncidencias = OrdenTrabajoSpoolBO.Instance.ObtenerTipoIncidencias();
            string resultado = "";
            if (ListaIncidencias != null && ListaIncidencias.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(ListaIncidencias);
            }
            else
            {
                resultado = "NODATA";
            }
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerDetalleIncidencias(int TipoIncidenciaID, int SpoolID)
        {
            List<IncidenciaDetalle> Detalle = OrdenTrabajoSpoolBO.Instance.ObtenerDetalleIncidencias(TipoIncidenciaID, SpoolID);
            string resultado = "";
            if (Detalle != null && Detalle.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(Detalle);
            }
            else
            {
                resultado = "NODATA";
            }
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerListaErrores(int TipoIncidenciaID)
        {
            List<ListaErrores> Lista = OrdenTrabajoSpoolBO.Instance.ObtenerListaErrores(TipoIncidenciaID);
            string resultado = "";
            if(Lista != null && Lista.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(Lista);
            }else
            {
                resultado = "NODATA";
            }
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerIncidencias(int SpoolID)
        {            
            List<IncidenciaC> Incidencias = OrdenTrabajoSpoolBO.Instance.ObtenerIncidencias(SpoolID);
            string resultado = "";            
            if(Incidencias != null && Incidencias.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(Incidencias);
            }else
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
            string respuesta = OrdenTrabajoSpoolBO.Instance.GuardarIncidencia(SpoolID, TipoIncidenciaID, MaterialSpoolID, JuntaSpoolID, ErrorIncidenciaID, Observacion, Usuario, SI, 2);
            var myData = new[] { new { result = respuesta } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]        
        public JsonResult GuardarAutorizacion(string Captura)
        {
            List<DetalleGuardarAutorizacion> Lista = JsonConvert.DeserializeObject<List<DetalleGuardarAutorizacion>>(Captura);
            string Usuario = SessionFacade.NombreCompleto;
            DataTable dt = ToDataTable.Instance.toDataTable(Lista);
            string respuesta = OrdenTrabajoSpoolBO.Instance.GuardaAutorizacion(dt, Usuario);
            var myData = new[] { new { result = respuesta } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public JsonResult EliminarIncidencia(int IncidenciaID, string Origen, int Accion)
        //{
        //    /*Origen: Pantalla donde se eliminó la incidencia*/
        //    string respuesta = OrdenTrabajoSpoolBO.Instance.ResolverEliminarIncidencia(IncidenciaID, Origen, SessionFacade.NombreCompleto, Accion);
        //    var myData = new[] { new { result = respuesta } };
        //    return Json(myData, JsonRequestBehavior.AllowGet);
        //}
    }   
}