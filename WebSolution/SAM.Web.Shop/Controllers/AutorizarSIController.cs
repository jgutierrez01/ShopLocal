using Newtonsoft.Json;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Busqueda;
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
    }   
}