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
using log4net;




namespace SAM.Web.Shop.Controllers
{
    public class PreparationShipmentController : AuthenticatedController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(PreparationShipmentController));

        public PreparationShipmentController(INavigationContext navContext) : base(navContext) { }

        #region Preparar
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
        public ActionResult CancelPreparation()
        {
            NavContext.SetNumbersControl(string.Empty);
            return View("Index", GetSearchModel(false));
        }


        [HttpGet]
        public ActionResult AddPreparation(SearchControlNumberModel model)
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


        public ActionResult DeletePreparation(int id)
        {
            string spools = NavContext.GetCurrentNumbersControl();
            List<NumeroControlBusqueda> ncbs = GetCurrentControlNumbers();
            NumeroControlBusqueda ncb = ncbs.Where(x => x.SpoolID == id).FirstOrDefault();
            if (ncb != null)
            {
                ncbs.Remove(ncb);
            }


            NavContext.SetNumbersControl(Helps.GetSpoolsCookie(ncbs));


            return View("Index", GetSearchModel(false));
        }


        [HttpPost]
        public ActionResult SavePreparation(WorkstatusModel model)
        {
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();
         
            bool exit = false;

            if (ModelState.IsValid)
            {
                List<NumeroControlBusqueda> currentControlNumbers = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
              
                IQueryable<int> otsIds = currentControlNumbers.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();
                IQueryable<WorkstatusSpool> workstatusSpools = WorkstatusSpoolBO.Instance.ObtenerWorkstatus(otsIds.ToArray()).AsQueryable();
                int[] otsWithWks = workstatusSpools.Select(x => x.OrdenTrabajoSpoolID).ToArray();
                int[] withoutPreviousProcess = workstatusSpools.Where(x => !x.TieneLiberacionDimensional).Select(x => x.OrdenTrabajoSpoolID).ToArray();
                
                //control number without workstatus
                model.ControlNumberNotConditions.AddRange(currentControlNumbers.Where(x => !otsWithWks.Contains(x.OrdenTrabajoSpoolID)));

                //control number without previous process
                model.ControlNumberNotConditions.AddRange(currentControlNumbers.Where(x => withoutPreviousProcess.Contains(x.OrdenTrabajoSpoolID)));
                workstatusSpools = workstatusSpools.Where(x => !withoutPreviousProcess.Contains(x.OrdenTrabajoSpoolID));
                otsWithWks = workstatusSpools.Select(x => x.OrdenTrabajoSpoolID).ToArray();
                
                // control numbers are processed
                model.ControlNumberToProcess = currentControlNumbers.Where(x => otsWithWks.Contains(x.OrdenTrabajoSpoolID)).ToList();
                
                List<int> lstSpoolsToPrepare = new List<int>();
                
                foreach (NumeroControlBusqueda ncb in model.ControlNumberToProcess)
                {
                    // pendiente validar Fechas
                    WorkstatusSpool wkSpool = workstatusSpools.Where(x => x.OrdenTrabajoSpoolID == ncb.OrdenTrabajoSpoolID).FirstOrDefault();
                    
                    if (wkSpool != null)
                    {
                        if (wkSpool.Preparado)
                        {
                            model.ControlNumberWhitProcess.Add(ncb);
                        }
                        else
                        {
                            lstSpoolsToPrepare.Add(wkSpool.WorkstatusSpoolID);
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
                    if (lstSpoolsToPrepare.Count > 0)
                    {
                        EmbarqueBO.Instance.PrepararSpools(lstSpoolsToPrepare.ToArray(),0, SessionFacade.UserId, Convert.ToDateTime(model.ProcessDate));
                    }

                    model.ControlNumberInvalidDate.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.FechaInvalida);
                    model.ControlNumberNotConditions.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.NoCumple);
                    model.ControlNumberWhitProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.ConProceso);
                    model.ControlNumberToProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.Procesado);

                    NavContext.SetNumbersControl(Helps.GetAllNumbersControlModelToString(model));

                    if (model.ControlNumberInvalidDate.Count > 0 || model.ControlNumberWhitProcess.Count > 0 || model.ControlNumberNotConditions.Count > 0)
                    {                        
                        return View("PreparationResults", model);
                    }
                    else
                    {
                        NavContext.SetNumbersControl(string.Empty);
                        exit = true;
                    }
                }
                catch (ExcepcionEmbarque e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault().Replace("<br />", string.Empty));

                    return View("Index", GetSearchModel(exit));
                }
            }

            return View("Index", GetSearchModel(exit));
        }

        public ActionResult OverwritePreparation(WorkstatusModel model)
        {
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();

            List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
            model.ControlNumberWhitProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.ConProceso).ToList();
            model.ControlNumberToProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.Procesado).ToList();
            model.ControlNumberNotConditions = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.NoCumple).ToList();
            model.ControlNumberInvalidDate = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.FechaInvalida).ToList();

            IQueryable<int> otsIds = model.ControlNumberWhitProcess.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();
            IQueryable<WorkstatusSpool> workstatusSpools = WorkstatusSpoolBO.Instance.ObtenerWorkstatus(otsIds.ToArray()).AsQueryable();
            bool exit = false;

            List<int> lstSpoolsToPrepare = new List<int>();

            foreach (NumeroControlBusqueda ncb in model.ControlNumberWhitProcess)
            {
                WorkstatusSpool wkSpool = workstatusSpools.Where(x => x.OrdenTrabajoSpoolID == ncb.OrdenTrabajoSpoolID).FirstOrDefault();

                if (wkSpool != null)
                {
                    lstSpoolsToPrepare.Add(wkSpool.WorkstatusSpoolID);
                }
            }

            try
            {
                if (lstSpoolsToPrepare.Count() > 0)
                {
                    EmbarqueBO.Instance.PrepararSpools(lstSpoolsToPrepare.ToArray(), 0, SessionFacade.UserId,Convert.ToDateTime(model.ProcessDate));
                }

                NavContext.SetNumbersControl(string.Empty);
                exit = true;
            }
            catch (ExcepcionEmbarque e)
            {
                ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault());
                return View("Index", GetSearchModel(exit));
            }

            return View("Index", GetSearchModel(exit));
        }

        #endregion


        private SearchControlNumberModel GetSearchModel(bool exit)
        {
            List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
            SearchControlNumberModel searchNumberControl = new SearchControlNumberModel();
            searchNumberControl.ProjectId = NavContext.GetCurrentProject().ID;
            searchNumberControl.Spools = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.AProcesar).ToList();
            searchNumberControl.Exit = exit;

            return searchNumberControl;
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

        public List<NumeroControlBusqueda> GetCurrentControlNumbers()
        {
            List<NumeroControlBusqueda> SpoolsAgregados = new List<NumeroControlBusqueda>();


            string spools = NavContext.GetCurrentNumbersControl();


            if (!string.IsNullOrEmpty(spools))
            {
                string[] lstSpools = spools.Substring(0, spools.Length - 1).Split(',');


                foreach (string spool in lstSpools)
                {
                    string[] camposSpool = spool.Substring(0, spool.Length).Split('.');
                    NumeroControlBusqueda ncb = new NumeroControlBusqueda()
                    {
                        SpoolID = Convert.ToInt32(camposSpool[0]),
                        Spool = camposSpool[1],
                        NumeroControl = camposSpool[2],
                        OrdenTrabajoSpoolID = Convert.ToInt32(camposSpool[3]),
                        ProyectoID = Convert.ToInt32(camposSpool[4])
                    };


                    SpoolsAgregados.Add(ncb);
                }
            }


            return SpoolsAgregados;
        }
        
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

        public ActionResult DeletePreparationShipmentResults(int id, WorkstatusModel model)
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

            return View("PreparationShipmentResults", model);
        }
    }
}
