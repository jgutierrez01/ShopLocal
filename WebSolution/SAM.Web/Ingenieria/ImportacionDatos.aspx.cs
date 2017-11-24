using System;
using System.Web;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using Resources;
using SAM.BusinessLogic.Ingenieria;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Modelo;


namespace SAM.Web.Ingenieria
{
    public partial class ImportacionDatos : SamPaginaPrincipal
    {
        const int MAXIMO_ELEMENTOS = 100;
        
        private bool ArchivosSubidos
        {
            get
            {
                if (ViewState["ArchivosSubidos"] ==null)
                {
                    return false;
                }
                return (bool)ViewState["ArchivosSubidos"];
            }
            set
            {
                ViewState["ArchivosSubidos"] = value;
            }
        }

       
        protected void Page_Init(object sender, EventArgs e)
        {
            
            //Hacemos que no se vea ningun control del radupload que permita agregar o quitar archivos
            RadUpload1.ControlObjectsVisibility = ControlObjectsVisibility.None;
            //establecemos en donde se almacenaran los archivos 
            RadUpload1.TargetPhysicalFolder = InterpreteDatos.PathParaAlmacenarArchivos;
            //establecemos la cultura del radupload
            RadUpload1.Culture = new CultureInfo(Cultura);

            //en caso que no exista la ruta para almacenar los archivosd la creamos
            if (!Directory.Exists(InterpreteDatos.PathParaAlmacenarArchivos))
            {
                Directory.CreateDirectory(InterpreteDatos.PathParaAlmacenarArchivos);
            }

            //si es la primera vez que se accede a la pagina eliminamos los archivos subidos previamente para este usuario
            if (!IsPostBack)
            {
                Directory.EnumerateFiles(InterpreteDatos.PathParaAlmacenarArchivos).ToList().ForEach(File.Delete);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.ing_ImportacionDatos);
            
            estableceVisibilidades();
        }

        private void estableceVisibilidades()
        {
            //Si ya se subieron los archivos haremos visible el panel que muestra la retroalimentacion
            RadPanelBar1.Visible = ArchivosSubidos;
            //el control apra cargar los archivos solo es visible si aun no se suben los archivos
            pnlInfoCarga.Visible = !ArchivosSubidos;
            //la informacion acerca de los archivos es visible si ya se subieron
            pnlInfoArchivosCargados.Visible = ArchivosSubidos;
            //el boton de revertir es visible si ya se subieron los archivos
            btnRevertir.Visible = ArchivosSubidos;
            phValidacionesSegundaPantalla.Visible = ArchivosSubidos;
            phValidacionesIniciales.Visible = !ArchivosSubidos;
            proyHeader.Visible = ArchivosSubidos;
            filtroGenerico.ProyectoEnabled = !ArchivosSubidos;
        }

        

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ArchivoSimple[] archivos = new ArchivoSimple[4];
            int i = 0;
            foreach (UploadedFile f in RadUpload1.UploadedFiles)
            {

                if (!File.Exists(InterpreteDatos.PathParaAlmacenarArchivos + InterpreteDatos.NombresArchivos[i]))
                {
                    File.Move(InterpreteDatos.PathParaAlmacenarArchivos + f.GetName(), InterpreteDatos.PathParaAlmacenarArchivos + InterpreteDatos.NombresArchivos[i]);
                }

                archivos[i++] = new ArchivoSimple
                                 {
                                     Nombre = f.GetNameWithoutExtension(),
                                     Stream = f.InputStream
                                 };
            }
            pnlCargaSinErrores.Visible = InterpretaDatos(archivos, false);
            ArchivosSubidos = true;
            estableceVisibilidades();
        }

        private InterpreteDatos _interpreteDatos { get; set; }

