using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Workstatus;
using Mimo.Framework.Extensions;
using Resources;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpImprimirEtiqueta : SamPaginaPopup
    {
        private bool _envioPDF = false;

        private string IDs
        {
            get
            {
                if (ViewState["IDs"] == null)
                {
                    ViewState["IDs"] = string.Empty;
                }

                return ViewState["IDs"].ToString();
            }
            set
            {
                ViewState["IDs"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IDs = Request.QueryString["IDs"];
        }

        protected void btnImprimir_OnClick(object sender, EventArgs e)
        {
            try
            {

                if (chkSeleccionadas.Checked)
                {
                    byte[] reporte = UtileriasReportes.ObtenEtiquetaEmbarque(IDs, string.Empty, string.Empty, string.Empty, EntityID.Value);
                    if (reporte == null)
                    {
                        throw (new BaseValidationException(MensajesAplicacion.Reporte_NoEncontrado));
                    }
                    UtileriasReportes.EnviaReporteComoPdf(this, reporte, "EtiquetasEmbarque.pdf");

                    _envioPDF = true;
                }
                else
                {
                    byte[] reporte = UtileriasReportes.ObtenEtiquetaEmbarque(IDs, txtNumeroEtiqueta.Text, txtNumeroControl.Text, txtOrdenTrabajo.Text, EntityID.Value);
                    if (reporte == null)
                    {
                        throw (new BaseValidationException(MensajesAplicacion.Reporte_NoEncontrado));
                    }
                    UtileriasReportes.EnviaReporteComoPdf(this, reporte, "EtiquetasEmbarque.pdf");

                    _envioPDF = true;
                }

                //if (chkSeleccionadas.Checked)
                //{
                //    int[] intIDs = IDs.Split(',').Select(n => n.SafeIntParse()).ToArray();
                //    int[] wksIDs = EmbarqueBO.Instance.ObtenIDsParaImpresion(intIDs, txtNumeroEtiqueta.Text, txtNumeroControl.Text, txtOrdenTrabajo.Text, EntityID.Value);
                //    ConstructorEtiquetaPDF.CreatePDFEmbarque(wksIDs);
                //    _envioPDF = true;
                //}
                //else
                //{
                //    int[] wksIDs = EmbarqueBO.Instance.ObtenIDsParaImpresion(null, txtNumeroEtiqueta.Text, txtNumeroControl.Text, txtOrdenTrabajo.Text, EntityID.Value);
                //    ConstructorEtiquetaPDF.CreatePDFEmbarque(wksIDs);

                //    _envioPDF = true;
                //}

                
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            //No enviar el HTML
            if (!_envioPDF)
            {
                base.OnPreRender(e);
            }
        }

        private string creaString(int[] ids)
        {
            string idsStr = string.Empty;

            foreach (int id in ids)
            {
                idsStr += id + ",";
            }

            return idsStr.Remove(idsStr.Length - 1);
        }
    }
}