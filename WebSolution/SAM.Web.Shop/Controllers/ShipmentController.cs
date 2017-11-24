using System.Globalization;
using Microsoft.Ajax.Utilities;
using Resources.Controllers;
using Resources.Views;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Workstatus;
using SAM.Web.Shop.Resources.Views.WorkStatus;
using SAM.Entities;
using SAM.Entities.Busqueda;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;
using SAM.Web.Common;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Resources;
using SAM.Web.Shop.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Helpers;
using SAM.Web.Shop.Resources.Models.Workstatus;
using System.IO;
using System.Security;
using SAM.BusinessLogic.Utilerias;
using log4net;


namespace SAM.Web.Shop.Controllers
{
    public class ShipmentController : AuthenticatedController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ShipmentController));

        public ShipmentController(INavigationContext navContext) : base(navContext) { }

        #region Embarque


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
                return View();
            }
            catch (SecurityException e)
            {
                return View();
            }


            return View();
        }


        [HttpGet]
        public ActionResult CancelShipment()
        {
            NavContext.SetNumbersControl(string.Empty);
            return View("Index", GetSearchModel(false, string.Empty, DateTime.Now.ToShortDateString()));
        }

        [HttpGet]
        public ActionResult AddShipment(SearchControlNumberModel model, WorkstatusModel shipment)
        {
            ViewData.Add("TypeReportId", shipment.TypeReportId);


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

        public ActionResult DeleteShipment(int id, string numberProcess, string dateProcess)
        {
            string spools = NavContext.GetCurrentNumbersControl();
            List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
            NumeroControlBusqueda ncb = ncbs.Where(x => x.SpoolID == id).FirstOrDefault();

            if (ncb != null)
            {
                ncbs.Remove(ncb);
            }
            
            NavContext.SetNumbersControl(Helps.GetSpoolsCookie(ncbs));

            return View("Index", GetSearchModel(false, numberProcess, dateProcess));
        }
        
        [HttpPost]
        public ActionResult SaveShipment(WorkstatusModel model)
        {
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();

            bool exit = false;

            if (ModelState.IsValid && ValidateModel(model, 2))
            {
                Embarque shipment = new Embarque
                {
                    ProyectoID = model.ProjectId,
                    FechaEstimada = Convert.ToDateTime(model.ProcessDate),
                    NumeroEmbarque = model.ProcessNumber
                };

                List<NumeroControlBusqueda> currentControlNumbers = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
             
                IQueryable<int> otsIds = currentControlNumbers.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();
                IQueryable<WorkstatusSpool> workstatusSpools = WorkstatusSpoolBO.Instance.ObtenerWorkstatus(otsIds.ToArray()).AsQueryable();
                int[] otsWithWks = workstatusSpools.Select(x => x.OrdenTrabajoSpoolID).ToArray();
                int[] withoutPreviousProcess = workstatusSpools.Where(x => !x.TieneLiberacionDimensional || !x.Preparado)
                                                            .Select(x => x.OrdenTrabajoSpoolID).ToArray();
                
                //control number without workstatus
                model.ControlNumberNotConditions.AddRange(currentControlNumbers.Where(x => !otsWithWks.Contains(x.OrdenTrabajoSpoolID)));
                
                //control number without previous process
                model.ControlNumberNotConditions.AddRange(currentControlNumbers.Where(x => withoutPreviousProcess.Contains(x.OrdenTrabajoSpoolID)));
                workstatusSpools = workstatusSpools.Where(x => !withoutPreviousProcess.Contains(x.OrdenTrabajoSpoolID));
                otsWithWks = workstatusSpools.Select(x => x.OrdenTrabajoSpoolID).ToArray();

                //Date Validation 
                string FechasDimension = ValidaFechasBO.Instance.ObtenerFechasDimensionales(workstatusSpools.Select(x => x.WorkstatusSpoolID).ToArray());
                string FechasPreparacion = ValidaFechasBO.Instance.ObtenerFechasPreparacion(workstatusSpools.Select(x => x.WorkstatusSpoolID).ToArray());

                //Se validan las fechas solo de los numeros de control que cuentan con proceso anterior
                List<int> whitDateNotValid = ValidateDates(otsWithWks, FechasDimension, FechasPreparacion, Convert.ToDateTime(model.ProcessDate));

                //control numbers with not valid date
                model.ControlNumberInvalidDate.AddRange(currentControlNumbers.Where(x => whitDateNotValid.Contains(x.OrdenTrabajoSpoolID)));
                workstatusSpools = workstatusSpools.Where(x => !whitDateNotValid.Contains(x.OrdenTrabajoSpoolID));
                otsWithWks = workstatusSpools.Select(x => x.OrdenTrabajoSpoolID).ToArray();

                // control numbers are processed
                model.ControlNumberToProcess = currentControlNumbers.Where(x => otsWithWks.Contains(x.OrdenTrabajoSpoolID)).ToList();

                List<int> lstSpoolsToProcess = new List<int>();

                foreach (NumeroControlBusqueda ncb in model.ControlNumberToProcess)
                {
                    WorkstatusSpool wkSpool = workstatusSpools.Where(x => x.OrdenTrabajoSpoolID == ncb.OrdenTrabajoSpoolID).FirstOrDefault();

                    if (wkSpool != null)
                    {
                        if (wkSpool.Embarcado)
                        {
                            model.ControlNumberWhitProcess.Add(ncb);
                        }
                        else
                        {
                            lstSpoolsToProcess.Add(wkSpool.WorkstatusSpoolID);
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
                        EmbarqueBO.Instance.GuardaEmbarque(shipment, lstSpoolsToProcess.ToArray(), SessionFacade.UserId);
                    }

                    model.ControlNumberInvalidDate.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.FechaInvalida);
                    model.ControlNumberNotConditions.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.NoCumple);
                    model.ControlNumberWhitProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.ConProceso);
                    model.ControlNumberToProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.Procesado);

                    NavContext.SetNumbersControl(Helps.GetAllNumbersControlModelToString(model));

                    if (model.ControlNumberInvalidDate.Count > 0 || model.ControlNumberWhitProcess.Count > 0 || model.ControlNumberNotConditions.Count > 0)
                    {                        
                        return View("ShipmentResults", model);
                    }
                    else
                    {
                        NavContext.SetNumbersControl(string.Empty);
                        exit = true;
                    }
                }
                catch (ExcepcionEmbarque e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault().Replace("<br/>", ""));
                    return View("Index", GetSearchModel(exit, model.ProcessNumber, model.ProcessDate));
                }
            }

            return View("Index", GetSearchModel(exit, string.Empty, model.ProcessDate));
        }

        [HttpPost]
        public ActionResult OverwriteShipment(WorkstatusModel model)
        {
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();

            List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
            model.ControlNumberWhitProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.ConProceso).ToList();
            model.ControlNumberToProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.Procesado).ToList();
            model.ControlNumberNotConditions = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.NoCumple).ToList();
            model.ControlNumberInvalidDate = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.FechaInvalida).ToList();

            IQueryable<int> otsIds = model.ControlNumberWhitProcess.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();
            IQueryable<WorkstatusSpool> workstatusSpools = WorkstatusSpoolBO.Instance.ObtenerWorkstatus(otsIds.ToArray()).AsQueryable();
            int[] lstNCToSent = workstatusSpools.Select(x => x.WorkstatusSpoolID).ToArray();
            
            bool exit = false;
            
            Embarque embarque = new Embarque
            {
                ProyectoID = model.ProjectId,
                FechaEstimada = Convert.ToDateTime(model.ProcessDate),
                NumeroEmbarque = model.ProcessNumber,
            };

            try
            {
                if (lstNCToSent.Count() > 0)
                {
                    EmbarqueBO.Instance.GuardaEmbarque(embarque, lstNCToSent, SessionFacade.UserId);
                }
                NavContext.SetNumbersControl(string.Empty);
                exit = true;
            }
            catch (ExcepcionEmbarque e)
            {
                ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault().Replace("<br />", ""));

                return View("Index", GetSearchModel(exit, model.ProcessNumber, model.ProcessDate));
            }

            return View("Index", GetSearchModel(exit, string.Empty, DateTime.Now.ToShortDateString()));
        }

        #endregion

        private List<int> ValidateDates(int[] otSpoolIds, string datesDimentional, string datesPrparation, DateTime dateProcess)
        {
            string[] lstDatesDimentional = datesDimentional.Substring(0, datesDimentional.Length).Split(',');
            string[] lstDatesPreparation = datesPrparation.Substring(0, datesPrparation.Length).Split(',');


            List<int> otSoolsNoValid = new List<int>();

            for (int i = 0; i < otSpoolIds.Count(); i++)
            {
                DateTime dateDimensional = DateTime.Now;
                DateTime datePreparation = DateTime.Now;


                if (CultureInfo.CurrentCulture.Name.Equals("es-MX"))
                {
                    string[] partesDateDimentional = lstDatesDimentional[i].Split('/');
                    lstDatesDimentional[i] = partesDateDimentional[1] + "/" + partesDateDimentional[0] + "/" + partesDateDimentional[2];


                    string[] partesDatePreparation = lstDatesPreparation[i].Split('/');
                    lstDatesPreparation[i] = partesDatePreparation[1] + "/" + partesDatePreparation[0] + "/" + partesDatePreparation[2];
                }


                dateDimensional = Convert.ToDateTime(lstDatesDimentional[i]);
                datePreparation = Convert.ToDateTime(lstDatesPreparation[i]);


                if (dateProcess < dateDimensional)
                {
                    otSoolsNoValid.Add(otSpoolIds[i]);
                }
                else if (dateProcess < datePreparation)
                {
                    otSoolsNoValid.Add(otSpoolIds[i]);
                }
            }

            return otSoolsNoValid;

        }

        private bool ValidateModel(WorkstatusModel model, int action)
        {
            bool isValid = true;



            if (model.ProcessDate == null)
            {
                ModelState.AddModelError(string.Empty, WorkStatusStrings.DateProcess_Required_ErrorMessage);
                isValid = false;
            }

            //action 2 Shipment
            if (string.IsNullOrEmpty(model.ProcessNumber) && action == 2)
            {
                ModelState.AddModelError(string.Empty, WorkStatusStrings.NumberShipment_Required_ErrorMessage);
                isValid = false;
            }

            return isValid;
        }


        private SearchControlNumberModel GetSearchModel(bool exit, string numberProcess, string dateProcess)
        {
            SearchControlNumberModel searchNumberControl = new SearchControlNumberModel();
            searchNumberControl.ProjectId = NavContext.GetCurrentProject().ID;
            searchNumberControl.Spools = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
            searchNumberControl.Exit = exit;
            searchNumberControl.NumberProcess = numberProcess;
            searchNumberControl.DateProcess = dateProcess;
            
            return searchNumberControl;
        }

        public ActionResult DeleteShipmentResults(int id, WorkstatusModel model)
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

            return View("ShipmentResults", model);
        }
    }
}
