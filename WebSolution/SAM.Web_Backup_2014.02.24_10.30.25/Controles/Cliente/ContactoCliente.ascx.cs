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
using SAM.Web.Classes;
using Mimo.Framework.Extensions;

namespace SAM.Web.Controles.Cliente
{
    public partial class Contacto : System.Web.UI.UserControl, IMappable
    {

        #region Propiedades ViewState

        private int NextID
        {
            get
            {
                if (ViewState["NextID"] == null)
                {
                    ViewState["NextID"] = -1;
                }
                return (int)ViewState["NextID"];
            }
            set
            {
                ViewState["NextID"] = value;
            }
        }

        private List<ContactoCliente> Contactos
        {
            get
            {
                if (ViewState["Contactos"] == null)
                {
                    ViewState["Contactos"] = new List<ContactoCliente>();
                }

                return (List<ContactoCliente>)ViewState["Contactos"];
            }
            set
            {
                ViewState["Contactos"] = value;
            }
        }

        #endregion

        #region IMappable Members

        public void Map(object entity)
        {
            List<ContactoCliente> lstContactos = (List<ContactoCliente>)((TrackableCollection<ContactoCliente>)entity).ToList();
            Contactos = lstContactos;
            grdContactos.DataSource = Contactos;
            grdContactos.DataBind();
        }

        public void Unmap(object entity)
        {
            TrackableCollection<ContactoCliente> lstContactos = (TrackableCollection<ContactoCliente>)entity;

            //Buscar en BD para saber si lo borraron
            for (int i = lstContactos.Count - 1; i >= 0; i--)
            {
                ContactoCliente cntCte = lstContactos[i];

                //buscar en memoria
                ContactoCliente mem = Contactos.Where(x => x.ContactoClienteID == cntCte.ContactoClienteID).SingleOrDefault();

                if (mem == null)
                {
                    //lo borraron
                    cntCte.StartTracking();
                    cntCte.MarkAsDeleted();
                    cntCte.StopTracking();
                }
            }

            //Barriendo la coleccion de memoria primero
            foreach (ContactoCliente cntCte in Contactos)
            {
                //buscarlo en BD
                ContactoCliente bd = lstContactos.Where(x => x.ContactoClienteID == cntCte.ContactoClienteID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {
                    //Es un alta
                    bd = new ContactoCliente();
                    lstContactos.Add(bd);
                }

                //copiar propiedades
                bd.StartTracking();
                bd.Puesto = cntCte.Puesto;
                bd.Nombre = cntCte.Nombre;
                bd.ApPaterno = cntCte.ApPaterno;
                bd.ApMaterno = cntCte.ApMaterno;
                bd.CorreoElectronico = cntCte.CorreoElectronico;
                bd.TelefonoOficina = cntCte.TelefonoOficina;
                bd.TelefonoParticular = cntCte.TelefonoParticular;
                bd.TelefonoCelular = cntCte.TelefonoCelular;
                bd.VersionRegistro = cntCte.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();
            }
        }

        #endregion

        protected void grdContactos_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdContactos.DataSource = Contactos;
        }

        protected void grdContactos_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int contactoID = e.CommandArgument.SafeIntParse();

            //si es edicion
            if (e.CommandName == "Editar")
            {
                btnAgregar.Visible = false;
                btnActualizar.Visible = true;
                
                ContactoCliente cntCte = Contactos.Where(x => x.ContactoClienteID == contactoID).SingleOrDefault();
                hdnContactoClienteID.Value = cntCte.ContactoClienteID.ToString();
                txtPuesto.Text = cntCte.Puesto;
                txtNombre.Text = cntCte.Nombre;
                txtApPaterno.Text = cntCte.ApPaterno;
                txtApMaterno.Text = cntCte.ApMaterno;
                txtCorreo.Text = cntCte.CorreoElectronico;
                txtTelOficina.Text = cntCte.TelefonoOficina;
                txtTelParticular.Text = cntCte.TelefonoParticular;
                txtTelCelular.Text = cntCte.TelefonoCelular;
            }
            else if (e.CommandName == "Borrar")
            {
                ContactoCliente cntCte = Contactos.Where(x => x.ContactoClienteID == contactoID).SingleOrDefault();
                Contactos.Remove(cntCte);
                grdContactos.DataSource = Contactos;
                grdContactos.DataBind();
            }
        }  

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            btnAgregar.Visible = true;
            btnActualizar.Visible = false;

            ContactoCliente cntCte;

            if (string.IsNullOrEmpty(hdnContactoClienteID.Value))
            {
                cntCte = new ContactoCliente();
                cntCte.ContactoClienteID = NextID--;
                Contactos.Add(cntCte);
            }
            else
            {
                cntCte = Contactos.Where(x => x.ContactoClienteID == hdnContactoClienteID.Value.SafeIntParse()).Single();
            }
            
            cntCte.Puesto = txtPuesto.Text;
            cntCte.Nombre = txtNombre.Text;
            cntCte.ApPaterno = txtApPaterno.Text;
            cntCte.ApMaterno = txtApMaterno.Text;
            cntCte.CorreoElectronico = txtCorreo.Text;
            cntCte.TelefonoOficina = txtTelOficina.Text;
            cntCte.TelefonoParticular = txtTelParticular.Text;
            cntCte.TelefonoCelular = txtTelCelular.Text;

            //limpia datos de captura
            limpiarDatosDeCaptura();
            
            grdContactos.DataSource = Contactos;
            grdContactos.DataBind();
            
        }

        private void limpiarDatosDeCaptura()
        {
            txtPuesto.Text = String.Empty;
            txtNombre.Text = String.Empty;
            txtApPaterno.Text = String.Empty;
            txtApMaterno.Text = String.Empty;
            txtCorreo.Text = String.Empty;
            txtTelOficina.Text = String.Empty;
            txtTelParticular.Text = String.Empty;
            txtTelCelular.Text = String.Empty;
            hdnContactoClienteID.Value = String.Empty;
        }

    }
}