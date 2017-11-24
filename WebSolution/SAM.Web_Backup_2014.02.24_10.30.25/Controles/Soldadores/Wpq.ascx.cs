using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Data;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities.Cache;
using Telerik.Web.UI;
using Mimo.Framework.WebControls;
using SAM.Entities;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Exceptions;
using Resources;

namespace SAM.Web.Controles.Soldadores
{
    public partial class Wpq : System.Web.UI.UserControl, IMappable
    {

        #region Propiedades ViewState

        public string PatioID
        {
            get
            {
                if (ViewState["PatioID"] == null)
                {
                    ViewState["PatioID"] = -1;
                }
                return ViewState["PatioID"].ToString();
            }
            set
            {
                ViewState["PatioID"] = value;
                cargarCombo();
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

        private List<Entities.Wpq> Wpqs
        {
            get
            {
                if (ViewState["Wpqs"] == null)
                {
                    ViewState["Wpqs"] = new List<Entities.Wpq>();

                }

                return (List<Entities.Wpq>)ViewState["Wpqs"];

            }
            set
            {
                ViewState["Wpqs"] = value;
            }
        }

        #endregion

        #region IMappable Members

        public void WpsCargaCombo(int patio)
        {
           
        }

        public void Map(object entity, string patio)
        {
            List<Entities.Wpq> lstWpqs = (List<Entities.Wpq>)((TrackableCollection<Entities.Wpq>)entity).ToList();
            Wpqs = lstWpqs;
            PatioID = patio;
            grdWps.DataSource = Wpqs;
            grdWps.DataBind();
        }


        public void Map(object entity)
        {
            List<Entities.Wpq> lstWpqs = (List<Entities.Wpq>)((TrackableCollection<Entities.Wpq>)entity).ToList();
            Wpqs = lstWpqs;

            grdWps.DataSource = Wpqs;
            grdWps.DataBind();
        }

        public void Unmap(object entity)
        {
            TrackableCollection<Entities.Wpq> lstWpqs = (TrackableCollection<Entities.Wpq>)entity;

            //Buscar en BD para saber si lo borraron
            for (int i = lstWpqs.Count - 1; i >= 0; i--)
            {
                Entities.Wpq wpq = lstWpqs[i];

                //buscar en memoria
                Entities.Wpq mem = Wpqs.Where(x => x.WpqID == wpq.WpqID).SingleOrDefault();

                if (mem == null)
                {
                    //lo borraron
                    wpq.StartTracking();
                    wpq.MarkAsDeleted();
                    wpq.StopTracking();
                }
            }

            //Barriendo la coleccion de memoria primero
            foreach (Entities.Wpq wpq in Wpqs)
            {
                //buscarlo en BD
                Entities.Wpq bd = lstWpqs.Where(x => x.WpqID == wpq.WpqID).SingleOrDefault();

                //si ya esta en BD es edicion, sino es alta
                if (bd == null)
                {
                    //Es un alta
                    bd = new Entities.Wpq();
                    lstWpqs.Add(bd);
                }

                //copiar propiedades
                bd.StartTracking();
                bd.WpsID = wpq.WpsID;
                bd.FechaInicio = wpq.FechaInicio;
                bd.FechaVigencia = wpq.FechaVigencia;
                bd.VersionRegistro = wpq.VersionRegistro;
                bd.UsuarioModifica = SessionFacade.UserId;
                bd.FechaModificacion = DateTime.Now;
                bd.StopTracking();
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) 
            {
                cargarCombo();
            }
        }

        protected void cargarCombo()
        {
            IList<Wps> wps = SoldadorBO.Instance.ObtenerWpsPorPatio(PatioID.SafeIntParse());
            IEnumerable<WpsCache> Wps =
                CacheCatalogos.Instance.ObtenerWps().Where(
                    x => wps.Select(y => y.WpsID).Contains(x.ID));

            ddlWps.BindToEntiesWithEmptyRow(Wps);
        }

        protected void grdWps_OnNeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdWps.DataSource = Wpqs;
        }

        protected void grdWps_ItemCommand(object sender, GridCommandEventArgs e)
        {
            int wpqID = e.CommandArgument.SafeIntParse();

            //si es edicion
            if (e.CommandName == "Editar")
            {
                btnAgregar.Visible = false;
                btnActualizar.Visible = true;

                Entities.Wpq wpq = Wpqs.Where(x => x.WpqID == wpqID).SingleOrDefault();
                hdnWpqID.Value = wpq.WpqID.ToString();
                ddlWps.SelectedValue = wpq.WpsID.SafeStringParse();
                dtpFechaInicial.SelectedDate = wpq.FechaInicio;
                dtpFechaVencimiento.SelectedDate = wpq.FechaVigencia;
            }
            else if (e.CommandName == "Borrar")
            {
                Entities.Wpq wpq = Wpqs.Where(x => x.WpqID == wpqID).SingleOrDefault();
                Wpqs.Remove(wpq);
                grdWps.DataSource = Wpqs;
                grdWps.DataBind();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            btnAgregar.Visible = true;
            btnActualizar.Visible = false;
            try
            {
                Entities.Wpq wpq;

                if (string.IsNullOrEmpty(hdnWpqID.Value))
                {

                    //Validar si, wsp no esta asignado para guardarlo.
                    if (wpsEstaAsignado(ddlWps.SelectedValue.SafeIntParse()))
                    {
                        throw new ExcepcionDuplicados(MensajesErrorWeb.Excepcion_WpsEstaAsignado);
                    }

                    wpq = new Entities.Wpq();
                    wpq.WpqID = NextID--;
                    Wpqs.Add(wpq);
                }
                else
                {
                    wpq = Wpqs.Where(x => x.WpqID == hdnWpqID.Value.SafeIntParse()).Single();
                }
                //
                wpq.WpsID = ddlWps.SelectedValue.SafeIntParse();
                wpq.Wps = WpsBO.Instance.Obtener(wpq.WpsID);
                wpq.FechaInicio = dtpFechaInicial.SelectedDate.Value;
                wpq.FechaVigencia = dtpFechaVencimiento.SelectedDate.Value;                
                //limpia datos de captura
                limpiarDatosDeCaptura();
                grdWps.DataSource = Wpqs;
                grdWps.DataBind();
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
                        ValidationGroup = "vgWpq"
                    };
                    Page.Form.Controls.Add(cv);
                }
            }
        }

        protected void grdWps_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                //string valuexc = dataItem.FindControl("WpsID").ToString();
                SAM.Entities.Wpq wpss = (SAM.Entities.Wpq)e.Item.DataItem;
                int idWps = wpss.WpsID;//dataItem["WpsID"].Text.SafeIntParse();
                dataItem["Wps"].Text = CacheCatalogos.Instance.ObtenerWps().Where(x => x.ID == idWps).SingleOrDefault().Nombre;
            }
        }        

        private void limpiarDatosDeCaptura()
        {
            ddlWps.SelectedIndex = 0;
            dtpFechaInicial.Clear();
            dtpFechaVencimiento.Clear();
            hdnWpqID.Value = String.Empty;
        }

        private bool wpsEstaAsignado(int wpsID)
        {
            return Wpqs.Any(x => x.WpsID == wpsID);
        }
    }
}
    
