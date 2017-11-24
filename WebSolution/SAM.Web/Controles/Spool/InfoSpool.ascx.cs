using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities;
using SAM.BusinessObjects.Proyectos;
using System.Globalization;

namespace SAM.Web.Controles.Spool
{
    public partial class InfoSpool : System.Web.UI.UserControl, IMappable
    {
        #region Propiedades Privadas en ViewState
        private int? SpoolID
        {
            get
            {
                if (ViewState["SpoolID"] != null)
                {
                    return (int)ViewState["SpoolID"];
                }

                return null;
            }
            set
            {
                ViewState["SpoolID"] = value;
            }
        }

        private byte[] VersionRegistro
        {
            get
            {
                if (ViewState["VersionRegistro"] != null)
                {
                    return (byte[])ViewState["VersionRegistro"];
                }

                return null;
            }
            set
            {
                ViewState["VersionRegistro"] = value;
            }
        }
        #endregion

        #region IMappable Members
        public void Map(object entity)
        {
            Entities.Spool spool = (Entities.Spool)entity;
            Entities.SpoolHold spoolHold = SpoolHoldBO.Instance.Obtener(spool.SpoolID);
            ProyectoNomenclaturaSpool nomenclatura = ProyectoBO.Instance.ObtenerNomenclaturaSpool(spool.ProyectoID);
            if (spool.RequierePruebaHidrostatica.SafeStringParse() != "")
            {
                cargaCombos(true);
            }
            else
            {
                cargaCombos();
            }

            SpoolID = spool.SpoolID;

            VersionRegistro = spool.VersionRegistro;

            //Llenar controles
            txtSpool.Text = spool.Nombre;
            txtDibujo.Text = spool.Dibujo;
            txtExpecificacion.Text = spool.Especificacion;
            txtCedula.Text = spool.Cedula;
            txtPnd.Text = spool.PorcentajePnd.SafeStringParse();
            ddlTipoAcero1.SelectedValue = spool.FamiliaAcero1ID.ToString();
            ddlTipoAcero2.SelectedValue = spool.FamiliaAcero2ID != null ? spool.FamiliaAcero2ID.SafeStringParse() : "";
            chkRequierePwht.Checked = spool.RequierePwht;
            chkPendienteDocumental.Checked = spool.PendienteDocumental;
            txtPeso.Text = spool.Peso.SafeStringParse();
            txtArea.Text = spool.Area.SafeStringParse();
            txtRevision.Text = spool.Revision;
            txtRevisionCliente.Text = spool.RevisionCliente;
            txtPrioridad.Text = spool.Prioridad.HasValue ? spool.Prioridad.SafeStringParse() : "999";
            txtPdi.Text = String.Format("{0:0.000}", spool.Pdis);
            txtDiametroMayor.Text = String.Format("{0:0.0000}", spool.DiametroMayor);

            txtSegmento1.Text = spool.Segmento1 != null ? spool.Segmento1 : string.Empty;
            txtSegmento2.Text = spool.Segmento2 != null ? spool.Segmento2 : string.Empty;
            txtSegmento3.Text = spool.Segmento3 != null ? spool.Segmento3 : string.Empty;
            txtSegmento4.Text = spool.Segmento4 != null ? spool.Segmento4 : string.Empty;
            txtSegmento5.Text = spool.Segmento5 != null ? spool.Segmento5 : string.Empty;
            txtSegmento6.Text = spool.Segmento6 != null ? spool.Segmento6 : string.Empty;
            txtSegmento7.Text = spool.Segmento7 != null ? spool.Segmento7 : string.Empty;

            if (nomenclatura.CantidadSegmentosSpool > 0)
            {
                txtSegmento1.Label = nomenclatura.SegmentoSpool1;                
                txtSegmento1.Visible = true;
                if (nomenclatura.CantidadSegmentosSpool > 1)
                {
                    txtSegmento2.Label = nomenclatura.SegmentoSpool2;                    
                    txtSegmento2.Visible = true;
                    if (nomenclatura.CantidadSegmentosSpool > 2)
                    {
                        txtSegmento3.Label = nomenclatura.SegmentoSpool3;                        
                        txtSegmento3.Visible = true;
                        if (nomenclatura.CantidadSegmentosSpool > 3)
                        {
                            txtSegmento4.Label = nomenclatura.SegmentoSpool4;                            
                            txtSegmento4.Visible = true;
                            if (nomenclatura.CantidadSegmentosSpool > 4)
                            {
                                txtSegmento5.Label = nomenclatura.SegmentoSpool5;                                
                                txtSegmento5.Visible = true;
                                if (nomenclatura.CantidadSegmentosSpool > 5)
                                {
                                    txtSegmento6.Label = nomenclatura.SegmentoSpool6;                                    
                                    txtSegmento6.Visible = true;
                                    if (nomenclatura.CantidadSegmentosSpool > 6)
                                    {
                                        txtSegmento7.Label = nomenclatura.SegmentoSpool7;                                        
                                        txtSegmento7.Visible = true;
                                    }
                                }

                            }

                        }

                    }
                }
            }

            chkLiberadoParaCruce.Checked = spool.AprobadoParaCruce;
            
            bool requierePruebaHidrostatica = false;
            Boolean.TryParse(spool.RequierePruebaHidrostatica.SafeStringParse(), out requierePruebaHidrostatica);
            //chkRequierePruebaHidrostatica.Checked = requierePruebaHidrostatica;
            if (requierePruebaHidrostatica)
            {
                ddRequierePruebaHidrostatica.SelectedValue = "true";
            }
            else if (spool.RequierePruebaHidrostatica.SafeStringParse() == "")
            {
                ddRequierePruebaHidrostatica.SelectedValue = "";
            }
            else
            {
                ddRequierePruebaHidrostatica.SelectedValue = "false";
            }

            chkHoldCalidad.Checked = spoolHold != null ? spoolHold.TieneHoldCalidad : false;
            chkHoldIng.Checked = spoolHold != null ? spoolHold.TieneHoldIngenieria : false;
            chkConfinado.Checked = spoolHold != null ? spoolHold.Confinado : false;


        }