        private bool InterpretaDatos(ArchivoSimple[] archivos, bool registraNoEncontrados)
        {
            bool defaultRetorno = false;
            List<int> spoolIdNoHomologar =
                repHomologacion.Items.Cast<RepeaterItem>().Where(x => ((RadioButton) x.FindControl("rbRechazar")).Checked)
                    .Select(x => ((RadioButton)x.FindControl("rbRechazar")).Attributes["Value"].SafeIntParse()).ToList();

            int proyectoID =
                filtroGenerico.ProyectoSelectedValue.SafeIntParse();
            List<FamiliaAcero> familiaAceros = (from RepeaterItem item in repFamiliasAcero.Items
                                                let ddlFamMaterial = (DropDownList)item.FindControl("ddlFamilia")
                                                let lblAcero = (Label)item.FindControl("lblAcero")
                                                select new FamiliaAcero
                                                            {
                                                                Nombre = lblAcero.Text,
                                                                FamiliaMaterialID = ddlFamMaterial.SelectedValue.SafeIntParse()
                                                            }).ToList();
            _interpreteDatos = new InterpreteDatos(proyectoID, archivos, familiaAceros, SessionFacade.UserId);
            if (registraNoEncontrados)
            {
                return _interpreteDatos.ProcesaYRegistraSiEsNecesario(familiaAceros, spoolIdNoHomologar);
            }
            else
            {
                _interpreteDatos.ProcesaSiNoFaltanDatos();

                repErrores.DataSource = _interpreteDatos.Errores.Take(MAXIMO_ELEMENTOS);
                repErrores.DataBind();
                RadPanelBar1.Items.FindItemByValue("Errores").Visible = _interpreteDatos.Errores.Count > 0;

                /*if (_interpreteDatos.SpoolsHomologables.Count > 0 || _interpreteDatos.SpoolsNoHomologables.Count > 0)
                {
                    pnlHomologacion.Visible = true;
                    phSpoolDespachos.Visible = true;
                }*/

                if (_interpreteDatos.Errores.Count == 0)
                {
                    repErrores.Visible = false;
                    defaultRetorno = true;
                    pnlCargaSinErrores.Visible = true;
                    btnRegistar.Visible = true;
                    phInstruccionesAceptar.Visible = true;

                   /* List<Spool> listaHomologacion = new List<Spool>(_interpreteDatos.SpoolsHomologables);
                    listaHomologacion.AddRange(_interpreteDatos.SpoolsNoHomologables);
                    repHomologacion.DataSource = listaHomologacion;
                    repHomologacion.DataBind();*/

                    repCortes.DataSource = _interpreteDatos.TipoCorteNoEncontrados.Take(MAXIMO_ELEMENTOS);
                    repCortes.DataBind();
                    RadPanelBar1.Items.FindItemByValue("Cortes").Visible =
                        _interpreteDatos.TipoCorteNoEncontrados.Count > 0;

                    repJuntas.DataSource = _interpreteDatos.TipoJuntaNoEncontradas.Take(MAXIMO_ELEMENTOS);
                    repJuntas.DataBind();
                    RadPanelBar1.Items.FindItemByValue("Juntas").Visible =
                        _interpreteDatos.TipoJuntaNoEncontradas.Count > 0;

                    repFabAreas.DataSource = _interpreteDatos.FabAreasNoEncontradas.Take(MAXIMO_ELEMENTOS);
                    repFabAreas.DataBind();
                    RadPanelBar1.Items.FindItemByValue("FabAreas").Visible =
                        _interpreteDatos.FabAreasNoEncontradas.Count > 0;

                    repDiametros.DataSource = _interpreteDatos.DiametrosNoEncontrados.Take(MAXIMO_ELEMENTOS);
                    repDiametros.DataBind();
                    RadPanelBar1.Items.FindItemByValue("Diametros").Visible =
                        _interpreteDatos.DiametrosNoEncontrados.Count > 0;

                    repCedulas.DataSource = _interpreteDatos.CedulasNoEncontradas.Take(MAXIMO_ELEMENTOS);
                    repCedulas.DataBind();
                    RadPanelBar1.Items.FindItemByValue("Cedulas").Visible =
                        _interpreteDatos.CedulasNoEncontradas.Count > 0;

                    lblSpoolsImportados.Text = string.Format(lblSpoolsImportados.Text,
                                                                _interpreteDatos.NumSpoolsEnArchivo);
                    

                    /*lblSpoolsImportables.Text = string.Format(lblSpoolsImportables.Text,
                                                              _interpreteDatos.SpoolsNuevos.Count +
                                                              _interpreteDatos.SpoolsQueCambioRevision.Count -
                                                              _interpreteDatos.SpoolsNoHomologables.Count -
                                                              _interpreteDatos.SpoolsHomologables.Count);
                     * 
                    lblSpoolDespachos.Text = string.Format(lblSpoolDespachos.Text,
                                                           _interpreteDatos.SpoolsHomologables.Count +
                                                           _interpreteDatos.SpoolsNoHomologables.Count,
                                                           _interpreteDatos.SpoolsHomologables.Count,
                                                           _interpreteDatos.SpoolsNoHomologables.Count);*/

                    List<string> reversed = _interpreteDatos.ItemCodeNoEncontrados.ToList();
                    reversed.Reverse();

                    var itemCodes = (from item in reversed
                                     select
                                         new
                                             {
                                                 itemCode = item.ToLowerInvariant().Split('|')[0],
                                                 descripcion = item.ToLowerInvariant().Split('|')[1]
                                             }).ToList();
                    repItemCodes.DataSource = itemCodes;
                    repItemCodes.DataBind();
                    RadPanelBar1.Items.FindItemByValue("ItemCodes").Visible = itemCodes.Count > 0;

                    repFamiliasAcero.DataSource = _interpreteDatos.FamiliaAceroNoEncontradas;
                    repFamiliasAcero.DataBind();
                    RadPanelBar1.FindItemByValue("Aceros").Visible = _interpreteDatos.FamiliaAceroNoEncontradas.Count > 0;
                    
                    RadPanelBar1.DataBind();
                }
                else
                {
                    pnlCargaSinErrores.Visible = false;
                    RadPanelBar1.Items.Cast<RadPanelItem>().ToList().Where(x => !x.Value.EqualsIgnoreCase("Errores")).
                        ToList().ForEach(x => x.Visible = false);
                }
            }


            return defaultRetorno;
        }

        
        protected void rep_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(WebUtils.IsItem(e.Item))
            {
                ((Label) e.Item.FindControl("lblAcero")).Text = e.Item.DataItem.ToString();
                DropDownList ddlFamilia = (DropDownList)e.Item.FindControl("ddlFamilia");
                ddlFamilia.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasMaterial());
            }
        }

        
        protected void btnRegistar_OnClick(object sender, EventArgs e)
        {
            ArchivoSimple[] archivos = InterpreteDatos.ObtenerArchivosSubidos();
            
            if(InterpretaDatos(archivos,true))
            {
                using (SamContext ctx = new SamContext()) 
                {
                    /*
                     CATEGORIAS 
                        0 - SPOOLS TOTALMENTE NUEVOS 
                        1 - SPOOLS SIN ORDEN DE TRABAJO 
                        2 - SPOOLS CON ORDEN DE TRABAJO (POR HOMOLOGAR) 
                        3 - SPOOLS DESCARTADOS 
                     */

                    string SessionID = HttpContext.Current.Session.SessionID; 
                    int ProjectoID = filtroGenerico.ProyectoSelectedValue.SafeIntParse(); 

                    var dtsSummaryLogEntries =
                        (
                            from a in ctx.DTSSummaryLog
                            where a.SAMWebSessionID == SessionID  &&
                                    a.SAMWebProjectID == ProjectoID
                            select a
                        );

                    List<LigaMensaje> ligasMensaje = new List<LigaMensaje>();

                    LigaMensaje ligaMensaje = null;

                    if (dtsSummaryLogEntries != null && dtsSummaryLogEntries.Count() > 0)
                    {
                        foreach (DTSSummaryLog dtsSummaryLogEntry in dtsSummaryLogEntries)
                        {
                            switch (dtsSummaryLogEntry.Category)
                            {
                                case (short)Entities.ResumenSpoolsImportados.Nuevos: //0 - SPOOLS NUEVOS 
                                        ligaMensaje = new LigaMensaje
                                        {
                                            Texto = dtsSummaryLogEntry.CountByCategory.ToString() + " " + MensajesIngenieria.ImportacionDatos_SpoolsNuevos.ToString(),
                                            Url = string.Format(WebConstants.IngenieriaUrl.LST_INGENIERIAPID, ProjectoID) 
                                        };
                                        ligasMensaje.Add(ligaMensaje);
                                    break;
                                case (short)Entities.ResumenSpoolsImportados.SinODT: //1 - SPOOLS SIN ORDEN DE TRABAJO 
                                        ligaMensaje = new LigaMensaje
                                        {
                                            Texto = dtsSummaryLogEntry.CountByCategory.ToString() + " " + MensajesIngenieria.ImportacionDatos_SpoolsReemplazados.ToString(),
                                            Url = string.Format(WebConstants.IngenieriaUrl.LST_INGENIERIAPID, ProjectoID) 
                                        };
                                        ligasMensaje.Add(ligaMensaje);
                                    break;
                                case (short)Entities.ResumenSpoolsImportados.ConODT: //2 - SPOOLS CON ORDEN DE TRABAJO (POR HOMOLOGAR) 
                                        ligaMensaje = new LigaMensaje
                                        {
                                            Texto = dtsSummaryLogEntry.CountByCategory.ToString() + " " + MensajesIngenieria.ImportacionDatos_SpoolsHomologar.ToString(),
                                            Url = string.Format(WebConstants.IngenieriaUrl.PENDIENTES_HOMOLOGAR, ProjectoID) 
                                        };
                                        ligasMensaje.Add(ligaMensaje);
                                    break;
                                case (short)Entities.ResumenSpoolsImportados.Descartados: //3 - SPOOLS DESCARTADOS POR NO SER VERSION ANTERIOR
                                    ligaMensaje = new LigaMensaje
                                    {
                                        Texto = dtsSummaryLogEntry.CountByCategory.ToString() + " " + MensajesIngenieria.ImportacionDatos_SpoolsDescartados.ToString(),
                                        Url = string.Format(WebConstants.IngenieriaUrl.LST_INGENIERIAPID, ProjectoID)
                                    };
                                    ligasMensaje.Add(ligaMensaje);
                                    break;
                            }
                        }
                    }
                    else 
                    {
                        ligaMensaje = new LigaMensaje
                        {
                            Texto = MensajesIngenieria.ImportacionDatos_SinSpools,
                            Url = string.Format(WebConstants.IngenieriaUrl.LST_INGENIERIAPID, ProjectoID) 
                        };
                        ligasMensaje.Add(ligaMensaje);
                    }

                    ligaMensaje = new LigaMensaje
                    {
                        Texto = MensajesIngenieria.ImportacionDatos_LigaImportacionDatos,
                        Url = WebConstants.IngenieriaUrl.IMPORTACION_DATOS
                    };
                    ligasMensaje.Add(ligaMensaje);


                    UtileriaRedireccion.RedireccionaExitoIngenieria(MensajesIngenieria.ImportacionDatos_Titulo, MensajesIngenieria.ImportacionDatos_MensajeExito, ligasMensaje);

                        /*UtileriaRedireccion.RedireccionaExitoIngenieria(MensajesIngenieria.ImportacionDatos_Titulo
                                                                        , MensajesIngenieria.ImportacionDatos_MensajeExito,
                                                                        new List<LigaMensaje>
                                                                    {
                                                                        new LigaMensaje
                                                                            {
                                                                                Texto =
                                                                                    MensajesIngenieria.
                                                                                    ImportacionDatos_LigaImportacionDatos,
                                                                                Url =
                                                                                    WebConstants.IngenieriaUrl.
                                                                                    IMPORTACION_DATOS
                                                                            },
                                                                        new LigaMensaje
                                                                            {
                                                                                Texto =
                                                                                    MensajesIngenieria.
                                                                                    Ingenieria_ListadoIngenieria,
                                                                                Url =
                                                                                    WebConstants.IngenieriaUrl.
                                                                                    LST_INGENIERIA
                                                                            }
                                                                    });*/
                }
            }
        }

        protected void btnRevertirClick(object sender, EventArgs e)
        {
          Response.Redirect(WebConstants.IngenieriaUrl.IMPORTACION_DATOS);
        }

        protected void repHomologacion_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                Literal litManual = (Literal) e.Item.FindControl("litResolverManualmente");
                RadioButton rbRechazar = (RadioButton) e.Item.FindControl("rbRechazar");
                RadioButton rbAceptar = (RadioButton) e.Item.FindControl("rbAceptar");
                HyperLink hlNombreSpool = (HyperLink) e.Item.FindControl("hlNombreSpool");
                Spool spool = e.Item.DataItem as Spool;
                //bool spoolHomologable =
                //    !_interpreteDatos.SpoolsNoHomologables.Any(x => x.Nombre.EqualsIgnoreCase(spool.Nombre));

                //litManual.Visible = !spoolHomologable;
                //rbRechazar.Visible = spoolHomologable;
                //rbAceptar.Visible = spoolHomologable;
                //rbRechazar.GroupName = "rb" + spool.SpoolID;
                //rbAceptar.GroupName = "rb" + spool.SpoolID;
                //rbRechazar.Attributes["Value"] = spool.SpoolID.ToString();



                //hlNombreSpool.Text = spool.Nombre;
                //hlNombreSpool.NavigateUrl = "javascript:Sam.Ingenieria.AbrePopupHomologacion(" + spool.SpoolID + "," + filtroGenerico.ProyectoSelectedValue + "," + (spoolHomologable? 0: 1) +" )";
            }
        }
    }
}

