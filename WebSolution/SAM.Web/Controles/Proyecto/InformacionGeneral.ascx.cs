using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessLogic;
using SAM.Entities;
using SAM.BusinessObjects;
using SAM.BusinessObjects.Proyectos;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Cache;
using System.Web.Security;
using Mimo.Framework.WebControls;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Controles.Proyecto
{
    public partial class InformacionGeneral : System.Web.UI.UserControl, IMappable
    {
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// carga la informacion a los combo boxes de cliente y patio dejando un renglón vacío para cada uno
        /// </summary>
        public void CargaCombos()
        {
            ddlCliente.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerClientes());
            ddlPatio.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerPatios());
            dtpFechaInicial.SelectedDate = DateTime.Today;
        }

        #region IMappable Members

        public void Map(object entity)
        {
            Entities.Proyecto proyecto = (Entities.Proyecto)entity;

            ProyectoID = proyecto.ProyectoID;
            VersionRegistro = proyecto.VersionRegistro;

            txtNombre.Text = proyecto.Nombre;
            txtDescripcion.Text = proyecto.Nombre;

            txtCodigoNumUnico.Text = proyecto.ProyectoConfiguracion.PrefijoNumeroUnico;
            txtCodigoOrdTrabajo.Text = proyecto.ProyectoConfiguracion.PrefijoOrdenTrabajo;
            txtDigitosNumUnico.Text = proyecto.ProyectoConfiguracion.DigitosNumeroUnico.ToString();
            txtDigitosOrdTrabajo.Text = proyecto.ProyectoConfiguracion.DigitosOrdenTrabajo.ToString();

            ddlCliente.SelectedValue = proyecto.ClienteID.ToString();
            ddlCliente.Enabled = false;

            ddlPatio.SelectedValue = proyecto.PatioID.ToString();
            ddlPatio.Enabled = false;

            chkStatus.Checked = proyecto.Activo;

            picker.ColorID = proyecto.ColorID;
        }

        public void Unmap(object entity)
        {
            Entities.Proyecto proyecto = (Entities.Proyecto)entity;

            proyecto.StartTracking();

            if (ProyectoID != null)
            {
                proyecto.ProyectoID = ProyectoID.Value;
                proyecto.VersionRegistro = VersionRegistro;
            }

            proyecto.Nombre = txtNombre.Text;
            proyecto.Descripcion = txtDescripcion.Text;
            proyecto.ClienteID = ddlCliente.SelectedValue.SafeIntParse();
            proyecto.FechaInicio = dtpFechaInicial.SelectedDate.Value.Date;
            proyecto.PatioID = ddlPatio.SelectedValue.SafeIntParse();
            proyecto.Activo = chkStatus.Checked;
            proyecto.ColorID = (int)picker.ColorID;
            proyecto.FechaModificacion = DateTime.Now;
            proyecto.UsuarioModifica = SessionFacade.UserId;

            proyecto.ProyectoConfiguracion.StartTracking();

            proyecto.ProyectoConfiguracion.PrefijoNumeroUnico = txtCodigoNumUnico.Text;
            proyecto.ProyectoConfiguracion.PrefijoOrdenTrabajo = txtCodigoOrdTrabajo.Text;
            proyecto.ProyectoConfiguracion.DigitosNumeroUnico = Byte.Parse(txtDigitosNumUnico.Text);
            proyecto.ProyectoConfiguracion.DigitosOrdenTrabajo = Byte.Parse(txtDigitosOrdTrabajo.Text);
            proyecto.ProyectoConfiguracion.CuadroTubero = 0;
            proyecto.ProyectoConfiguracion.CuadroRaiz = 0;
            proyecto.ProyectoConfiguracion.CuadroRelleno = 0;
            proyecto.ProyectoConfiguracion.FechaModificacion = DateTime.Now;
            proyecto.ProyectoConfiguracion.UsuarioModifica = SessionFacade.UserId;
            
        }

        #endregion
    }
}