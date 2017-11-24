using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.Web.Common;

namespace SAM.Web.Controles.Proyecto
{
    public partial class Configuracion : System.Web.UI.UserControl, IMappable
    {
        private RequiredLabeledTextBox [] segmentos;

        private int? ProyectoID
        {
            get 
            {
                if (ViewState["ProyectoID"] != null)
                {
                    return (int)ViewState["ProyectoID"];
                }
                return null;
            }
            set
            {
                ViewState["ProyectoID"] = value;
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

        protected void Page_Init(object sender, EventArgs e)
        {
            segmentos = new RequiredLabeledTextBox[]{txtSegmento1, txtSegmento2, txtSegmento3, txtSegmento4, txtSegmento5, txtSegmento6, txtSegmento7};
        }

        protected void ddlNumSegmentos_SelectedIndexChanged(object sender, EventArgs e)
        {
            int visibles = ddlNumSegmentos.SelectedValue.SafeIntParse();
            estableceVisibilidadSegmentos(visibles);
        }

        private void estableceVisibilidadSegmentos(int elementosVisibles)
        {
            for (int i = 0; i < segmentos.Length; i++)
            {
                segmentos[i].Enabled = false;
            }

            if (elementosVisibles > 0)
            {
                for (int i = 0; i < elementosVisibles; i++)
                {
                    segmentos[i].Enabled = true;
                    segmentos[i].CssClass = "required";
                }
            }

            for (int i = elementosVisibles; i < segmentos.Length; i++)
            {
                segmentos[i].Text = string.Empty;
                segmentos[i].CssClass = string.Empty;
            }
        }


        #region IMappable Members

        public void Map(object entity)
        {
            Entities.Proyecto proyectoCargado = (Entities.Proyecto)entity;

            ProyectoID = proyectoCargado.ProyectoID;
            VersionRegistro = proyectoCargado.VersionRegistro;

            if (proyectoCargado.ProyectoNomenclaturaSpool == null)
            {
                estableceVisibilidadSegmentos(0);
                ddlNumSegmentos.SelectedIndex = 0;
            }
            else
            {
                ddlNumSegmentos.SelectedValue = proyectoCargado.ProyectoNomenclaturaSpool.CantidadSegmentosSpool.ToString();
                estableceVisibilidadSegmentos(proyectoCargado.ProyectoNomenclaturaSpool.CantidadSegmentosSpool);
                
                txtSegmento1.Text = proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool1;
                txtSegmento2.Text = proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool2;
                txtSegmento3.Text = proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool3;
                txtSegmento4.Text = proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool4;
                txtSegmento5.Text = proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool5;
                txtSegmento6.Text = proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool6;
                txtSegmento7.Text = proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool7;
            }

            txtAngulo.Text = proyectoCargado.ProyectoConfiguracion.AnguloBisel;
            txtTolerancia.Text = proyectoCargado.ProyectoConfiguracion.ToleranciaCortes.ToString();
            txtCostoCuadroArmado.Text = proyectoCargado.ProyectoConfiguracion.CuadroTubero.ToString();
            txtCostoCuadroRaiz.Text = proyectoCargado.ProyectoConfiguracion.CuadroRaiz.ToString();
            txtCostoCuadroRelleno.Text = proyectoCargado.ProyectoConfiguracion.CuadroRelleno.ToString();          
            chkActualizaLocalizacion.Checked = proyectoCargado.ProyectoConfiguracion.ActualizaLocalizacion;
        }

        public void Unmap(object entity)
        {
            Entities.Proyecto proyectoCargado = (Entities.Proyecto)entity;

            proyectoCargado.StartTracking();
            proyectoCargado.ProyectoNomenclaturaSpool.StartTracking();
            proyectoCargado.ProyectoConfiguracion.StartTracking();

            if (ProyectoID != null)
            {
                proyectoCargado.ProyectoID = ProyectoID.Value;
                proyectoCargado.VersionRegistro = VersionRegistro;
            }

            proyectoCargado.ProyectoConfiguracion.AnguloBisel = txtAngulo.Text;
            proyectoCargado.ProyectoConfiguracion.ToleranciaCortes = txtTolerancia.Text.SafeIntParse();
            proyectoCargado.ProyectoConfiguracion.CuadroTubero = txtCostoCuadroArmado.Text.SafeDecimalParse();
            proyectoCargado.ProyectoConfiguracion.CuadroRaiz = txtCostoCuadroRaiz.Text.SafeDecimalParse();
            proyectoCargado.ProyectoConfiguracion.CuadroRelleno = txtCostoCuadroRelleno.Text.SafeDecimalParse();
            proyectoCargado.ProyectoNomenclaturaSpool.CantidadSegmentosSpool = ddlNumSegmentos.SelectedValue.SafeIntParse();

            proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool1 = txtSegmento1.Text;
            proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool2 = txtSegmento2.Text;
            proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool3 = txtSegmento3.Text;
            proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool4 = txtSegmento4.Text;
            proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool5 = txtSegmento5.Text;
            proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool6 = txtSegmento6.Text;
            proyectoCargado.ProyectoNomenclaturaSpool.SegmentoSpool7 = txtSegmento7.Text;

            proyectoCargado.ProyectoNomenclaturaSpool.UsuarioModifica = SessionFacade.UserId;
            proyectoCargado.ProyectoConfiguracion.UsuarioModifica = SessionFacade.UserId;
            proyectoCargado.ProyectoConfiguracion.FechaModificacion = DateTime.Now;
            proyectoCargado.ProyectoNomenclaturaSpool.FechaModificacion = DateTime.Now;
            proyectoCargado.ProyectoConfiguracion.ActualizaLocalizacion = chkActualizaLocalizacion.Checked;
        }

        #endregion
    }
}