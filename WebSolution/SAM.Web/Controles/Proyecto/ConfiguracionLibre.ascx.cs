using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Exceptions;
using SAM.Entities;
using SAM.Web.Common;

namespace SAM.Web.Controles.Proyecto
{
    public partial class ConfiguracionLibre : System.Web.UI.UserControl, IMappable
    {
        private RequiredLabeledTextBox[] camposRecepcion;
        private RequiredLabeledTextBox[] camposNumeroUnico;

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
            camposRecepcion = new RequiredLabeledTextBox[] 
                { txtCampoRecepcion1, txtCampoRecepcion2, txtCampoRecepcion3, txtCampoRecepcion4, txtCampoRecepcion5 };
            camposNumeroUnico = new RequiredLabeledTextBox[]
                { txtCampoNumeroUnico1, txtCampoNumeroUnico2, txtCampoNumeroUnico3, txtCampoNumeroUnico4, txtCampoNumeroUnico5 };
        }

        protected void ddlNumCamposRecepcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            int visibles = ddlNumCamposRecepcion.SelectedValue.SafeIntParse();
            estableceVisibilidadCampos(visibles, camposRecepcion);
        }

        protected void ddlNumCamposNumeroUnico_SelectedIndexChanged(object sender, EventArgs e)
        {
            int visibles = ddlNumCamposNumeroUnico.SelectedValue.SafeIntParse();
            estableceVisibilidadCampos(visibles, camposNumeroUnico);
        }

        private void estableceVisibilidadCampos(int elementosVisibles, RequiredLabeledTextBox[] campos)
        {
            for (int i = 0; i < campos.Length; i++)
            {
                campos[i].Enabled = false;
            }

            if (elementosVisibles > 0)
            {
                for (int i = 0; i < elementosVisibles; i++)
                {
                    campos[i].Enabled = true;
                    campos[i].CssClass = "required";
                }
            }

            for (int i = elementosVisibles; i < campos.Length; i++)
            {
                campos[i].Text = string.Empty;
                campos[i].CssClass = string.Empty;
            }
        }

        #region IMappable Members

        public void Map(object entity)
        {
            Entities.Proyecto proyectoCargado = (Entities.Proyecto)entity;

            ProyectoID = proyectoCargado.ProyectoID;
            VersionRegistro = proyectoCargado.VersionRegistro;

            proyectoCargado.StartTracking();
            proyectoCargado.ProyectoCamposRecepcion.StartTracking();

            if (proyectoCargado.ProyectoCamposRecepcion == null)
            {
                //establecemos visibilidad para campos recepción y campos número único
                estableceVisibilidadCampos(0, camposRecepcion);
                estableceVisibilidadCampos(0, camposNumeroUnico);
            }
            else
            {
                // Precargamos información de los campos de recepción
                ddlNumCamposRecepcion.SelectedValue = proyectoCargado.ProyectoCamposRecepcion.CantidadCamposRecepcion.ToString();
                estableceVisibilidadCampos(proyectoCargado.ProyectoCamposRecepcion.CantidadCamposRecepcion, camposRecepcion);

                txtCampoRecepcion1.Text = proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion1;
                txtCampoRecepcion2.Text = proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion2;
                txtCampoRecepcion3.Text = proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion3;
                txtCampoRecepcion4.Text = proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion4;
                txtCampoRecepcion5.Text = proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion5;

                // Precargamos información de los campos de número único
                ddlNumCamposNumeroUnico.SelectedValue = proyectoCargado.ProyectoCamposRecepcion.CantidadCamposNumeroUnico.ToString();
                estableceVisibilidadCampos(proyectoCargado.ProyectoCamposRecepcion.CantidadCamposNumeroUnico, camposNumeroUnico);

                txtCampoNumeroUnico1.Text = proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico1;
                txtCampoNumeroUnico2.Text = proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico2;
                txtCampoNumeroUnico3.Text = proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico3;
                txtCampoNumeroUnico4.Text = proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico4;
                txtCampoNumeroUnico5.Text = proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico5;
            }
        }
        public void Unmap(object entity)
        {
            Entities.Proyecto proyectoCargado = (Entities.Proyecto)entity;

            proyectoCargado.StartTracking();
            proyectoCargado.ProyectoCamposRecepcion.StartTracking();

            if (ProyectoID != null)
            {
                proyectoCargado.ProyectoID = ProyectoID.Value;
                proyectoCargado.VersionRegistro = VersionRegistro;

                proyectoCargado.ProyectoCamposRecepcion.CantidadCamposRecepcion = ddlNumCamposRecepcion.SelectedValue.SafeIntParse();
                proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion1 = txtCampoRecepcion1.Text;
                proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion2 = txtCampoRecepcion2.Text;
                proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion3 = txtCampoRecepcion3.Text;
                proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion4 = txtCampoRecepcion4.Text;
                proyectoCargado.ProyectoCamposRecepcion.CampoRecepcion5 = txtCampoRecepcion5.Text;

                proyectoCargado.ProyectoCamposRecepcion.CantidadCamposNumeroUnico = ddlNumCamposNumeroUnico.SelectedValue.SafeIntParse();
                proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico1 = txtCampoNumeroUnico1.Text;
                proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico2 = txtCampoNumeroUnico2.Text;
                proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico3 = txtCampoNumeroUnico3.Text;
                proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico4 = txtCampoNumeroUnico4.Text;
                proyectoCargado.ProyectoCamposRecepcion.CampoNumeroUnico5 = txtCampoNumeroUnico5.Text;

                proyectoCargado.ProyectoCamposRecepcion.UsuarioModifica = SessionFacade.UserId;
                proyectoCargado.ProyectoCamposRecepcion.FechaModificacion = DateTime.Now;
            }
        }

        #endregion
    }
}