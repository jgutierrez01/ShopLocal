using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects.Produccion;
using System.Data;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities.Personalizadas;
using SAM.Web.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpReporteTransferencia : SamPaginaPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OrdenTrabajoSpool odts = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
                List<ReporteTransferencia> reporteTransferencia = TransferenciaSpoolBO.Instance.ObtenerTransferenciasSpool(EntityID.Value);
                cargaDatos(reporteTransferencia, odts);
            }
        }

        private void cargaDatos(List<ReporteTransferencia> rpt, OrdenTrabajoSpool odts)
        {
            lblSpool.Text = odts.Spool.Nombre;
            lblNumControl.Text = odts.NumeroControl;

            generaDataSet(rpt);
        }

        private void generaDataSet(List<ReporteTransferencia> rpt)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("TipoAccionTransferenciaEnum");
            ds.Tables[0].Columns.Add("Accion");
            ds.Tables[0].Columns.Add("Fecha");
            ds.Tables[0].Columns.Add("Etiqueta");
            var i = 0;
            foreach (var row in rpt)
            {
                //Preparacion
                string strTipoAccionTransferenciaEnum = ((int)TipoAccionTransferenciaEnum.Preparacion).ToString() + "," + row.TransferenciaSpoolID.ToString();
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                ds.Tables[0].Rows[i]["TipoAccionTransferenciaEnum"] = strTipoAccionTransferenciaEnum;
                ds.Tables[0].Rows[i]["Accion"] = Cultura == LanguageHelper.INGLES ? "Packing" : "Preparación";
                ds.Tables[0].Rows[i]["Fecha"] = row.FechaPreparacion.SafeDateAsStringParse();

                i++;
                //Transferencia
                strTipoAccionTransferenciaEnum = ((int)TipoAccionTransferenciaEnum.Transferencia).ToString() + "," + row.TransferenciaSpoolID.ToString();
                ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
                ds.Tables[0].Rows[i]["TipoAccionTransferenciaEnum"] = strTipoAccionTransferenciaEnum;
                ds.Tables[0].Rows[i]["Accion"] = Cultura == LanguageHelper.INGLES ? "transfer" : "Transferencia";
                ds.Tables[0].Rows[i]["Fecha"] = row.FechaTransferencia.SafeDateAsStringParse();
                ds.Tables[0].Rows[i]["Etiqueta"] = row.NumeroTransferencia;

                i++;
            }

            repReportes.DataSource = ds.Tables[0];
            repReportes.DataBind();
        }

        protected void repReportes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (string.IsNullOrEmpty(drv["Fecha"].ToString()) && string.IsNullOrEmpty(drv["Etiqueta"].ToString()))
                {
                    ImageButton lnkBorra = e.Item.FindControl("lnkBorra") as ImageButton;
                    lnkBorra.Visible = false;
                }
            }
        }
        protected void repReportes_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Borra")
            {
                try
                {

                    TransferenciaSpoolBO.Instance.BorrarTransferencia(EntityID.Value, e.CommandArgument.SafeStringParse(), SessionFacade.UserId);
                    OrdenTrabajoSpool odts = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
                    List<ReporteTransferencia> reporteTransferencia = TransferenciaSpoolBO.Instance.ObtenerTransferenciasSpool(EntityID.Value);
                    cargaDatos(reporteTransferencia, odts);
                }
                catch (Mimo.Framework.Exceptions.BaseValidationException ex)
                {
                    RenderErrors(ex);
                }
            }
        }

        protected void btnActualiza_OnClick(object sender, EventArgs e)
        {
            JsUtils.RegistraScriptActualizaGridGenerico(this);
        }
    }
}