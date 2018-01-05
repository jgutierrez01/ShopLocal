using Newtonsoft.Json;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Busqueda;
using SAM.Web.Shop.Utils;
using System;
using System.Collections.Generic;
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
            List<CuadranteNumeroControlSQ> ListaSpools = OrdenTrabajoSpoolBO.Instance.ObtenerSpoolsPorSQyProyecto(SI, ProyectoID);
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
    }   
}