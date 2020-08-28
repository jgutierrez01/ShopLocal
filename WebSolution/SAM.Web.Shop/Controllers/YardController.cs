using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Resources.Controllers;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Cache;
using SAM.Web.Common;
using SAM.Web.Shop.Models;
using SAM.Web.Shop.Utils;
using SAM.Entities.Busqueda;
using System.Security;
using System.IO;
using System;
using SAM.Web.Shop.Filters;


namespace SAM.Web.Shop.Controllers
{
    public class YardController : AuthenticatedController
    {
        public YardController(INavigationContext navContext) : base(navContext) { }


        [HttpGet]
        public ActionResult Index(int? yardId, int? projectId)
        {

            //int PerfilID = 0, total = 0;
            //List<int> ListaPerfilesCalidad = new List<int>();
            //ListaPerfilesCalidad.Add(6);
            //ListaPerfilesCalidad.Add(58);
            //ListaPerfilesCalidad.Add(59);
            //ListaPerfilesCalidad.Add(62);
            //ListaPerfilesCalidad.Add(67);
            //ListaPerfilesCalidad.Add(72);
            //ListaPerfilesCalidad.Add(76);
            //ListaPerfilesCalidad.Add(87);
            try
            {
                NavContext.SetNumbersControl(string.Empty);
                
                if (yardId.HasValue)
                {
                    NavContext.SetYard(yardId.Value);
                }
                if (projectId.HasValue)
                {
                    NavContext.SetProject(projectId.Value);
                }

                if (NavContext.GetCurrentProject().ID > 0)
                {                                        
                    //try
                    //{
                    //    PerfilID = int.Parse(Session["PerfilID"].ToString());
                    //}
                    //catch (Exception)
                    //{
                    //    PerfilID = 0;
                    //}
                   
                    //if(PerfilID > 0)
                    //{
                    //    try
                    //    {
                    //        total = (from a in ListaPerfilesCalidad where a == PerfilID select a).ToList().Count;
                    //    }
                    //    catch (Exception)
                    //    {
                    //        total = 0;
                    //    }                        
                    //}
                    
                    //if(total > 0)
                    //{
                    //    return RedirectToAction("Index", "Incidencias");
                    //}
                    //else
                    //{
                        SearchControlNumberModel modelNw = new SearchControlNumberModel()
                        {
                            ProjectId = NavContext.GetCurrentProject().ID,
                            Spools = new List<NumeroControlBusqueda>(),
                            DateProcess = DateTime.Now.ToShortDateString(),
                            NumberProcess = string.Empty,
                            Exit = false
                        };
                        return View(modelNw);
                    //}                    
                }
            }
            catch (InvalidDataException e)
            {
                //ModelState.AddModelError("ProjectId", e.Message);
                //try
                //{
                //    PerfilID = int.Parse(Session["PerfilID"].ToString());
                //}
                //catch (Exception)
                //{
                //    PerfilID = 0;
                //}
                //if (PerfilID > 0)
                //{

                //    try
                //    {
                //        total = (from a in ListaPerfilesCalidad where a == PerfilID select a).ToList().Count;
                //    }
                //    catch (Exception)
                //    {
                //        total = 0;
                //    }

                //}

                //if (total > 0)
                //{
                //    return RedirectToAction("Index", "Incidencias");
                //}
                //else
                //{
                    return View();
                //}
            }
            catch (SecurityException e)
            {
                //ModelState.AddModelError("ProjectId", e.Message);
                //try
                //{
                //    PerfilID = int.Parse(Session["PerfilID"].ToString());
                //}
                //catch (Exception)
                //{
                //    PerfilID = 0;
                //}
                //if (PerfilID > 0)
                //{

                //    try
                //    {
                //        total = (from a in ListaPerfilesCalidad where a == PerfilID select a).ToList().Count;
                //    }
                //    catch (Exception)
                //    {
                //        total = 0;
                //    }

                //}

                //if (total > 0)
                //{
                //    return RedirectToAction("Index", "Incidencias");
                //}
                //else
                //{
                    return View();
                //}
            }          
                        
            //try
            //{
            //    PerfilID = int.Parse(Session["PerfilID"].ToString());
            //}
            //catch (Exception)
            //{
            //    PerfilID = 0;
            //}            
            //if (PerfilID > 0)
            //{

            //    try
            //    {
            //        total = (from a in ListaPerfilesCalidad where a == PerfilID select a).ToList().Count;
            //    }
            //    catch (Exception)
            //    {
            //        total = 0;
            //    }

            //}

            //if (total > 0)
            //{
            //    return RedirectToAction("Index", "Incidencias");
            //}
            //else
            //{
                return View();
            //}            
        }

        [HttpGet]
        public ActionResult Search(SearchControlNumberModel model)
        {
            if (ModelState.IsValid)
            {
                ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectId);
                

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
                            
                            if (model.TypeSearch == 2)
                            {
                                return RedirectToAction("CertificationReports", "ControlNumber", new { controlNumberId = controlNumberId[0] });
                            }
                            else
                            {
                              
                                return RedirectToAction("SummaryShop", "ControlNumber", new { controlNumberId = controlNumberId[0] });
                            }
                        }


                        ModelState.AddModelError(string.Empty, YardStrings.ControlNumber_ErrorMessage_DoesNotExist);


                        break;
                    case "sp":
                        int totalRows;

                        OrdenTrabajoSpoolBO.Instance.BuscarPorNombreDeSpool(model.SpoolName, project.ID, 0, 100, out totalRows);
                        
                        if (totalRows == 0)
                        {
                            ModelState.AddModelError(string.Empty, YardStrings.Search_NoResults);
                        }
                        else
                        {
                            NavContext.SetProject(project.ID);
                            return RedirectToAction("SpoolSearch", "Yard", new { spoolName = model.SpoolName });                           
                        }
                        
                        break;
                    default:
                        ModelState.AddModelError(string.Empty, YardStrings.Search_InvalidType);
                        break;
                }
            }


            return View("Index", model);
        }


        [HttpGet]
        public ActionResult SpoolSearch(string spoolName, int? pageNumber, int? pageSize)
        {
            int totalRows;


            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }


            if (!pageSize.HasValue)
            {
                pageSize = 10;
            }


            int skip = (pageNumber.Value - 1) * pageSize.Value;


            List<NumeroControlBusqueda> results = OrdenTrabajoSpoolBO.Instance.BuscarPorNombreDeSpool(spoolName, NavContext.GetCurrentProject().ID, skip, pageSize.Value, out totalRows);
            PagedResult<NumeroControlBusqueda> page = new PagedResult<NumeroControlBusqueda>(results, pageSize.Value, pageNumber.Value, totalRows);


            return View("SpoolSearch", new SearchSpoolResultsModel { Results = page, Spool = spoolName });
        }


        [HttpGet]
        [ActionName("Spools")]
        public ActionResult GetSpools(string spoolName, int? pageNumber, int? pageSize)
        {
            int totalRows;


            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }


            if (!pageSize.HasValue)
            {
                pageSize = 10;
            }


            int skip = (pageNumber.Value - 1) * pageSize.Value;


            List<NumeroControlBusqueda> results = OrdenTrabajoSpoolBO.Instance.BuscarPorNombreDeSpool(spoolName, NavContext.GetCurrentProject().ID, skip, pageSize.Value, out totalRows);


            return PartialView("_SpoolSearchResults", results);
        }
    }
}
