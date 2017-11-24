using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Administracion;
using Mimo.Framework.Extensions;

namespace SAM.Web.Administracion
{
    /// <summary>
    /// Esta página proporciona la funcionalidad para dar de alta un nuevo periodo de destajo.
    /// El UI solicita al usuario los datos del periodo, tales como año, semana, fechas y días festivos.
    /// En base a eso se verifica que el periodo no exista ya en la BD, en caso de no existir el sistema calcula
    /// el destajo correspondiente y redirecciona al usuario a la página con el detalle del periodo.
    /// </summary>
    public partial class AltaPeriodoDestajo : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Destajos);
            }
        }

        /// <summary>
        /// Genera el nuevo periodo de destajo y después redirecciona al detalle del mismo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGenerar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                try
                {
                    int periodoDestajoID =
                        DestajoBL.Instance.GeneraNuevoPeriodoDestajo(txtAnio.Text.SafeIntParse(),
                                                                     txtSemana.Text,
                                                                     dtpInicio.SelectedDate.Value.Date,
                                                                     dtpFinal.SelectedDate.Value.Date,
                                                                     txtDiasFestivos.Text.SafeIntParse(),
                                                                     SessionFacade.UserId);

                    Response.Redirect(string.Format(WebConstants.AdminUrl.DetallePeriodoDestajo, periodoDestajoID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
    }
}