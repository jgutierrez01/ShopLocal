using System;
using System.Globalization;
using System.Linq;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Administracion;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using System.Web.UI.WebControls;

namespace SAM.Web.Catalogos
{
    public partial class ImportaPeq : SamPaginaPrincipal
    {

        private int _tipoJuntaId;
        private int _famAcerId;
        private int _proyectoId;

        protected void Page_Init(object sender, EventArgs e)
        {
            rdArchivo.ControlObjectsVisibility = ControlObjectsVisibility.None;
            rdArchivo.Culture = new CultureInfo(LanguageHelper.CustomCulture);
            
            titulo.NavigateUrl = string.Format( WebConstants.CatalogoUrl.TBL_PEQ,
                                                Request.Params["TID"].SafeIntParse(),
                                                Request.Params["FID"].SafeIntParse(),
                                                Request.Params["PID"].SafeIntParse());
        }

        // recibe del parametro el id correspondiente al dropdown
        protected void Page_Load(object sender, EventArgs e)
        {
            _tipoJuntaId = Request.Params["TID"].SafeIntParse();
            _famAcerId = Request.Params["FID"].SafeIntParse();
            _proyectoId = Request.Params["PID"].SafeIntParse();

            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_PulgadasEquivalentes);
                TipoJuntaCache tipoJunta =
                    CacheCatalogos.Instance.ObtenerTiposJunta().SingleOrDefault(x => x.ID == _tipoJuntaId);
                FamAceroCache famAcero =
                    CacheCatalogos.Instance.ObtenerFamiliasAcero().SingleOrDefault(x => x.ID == _famAcerId);
                ProyectoCache proyecto = CacheCatalogos.Instance.ObtenerProyectos().SingleOrDefault(x => x.ID == _proyectoId);

                if (tipoJunta != null)
                {
                    txtTipoJunta.Text = tipoJunta.Text;
                }
                if(famAcero!=null)
                {
                    txtFamAcero.Text = famAcero.Text;
                }


                //txtFamAcero.Text 
                txtProyecto.Text = (proyecto != null ? proyecto.Nombre : string.Empty);
            }
        }


        // si la extension del archivo es valida, se procesara
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                try
                {
                    UploadedFile f = rdArchivo.UploadedFiles[0];
                    PeqBL.Instance.ProcesaPeq(f.InputStream, SessionFacade.UserId, _tipoJuntaId, _famAcerId, _proyectoId);
                    string url = string.Format(WebConstants.CatalogoUrl.TBL_PEQ,
                                               _tipoJuntaId,
                                               _famAcerId,
                                               _proyectoId);
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