using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Produccion;
using SAM.Entities;
using SAM.Web.Classes;
using Telerik.Web.UI;

namespace SAM.Web.Controles.Proyecto
{
    public partial class FiltroGenerico : UserControl
    {
        public event EventHandler DdlProyecto_SelectedIndexChanged;
        public event EventHandler RadCmbOrdenTrabajo_OnSelectedIndexChanged;
        public event EventHandler RadCmbNumeroControl_OnSelectedIndexChanged;
        public event EventHandler RadCmbNumeroUnico_OnSelectedIndexChanged;

        public bool FiltroProyecto
        {
            get
            {
                return phProyecto.Visible; 
            }
            set
            {
                phProyecto.Visible = value;
            }
        }
        public bool FiltroCuadrante
        {
            get { return phCuadrantes.Visible; }
            set { phCuadrantes.Visible = value; }
        }
        public bool FiltroNumeroUnico
        {
            get
            {
                return phNumeroUnico.Visible;
            }
            set
            {
                phNumeroUnico.Visible = value;
            }
        }
        public bool FiltroNumeroControl
        {
            get
            {
                return phNumeroControl.Visible;
            }
            set
            {
                phNumeroControl.Visible = value;
            }
        }
        public bool FiltroOrdenTrabajo
        {
            get
            {
                return phOrdenTrabajo.Visible;
            }
            set
            {
                phOrdenTrabajo.Visible = value;
            }
        }

        public bool CuadranteAutoPostBack
        {
            get { return radComboCuadrante.AutoPostBack; }
            set { radComboCuadrante.AutoPostBack = value; }
        }
        public bool ProyectoAutoPostBack
        {
            get
            {
                return ddlProyecto.AutoPostBack;
            }
            set
            {
                ddlProyecto.AutoPostBack = value;
            }
        }
        public bool NumeroUnicoAutoPostBack
        {
            get
            {
                return radCmbNumeroUnico.AutoPostBack;
            }
            set
            {
                radCmbNumeroUnico.AutoPostBack = value;
            }
        }
        public bool NumeroControlAutoPostBack
        {
            get
            {
                return radCmbNumeroControl.AutoPostBack;
            }
            set
            {
                radCmbNumeroControl.AutoPostBack = value;
            }
        }
        public bool OrdenTrabajoAutoPostBack
        {
            get
            {
                return radCmbOrdenTrabajo.AutoPostBack;
            }
            set
            {
                radCmbOrdenTrabajo.AutoPostBack = value;
            }
        }

        public Unit NumeroUnicoWidth
        {
            get
            {
                return radCmbNumeroUnico.Width;
            }
            set
            {
                radCmbNumeroUnico.Width = value;
            }
        }
        public Unit CuadranteWidth
        {
            get { return radComboCuadrante.Width; }
            set { radComboCuadrante.Width = value; }
        }
        public Unit OrdenTrabajoWidth
        {
            get
            {
                return radCmbOrdenTrabajo.Width;
            }
            set
            {
                radCmbOrdenTrabajo.Width = value;
            }
        }
        public Unit NumeroControlWidth
        {
            get
            {
                return radCmbNumeroControl.Width;
            }
            set
            {
                radCmbNumeroControl.Width = value;
            }
        }

        public bool ProyectoRequerido
        {
            get
            {
                return phProyReq.Visible;
            }
            set
            {
                phProyReq.Visible = value;
            }
        }
        public bool CuadranteRequerido
        {
            get { return phFCuadrante.Visible; }
            set { phFCuadrante.Visible = value; }
        }
        public bool NumeroUnicoRequerido
        {
            get
            {
                return phNumUnicoRequerido.Visible;
            }
            set
            {
                phNumUnicoRequerido.Visible = value;
            }
        }
        public bool NumeroControlRequerido
        {
            get
            {
                return phNumControl.Visible;
            }
            set
            {
                phNumControl.Visible = value;
            }
        }
        public bool OrdenTrabajoRequerido
        {
            get
            {
                return phOtReq.Visible;
            }
            set
            {
                phOtReq.Visible = value;
            }
        }

        public string ProyectoHeaderID { get; set; }

