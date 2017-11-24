using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessObjects.Catalogos;
using SAM.Web.Common;
using Telerik.Web.UI;
using Mimo.Framework.WebControls;
using SAM.Entities;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Exceptions;
using Resources;

namespace SAM.Web.Controles.Patios
{
    public partial class Maquina : System.Web.UI.UserControl, IMappable
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

        private List<Entities.Maquina> Maquinas
        {
            get
            {
                if (ViewState["Maquinas"] == null)
                {
                    ViewState["Maquinas"] = new List<Entities.Maquina>();
                }

                return (List<Entities.Maquina>)ViewState["Maquinas"];
            }
            set
            {
                ViewState["Maquinas"] = value;
            }
        }

        #endregion

        #region IMappable Members

        public void Map(object entity)
        {
            List<Entities.Maquina> lstMaquinas = (List<Entities.Maquina>)((TrackableCollection<Entities.Maquina>)entity).ToList();
            Maquinas = lstMaquinas;
            grdMaquinas.DataSource = Maquinas;
            grdMaquinas.DataBind();
        }

        public void Unmap(object entity)
        {
            TrackableCollection<Entities.Maquina> lstMaquinas = (TrackableCollection<Entities.Maquina>)entity;

            //Buscar en BD para saber si lo borraron
            for (int i = lstMaquinas.Count - 1; i >= 0; i--)
            {
                Entities.Maquina maquina = lstMaquinas[i];

                //buscar en memoria
                Entities.Maquina mem = Maquinas.Where(x => x.MaquinaID == maquina.MaquinaID).SingleOrDefault();

                if (mem == null)
                {
                    //lo borraron
                    maquina.StartTracking();
                    maquina.MarkAsDeleted();
                    maquina.StopTracking();
                }
            }

            //Barriendo la coleccion de memoria primero
            foreach (Entities.Maquina maquina in Maquinas)
            {
                //buscarlo en BD
                Entities.Maquina bd = lstMaquinas.Where(x => x.MaquinaID == maquina.MaquinaID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {
                    //Es un alta
                    bd = new Entities.Maquina();
                    lstMaquinas.Add(bd);
                }

                //copiar propiedades
                bd.StartTracking();
                bd.Nombre = maquina.Nombre;
                bd.MermaTeorica = maquina.MermaTeorica;
                bd.VersionRegistro = maquina.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();
            }
        }

        #endregion

        protected void grdMaquinas_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdMaquinas.DataSource = Maquinas;
        }

        protected void grdMaquinas_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            int maquinaID = e.CommandArgument.SafeIntParse();

            //si es edicion
            if (e.CommandName == "Editar")
            {
                btnAgregar.Visible = false;
                btnActualizar.Visible = true;

                Entities.Maquina maquina = Maquinas.Where(x => x.MaquinaID == maquinaID).SingleOrDefault();
                hdnMaquinaID.Value = maquina.MaquinaID.ToString();
                txtNombre.Text = maquina.Nombre;
                txtMermaTeorica.Text = maquina.MermaTeorica!=-1 ? maquina.MermaTeorica.SafeStringParse() : "";
            }
            else if (e.CommandName == "Borrar")
            {
                Entities.Maquina maquina = Maquinas.Where(x => x.MaquinaID == maquinaID).SingleOrDefault();
                Maquinas.Remove(maquina);
                grdMaquinas.DataSource = Maquinas;
                grdMaquinas.DataBind();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            btnAgregar.Visible = true;
            btnActualizar.Visible = false;

            try
            {
                Entities.Maquina maquina;

                if (string.IsNullOrEmpty(hdnMaquinaID.Value))
                {

                    //Validar si, no existe el nombre del taller para guardarlo.
                    if (maquinaExiste(txtNombre.Text))
                    {
                        throw new ExcepcionDuplicados(MensajesErrorWeb.Excepcion_NombreExiste);
                    }

                    maquina = new Entities.Maquina();
                    maquina.MaquinaID = NextID--;
                    Maquinas.Add(maquina);
                }
                else
                {
                    maquina = Maquinas.Where(x => x.MaquinaID == hdnMaquinaID.Value.SafeIntParse()).Single();
                }

                maquina.Nombre = txtNombre.Text;

                maquina.MermaTeorica = string.IsNullOrEmpty(txtMermaTeorica.Text) ? 0 : txtMermaTeorica.Text.SafeIntParse();

                //limpia datos de captura
                limpiarDatosDeCaptura();
                grdMaquinas.DataSource = Maquinas;
                grdMaquinas.DataBind();
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
                        ValidationGroup = "vgMaquina"
                    };
                    Page.Form.Controls.Add(cv);
                }
            }

        }

        private void limpiarDatosDeCaptura()
        {

            txtNombre.Text = String.Empty;
            txtMermaTeorica.Text = String.Empty;
            hdnMaquinaID.Value = String.Empty;
        }

        private bool maquinaExiste(string nombre)
        {
            return Maquinas.Any(x => x.Nombre == nombre);
        }
    }
}