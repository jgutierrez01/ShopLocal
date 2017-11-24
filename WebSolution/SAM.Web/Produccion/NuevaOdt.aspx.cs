using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Proyectos;
using SAM.Entities;
using SAM.BusinessLogic.Produccion;
using Mimo.Framework.Exceptions;
using SAM.Web.Common;

namespace SAM.Web.Produccion
{
    /// <summary>
    /// Genera una nueva orden de trabajo en blanco para el proyecto especificado.
    /// </summary>
    public partial class NuevaOdt : SamPaginaPrincipal
    {

        /// <summary>
        /// Carga el combo de proyectos con aquellos proyectos a los que el usuario
        /// tiene acceso.  Por default inicializa la fecha de la ODT a la fecha del día de hoy.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_CruceForzado);
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
                dtpFecha.SelectedDate = DateTime.Now;
            }
        }


        /// <summary>
        /// Al cambiar el proyecto seleccionado se auto-calcula el siguiente consecutivo
        /// sugerido por el sistema y se actualiza el drop-down con la lista de talleres
        /// disponibles para ese proyecto.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID > 0)
            {
                txtOdt.Text = ProyectoBO.Instance.SiguienteConsecutivoOdt(proyectoID).ToString();
                ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorProyecto(proyectoID));
                proyEncabezado.BindInfo(proyectoID);
                proyEncabezado.Visible = true;
            }
            else
            {
                ddlTaller.Items.Clear();
                txtOdt.Text = string.Empty;
                proyEncabezado.Visible = false;
            }
        }

        /// <summary>
        /// Se dispara cuando el usuario da click en el botón de generar ODT.
        /// Manda llamar un método de business logic que se encarga de las validaciones
        /// de negocio así como de generar la nueva ODT.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCruceForzado_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    OrdenTrabajo odt =
                    OrdenTrabajoBL.Instance.GeneraNueva(ddlProyecto.SelectedValue.SafeIntParse(),
                                                        ddlTaller.SelectedValue.SafeIntParse(),
                                                        txtOdt.Text.SafeIntParse(),
                                                        dtpFecha.SelectedDate.Value,
                                                        SessionFacade.UserId,
                                                        false);

                    //Redireccionar a la página que muestra el detalle de la orden de trabajo
                    Response.Redirect(string.Format(WebConstants.ProduccionUrl.DetalleOdt, odt.OrdenTrabajoID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCruceForzadoAsignado_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    OrdenTrabajo odt =
                    OrdenTrabajoBL.Instance.GeneraNueva(ddlProyecto.SelectedValue.SafeIntParse(),
                                                        ddlTaller.SelectedValue.SafeIntParse(),
                                                        txtOdt.Text.SafeIntParse(),
                                                        dtpFecha.SelectedDate.Value,
                                                        SessionFacade.UserId,
                                                        true);

                    //Redireccionar a la página que muestra el detalle de la orden de trabajo
                    Response.Redirect(string.Format(WebConstants.ProduccionUrl.DetalleOdt, odt.OrdenTrabajoID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void brnCruceForzadoCsv_Click(object sender, EventArgs e)
        {
            Response.Redirect(WebConstants.ProduccionUrl.CrucePorImportacionCsv, true);
        }
    }
}