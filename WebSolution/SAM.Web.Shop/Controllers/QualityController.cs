using log4net;
using Mimo.Framework.Exceptions;
using Resources.Controllers;
using Resources.Views;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Entities.Busqueda;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;
using SAM.Web.Common;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Resources.Views.WorkStatus;
using SAM.Web.Shop.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;


namespace SAM.Web.Shop.Controllers
{
    public class QualityController : AuthenticatedController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(QualityController));

        public QualityController(INavigationContext navContext) : base(navContext) { }

        #region Liberacion calidad


        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                NavContext.SetNumbersControl(string.Empty);
                if (NavContext.GetCurrentProject().ID > 0)
                {
                    SearchControlNumberModel modelNw = new SearchControlNumberModel()
                    {
                        ProjectId = NavContext.GetCurrentProject().ID,
                        Spools = new List<NumeroControlBusqueda>(),
                        DateProcess = DateTime.Now.ToShortDateString(),
                        NumberProcess = string.Empty,
                        Exit = false
                    };


                    return View(modelNw);
                }
            }
            catch (InvalidDataException e)
            {
                ModelState.AddModelError("ProjectId", e.Message);
                return View();
            }
            catch (SecurityException e)
            {
                ModelState.AddModelError("ProjectId", e.Message);
                return View();
            }


            return View();
        }


        [HttpGet]
        public ActionResult CancelRelease()
        {
            NavContext.SetNumbersControl(string.Empty);
            return View("Index", GetSearchModel(false));
        }


        [HttpGet]
        public ActionResult AddRelease(SearchControlNumberModel model, WorkstatusModel shipment)
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


        public ActionResult DeleteRelease(int id)
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
        public ActionResult SaveRelease(WorkstatusModel model)
        {
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();

            bool exit = false;

            if (ModelState.IsValid && ValidateModel(model))
            {
                List<NumeroControlBusqueda> currentControlNumbers = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());

                IQueryable<int> otsIds = currentControlNumbers.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();
                IQueryable<WorkstatusSpool> workstatusSpools = WorkstatusSpoolBO.Instance.ObtenerWorkstatus(otsIds.ToArray()).AsQueryable();
                int[] otsWithWks = workstatusSpools.Select(x => x.OrdenTrabajoSpoolID).ToArray();

                //control number without workstatus
                model.ControlNumberNotConditions.AddRange(currentControlNumbers.Where(x => !otsWithWks.Contains(x.OrdenTrabajoSpoolID)));
                
                // control numbers are processed
                model.ControlNumberToProcess = currentControlNumbers.Where(x => otsWithWks.Contains(x.OrdenTrabajoSpoolID)).ToList();

                List<int> lstSpoolsToProcess = new List<int>();
                
                foreach (NumeroControlBusqueda ncb in model.ControlNumberToProcess)
                {
                    WorkstatusSpool wkSpool = workstatusSpools.Where(x => x.OrdenTrabajoSpoolID == ncb.OrdenTrabajoSpoolID).FirstOrDefault();

                    if (wkSpool != null)
                    {
                        if (wkSpool.FechaLiberacionCalidad.HasValue)
                        {
                            if (!model.ControlNumberWhitProcess.Contains(ncb))
                            {
                                model.ControlNumberWhitProcess.Add(ncb);
                            }
                        }
                        else
                        {
                            lstSpoolsToProcess.Add(wkSpool.OrdenTrabajoSpoolID);
                        }
                    }
                    else
                    {
                        if (!model.ControlNumberNotConditions.Contains(ncb))
                        {
                            model.ControlNumberNotConditions.Add(ncb);
                        }
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
                    if (lstSpoolsToProcess.Count > 0)
                    {
                        WorkstatusSpoolBO.Instance.GuardarFechasLiberacionCalidad(Convert.ToDateTime(model.ProcessDate), lstSpoolsToProcess.ToArray(), SessionFacade.UserId);
                    }

                    model.ControlNumberInvalidDate.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.FechaInvalida);
                    model.ControlNumberNotConditions.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.NoCumple);
                    model.ControlNumberWhitProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.ConProceso);
                    model.ControlNumberToProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.Procesado);

                    NavContext.SetNumbersControl(Helps.GetAllNumbersControlModelToString(model));

                    if (model.ControlNumberWhitProcess.Count > 0 || model.ControlNumberInvalidDate.Count > 0 || model.ControlNumberNotConditions.Count > 0)
                    {
                        return View("ReleaseResults", model);
                    }
                    else
                    {
                        NavContext.SetNumbersControl(string.Empty);
                        exit = true;
                    }
                }
                catch (ExcepcionPintura e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault());
                    return View("Index", GetSearchModel(exit));
                }
            }

            return View("Index", GetSearchModel(exit));
        }

        public ActionResult OverwriteRelease(WorkstatusModel model)
        {
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();

            bool exit = false;

            if (ModelState.IsValid && ValidateModel(model))
            {
                List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
                model.ControlNumberWhitProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.ConProceso).ToList();
                model.ControlNumberToProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.Procesado).ToList();
                model.ControlNumberNotConditions = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.NoCumple).ToList();
                model.ControlNumberInvalidDate = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.FechaInvalida).ToList();

                IQueryable<int> otsIds = model.ControlNumberWhitProcess.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();
                IQueryable<WorkstatusSpool> workstatusSpools = WorkstatusSpoolBO.Instance.ObtenerWorkstatus(otsIds.ToArray()).AsQueryable();

                List<string> errors = new List<string>();

                foreach (NumeroControlBusqueda ncb in model.ControlNumberWhitProcess)
                {
                    WorkstatusSpool wkSpool = workstatusSpools.Where(x => x.OrdenTrabajoSpoolID == ncb.OrdenTrabajoSpoolID).FirstOrDefault();
                    
                    if (wkSpool != null)
                    {
                        try
                        {
                            WorkstatusSpoolBO.Instance.GuardarFechaLiberacionCalidad(Convert.ToDateTime(model.ProcessDate), ncb.OrdenTrabajoSpoolID, SessionFacade.UserId);
                        }
                        catch (BaseValidationException e)
                        {
                            errors.AddRange(e.Details.ToList());
                        }
                    }
                    else
                    {
                        model.ControlNumberInvalidDate.Add(ncb);
                    }
                }

                if (errors.Count > 0)
                {
                    foreach (string error in errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    return View("Index", GetSearchModel(exit));
                }
                else
                {
                    NavContext.SetNumbersControl(string.Empty);
                    exit = true;
                }
            }

            return View("Index", GetSearchModel(exit));
        }


        #endregion
        

        private bool ValidateModel(WorkstatusModel model)
        {
            bool isValid = true;


            if (model.ProcessDate == null)
            {
                ModelState.AddModelError(string.Empty, WorkStatusStrings.DateProcess_Required_ErrorMessage);
                isValid = false;
            }


            return isValid;
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

        public ActionResult DeleteReleaseResults(int id, WorkstatusModel model)
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

            return View("QualityResults", model);
        }
    }
}
