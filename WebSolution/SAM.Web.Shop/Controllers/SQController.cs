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
                NavContext.SetNumbersControl("");
                NavContext.SetNumbersControlCuadranteSQ("");
                NavContext.SetNumbersControlCuadranteSQEditar("");
                if (NavContext.GetCurrentProject().ID > 0)
                {
                    sqmodel.ProjectIdADD = NavContext.GetCurrentProject().ID;
                    sqmodel.ProjectIdEditar = NavContext.GetCurrentProject().ID;
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
            ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdADD);
            if (NavContext.GetCurrentProjectSQ() == null || (model.ProjectIdADD != NavContext.GetCurrentProjectSQ().ID))
            {
                NavContext.SetProject(project.ID);
                NavContext.SetNumbersControlCuadranteSQ("");
                model.ListaElementos = new List<LayoutGridSQ>();
            }
            else
            {
                model.ListaElementos = new List<LayoutGridSQ>();
                model.ListaElementos.AddRange(Helps.GetListadoCuadrantesNumeroControlSQ(NavContext.GetCurrentNCSQ()));
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

                        List<int> controlNumberId = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolIDsPorNumeroDeControlYProyectoSQ(controlNumbers, project.ID);

                        if (controlNumberId.Count > 0)
                        {

                            OrdenTrabajoSpoolSQ ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoSpoolConSQ(controlNumberId[0]);
                            CuadranteSQ cuadrante = OrdenTrabajoSpoolBO.Instance.BuscarCuadrante(model.QuadrantIdNCADD, project.ID);
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
                            List<LayoutGridSQ> listaSoloNumeroControlNuevos = (from p in listaElementos
                                                                               where !(from ex in model.ListaElementos
                                                                                       select ex.OrdenTrabajoSpoolID).Contains(p.OrdenTrabajoSpoolID)
                                                                               select p).ToList();

                            List<LayoutGridSQ> ListaTieneSQCliente = (from a in listaSoloNumeroControlNuevos where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                            List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in listaSoloNumeroControlNuevos where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
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
                                List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).OrderBy(c => c.NumeroControl).ToList();
                                string auxString = NavContext.GetCurrentNCSQ();
                                string numerosControl = "";
                                List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                if (auxString != "" && auxString != "[]")
                                {
                                    NavContext.SetNumbersControlCuadranteSQ("");
                                    ListaTmp.InsertRange(0, ListaSinRepetidos);                                    
                                    numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                    NavContext.SetNumbersControlCuadranteSQ(numerosControl);                                    
                                    model.ListaElementos = ListaTmp;

                                }
                                else
                                {
                                    string numeroControlCuadranteSQ = NavContext.GetCurrentNCSQ();
                                    numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementos);                                    
                                    NavContext.SetNumbersControlCuadranteSQ(numeroControlCuadranteSQ);
                                }                                                          
                                model.TieneDatosGridAdd = true;

                                //string numeroControlCuadranteSQ = NavContext.GetCurrentNCSQ();
                                //numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaNoTieneSQInterno, model.ListaElementos);                                
                                //NavContext.SetNumbersControlCuadranteSQ(numeroControlCuadranteSQ);
                            }
                            else
                            {
                                //NavContext.SetNumbersControlCuadranteSQ("");
                                model.TieneDatosGridAdd = false;
                            }
                        }
                        else
                        {
                            TempData["sinDatos"] += "No se encontraron datos que coincidieran con su busqueda";
                            //ModelState.AddModelError(string.Empty, WorkStatusStrings.Mensaje_NumeroControlCuadrante);
                        }
                    }
                    break;
                case "c":
                    if (model.QuadrantIdCADD != 0)
                    {
                        List<CuadranteNumeroControlSQ> numeroControl = OrdenTrabajoSpoolBO.Instance.BuscarPorCuadranteSQ(model.QuadrantIdCADD, project.ID);

                        if (numeroControl.Count > 0)
                        {
                            // model.ListaElementos = new List<LayoutGridSQ>();
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
                            //List<LayoutGridSQ> listaSoloNumeroControlNuevos = (from p in listaElementos
                            //                                                   where !(from ex in model.ListaElementos
                            //                                                           select ex.OrdenTrabajoSpoolID).Contains(p.OrdenTrabajoSpoolID)
                            //                                                   select p).ToList();

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
                                List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).OrderBy(c => c.NumeroControl).ToList();
                                string auxString = NavContext.GetCurrentNCSQ();
                                string numerosControl = "";
                                List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                if (auxString != "" && auxString != "[]")
                                {
                                    NavContext.SetNumbersControlCuadranteSQ("");
                                    ListaTmp.InsertRange(0, ListaSinRepetidos);                                    
                                    numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                    NavContext.SetNumbersControlCuadranteSQ(numerosControl);
                                    model.ListaElementos = ListaTmp;
                                }
                                else
                                {
                                    string numeroControlCuadranteSQ = NavContext.GetCurrentNCSQ();
                                    numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementos);
                                    NavContext.SetNumbersControlCuadranteSQ(numeroControlCuadranteSQ);
                                }
                                model.TieneDatosGridAdd = true;                               
                                //string numeroControlCuadranteSQ = NavContext.GetCurrentNCSQ();
                                //numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaNoTieneSQInterno, model.ListaElementos);
                                //NavContext.SetNumbersControlCuadranteSQ(numeroControlCuadranteSQ);
                            }
                            else
                            {
                                model.TieneDatosGridAdd = false;
                                //NavContext.SetNumbersControlCuadranteSQ("");
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
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult EditarNC(SQModel model, string Command)
        {
            model.SeleccionAgregarEditar = "2";
            ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdEditar);

            if (NavContext.GetCurrentProjectSQEditar() == null || (model.ProjectIdEditar != NavContext.GetCurrentProjectSQ().ID))
            {
                NavContext.SetProjectEdit(project.ID);
                NavContext.SetNumbersControlCuadranteSQEditar("");
                model.ListaElementosPorSQ = new List<LayoutGridSQ>();
            }

            //limpiar la lista sq en memoria.
            if (model.SQ != NavContext.GetSQ())
            {
                NavContext.SetSQ(model.SQ);
                NavContext.SetNumbersControlCuadranteSQEditar("");
                model.ListaElementosPorSQ = new List<LayoutGridSQ>();
            }
            else
            {
                model.ListaElementosPorSQ = new List<LayoutGridSQ>();
                model.ListaElementosPorSQ.AddRange(Helps.GetListadoCuadrantesNumeroControlSQEditar(NavContext.GetCurrentNCSQEditar()));
                if (model.ListaElementosPorSQ.Count > 0) model.TieneDatosGrid = true;
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

                            List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).OrderBy(c => c.NumeroControl).ToList();
                            string auxString = NavContext.GetCurrentNCSQEditar();
                            string numerosControl = "";
                            List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                            if (auxString != "")
                            {
                                NavContext.SetNumbersControlCuadranteSQEditar("");
                                ListaTmp.InsertRange(0, ListaSinRepetidos);
                                //numerosControl = Helps.GetNumberControlsSQCookies(ListaTmp, model.ListaElementos);
                                var aux = ListaTmp;
                                List<LayoutGridSQ> NoRepetidos = aux.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).OrderBy(c => c.NumeroControl).ToList(); 
                                numerosControl = JsonConvert.SerializeObject(NoRepetidos);
                                NavContext.SetNumbersControlCuadranteSQEditar(numerosControl);
                                model.ListaElementosPorSQ = NoRepetidos;
                            }
                            else
                            {
                                string numeroControlCuadranteSQ = NavContext.GetCurrentNCSQEditar();
                                numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementosPorSQ);
                                NavContext.SetNumbersControlCuadranteSQEditar(numeroControlCuadranteSQ);
                            }                           
                            //List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).ToList();
                            //model.ListaElementosPorSQ = ListaSinRepetidos;
                            //string Result = JsonConvert.SerializeObject(ListaSinRepetidos);
                            //NavContext.SetNumbersControlCuadranteSQEditar(Result);
                            model.TieneDatosGrid = true;
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
                                    var Lista = ListaNoTieneSQInterno;
                                    List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).OrderBy(c => c.NumeroControl).ToList();
                                    string auxString = NavContext.GetCurrentNCSQEditar();
                                    string numerosControl = "";
                                    List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                    if (auxString != "")
                                    {
                                        NavContext.SetNumbersControlCuadranteSQEditar("");
                                        ListaTmp.InsertRange(0, ListaSinRepetidos);
                                        //numerosControl = Helps.GetNumberControlsSQCookies(ListaTmp, model.ListaElementos);
                                        numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                        NavContext.SetNumbersControlCuadranteSQEditar(numerosControl);
                                        model.ListaElementosPorSQ = ListaTmp;
                                    }
                                    else
                                    {
                                        string numeroControlCuadranteSQ = NavContext.GetCurrentNCSQEditar();
                                        numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementosPorSQ);
                                        NavContext.SetNumbersControlCuadranteSQEditar(numeroControlCuadranteSQ);
                                    }

                                    //string numeroControlCuadranteSQEditar = NavContext.GetCurrentNCSQEditar();
                                    //numeroControlCuadranteSQEditar = Helps.GetNumberControlsSQCookies(ListaNoTieneSQInterno, model.ListaElementosPorSQ);
                                    //NavContext.SetNumbersControlCuadranteSQEditar(numeroControlCuadranteSQEditar);
                                    model.TieneDatosGrid = true;
                                }
                                else
                                {
                                    //NavContext.SetNumbersControlCuadranteSQEditar("");
                                    if(TempData["errorEditar"] == null)
                                    {
                                        TempData["Repetido"] = "El Spool " + listaElementos[0].NumeroControl + " ya existe en el Grid";
                                    }                                    
                                    model.TieneDatosGrid = false;
                                }
                            }
                            else
                            {
                                TempData["sinDatos"] += "No se encontraron datos que coincidieran con su busqueda";
                                //ModelState.AddModelError(string.Empty, WorkStatusStrings.Mensaje_NumeroControlCuadrante);
                            }
                        }
                        break;
                    case "c":
                        if (model.QuadrantIdCEdit != 0)
                        {
                            List<CuadranteNumeroControlSQ> numeroControl = OrdenTrabajoSpoolBO.Instance.BuscarPorCuadranteSQ(model.QuadrantIdCEdit, model.ProjectIdEditar);
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

                                //List<LayoutGridSQ> ListaTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == true select a).ToList();
                                //List<LayoutGridSQ> ListaNoTieneHold = (from a in ListaNoTieneSQCliente where a.TieneHoldIngenieria == false select a).ToList();
                                //for (int i = 0; i < ListaTieneHold.Count; i++)
                                //{
                                //    datosAsignados += " el Spool " + ListaTieneHold[i].NumeroControl + " se encuentra en hold  " + "<br>";
                                //}

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
                                    List<LayoutGridSQ> ListaSinRepetidos = Lista.GroupBy(a => a.OrdenTrabajoSpoolID).Select(b => b.First()).OrderBy(c => c.NumeroControl).ToList();
                                    string auxString = NavContext.GetCurrentNCSQEditar();
                                    string numerosControl = "";
                                    List<LayoutGridSQ> ListaTmp = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(auxString);
                                    if (auxString != "")
                                    {
                                        NavContext.SetNumbersControlCuadranteSQEditar("");
                                        ListaTmp.InsertRange(0, ListaSinRepetidos);
                                        //numerosControl = Helps.GetNumberControlsSQCookies(ListaTmp, model.ListaElementos);
                                        numerosControl = JsonConvert.SerializeObject(ListaTmp);
                                        NavContext.SetNumbersControlCuadranteSQEditar(numerosControl);
                                        model.ListaElementosPorSQ = ListaTmp;
                                    }
                                    else
                                    {
                                        string numeroControlCuadranteSQ = NavContext.GetCurrentNCSQEditar();
                                        numeroControlCuadranteSQ = Helps.GetNumberControlsSQCookies(ListaSinRepetidos, model.ListaElementosPorSQ);
                                        NavContext.SetNumbersControlCuadranteSQEditar(numeroControlCuadranteSQ);
                                    }
                                    //string numeroControlCuadranteSQEditar = NavContext.GetCurrentNCSQEditar();
                                    //numeroControlCuadranteSQEditar = Helps.GetNumberControlsSQCookies(ListaNoTieneSQInterno, model.ListaElementosPorSQ);
                                    //NavContext.SetNumbersControlCuadranteSQEditar(numeroControlCuadranteSQEditar);
                                    model.TieneDatosGrid = true;                                   
                                }
                                else
                                {
                                    //NavContext.SetNumbersControlCuadranteSQEditar("");
                                    model.TieneDatosGrid = false;
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
            return View("Index", model);
        }       

        [HttpPost]
        public ActionResult SaveNCSQADD(SQModel model)
        {
            bool FaltaDatoC = false, FaltaDatoNC = false;
            model.SeleccionAgregarEditar = "1";
            if (ModelState.IsValid /*&& ValidaModel(model)*/&& model.ProjectIdADD > 0)
            {
                if (model.SearchTypeADD == "c")
                {
                    if (model.QuadrantIdCADD == 0)
                    {
                        FaltaDatoC = true;
                    }
                }
                else
                {
                    if (model.QuadrantIdNCADD == 0)
                    {
                        FaltaDatoNC = true;
                    }
                }

                if (FaltaDatoC || FaltaDatoNC)
                {
                    TempData["errorSaveAdd"] = "Porfavor Seleccione Un Cuadrante";
                }
                else
                {
                    ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdADD);
                    List<LayoutGridSQ> currentControlNumbers = Helps.GetListadoCuadrantesNumeroControlSQ(NavContext.GetCurrentNCSQ());
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
                                    //string SQConsecutivo = OrdenTrabajoSpoolBO.Instance.GuardarNumeroControlSQ(ToDataTable.Instance.toDataTable(currentControlNumbers), SessionFacade.UserId, project.ID, "");
                                    string SQConsecutivo = OrdenTrabajoSpoolBO.Instance.GuardarNumeroControlSQ(ToDataTable.Instance.toDataTable(ListaNoTieneSQInterno), SessionFacade.UserId, SessionFacade.NombreCompleto, project.ID, "");
                                    NavContext.SetNumbersControlCuadranteSQ("");//cuando guardo elimino la lista en memoria de los spools cargados
                                    if (SQConsecutivo == model.SQ)
                                    {
                                        TempData["ActualizadoCorrecto"] = "SQ: " + model.SQ + " Actualizado Correctamente";
                                    }
                                    else if (SQConsecutivo != "0")
                                    {
                                        TempData["success"] = SQConsecutivo;
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
                                NavContext.SetNumbersControlCuadranteSQ("");
                            }
                        }
                    }
                }
                    
            }
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult SaveNCSQADDEdit(SQModel model)
        {            
            model.SeleccionAgregarEditar = "2";
            if (ModelState.IsValid/* && ValidaModel(model)*/&& model.ProjectIdEditar > 0)
            {                
                if (model.SQ == "" || model.SQ == null)
                {
                    TempData["errorSaveEdit"] = "Porfavor Ingrese SOL. INSPECT";
                }
                else
                {
                    ProyectoCache project = UserScope.MisProyectos.Single(p => p.ID == model.ProjectIdEditar);
                    List<LayoutGridSQ> currentControlNumbers = Helps.GetListadoCuadrantesNumeroControlSQEditar(NavContext.GetCurrentNCSQEditar());
                    List<LayoutGridSQ> ListaConDatosNumeroControl = OrdenTrabajoSpoolBO.Instance.ListaNumControlConSpoolID(ToDataTable.Instance.toDataTable(currentControlNumbers));
                    string datosAsignados = "";
                    //List<LayoutGridSQ> listaSoloNumeroControlNuevos = (from a in currentControlNumbers join b in ListaConDatosNumeroControl on a.OrdenTrabajoSpoolID equals b.OrdenTrabajoSpoolID select b).ToList();

                    List<LayoutGridSQ> ListaTieneSQCliente = (from a in ListaConDatosNumeroControl where (a.SqCliente != "" && a.SqCliente != null) select a).ToList();
                    List<LayoutGridSQ> ListaNoTieneSQCliente = (from a in ListaConDatosNumeroControl where (a.SqCliente == "" || a.SqCliente == null) select a).ToList();
                    for (int a = 0; a < ListaTieneSQCliente.Count; a++)
                    {
                        datosAsignados += " El Spool " + ListaTieneSQCliente[a].NumeroControl + " tiene SQ Cliente Agregado, se ignora guardado. <br>";
                    }
                    int cont = 0;
                    foreach(var item in ListaNoTieneSQCliente)
                    {
                        if(item.SQ != "" && item.SQ != null)
                        {
                            cont++;
                        }
                    }
                    //Condicion que evalua si todos los spools de un sq son iguales solamente muestra mensaje de editado correcto ya que no hay cambios
                    if(cont > 0) 
                    {
                        TempData["ActualizadoCorrecto"] = "SQ: " + model.SQ + " Actualizado Correctamente";
                    }
                    else
                    {
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
                        if (ListaNoTieneSQInterno.Count > 0)
                        {
                            try
                            {
                                string SQConsecutivo = OrdenTrabajoSpoolBO.Instance.GuardarNumeroControlSQ(ToDataTable.Instance.toDataTable(ListaNoTieneSQInterno), SessionFacade.UserId, SessionFacade.NombreCompleto, model.ProjectIdEditar, model.SQ);
                                NavContext.SetNumbersControlCuadranteSQEditar("");//cuando guardo elimino la lista en memoria de los spools cargados
                                if (SQConsecutivo == model.SQ)
                                {
                                    TempData["ActualizadoCorrecto"] = "SQ: " + model.SQ + " Actualizado Correctamente";
                                }
                                else if (SQConsecutivo != "0")
                                {
                                    TempData["success"] = SQConsecutivo;
                                }
                            }
                            catch (Exception e) { }
                        }
                        else
                        {
                            NavContext.SetNumbersControlCuadranteSQEditar("");
                        }
                    }                    
                }
            }
            return View("Index", model);
        }
       
        public ActionResult DeleteNumeroControlSQ(string numeroControlSQ)
        {
            string ncSQ = NavContext.GetCurrentNCSQ();
            List<LayoutGridSQ> listaNcSQ = Helps.GetListadoCuadrantesNumeroControlSQ(NavContext.GetCurrentNCSQ());
            LayoutGridSQ ncActualSQ = listaNcSQ.Where(x => x.NumeroControl == numeroControlSQ).FirstOrDefault();
            if (ncActualSQ != null)
            {
                listaNcSQ.Remove(ncActualSQ);
            }                                      
            NavContext.SetNumbersControlCuadranteSQ(Helps.GetNumberControlsSQCookies(listaNcSQ));

            return View("Index", GetModelSQ());
        }
        public ActionResult DeleteNumeroControlSQEditar(string numeroControlSQ)
        {
            string ncSQ = NavContext.GetCurrentNCSQEditar();
            List<LayoutGridSQ> listaNcSQ = Helps.GetListadoCuadrantesNumeroControlSQEditar(NavContext.GetCurrentNCSQEditar());
            LayoutGridSQ ncActualSQ = listaNcSQ.Where(x => x.NumeroControl == numeroControlSQ).FirstOrDefault();
            if (ncActualSQ != null)
            {
                listaNcSQ.Remove(ncActualSQ);
            }

            NavContext.SetNumbersControlCuadranteSQEditar(Helps.GetNumberControlsSQCookies(listaNcSQ));

            return View("Index", GetModelSQEditar());
        }

        private SQModel GetModelSQ()
        {
            List<LayoutGridSQ> listaElementos = Helps.GetListadoCuadrantesNumeroControlSQ(NavContext.GetCurrentNCSQ());
            SQModel sqlModel = new SQModel();
            sqlModel.ListaElementos = listaElementos;
            sqlModel.SeleccionAgregarEditar = "1";
            sqlModel.ProjectIdADD = NavContext.GetCurrentProjectSQ().ID;            
            return sqlModel;
        }

        private SQModel GetModelSQEditar()
        {
            List<LayoutGridSQ> listaElementos = Helps.GetListadoCuadrantesNumeroControlSQEditar(NavContext.GetCurrentNCSQEditar());
            SQModel sqlModel = new SQModel();
            sqlModel.ListaElementosPorSQ = listaElementos;
            sqlModel.SeleccionAgregarEditar = "2";
            sqlModel.ProjectIdEditar = NavContext.GetCurrentProjectSQEditar().ID;                               
            sqlModel.SQ = NavContext.GetSQ();
            return sqlModel;
        }


    }
}