using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using System.Web;

namespace SAM.Web.Administracion
{
    public partial class PopupEstimacionJunta : SamPaginaPopup
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
        private string JuntaWorkStatusIds
        {
            get
            {
                return ViewState["JuntaWorkStatusIds"].ToString();
            }
            set
            {
                ViewState["JuntaWorkStatusIds"] = value;
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
                    string mensaje = string.Format("El usuario {0} está intentando generar una estimación de juntas para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, ProyectoID);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                JuntaWorkStatusIds = Request.QueryString["JWIDS"];
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
           List<ConceptoEstimacion> conceptoEstimacion = ConceptoEstimacionBO.Instance.ObtenerConceptosEstimacionJunta();

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
            pnlExistente.Visible = true;
            pnlNueva.Visible = false;
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
        /// oculta el lado de estimacion existente 
        /// y muestra la de estimacion nueva
        /// </summary>
        private void MostrarEstimacionNueva()
        {
           dtpFechaEstimacion.SelectedDate = DateTime.Now;           
            pnlExistente.Visible = false;
            pnlNueva.Visible = true;
        }

        /// <summary>
        /// hace todas las validaciones correspondientes y al final agrega el concepto estimacion a la estimacionJunta
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEstimar_Click(object sender, EventArgs e)
        {
            int[] ids = JuntaWorkStatusIds.Split(',').Select(n => int.Parse(n)).ToArray();
            
                if (Page.IsValid)
                {
                    try
                    {
                        EstimacionJuntaBO.Instance.SinchkBoxSeleccionados(ChkBoxSeleccionados());

                        EstimacionJuntaBO.Instance.JuntaWorkStatusSinConcepto(ConceptoSinEstimacion(ids));
                        
                        if (radNuevaEstimacion.Checked)
                        {
                            EstimacionJuntaBO.Instance.ExisteEstimacion(txtNumerioEstimacionNueva.Text, ProyectoID);
                            
                            EstimacionBO.Guarda(CreaEstimacionPopUp());
                            GenerarConceptosJuntas(ids, txtNumerioEstimacionNueva.Text);
                            phControles.Visible = false;
                            phMensaje.Visible = true;
                        }
                        else
                        {
                            GenerarConceptosJuntas(ids, ddlNumeroEstimacionExistente.SelectedItem.ToString());
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
                        EstimacionJunta estJunt = EstimacionJuntaBO.Instance.TraerEstimacionJunta(id, estimacion);

                        if (estJunt != null)
                        {
                            JuntaWorkstatus JuntaWs = JuntaWorkstatusBO.Instance.ObtenerJuntaWorkStatusPorID(id);
                            
                            if (LanguageHelper.CustomCulture == LanguageHelper.ESPANOL)
                            {

                                conceptos.Add("Junta " + JuntaWs.OrdenTrabajoSpool.NumeroControl + " / " + JuntaWs.EtiquetaJunta +
                                              " ya ha sido estimada bajo el concepto: \"" + chk.Text + "\"");
                            }
                            else
                            {
                                conceptos.Add("Joint wel " + JuntaWs.OrdenTrabajoSpool.NumeroControl + " / " + JuntaWs.EtiquetaJunta +
                                              " has already been estimated under the concept: \"" + chk.Text + "\"");
                            }
                        }
                    }
                }
            }
            return conceptos;
        }



        /// <summary>
        /// crea el objeto de estimacionJunta y lo guarda en la base de datos, por cada ConceptoEstimacion seleccionado
        /// </summary>
        /// <param name="ids">ids de los juntaWorkstatus</param>
        /// <param name="numeroEstimacion">el numero de la esrtimacion que se agregara a la junta</param>
        private void GenerarConceptosJuntas(int[] ids, string numeroEstimacion)
        {
            List<int> Estimaciones = new List<int>();
     
            foreach (ListItem chk in chkEstimaciones.Items)
            {
                if (chk.Selected)
                {
                    Estimaciones.Add(chk.Value.SafeIntParse());
                }
            }
            EstimacionJuntaBO.Guarda(ids,numeroEstimacion, Estimaciones);
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

    }
}