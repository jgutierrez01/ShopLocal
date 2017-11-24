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
    public partial class Estacion : System.Web.UI.UserControl, IMappable
    {
        #region Propiedades privadas
        private List<Entities.Estacion> Estaciones
        {
            get
            {
                if (ViewState["Estaciones"] == null)
                {
                    ViewState["Estaciones"] = new List<Entities.Estacion>();
                }

                return (List<Entities.Estacion>)ViewState["Estaciones"];
            }
            set
            {
                ViewState["Estaciones"] = value;
            }
        }
        private List<Entities.Estacion> EstacionesEliminadas
        {
            get
            {
                if (ViewState["EstacionesEliminadas"] == null)
                {
                    ViewState["EstacionesEliminadas"] = new List<Entities.Estacion>();
                }

                return (List<Entities.Estacion>)ViewState["EstacionesEliminadas"];
            }
            set
            {
                ViewState["EstacionesEliminadas"] = value;
            }
        }
        private List<Entities.Taller> Talleres
        {
            get
            {
                if (ViewState["Talleres"] == null)
                {
                    ViewState["Talleres"] = new List<Entities.Taller>();
                }

                return (List<Entities.Taller>)ViewState["Talleres"];
            }
            set
            {
                ViewState["Talleres"] = value;
            }
        }
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
        #endregion

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            btnAgregar.Visible = true;
            btnActualizar.Visible = false;

            try
            {
                Entities.Estacion estacion;

                if (string.IsNullOrEmpty(hdnEstacionID.Value))
                {
                    if (estacionExiste(txtNombre.Text, ddlTaller.SelectedValue.SafeIntParse()))
                    {
                        throw new ExcepcionDuplicados(MensajesErrorWeb.Excepcion_NombreExiste);
                    }

                    estacion = new Entities.Estacion();
                    estacion.EstacionID = NextID--;
                    Estaciones.Add(estacion);
                }
                else
                {
                    estacion = Estaciones.Where(x => x.EstacionID == hdnEstacionID.Value.SafeIntParse()).Single();
                }

                estacion.Nombre = txtNombre.Text;
                estacion.TallerID = ddlTaller.SelectedValue.SafeIntParse();

                //limpia datos de captura
                limpiarDatosDeCaptura();
                grdEstaciones.DataSource = Estaciones;
                grdEstaciones.DataBind();

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
                        ValidationGroup = "vgEstacion"
                    };
                    Page.Form.Controls.Add(cv);
                }
            }
        }
        protected void grdEstaciones_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdEstaciones.DataSource = Estaciones;
        }
        protected void grdEstaciones_ItemCommand(object sender, GridCommandEventArgs e)
        {
            int estacionID = e.CommandArgument.SafeIntParse();

            //si es edicion
            if (e.CommandName == "Editar")
            {
                btnAgregar.Visible = false;
                btnActualizar.Visible = true;

                Entities.Estacion estacion = Estaciones.Where(x => x.EstacionID == estacionID).SingleOrDefault();
                hdnEstacionID.Value = estacion.EstacionID.ToString();
                txtNombre.Text = estacion.Nombre;
                ddlTaller.SelectedValue = estacion.TallerID.ToString();

            }
            else if (e.CommandName == "Borrar")
            {
                Entities.Estacion estacion = Estaciones.Where(x => x.EstacionID == estacionID).SingleOrDefault();
                Estaciones.Remove(estacion);
                EstacionesEliminadas.Add(estacion);
                grdEstaciones.DataSource = Estaciones;
                grdEstaciones.DataBind();
            }
        }

        private void limpiarDatosDeCaptura()
        {

            txtNombre.Text = String.Empty;
            hdnEstacionID.Value = String.Empty;
            ddlTaller.SelectedIndex = 0;
        }
        private bool estacionExiste(string nombre, int tallerID)
        {
            return Estaciones.Any(x => x.Nombre == nombre && x.TallerID == tallerID);
        }
    
        public void  Map(object entity)
        {
            List<Entities.Taller> lstTalleres = (List<Entities.Taller>)((TrackableCollection<Entities.Taller>)entity).ToList();
            Talleres = lstTalleres;
            Estaciones = Talleres.SelectMany(x => x.Estacion.Select(y => y)).ToList();

            ddlTaller.DataSource = Talleres;
            ddlTaller.DataValueField = "TallerID";
            ddlTaller.DataTextField = "Nombre";
            ddlTaller.DataBind();
            ddlTaller.Items.Insert(0, new ListItem("", "")); 

            grdEstaciones.DataSource = Estaciones;
            grdEstaciones.DataBind();
        }
        public void  Unmap(object entity)
        {
            TrackableCollection<Entities.Taller> lstTalleres = (TrackableCollection<Entities.Taller>)entity;
            List<Entities.Estacion> lstEstaciones = lstTalleres.SelectMany(x => x.Estacion.Select(y => y)).ToList();

            //Barriendo la coleccion de memoria primero
            foreach (Entities.Estacion estacion in Estaciones)
            {
                //buscarlo en BD
                Entities.Estacion bd = EstacionBO.Instance.ObtenerTodos().Where(x => x.EstacionID == estacion.EstacionID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {
                    //Es un alta
                    bd = new Entities.Estacion();
                }

                //copiar propiedades
                bd.StartTracking();
                bd.Nombre = estacion.Nombre;
                bd.TallerID = estacion.TallerID;
                bd.VersionRegistro = estacion.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();

                EstacionBO.Instance.Guarda(bd);
            }

            foreach (Entities.Estacion estacion in EstacionesEliminadas)
            {
                EstacionBO.Instance.Borra(estacion.EstacionID);
            }
        }
    }
}