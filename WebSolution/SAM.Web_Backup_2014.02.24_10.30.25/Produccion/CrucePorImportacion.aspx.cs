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
using SAM.BusinessLogic.Produccion;


namespace SAM.Web.Produccion
{
    public partial class CrucePorImportacion : SamPaginaPrincipal
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnCruzar_Click(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                try
                {
                    UploadedFile f = rdArchivo.UploadedFiles[0];
                    OrdenTrabajoBL.Instance.AgregaSpoolPorImportacion(f.InputStream, SessionFacade.UserId);
                    Response.Redirect(WebConstants.ProduccionUrl.ListaOrdenesDeTrabajo);
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