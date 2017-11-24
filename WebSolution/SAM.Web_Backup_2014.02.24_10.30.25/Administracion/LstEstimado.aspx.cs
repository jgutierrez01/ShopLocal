using System;
using System.Collections.Generic;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Produccion;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Administracion
{
    public partial class LstEstimado : SamPaginaPrincipal
    {

        #region ViewState de los filtros

        /// <summary>
        /// ID del proyecto seleccionado en el dropdown
        /// </summary>
        private int _proyecto
        {
            get
            {
                    return (int)ViewState["proyecto"];
            }
            set
            {
                ViewState["proyecto"] = value;
            }
        }

        /// <summary>
        /// fecha de inicio en la cual se hara el filtro
        /// </summary>
         private DateTime? _desde
         {
             get
             {
                 if (ViewState["desde"] != null)
                 {
                     return (DateTime)ViewState["desde"];
                 }
                 return null;
             }
             set
             {
                 ViewState["desde"] = value;
             }
         }

         /// <summary>
         ///  fecha final en la cual se hara el filtro
         /// </summary>
         private DateTime? _hasta
         {
             get
             {
                 if (ViewState["hasta"] != null)
                 {
                     return (DateTime)ViewState["hasta"];
                 }
                 return null;
             }
             set
             {
                 ViewState["hasta"] = value;
             }
         }

        /// <summary>
        /// numero de estimacion a buscar en el filtro
        /// </summary>
         private string _numeroEstimaciones
         {
             get
             {
                 if (ViewState["numeroEstimaciones"] != null)
                 {
                     return (string)ViewState["numeroEstimaciones"];
                 }
                 return null;
             }
             set
             {
                 ViewState["numeroEstimaciones"] = value;
             }
         }

        #endregion

         protected void Page_Load(object sender, EventArgs e)
        {
           if (!Page.IsPostBack)
           {
               Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Estimaciones);
                CargaCombo();
            }
        }

        //metodo para cargar el combo "ddlProyecto".
        private void CargaCombo()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        /// <summary>
        /// manda a llamar el rebind del grdEstimacion para que se generen los renglones
        /// </summary>
        protected void btnMostrar_OnClick(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                //Guardar en ViewState los filtros
                _proyecto = ddlProyecto.SelectedValue.SafeIntParse();
                _desde = dtpDesde.SelectedDate;
                _hasta = dtpHasta.SelectedDate;
                _numeroEstimaciones = txtNumeroEstimaciones.Text;

                phGrid.Visible = true;
                grdEstimacion.Rebind();
            }
        }

        /// <summary>
        /// trae el DataSource filtrado con sus resultados para mostrar al grdEstimacion
        /// </summary>
        private void EstablecerDataSource()
        {
            //llama a llenar el grid con los filtros
            grdEstimacion.DataSource = EstimacionBO.Instance.ObtenerConFiltros(_proyecto, 
                                                                                _desde, 
                                                                                _hasta,
                                                                                _numeroEstimaciones
                                                                               );
        }

        /// <summary>
        /// desplega el header del proyecto
        /// </summary>
        protected void ddlProyecto_SelectedItemChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                proyHeader.BindInfo(proyectoID);
                proyHeader.Visible = true;
            }
            else
            {
                proyHeader.Visible = false;
            }
        }

        /// <summary>
        /// llena el grid con la informacion requerida
        /// </summary>
        protected void grdEstimacion_OnNeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        /// <summary>
        /// actualiza el grdEstimacion
        /// </summary>
        protected void lnkActualizar_onClick(object sender, EventArgs e)
        {
            grdEstimacion.ResetBind();
            grdEstimacion.Rebind();
        }

        /// <summary>
        /// Accion dependiendo al botton del templeate seleccionado
        /// Borrar. Borra el registro seleccionado
        /// </summary>
        protected void grdEstimado_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                try
                {
                    int estimacionID = e.CommandArgument.SafeIntParse();
                    EstimacionBO.Instance.Borra(estimacionID);
                    EstablecerDataSource();
                    grdEstimacion.DataBind();
                }
                catch (BaseValidationException ex)
                {
                   RenderErrors(ex);
                }
            }
       }
    }
}