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
using SAM.BusinessObjects;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Exceptions;
using Resources;

namespace SAM.Web.Controles.Patios
{
    public partial class Taller : UserControl, IMappable
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

        #endregion

        #region IMappable Members

        public void Map(object entity)
        {
            List<Entities.Taller> lstTalleres = (List<Entities.Taller>)((TrackableCollection<Entities.Taller>)entity).ToList();
            Talleres = lstTalleres;
            grdTalleres.DataSource = Talleres;
            grdTalleres.DataBind();
        }

        public void Unmap(object entity)
        {
            TrackableCollection<Entities.Taller> lstTalleres = (TrackableCollection<Entities.Taller>)entity;

            //Buscar en BD para saber si lo borraron
            for (int i = lstTalleres.Count - 1; i >= 0; i--)
            {
                Entities.Taller taller = lstTalleres[i];

                //buscar en memoria
                Entities.Taller mem = Talleres.Where(x => x.TallerID == taller.TallerID).SingleOrDefault();

                if (mem == null)
                {
                    //lo borraron
                    taller.StartTracking();
                    taller.MarkAsDeleted();
                    taller.StopTracking();
                }
            }

            //Barriendo la coleccion de memoria primero
            foreach (Entities.Taller taller in Talleres)
            {
                //buscarlo en BD
                Entities.Taller bd = lstTalleres.Where(x => x.TallerID == taller.TallerID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {
                    //Es un alta
                    bd = new Entities.Taller();
                    lstTalleres.Add(bd);
                }

                //copiar propiedades
                bd.StartTracking();
                bd.Nombre = taller.Nombre;
                bd.VersionRegistro = taller.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();
            }
        }

        #endregion

        protected void grdTalleres_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdTalleres.DataSource = Talleres;
        }

        protected void grdTalleres_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int tallerID = e.CommandArgument.SafeIntParse();

            //si es edicion
            if (e.CommandName == "Editar")
            {
                btnAgregar.Visible = false;
                btnActualizar.Visible = true;

                Entities.Taller taller = Talleres.Where(x => x.TallerID == tallerID).SingleOrDefault();
                hdnTallerID.Value = taller.TallerID.ToString();
                txtNombre.Text = taller.Nombre;
               
            }
            else if (e.CommandName == "Borrar")
            {
                Entities.Taller taller = Talleres.Where(x => x.TallerID == tallerID).SingleOrDefault();
                Talleres.Remove(taller);
                grdTalleres.DataSource = Talleres;
                grdTalleres.DataBind();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            btnAgregar.Visible = true;
            btnActualizar.Visible = false;

            try
            {
   
                Entities.Taller taller;

                if (string.IsNullOrEmpty(hdnTallerID.Value))
                {
                    //Validar si, no existe el nombre del taller para guardarlo.
                    if (tallerExiste(txtNombre.Text))
                    {
                        throw new ExcepcionDuplicados(MensajesErrorWeb.Excepcion_NombreExiste); 
                    }

                    taller = new Entities.Taller();
                    taller.TallerID = NextID--;
                    Talleres.Add(taller);
                }
                else
                {
                    taller = Talleres.Where(x => x.TallerID == hdnTallerID.Value.SafeIntParse()).Single();
                }
            
                taller.Nombre = txtNombre.Text;
            
                //limpia datos de captura
                limpiarDatosDeCaptura();
                grdTalleres.DataSource = Talleres;
                grdTalleres.DataBind();
            
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
                        ValidationGroup = "vgTaller"
                    };
                    Page.Form.Controls.Add(cv);
                }
            }

        }

       

        private void limpiarDatosDeCaptura()
        {
            
            txtNombre.Text = String.Empty;
            hdnTallerID.Value = String.Empty;
        }

        private bool tallerExiste(string nombre)
        {
            return Talleres.Any(x => x.Nombre == nombre);
        }
    }
}
