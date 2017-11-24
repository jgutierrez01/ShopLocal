using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Catalogos;
using Telerik.Web.UI;
using Mimo.Framework.WebControls;
using SAM.Entities;

namespace SAM.Web.Controles.Cliente
{
    public partial class Cliente : System.Web.UI.UserControl, IMappable
    {

        private int? ClienteID
        {
            get
            {
                if (ViewState["ClienteID"] != null)
                {
                    return (int)ViewState["ClienteID"];
                }

                return null;
            }
            set
            {
                ViewState["ClienteID"] = value;
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
            Entities.Cliente cliente = (Entities.Cliente)entity;

            ClienteID = cliente.ClienteID;
            VersionRegistro = cliente.VersionRegistro;

            txtNombreCliente.Text = cliente.Nombre;
            txtCiudad.Text = cliente.Ciudad;
            txtDireccion.Text = cliente.Direccion;
            txtEstado.Text = cliente.Estado;
            txtPais.Text = cliente.Pais;
        }

        public void Unmap(object entity)
        {
            Entities.Cliente cliente = (Entities.Cliente)entity;

            cliente.StartTracking();

            if (ClienteID != null)
            {
                cliente.ClienteID = ClienteID.Value;
                cliente.VersionRegistro = VersionRegistro;
            }


            cliente.Nombre = txtNombreCliente.Text;
            cliente.Ciudad = txtCiudad.Text;
            cliente.Direccion = txtDireccion.Text;
            cliente.Estado = txtEstado.Text;
            cliente.Pais = txtPais.Text;
        }

        #endregion
    }
}