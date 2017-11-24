using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Mimo.Framework.Extensions;
using SAM.Entities;
using SAM.BusinessObjects.Ingenieria;
using SAM.Web.Classes;
using SAM.BusinessLogic.Ingenieria;
using SAM.BusinessObjects.Catalogos;
using System.Text.RegularExpressions;
using SAM.BusinessObjects.Workstatus;
using SAM.Web.Common;

namespace SAM.Web.Ingenieria
{
    public partial class PopUpJuntaAdicional : System.Web.UI.Page
    {
        #region Propiedades privadas
        private int MaterialSpoolID
        {
            get
            {
                return (int)ViewState["MaterialSpoolID"];
            }
            set
            {
                ViewState["MaterialSpoolID"] = value;
            }
        }
        private MaterialSpool MatSpool
        {
            get
            {
                return (MaterialSpool)ViewState["MatSpool"];
            }
            set
            {
                ViewState["MatSpool"] = value;
            }
        }
        private List<JuntaSpool> JuntasSpool
        {
            get
            {
                return (List<JuntaSpool>)ViewState["JuntasSpool"];
            }
            set
            {
                ViewState["JuntasSpool"] = value;
            }
        }
        private TipoJunta TipoJuntaBW
        {
            get
            {
                return (TipoJunta)ViewState["TipoJuntaBW"];
            }
            set
            {
                ViewState["TipoJuntaBW"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MaterialSpoolID = Request.QueryString["ID"].SafeIntParse();
                MatSpool = MaterialSpoolBO.Instance.Obtener(MaterialSpoolID);
                JuntasSpool = JuntaSpoolBO.Instance.ObtenerJuntasPorSpoolID(MatSpool.SpoolID).ToList();
                TipoJuntaBW = TipoJuntaBO.Instance.ObtenerTodos().FirstOrDefault(x => x.Codigo == TipoJuntas.BW);

                cargaDatosMaterial();
                cargaCombos();
                sugiereEtiquetaMaterial();
            }
            else
            {
                cargaDatosMaterial();
            }
        }

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            var materiales = MaterialSpoolBO.Instance.ObtenerDetalleMaterialPorSpool(MatSpool.SpoolID);
            var cortes = CorteSpoolBO.Instance.ObtenerPorSpool(MatSpool.SpoolID);

            bool errorDistanciaMenor = txtDistancia.Text.SafeIntParse() > MatSpool.Cantidad;
            bool errorJuntaEtiquetaRepetida = JuntasSpool.Any(x => x.Etiqueta == txtEtiqueta.Text);
            bool errorMaterialConDespacho = MatSpool.Despacho.Any(x => x.Cancelado == false);
            bool errorMaterialEtiquetaRepetida = materiales.Any(x => x.Etiqueta == txtEtiquetaMaterial.Text);
            bool errorCorteEtiquetaRepetida = cortes.Any(x => x.EtiquetaSeccion == txtEtiquetaTubo.Text);

            #region Custom Validators
            if (errorDistanciaMenor)
            {
                var cv = new CustomValidator
                {
                    ErrorMessage = MensajesErrorUI.Excepcion_DistanciaMenor,
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "valJuntaAdicional"
                };
                Page.Form.Controls.Add(cv);
            }
            if (errorJuntaEtiquetaRepetida)
            {
                var cv = new CustomValidator
                {
                    ErrorMessage = string.Format(MensajesErrorUI.Excepcion_JuntaEtiqueta, txtEtiqueta.Text),
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "valJuntaAdicional"
                };
                Page.Form.Controls.Add(cv);
            }
            if (errorMaterialConDespacho)
            {
                var cv = new CustomValidator
                {
                    ErrorMessage = MensajesErrorUI.Excepcion_MaterialDespachado,
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "valJuntaAdicional"
                };
                Page.Form.Controls.Add(cv);
            }
            if (errorMaterialEtiquetaRepetida)
            {
                var cv = new CustomValidator
                {
                    ErrorMessage = string.Format(MensajesErrorUI.Excepcion_MaterialEtiqueta, txtEtiquetaMaterial.Text),
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "valJuntaAdicional"
                };
                Page.Form.Controls.Add(cv);
            }
            if (errorCorteEtiquetaRepetida)
            {
                var cv = new CustomValidator
                {
                    ErrorMessage = string.Format(MensajesErrorUI.Excepcion_SegmentoExistente, txtEtiquetaTubo.Text),
                    IsValid = false,
                    Display = ValidatorDisplay.None,
                    ValidationGroup = "valJuntaAdicional"
                };
                Page.Form.Controls.Add(cv);
            }
            #endregion

            if (Page.IsValid)
            {
                guardaHistoricos();
                guarda();

                if (Page.IsValid)
                {
                    JsUtils.RegistraScriptActualizayCierraJuntaAdicional(this.Page);
                }
            }
        }

