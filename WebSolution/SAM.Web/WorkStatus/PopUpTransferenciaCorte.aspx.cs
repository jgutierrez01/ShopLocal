using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities;
using SAM.Web.Classes;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Materiales;
using SAM.Web.Common;

namespace SAM.Web.WorkStatus
{
    public partial class PopUpTransferenciaCorte : SamPaginaPopup
    {
        private string IDs
        {
            get
            {
                if (ViewState["IDs"] == null)
                {
                    ViewState["IDs"] = string.Empty;
                }

                return ViewState["IDs"].ToString();
            }
            set
            {
                ViewState["IDs"] = value;
            }
        }

        private int OrdenTrabajoID
        {
            get
            {
                return ViewState["OrdenTrabajoID"].SafeIntParse();
            }
            set
            {
                ViewState["OrdenTrabajoID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IDs = Request.QueryString["IDs"];
                OrdenTrabajoID = Request.QueryString["OT"].SafeIntParse();

                if (!SeguridadQs.TieneAccesoAOrdenDeTrabajo(OrdenTrabajoID))
                {
                    //Generar error 401 (Unauthorized access)
                    string mensaje = string.Format("El usuario {0} está intentando transferir material a corte para una ODT {1} a la cual no tiene permisos", SessionFacade.UserId, OrdenTrabajoID);
                    UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                }

                cargaCombos();
            }
        }

        private void cargaCombos()
        {
            Proyecto proyecto = ProyectoBO.Instance.Obtener(EntityID.Value);
            List<UbicacionFisica> ubicacion = UbicacionFisicaBO.Instance.ObtenerPorPatioID(proyecto.PatioID);
            ddlLocalizacion.BindToEnumerableWithEmptyRow(ubicacion, "Nombre", "UbicacionFisicaID", -1);
        }

        protected void btnTransferir_Click(object sender, EventArgs e)
        {
            //validar los controles de prioridad
            Page.Validate("vgTranferir");

            if (IsValid)
            {

                try
                {
                    int [] idArray = IDs.Split(',').Select(n => int.Parse(n)).ToArray();

                    NumeroUnicoBO.Instance.TransfiereACorte(    idArray,
                                                                SessionFacade.UserId, 
                                                                OrdenTrabajoID, 
                                                                ddlLocalizacion.SelectedValue.SafeIntParse());

                    //Actualiza el grid de la ventana padre para que refleje que el reporte ya se generó
                    JsUtils.RegistraScriptActualizaGridGenerico(this);
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
    }
}