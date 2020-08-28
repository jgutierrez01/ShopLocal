using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SAM.Web.Shop.Utils;
using SAM.Web.Shop.Models;
using SAM.Entities.Cache;
using SAM.Web.Common;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Busqueda;
using SAM.Entities.Personalizadas.Shop;
using Newtonsoft.Json;

namespace SAM.Web.Shop.Controllers
{
    public class SQController : AuthenticatedController
    {
        public SQController(INavigationContext navContext) : base(navContext) { }
        // GET: SQ
        public ActionResult Index()
        {
            SQModel sqmodel = new SQModel();
            try
            {
                if (NavContext.GetDataFromSession<string>(Session, "Vista") != "" && NavContext.GetDataFromSession<string>(Session, "Vista") != null)
                {
                    sqmodel.SeleccionAgregarEditar = NavContext.GetDataFromSession<string>(Session, "Vista");
                } else
                {
                    sqmodel.SeleccionAgregarEditar = "1";
                }
                //sqmodel.SeleccionAgregarEditar = "1";
                sqmodel.ViewFormAdd = true;
                sqmodel.ViewFormEdit = false;
                sqmodel.ViewGridAdd = false;
                sqmodel.ViewGridEdit = false;
                NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                if (NavContext.GetCurrentProject().ID > 0)
                {
                    sqmodel.ProjectIdADD = NavContext.GetCurrentProject().ID;
                    sqmodel.ProjectIdEditar = NavContext.GetCurrentProject().ID;
                }
                if (NavContext.getCuadranteID() != "")
                {
                    sqmodel.CuadranteID = int.Parse(NavContext.getCuadranteID());
                }
            }
            catch (Exception e)
            {
                return View(sqmodel);
            }

            return View(sqmodel);
        }

        [HttpGet]
        public ActionResult AddNC(SQModel model)
        {
            NavContext.SetDataToSession<string>(Session, "Vista", "1");
            model.SeleccionAgregarEditar = "1";
            if (model.QuadrantIdCADD == 0)
            {
                if (model.QuadrantIdNCADD == 0)
                {
                    if (model.QuadrantIdCEdit == 0)
                    {
                        if (model.QuadrantIdNCEdit != 0)
                        {
                            model.CuadranteID = model.QuadrantIdNCEdit;
                        }
                    }
                    else
                    {
                        model.CuadranteID = model.QuadrantIdCEdit;
                    }
                }
                else
                {
                    model.CuadranteID = model.QuadrantIdNCADD;
                }
            }
            else
            {
                model.CuadranteID = model.QuadrantIdCADD;
            }
            if (model.CuadranteID == 0)
            {
                if (NavContext.getCuadranteID() != "" && NavContext.getCuadranteID() != null)
                {
                    model.CuadranteID = int.Parse(NavContext.getCuadranteID());
                }
            }
            else
            {
                NavContext.setCuadranteID(model.CuadranteID.ToString());
            }

            ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdADD);
            if (NavContext.GetCurrentProjectSQ() == null || (model.ProjectIdADD != NavContext.GetCurrentProjectSQ().ID))
            {
                NavContext.SetProject(project.ID);
                NavContext.SetProjectEdit(project.ID);
                NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                model.ListaElementos = new List<LayoutGridSQ>();
            }
            else
            {
                model.ListaElementos = new List<LayoutGridSQ>();
                List<LayoutGridSQ> Lista = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd"));
                if (Lista == null)
                {
                    Lista = new List<LayoutGridSQ>();
                }
                model.ListaElementos.AddRange(Lista);
                if (model.ListaElementos.Count > 0) model.TieneDatosGridAdd = true;
            }

            switch (model.SearchTypeADD)
            {
                case "nc":

                    if (model.WorkOrderNumberADD == 0 || model.WorkOrderNumberADD == null)
                    {
                        TempData["MsgError"] += "Ingrese Orden de Trabajo <br>";
                    }
                    if (model.ControlNumberADD == 0 || model.ControlNumberADD == null)
                    {
                        TempData["MsgError"] += "Ingrese Numero De Control <br>";
                    }
                    if (model.QuadrantIdNCADD == 0 || model.QuadrantIdNCADD.ToString() == null)
                    {
                        TempData["MsgError"] += "Seleccione Un Cuadrante <br>";
                    }
                    if (TempData["MsgError"] == null)
                    {
                        List<string> controlNumbers = new List<string>();
                        for (int i = 1; i <= project.DigitosOdt; i++)
                        {
                            controlNumbers.Add(project.PrefijoOdt + model.WorkOrderNumberADD.ToString().PadLeft(i, '0') +
                                                    "-" + model.ControlNumberADD.ToString().PadLeft(3, '0'));
                        }
                        List<int> controlNumberId = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolIDsPorNumeroDeControlYProyectoSQ(controlNumbers, model.ProjectIdADD);
                        if (controlNumberId.Count > 0)
                        {

                            OrdenTrabajoSpoolSQ ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolConSQ(controlNumberId[0]);
                            CuadranteSQ cuadrante = OrdenTrabajoSpoolBO.Instance.BuscarCuadrante(model.QuadrantIdNCADD, model.ProjectIdADD);
                            if (ots != null && cuadrante != null)
                            {
                                //VALIDO EL OK FABRICACION
                                if (ots.OkFab)
                                {
                                    NavContext.setCuadranteID(cuadrante.CuadranteID.ToString());
                                    List<LayoutGridSQ> listaElementos = new List<LayoutGridSQ>();
                                    if (cuadrante != null)
                                    {
                                        listaElementos.Add(new LayoutGridSQ
                                        {
                                            Accion = 1,
                                            Cuadrante = cuadrante.Cuadrante,
                                            CuadranteID = cuadrante.CuadranteID,
                                            NumeroControl = ots.NumeroControl,
                                            SpoolID = ots.SpoolID,
                                            OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                                            SqCliente = ots.SqCliente,
                                            SQ = ots.sqinterno,
                                            TieneHoldIngenieria = ots.TieneHoldIngenieria,
                                            OkPnd = ots.OkPnd,
                                            Incidencias = ots.Incidencias,
                                            Granel = ots.Granel
                                        });
                                    }
                                    string datosAsignados = "";

                                    List<LayoutGridSQ> ListaTieneSQCliente = (from a in listaElementos where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                                    List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in listaElementos where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                                    for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                                    {
                                        datosAsignados += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SI Agregado <br>";
                                    }

                                    List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == true select a).ToList();
                                    List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == false select a).ToList();
                                    for (int i = 0; i < ListaTieneHold.Count; i++)
                                    {
                                        datosAsignados += " El Spool " + ListaTieneHold[i].NumeroControl + " se encuentra en hold  " + "<br>";
                                    }

                                    List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ != "" && a.SQ != null) select a).ToList();
                                    List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ == "" || a.SQ == null) select a).ToList();
                                    for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                                    {
                                        datosAsignados += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en la SI: " + ListaTieneSQInterno[i].SQ + "<br>";
                                    }

                                    if (datosAsignados != "")
                                    {
                                        TempData["errorAdd"] += datosAsignados;
                                    }


