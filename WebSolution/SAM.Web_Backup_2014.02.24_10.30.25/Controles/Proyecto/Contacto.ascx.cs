using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Proyectos;
using SAM.Web.Classes;
using Mimo.Framework.WebControls;
using SAM.Entities;

namespace SAM.Web.Controles.Proyecto
{
    public partial class Contacto : System.Web.UI.UserControl, IMappable
    {
        private int ? ContactoID
        {
            get
            {
                if (ViewState["ContactoID"] != null)
                {
                    return (int)ViewState["ContactoID"];
                }

                return null;
            }
            set
            {
                ViewState["ContactoID"] = value;
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

        #region IMappable Members

        public void Map(object entity)
        {
            Entities.Contacto contacto = (Entities.Contacto)entity;

            ContactoID = contacto.ContactoID;
            VersionRegistro = contacto.VersionRegistro;

            txtNombre.Text = contacto.Nombre;
            txtApellidoPat.Text = contacto.ApPaterno;
            txtApellidoMat.Text = contacto.ApMaterno;
            txtTelefonoParticular.Text = contacto.TelefonoParticular;
            txtTelefonoOficina.Text = contacto.TelefonoOficina;
            txtCelular.Text = contacto.TelefonoCelular;
            txtCorreo.Text = contacto.CorreoElectronico;

        }

        public void Unmap(object entity)
        {
            Entities.Contacto contacto = (Entities.Contacto)entity;

            contacto.StartTracking();

            if (ContactoID != null)
            {
                contacto.ContactoID = ContactoID.Value;
                contacto.VersionRegistro = VersionRegistro;
            }

            contacto.Nombre = txtNombre.Text;
            contacto.ApPaterno = txtApellidoPat.Text;
            contacto.ApMaterno = txtApellidoMat.Text;
            contacto.CorreoElectronico = txtCorreo.Text;
            contacto.TelefonoParticular = txtTelefonoParticular.Text;
            contacto.TelefonoOficina = txtTelefonoOficina.Text;
            contacto.TelefonoCelular = txtCelular.Text;
            contacto.UsuarioModifica = SessionFacade.UserId;
            contacto.FechaModificacion = DateTime.Now;
        }

        #endregion
    }
}
