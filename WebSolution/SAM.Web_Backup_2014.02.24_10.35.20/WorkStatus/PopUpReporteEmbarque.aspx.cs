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

namespace SAM.Web.WorkStatus
{
    public partial class PopUpReporteEmbarque : SamPaginaPopup
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                OrdenTrabajoSpool odts = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
                WorkstatusSpool wksSpool = WorkstatusSpoolBO.Instance.ObtenerConEmbarque(EntityID.Value);
                cargaDatos(wksSpool, odts);
            }
        }

        private void cargaDatos(WorkstatusSpool wks, OrdenTrabajoSpool odts)
        {
            lblSpool.Text = odts.Spool.Nombre;
            lblNumControl.Text = odts.NumeroControl;

            generaDataSet(wks, odts);
        }

        private void generaDataSet(WorkstatusSpool wks, OrdenTrabajoSpool odts)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].Columns.Add("TipoAccionEmbarqueEnum");
            ds.Tables[0].Columns.Add("Accion");
            ds.Tables[0].Columns.Add("Fecha");
            ds.Tables[0].Columns.Add("Etiqueta");

            //Etiquetado
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[0]["TipoAccionEmbarqueEnum"] = (int)TipoAccionEmbarqueEnum.Etiquetado;
            ds.Tables[0].Rows[0]["Accion"] = Cultura == LanguageHelper.INGLES ? "Labeling" : "Etiquetado";
            if (!string.IsNullOrEmpty(odts.Spool.NumeroEtiqueta))
            {
                ds.Tables[0].Rows[0]["Fecha"] = odts.Spool.FechaEtiqueta;
                ds.Tables[0].Rows[0]["Etiqueta"] = odts.Spool.NumeroEtiqueta;
            }

            //Preparacion
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[1]["TipoAccionEmbarqueEnum"] = (int)TipoAccionEmbarqueEnum.Preparacion;
            ds.Tables[0].Rows[1]["Accion"] = Cultura == LanguageHelper.INGLES ? "Packing" : "Preparación";
            if (wks != null && wks.Preparado)
            {
                ds.Tables[0].Rows[1]["Fecha"] = wks.FechaPreparacion;
            }

            //Embarque
            ds.Tables[0].Rows.Add(ds.Tables[0].NewRow());
            ds.Tables[0].Rows[2]["TipoAccionEmbarqueEnum"] = (int)TipoAccionEmbarqueEnum.Embarque;
            ds.Tables[0].Rows[2]["Accion"] = Cultura == LanguageHelper.INGLES ? "Shipping" : "Embarque";
            if (wks != null &&  wks.Embarcado)
            {
                ds.Tables[0].Rows[2]["Fecha"] = wks.EmbarqueSpool.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).FirstOrDefault().Embarque.FechaEmbarque;
                ds.Tables[0].Rows[2]["Etiqueta"] = wks.EmbarqueSpool.Where(x => x.WorkstatusSpoolID == wks.WorkstatusSpoolID).FirstOrDefault().Embarque.NumeroEmbarque;
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
                    EmbarqueBO.Instance.EliminaEmbarqueSpool(EntityID.Value, SessionFacade.UserId, e.CommandArgument.SafeIntParse());
                    OrdenTrabajoSpool odts = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
                    WorkstatusSpool wksSpool = WorkstatusSpoolBO.Instance.ObtenerConEmbarque(EntityID.Value);
                    cargaDatos(wksSpool, odts);
                }
                catch (ExcepcionEmbarque ex)
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