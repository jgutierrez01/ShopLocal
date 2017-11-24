using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Materiales;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;
using SAM.Web.Classes;

namespace SAM.Web.Materiales
{
    public partial class PopUpOdtReqMaterial : SamPaginaPrincipal
    {
        private int[] _materialSpoolID
        {
            get
            {
                if (ViewState["_materialSpoolID"] != null)
                    return (int[])ViewState["_materialSpoolID"];
                else
                    return null;
            }
            set
            {
                ViewState["_materialSpoolID"] = value;
            }
        }

        private int _numeroUnico
        {
            get
            {
                if (ViewState["_numeroUnico"] != null)
                    return (int)ViewState["_numeroUnico"];
                else
                    return -1;
            }
            set
            {
                ViewState["_numeroUnico"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _numeroUnico = Request.QueryString["NU"].SafeIntParse();
            _materialSpoolID = Request.QueryString["MS"].Split(',').Select(n => n.SafeIntParse()).ToArray();            

            List<GrdOdtReqMateial> Datasource = NumeroUnicoBO.Instance.llenaGridOdtReqMaterial(_materialSpoolID);
            grdSpools.DataSource = Datasource;            

        }

        protected void btnCongelar_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                GridDataItem[] items = grdSpools.MasterTableView.GetSelectedItems();

                int[] MaterialSpool = items.Select(x => x.GetDataKeyValue("MaterialSpoolID").SafeIntParse()).ToArray();
                int[] OrdenTrabajoSpool = items.Select(x => x.GetDataKeyValue("OrdenTrabajoSpool").SafeIntParse()).ToArray();
                int cantidad = items.Select(x => x.GetDataKeyValue("Cantidad").SafeIntParse()).Sum();
                try
                {
                    NumeroUnicoBO.Instance.AgregaCongeladoOdt(_numeroUnico, MaterialSpool, OrdenTrabajoSpool, cantidad, SessionFacade.UserId, DateTime.Now);
                    pnlAcciones.Visible = true;
                    phSpools.Visible = false;
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
    }
}