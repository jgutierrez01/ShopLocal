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


namespace SAM.Web.Controles.Patios
{
    public partial class Patio : System.Web.UI.UserControl, IMappable
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private int? PatioID
        {
            get
            {
                if (ViewState["PatioID"] != null)
                {
                    return (int)ViewState["PatioID"];
                }

                return null;
            }
            set
            {
                ViewState["PatioID"] = value;
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
            Entities.Patio patio = (Entities.Patio)entity;

            PatioID = patio.PatioID;
            VersionRegistro = patio.VersionRegistro;

            txtNombre.Text = patio.Nombre;
            txtPropietario.Text = patio.Propietario;
            txtDescripcion.Text = patio.Descripcion;
        }

        public void Unmap(object entity)
        {
            Entities.Patio patio = (Entities.Patio)entity;

            patio.StartTracking();

            if (PatioID != null)
            {
                patio.PatioID = PatioID.Value;
                patio.VersionRegistro = VersionRegistro;
            }


            patio.Nombre = txtNombre.Text;
            patio.Propietario = txtPropietario.Text;
            patio.Descripcion = txtDescripcion.Text;
        }

        #endregion
    }
}