        protected void btnCancelar_OnClick(object sender, EventArgs e)
        {
            JsUtils.RegistraScriptCierraVentana(this.Page);
        }

        /// <summary>
        /// Carga la información del material spool seleccionado
        /// Carga los datos principales de las juntas involucradas al material
        /// </summary>
        private void cargaDatosMaterial()
        {
            lblItemCode.Text += MatSpool.ItemCode.Codigo;
            lblDescripcion.Text += MatSpool.DescripcionMaterial;
            lblCantidad.Text += MatSpool.Cantidad;
            lblDiametro1.Text += MatSpool.Diametro1;
            lblDiametro2.Text += MatSpool.Diametro2;
            lblEtiqueta.Text += MatSpool.Etiqueta;

            CorteSpool corteSpool = CorteSpoolBO.Instance.ObtenerPorSpool(MatSpool.SpoolID)
                                                         .FirstOrDefault(x => x.EtiquetaMaterial == MatSpool.Etiqueta);
            lblEtiquetaSeccion.Text = corteSpool.EtiquetaSeccion;


            string etiquetaMaterial;
            // Se comprueba si existen juntas con la etiqueta tal cual del material
            bool existeJunta = JuntasSpool.Any(x => (x.EtiquetaMaterial1 == MatSpool.Etiqueta || 
                                                     x.EtiquetaMaterial2 == MatSpool.Etiqueta
                                                    ) && x.TipoJuntaID == TipoJuntaBW.TipoJuntaID);
            if (existeJunta)
            {
                etiquetaMaterial = MatSpool.Etiqueta;
            }
            else
            {
                // agregamos un cero inicial al nombre de la etiqueta
                etiquetaMaterial = Regex.Replace(MatSpool.Etiqueta, "[^0-9]", "");
                etiquetaMaterial = etiquetaMaterial.Length == 1 ? etiquetaMaterial = string.Format("0{0}", MatSpool.Etiqueta) : MatSpool.Etiqueta;
            }

            var juntasSpoolInvolucradas = JuntasSpool
                                          .Where(x => (x.EtiquetaMaterial2 == etiquetaMaterial ||
                                                       x.EtiquetaMaterial1 == etiquetaMaterial
                                                      ) && x.TipoJuntaID == TipoJuntaBW.TipoJuntaID)
                                          .ToList();

            var juntasPorNumeros = juntasSpoolInvolucradas.Where(x => !Char.IsLetter(x.Etiqueta.First())).ToList();
            var listaJuntasSpool = juntasPorNumeros.OrderBy(x => Regex.Replace(x.Etiqueta, "[^0-9]", "").SafeIntParse()).ToList();
            listaJuntasSpool.AddRange(juntasSpoolInvolucradas.Except(listaJuntasSpool).OrderBy(x => x.Etiqueta));

            Literal litInfoJunta;
            string litEtiqueta = this.GetLocalResourceObject("Etiqueta").ToString();
            string litLocalizacion = this.GetLocalResourceObject("Localizacion").ToString();
            string lblInfoJunta = string.Concat("<b>", litEtiqueta, "</b> {0}<br /><b>", litLocalizacion, "</b> {1}<br /><br />");
            foreach (JuntaSpool juntaInvolucrada in listaJuntasSpool)
            {
                litInfoJunta = new Literal();
                litInfoJunta.Text = string.Format(lblInfoJunta,
                                                  juntaInvolucrada.Etiqueta,
                                                  string.Concat(juntaInvolucrada.EtiquetaMaterial1, 
                                                                " - ",
                                                                juntaInvolucrada.EtiquetaMaterial2)
                                                  );
                pnlJuntas.Controls.Add(litInfoJunta);
            }

            juntaCamposExtras.Visible = listaJuntasSpool.Count != 0 ? false : true;
        }

