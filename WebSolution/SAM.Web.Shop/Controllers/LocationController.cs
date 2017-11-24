using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using log4net;
using Mimo.Framework.Exceptions;
using Resources.Controllers;
using Resources.Models;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using SAM.Entities.Busqueda;
using SAM.Entities.Cache;
using SAM.Entities.Personalizadas;
using SAM.Web.Common;
using SAM.Web.Shop.Filters;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Resources.Views.WorkStatus;
using SAM.Web.Shop.Utils;

namespace SAM.Web.Shop.Controllers
{
    //[IsAuthorized]
    public class LocationController : AuthenticatedController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(LocationController));
        public LocationController(INavigationContext navContext) : base(navContext) { }

        #region location physical

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
        public ActionResult CancelLocation()
        {
            NavContext.SetNumbersControl(string.Empty);
            return View("Index", GetSearchModel(false));
        }


        [HttpGet]
        public ActionResult AddLocation(SearchControlNumberModel search, LocationModel location)
        {          
            if (ModelState.IsValid)
            {              
                ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == search.ProjectId);
                search.Spools = new List<NumeroControlBusqueda>();
                search.Spools.AddRange(Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl()));

                switch (search.SearchType)
                {
                    case "cn":

                        List<string> controlNumbers = new List<string>(); 
                        
                        for (int i = 1; i <= project.DigitosOdt; i++ ) 
                        { 
                           controlNumbers.Add( project.PrefijoOdt + search.WorkOrderNumber.ToString().PadLeft(i, '0') + 
                                                   "-" + search.ControlNumber.ToString().PadLeft(3, '0')); 
                        }

                        List<int> controlNumberId = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolIDsPorNumeroDeControlYProyecto(controlNumbers, project.ID);

                        if (controlNumberId.Count > 0)
                        {
                            NavContext.SetProject(project.ID);
                            OrdenTrabajoSpool ots = OrdenTrabajoSpoolBO.Instance.Obtener(controlNumberId[0]);
                            NumeroControlBusqueda ncb = OrdenTrabajoSpoolBO.Instance.BuscarPorIdSpool(ots.SpoolID, search.ProjectId);
                            
                            if (!search.Spools.Where(x => x.SpoolID == ncb.SpoolID).Any())
                            {
                                string spools = NavContext.GetCurrentNumbersControl();
                                spools = Helps.GetSpoolCookies(ncb, search.Spools);

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

                         List<NumeroControlBusqueda> ncbs = OrdenTrabajoSpoolBO.Instance.BuscarPorNombreDeSpool(search.SpoolName, project.ID);


                        if (ncbs.Count == 0)
                        {
                            ModelState.AddModelError(string.Empty, YardStrings.Search_NoResults);
                        }
                        else
                        {
                            NavContext.SetProject(project.ID);            
          
                            string spools = NavContext.GetCurrentNumbersControl();
                            spools = Helps.GetSpoolsCookies(ncbs, search.Spools);

                            NavContext.SetNumbersControl(spools);

                            _logger.Info("add spool: " + search.SpoolName);                          
                        }

                        break;

                    default:
                        ModelState.AddModelError(string.Empty, YardStrings.Search_InvalidType);
                        break;
                }
            }
            search.QuadrantId = location.QuadrantId;
            search.Exit = false;
            return View("Index", search);
        }


        public ActionResult DeleteLocation(int id)
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
        public ActionResult SaveLocation(LocationModel model)
        {           
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();
           
            bool exit = false;

            if (model.QuadrantId == 0)
            {
                try
                {
                    throw new ExcepcionReportes(string.Format(SearchStrings.QuadrantName_Required_ErrorMessage));                   
                }
                catch (BaseValidationException e)
                {
                     ModelState.AddModelError("QuadrantId", e.Details.FirstOrDefault());
                }
            }

            if (ModelState.IsValid && ValidaModel(model))
            {
                List<NumeroControlBusqueda> currentControlNumbers = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
                
                //ya tienen cuadrante asignado
                model.ControlNumberWhitProcess =  currentControlNumbers.Where(x =>  x.CuadranteId != 0).ToList();
                model.ControlNumberWhitProcess.ToList().ForEach(c => c.TipoNC = TipoNumeroControlEnum.ConProceso);             
                
                //seran procesados
                model.ControlNumberToProcess = currentControlNumbers.Where(x => x.CuadranteId == 0).ToList();

                List<int> lstSpoolsToProcess = model.ControlNumberToProcess.Select(x => x.OrdenTrabajoSpoolID).ToList();

                try
                {
                    if (lstSpoolsToProcess.Count > 0)
                    {
                        CuadranteBO.Instance.GuardarCuadranteSpools(model.QuadrantId, lstSpoolsToProcess.ToArray(), Convert.ToDateTime(model.ProcessDate),SessionFacade.UserId);
                    }

                    List<NumeroControlBusqueda> spoolsActualizados = new List<NumeroControlBusqueda>();

                    foreach(NumeroControlBusqueda ncb in model.ControlNumberToProcess)
                    {
                       NumeroControlBusqueda ncbActualizado = OrdenTrabajoSpoolBO.Instance.BuscarPorIdSpool(ncb.SpoolID, ncb.ProyectoID);
                       ncbActualizado.TipoNC = TipoNumeroControlEnum.Procesado;
                       spoolsActualizados.Add(ncbActualizado);                        
                    }

                    model.ControlNumberToProcess = spoolsActualizados;

                    NavContext.SetNumbersControl(Helps.GetAllNumbersControlModelToString(model));

                    if (model.ControlNumberWhitProcess.Count > 0 )
                    {                       
                        return View("LocationResults", model);
                    }
                    else
                    {
                        NavContext.SetNumbersControl(string.Empty);
                        exit = true;
                    }
                }
                catch (BaseValidationException e)
                {
                    ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault());
                    return View("Index", GetSearchModel(exit));
                }
            }

            return View("Index", GetSearchModel(exit));
        }

        public ActionResult OverwriteLocation(LocationModel model)
        {
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();

            bool exit = false;

            if (ModelState.IsValid && ValidaModel(model))
            {
                List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
                model.ControlNumberWhitProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.ConProceso).ToList();
                model.ControlNumberToProcess = ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.Procesado).ToList();

                List<int> lstSpoolsToProcess = new List<int>();

                lstSpoolsToProcess.AddRange(model.ControlNumberWhitProcess.Select(x => x.OrdenTrabajoSpoolID).ToList());

                try
                {
                    if (lstSpoolsToProcess.Count > 0)
                    {
                        CuadranteBO.Instance.GuardarCuadranteSpools(model.QuadrantId, lstSpoolsToProcess.ToArray(), Convert.ToDateTime(model.ProcessDate), SessionFacade.UserId);
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.Message);

                    return View("Index", GetSearchModel(exit));
                }

                NavContext.SetNumbersControl(string.Empty);
                exit = true;
            }

            return View("Index", GetSearchModel(exit));
        }

        #endregion

        private bool ValidaModel(LocationModel model)
        {
            bool isValid = true;

            //if (model.QuadrantId == 0)
            //{
            //    ModelState.AddModelError(string.Empty, WorkStatusStrings.QuadrantId_Required_ErrorMessage);
            //    isValid = false;
            //}

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
          
            searchNumberControl.Spools = ncbs.ToList();
            searchNumberControl.Exit = exit;

            return searchNumberControl;
        }

        public ActionResult DeleteLocationResults(int id, LocationModel model)
        {
            model.ControlNumberToProcess = new List<NumeroControlBusqueda>();
            model.ControlNumberWhitProcess = new List<NumeroControlBusqueda>();

            List<NumeroControlBusqueda> ncbs = Helps.GeControlNumbersStringToNCB(NavContext.GetCurrentNumbersControl());
           
            NumeroControlBusqueda ncb = ncbs.Where(x => x.SpoolID == id).FirstOrDefault();

            if (ncb != null)
            {
                ncbs.Remove(ncb);
            }

            NavContext.SetNumbersControl(Helps.GetSpoolsCookie(ncbs));

            model.ControlNumberWhitProcess.AddRange(ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.ConProceso).ToList());
            model.ControlNumberToProcess.AddRange(ncbs.Where(x => x.TipoNC == TipoNumeroControlEnum.Procesado).ToList());
                      
            return View("LocationResults", model);
        }
    }
}
