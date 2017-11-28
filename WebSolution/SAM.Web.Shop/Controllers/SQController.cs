using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SAM.Web.Shop.Utils;
using SAM.Web.Shop.Models;
using SAM.Entities.Cache;
using SAM.Web.Common;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using SAM.Entities.Busqueda;
using SAM.Web.Shop.Resources.Views.WorkStatus;
using SAM.Entities.Personalizadas.Shop;
using Newtonsoft.Json;
using System.IO;
using System.Security;


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
                sqmodel.SeleccionAgregarEditar = "1";          
                NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                if (NavContext.GetCurrentProject().ID > 0)
                {
                    sqmodel.ProjectIdADD = NavContext.GetCurrentProject().ID;
                    sqmodel.ProjectIdEditar = NavContext.GetCurrentProject().ID;
                }
                if(NavContext.getCuadranteID() != "")
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
            model.SeleccionAgregarEditar = "1";
            if(model.QuadrantIdCADD == 0)
            {
                if(model.QuadrantIdNCADD == 0)
                {
                    if(model.QuadrantIdCEdit == 0)
                    {
                        if(model.QuadrantIdNCEdit != 0)
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
            NavContext.setCuadranteID(model.CuadranteID.ToString());
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
                if(Lista == null)
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
                                    TieneHoldIngenieria = ots.TieneHoldIngenieria
                                });
                            }
                            string datosAsignados = "";
                            
                            List<LayoutGridSQ> ListaTieneSQCliente = (from a in listaElementos where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in listaElementos where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                            for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                            {
                                datosAsignados += " el Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SQ Cliente Agregado <br>";
                            }

                            List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == true select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == false select a).ToList();
                            for (int i = 0; i < ListaTieneHold.Count; i++)
                            {
                                datosAsignados += " el Spool " + ListaTieneHold[i].NumeroControl + " se encuentra en hold  " + "<br>";
                            }

                            List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ != "" && a.SQ != null) select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ == "" || a.SQ == null) select a).ToList();
                            for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                            {
                                datosAsignados += " el Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en el SQ " + ListaTieneSQInterno[i].SQ + "<br>";
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
                                if(ListaTmp == null)
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
                                    if(lista != null)
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
                            TempData["sinDatos"] += "No se encontraron datos que coincidieran con su busqueda";                            
                        }
                    }
                    break;
                case "c":
                    if (model.QuadrantIdCADD != 0)
                    {
                        NavContext.setCuadranteID(model.QuadrantIdCADD.ToString());
                        List<CuadranteNumeroControlSQ> numeroControl = OrdenTrabajoSpoolBO.Instance.BuscarPorCuadranteSQ(model.QuadrantIdCADD, model.ProjectIdADD, 1);                        
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
                                    TieneHoldIngenieria = numeroControl[i].TieneHoldIngenieria
                                });
                            }
                            string datosAsignados = "";
                            
                            List<LayoutGridSQ> ListaTieneSQCliente = (from a in listaElementos where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in listaElementos where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                            for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                            {
                                datosAsignados += " el Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SQ Cliente Agregado <br>";
                            }

                            List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == true select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == false select a).ToList();
                            for (int i = 0; i < ListaTieneHold.Count; i++)
                            {
                                datosAsignados += " el Spool " + ListaTieneHold[i].NumeroControl + " se encuentra en hold  " + "<br>";
                            }

                            List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ != "" && a.SQ != null) select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneHold where (a.SQ == "" || a.SQ == null) select a).ToList();
                            for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                            {
                                datosAsignados += " el Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en el SQ " + ListaTieneSQInterno[i].SQ + "<br>";
                            }

                            if (datosAsignados != "")
                            {
                                TempData["errorAdd"] += datosAsignados;                                
                            }
                            if (ListaNoTieneSQInterno.Count > 0)
                            {
                                var Lista = ListaNoTieneSQInterno;
                                List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList(); //.OrderBy(c => c.NumeroControl).ToList();                                
                                string auxString = NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd");                                
                                string numerosControl = "";
                                List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);                              
                                if (auxString != "" && auxString != "[]" && auxString != null)
                                {                                    
                                    NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                                    if(ListaTmp == null)
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
                                    if(lista != null)
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

                default:
                    break;
            }
            if(project != null)
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
            NavContext.setCuadranteID(model.CuadranteID.ToString());
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
                                TieneHoldIngenieria = numeroControl[i].TieneHoldIngenieria
                            });
                        }
                        var Lista = listaElementos;
                        if (Lista != null)
                        {
                            if(model.QuadrantIdCEdit != 0)
                            {
                                NavContext.setCuadranteID(model.QuadrantIdCEdit.ToString());
                            }
                            else
                            {
                                NavContext.setCuadranteID(model.QuadrantIdNCEdit.ToString());
                            }

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
                if(model.SQ != null && model.SQ != "")
                {
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
                                            TieneHoldIngenieria = ots.TieneHoldIngenieria
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
                                        datosAsignados += " el Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SQ Cliente Agregado <br>";
                                    }

                                    List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneSQCliente where (a.SQ != "" && a.SQ != null) select a).ToList();
                                    List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneSQCliente where (a.SQ == "" || a.SQ == null) select a).ToList();
                                    for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                                    {
                                        datosAsignados += " el Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en el SQ " + ListaTieneSQInterno[i].SQ + "<br>";
                                    }

                                    if (datosAsignados != "")
                                    {
                                        TempData["errorEditar"] += datosAsignados;
                                    }

                                    if (ListaNoTieneSQInterno.Count > 0)
                                    {
                                        if (model.QuadrantIdCEdit != 0)
                                        {
                                            NavContext.setCuadranteID(model.QuadrantIdCEdit.ToString());
                                        }
                                        else
                                        {
                                            NavContext.setCuadranteID(model.QuadrantIdNCEdit.ToString());
                                        }

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
                                    TempData["sinDatos"] += "No se encontraron datos que coincidieran con su busqueda";
                                }
                            }
                            break;
                        case "c":
                            if (model.QuadrantIdCEdit != 0)
                            {
                                NavContext.setCuadranteID(model.QuadrantIdCEdit.ToString());
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
                                            TieneHoldIngenieria = numeroControl[i].TieneHoldIngenieria
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
                                        datosAsignados += " el Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SQ Cliente Agregado <br>";
                                    }

                                    List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneSQCliente where (a.SQ != "" && a.SQ != null) select a).ToList();
                                    List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneSQCliente where (a.SQ == "" || a.SQ == null) select a).ToList();
                                    for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                                    {
                                        datosAsignados += " el Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en el SQ " + ListaTieneSQInterno[i].SQ + "<br>";
                                    }

                                    if (datosAsignados != "")
                                    {
                                        TempData["errorAdd"] += datosAsignados;
                                    }
                                    if (ListaNoTieneSQInterno.Count > 0)
                                    {
                                        var Lista = ListaNoTieneSQInterno;
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

                        default:
                            break;
                    }
                }
                else
                {
                    TempData["FaltaSQ"] += "Porfavor Ingrese Sol. Inspect";
                }                                
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
            model.SeleccionAgregarEditar = "1";
            if (ModelState.IsValid /*&& ValidaModel(model)*/&& model.ProjectIdADD > 0)
            {
                ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdADD);                                             
                List<LayoutGridSQ> currentControlNumbers = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlAdd"));
                //Obtener SpoolID
                List<LayoutGridSQ> ListaConDatosNumeroControl = OrdenTrabajoSpoolBO.Instance.ListaNumControlConSpoolID(ToDataTable.Instance.toDataTable(currentControlNumbers));
                if(ListaConDatosNumeroControl != null)
                {
                    if (ListaConDatosNumeroControl.Count > 0)
                    {
                        string datosAsignados = "";
                        List<LayoutGridSQ> listaSoloNumeroControlNuevos = (from a in currentControlNumbers join b in ListaConDatosNumeroControl on a.OrdenTrabajoSpoolID equals b.OrdenTrabajoSpoolID select b).ToList();
                        List<LayoutGridSQ> ListaTieneSQCliente = (from a in listaSoloNumeroControlNuevos where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                        List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in listaSoloNumeroControlNuevos where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                        for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                        {
                            datosAsignados += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SQ Cliente Agregado, se ignora guardado.  <br>";
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
                            datosAsignados += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en el SQ " + ListaTieneSQInterno[i].SQ + ", se ignora guardado. <br>";
                        }

                        if (datosAsignados != "")
                        {
                            TempData["errorSaveAdd"] += datosAsignados;
                        }
                        if (ListaNoTieneSQInterno.Count > 0)
                        {
                            try
                            {                                    
                                string SQConsecutivo = OrdenTrabajoSpoolBO.Instance.GuardarNumeroControlSQ(ToDataTable.Instance.toDataTable(ListaNoTieneSQInterno), SessionFacade.UserId, SessionFacade.NombreCompleto, project.ID, "");                                   
                                NavContext.SetDataToSession<string>(Session, "ListaNumControlAdd", "");
                                if (SQConsecutivo == model.SQ)
                                {
                                    TempData["ActualizadoCorrecto"] = "SQ: " + model.SQ + " Actualizado Correctamente";
                                }
                                else if (SQConsecutivo != "0")
                                {
                                    TempData["success"] = SQConsecutivo;
                                }
                                    
                                if(model.QuadrantIdCADD != 0)
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
            model.SeleccionAgregarEditar = "2";
            if (ModelState.IsValid /*&& ValidaModel(model)*/&& model.ProjectIdEditar > 0)
            {
                ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdEditar);                                 
                List<LayoutGridSQ> currentControlNumbers = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit") == null ? "" : NavContext.GetDataFromSession<string>(Session, "ListaNumControlEdit"));
                List<LayoutGridSQ> ListaConDatosNumeroControl = OrdenTrabajoSpoolBO.Instance.ListaNumControlConSpoolID(ToDataTable.Instance.toDataTable(currentControlNumbers));
                string datosAsignados = "";

                List<LayoutGridSQ> ListaTieneSQCliente = (from a in ListaConDatosNumeroControl where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in ListaConDatosNumeroControl where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                {
                    datosAsignados += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SQ Cliente Agregado, se ignora guardado. <br>";
                }
                    
                List<LayoutGridSQ> ListaTieneSQInterno = (from a in ListaNoTieneSQCliente where (a.SQ != "" && a.SQ != null && a.SQ != model.SQ) select a).ToList();
                List<LayoutGridSQ> ListaNoTieneSQInterno = (from a in ListaNoTieneSQCliente where (a.SQ == "" || a.SQ == null) select a).ToList();
                for (int i = 0; i < ListaTieneSQInterno.Count; i++)
                {
                    datosAsignados += " El Spool " + ListaTieneSQInterno[i].NumeroControl + " se encuentra en el SQ " + ListaTieneSQInterno[i].SQ + ", se ignora guardado. <br>";
                }

                if (datosAsignados != "")
                {
                    TempData["errorSaveEdit"] += datosAsignados;
                }
                List<LayoutGridSQ> ListaMismos = (from a in ListaNoTieneSQCliente where a.SQ == model.SQ select a).ToList();
                if(ListaNoTieneSQCliente.Count == ListaMismos.Count && ListaNoTieneSQInterno.Count == 0)
                {
                    TempData["ActualizadoCorrecto"] = "SQ: " + model.SQ + " Actualizado Correctamente";
                }
                else
                {
                    if (ListaNoTieneSQInterno.Count > 0)
                    {
                        try
                        {
                            string SQConsecutivo = OrdenTrabajoSpoolBO.Instance.GuardarNumeroControlSQ(ToDataTable.Instance.toDataTable(ListaNoTieneSQInterno), SessionFacade.UserId, SessionFacade.NombreCompleto, model.ProjectIdEditar, model.SQ);
                            NavContext.SetDataToSession<string>(Session, "ListaNumControlEdit", "");
                            if (SQConsecutivo == model.SQ)
                            {
                                TempData["ActualizadoCorrecto"] = "SQ: " + model.SQ + " Actualizado Correctamente";
                            }
                            else if (SQConsecutivo != "0")
                            {
                                TempData["success"] = SQConsecutivo;
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


    }
}