        public ListItem ProyectoSelectedItem{
            get { return ddlProyecto.SelectedItem; }
        }
        public RadComboBoxItem CuadranteSelectedItem
        {
            get { return radComboCuadrante.SelectedItem; }
        }
        public int CuadrantesIndex
        {
            get 
            {
                return radComboCuadrante.SelectedIndex;
            }
            set
            {
                radComboCuadrante.SelectedIndex = value;
            }
        }
        public RadComboBoxItem NumeroUnicoSelectedItem
        {
            get
            {
                return radCmbNumeroUnico.SelectedItem;
            }
        }
        public RadComboBoxItem OrdenTrabajoSelectedItem
        {
            get
            {
                return radCmbOrdenTrabajo.SelectedItem;
            }
        }
        public RadComboBoxItem NumeroControlSelectedItem
        {
            get
            {
                return radCmbNumeroControl.SelectedItem;
            }
        }

        public int NumeroControlIndex
        {
            get
            {
                return radCmbNumeroControl.SelectedIndex;
            }
            set
            {
                radCmbNumeroControl.SelectedIndex = value;
            }
        }
        
        public string ProyectoSelectedValue
        {
            get 
            {
                return ddlProyecto.SelectedValue;
            }
        }
        public string CuadranteSelectedValue
        {
            get { return radComboCuadrante.SelectedValue; }
        }
        public string NumeroUnicoSelectedValue
        {
            get
            {
                return radCmbNumeroUnico.SelectedValue;
            }
        }
        public string OrdenTrabajoSelectedValue
        {
            get
            {
                return radCmbOrdenTrabajo.SelectedValue;
            }
        }
        public string NumeroControlSelectedValue
        {
            get
            {
                return radCmbNumeroControl.SelectedValue;
            }
        }

        public bool ProyectoEnabled
        {
            get
            {
                return ddlProyecto.Enabled;
            }
            set
            {
                ddlProyecto.Enabled = value;
            }
        }

        public bool CuadranteEnabled
        {
            get { return radComboCuadrante.Enabled; }
            set { radComboCuadrante.Enabled = value; }
        }
        public bool OrdenTrabajoEnabled
        {
            get
            {
                return radCmbOrdenTrabajo.Enabled;
            }
            set
            {
                radCmbOrdenTrabajo.Enabled = value;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session.IsNewSession || !SessionFacade.EstaLoggeado)
            {
                SeguridadWeb.LogoutImmediately();
            }

            configuraAnchos();
            configuraFiltros();
            configuraPostbacks();
            configuraRequeridos();
            ddlProyecto.Attributes["onChange"] = "Sam.Filtro.DdlProyectoOnClientSelectedIndexChanged()";
        }

        public void LimpiarCombos()
        {
            radComboCuadrante.SelectedIndex = -1;
            radComboCuadrante.Text = "";
            
            radCmbNumeroControl.Text = "";
            radCmbNumeroControl.SelectedIndex = -1;
        }

        public void CargaProyecto(int proyectoID)
        {
            ddlProyecto.SelectedValue = proyectoID.ToString();
        }

        private void configuraAnchos()
        {
            radCmbNumeroControl.Width = NumeroControlWidth;
            radCmbOrdenTrabajo.Width = OrdenTrabajoWidth;
            radCmbNumeroUnico.Width =  NumeroUnicoWidth;
            radComboCuadrante.Width = CuadranteWidth;
        }

        private void configuraRequeridos()
        {
            phProyReq.Visible = ProyectoRequerido;
            rfvProyecto.Enabled = ProyectoRequerido;
            
            phOtReq.Visible = OrdenTrabajoRequerido;
            rfvOt.Enabled = OrdenTrabajoRequerido;
            
            phNumControl.Visible = NumeroControlRequerido;
            rfvNumControl.Enabled = NumeroControlRequerido;
            
            phNumUnicoRequerido.Visible = NumeroUnicoRequerido;
            rfvNumUnico.Enabled = NumeroUnicoRequerido;

            phFCuadrante.Visible = CuadranteRequerido;
            rfvCuadrante.Enabled = CuadranteRequerido;

        }

        private void configuraPostbacks()
        {
            ddlProyecto.AutoPostBack = ProyectoAutoPostBack;
            radCmbOrdenTrabajo.AutoPostBack = OrdenTrabajoAutoPostBack;
            radCmbNumeroUnico.AutoPostBack = NumeroUnicoAutoPostBack;
            radCmbNumeroControl.AutoPostBack = NumeroControlAutoPostBack;
            radComboCuadrante.AutoPostBack = CuadranteAutoPostBack;
        }

        private void configuraFiltros()
        {
            phNumeroControl.Visible = FiltroNumeroControl;
            phNumeroUnico.Visible = FiltroNumeroUnico;
            phProyecto.Visible = FiltroProyecto ;
            phOrdenTrabajo.Visible = FiltroOrdenTrabajo ;
            phCuadrantes.Visible = FiltroCuadrante;

            if (phProyecto.Visible)
            {
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            }
        }

