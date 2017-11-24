using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using SAM.BusinessLogic.Administracion;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Common;
using System.Globalization;

namespace SAM.Web.Proyectos
{
    public partial class PesoItemCode : SamPaginaPrincipal
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

            rdArchivo.Culture = new CultureInfo(LanguageHelper.CustomCulture);

            if (!Page.IsPostBack)
            {
                ProyectoID = Request.QueryString["ID"].SafeIntParse();

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_ItemCodes, ProyectoID);
                headerProyecto.BindInfo(ProyectoID);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_ITEM_CODES, ProyectoID);
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
                    PesoItemCodeBL.Instance.ProcesaPeso(f.InputStream, SessionFacade.UserId, ProyectoID);
                    string url = string.Format(WebConstants.ProyectoUrl.LST_ITEM_CODES, ProyectoID);
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