        /// <summary>
        /// Sugiere la etiqueta del nuevo material en base a las ya existentes dentro del spool
        /// </summary>
        private void sugiereEtiquetaMaterial()
        {
            var materiales = MaterialSpoolBO.Instance.ObtenerDetalleMaterialPorSpool(MatSpool.SpoolID);
            int etiquetaNumericaConsecutiva = (from m in materiales
                                               select Regex.Replace(m.Etiqueta, "[^0-9]", "").SafeIntParse()
                                              ).OrderByDescending(x => x)
                                               .First() + 1;

            txtEtiquetaMaterial.Text = etiquetaNumericaConsecutiva.ToString();
        }

        /// <summary>
        /// Guarda los historicos del spool
        /// </summary>
        private void guardaHistoricos()
        {
            IngenieriaBL.Instance.GeneraHistoriales(MatSpool.SpoolID);
        }

        /// <summary>
        /// Guardamos cambios en juntas
        /// </summary>
        private void guarda()
        {
            int cantidadOriginal;
            string etiquetaOriginal;

            #region EdiciónMaterial
            MaterialSpool materialSpool = MaterialSpoolBO.Instance.Obtener(MaterialSpoolID);
            cantidadOriginal = materialSpool.Cantidad;
            etiquetaOriginal = materialSpool.Etiqueta;

            materialSpool.StartTracking();
            materialSpool.Cantidad = txtDistancia.Text.SafeIntParse();
            materialSpool.Peso = (materialSpool.Cantidad * materialSpool.Peso) / cantidadOriginal;
            materialSpool.UsuarioModifica = SessionFacade.UserId;
            materialSpool.FechaModificacion = DateTime.Now;
            materialSpool.StopTracking();
            #endregion
            #region EdiciónCorte
            CorteSpool corteSpool = CorteSpoolBO.Instance.ObtenerPorSpool(materialSpool.SpoolID)
                                                         .FirstOrDefault(x => x.EtiquetaMaterial == materialSpool.Etiqueta);
            corteSpool.StartTracking();
            corteSpool.Cantidad = materialSpool.Cantidad;
            corteSpool.UsuarioModifica = SessionFacade.UserId;
            corteSpool.FechaModificacion = DateTime.Now;
            corteSpool.StopTracking();
            #endregion

            #region CreaciónMaterial
            MaterialSpool nuevoMaterialSpool = new MaterialSpool();
            nuevoMaterialSpool.StartTracking();
            nuevoMaterialSpool.SpoolID = materialSpool.SpoolID;
            nuevoMaterialSpool.ItemCodeID = materialSpool.ItemCodeID;
            nuevoMaterialSpool.Diametro1 = materialSpool.Diametro1;
            nuevoMaterialSpool.Diametro2 = materialSpool.Diametro2;
            nuevoMaterialSpool.Cantidad = (cantidadOriginal - txtDistancia.Text.SafeIntParse());
            nuevoMaterialSpool.Etiqueta = txtEtiquetaMaterial.Text;
            nuevoMaterialSpool.Peso = (nuevoMaterialSpool.Cantidad * materialSpool.Peso) / cantidadOriginal;
            nuevoMaterialSpool.Especificacion = materialSpool.Especificacion;
            nuevoMaterialSpool.Grupo = materialSpool.Grupo;
            nuevoMaterialSpool.UsuarioModifica = SessionFacade.UserId;
            nuevoMaterialSpool.FechaModificacion = DateTime.Now;
            nuevoMaterialSpool.DescripcionMaterial = materialSpool.DescripcionMaterial;
            nuevoMaterialSpool.StopTracking();
            #endregion
            #region CreaciónCorte
            CorteSpool nuevoCorteSpool = new CorteSpool();
            nuevoCorteSpool.StartTracking();
            nuevoCorteSpool.SpoolID = corteSpool.SpoolID;
            nuevoCorteSpool.ItemCodeID = corteSpool.ItemCodeID;
            nuevoCorteSpool.TipoCorte1ID = corteSpool.TipoCorte1ID;
            nuevoCorteSpool.TipoCorte2ID = corteSpool.TipoCorte2ID;
            nuevoCorteSpool.EtiquetaMaterial = nuevoMaterialSpool.Etiqueta;
            nuevoCorteSpool.EtiquetaSeccion = txtEtiquetaTubo.Text;
            nuevoCorteSpool.Diametro = corteSpool.Diametro;
            nuevoCorteSpool.InicioFin = corteSpool.InicioFin;
            nuevoCorteSpool.Cantidad = nuevoMaterialSpool.Cantidad;
            nuevoCorteSpool.UsuarioModifica = SessionFacade.UserId;
            nuevoCorteSpool.FechaModificacion = DateTime.Now;
            nuevoCorteSpool.StopTracking();
            #endregion

            #region Edición de localización de juntas
            // Editamos localización de juntas con el material editado
            string etiquetaMaterial;

            bool existeJunta = JuntasSpool.Any(x => (x.EtiquetaMaterial1 == etiquetaOriginal ||
                                                     x.EtiquetaMaterial2 == etiquetaOriginal
                                                    ) && x.TipoJuntaID == TipoJuntaBW.TipoJuntaID);
            if (existeJunta)
            {
                etiquetaMaterial = etiquetaOriginal;
            }
            else
            {
                etiquetaMaterial = Regex.Replace(etiquetaOriginal, "[^0-9]", "");
                etiquetaMaterial = etiquetaMaterial.Length == 1 ? etiquetaMaterial = string.Format("0{0}", etiquetaOriginal) : etiquetaOriginal;
            }

            var juntasSpool = JuntasSpool.Where(x => (x.EtiquetaMaterial1 == etiquetaOriginal ||
                                                      x.EtiquetaMaterial2 == etiquetaOriginal
                                                     ) && x.TipoJuntaID == TipoJuntaBW.TipoJuntaID
                                               ).ToList();

            var juntasPorNumeros = juntasSpool.Where(x => !Char.IsLetter(x.Etiqueta.First())).ToList();
            var listaJuntasSpool = juntasPorNumeros.OrderBy(x => Regex.Replace(x.Etiqueta, "[^0-9]", "").SafeIntParse()).ToList();
            listaJuntasSpool.AddRange(juntasSpool.Except(listaJuntasSpool).OrderBy(x => x.Etiqueta));

            JuntaSpool juntaMenor = listaJuntasSpool.FirstOrDefault();
            JuntaSpool juntaMayor = null;
            if (listaJuntasSpool.Count > 1)
            {
                // Existe junta mayor, la actualizamos
                juntaMayor = listaJuntasSpool.Last();
                juntaMayor.StartTracking();
                if (juntaMayor.EtiquetaMaterial1 == etiquetaMaterial)
                {
                    juntaMayor.EtiquetaMaterial1 = nuevoMaterialSpool.Etiqueta.Length == 1 ? string.Format("0{0}", nuevoMaterialSpool.Etiqueta) 
                                                                                           : nuevoMaterialSpool.Etiqueta;
                }
                else
                {
                    juntaMayor.EtiquetaMaterial2 = nuevoMaterialSpool.Etiqueta.Length == 1 ? string.Format("0{0}", nuevoMaterialSpool.Etiqueta)
                                                                                           : nuevoMaterialSpool.Etiqueta;
                }
                juntaMayor.UsuarioModifica = SessionFacade.UserId;
                juntaMayor.FechaModificacion = DateTime.Now;
                juntaMayor.StopTracking();
            }

            List<JuntaSpool> juntasNoBW = obtenerJuntasNoBWSinSeleccionar();
            foreach(JuntaSpool juntaSpool in juntasNoBW)
            {
                juntaSpool.StartTracking();
                if (juntaSpool.EtiquetaMaterial1 == etiquetaMaterial)
                {
                    juntaSpool.EtiquetaMaterial1 = nuevoMaterialSpool.Etiqueta.Length == 1 ? string.Format("0{0}", nuevoMaterialSpool.Etiqueta)
                                                                                           : nuevoMaterialSpool.Etiqueta;
                }
                else
                {
                    juntaSpool.EtiquetaMaterial2 = nuevoMaterialSpool.Etiqueta.Length == 1 ? string.Format("0{0}", nuevoMaterialSpool.Etiqueta)
                                                                                           : nuevoMaterialSpool.Etiqueta;
                }
                juntaSpool.UsuarioModifica = SessionFacade.UserId;
                juntaSpool.FechaModificacion = DateTime.Now;
                juntaSpool.StopTracking();
            }
            #endregion

            #region CreaciónJuntaSpool
            JuntaSpool nuevaJuntaSpool = new JuntaSpool();
            nuevaJuntaSpool.StartTracking();
            nuevaJuntaSpool.SpoolID = MatSpool.SpoolID;
            nuevaJuntaSpool.TipoJuntaID = TipoJuntaBW.TipoJuntaID;
            nuevaJuntaSpool.FabAreaID = FabAreaBO.Instance.ObtenerTodos().FirstOrDefault(x => x.Codigo == FabAreas.SHOP).FabAreaID;
            nuevaJuntaSpool.Etiqueta = txtEtiqueta.Text;
            nuevaJuntaSpool.EtiquetaMaterial1 = etiquetaMaterial;
            nuevaJuntaSpool.EtiquetaMaterial2 = Regex.Replace(nuevoMaterialSpool.Etiqueta, "[^0-9]", "").Length == 1 ? string.Concat("0", nuevoMaterialSpool.Etiqueta) : nuevoMaterialSpool.Etiqueta;
            nuevaJuntaSpool.Cedula = juntaMenor != null ? juntaMenor.Cedula : ddlCedula.SelectedItem.Text;
            nuevaJuntaSpool.FamiliaAceroMaterial1ID = juntaMenor != null ? juntaMenor.FamiliaAceroMaterial1ID : ddlFamiliaAcero.SelectedValue.SafeIntParse();
            nuevaJuntaSpool.FamiliaAceroMaterial2ID = juntaMenor != null ? juntaMenor.FamiliaAceroMaterial2ID : null;
            nuevaJuntaSpool.Diametro = juntaMenor != null ? juntaMenor.Diametro : nuevoMaterialSpool.Diametro1;
            nuevaJuntaSpool.Espesor = juntaMenor != null ? juntaMenor.Espesor : null;
            nuevaJuntaSpool.KgTeoricos = juntaMenor != null ? juntaMenor.KgTeoricos : null;
            nuevaJuntaSpool.Peqs = juntaMenor != null ? juntaMenor.Peqs : null;
            nuevaJuntaSpool.UsuarioModifica = SessionFacade.UserId;
            nuevaJuntaSpool.FechaModificacion = DateTime.Now;
            nuevaJuntaSpool.EsManual = juntaMenor != null ? juntaMenor.EsManual : true;
            nuevaJuntaSpool.StopTracking();
            #endregion

            if (Page.IsValid)
            {
                MaterialSpoolBO.Instance.Guarda(materialSpool);
                MaterialSpoolBO.Instance.Guarda(nuevoMaterialSpool);
                CorteSpoolBO.Instance.Guarda(corteSpool);
                CorteSpoolBO.Instance.Guarda(nuevoCorteSpool);
                if (juntaMayor != null)
                    JuntaSpoolBO.Instance.Guarda(juntaMayor);
                if(juntasNoBW.Count != 0)
                    JuntaSpoolBO.Instance.GuardaJuntas(juntasNoBW);
                JuntaSpoolBO.Instance.Guarda(nuevaJuntaSpool);

                // agregamos la nueva junta al bastón de la junta menor en caso de que cuente con una.
                if (juntaMenor != null)
                {
                    BastonSpoolJunta nuevoBastonSpoolJunta;
                    var bastonesJuntasSpool = BastonBO.Instance.ObtenerPorSpool(juntaMenor.SpoolID)
                                                               .SelectMany(x => x.BastonSpoolJunta).ToList();
                    BastonSpoolJunta bastonSpoolJunta = bastonesJuntasSpool.FirstOrDefault(x => x.JuntaSpoolID == juntaMenor.JuntaSpoolID);
                    if (bastonSpoolJunta != null)
                    {
                        nuevoBastonSpoolJunta = new BastonSpoolJunta();
                        nuevoBastonSpoolJunta.StartTracking();
                        nuevoBastonSpoolJunta.BastonSpoolID = bastonSpoolJunta.BastonSpoolID;
                        nuevoBastonSpoolJunta.JuntaSpoolID = nuevaJuntaSpool.JuntaSpoolID;
                        nuevoBastonSpoolJunta.UsuarioModifica = SessionFacade.UserId;
                        nuevoBastonSpoolJunta.FechaModificacion = DateTime.Now;
                        nuevoBastonSpoolJunta.StopTracking();

                        BastonSpool baston = BastonBO.Instance.Obtener(nuevoBastonSpoolJunta.BastonSpoolID);
                        baston.BastonSpoolJunta.Add(nuevoBastonSpoolJunta);
                        BastonBO.Instance.Guarda(baston);
                    }
                }
            }
        }

