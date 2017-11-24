using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using SAM.Entities;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using Telerik.Web.UI;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Excepciones;
using Resources;

namespace SAM.Web.Controles.Patios
{
    public partial class Localizaciones : UserControl, IMappable
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

        private List<Entities.UbicacionFisica> Ubicaciones
        {
            get
            {
                if (ViewState["Ubicaciones"] == null)
                {
                    ViewState["Ubicaciones"] = new List<Entities.UbicacionFisica>();
                }

                return (List<Entities.UbicacionFisica>)ViewState["Ubicaciones"];
            }
            set
            {
                ViewState["Ubicaciones"] = value;
            }
        }

        #endregion

        #region IMappable Members

        public void Map(object entity)
        {
            List<Entities.UbicacionFisica> lstUbicaciones = (List<Entities.UbicacionFisica>)((TrackableCollection<Entities.UbicacionFisica>)entity).ToList();
            Ubicaciones = lstUbicaciones;
            grdUbicaciones.DataSource = Ubicaciones;
            grdUbicaciones.DataBind();
        }

        public void Unmap(object entity)
        {
            TrackableCollection<Entities.UbicacionFisica> lstUbicaciones = (TrackableCollection<Entities.UbicacionFisica>)entity;

            //Buscar en BD para saber si lo borraron
            for (int i = lstUbicaciones.Count - 1; i >= 0; i--)
            {
                Entities.UbicacionFisica UbicacionFisica = lstUbicaciones[i];

                //buscar en memoria
                Entities.UbicacionFisica mem = Ubicaciones.Where(x => x.UbicacionFisicaID == UbicacionFisica.UbicacionFisicaID).SingleOrDefault();

                if (mem == null)
                {
                    //lo borraron
                    UbicacionFisica.StartTracking();
                    UbicacionFisica.MarkAsDeleted();
                    UbicacionFisica.StopTracking();
                }
            }

            //Barriendo la coleccion de memoria primero
            foreach (Entities.UbicacionFisica UbicacionFisica in Ubicaciones)
            {
                //buscarlo en BD
                Entities.UbicacionFisica bd = lstUbicaciones.Where(x => x.UbicacionFisicaID == UbicacionFisica.UbicacionFisicaID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {
                    //Es un alta
                    bd = new Entities.UbicacionFisica();
                    lstUbicaciones.Add(bd);
                }

                //copiar propiedades
                bd.StartTracking();
                bd.Nombre = UbicacionFisica.Nombre;
                bd.EsAreaCorte = UbicacionFisica.EsAreaCorte;
                bd.VersionRegistro = UbicacionFisica.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();
            }
        }

        #endregion

        protected void grdUbicaciones_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdUbicaciones.DataSource = Ubicaciones;
        }

        protected void grdUbicaciones_ItemCommand(object sender, GridCommandEventArgs e)
        {
            int UbicacionFisicaID = e.CommandArgument.SafeIntParse();

            //si es edicion
            if (e.CommandName == "Editar")
            {

                btnAgregar.Visible = false;
                btnActualizar.Visible = true;

                Entities.UbicacionFisica ubicacion = Ubicaciones.Where(x => x.UbicacionFisicaID == UbicacionFisicaID).SingleOrDefault();
                hdnUbicacionID.Value = ubicacion.UbicacionFisicaID.ToString();
                txtNombre.Text = ubicacion.Nombre;
                chkAreacorte.Checked = ubicacion.EsAreaCorte;
               
            }
            else if (e.CommandName == "Borrar")
            {
                Entities.UbicacionFisica UbicacionFisica = Ubicaciones.Where(x => x.UbicacionFisicaID == UbicacionFisicaID).SingleOrDefault();
                Ubicaciones.Remove(UbicacionFisica);
                grdUbicaciones.DataSource = Ubicaciones;
                grdUbicaciones.DataBind();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            btnAgregar.Visible = true;
            btnActualizar.Visible = false;

            try
            {
                Entities.UbicacionFisica UbicacionFisica;

                if (string.IsNullOrEmpty(hdnUbicacionID.Value))
                {

                    //Validar si, no existe el nombre del taller para guardarlo.
                    if (localizacionExiste(txtNombre.Text))
                    {
                        throw new ExcepcionDuplicados(MensajesErrorWeb.Excepcion_NombreExiste);
                    }

                    UbicacionFisica = new Entities.UbicacionFisica();
                    UbicacionFisica.UbicacionFisicaID = NextID--;
                    Ubicaciones.Add(UbicacionFisica);
                }
                else
                {
                    UbicacionFisica = Ubicaciones.Where(x => x.UbicacionFisicaID == hdnUbicacionID.Value.SafeIntParse()).Single();
                }

                UbicacionFisica.Nombre = txtNombre.Text;
                UbicacionFisica.EsAreaCorte = chkAreacorte.Checked;

                //limpia datos de captura
                limpiarDatosDeCaptura();
                grdUbicaciones.DataSource = Ubicaciones;
                grdUbicaciones.DataBind();
            }

            catch (BaseValidationException ex)
            {
                foreach (string detail in ex.Details)
                {
                    var cv = new CustomValidator
                    {
                        ErrorMessage = detail,
                        IsValid = false,
                        Display = ValidatorDisplay.None,
                        ValidationGroup = "vgLocalizacion"
                    };
                    Page.Form.Controls.Add(cv);
                }
            }
        }

        private void limpiarDatosDeCaptura()
        {
            
            txtNombre.Text = String.Empty;
            chkAreacorte.Checked = false;
            hdnUbicacionID.Value = String.Empty;
        }

        private bool localizacionExiste(string nombre)
        {
            return Ubicaciones.Any(x => x.Nombre == nombre);
        }

    }
}
