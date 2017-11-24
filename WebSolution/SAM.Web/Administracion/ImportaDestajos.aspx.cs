using System;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic.Administracion;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;

namespace SAM.Web.Administracion
{
    public partial class ImportaDestajos : SamPaginaPrincipal
    {
        private int _proyectoId;
        private int _famAceroId;
        private int _tipoJuntaId;
        private int _procesoId;
        private string _procesoValor;

        protected void Page_Init(object sender, EventArgs e)
        {
            rdArchivo.ControlObjectsVisibility = ControlObjectsVisibility.None;
            rdArchivo.Culture = new CultureInfo(LanguageHelper.CustomCulture);
            titulo.NavigateUrl = string.Format(WebConstants.AdminUrl.TBL_DESTAJOS,
                                               Request.Params["PTOID"].SafeIntParse(),
                                               Request.Params["FID"].SafeIntParse(),
                                               Request.Params["TID"].SafeIntParse(),
                                               Request.Params["PSO"].SafeStringParse(),
                                               Request.Params["PSOID"].SafeIntParse());
        }

        // Recibe del parametro el id correspondiente al dropdown
        protected void Page_Load(object sender, EventArgs e)
        {
            _proyectoId = Request.Params["PTOID"].SafeIntParse();
            _famAceroId = Request.Params["FID"].SafeIntParse();
            _tipoJuntaId = Request.Params["TID"].SafeIntParse();
            _procesoId = Request.Params["PSOID"].SafeIntParse();
            _procesoValor = Request.Params["PSO"].SafeStringParse();

            if(!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.adm_Destajos);
                SoltarCombo();
            }
        }

            // Asigna los valores de los ComboBox a sus respectivos lugares
        private void SoltarCombo()
        {
                proyEncabezado.BindInfo(_proyectoId.SafeIntParse());
                ProcesoRellenoCache proRelleno;
                ProcesoRaizCache proRaiz;
                FamAceroCache famAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero().SingleOrDefault(x => x.ID == _famAceroId);
                TipoJuntaCache tipoJunta = CacheCatalogos.Instance.ObtenerTiposJunta().SingleOrDefault(x => x.ID == _tipoJuntaId);

                if(_procesoValor.StartsWith("RE"))
                {
                    proRelleno = CacheCatalogos.Instance.ObtenerProcesosRelleno().SingleOrDefault(x => x.ID == _procesoId);
                    txtProceso.Text = proRelleno.Text;
                }
                else if(_procesoValor.StartsWith("R"))
                {
                    proRaiz = CacheCatalogos.Instance.ObtenerProcesosRaiz().SingleOrDefault(x => x.ID == _procesoId);
                    txtProceso.Text = proRaiz.Text;
                }
                else
                {
                    txtProceso.Text = _procesoValor;
                }

                txtFamAcero.Text = famAcero.Text;
                txtTipoJunta.Text = tipoJunta.Text;
        }

        // Si la extension del archivo es valida, se procesara
        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            Validate();
            if(IsValid)
            {
                try
                {
                    UploadedFile f = rdArchivo.UploadedFiles[0];
                    DestajoBL.Instance.ProcesaDestajo(f.InputStream, SessionFacade.UserId, _famAceroId, _tipoJuntaId, _procesoValor, _procesoId, _proyectoId);
                    string url = string.Format(WebConstants.AdminUrl.TBL_DESTAJOS,
                                               _proyectoId, _famAceroId, _tipoJuntaId, _procesoValor,_procesoId);
                    Response.Redirect(url);
                }
                catch (BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void cusExtensionArchivos_OnServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = false;

            if (rdArchivo.UploadedFiles.Count > 0)
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