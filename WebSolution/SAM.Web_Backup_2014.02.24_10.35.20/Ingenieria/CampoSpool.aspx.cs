using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using Mimo.Framework.Common;
using SAM.Web.Classes;
using Telerik.Web.UI;
using SAM.BusinessLogic.Administracion;

namespace SAM.Web.Ingenieria
{
    public partial class CampoSpool : SamPaginaPrincipal
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
                    if (rdArchivoSpool.UploadedFiles.Count > 0)
                    {
                        UploadedFile f = rdArchivoSpool.UploadedFiles[0];
                        CampoSpoolBL.Instance.ProcesaPeso(f.InputStream, SessionFacade.UserId, ProyectoID);
                    }

                    if (rdArchivoJuntaSpool.UploadedFiles.Count > 0)
                    {
                        UploadedFile f = rdArchivoJuntaSpool.UploadedFiles[0];
                        CamposJuntaSpoolBL.Instance.ProcesaCamposAdicionales(f.InputStream, SessionFacade.UserId, ProyectoID);
                    }
                    
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

            if (rdArchivoSpool.UploadedFiles.Count > 0)
            {
                UploadedFile uf = rdArchivoSpool.UploadedFiles[0];
                string extension = uf.GetExtension();

                if (extension.EqualsIgnoreCase(".csv"))
                {
                    args.IsValid = true;
                }
            }

            if (rdArchivoJuntaSpool.UploadedFiles.Count > 0)
            {
                UploadedFile uf = rdArchivoJuntaSpool.UploadedFiles[0];
                string extension = uf.GetExtension();

                if (extension.EqualsIgnoreCase(".csv"))
                {
                    args.IsValid = true;
                }
            }
        }
    }
}