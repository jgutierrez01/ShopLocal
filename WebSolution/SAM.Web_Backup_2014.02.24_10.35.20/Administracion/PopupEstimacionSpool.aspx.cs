using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using SAM.Web.Classes;
using System.Web;

namespace SAM.Web.Administracion
{
    public partial class PopupEstimacionSpool : SamPaginaPopup
    {
        #region ViewStates
        /// <summary>
        /// ID del proyecto en cuestión
        /// </summary>
        private int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        /// <summary>
        /// CSV string que contiene los IDs de los spools a los cuales se le van a generar las ODTs
        /// </summary>
        private string WorkStatusSpoolIds
        {
            get
            {
                return ViewState["WorkStatusSpoolIds"].ToString();
            }
            set
            {
                ViewState["WorkStatusSpoolIds"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProyectoID = Request.QueryString["PID"].SafeIntParse();

                if (!SeguridadQs.TieneAccesoAProyecto(ProyectoID))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando generar una estimación de spools para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, ProyectoID);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                WorkStatusSpoolIds = Request.QueryString["JWIDS"];
                MostrarEstimacionNueva();
                carga();
            }
        }

        /// <summary>
        /// genera el ddl y el listbox del popup
        /// </summary>
        private void carga()
        {
            radNuevaEstimacion.Checked = true;
            //se genera el ddl
            ddlNumeroEstimacionExistente.BindToEnumerableWithEmptyRow(EstimacionBO.Instance.obtenerEstimaciones(ProyectoID), "NumeroEstimacion", "EstimacionID", null);

            //de generan los checkbox
            List<ConceptoEstimacion> conceptoEstimacion = ConceptoEstimacionBO.Instance.ObtenerConceptosEstimacionSpool();

            chkEstimaciones.DataSource = conceptoEstimacion;
            if (LanguageHelper.CustomCulture == LanguageHelper.ESPANOL)
            {
                chkEstimaciones.DataTextField = "Nombre";
            }
            else
            {
                chkEstimaciones.DataTextField = "NombreIngles";
            }
            chkEstimaciones.DataValueField = "ConceptoEstimacionID";
            chkEstimaciones.DataBind();
            
        }

        /// <summary>
        /// hace todas las validaciones correspondientes y al final agrega el concepto estimacion a la estimacionSpool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEstimar_Click(object sender, EventArgs e)
        {
            int[] ids = WorkStatusSpoolIds.Split(',').Select(n => int.Parse(n)).ToArray();
            using (SamContext ctx = new SamContext())
            {
                if (Page.IsValid)
                {
                    try
                    {
                        EstimacionSpoolBO.Instance.chkBoxSeleccionados(ChkBoxSeleccionados());

                        EstimacionSpoolBO.Instance.WorkStatusSpoolSinConcepto(ConceptoSinEstimacion(ids));

                        if (radNuevaEstimacion.Checked)
                        {
                            if ((EstimacionSpoolBO.ExisteEstimacion(ctx, txtNumerioEstimacionNueva.Text, ProyectoID)))
                            {
                                EstimacionBO.Guarda(CreaEstimacionPopUp());
                                GenerarConceptosSpools(ids, txtNumerioEstimacionNueva.Text);
                                phControles.Visible = false;
                                phMensaje.Visible = true;
                            }
                        }
                        else
                        {
                            GenerarConceptosSpools(ids, ddlNumeroEstimacionExistente.SelectedItem.ToString());
                            phControles.Visible = false;
                            phMensaje.Visible = true;
                        }
                        JsUtils.RegistraScriptActualizaGridGenerico(this);
                    }
                    catch (BaseValidationException bve)
                    {
                        RenderErrors(bve);
                    }
                }
            }
        }

        private List<string> ConceptoSinEstimacion(int[] ids)
        {
            List<string> conceptos = new List<string>();
            foreach (var id in ids)
            {
                foreach (ListItem chk in chkEstimaciones.Items)
                {
                    if (chk.Selected)
                    {
                        int estimacion = chk.Value.SafeIntParse();
                        EstimacionSpool estSpool = EstimacionSpoolBO.Instance.TraerEstimacionSpool(id, estimacion);

                        if (estSpool != null)
                        {
                            WorkstatusSpool Spool = JuntaSpoolBO.Instance.ObtenerWorkStatusspoolPorID(id);
                            if (LanguageHelper.CustomCulture == LanguageHelper.ESPANOL)
                            {
                                conceptos.Add("Spool " + Spool.OrdenTrabajoSpool.NumeroControl +
                                              " ya ha sido estimada bajo el concepto: \"" + chk.Text + "\"");
                            }
                            else
                            {
                                conceptos.Add("Spool " + Spool.OrdenTrabajoSpool.NumeroControl +
                                              " has been estimated under the concept: \"" + chk.Text + "\"");
                            }
                        }
                    }
                }
            }
            return conceptos;
        }

        private bool ChkBoxSeleccionados()
        {
            foreach (ListItem chk in chkEstimaciones.Items)
            {
                if (chk.Selected)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// crea el objeto de estimacionJunta y lo guarda en la base de datos, por cada ConceptoEstimacion seleccionado
        /// </summary>
        /// <param name="ids">ids de los juntaWorkstatus</param>
        /// <param name="numeroEstimacion">el numero de la esrtimacion que se agregara al spool</param>
        private void GenerarConceptosSpools(int[] ids, string numeroEstimacion)
        {
            List<int> Estimaciones = new List<int>();

            foreach (ListItem chk in chkEstimaciones.Items)
            {
                if (chk.Selected)
                {
                    Estimaciones.Add(chk.Value.SafeIntParse());
                }
            }

            EstimacionSpoolBO.Guarda(ids, numeroEstimacion, Estimaciones);
        }

        /// <summary>
        /// genera el objeto estimacion para despues mandarlo a guardar a la base de datos
        /// </summary>
        /// <returns>Estimacion</returns>
        private Estimacion CreaEstimacionPopUp()
        {
            Estimacion estimacion = new Estimacion();
            estimacion.NumeroEstimacion = txtNumerioEstimacionNueva.Text;
            estimacion.FechaEstimacion = dtpFechaEstimacion.SelectedDate.Value;
            estimacion.ProyectoID = ProyectoID;
            estimacion.FechaModificacion = DateTime.Now;
            estimacion.UsuarioModifica = SessionFacade.UserId;
            return estimacion;
        }  
        
        /// <summary>
        /// llama al metodo para ocultar el lado de estimacion existente 
        /// y mostrar la de estimacion nueva
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radNuevaEstimacion_OnCheckedChanged(object sender, EventArgs e)
        {
            MostrarEstimacionNueva();
        }

        /// <summary>
        /// llama al metodo para ocultar el lado de estimacion nueva 
        /// y mostrar la de estimacion existente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void radEstimacionExistente_OnCheckedChanged(object sender, EventArgs e)
        {
            MostrarEstimacionExistente();
        }

        /// <summary>
        /// oculta el lado de estimacion nueva 
        /// y muestra la de estimacion existente
        /// </summary>
        private void MostrarEstimacionExistente()
        {
            pnExistente.Visible = true;
            pnNuevo.Visible = false;
        }

        /// <summary>
        /// oculta el lado de estimacion existente 
        /// y muestra la de estimacion nueva
        /// </summary>
        private void MostrarEstimacionNueva()
        {
            pnExistente.Visible = false;
            pnNuevo.Visible = true;
            dtpFechaEstimacion.SelectedDate = DateTime.Now;
        }

    }
}

////////////////////////////////////////////////////////////////