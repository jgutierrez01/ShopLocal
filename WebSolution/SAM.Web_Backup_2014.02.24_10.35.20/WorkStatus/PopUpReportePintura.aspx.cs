using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.BusinessObjects.Produccion;
using System.Data;
using Mimo.Framework.Common;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpReportePintura : SamPaginaPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WorkstatusSpool wksSpool = WorkstatusSpoolBO.Instance.ObtenerConPintura(EntityID.Value);
                cargaDatos(wksSpool);
            }
        }

        private void cargaDatos(WorkstatusSpool wks)
        {
            lblSpool.Text = wks.OrdenTrabajoSpool.Spool.Nombre;
            lblNumControl.Text = wks.OrdenTrabajoSpool.NumeroControl;
            chkLiberado.Checked = wks.LiberadoPintura;
            lblSistema.Text = wks.OrdenTrabajoSpool.Spool.SistemaPintura;
            lblColor.Text = wks.OrdenTrabajoSpool.Spool.ColorPintura;
            lblCodigo.Text = wks.OrdenTrabajoSpool.Spool.CodigoPintura;

            generaDataSet(wks.PinturaSpool.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).FirstOrDefault());
        }

        private void generaDataSet(PinturaSpool pintura)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("TipoReporteEnum");
            ds.Tables[0].Columns.Add("TipoReporte");
            ds.Tables[0].Columns.Add("FechaReporte");
            ds.Tables[0].Columns.Add("NumeroReporte");

            //SandBlast
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[0]["TipoReporteEnum"] = (int)TipoPinturaEnum.SandBlast;
            ds.Tables[0].Rows[0]["TipoReporte"] = "Sand-Blast";
            if (pintura != null)
            {
                ds.Tables[0].Rows[0]["FechaReporte"] = pintura.FechaSandblast;
                ds.Tables[0].Rows[0]["NumeroReporte"] = pintura.ReporteSandblast;
            }

            //Primario
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[1]["TipoReporteEnum"] = (int)TipoPinturaEnum.Primario;
            ds.Tables[0].Rows[1]["TipoReporte"] = Cultura == LanguageHelper.INGLES ? "Primary" : "Primario";
            if (pintura != null)
            {
                ds.Tables[0].Rows[1]["FechaReporte"] = pintura.FechaPrimarios;
                ds.Tables[0].Rows[1]["NumeroReporte"] = pintura.ReportePrimarios;
            }

            //Intermedios
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[2]["TipoReporteEnum"] = (int)TipoPinturaEnum.Intermedio;
            ds.Tables[0].Rows[2]["TipoReporte"] = Cultura == LanguageHelper.INGLES ? "Intermediate" : "Intermedios";
            if (pintura != null)
            {
                ds.Tables[0].Rows[2]["FechaReporte"] = pintura.FechaIntermedios;
                ds.Tables[0].Rows[2]["NumeroReporte"] = pintura.ReporteIntermedios;
            }

            //Acabado Visual
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[3]["TipoReporteEnum"] = (int)TipoPinturaEnum.AcabadoVisual;
            ds.Tables[0].Rows[3]["TipoReporte"] = Cultura == LanguageHelper.INGLES ? "Final Coat" : "Acabado Visual";
            if (pintura != null)
            {
                ds.Tables[0].Rows[3]["FechaReporte"] = pintura.FechaAcabadoVisual;
                ds.Tables[0].Rows[3]["NumeroReporte"] = pintura.ReporteAcabadoVisual;
            }

            //Adhesion
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[4]["TipoReporteEnum"] = (int)TipoPinturaEnum.Adherencia;
            ds.Tables[0].Rows[4]["TipoReporte"] = Cultura == LanguageHelper.INGLES ? "Adhesion" : "Adherencia";
            if (pintura != null)
            {
                ds.Tables[0].Rows[4]["FechaReporte"] = pintura.FechaAdherencia;
                ds.Tables[0].Rows[4]["NumeroReporte"] = pintura.ReporteAdherencia;
            }

            //Pull Off
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[5]["TipoReporteEnum"] = (int)TipoPinturaEnum.PullOff;
            ds.Tables[0].Rows[5]["TipoReporte"] = "Pull Off";
            if (pintura != null)
            {
                ds.Tables[0].Rows[5]["FechaReporte"] = pintura.FechaPullOff;
                ds.Tables[0].Rows[5]["NumeroReporte"] = pintura.ReportePullOff;
            }

            repReportes.DataSource = ds.Tables[0];
            repReportes.DataBind();
        }

        protected void repReportes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (string.IsNullOrEmpty(drv["FechaReporte"].ToString()) && string.IsNullOrEmpty(drv["NumeroReporte"].ToString()))
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
                PinturaBO.Instance.EliminaPinturaSpool(EntityID.Value, SessionFacade.UserId, e.CommandArgument.SafeIntParse());

                WorkstatusSpool wksSpool = WorkstatusSpoolBO.Instance.ObtenerConPintura(EntityID.Value);
                cargaDatos(wksSpool);
            }

        }

        protected void btnBorraSistema_OnClick(object sender, EventArgs e)
        {
            PinturaBO.Instance.BorraSistema(EntityID.Value, SessionFacade.UserId);
            lblSistema.Text = string.Empty;
            lblColor.Text = string.Empty;
            lblCodigo.Text = string.Empty;
            JsUtils.RegistraScriptActualizaGridGenerico(this);
        }

        protected void btnBorraLiberacion_OnClick(object sender, EventArgs e)
        {
            PinturaBO.Instance.BorraLiberacion(EntityID.Value, SessionFacade.UserId);
            chkLiberado.Checked = false;
            JsUtils.RegistraScriptActualizaGridGenerico(this);
        }

        protected void btnActualiza_OnClick(object sender, EventArgs e)
        {
            JsUtils.RegistraScriptActualizaGridGenerico(this);
        }

    }
}