using System.Web.Mvc;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using SAM.Common;
using SAM.Entities.Personalizadas;
using SAM.Web.Common;
using SAM.Web.Shop.Mappers;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Utils;
using SAM.Entities;
using SAM.BusinessLogic.Calidad;
using SAM.Entities.Personalizadas.Shop;
using System.Collections.Generic;

namespace SAM.Web.Shop.Controllers
{
    public class ControlNumberController : AuthenticatedController
    {
        public ControlNumberController(INavigationContext navContext) : base(navContext) { }

        [HttpGet]
        public ActionResult Index(int controlNumberId)
        {
            OrdenTrabajoSpool controlNumber = OrdenTrabajoSpoolBO.Instance.Obtener(controlNumberId);
            DetSpool spool = SpoolBO.Instance.ObtenerDetalleCompleto(controlNumber.SpoolID);

            NavContext.SetControlNumber(controlNumber);

            //ControlNumberModel model = ControlNumberMapper.FromSpoolAndControlNumber(spool, controlNumber);
            //model.Drawings = DrawingHelper.GetImagePathsForDrawing("IPC III", "110-6-FG-6226-1CB1S");
            NavContext.SetProject(spool.ProyectoID);
            return RedirectToAction("SummaryShop", "ControlNumber", new { controlNumberId = controlNumber.OrdenTrabajoSpoolID });           
        }

        [HttpGet]
        public ActionResult DrawingPageImage(DrawingPage page)
        {
            string imagePath = Configuracion.CalidadRutaDossier + @"\" + page.ProjectName + @"\Reportes\Dibujo\Paginas-" + page.DrawingName + @"\" + page.PageName + page.Extension;

            return File(imagePath, "image/jpg");
        }

        [HttpGet]
        public ActionResult CertificationReports(int controlNumberId)
        {
            OrdenTrabajoSpool controlNumber = OrdenTrabajoSpoolBO.Instance.Obtener(controlNumberId);
            NavContext.SetControlNumber(controlNumber);


            CertificationReport cr = CertificacionBL.Instance.ObtenerReporteCertificacionShop(controlNumber.SpoolID.ToString(), NavContext.GetCurrentProject().ID);


            CertificationReportModel crm = Helps.ConvertObjects<CertificationReportModel, CertificationReport>(cr);

            if (crm.DetailSpool == null)
            {
                crm.DetailSpool = new DetailSpoolModel();
            }

            if (crm.DetailMaterials == null)
            {
                crm.DetailMaterials = new List<MaterialModel>();
            }

            if (crm.DetailJoints == null)
            {
                crm.DetailJoints = new List<DetailJointModel>();
            }

            if (crm.DetailCheckListShipping == null)
            {
                crm.DetailCheckListShipping = new CheckListShippingModel();
            }
            crm.Project = NavContext.GetCurrentProject().Nombre;
            crm.Yard = NavContext.GetCurrentYard().Nombre;
            crm.NumberControl = NavContext.GetCurrentControlNumber().ControlNumber;
            //crm.Granel = OrdenTrabajoSpoolBO.Instance.EsGranel(controlNumber.SpoolID);
            return View("CertificationReport", crm);
        }

        [HttpGet]
        public ActionResult SummaryShop(int controlNumberId)
        {
            OrdenTrabajoSpool controlNumber = OrdenTrabajoSpoolBO.Instance.Obtener(controlNumberId);
            NavContext.SetControlNumber(controlNumber);

            SummarySpool cr = CertificacionBL.Instance.ObtenerReporteSummaryShop(controlNumber.SpoolID.ToString(), NavContext.GetCurrentProject().ID);


            SummarySpoolModel crm = Helps.ConvertObjects<SummarySpoolModel, SummarySpool>(cr);

            if (crm.DetailMaterials == null)
            {
                crm.DetailMaterials = new List<DetailMaterialSummaryModel>();
            }

            if (crm.DetailJoints == null)
            {
                crm.DetailJoints = new List<DetailSummaryJointModel>();
            }

            crm.FechaAcabado = cr.FechaAcabado;
            crm.FechaPrimario = cr.FechaPrimario;
            crm.FechaLiberacionDimensional = cr.FechaLiberacionDimensional;
            crm.ReporteLiberacionDimensional = cr.ReporteLiberacionDimensional;
            crm.Cuadrante = cr.Cuadrante;
            crm.SistemaPintura = cr.SistemaPintura;
            crm.Hold = cr.Hold;
            crm.GrupoAcero = cr.GrupoAcero; //Add- 13-02-2018
            crm.Project = NavContext.GetCurrentProject().Nombre;
            crm.ProjectID = NavContext.GetCurrentProject().ID;
            crm.Yard = NavContext.GetCurrentYard().Nombre;
            crm.NumberControl = NavContext.GetCurrentControlNumber().ControlNumber;
            crm.NumberControlId = NavContext.GetCurrentControlNumber().ControlNumberId;
            crm.Granel = OrdenTrabajoSpoolBO.Instance.EsGranel(controlNumber.SpoolID);

            return View("SummarySpool", crm);
        }
    }	
}