using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;

namespace SAM.Web.Catalogos
{
    public partial class ImportConfigProyecto : SamPaginaPrincipal
    {
        private int _proyectoId;

        protected void Page_Load(object sender, EventArgs e)
        {
            _proyectoId = Request.Params["ID"].SafeIntParse();

            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.cat_ImportConfigProyecto);
                cargaInformacion();
            }
        }

        private void cargaInformacion()
        {
            proyEncabezado.BindInfo(_proyectoId);
            ctrlTablaDestajos.ProyectoID = _proyectoId;
            ctrlICEquivalentes.ProyectoID = _proyectoId;
            ctrlWPS.ProyectoID = _proyectoId;
        }
    }
}