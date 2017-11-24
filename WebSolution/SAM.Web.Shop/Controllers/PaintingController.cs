using Resources.Controllers;
using Resources.Views;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Busqueda;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;
using SAM.Web.Common;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SAM.BusinessObjects.Excepciones;
using SAM.Web.Shop.Resources.Views.WorkStatus;
using SAM.Entities.Grid;
using Mimo.Framework.Common;
using System.IO;
using System.Security;
using System.Globalization;
using log4net;


namespace SAM.Web.Shop.Controllers
{
    public class PaintingController : AuthenticatedController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PaintingController));

        public PaintingController(INavigationContext navContext) : base(navContext) { }
        
        #region Pintura
        
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                NavContext.SetNumbersControl(string.Empty);
                if (NavContext.GetCurrentProject().ID > 0)
                {
                    SearchControlNumberModel model = new SearchControlNumberModel()
                    {
                        ProjectId = NavContext.GetCurrentProject().ID,
                        Spools = new List<NumeroControlBusqueda>(),
                        DateProcess = DateTime.Now.ToShortDateString(),
                        NumberProcess = string.Empty,
                        Exit = false
                    };

                    return View(model);
                }
            }
            catch (InvalidDataException e)
            {
                return View();
            }
            catch (SecurityException e)
            {
                return View();
            }

            return View();
        }
        
        [HttpGet]
        public ActionResult CancelPainting()
        {
            NavContext.SetNumbersControl(string.Empty);
            return View("Index", GetSearchModel(false));
        }

        [HttpGet]
        public ActionResult AddPainting(SearchControlNumberModel model, WorkstatusModel shipment)
        {
            if (ModelState.IsValid)
            {
                ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectId);
                model.Spools = new List<NumeroControlBusqueda>();
                model.Spools.AddRange(Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl()));

                switch (model.SearchType)
                {
                    case "cn":

                        List<string> controlNumbers = new List<string>(); 
                        
                        for (int i = 1; i <= project.DigitosOdt; i++ ) 
                        { 
                           controlNumbers.Add( project.PrefijoOdt + model.WorkOrderNumber.ToString().PadLeft(i, '0') + 
                                                   "-" + model.ControlNumber.ToString().PadLeft(3, '0')); 
                        }

                        List<int> controlNumberId = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolIDsPorNumeroDeControlYProyecto(controlNumbers, project.ID);

                        if (controlNumberId.Count > 0)
                        {
                            NavContext.SetProject(project.ID);
                            OrdenTrabajoSpool ots = OrdenTrabajoSpoolBO.Instance.Obtener(controlNumberId[0]);
                            NumeroControlBusqueda ncb = OrdenTrabajoSpoolBO.Instance.BuscarPorIdSpool(ots.SpoolID, model.ProjectId);

                            if (!model.Spools.Where(x => x.SpoolID == ncb.SpoolID).Any())
                            {
                                string spools = NavContext.GetCurrentNumbersControl();
                                spools = Helps.GetSpoolCookies(ncb, model.Spools);

                                NavContext.SetNumbersControl(spools);

                                _logger.Info("add spool: " + ots.NumeroControl);
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, WorkStatusStrings.Mensaje_SpoolAgregado);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, YardStrings.ControlNumber_ErrorMessage_DoesNotExist);
                        }

                        if (controlNumberId.Count > 0)
                        {
                            NavContext.SetProject(project.ID);
                        }

                        break;

                    case "sp":

                        List<NumeroControlBusqueda> ncbs = OrdenTrabajoSpoolBO.Instance.BuscarPorNombreDeSpool(model.SpoolName, project.ID);


                        if (ncbs.Count == 0)
                        {
                            ModelState.AddModelError(string.Empty, YardStrings.Search_NoResults);
                        }
                        else
                        {
                            NavContext.SetProject(project.ID);

                            string spools = NavContext.GetCurrentNumbersControl();
                            spools = Helps.GetSpoolsCookies(ncbs, model.Spools);

                            NavContext.SetNumbersControl(spools);

                            _logger.Info("add spool: " + model.SpoolName);
                        }

                        break;

                    default:
                        ModelState.AddModelError(string.Empty, YardStrings.Search_InvalidType);
                        break;
                }
            }

            model.Exit = false;
            return View("Index", model);
        }


        public ActionResult DeletePainting(int id)
        {
            string spools = NavContext.GetCurrentNumbersControl();
            List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
            NumeroControlBusqueda ncb = ncbs.Where(x => x.SpoolID == id).FirstOrDefault();
            if (ncb != null)
            {
                ncbs.Remove(ncb);
            }

            NavContext.SetNumbersControl(Helps.GetSpoolsCookie(ncbs));

            return View("Index", GetSearchModel(false));
        }

        [HttpPost]
        public ActionResult SavePainting(WorkstatusModel model)
        {
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();
            
            bool exit = false;
            if (ModelState.IsValid && ValidateModel(model, 2))
            {
                PinturaSpool paintSpool = GetPaintSpool(model);

                List<NumeroControlBusqueda> currentControlNumbers = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
                IQueryable<int> otsIds = currentControlNumbers.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();               
                IQueryable<WorkstatusSpool> workstatusSpools = WorkstatusSpoolBO.Instance.ObtenerWorkstatus(otsIds.ToArray()).AsQueryable();
                currentControlNumbers.ToList().ForEach(x => x.TieneWorkstatusSpool = workstatusSpools.Where(w => w.OrdenTrabajoSpoolID == x.OrdenTrabajoSpoolID).Select(w => w.WorkstatusSpoolID).FirstOrDefault());

                //sin proceso anterior ya no es obligatoria la requisición Issue #398, 407
                //int[] withoutPreviousProcess = workstatusSpools.Where(x => !x.TieneRequisicionPintura ).Select(x => x.OrdenTrabajoSpoolID).ToArray();
                
                //Get Spools without paint system
                int[] withoutPreviousProcess = WorkstatusSpoolBO.Instance.ObtenerOtsSpoolSistemaEmpty(otsIds.ToArray());
              
                //control number without previous process
                model.ControlNumberNotConditions.AddRange(currentControlNumbers.Where(x => withoutPreviousProcess.Contains(x.OrdenTrabajoSpoolID)));
            
                // control numbers are processed
                model.ControlNumberToProcess.AddRange(currentControlNumbers.Where(x => !withoutPreviousProcess.Contains(x.OrdenTrabajoSpoolID)));
                
                string otIDs = string.Empty;
                string rIDs = string.Empty;
                
                foreach (NumeroControlBusqueda ncb in model.ControlNumberToProcess)
                {
                    WorkstatusSpool wkSpool = workstatusSpools.Where(x => x.OrdenTrabajoSpoolID == ncb.OrdenTrabajoSpoolID).FirstOrDefault();

                    if (wkSpool == null)
                    {
                        wkSpool = new WorkstatusSpool()
                        {
                            OrdenTrabajoSpoolID = ncb.OrdenTrabajoSpoolID,
                            UsuarioModifica = SessionFacade.UserId,
                            FechaModificacion = DateTime.Now
                        };

                        WorkstatusSpoolBO.Instance.GuardarWorkstatus(wkSpool);
                        wkSpool = workstatusSpools.Where(x => x.OrdenTrabajoSpoolID == ncb.OrdenTrabajoSpoolID).FirstOrDefault();
                    }
                    
                    if(wkSpool != null)
                    {
                        RequisicionPintura rp = RequisicionPinturaBO.Instance.ObtenerRequisicionPinturaByWks(wkSpool.WorkstatusSpoolID);
                        RequisicionPinturaDetalle rpd = null;
                        bool dateProcessIsValid = true;
                        bool exist = false;

                        if (rp != null)
                        {
                            //valida fecha vs Fecha Requisicion
                            dateProcessIsValid = ValidateDatePreviousProcess(rp.FechaRequisicion, Convert.ToDateTime(model.ProcessDate));
                        }

                        OrdenTrabajoSpool ots = OrdenTrabajoSpoolBO.Instance.Obtener(ncb.OrdenTrabajoSpoolID);
                        exist = ValidateExistProcess(model.ProjectId, ots.OrdenTrabajoID, rp == null ? -1 : rp.RequisicionPinturaID, model.TypeReportId, ncb);

                        if (!dateProcessIsValid )
                        {
                            model.ControlNumberInvalidDate.Add(ncb);
                        }
                        else if (exist)
                        {
                            model.ControlNumberWhitProcess.Add(ncb);
                        }
                        else// not exist and date valid
                        {
                            if (rp != null)
                            {
                                rpd = rp.RequisicionPinturaDetalle.Where(x => x.WorkstatusSpoolID == wkSpool.WorkstatusSpoolID).FirstOrDefault();
                            }
                          
                            otIDs += ots.OrdenTrabajoSpoolID + ",";

                            if (rpd != null)
                            {
                                rIDs += rpd.RequisicionPinturaDetalleID + ",";
                            }
                            else
                            { 
                                rIDs += -1 + ","; 
                            }
                        }                       
                    }
                    else
                    {
                        model.ControlNumberNotConditions.Add(ncb);
                    }
                }

                foreach (NumeroControlBusqueda ncb in model.ControlNumberWhitProcess)
                {
                    if (model.ControlNumberToProcess.Contains(ncb))
                    {
                        model.ControlNumberToProcess.Remove(ncb);
                    }
                }
                foreach (NumeroControlBusqueda ncb in model.ControlNumberInvalidDate)
                {
                    if (model.ControlNumberToProcess.Contains(ncb))
                    {
                        model.ControlNumberToProcess.Remove(ncb);
                    }
                }
                foreach (NumeroControlBusqueda ncb in model.ControlNumberNotConditions)
                {
                    if (model.ControlNumberToProcess.Contains(ncb))
                    {
                        model.ControlNumberToProcess.Remove(ncb);
                    }
                }

                try
                {
                    if (!string.IsNullOrEmpty(rIDs))
                    {
                        PinturaBO.Instance.GuardaPinturaSpool(paintSpool, otIDs.Substring(0, otIDs.Length - 1), rIDs.Substring(0, rIDs.Length - 1), false, string.Empty, SessionFacade.UserId);
                    }

                    model.ControlNumberInvalidDate.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.FechaInvalida);
                    model.ControlNumberNotConditions.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.NoCumple);
                    model.ControlNumberWhitProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.ConProceso);
                    model.ControlNumberToProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.Procesado);

                    NavContext.SetNumbersControl(Helps.GetAllNumbersControlModelToString(model));

                    if (model.ControlNumberInvalidDate.Count > 0 || model.ControlNumberWhitProcess.Count > 0 || model.ControlNumberNotConditions.Count > 0)
                    {
                        return View("PaintingResults", model);
                    }
                    else
                    {
                        NavContext.SetNumbersControl(string.Empty);
                        exit = true;
                    }
                }
                catch (ExcepcionPintura e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault().Replace("<br />", string.Empty));
                    return View("Index", GetSearchModel(exit));
                }
                catch (ExcepcionReportes e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault().Replace("<br />", string.Empty));
                    return View("Index", GetSearchModel(exit));
                }
            }

            return View("Index", GetSearchModel(exit));
        }

        private bool ValidateExistProcess(int projectId, int otId, int? reqPaintId, int typeReportId, NumeroControlBusqueda ncb)
        {
            int requisicionId = reqPaintId == null ? -1 : reqPaintId.Value;

            List<GrdPintura> lstPintura = PinturaBO.Instance.ObtenerListadoPintura(projectId, otId, requisicionId);
            bool exist = true;
            GrdPintura paint = null;
            switch (typeReportId)
            {
                case 1:
                    paint = lstPintura.Where(x => x.FechaSandBlast != null && !string.IsNullOrEmpty(x.ReporteSandBlast) && x.NumeroControl == ncb.NumeroControl).FirstOrDefault();
                    break;
                case 2:
                    paint = lstPintura.Where(x => x.FechaPrimario != null && !string.IsNullOrEmpty(x.ReportePrimario) && x.NumeroControl == ncb.NumeroControl).FirstOrDefault();
                    break;
                case 3:
                    paint = lstPintura.Where(x => x.FechaIntermedio != null && !string.IsNullOrEmpty(x.ReporteIntermedio) && x.NumeroControl == ncb.NumeroControl).FirstOrDefault();
                    break;
                case 4:
                    paint = lstPintura.Where(x => x.FechaAcabadoVisual != null && !string.IsNullOrEmpty(x.ReporteAcabadoVisual) && x.NumeroControl == ncb.NumeroControl).FirstOrDefault();
                    break;
                case 5:
                    paint = lstPintura.Where(x => x.FechaAdherencia != null && !string.IsNullOrEmpty(x.ReporteAdherencia) && x.NumeroControl == ncb.NumeroControl).FirstOrDefault();
                    break;
                case 6:
                    paint = lstPintura.Where(x => x.FechaPullOff != null && !string.IsNullOrEmpty(x.ReportePullOff) && x.NumeroControl == ncb.NumeroControl).FirstOrDefault();
                    break;
                default:
                    paint = null;
                    break;
            }

            if (paint == null)
            {
                exist = false;
            }

            return exist;
        }

        private SearchControlNumberModel GetSearchModel(bool exit)
        {
            List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
            SearchControlNumberModel searchNumberControl = new SearchControlNumberModel();
            searchNumberControl.ProjectId = NavContext.GetCurrentProject().ID;
            searchNumberControl.Spools = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.AProcesar).ToList();
            searchNumberControl.Exit = exit;

            return searchNumberControl;
        }

        public ActionResult OverwritePainting(WorkstatusModel model)
        {
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();
            
            bool exit = false;
            PinturaSpool paintSpool = GetPaintSpool(model);

            List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
            model.ControlNumberWhitProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.ConProceso).ToList();
            model.ControlNumberToProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.Procesado).ToList();
            model.ControlNumberNotConditions = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.NoCumple).ToList();
            model.ControlNumberInvalidDate = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.FechaInvalida).ToList();

            IQueryable<int> otsIds = model.ControlNumberWhitProcess.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();
            IQueryable<WorkstatusSpool> workstatusSpools = WorkstatusSpoolBO.Instance.ObtenerWorkstatus(otsIds.ToArray()).AsQueryable();

            string otlIDs = string.Empty;
            string rIDs = string.Empty;

            foreach (NumeroControlBusqueda ncb in model.ControlNumberWhitProcess)
            {
                WorkstatusSpool wkSpool = workstatusSpools.Where(x => x.OrdenTrabajoSpoolID == ncb.OrdenTrabajoSpoolID).FirstOrDefault();

                if (wkSpool != null)
                {
                    RequisicionPintura rp = RequisicionPinturaBO.Instance.ObtenerRequisicionPinturaByWks(wkSpool.WorkstatusSpoolID);
                    RequisicionPinturaDetalle rpd = null;

                    if (rp != null)
                    {
                        rpd = rp.RequisicionPinturaDetalle.Where(x => x.WorkstatusSpoolID == wkSpool.WorkstatusSpoolID).FirstOrDefault();
                        
                        otlIDs += wkSpool.OrdenTrabajoSpoolID + ",";

                        if (rpd != null)
                        {                          
                            rIDs += rpd.RequisicionPinturaDetalleID + ",";
                        }
                        else
                        {
                            rIDs += -1 + ",";
                        }
                    }
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(rIDs))
                {
                    PinturaBO.Instance.GuardaPinturaSpool(paintSpool, otlIDs.Substring(0, otlIDs.Length - 1), rIDs.Substring(0, rIDs.Length - 1), false, string.Empty, SessionFacade.UserId);
                }

                NavContext.SetNumbersControl(string.Empty);


                exit = true;
            }
            catch (ExcepcionPintura e)
            {
                ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault());
                return View("Index", GetSearchModel(exit));
            }
            catch (ExcepcionReportes e)
            {
                ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault());
                return View("Index", GetSearchModel(exit));
            }


            return View("Index", GetSearchModel(exit));
        }

        #endregion

        private DateTime FormatDate(string date)
        {
            if (CultureInfo.CurrentCulture.Name == "es-MX")
            {
                string[] partesDate = date.Split('/');
                date = partesDate[1] + "/" + partesDate[0] + "/" + partesDate[2];
            }

            return Convert.ToDateTime(date);
        }

        private bool ValidateDatePreviousProcess(DateTime datePreviousProcess, DateTime dateProcess)
        {
            if (dateProcess < datePreviousProcess)
            {
                return false;
            }

            return true;
        }

        private PinturaSpool GetPaintSpool(WorkstatusModel model)
        {            
            PinturaSpool paintSpool = new PinturaSpool();
            paintSpool.ProyectoID = model.ProjectId;

            switch (model.TypeReportId)
            {
                case 1:
                    paintSpool.FechaSandblast = Convert.ToDateTime(model.ProcessDate);
                    paintSpool.ReporteSandblast = model.ProcessNumber;
                    break;


                case 2:
                    paintSpool.FechaPrimarios = Convert.ToDateTime(model.ProcessDate);
                    paintSpool.ReportePrimarios = model.ProcessNumber;
                    break;


                case 3:
                    paintSpool.FechaIntermedios = Convert.ToDateTime(model.ProcessDate);
                    paintSpool.ReporteIntermedios = model.ProcessNumber;
                    break;


                case 4:
                    paintSpool.FechaAcabadoVisual = Convert.ToDateTime(model.ProcessDate);
                    paintSpool.ReporteAcabadoVisual = model.ProcessNumber;
                    break;


                case 5:
                    paintSpool.FechaAdherencia = Convert.ToDateTime(model.ProcessDate);
                    paintSpool.ReporteAdherencia = model.ProcessNumber;
                    break;


                case 6:
                    paintSpool.FechaPullOff = Convert.ToDateTime(model.ProcessDate);
                    paintSpool.ReportePullOff = model.ProcessNumber;
                    break;


                default:
                    ModelState.AddModelError(string.Empty, YardStrings.Search_InvalidType);
                    break;
            }


            return paintSpool;
        }

        private bool ValidateModel(WorkstatusModel model, int accion)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(model.ProcessNumber))
            {
                ModelState.AddModelError(string.Empty, WorkStatusStrings.NumberReport_Required_ErrorMessage);
                isValid = false;
            }

            if (model.ProcessDate == null)
            {
                ModelState.AddModelError(string.Empty, WorkStatusStrings.DateProcess_Required_ErrorMessage);
                isValid = false;
            }

            // action 2 painting
            if (model.TypeReportId == 0 && accion == 2)
            {
                ModelState.AddModelError(string.Empty, WorkStatusStrings.TypeReport_Required_ErrorMessage);
                isValid = false;
            }

            return isValid;
        }

        public ActionResult DeletePaintingResults(int id, WorkstatusModel model)
        {
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();

            List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());

            NumeroControlBusqueda ncb = ncbs.Where(x => x.SpoolID == id).FirstOrDefault();

            if (ncb != null)
            {
                ncbs.Remove(ncb);
            }

            NavContext.SetNumbersControl(Helps.GetSpoolsCookie(ncbs));

            model.ControlNumberWhitProcess.AddRange(ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.ConProceso).ToList());
            model.ControlNumberToProcess.AddRange(ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.Procesado).ToList());
            model.ControlNumberInvalidDate.AddRange(ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.FechaInvalida).ToList());
            model.ControlNumberNotConditions.AddRange(ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.NoCumple).ToList());

            return View("PaintingResults", model);
        }
    }
}
