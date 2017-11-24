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
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Controles.Soldadores
{
    public partial class Soldador : System.Web.UI.UserControl
    {
        public event EventHandler patioSeleccionado;
        #region Propiedades ViewState

        private int? SoldadorID
        {
            get
            {
                if (ViewState["SoldadorID"] != null)
                {
                    return (int)ViewState["SoldadorID"];
                }

                return null;
            }
            set
            {
                ViewState["SoldadorID"] = value;
            }
        }

        public string patioItem
        {
            get
            {
                return ddlPatios.SelectedValue.SafeStringParse();
            }
            set
            {
                ddlPatios.SelectedValue = value;
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
            Entities.Soldador soldador = (Entities.Soldador)entity;

            SoldadorID = soldador.SoldadorID;
            VersionRegistro = soldador.VersionRegistro;

            txtCodigo.Text = soldador.Codigo;
            txtNombre.Text = soldador.Nombre;
            txtApPaterno.Text = soldador.ApPaterno;
            txtApMaterno.Text = soldador.ApMaterno;
            txtNumEmpleado.Text = soldador.NumeroEmpleado;
            ddlPatios.SelectedValue = soldador.PatioID.ToString();
            chkActivo.Checked = soldador.Activo;
        }

        public void Unmap(object entity)
        {
            Entities.Soldador soldador = (Entities.Soldador)entity;

            soldador.StartTracking();

            if (SoldadorID != null)
            {
                soldador.SoldadorID = SoldadorID.Value;
                soldador.VersionRegistro = VersionRegistro;
            }

            soldador.Codigo = txtCodigo.Text;
            soldador.Nombre = txtNombre.Text;
            soldador.ApPaterno = txtApPaterno.Text;
            soldador.ApMaterno = txtApMaterno.Text;
            soldador.NumeroEmpleado = txtNumEmpleado.Text;
            soldador.Activo = chkActivo.Checked;
            soldador.PatioID = ddlPatios.SelectedValue.SafeIntParse();
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            { 
                cargaCombo();
            }
        }

        private void cargaCombo()
        {
            ddlPatios.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerPatios());
        }


        protected void ddlPatios_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (patioSeleccionado != null)
            {
                patioSeleccionado(sender, e);
            }
        }
    }
}