        protected void ddlProyectoSelectedIndexChanged(object sender, EventArgs e)
        {
            radCmbOrdenTrabajo.Items.Clear();
            radCmbNumeroControl.Items.Clear();
            radCmbNumeroUnico.Items.Clear();

            
            estableceProyHeader();

            if(DdlProyecto_SelectedIndexChanged != null)
            {
                DdlProyecto_SelectedIndexChanged(sender, e);
            }
        }

        private void estableceProyHeader()
        {
            //buscamos en el padre el proyect header y bindeamos en caso de estar
            Header proyHeader;
            if (!string.IsNullOrEmpty(ProyectoHeaderID))
            {
                proyHeader = Parent.FindControl(ProyectoHeaderID) as Header;
                if (proyHeader != null)
                {
                    if (!string.IsNullOrEmpty(ProyectoSelectedValue))
                    {
                        proyHeader.BindInfo(ProyectoSelectedValue.SafeIntParse());
                        proyHeader.Visible = true;
                    }
                    else
                    {
                        proyHeader.Visible = false;
                    }
                }
            }
        }

        protected void radCmbOrdenTrabajo_OnSelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int odtID = radCmbOrdenTrabajo.SelectedValue.SafeIntParse();
            //obtener proyectoID y ponerselo a ddlProyecto
            if (odtID != -1)
            {
                OrdenTrabajo ot = OrdenTrabajoBO.Instance.Obtener(odtID);
                if(ot!=null)
                {
                    ddlProyecto.SelectedValue = ot.ProyectoID.ToString();
                    estableceProyHeader();
                }
            }
            //limpiamos numeros Control y numeros Unicos 
            radCmbNumeroUnico.Items.Clear();
            radCmbNumeroControl.Items.Clear();

            if(RadCmbOrdenTrabajo_OnSelectedIndexChanged!=null)
            {
                RadCmbOrdenTrabajo_OnSelectedIndexChanged(o,e);
            }
        }

        protected void radCmbNumeroUnico_OnSelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //Obtenemos numero unico
            int nuID = radCmbNumeroUnico.SelectedValue.SafeIntParse();
            if (nuID != -1)
            {
                NumeroUnico nu = NumeroUnicoBO.Instance.Obtener(nuID);
                if (nu != null)
                {
                    ddlProyecto.SelectedValue = nu.ProyectoID.ToString();
                    estableceProyHeader();
                }
            }
            radCmbNumeroControl.Items.Clear();
            radCmbOrdenTrabajo.Items.Clear();
            if (RadCmbNumeroUnico_OnSelectedIndexChanged!=null)
            {
                RadCmbNumeroUnico_OnSelectedIndexChanged(o,e);
            }
        }

        protected void radComboCuadrante_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }

        protected void radCmbNumeroControl_OnSelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            //Traemos orden de trabajo spool con orden de trabajo para obtener el proyecto ID
            OrdenTrabajoSpool ots = OrdenTrabajoSpoolBO.Instance.ObtenerConOrdenTrabajo(radCmbNumeroControl.SelectedValue.SafeIntParse());
            if(ots!=null)
            {
                //cambiamos el valor del combo del proyecto
                ddlProyecto.SelectedValue = ots.OrdenTrabajo.ProyectoID.ToString();
                //establecemos el control header del proyecto
                estableceProyHeader();
                //cambiamos el valor del combo de la orden de trabajo
                radCmbOrdenTrabajo.SelectedValue = ots.OrdenTrabajo.OrdenTrabajoID.ToString();
                radCmbOrdenTrabajo.Text = ots.OrdenTrabajo.NumeroOrden;    
            }

            //Si hay alguien suscrito al evento lo disparamos
            if (RadCmbNumeroControl_OnSelectedIndexChanged != null)
            {
                RadCmbNumeroControl_OnSelectedIndexChanged(o, e);
            }
        }

        protected void cusValOrdenTrabajo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radCmbOrdenTrabajo.SelectedValue.SafeIntParse() > 0;
        }

        protected void cusValNumeroControl_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radCmbNumeroControl.SelectedValue.SafeIntParse() > 0;
        }

        protected void cusValNumeroUnico_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radCmbNumeroUnico.SelectedValue.SafeIntParse() > 0;
        }

        protected void rfvCuadrante_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radComboCuadrante.SelectedValue.SafeIntParse() > 0;
        }
    }
}
