using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.MobileControls;
using SAM.Mobile.Clases;
using SAM.Entities;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Produccion;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Workstatus;
using SAM.BusinessObjects.Catalogos;
using SAM.Mobile.Paginas.App_LocalResources;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using System.Configuration;
using Resources;
using SAM.BusinessObjects.Excepciones;

namespace SAM.Mobile.Paginas
{
    public partial class DetalleInspeccionDimensional : PaginaMovilAutenticado
    {
        #region Propiedades
        Spool _spool
        {
            get { return (Spool)ViewState["Spool"]; }
            set { ViewState["Spool"] = value; }
        }
        OrdenTrabajoSpool _ots
        {
            get
            {
                return (OrdenTrabajoSpool)ViewState["OrdenTrabajoSpool"];
            }
            set
            {
                ViewState["OrdenTrabajoSpool"] = value;
            }
        }
        string _fechas
        {
            get { return ViewState["Fechas"].ToString(); }
            set { ViewState["Fechas"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (EntityID != null)
                {
                    Carga();
                    CargaComboFecha();
                }
            }
        }

        protected void cmdOK_OnClick(object sender, EventArgs e)
        {
            
            //Revisar Formato Fecha
            if (lstFecha.Selection.Text.SafeDateAsStringParse() == null)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsVisual_FechaInvalida;
                return;
            }

            //Revisar que hayan seleccionado un resultado
            if (lstResultado.Selection.Text == String.Empty)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsVisual_SeleccionDeResultado;
                return;
            }

          
            //Guardar
            try
            {
                if (Guardar())
                {
                    lblError.Text = MensajesError.InsDimensional_ModificacionOK;
                }
                else
                {
                    lblError.Text = MensajesError.InsDimensional_OK;
                }
                lblError.Visible = true;
                      
            }
            catch (ExcepcionInspeccionDimensionalPatio ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Details[0];
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = MensajesError.InsDimensional_ErrorBD;
            }
        }

        protected void Carga()
        {
            _spool = SpoolBO.Instance.Obtener(EntityID.Value);
            _ots = OrdenTrabajoSpoolBO.Instance.ObtenerOrdenTrabajoPorSpoolID(EntityID.Value);
            _fechas = ValidaFechasBO.Instance.ObtenerFechasSoldaduraPorSpoolConcatenadas(new int[] { _spool.SpoolID }, false);

            if (_fechas != string.Empty)
            {
                hfFechaSold.Value = _fechas;
            }

            hfConfirmacion.Value = MensajesMobile.ConfirmacionFechaMenorLD;
            lblNoOdt2.Text = _ots.OrdenTrabajo.NumeroOrden;
            lblNoControl2.Text = _ots.NumeroControl;
            lblSpool2.Text = _spool.Nombre;
            CargaComboResultados();
        }

        protected void CargaComboResultados()
        {
            lstResultado.Items.Add(new MobileListItem(MensajesMobile.Reprobado, "0"));
            lstResultado.Items.Add(new MobileListItem(MensajesMobile.Aprobado, "1"));
            lstResultado.Items.Insert(0, String.Empty);
        }

