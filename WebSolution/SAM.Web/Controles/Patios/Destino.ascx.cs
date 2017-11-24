using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.WebControls;
using SAM.Entities;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using Resources;
using Mimo.Framework.Exceptions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;

namespace SAM.Web.Controles.Patios
{
    public partial class Destino : System.Web.UI.UserControl, IMappable
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

        private List<Entities.Destino> Destinos
        {
            get
            {
                if (ViewState["Destinos"] == null)
                {
                    ViewState["Destinos"] = new List<Entities.Destino>();
                }

                return (List<Entities.Destino>)ViewState["Destinos"];
            }
            set
            {
                ViewState["Destinos"] = value;
            }
        }


        private List<Entities.Cuadrante> Cuadrantes
        {
            get
            {
                if (ViewState["Cuadrantes"] == null)
                {
                    ViewState["Cuadrantes"] = new List<Entities.Cuadrante>();
                }

                return (List<Entities.Cuadrante>)ViewState["Cuadrantes"];
            }
            set
            {
                ViewState["Cuadrantes"] = value;
            }
        }

        #endregion

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            btnAgregar.Visible = true;
            btnActualizar.Visible = false;
            try
            {
                if (!string.IsNullOrEmpty(txtNombre.Text))
                {
                    Entities.Destino destino;

                    if (string.IsNullOrEmpty(hdnDestinoID.Value))
                    {
                        if (destinoExiste(txtNombre.Text))
                        {
                            throw new ExcepcionDuplicados(MensajesErrorWeb.Excepcion_NombreExiste);
                        }

                        destino = new Entities.Destino();
                        destino.DestinoID = NextID--;
                        Destinos.Add(destino);
                    }
                    else
                    {
                        destino = Destinos.Where(x => x.DestinoID == hdnDestinoID.Value.SafeIntParse()).Single();
                    }
                   
                    destino.Nombre = txtNombre.Text;                                                
                                      
                    //limpia datos de captura
                    limpiarDatosDeCaptura();
                    grdDestinos.DataSource = Destinos;
                    grdDestinos.DataBind();
                }

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
                        ValidationGroup = "vgDestino"
                    };
                    Page.Form.Controls.Add(cv);
                }
            }
        }

        private bool destinoExiste(string nombre)
        {
            return Destinos.Any(x => x.Nombre == nombre);
        }

        private void limpiarDatosDeCaptura()
        {
            txtNombre.Text = String.Empty;
            hdnDestinoID.Value = String.Empty;           
        }

        protected void grdDestinos_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdDestinos.DataSource = Destinos;
        }
        protected void grdDestinos_ItemCommand(object sender, GridCommandEventArgs e)
        {
            int destinoID = e.CommandArgument.SafeIntParse();

            //si es edicion
            if (e.CommandName == "Editar")
            {
                btnAgregar.Visible = false;
                btnActualizar.Visible = true;

                Entities.Destino destino = Destinos.Where(x => x.DestinoID == destinoID).SingleOrDefault();
                hdnDestinoID.Value = destino.DestinoID.ToString();
                txtNombre.Text = destino.Nombre;            

            }
            else if (e.CommandName == "Borrar")
            {
                Entities.Destino destino = Destinos.Where(x => x.DestinoID == destinoID).SingleOrDefault();
                Destinos.Remove(destino);

                grdDestinos.DataSource = Destinos;
                grdDestinos.DataBind();

                limpiarDatosDeCaptura();
                btnAgregar.Visible = true;
                btnActualizar.Visible = false;
            }
        }
        public void Map(object entity)
        {
            List<Entities.Destino> lstDestinos = (List<Entities.Destino>)((TrackableCollection<Entities.Destino>)entity).ToList();
            Destinos = lstDestinos;            

            grdDestinos.DataSource = Destinos;
            grdDestinos.DataBind();
        }
        public void Unmap(object entity)
        {

            TrackableCollection<Entities.Destino> lstDestinos = (TrackableCollection<Entities.Destino>)entity;

            //Buscar en BD para saber si lo borraron
            for (int i = lstDestinos.Count - 1; i >= 0; i--)
            {
                Entities.Destino destino = lstDestinos[i];

                //buscar en memoria
                Entities.Destino mem = Destinos.Where(x => x.DestinoID == destino.DestinoID).SingleOrDefault();

                if (mem == null)
                {
                    //lo borraron
                    destino.StartTracking();
                    destino.MarkAsDeleted();
                    destino.StopTracking();
                   
                }
            }

            //Barriendo la coleccion de memoria primero
            foreach (Entities.Destino destino in Destinos)
            {
               
                //buscarlo en BD
                Entities.Destino bd = lstDestinos.Where(x => x.DestinoID == destino.DestinoID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {                    
                    //Es un alta
                    bd = new Entities.Destino();
                    lstDestinos.Add(bd);
                }
                

               
                //copiar propiedades
                bd.StartTracking();
                bd.Nombre = destino.Nombre;               
                bd.VersionRegistro = destino.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();

                
            }


        }
    }
}