        private void cargaCombos()
        {
            if (juntaCamposExtras.Visible)
            {
                ddlCedula.DataSource = CedulaBO.Instance.ObtenerTodos();
                ddlCedula.DataTextField = "Codigo";
                ddlCedula.DataValueField = "CedulaID";
                ddlCedula.DataBind();

                ddlFamiliaAcero.DataSource = FamiliaAceroBO.Instance.ObtenerTodas();
                ddlFamiliaAcero.DataTextField = "Nombre";
                ddlFamiliaAcero.DataValueField = "FamiliaAceroID";
                ddlFamiliaAcero.DataBind();
            }

            string etiquetaMaterial;
            // Se comprueba si existen juntas con la etiqueta tal cual del material
            bool existeJunta = JuntasSpool.Any(x => (x.EtiquetaMaterial1 == MatSpool.Etiqueta ||
                                                     x.EtiquetaMaterial2 == MatSpool.Etiqueta
                                                    ) && x.TipoJuntaID != TipoJuntaBW.TipoJuntaID);
            if (existeJunta)
            {
                etiquetaMaterial = MatSpool.Etiqueta;
            }
            else
            {
                // agregamos un cero inicial al nombre de la etiqueta
                etiquetaMaterial = Regex.Replace(MatSpool.Etiqueta, "[^0-9]", "");
                etiquetaMaterial = etiquetaMaterial.Length == 1 ? etiquetaMaterial = string.Format("0{0}", MatSpool.Etiqueta) : MatSpool.Etiqueta;
            }

            chklJuntas.DataSource = (from j in JuntasSpool
                                     where (j.EtiquetaMaterial1 == etiquetaMaterial ||
                                           j.EtiquetaMaterial2 == etiquetaMaterial) &&
                                           j.TipoJuntaID != TipoJuntaBW.TipoJuntaID
                                     let _descripcion = string.Format("{0}, ({1}-{2})", j.Etiqueta, j.EtiquetaMaterial1, j.EtiquetaMaterial2)
                                     select new
                                     {
                                         JuntaSpoolID = j.JuntaSpoolID,
                                         Descripcion = _descripcion
                                     }).ToList();
            chklJuntas.DataValueField = "JuntaSpoolID";
            chklJuntas.DataTextField = "Descripcion";
            chklJuntas.DataBind();
        }

        private List<JuntaSpool> obtenerJuntasNoBWSinSeleccionar()
        {
            List<int> juntasNoBWIds = chklJuntas.Items
                                        .Cast<ListItem>()
                                        .Where(x => !x.Selected)
                                        .Select(x => x.Value.SafeIntParse())
                                        .ToList();
            return (from j in JuntasSpool
                    where juntasNoBWIds.Contains(j.JuntaSpoolID)
                    select j).ToList();
        }
    }
}