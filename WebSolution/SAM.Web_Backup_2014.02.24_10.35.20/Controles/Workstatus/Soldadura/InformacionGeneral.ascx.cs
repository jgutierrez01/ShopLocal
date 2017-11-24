using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities.Grid;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Common;
using SAM.BusinessLogic.Workstatus;
using Mimo.Framework.WebControls;

namespace SAM.Web.Controles.Workstatus.Soldadura
{
    public partial class InformacionGeneral : System.Web.UI.UserControl
    {

        public MappableDropDown ddlWps;
        public MappableDropDown ddlWpsRelleno;
        public event EventHandler WpsSeleccionado;
        public event EventHandler WpsRellenoSeleccionado;
        public event EventHandler WpsDiferenteCambio;

        public bool WpsIguales
        {
            get
            {
                return !chkWpsDiferentes.Checked;
            }
        }

        public string material1
        {
            get
            {
                return txtMaterial1.Text;
            }
        }

        public string material2
        {
            get 
            {
                return txtMaterial2.Text;
            }
        }

        public string wpsRellenoItem
        {
            get
            {
                return ddlWpsRelleno.SelectedValue.SafeStringParse();
            }
            set
            {
                ddlWpsRelleno.SelectedValue = value;
            }
        }

        public string wpsItem
        {
            get 
            {
                return ddlWps.SelectedValue.SafeStringParse();
            }
            set
            {
                ddlWps.SelectedValue = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public OrdenTrabajoJunta CargaInformacion(int juntaSpooID, bool readOnly)
        {
            OrdenTrabajoJunta odtJ = SoldaduraBO.Instance.ObtenerInformacionParaSoldadura(juntaSpooID);

            NumControl.Text = odtJ.OrdenTrabajoSpool.NumeroControl;

            List<TipoJuntaCache> tj = CacheCatalogos.Instance.ObtenerTiposJunta();
            Junta.Text = odtJ.JuntaSpool.Etiqueta;
            Localizacion.Text = String.Format("{0} - {1}", odtJ.JuntaSpool.EtiquetaMaterial1, odtJ.JuntaSpool.EtiquetaMaterial2);
            Tipo.Text = tj.Single(x => x.ID == odtJ.JuntaSpool.TipoJuntaID).Nombre;
            Cedula.Text = odtJ.JuntaSpool.Cedula;
            NombreSpool.Text = odtJ.JuntaSpool.Spool.Nombre;

            List<FamAceroCache> tm = CacheCatalogos.Instance.ObtenerFamiliasAcero();
            txtMaterial1.Text = tm.Single(x => x.ID == odtJ.JuntaSpool.FamiliaAceroMaterial1ID).Nombre;
            txtMaterial2.Text = odtJ.JuntaSpool.FamiliaAceroMaterial2ID != null ? tm.Single(x => x.ID == odtJ.JuntaSpool.FamiliaAceroMaterial2ID).Nombre : txtMaterial1.Text;
            hdnProyectoID.Value = odtJ.JuntaSpool.Spool.ProyectoID.ToString();
            ddlTaller.BindToEntiesWithEmptyRow(UserScope.TalleresPorProyecto(hdnProyectoID.Value.SafeIntParse()));

            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            {
                mdpFechaSoldadura.SelectedDate = DateTime.Now.AddDays(-2);
                mdpFechaReporte.SelectedDate = DateTime.Now.AddDays(-2);
            }
            else
            {
                mdpFechaSoldadura.SelectedDate = DateTime.Now.AddDays(-1);
                mdpFechaReporte.SelectedDate = DateTime.Now.AddDays(-1);
            }

            return odtJ;
        }

        /// <summary>
        /// Carga la informacion de los controles en modo solo lectura
        /// </summary>
        public void CargaInformacionReadOnly(JuntaWorkstatus jws, JuntaSoldadura js)
        {
            chkWpsDiferentes.Enabled = false;
            pnlLeyendas.Visible = true;
            mdpFechaReporte.SelectedDate = js.FechaReporte;
            mdpFechaSoldadura.SelectedDate = js.FechaSoldadura;
            ddlTaller.SelectedValue = js.TallerID.ToString();
            ddlWps.Items.Add(js.Wps.Nombre);
            ddlWps.SelectedIndex = 1;
            string wpsRelleno = js.Wps1 == null ? js.Wps.Nombre : js.Wps1.Nombre;
            ddlWpsRelleno.Items.Add(wpsRelleno);
            ddlWps.SelectedIndex = 1;
            //chkPWHT.Checked = js.Wps.RequierePwht;
            //chkPreheat.Checked = js.Wps.RequierePreheat;

            if (js.Wps.RequierePreheat)
            {
                lblNoPreheat.Visible = false;
                lblSiPreheat.Visible = true;
            }
            else
            {
                lblNoPreheat.Visible = true;
                lblSiPreheat.Visible = false;
            }

            if (js.Wps.RequierePwht)
            {
                lblNoPwht.Visible = false;
                lblSiPwht.Visible = true;
            }
            else
            {
                lblNoPwht.Visible = true;
                lblSiPwht.Visible = false;
            }

            txtObservaciones.Text = js.Observaciones;

            mdpFechaSoldadura.Enabled = false;
            mdpFechaReporte.Enabled = false;
            ddlTaller.Enabled = false;
            ddlWps.Enabled = false;
            txtObservaciones.ReadOnly = true;
        }


        /// <summary>
        /// Carga los WPS que coincidan con los procesos y materiales seleccionados.
        /// </summary>
        public void CargaWPS(int juntaSpoolID, int procesoRaizID, int procesoRellenoID, decimal? espesorJunta)
        {
            if (!chkWpsDiferentes.Checked)
            {
                if (procesoRaizID > 0 && procesoRellenoID > 0)
                {
                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                    juntaSpoolID,
                                                    hdnProyectoID.Value.SafeIntParse(),
                                                    procesoRaizID,
                                                    procesoRellenoID,
                                                    txtMaterial1.Text,
                                                    txtMaterial2.Text,
                                                    espesorJunta,
                                                    chkWpsDiferentes.Checked,
                                                    true);

                    ddlWps.Enabled = true;
                    ddlWps.BindToEntiesWithEmptyRow(origen);
                    ddlWpsRelleno.Enabled = true;
                    ddlWpsRelleno.BindToEntiesWithEmptyRow(origen);
                }
            }
            else
            {
                if (procesoRaizID > 0)
                {
                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                    juntaSpoolID,
                                                    hdnProyectoID.Value.SafeIntParse(),
                                                    procesoRaizID,
                                                    procesoRellenoID,
                                                    txtMaterial1.Text,
                                                    txtMaterial2.Text,
                                                    espesorJunta,
                                                    chkWpsDiferentes.Checked,
                                                    true);

                    ddlWps.Enabled = true;
                    ddlWps.BindToEntiesWithEmptyRow(origen);
                }
            }
        }

