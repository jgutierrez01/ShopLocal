using Newtonsoft.Json;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Busqueda;
using SAM.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAM.Web.Shop.Controllers
{
    public class IncidenciasController : Controller
    {
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
    }
}