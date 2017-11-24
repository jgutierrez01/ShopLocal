using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using Org.BouncyCastle.Asn1.Ocsp;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Controles.ImportarProyecto
{
    public partial class TablaDestajos : System.Web.UI.UserControl
    {

        #region variables publicas


        public int ProyectoID
        {
            get { return hdnProyectoID.Value.SafeIntParse(); }
            set { hdnProyectoID.Value = value.ToString(); }
        }

        private int ProyectoExportaID
        {
            get
            {
                return ViewState["ProyectoExportaID"].SafeIntParse();
            }
            set
            {
                ViewState["ProyectoExportaID"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                cargaCombos();
            }
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            ProyectoExportaID = ddlProyecto2.SelectedValue.SafeIntParse();
            if (ImportaDestajos())
            {
                lblMensajeExito.Text = string.Format(lblMensajeExito.Text, ddlProyecto2.SelectedItem.Text);
                phMensajeExito.Visible = true;
            }
        }

        protected void ddlProyecto2_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProyecto2.SelectedIndex != 0)
            {
                btnImportar.Attributes["OnClick"] = "return Sam.Confirma(21);";
            }
            else
            {
                btnImportar.Attributes["OnClick"] = string.Empty;
            }
        }

        private void cargaCombos()
        {
            ddlProyecto2.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        private bool ImportaDestajos()
        {
            bool resultado = true;

            try
            {
                ProyectoConfiguracionBO.Instance.ImportaDestajos(ProyectoExportaID, ProyectoID, SessionFacade.UserId);
            }
            catch (BaseValidationException ex)
            {
                foreach (string detail in ex.Details)
                {
                    var cv = new CustomValidator
                    {
                        ErrorMessage = detail,
                        IsValid = false,
                        Display = ValidatorDisplay.None,
                        ValidationGroup = "vgTabla"
                    };
                    Page.Form.Controls.Add(cv);
                }
                resultado = false;
            }

            return resultado;
        }
    }
}