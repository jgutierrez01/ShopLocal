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
    public class RequisitionPaintingController : AuthenticatedController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(RequisitionPaintingController));

        public RequisitionPaintingController(INavigationContext navContext) : base(navContext) { }


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
        public ActionResult AddRequisitionPainting(SearchControlNumberModel model, WorkstatusModel shipment)
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


        [HttpGet]
        public ActionResult CancelRequisitionPainting()
        {
            NavContext.SetNumbersControl(string.Empty);
            return View("Index", GetSearchModel(false));
        }


        public ActionResult DeleteRequisitionPainting(int id)
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
        public ActionResult SaveRequisitionPainting(WorkstatusModel model)
        {
            model.ControlNumberInvalidDate = new List<NumeroControlBusqueda>();
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberNotConditions = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();

            string lang = CultureInfo.CurrentCulture.Name;
            bool exit = false;

            if (ValidateModel(model, 1))
            {
                List<NumeroControlBusqueda> currentControlNumbers = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
             
                IQueryable<int> otsIds = currentControlNumbers.Select(x => x.OrdenTrabajoSpoolID).AsQueryable();

                //get WorkstatusSpool
                IQueryable<WorkstatusSpool> workstatusSpools = WorkstatusSpoolBO.Instance.ObtenerWorkstatus(otsIds.ToArray()).AsQueryable();
                currentControlNumbers.ToList().ForEach(x => x.TieneWorkstatusSpool = workstatusSpools.Where(w => w.OrdenTrabajoSpoolID == x.OrdenTrabajoSpoolID).Select(w => w.WorkstatusSpoolID).FirstOrDefault());
                  
                //control number have subsequent process
                int[] withsubsequentProcess = workstatusSpools.Where(x => x.TienePintura).Select(x => x.OrdenTrabajoSpoolID).ToArray();
                model.ControlNumberNotConditions.AddRange(currentControlNumbers.Where(x => withsubsequentProcess.Contains(x.OrdenTrabajoSpoolID)));
             
                //Date Validation         
                string datesDimension = ValidaFechasBO.Instance.ObtenerFechasDimensionalesPorOts(currentControlNumbers.Select(x => x.OrdenTrabajoSpoolID).ToArray());
                List<int> whitDateNotValid = ValidateDateRequisition(currentControlNumbers.Select(x => x.OrdenTrabajoSpoolID).ToArray(), datesDimension, Convert.ToDateTime(model.ProcessDate));
                
                //control numbers with not valid date
                model.ControlNumberInvalidDate.AddRange(currentControlNumbers.Where(x => whitDateNotValid.Contains(x.OrdenTrabajoSpoolID)));
                workstatusSpools = workstatusSpools.Where(x => !whitDateNotValid.Contains(x.OrdenTrabajoSpoolID));
                 
                // control numbers are processed
                model.ControlNumberToProcess = currentControlNumbers.Where(x => !whitDateNotValid.Contains(x.OrdenTrabajoSpoolID) && !withsubsequentProcess.Contains(x.OrdenTrabajoSpoolID)).ToList();

                List<int> lstSpoolsToPrepare = new List<int>();

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
                   
                    if (wkSpool.TieneRequisicionPintura)
                    {
                        model.ControlNumberWhitProcess.Add(ncb);
                    }
                    else
                    {
                        bool fechaValida = ValidaFechasBO.Instance.ValidaFechaPintura(Convert.ToDateTime(model.ProcessDate), wkSpool.WorkstatusSpoolID);

                        if (!fechaValida)
                        {
                            model.ControlNumberInvalidDate.Add(ncb);
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
                        RequisicionPinturaBO.Instance.GeneraRequisicionShop(model.ProjectId, model.ProcessNumber, Convert.ToDateTime(model.ProcessDate), lstSpoolsToPrepare.ToArray(), SessionFacade.UserId);
                    }

                    model.ControlNumberInvalidDate.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.FechaInvalida);
                    model.ControlNumberNotConditions.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.NoCumple);
                    model.ControlNumberWhitProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.ConProceso);
                    model.ControlNumberToProcess.ToList().ForEach(x => x.TipoNC = TipoNumeroControlEnum.Procesado);

                    NavContext.SetNumbersControl(Helps.GetAllNumbersControlModelToString(model));

                    if (model.ControlNumberInvalidDate.Count > 0 || model.ControlNumberWhitProcess.Count > 0 || model.ControlNumberNotConditions.Count > 0)
                    {                 
                        return View("RequisitionPaintingResults", model);
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


                    return View("RequisitionPainting", GetSearchModel(exit));
                }
                catch (ExcepcionReportes e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault().Replace("<br />", string.Empty));


                    return View("Index", GetSearchModel(exit));
                }
            }

            return View("Index", GetSearchModel(exit));
        }

        public ActionResult OverwriteRequisitionPainting(WorkstatusModel model)
        {
            bool exit = false;
            if (ModelState.IsValid)
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
                    if (lstSpoolsToPrepare.Count > 0)
                    {
                        string lang = CultureInfo.CurrentCulture.Name;
                        RequisicionPinturaBO.Instance.GeneraRequisicionShop(model.ProjectId, model.ProcessNumber, Convert.ToDateTime(model.ProcessDate), lstSpoolsToPrepare.ToArray(), SessionFacade.UserId);
                    }

                    NavContext.SetNumbersControl(string.Empty);
                    exit = true;
                }
                catch (ExcepcionPintura e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault().Replace("<br />", string.Empty));
                    model.ControlNumberWhitProcess = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
                    return View("Index", GetSearchModel(exit));
                }
                catch (ExcepcionReportes e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault().Replace("<br />", string.Empty));
                    model.ControlNumberWhitProcess = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
                    return View("Index", GetSearchModel(exit));
                }
            }
            return View("Index", GetSearchModel(exit));
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

        private List<int> ValidateDateRequisition(int[] otSpoolIds, string datesDimentional, DateTime dateProcess)
        {
            string[] lstDatesDimentional = datesDimentional.Substring(0, datesDimentional.Length).Split(',');
            List<int> otSpoolsNoValid = new List<int>();

            for (int i = 0; i < otSpoolIds.Count(); i++)
            {
                DateTime? dateDimensional = null;

                if (CultureInfo.CurrentCulture.Name.Equals("es-MX"))
                {
                    string[] partesDate = lstDatesDimentional[i].Split('/');
                    lstDatesDimentional[i] = partesDate[1] + "/" + partesDate[0] + "/" + partesDate[2];
                }

                if (!string.IsNullOrEmpty(lstDatesDimentional[i]))
                {
                    dateDimensional = Convert.ToDateTime(lstDatesDimentional[i]);
                }

                if (dateDimensional.HasValue)
                {
                    if (dateProcess < dateDimensional)
                    {
                        otSpoolsNoValid.Add(otSpoolIds[i]);
                    }
                }
            }

            return otSpoolsNoValid;
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

        public ActionResult DeleteRequisitionPaintingResults(int id, WorkstatusModel model)
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

            return View("RequisitionPaintingResults", model);
        }

       

    }
}