        public void Unmap(object entity)
        {
            Entities.Spool spool = (Entities.Spool)entity;

            if (SpoolID != null)
            {
                spool.SpoolID = SpoolID.Value;
                spool.VersionRegistro = VersionRegistro;
            }

            //Establecer los datos en las propiedades
            spool.Nombre = txtSpool.Text;
            spool.Dibujo = txtDibujo.Text;
            spool.Especificacion = txtExpecificacion.Text;
            spool.Cedula = txtCedula.Text;
            spool.PorcentajePnd = txtPnd.Text.SafeIntParse(0);
            spool.FamiliaAcero1ID = ddlTipoAcero1.SelectedValue.SafeIntParse();
            spool.FamiliaAcero2ID = ddlTipoAcero2.SelectedValue.SafeIntParse() > 0 ? ddlTipoAcero2.SelectedValue.SafeIntParse() : (int?)null;
            spool.RequierePwht = chkRequierePwht.Checked;
            spool.Peso = txtPeso.Text.SafeDecimalParse();
            spool.Area = txtArea.Text.SafeDecimalParse();
            spool.Revision = txtRevision.Text;
            spool.RevisionCliente = txtRevisionCliente.Text;
            spool.Prioridad = txtPrioridad.Text.SafeIntParse();
            spool.Pdis = txtPdi.Text.SafeDecimalParse();
            spool.PendienteDocumental = chkPendienteDocumental.Checked;
            spool.Segmento1 = txtSegmento1.Text;
            spool.Segmento2 = txtSegmento2.Text;
            spool.Segmento3 = txtSegmento3.Text;
            spool.Segmento4 = txtSegmento4.Text;
            spool.Segmento5 = txtSegmento5.Text;
            spool.Segmento6 = txtSegmento6.Text;
            spool.Segmento7 = txtSegmento7.Text;
            spool.AprobadoParaCruce = chkLiberadoParaCruce.Checked;
            spool.DiametroMayor = txtDiametroMayor.Text.SafeDecimalParse();
            if (ddRequierePruebaHidrostatica.SelectedValue != "")
            {
                spool.RequierePruebaHidrostatica = ddRequierePruebaHidrostatica.SelectedValue;
            }
            else
            {
                spool.RequierePruebaHidrostatica = null;
            }
        }

        #endregion

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        cargaCombos();
        //    }
        //}

        private void cargaCombos(bool hidrostatica = false)
        {
            ddlTipoAcero1.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasAcero());
            ddlTipoAcero2.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerFamiliasAcero());
            if (!hidrostatica)
            {
                ddRequierePruebaHidrostatica.Items.Add(new ListItem("", ""));
            }
            ddRequierePruebaHidrostatica.Items.Add(new ListItem(CultureInfo.CurrentCulture.Name == "en-US" ? "Yes" : "Si", "1"));
            ddRequierePruebaHidrostatica.Items.Add(new ListItem("No", "0"));
        }
    }
}