        protected bool Guardar()
        {
            string errores = string.Empty;
            bool modificacion = true;

            //Verificar que la junta tenga registro en JuntaWorkstatus
            WorkstatusSpool workstatusSpool = InspeccionDimensionalBO.Instance.ObtenerWorkStatusSpoolPorSpoolID(EntityID.Value);
            InspeccionDimensionalPatio inspeccionDimensional = null;

            if (workstatusSpool !=  null)
            {
                bool estatusReporte = false;
                 
                if (lstResultado.Selection.Value.SafeIntParse() == 1)
                {
                    estatusReporte = true;
                }   

                //Revisar si el spool tiene Reporte
                if (InspeccionDimensionalBO.Instance.TieneLiberacionDimensional(workstatusSpool))
                {
                    //ya tiene inspeccion dimencional patio
                    List<string> error = new List<string>();
                    error.Add(MensajesError.InsDimensional_SpoolYaTieneInspeccionDimensional);
                    throw new ExcepcionInspeccionDimensionalPatio(error);
                }
                
                //Revisar si el spool tiene InspeccionDimensionalPatio
                inspeccionDimensional = InspeccionDimensionalBO.Instance.TieneInspeccionDimensionalPatio(workstatusSpool);
                //{
                //    //ya tiene inspeccion dimencional patio
                //    List<string> error = new List<string>();
                //    error.Add(MensajesError.InsDimensional_TieneInspeccionDimensional);                            
                //    throw new ExcepcionInspeccionDimensionalPatio(error);
                //}

                
            }

            //Generar Registro en InspeccionDimensionalPatio

            if (inspeccionDimensional == null)
            {
                inspeccionDimensional = new InspeccionDimensionalPatio();
                modificacion = false;
            }
            else
            {
                inspeccionDimensional.StartTracking();
            }
            inspeccionDimensional.FechaInspeccion = Convert.ToDateTime(lstFecha.Selection.Text);
            inspeccionDimensional.Observaciones = txtObservaciones.Text;
            inspeccionDimensional.FechaModificacion = DateTime.Now;
            inspeccionDimensional.UsuarioModifica = SessionFacade.UserId;
            
            if (lstResultado.Selection.Value.SafeIntParse() == 1)
            {
                inspeccionDimensional.Aprobado = true;
            }
            else
            {
                inspeccionDimensional.Aprobado = false;
            }

            //creamos o editamos junta workstatus
            if (workstatusSpool == null)
            {
                //No existe, hay que crear registro
                workstatusSpool = new WorkstatusSpool();
                workstatusSpool.OrdenTrabajoSpoolID = _ots.OrdenTrabajoSpoolID;
                workstatusSpool.UltimoProcesoID = UltimoProcesoEnum.InspeccionDimensional.SafeIntParse();

                if (lstResultado.Selection.Value.SafeIntParse() == 1)
                {
                    workstatusSpool.TieneLiberacionDimensional = true;
                }
                else
                {
                    workstatusSpool.TieneLiberacionDimensional = false;
                }

                workstatusSpool.TieneRequisicionPintura = false;
                workstatusSpool.TienePintura = false;
                workstatusSpool.LiberadoPintura = false;
                workstatusSpool.Preparado = false;
                workstatusSpool.Embarcado = false;
                workstatusSpool.Certificado = false;
                workstatusSpool.UsuarioModifica = SessionFacade.UserId;
                workstatusSpool.FechaModificacion = DateTime.Now;
            }
            else
            {  
                workstatusSpool.StartTracking();
                workstatusSpool.UsuarioModifica = SessionFacade.UserId;
                workstatusSpool.FechaModificacion = DateTime.Now;
                workstatusSpool.TieneLiberacionDimensional = inspeccionDimensional.Aprobado;
                workstatusSpool.UltimoProcesoID = UltimoProcesoEnum.InspeccionDimensional.SafeIntParse();
            }

            InspeccionDimensionalBO.Instance.GuardaInspeccionDimensional(workstatusSpool, inspeccionDimensional);
            return modificacion;
        }

        protected void CargaComboFecha()
        {
            int diasAtras = ConfigurationManager.AppSettings["FechaDiasAtras"].SafeIntParse();
            int diasAdelante = ConfigurationManager.AppSettings["FechaDiasAdelante"].SafeIntParse();

            DateTime fechaInicial = DateTime.Now.AddDays(diasAtras * -1);
            DateTime fechaFinal = DateTime.Now.AddDays(diasAdelante);

            for (DateTime fecha = fechaInicial; fecha <= fechaFinal; fecha = fecha.AddDays(1))
            {
                lstFecha.Items.Add(new MobileListItem { Text = fecha.ToShortDateString(), Value = fecha.ToShortDateString() });
            }
        }
    }
}