using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Administracion;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;

namespace SAM.Web.Catalogos
{
    public partial class ImportaEspesores : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_Espesores);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            rdArchivo.ControlObjectsVisibility = ControlObjectsVisibility.None;
            rdArchivo.Culture = new CultureInfo(LanguageHelper.CustomCulture);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Validate();
            if(IsValid)
            {
                try
                {
                    UploadedFile f = rdArchivo.UploadedFiles[0];
                    EspesorBL.Instance.ProcesaArchivoEspesores(f.InputStream, SessionFacade.UserId);
                    Response.Redirect(WebConstants.CatalogoUrl.TBL_ESPESOR);
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

            if(rdArchivo.UploadedFiles.Count > 0)
            {
                UploadedFile uf = rdArchivo.UploadedFiles[0];
                string extension = uf.GetExtension();

                if(extension.EqualsIgnoreCase(".csv"))
                {
                    args.IsValid = true;
                }
            }
        }
    }
}