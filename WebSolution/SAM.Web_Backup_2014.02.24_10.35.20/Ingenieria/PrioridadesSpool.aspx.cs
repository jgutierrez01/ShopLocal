using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessLogic.Administracion;

namespace SAM.Web.Ingenieria
{
    public partial class PrioridadesSpool : SamPaginaPrincipal
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(Classes.WebConstants.SubMenuItemEnum.ing_IngenieriaProyecto);

                ProyectoID = Request.QueryString["ID"].SafeIntParse();
                headerProyecto.BindInfo(ProyectoID);
                titulo.NavigateUrl = string.Format(WebConstants.IngenieriaUrl.LST_INGENIERIAPID, ProyectoID);
            }
        }

        public void btnGuardar_OnClick(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                try
                {
                    UploadedFile f = rdArchivo.UploadedFiles[0];
                    CampoSpoolBL.Instance.ProcesaPrioridades(f.InputStream, SessionFacade.UserId, ProyectoID);
                    string url = string.Format(WebConstants.IngenieriaUrl.LST_INGENIERIAPID, ProyectoID);
                    Response.Redirect(url);
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void cusExtensionArchivos_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = false;

            if (rdArchivo.UploadedFiles.Count > 0)
            {
                UploadedFile uf = rdArchivo.UploadedFiles[0];
                string extension = uf.GetExtension();

                if (extension.EqualsIgnoreCase(".csv"))
                {
                    args.IsValid = true;
                }
            }
        }
    }
}