        /// <summary>
        /// Carga los WPS que coincidan con los procesos y materiales seleccionados.
        /// </summary>
        public void CargaWPSRelleno(int juntaSpoolID, int procesoRaizID, int procesoRellenoID, decimal? espesorJunta)
        {
            if (!chkWpsDiferentes.Checked)
            {
                if (procesoRaizID > 0 && procesoRellenoID > 0)
                {
                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                    juntaSpoolID,
                                                    hdnProyectoID.Value.SafeIntParse(),
                                                    procesoRaizID,
                                                    procesoRellenoID,
                                                    txtMaterial1.Text,
                                                    txtMaterial2.Text,
                                                    espesorJunta,
                                                    chkWpsDiferentes.Checked,
                                                    true);

                    ddlWpsRelleno.Enabled = true;
                    ddlWpsRelleno.BindToEntiesWithEmptyRow(origen);
                    ddlWps.Enabled = true;
                    ddlWps.BindToEntiesWithEmptyRow(origen);
                }
            }
            else
            {
                if (procesoRellenoID > 0)
                {
                    IEnumerable<WpsCache> origen = SoldaduraBL.Instance.ObtenerWps(
                                                    juntaSpoolID,
                                                    hdnProyectoID.Value.SafeIntParse(),
                                                    procesoRaizID,
                                                    procesoRellenoID,
                                                    txtMaterial1.Text,
                                                    txtMaterial2.Text,
                                                    espesorJunta,
                                                    chkWpsDiferentes.Checked,
                                                    true);

                    ddlWpsRelleno.Enabled = true;
                    ddlWpsRelleno.BindToEntiesWithEmptyRow(origen);
                }
            }
        }

       
        

        public void UnBindInformacion(JuntaSoldadura js)
        {
            js.FechaSoldadura = mdpFechaSoldadura.SelectedDate.Value;
            js.FechaReporte = mdpFechaReporte.SelectedDate.Value;
            js.TallerID = ddlTaller.SelectedValue.SafeIntParse();
            js.WpsID = ddlWps.SelectedValue.SafeIntParse();
            js.Observaciones = txtObservaciones.Text;
            js.UsuarioModifica = SessionFacade.UserId;
            js.FechaModificacion = DateTime.Now;
        }


        //Carga la informacion del wps
        protected void ddlWpsRelleno_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WpsRellenoSeleccionado != null)
            {
                WpsRellenoSeleccionado(sender, e);
            }
        }

        //Carga la informacion del wps
        protected void ddlWps_SelectedIndexChanged(object sender, EventArgs e)
        {
            WpsCache wps = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == ddlWps.SelectedValue.SafeIntParse()).SingleOrDefault();

            if (wps != null)
            {
                //chkPreheat.Checked = wps.RequierePreheat;
                //chkPWHT.Checked = wps.RequierePwht;

                if (wps.RequierePreheat)
                {
                    lblNoPreheat.Visible = false;
                    lblSiPreheat.Visible = true;
                }
                else
                {
                    lblNoPreheat.Visible = true;
                    lblSiPreheat.Visible = false;
                }

                if (wps.RequierePwht)
                {
                    lblNoPwht.Visible = false;
                    lblSiPwht.Visible = true;
                }
                else
                {
                    lblNoPwht.Visible = true;
                    lblSiPwht.Visible = false;
                }
            }

            if (WpsSeleccionado != null)
            {
                WpsSeleccionado(sender, e);
            }
        }

        protected void chkWpsDiferentes_OnCheckedChanged(object sender, EventArgs e)
        {            
            if (WpsDiferenteCambio != null)
            {
                WpsDiferenteCambio(sender, e);
            }

            ddlWps.Items.Clear();
            ddlWpsRelleno.Items.Clear();

        }
    }
}