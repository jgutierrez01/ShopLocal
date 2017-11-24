using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities;
using SAM.BusinessObjects.Administracion;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic.Utilerias;
using SAM.BusinessLogic.Administracion;

namespace SAM.Web.Administracion
{
    public partial class PopUpEdicionPendiente : SamPaginaPopup
    {
        private Pendiente _pendiente
        {
            get
            {
                if (ViewState["Pendiente"] == null)
                {
                    ViewState["Pendiente"] = new Pendiente();
                }
                return (Pendiente)ViewState["Pendiente"];
            }
            set
            {
                ViewState["Pendiente"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                cargarControles();
            }
        }

        protected void grdHistorial_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            establecerDataSource();
        }

        private void establecerDataSource()
        {
            grdHistorial.DataSource = PendienteDetalleBO.Instance.ObtenerPorPendienteID(EntityID.Value);
        }

        private void cargarControles()
        {

            
            _pendiente = PendienteBO.Instance.ObtenerPendientePorID(EntityID.Value);

            //Pendiente p = new Pendiente();
            //p = PendienteBO.Instance.ObtenerPendientePorID(EntityID.Value);

            ddlArea.BindToEnumerableWithEmptyRow(CacheCatalogos.Instance.ObtenerCategoriaPendiente(), "Nombre", "ID", -1);
            hdnProyectoID.Value = _pendiente.ProyectoID.SafeStringParse();
            lblProyectoData.Text = _pendiente.Proyecto.Nombre;
            lblTituloData.Text = _pendiente.Titulo;
            lblDescripcionData.Text = _pendiente.Descripcion;
            lblFechaAperturaData.Text = _pendiente.FechaApertura.ToShortDateString();
            ddlArea.SelectedValue = _pendiente.CategoriaPendienteID.SafeStringParse();


            if (_pendiente.Estatus == EstatusPendiente.Abierto)
            {
                ddlEstatus.SelectedValue = "A";
            }

            if (_pendiente.Estatus == EstatusPendiente.Cerrado)
            {
                ddlEstatus.SelectedValue = "C";
            }

            if (_pendiente.Estatus == EstatusPendiente.Resuelto)
            {
                ddlEstatus.SelectedValue = "R";
            }

            rcbResponsable.Text = _pendiente.UsuarioResponsable.Nombre + " " + _pendiente.UsuarioResponsable.ApPaterno + " " + _pendiente.UsuarioResponsable.ApMaterno;
            rcbResponsable.SelectedValue = _pendiente.AsignadoA.SafeStringParse();
            
            establecerDataSource();
            grdHistorial.DataBind();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Pendiente p;
            PendienteDetalle pd = null;
            bool enviaMail = false;

            p = PendienteBO.Instance.ObtenerPendientePorID(EntityID.SafeIntParse());

            if (p == null)
            {
                p = new Pendiente();
            }

            //Derminar si se envia el correo de notificacion.
            //**Cuando se cambia al responsable y el estatus es igual a Abierto.**
            if (p.AsignadoA != rcbResponsable.SelectedValue.SafeGuidParse() && p.Estatus == EstatusPendiente.Abierto)
            {
                enviaMail = true;
            }

            //**Cuando se cambia el estatus de resuleto o cerrado a Abierto.**
            if (p.Estatus != ddlEstatus.SelectedValue && ddlEstatus.SelectedValue == EstatusPendiente.Abierto)
            {
                enviaMail = true;
            }

            p.StartTracking();
            p.CategoriaPendienteID = ddlArea.SelectedValue.SafeIntParse();
            p.AsignadoA = rcbResponsable.SelectedValue.SafeGuidParse();
            p.Estatus = ddlEstatus.SelectedValue;
            p.UsuarioModifica = SessionFacade.UserId;
            p.FechaModificacion = DateTime.Now;
            p.VersionRegistro = VersionRegistro;            

            pd = new PendienteDetalle();
            pd.CategoriaPendienteID = p.CategoriaPendienteID;
            pd.EsAlta = false;
            pd.Responsable = p.AsignadoA;
            pd.Estatus = ddlEstatus.SelectedValue;
            pd.Observaciones = txtObservaciones.Text;
            pd.UsuarioModifica = SessionFacade.UserId;
            pd.FechaModificacion = DateTime.Now;

            p.PendienteDetalle.Add(pd);
            p.StopTracking();
            
            try
            {
                PendienteBL.Instance.Guarda(p, pd.Responsable, lblProyectoData.Text, enviaMail);
                JsUtils.RegistraScriptActualizaGridGenerico(this);
            }
            catch (BaseValidationException ex)
            {
                RenderErrors(ex, "vgEdicionPendiente");
            }

        }
    }
}