                                    if (ListaNoTieneSQInterno.Count > 0)
                                    {
                                        if (model.QuadrantIdNCADD != 0)
                                        {
                                            NavContext.setCuadranteID(model.QuadrantIdNCADD.ToString());
                                        }
                                        else
                                        {
                                            NavContext.setCuadranteID(model.QuadrantIdCADD.ToString());
                                        }
                                        var Lista = ListaNoTieneSQInterno;
                                        List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList();//.OrderBy(c => c.NumeroControl).ToList();
                                        string auxString = NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd");
                                        string numerosControl = "";
                                        List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                        if (ListaTmp == null)
                                        {
                                            ListaTmp = new List<LayoutGridSQ>();
                                        }
                                        if (auxString != "" && auxString != "[]" && auxString != null)
                                        {
                                            NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                                            ListaTmp.InsertRange(0, ListaSinRepetidos);
                                            var OtraLista = ListaTmp;
                                            ListaTmp = OtraLista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList();
                                            numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                            NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", numerosControl);
                                            model.ListaElementos = ListaTmp;
                                        }
                                        else
                                        {
                                            var lista = model.ListaElementos;
                                            if (lista != null)
                                            {
                                                model.ListaElementos = lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList();
                                            }
                                            string numeroControles = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementos);
                                            NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", numeroControles);
                                        }
                                        model.TieneDatosGridAdd = true;
                                    }
                                    else
                                    {
                                        model.TieneDatosGridAdd = false;
                                    }
                                }
                                else
                                {
                                    TempData["errorAdd"] += " El Spool " + ots.NumeroControl + " no tiene Ok Fabricación <br>";
                                    model.TieneDatosGridAdd = false;
                                }
                            }
                            else
                            {
                                TempData["sinDatos"] += "No se encontraron datos que coincidieran con su busqueda";
                            }
                        }                                                        
                        else
                        {
                            TempData["sinDatos"] += "No se encontraron datos que coincidieran con su busqueda";
                        }
                    }
                    break;
                case "c":
                    if (model.QuadrantIdCADD != 0)
                    {
                        NavContext.setCuadranteID(model.QuadrantIdCADD.ToString());
                        List<CuadranteNumeroControlSQ> numeroControl = OrdenTrabajoSpoolBO.Instance.BuscarPorCuadranteSQ(model.QuadrantIdCADD, model.ProjectIdADD, 1);
                        if (numeroControl != null && numeroControl.Count > 0)
                        {
                            List<LayoutGridSQ> listaElementos = new List<LayoutGridSQ>();
                            for (int i = 0; i < numeroControl.Count; i++)
                            {
                                listaElementos.Add(new LayoutGridSQ
                                {
                                    Accion = numeroControl[i].Accion,
                                    Cuadrante = numeroControl[i].Cuadrante,
                                    CuadranteID = numeroControl[i].CuadranteID,
                                    NumeroControl = numeroControl[i].NumeroControl,
                                    SpoolID = numeroControl[i].SpoolID,
                                    OrdenTrabajoSpoolID = numeroControl[i].OrdenTrabajoSpoolID,
                                    SqCliente = numeroControl[i].SqCliente,
                                    SQ = numeroControl[i].SQ,
                                    TieneHoldIngenieria = numeroControl[i].TieneHoldIngenieria,
                                    OkPnd = numeroControl[i].OkPnd,
                                    Incidencias = numeroControl[i].Incidencias,
                                    Granel = numeroControl[i].Granel,
                                    OkFab = numeroControl[i].OkFab                                                                   
                                });
                            }
                            string datosAsignados = "";

                            List<LayoutGridSQ> ListaTieneSQCliente = (from a in listaElementos where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in listaElementos where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                            for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                            {
                                datosAsignados += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SI Agregado <br>";
                            }

                            List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == true select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == false select a).ToList();
                            for (int i = 0; i < ListaTieneHold.Count; i++)
                            {
                                datosAsignados += " El Spool " + ListaTieneHold[i].NumeroControl + " se encuentra en hold  " + "<br>";
                            }

                            List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ != "" && a.SQ != null) select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ == "" || a.SQ == null) select a).ToList();
                            for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                            {
                                datosAsignados += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en la SI: " + ListaTieneSQInterno[i].SQ + "<br>";
                            }

                            List<LayoutGridSQ> ListaTieneOkFab = (from a in ListaNoTieneSQInterno where a.OkFab select a).ToList(); //Solo Agrego aquellos que tienen okfab
                            List<LayoutGridSQ> ListaNoTieneOkFab = (from a in ListaNoTieneSQInterno where !a.OkFab select a).ToList(); //Muestro warning aquellos que le falta ok fab                            
                            for (int i = 0; i < ListaNoTieneOkFab.Count; i++)
                            {
                                datosAsignados += " El Spool " + ListaNoTieneOkFab[i].NumeroControl + " Le Falta Ok Fabricación <br>";
                            }
                            

                            if (datosAsignados != "")
                            {
                                TempData["errorAdd"] += datosAsignados;
                            }
                            if (ListaTieneOkFab.Count > 0)
                            {
                                var Lista = ListaTieneOkFab;
                                List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList(); //.OrderBy(c => c.NumeroControl).ToList();                                
                                string auxString = NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd");
                                string numerosControl = "";
                                List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                if (auxString != "" && auxString != "[]" && auxString != null)
                                {
                                    NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                                    if (ListaTmp == null)
                                    {
                                        ListaTmp = new List<LayoutGridSQ>();
                                    }
                                    ListaTmp.InsertRange(0, ListaSinRepetidos);
                                    var Lista2 = ListaTmp;
                                    ListaTmp = Lista2.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList(); //.OrderBy(c => c.NumeroControl).ToList();
                                    numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                    NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", numerosControl);
                                    model.ListaElementos = ListaTmp;
                                }
                                else
                                {
                                    var lista = model.ListaElementos;
                                    if (lista != null)
                                    {
                                        model.ListaElementos = lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList();
                                    }
                                    string numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementos);
                                    NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", numeroControlCuadranteSQ);
                                }
                                model.TieneDatosGridAdd = true;
                            }
                            else
                            {
                                model.TieneDatosGridAdd = false;
                            }
                        }
                        else
                        {
                            TempData["sinDatos"] += "No se encontraron datos que coincidieran con su busqueda";
                        }
                    }
                    break;
                case "s":
                    //Spools Resueltos
                    List<AgregarSI> ListaSpoolsResueltos = JsonConvert.DeserializeObject<List<AgregarSI>>(NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos"));
                    if (ListaSpoolsResueltos != null)
                    {
                        List<AgregarSI> ListaResueltosPorProyecto = new List<AgregarSI>();
                        ListaResueltosPorProyecto = ListaSpoolsResueltos.Where(x => x.ProyectoID == model.ProjectIdADD).ToList();
                        if(ListaResueltosPorProyecto.Count > 0)
                        {
                            //Los ingreso al cuadrante seleccionado
                            CuadranteSQ cuadrantes = OrdenTrabajoSpoolBO.Instance.BuscarCuadrante(model.QuadrantIdCADD, model.ProjectIdADD);
                            List<LayoutGridSQ> ListaGeneral = new List<LayoutGridSQ>();
                            foreach (var item in ListaResueltosPorProyecto)
                            {
                                ListaGeneral.Add(new LayoutGridSQ
                                {
                                    Accion = 1,
                                    Cuadrante = cuadrantes.Cuadrante,
                                    CuadranteID = cuadrantes.CuadranteID,
                                    NumeroControl = item.NumeroControl,
                                    SpoolID = item.SpoolID,
                                    OrdenTrabajoSpoolID = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolIDPorNumeroDeControl(item.NumeroControl).GetValueOrDefault(),
                                    SqCliente = item.SqCliente,
                                    SQ = item.SI,
                                    TieneHoldIngenieria = item.Hold,
                                    OkPnd = false,
                                    Incidencias = OrdenTrabajoSpoolBO.Instance.ObtenerNumeroIncidencias(item.SpoolID),
                                    Granel = item.Granel
                                });
                            }
                            string errores = "";
                            List<LayoutGridSQ> ListaTieneSQCliente = (from a in ListaGeneral where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in ListaGeneral where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                            for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                            {
                                errores += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SI Agregado <br>";
                            }

                            List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == true select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == false select a).ToList();
                            for (int i = 0; i < ListaTieneHold.Count; i++)
                            {
                                errores += " El Spool " + ListaTieneHold[i].NumeroControl + " se encuentra en hold  " + "<br>";
                            }

                            List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ != "" && a.SQ != null) select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ == "" || a.SQ == null) select a).ToList();
                            for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                            {
                                errores += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en la SI: " + ListaTieneSQInterno[i].SQ + "<br>";
                            }

                            if (errores != "")
                            {
                                TempData["errorAdd"] += errores;
                            }

                            if (ListaNoTieneSQInterno.Count > 0)
                            {
                                var Lista = ListaNoTieneSQInterno;
                                List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList();
                                string auxString = NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd");
                                string numerosControl = "";
                                List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                if (auxString != "" && auxString != "[]" && auxString != null)
                                {
                                    NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                                    if (ListaTmp == null)
                                    {
                                        ListaTmp = new List<LayoutGridSQ>();
                                    }
                                    ListaTmp.InsertRange(0, ListaSinRepetidos);
                                    var Lista2 = ListaTmp;
                                    ListaTmp = Lista2.GroupBy(a => a.SpoolID).Select(b => b.First()).ToList();
                                    numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                    NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", numerosControl);
                                    //Limpio lista de spools generados                        
                                    List<AgregarSI> SpoolsResueltos = JsonConvert.DeserializeObject<List<AgregarSI>>(NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos"));
                                    SpoolsResueltos.RemoveAll(a => Lista.Any(b => a.ProyectoID == model.ProjectIdADD));
                                    NavContext.SetDataToSession<string>(Session, "ListaSpoolsResueltos", "");
                                    NavContext.SetDataToSession<string>(Session, "ListaSpoolsResueltos", JsonConvert.SerializeObject(SpoolsResueltos));
                                    //Agrego spools a grid
                                    model.ListaElementos = ListaTmp;
                                }
                                else
                                {
                                    var lista = model.ListaElementos;
                                    if (lista != null)
                                    {
                                        model.ListaElementos = lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList();
                                    }
                                    string numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementos);
                                    NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", numeroControlCuadranteSQ);
                                }
                                model.TieneDatosGridAdd = true;
                            }
                            else
                            {
                                model.TieneDatosGridAdd = false;
                            }
                        }
                        else
                        {
                            TempData["sinDatos"] += "No Se Encontró Spools Resueltos";
                        }                        
                    }
                    else
                    {
                        TempData["sinDatos"] += "No se encontró Spools Resueltos";
                    }
                    break;
                default:
                    break;
            }
            model.ViewFormEdit = false;
            model.ViewFormAdd = true;
            model.ViewGridEdit = false;
            if (model.ListaElementos.Count > 0)
            {
                model.ViewGridAdd = true;
            } else
            {
                model.ViewGridAdd = false;
            }

            if (project != null)
            {
                NavContext.SetProject(project.ID);
                NavContext.SetProjectEdit(project.ID);
                model.ProjectIdADD = project.ID;
                model.ProjectIdEditar = project.ID;
            }
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult EditarNC(SQModel model, string Command)
        {
            NavContext.SetDataToSession<string>(Session, "Vista", "2");
            model.SeleccionAgregarEditar = "2";
            ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdEditar);
            if (model.QuadrantIdCEdit == 0)
            {
                if (model.QuadrantIdNCEdit == 0)
                {
                    if (model.QuadrantIdCADD == 0)
                    {
                        if (model.QuadrantIdNCADD != 0)
                        {
                            model.CuadranteID = model.QuadrantIdNCADD;
                        }
                    }
                    else
                    {
                        model.CuadranteID = model.QuadrantIdCADD;
                    }
                }
                else
                {
                    model.CuadranteID = model.QuadrantIdNCEdit;
                }
            }
            else
            {
                model.CuadranteID = model.QuadrantIdCADD;
            }
            if (model.CuadranteID == 0)
            {
                if (NavContext.getCuadranteID() != "" && NavContext.getCuadranteID() != null)
                {
                    model.CuadranteID = int.Parse(NavContext.getCuadranteID());
                }
            } else
            {
                NavContext.setCuadranteID(model.CuadranteID.ToString());
            }

            NavContext.SetProject(project.ID);
            NavContext.SetProjectEdit(project.ID);

            if (NavContext.GetCurrentProjectSQEditar() == null || (model.ProjectIdEditar != NavContext.GetCurrentProjectSQ().ID))
            {
                NavContext.SetProjectEdit(project.ID);
                NavContext.SetProject(project.ID);
                NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                model.ListaElementosPorSQ = new List<LayoutGridSQ>();
            }

            //limpiar la lista sq en memoria.
            if (model.SQ != NavContext.GetSQ())
            {
                NavContext.SetSQ(model.SQ);
                NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                model.ListaElementosPorSQ = new List<LayoutGridSQ>();
            }
            else
            {

                model.ListaElementosPorSQ = new List<LayoutGridSQ>();
                List<LayoutGridSQ> Lista = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit"));
                if (Lista == null)
                {
                    Lista = new List<LayoutGridSQ>();
                }
                model.ListaElementosPorSQ.AddRange(Lista);
                if (model.ListaElementosPorSQ.Count > 0) model.TieneDatosGridEdit = true;
            }

            if (Command != null && Command.Equals("BuscarSQ"))
            {
                if (model.SQ != "" && model.SQ != null)
                {
                    List<CuadranteNumeroControlSQ> numeroControl = OrdenTrabajoSpoolBO.Instance.ObtenerNumeroControlPorSQ(model.SQ, model.ProjectIdEditar);
                    if (numeroControl.Count > 0)
                    {
                        List<LayoutGridSQ> listaElementos = new List<LayoutGridSQ>();
                        for (int i = 0; i < numeroControl.Count; i++)
                        {
                            listaElementos.Add(new LayoutGridSQ
                            {
                                Accion = numeroControl[i].Accion,
                                Cuadrante = numeroControl[i].Cuadrante,
                                CuadranteID = numeroControl[i].CuadranteID,
                                NumeroControl = numeroControl[i].NumeroControl,
                                SpoolID = numeroControl[i].SpoolID,
                                OrdenTrabajoSpoolID = numeroControl[i].OrdenTrabajoSpoolID,
                                SqCliente = numeroControl[i].SqCliente,
                                SQ = numeroControl[i].SQ,
                                TieneHoldIngenieria = numeroControl[i].TieneHoldIngenieria,
                                OkPnd = numeroControl[i].OkPnd,
                                Incidencias = numeroControl[i].Incidencias,
                                Granel = numeroControl[i].Granel

                            });
                        }
                        var Lista = listaElementos;
                        if (Lista != null)
                        {

                            List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList(); // .OrderBy(c => c.NumeroControl).ToList();                            
                            string auxString = NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit");
                            string numerosControl = "";
                            List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                            if (auxString != "" && auxString != null)
                            {
                                NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                                if (ListaTmp == null)
                                {
                                    ListaTmp = new List<LayoutGridSQ>();
                                }
                                ListaTmp.InsertRange(0, ListaSinRepetidos);
                                var Lista2 = ListaTmp;
                                ListaTmp = Lista2.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList(); //.OrderBy(c => c.NumeroControl).ToList();
                                //numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                numerosControl = JsonConvert.SerializeObject(ListaSinRepetidos);
                                NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", numerosControl);
                                model.ListaElementosPorSQ = ListaSinRepetidos;
                                //model.ListaElementosPorSQ = ListaTmp;                                
                            }
                            else
                            {
                                string numeroControles = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementosPorSQ);
                                NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", numeroControles);
                            }
                            model.TieneDatosGridEdit = true;
                        }
                    }
                    else
                    {
                        TempData["sinDatos"] = "No se encontraron datos que coincidieran con su busqueda";
                    }
                }
                else
                {
                    TempData["FaltaSQ"] = "Ingrese Sol. Inspec";
                }
            }
            else
            {
                //Agregar
                //if (model.SQ != null && model.SQ != "")
                //{
                switch (model.SearchTypeEdit)
                {
                    case "nc":

                        if (model.WorkOrderNumberEdit == 0 || model.WorkOrderNumberEdit == null)
                        {
                            TempData["MsgError"] += "Ingrese Orden de Trabajo <br>";
                        }
                        if (model.ControlNumberEDIT == 0 || model.ControlNumberEDIT == null)
                        {
                            TempData["MsgError"] += "Ingrese Numero De Control <br>";
                        }
                        if (model.QuadrantIdNCEdit == 0 || model.QuadrantIdNCEdit.ToString() == null)
                        {
                            TempData["MsgError"] += "Seleccione Un Cuadrante <br>";
                        }
                        if (TempData["MsgError"] == null)
                        {
                            List<string> controlNumbers = new List<string>();
                            for (int i = 1; i <= project.DigitosOdt; i++)
                            {
                                controlNumbers.Add(project.PrefijoOdt + model.WorkOrderNumberEdit.ToString().PadLeft(i, '0') +
                                                        "-" + model.ControlNumberEDIT.ToString().PadLeft(3, '0'));
                            }
                            List<int> controlNumberId = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolIDsPorNumeroDeControlYProyectoSQ(controlNumbers, model.ProjectIdEditar);
                            if (controlNumberId.Count > 0)
                            {
                                OrdenTrabajoSpoolSQ ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolConSQ(controlNumberId[0]);
                                CuadranteSQ cuadrante = OrdenTrabajoSpoolBO.Instance.BuscarCuadrante(model.QuadrantIdNCEdit, model.ProjectIdEditar);
                                List<LayoutGridSQ> listaElementos = new List<LayoutGridSQ>();
                                if (cuadrante != null)
                                {
                                    listaElementos.Add(new LayoutGridSQ
                                    {
                                        //Accion = 1,
                                        Accion = 2,
                                        Cuadrante = cuadrante.Cuadrante,
                                        CuadranteID = cuadrante.CuadranteID,
                                        NumeroControl = ots.NumeroControl,
                                        SpoolID = ots.SpoolID,
                                        OrdenTrabajoSpoolID = ots.OrdenTrabajoSpoolID,
                                        SqCliente = ots.SqCliente,
                                        SQ = ots.sqinterno,
                                        TieneHoldIngenieria = ots.TieneHoldIngenieria,
                                        OkPnd = ots.OkPnd,
                                        Incidencias = ots.Incidencias,
                                        Granel = ots.Granel
                                    });
                                }
                                if (ots.OkFab)
                                {
                                    string datosAsignados = "";
                                    List<LayoutGridSQ> listaSoloNumeroControlNuevos = (from p in listaElementos
                                                                                       where !(from ex in model.ListaElementosPorSQ
                                                                                               select ex.OrdenTrabajoSpoolID).Contains(p.OrdenTrabajoSpoolID)
                                                                                       select p).ToList();

                                    List<LayoutGridSQ> ListaTieneSQCliente = (from a in listaSoloNumeroControlNuevos where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                                    List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in listaSoloNumeroControlNuevos where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                                    for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                                    {
                                        datosAsignados += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SI Agregado <br>";
                                    }

                                    List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == true select a).ToList();
                                    List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == false select a).ToList();
                                    for (int i = 0; i < ListaTieneHold.Count; i++)
                                    {
                                        datosAsignados += " El Spool " + ListaTieneHold[i].NumeroControl + " se encuentra en hold  " + "<br>";
                                    }

                                    List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ != "" && a.SQ != null) select a).ToList();
                                    List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ == "" || a.SQ == null) select a).ToList();
                                    for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                                    {
                                        datosAsignados += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en la SI: " + ListaTieneSQInterno[i].SQ + "<br>";
                                    }


                                    if (datosAsignados != "")
                                    {
                                        TempData["errorEditar"] += datosAsignados;
                                    }

                                    if (ListaNoTieneSQInterno.Count > 0)
                                    {

                                        var Lista = ListaNoTieneSQInterno;
                                        List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList();//.OrderBy(c => c.NumeroControl).ToList();
                                        string auxString = NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit");
                                        string numerosControl = "";
                                        List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                        if (ListaTmp == null)
                                        {
                                            ListaTmp = new List<LayoutGridSQ>();
                                        }
                                        if (auxString != "" && auxString != "[]" && auxString != null)
                                        {
                                            NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                                            ListaTmp.InsertRange(0, ListaSinRepetidos);
                                            var miLista = ListaTmp;
                                            ListaTmp = miLista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList();
                                            numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                            NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", numerosControl);
                                            model.ListaElementosPorSQ = ListaTmp;
                                        }
                                        else
                                        {
                                            string numeroControles = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementosPorSQ);
                                            NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", numeroControles);
                                        }
                                        model.TieneDatosGridEdit = true;
                                    }
                                    else
                                    {
                                        if (TempData["errorEditar"] == null)
                                        {
                                            TempData["Repetido"] = "El Spool " + listaElementos[0].NumeroControl + " ya existe en el Grid";
                                        }
                                        model.TieneDatosGridEdit = false;
                                    }
                                }
                                else
                                {
                                    TempData["errorEditar"] += "El Spool: <b>" + ots.NumeroControl + "</b> no tiene Ok Fabricación";                                    
                                }
                            }
                            else
                            {
                                TempData["sinDatos"] += "No se encontraron datos que coincidieran con su busqueda";
                            }
                        }
                        break;
                    case "c":
                        if (model.QuadrantIdCEdit != 0)
                        {
                            List<CuadranteNumeroControlSQ> numeroControl = OrdenTrabajoSpoolBO.Instance.BuscarPorCuadranteSQ(model.QuadrantIdCEdit, model.ProjectIdEditar, 2);
                            if (numeroControl.Count > 0)
                            {
                                List<LayoutGridSQ> listaElementos = new List<LayoutGridSQ>();
                                for (int i = 0; i < numeroControl.Count; i++)
                                {
                                    listaElementos.Add(new LayoutGridSQ
                                    {
                                        Accion = numeroControl[i].Accion,
                                        Cuadrante = numeroControl[i].Cuadrante,
                                        CuadranteID = numeroControl[i].CuadranteID,
                                        NumeroControl = numeroControl[i].NumeroControl,
                                        SpoolID = numeroControl[i].SpoolID,
                                        OrdenTrabajoSpoolID = numeroControl[i].OrdenTrabajoSpoolID,
                                        SqCliente = numeroControl[i].SqCliente,
                                        SQ = numeroControl[i].SQ,
                                        TieneHoldIngenieria = numeroControl[i].TieneHoldIngenieria,
                                        OkPnd = numeroControl[i].OkPnd,
                                        Incidencias = numeroControl[i].Incidencias,
                                        Granel = numeroControl[i].Granel,
                                        OkFab = numeroControl[i].OkFab
                                    });
                                }
                                string datosAsignados = "";
                                List<LayoutGridSQ> listaSoloNumeroControlNuevos = (from p in listaElementos
                                                                                   where !(from ex in model.ListaElementosPorSQ
                                                                                           select ex.OrdenTrabajoSpoolID).Contains(p.OrdenTrabajoSpoolID)
                                                                                   select p).ToList();

                                List<LayoutGridSQ> ListaTieneSQCliente = (from a in listaSoloNumeroControlNuevos where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                                List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in listaSoloNumeroControlNuevos where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                                for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                                {
                                    datosAsignados += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SI Agregado <br>";
                                }

                                List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneSQCliente where (a.SQ != "" && a.SQ != null) select a).ToList();
                                List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneSQCliente where (a.SQ == "" || a.SQ == null) select a).ToList();
                                for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                                {
                                    datosAsignados += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en la SI: " + ListaTieneSQInterno[i].SQ + "<br>";
                                }

                                List<LayoutGridSQ> ListaNoTieneOkFab = (from a in ListaNoTieneSQInterno where !a.OkFab select a).ToList(); //Muestro warning a los que le falta ok fabricacion
                                List<LayoutGridSQ> ListaTieneOkFab = (from a in ListaNoTieneSQInterno where a.OkFab select a).ToList(); //Agrego aquellos que tiene ok fabricación
                                for (int i = 0; i < ListaNoTieneOkFab.Count; i++)
                                {
                                    datosAsignados += " El Spool <b>" + ListaNoTieneOkFab[i].NumeroControl + "</b> Le Falta Ok Fabricación <br>";
                                }

                                if (datosAsignados != "")
                                {
                                    TempData["errorEditar"] += datosAsignados;
                                }
                                //if (ListaNoTieneSQInterno.Count > 0)
                                if (ListaTieneOkFab.Count > 0)
                                {
                                    //var Lista = ListaNoTieneSQInterno;
                                    var Lista = ListaTieneOkFab;
                                    List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList(); //.OrderBy(c => c.NumeroControl).ToList();                                
                                    string auxString = NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit");
                                    string numerosControl = "";
                                    List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                    if (auxString != "" && auxString != "[]" && auxString != null)
                                    {
                                        NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                                        if (ListaTmp == null)
                                        {
                                            ListaTmp = new List<LayoutGridSQ>();
                                        }
                                        ListaTmp.InsertRange(0, ListaSinRepetidos);
                                        var Lista2 = ListaTmp;
                                        ListaTmp = Lista2.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList(); //.OrderBy(c => c.NumeroControl).ToList();
                                        numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                        NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", numerosControl);
                                        model.ListaElementosPorSQ = ListaTmp;
                                    }
                                    else
                                    {
                                        string numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementosPorSQ);
                                        NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", numeroControlCuadranteSQ);
                                    }
                                    model.TieneDatosGridEdit = true;
                                }
                                else
                                {
                                    model.TieneDatosGridEdit = false;
                                }
                            }
                            else
                            {
                                TempData["sinDatos"] += "No se encontraron datos que coincidieran con su busqueda";
                            }
                        }
                        break;
                    case "s":
                        //Spools Resueltos
                        List<AgregarSI> ListaSpoolsResueltos = JsonConvert.DeserializeObject<List<AgregarSI>>(NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos"));
                        if (ListaSpoolsResueltos != null)
                        {
                            List<AgregarSI> ListaResueltosPorProyecto = new List<AgregarSI>();
                            ListaResueltosPorProyecto = ListaSpoolsResueltos.Where(x => x.ProyectoID == model.ProjectIdEditar).ToList();
                            if (ListaResueltosPorProyecto.Count > 0)
                            {
                                //Los ingreso al cuadrante seleccionado
                                CuadranteSQ cuadrantes = OrdenTrabajoSpoolBO.Instance.BuscarCuadrante(model.QuadrantIdCEdit, model.ProjectIdEditar);

                                List<LayoutGridSQ> ListaGeneral = new List<LayoutGridSQ>();
                                foreach (var item in ListaResueltosPorProyecto)
                                {
                                    ListaGeneral.Add(new LayoutGridSQ
                                    {
                                        Accion = 1,
                                        Cuadrante = cuadrantes.Cuadrante,
                                        CuadranteID = cuadrantes.CuadranteID,
                                        NumeroControl = item.NumeroControl,
                                        SpoolID = item.SpoolID,
                                        OrdenTrabajoSpoolID = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolIDPorNumeroDeControl(item.NumeroControl).GetValueOrDefault(),
                                        SqCliente = item.SqCliente,
                                        SQ = item.SI,
                                        TieneHoldIngenieria = item.Hold,
                                        OkPnd = false,
                                        Incidencias = OrdenTrabajoSpoolBO.Instance.ObtenerNumeroIncidencias(item.SpoolID),
                                        Granel = item.Granel
                                    });
                                }
                                string errores = "";
                                List<LayoutGridSQ> ListaTieneSQCliente = (from a in ListaGeneral where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                                List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in ListaGeneral where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                                for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                                {
                                    errores += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SI Agregado <br>";
                                }

                                List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == true select a).ToList();
                                List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == false select a).ToList();
                                for (int i = 0; i < ListaTieneHold.Count; i++)
                                {
                                    errores += " El Spool " + ListaTieneHold[i].NumeroControl + " se encuentra en hold  " + "<br>";
                                }

                                List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ != "" && a.SQ != null) select a).ToList();
                                List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ == "" || a.SQ == null) select a).ToList();
                                for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                                {
                                    errores += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en la SI: " + ListaTieneSQInterno[i].SQ + "<br>";
                                }

                                if (errores != "")
                                {
                                    TempData["errorEditar"] += errores;
                                }

                                if (ListaNoTieneSQInterno.Count > 0)
                                {
                                    var Lista = ListaNoTieneSQInterno;
                                    List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.SpoolID).Select(b => b.First()).ToList();
                                    string auxString = NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit");
                                    string numerosControl = "";
                                    List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                    if (auxString != "" && auxString != "[]" && auxString != null)
                                    {
                                        NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                                        if (ListaTmp == null)
                                        {
                                            ListaTmp = new List<LayoutGridSQ>();
                                        }
                                        ListaTmp.InsertRange(0, ListaSinRepetidos);
                                        var Lista2 = ListaTmp;
                                        ListaTmp = Lista2.GroupBy(a => a.SpoolID).Select(b => b.First()).ToList();
                                        numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                        NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", numerosControl);
                                        //Limpio lista de spools generados                        
                                        List<AgregarSI> SpoolsResueltos = JsonConvert.DeserializeObject<List<AgregarSI>>(NavContext.GetDataFromSession<string>(Session, "ListaSpoolsResueltos"));
                                        SpoolsResueltos.RemoveAll(a => Lista.Any(b => a.ProyectoID == model.ProjectIdADD));
                                        NavContext.SetDataToSession<string>(Session, "ListaSpoolsResueltos", "");
                                        NavContext.SetDataToSession<string>(Session, "ListaSpoolsResueltos", JsonConvert.SerializeObject(SpoolsResueltos));
                                        //Agrego spools a grid
                                        model.ListaElementosPorSQ = ListaTmp;
                                    }
                                    else
                                    {
                                        var lista = model.ListaElementosPorSQ;
                                        if (lista != null)
                                        {
                                            model.ListaElementosPorSQ = lista.GroupBy(a => a.SpoolID).Select(b => b.First()).ToList();
                                        }
                                        string numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementosPorSQ);
                                        NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", numeroControlCuadranteSQ);
                                    }
                                    model.TieneDatosGridEdit = true;
                                }
                                else
                                {
                                    model.TieneDatosGridEdit = false;
                                }
                            }
                            else
                            {
                                TempData["sinDatos"] += "No Se Encontró Spools Resueltos";
                            }
                        }
                        else
                        {
                            TempData["sinDatos"] += "No se encontró Spools Resueltos";
                        }
                        break;
                    default:
                        break;
                }
                              
            }
            if (model.ListaElementosPorSQ.Count > 0)
            {
                model.ViewGridEdit = true;
            }
            else
            {
                model.ViewGridEdit = false;
            }
            if (project != null)
            {
                NavContext.SetProject(project.ID);
                NavContext.SetProjectEdit(project.ID);
                model.ProjectIdADD = project.ID;
                model.ProjectIdEditar = project.ID;
            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult SaveNCSQADD(SQModel model)
        {
            NavContext.SetDataToSession<string>(Session, "Vista", "1");
            model.SeleccionAgregarEditar = "1";
            if (model.QuadrantIdCADD == 0)
            {
                if (model.QuadrantIdNCADD == 0)
                {
                    if (model.QuadrantIdCEdit == 0)
                    {
                        if (model.QuadrantIdNCEdit != 0)
                        {
                            model.CuadranteID = model.QuadrantIdNCEdit;
                        }
                    }
                    else
                    {
                        model.CuadranteID = model.QuadrantIdCEdit;
                    }
                }
                else
                {
                    model.CuadranteID = model.QuadrantIdNCADD;
                }
            }
            else
            {
                model.CuadranteID = model.QuadrantIdCADD;
            }
            if (model.CuadranteID == 0)
            {
                if (NavContext.getCuadranteID() != "" && NavContext.getCuadranteID() != null)
                {
                    model.CuadranteID = int.Parse(NavContext.getCuadranteID());
                }
            }
            else
            {
                NavContext.setCuadranteID(model.CuadranteID.ToString());
            }

            if (ModelState.IsValid && model.ProjectIdADD > 0)
            {
                ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdADD);
                List<LayoutGridSQ> currentControlNumbers = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd"));
                //Obtener SpoolID
                List<LayoutGridSQ> ListaConDatosNumeroControl = OrdenTrabajoSpoolBO.Instance.ListaNumControlConSpoolID(ToDataTable.Instance.toDataTable(currentControlNumbers));
                if (ListaConDatosNumeroControl != null)
                {
                    if (ListaConDatosNumeroControl.Count > 0)
                    {
                        string datosAsignados = "";
                        List<LayoutGridSQ> listaSoloNumeroControlNuevos = (from a in currentControlNumbers join b in ListaConDatosNumeroControl on a.OrdenTrabajoSpoolID equals b.OrdenTrabajoSpoolID select b).ToList();

                        List<LayoutGridSQ> ListaNoTieneOkFab = (from a in listaSoloNumeroControlNuevos where !a.OkPnd select a).ToList(); //Muestro warning a los que le falta Ok Fabricacion
                        List<LayoutGridSQ> ListaTieneOkFab = (from a in listaSoloNumeroControlNuevos where a.OkPnd select a).ToList(); //Agrego aquellos que tiene Ok Fabricacion
                        for (int i = 0; i < ListaNoTieneOkFab.Count; i++)
                        {
                            datosAsignados += " El Spool " + ListaNoTieneOkFab[i].NumeroControl + " Le Falta Ok Fabricación, Se Ignora Guardado. <br>";
                        }

                        List<LayoutGridSQ> ListaTieneSQCliente = (from a in ListaTieneOkFab where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                        List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in ListaTieneOkFab where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                        for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                        {
                            datosAsignados += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SI Agregado, se ignora guardado.  <br>";
                        }

                        List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == true select a).ToList();
                        List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == false select a).ToList();
                        for (int i = 0; i < ListaTieneHold.Count; i++)
                        {
                            datosAsignados += " El Spool " + ListaTieneHold[i].NumeroControl + " se encuentra en hold  " + ", se ignora guardado. <br>";
                        }

                        List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ != "" && a.SQ != null) select a).ToList();
                        List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ == "" || a.SQ == null) select a).ToList();
                        for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                        {
                            datosAsignados += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en la SI: " + ListaTieneSQInterno[i].SQ + ", se ignora guardado. <br>";
                        }

                        List<LayoutGridSQ> ListaTieneIncidencias = (from a in ListaNoTieneSQInterno where a.Incidencias > 0 select a).ToList();
                        List<LayoutGridSQ> ListaNoTieneIncidencias = (from a in ListaNoTieneSQInterno where a.Incidencias == 0 select a).ToList();
                        for (int i = 0; i < ListaTieneIncidencias.Count; i++)
                        {
                            datosAsignados += " El Spool " + ListaTieneIncidencias[i].NumeroControl + " tiene registradas incidencias, se ignora guardado. <br>";
                        }

                        if (datosAsignados != "")
                        {
                            TempData["errorSaveAdd"] += datosAsignados;
                        }
                        if (ListaNoTieneIncidencias.Count > 0)
                        {
                            try
                            {
                                string SQConsecutivo = OrdenTrabajoSpoolBO.Instance.GuardarNumeroControlSQ(ToDataTable.Instance.toDataTable(ListaNoTieneIncidencias), SessionFacade.UserId, SessionFacade.NombreCompleto, project.ID, "");
                                NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                                if (SQConsecutivo == model.SQ)
                                {
                                    TempData["ActualizadoCorrecto"] = "Sol. Inspect: " + model.SQ + " Actualizado Correctamente";
                                }
                                else if (SQConsecutivo != "0")
                                {
                                    TempData["success"] = SQConsecutivo;
                                } else
                                {
                                    TempData["errorSaveAdd"] = "Ocurrió Un Error Al Guardar SI";
                                }

                                if (model.QuadrantIdCADD != 0)
                                {
                                    NavContext.setCuadranteID(model.QuadrantIdCADD.ToString());
                                }
                                else
                                {
                                    NavContext.setCuadranteID(model.QuadrantIdNCADD.ToString());
                                }

                            }
                            catch (Exception e)
                            {
                                //  ModelState.AddModelError(string.Empty, e.Details.FirstOrDefault());
                                //  return View("Index", GetSearchModel(exit));
                            }
                        }
                        else
                        {
                            NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                        }
                    }
                }

                if (project != null)
                {
                    NavContext.SetProject(project.ID);
                    NavContext.SetProjectEdit(project.ID);
                    model.ProjectIdADD = project.ID;
                    model.ProjectIdEditar = project.ID;
                }
            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult SaveNCSQADDEdit(SQModel model)
        {
            if (model.SQ == null)
            {
                if (NavContext.GetDataFromSession<string>(Session, "SI") != "")
                {
                    model.SQ = NavContext.GetDataFromSession<string>(Session, "SI");
                }
            }
            NavContext.SetDataToSession<string>(Session, "Vista", "2");
            model.SeleccionAgregarEditar = "2";
            if (model.QuadrantIdCEdit == 0)
            {
                if (model.QuadrantIdNCEdit == 0)
                {
                    if (model.QuadrantIdCADD == 0)
                    {
                        if (model.QuadrantIdNCADD != 0)
                        {
                            model.CuadranteID = model.QuadrantIdNCADD;
                        }
                    }
                    else
                    {
                        model.CuadranteID = model.QuadrantIdCADD;
                    }
                }
                else
                {
                    model.CuadranteID = model.QuadrantIdNCEdit;
                }
            }
            else
            {
                model.CuadranteID = model.QuadrantIdCADD;
            }
            if (model.CuadranteID == 0)
            {
                if (NavContext.getCuadranteID() != "" && NavContext.getCuadranteID() != null)
                {
                    model.CuadranteID = int.Parse(NavContext.getCuadranteID());
                }
            }
            else
            {
                NavContext.setCuadranteID(model.CuadranteID.ToString());
            }
            ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdEditar);

            if (ModelState.IsValid /*&& ValidaModel(model)*/&& model.ProjectIdEditar > 0)
            {
                List<LayoutGridSQ> currentControlNumbers = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit"));
                List<LayoutGridSQ> ListaConDatosNumeroControl = OrdenTrabajoSpoolBO.Instance.ListaNumControlConSpoolID(ToDataTable.Instance.toDataTable(currentControlNumbers));
                string datosAsignados = "";

                //List<LayoutGridSQ> ListaNoTieneOkPnd = (from a in ListaConDatosNumeroControl where !a.OkPnd select a).ToList(); //Muestro warning a los que le falta okpnd
                //List<LayoutGridSQ> ListaTieneOkPnd = (from a in ListaConDatosNumeroControl where a.OkPnd select a).ToList(); //Agrego aquellos que tiene okpnd
                //for (int i = 0; i < ListaNoTieneOkPnd.Count; i++)
                //{
                //    datosAsignados += " El Spool " + ListaNoTieneOkPnd[i].NumeroControl + " Le Falta OkPnd Se Ignora Guardado. <br>";
                //}

                List<LayoutGridSQ> ListaTieneSQCliente = (from a in ListaConDatosNumeroControl where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in ListaConDatosNumeroControl where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                {
                    datosAsignados += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SI Agregada, se ignora guardado. <br>";
                }

                List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where (a.TieneHoldIngenieria) select a).ToList();
                List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where !a.TieneHoldIngenieria select a).ToList();
                for (int a = 0; a < ListaTieneHold.Count; a++)
                {
                    datosAsignados += " El Spool " + ListaTieneHold[a].NumeroControl + " Tiene Hold, Se Ignora Guardado. <br>";
                }

                //List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ != "" && a.SQ != null && a.SQ != model.SQ) select a).ToList();
                List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ == "" || a.SQ == null) select a).ToList();
                //for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                //{
                //    datosAsignados += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en la SI: " + ListaTieneSQInterno[i].SQ + ", se ignora guardado. <br>";
                //}

                List<LayoutGridSQ> ListaTodos = (from a in ListaNoTieneHold select a).ToList();
                for (int i = 0; i < ListaTodos.Count; i++)
                {
                    if (ListaTodos[i].Incidencias > 0)
                    {
                        datosAsignados += " El Spool " + ListaTodos[i].NumeroControl + " tiene registradas incidencias, se ignora guardado. <br>";
                    }
                    if (ListaTodos[i].SQ != "" && ListaTodos[i].SQ != null && ListaTodos[i].SQ != model.SQ)
                    {
                        datosAsignados += " El Spool " + ListaTodos[i].NumeroControl + " se encuentra en la SI: " + ListaTodos[i].SQ + ", se ignora guardado. <br>";
                    }
                }

                List<LayoutGridSQ> ListaNoTieneIncidencias = (from a in ListaNoTieneSQInterno where a.Incidencias == 0 select a).ToList();
                //for (int i = 0; i < ListaTieneIncidencias.Count; i++)
                //{
                //    datosAsignados += " El Spool " + ListaTieneIncidencias[i].NumeroControl + " tiene registradas incidencias, se ignora guardado. <br>";
                //}
                if (datosAsignados != "")
                {
                    TempData["errorSaveEdit"] += datosAsignados;
                }

                List<LayoutGridSQ> ListaMismos = (from a in ListaTodos where a.SQ == model.SQ select a).ToList();
                if (ListaTodos.Count == ListaMismos.Count && ListaNoTieneIncidencias.Count == 0)
                {
                    TempData["ActualizadoCorrecto"] = "Sol. Inspect: " + model.SQ + " Actualizado Correctamente";
                }
                else
                {
                    if (ListaNoTieneIncidencias.Count > 0)
                    {
                        try
                        {
                            string SQConsecutivo = OrdenTrabajoSpoolBO.Instance.GuardarNumeroControlSQ(ToDataTable.Instance.toDataTable(ListaNoTieneIncidencias), SessionFacade.UserId, SessionFacade.NombreCompleto, model.ProjectIdEditar, model.SQ);
                            NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                            if (SQConsecutivo == model.SQ)
                            {
                                TempData["ActualizadoCorrecto"] = "Sol. Inspect: " + model.SQ + " Actualizado Correctamente";
                            }
                            else if (SQConsecutivo != "0")
                            {
                                TempData["success"] = SQConsecutivo;
                            } else
                            {
                                TempData["errorSaveEdit"] = "Ocurrió Un Error Al Guardar SI";
                            }
                            if (model.QuadrantIdCEdit != 0)
                            {
                                NavContext.setCuadranteID(model.QuadrantIdCEdit.ToString());
                            }
                            else
                            {
                                NavContext.setCuadranteID(model.QuadrantIdNCEdit.ToString());
                            }
                        }
                        catch (Exception e) { }
                    }
                    else
                    {
                        NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                    }
                }

                if (project != null)
                {
                    NavContext.SetProject(project.ID);
                    NavContext.SetProjectEdit(project.ID);
                    model.ProjectIdADD = project.ID;
                    model.ProjectIdEditar = project.ID;
                }
            }
            return View("Index", model);
        }

        public ActionResult DeleteNumeroControlSQ(string numeroControlSQ, int ProyectoID, string SQ)
        {
            List<LayoutGridSQ> listaNcSQ = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd"));
            LayoutGridSQ ncActualSQ = listaNcSQ.Where(x => x.NumeroControl == numeroControlSQ).FirstOrDefault();
            if (ncActualSQ != null)
            {
                listaNcSQ.Remove(ncActualSQ);
                OrdenTrabajoSpoolBO.Instance.EliminarSpool(numeroControlSQ, ProyectoID, SQ);
            }
            NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", JsonConvert.SerializeObject(listaNcSQ));
            return View("Index", GetModelSQ(ProyectoID));
        }
        public ActionResult DeleteNumeroControlSQEditar(string numeroControlSQ, int ProyectoID, string SQ)
        {
            List<LayoutGridSQ> listaNcSQ = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit"));
            LayoutGridSQ ncActualSQ = listaNcSQ.Where(x => x.NumeroControl == numeroControlSQ).FirstOrDefault();
            if (ncActualSQ != null)
            {
                listaNcSQ.Remove(ncActualSQ);
                OrdenTrabajoSpoolBO.Instance.EliminarSpool(numeroControlSQ, ProyectoID, SQ);
            }
            NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", JsonConvert.SerializeObject(listaNcSQ));
            return View("Index", GetModelSQEditar(ProyectoID));
        }

        private SQModel GetModelSQ(int ProyectoID)
        {
            List<LayoutGridSQ> listaElementos = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd"));
            SQModel sqlModel = new SQModel();
            sqlModel.ListaElementos = listaElementos;
            sqlModel.SeleccionAgregarEditar = "1";
            sqlModel.ProjectIdADD = NavContext.GetCurrentProjectSQ().ID;
            sqlModel.ProjectIdEditar = NavContext.GetCurrentProjectSQ().ID;
            if (NavContext.getCuadranteID() != "")
            {
                sqlModel.CuadranteID = int.Parse(NavContext.getCuadranteID());
            }
            ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == ProyectoID);
            if (project != null)
            {
                NavContext.SetProject(project.ID);
                NavContext.SetProjectEdit(project.ID);
                sqlModel.ProjectIdADD = project.ID;
                sqlModel.ProjectIdEditar = project.ID;
            }
            return sqlModel;
        }

        private SQModel GetModelSQEditar(int ProyectoID)
        {
            List<LayoutGridSQ> listaElementos = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit"));
            SQModel sqlModel = new SQModel();
            sqlModel.ListaElementosPorSQ = listaElementos;
            sqlModel.SeleccionAgregarEditar = "2";
            sqlModel.ProjectIdEditar = NavContext.GetCurrentProjectSQEditar().ID;
            sqlModel.ProjectIdADD = NavContext.GetCurrentProjectSQEditar().ID;
            if (NavContext.getCuadranteID() != "")
            {
                sqlModel.CuadranteID = int.Parse(NavContext.getCuadranteID());
            }
            sqlModel.SQ = NavContext.GetSQ();
            ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == ProyectoID);
            if (project != null)
            {
                NavContext.SetProject(project.ID);
                NavContext.SetProjectEdit(project.ID);
                sqlModel.ProjectIdADD = project.ID;
                sqlModel.ProjectIdEditar = project.ID;
            }
            return sqlModel;
        }

        [HttpGet]
        public JsonResult VerificarConsecutivoProyecto(int ProyectoID)
        {
            string resultado = OrdenTrabajoSpoolBO.Instance.ObtenerConsecutivoProyecto(ProyectoID);
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
        [HttpGet]
        public JsonResult ObtenerIncidencias(int SpoolID)
        {
            List<IncidenciaC> Incidencias = OrdenTrabajoSpoolBO.Instance.ObtenerIncidencias(SpoolID);
            string resultado = "";
            if (Incidencias != null && Incidencias.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(Incidencias);
            }
            else
            {
                resultado = "NODATA";
            }
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerTipoIncidencias()
        {
            List<TipoIncidencia> ListaIncidencias = OrdenTrabajoSpoolBO.Instance.ObtenerTipoIncidencias();
            string resultado = "";
            if (ListaIncidencias != null && ListaIncidencias.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(ListaIncidencias);
            }
            else
            {
                resultado = "NODATA";
            }
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerDetalleIncidencias(int TipoIncidenciaID, int SpoolID)
        {
            List<IncidenciaDetalle> Detalle = OrdenTrabajoSpoolBO.Instance.ObtenerDetalleIncidencias(TipoIncidenciaID, SpoolID);
            string resultado = "";
            if (Detalle != null && Detalle.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(Detalle);
            }
            else
            {
                resultado = "NODATA";
            }
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerListaErrores(int TipoIncidenciaID)
        {
            List<ListaErrores> Lista = OrdenTrabajoSpoolBO.Instance.ObtenerListaErrores(TipoIncidenciaID);
            string resultado = "";
            if (Lista != null && Lista.Count > 0)
            {
                resultado = JsonConvert.SerializeObject(Lista);
            }
            else
            {
                resultado = "NODATA";
            }
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerNumeroIncidencias(int SpoolID)
        {
            int resultado = OrdenTrabajoSpoolBO.Instance.ObtenerNumeroIncidencias(SpoolID);
            var myData = new[] { new { result = resultado } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ResolverEliminarIncidencia(int IncidenciaID, string Origen, string Resolucion, int Accion)
        {
            /*Origen =  pantalla donde se elimino la incidencia*/
            string respuesta = OrdenTrabajoSpoolBO.Instance.ResolverEliminarIncidencia(IncidenciaID, Origen, Resolucion, SessionFacade.NombreCompleto, Accion);
            var myData = new[] { new { result = respuesta } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult InactivarSpoolSI(string NumeroControl, int ProyectoID)
        {
            List<LayoutGridSQ> ListaActualSpools = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit"));
            LayoutGridSQ spoolEncontrado = ListaActualSpools.Where(x => x.NumeroControl == NumeroControl).FirstOrDefault();
            if (spoolEncontrado != null)
            {
                ListaActualSpools.Remove(spoolEncontrado);
            }
            NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", JsonConvert.SerializeObject(ListaActualSpools));
            string respuesta = OrdenTrabajoSpoolBO.Instance.InactivarSpoolDeSI(NumeroControl, ProyectoID);
            var myData = new[] { new { result = respuesta } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet, ValidateInput(false)]
        public JsonResult ResolucionIncidencias(int SpoolID, int IncidenciaID, string Resolucion, string Origen, int Accion)
        {
            string respuesta = OrdenTrabajoSpoolBO.Instance.ResolverEliminarIncidencia(IncidenciaID, Origen, Resolucion, SessionFacade.NombreCompleto, Accion);
            var myData = new[] { new { result = respuesta } };
            return Json(myData, JsonRequestBehavior.AllowGet);
        }

    }
}