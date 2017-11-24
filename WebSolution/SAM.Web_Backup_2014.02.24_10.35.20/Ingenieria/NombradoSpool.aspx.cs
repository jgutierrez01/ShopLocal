using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;
using SAM.Web.Classes;
using SAM.Entities;
using SAM.BusinessObjects;
using SAM.BusinessLogic;
using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Exceptions;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using System.Resources;

namespace SAM.Web.Ingenieria
{
	public partial class NombradoSpool : SamPaginaPrincipal
	{
		protected void Page_Load(object sender, EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.ing_NombradoSpool);
                if (Request.QueryString["PID"] != null)
                {
                    int _proyectoId = Request.QueryString["PID"].ToString().SafeIntParse();
                    //cargaInformacion(_proyectoId);
                    cargaInformacion(_proyectoId);
                    
                }
                else
                {
                    cargaInformacion();
                }
            }
		}

        /// <summary>
        /// Carga la información de los proyectos a los que el usuario tiene acceso
        /// </summary>
        public void cargaInformacion()
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
        }

        public void cargaInformacion(int _proyectoId)
        {
            ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            ddlProyecto.Items.FindByValue(_proyectoId.ToString()).Selected = true;
            rcbSpool.Enabled = true;
            proyEncabezado.BindInfo(_proyectoId);
            pnlDatos.Visible = true;
        }

        /// <summary>
        /// Guarda el nuevo nombre del spool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNuevo.Text))
            {
                    BaseValidationException bve = new BaseValidationException(txtMensajeError.Text);
                    RenderErrors(bve);
            }
            else
            {
                string[] temporal = new string[7];
                string[] segmentos = new string[7] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

                temporal = txtNuevo.Text.Split('-');

                Spool spool = SpoolBO.Instance.Obtener(rcbSpool.SelectedValue.SafeIntParse());
                spool.StartTracking();

                int i = 0;
                foreach (string segmento in temporal)
                {
                    if (i <= 6)
                    {
                        segmentos[i] = segmento;
                    }
                    i++;
                }

                spool.Segmento1 = segmentos[0];
                spool.Segmento2 = segmentos[1];
                spool.Segmento3 = segmentos[2];
                spool.Segmento4 = segmentos[3];
                spool.Segmento5 = segmentos[4];
                spool.Segmento6 = segmentos[5];
                spool.Segmento7 = segmentos[6];

                if (segmentos[0] != string.Empty)
                {
                    spool.Nombre = segmentos[0];
                }

                if (segmentos[1] != string.Empty)
                {
                    spool.Nombre += "-" + segmentos[1];
                }

                if (segmentos[2] != string.Empty)
                {
                    spool.Nombre += "-" + segmentos[2];
                }

                if (segmentos[3] != string.Empty)
                {
                    spool.Nombre += "-" + segmentos[3];
                }

                if (segmentos[4] != string.Empty)
                {
                    spool.Nombre += "-" + segmentos[4];
                }

                if (segmentos[5] != string.Empty)
                {
                    spool.Nombre += "-" + segmentos[5];
                }

                if (segmentos[6] != string.Empty)
                {
                    spool.Nombre += "-" + segmentos[6];
                }

                spool.UsuarioModifica = SessionFacade.UserId;
                spool.FechaModificacion = DateTime.Now;

                spool.StopTracking();
                try
                {
                    SpoolBO.Instance.Guarda(spool);
                    UtileriaRedireccion.RedireccionaExitoIngenieria(MensajesIngenieria.NombradoSpool_Titulo,
                                                           MensajesIngenieria.NombradoSpool_Mensaje,
                                                           new List<LigaMensaje>()
                                                        {
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesIngenieria.NombradoSpool_LigaNombradoSpool, 
                                                                Url = WebConstants.IngenieriaUrl.NOMBRADO_SPOOL
                                                            },
                                                            new LigaMensaje
                                                            {
                                                                Texto = MensajesIngenieria.Ingenieria_ListadoIngenieria, 
                                                                Url = WebConstants.IngenieriaUrl.LST_INGENIERIA
                                                            }
                                                        });
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        /// <summary>
        /// Carga la informacion del proyecto seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlProyecto_SelectedIndexChanged(object sender, EventArgs e)
        {
            rcbSpool.Enabled = true;
            proyEncabezado.BindInfo(ddlProyecto.SelectedValue.SafeIntParse());
            pnlDatos.Visible = true;
        }

	    protected void cusRcbSpool_ServerValidate(object source, ServerValidateEventArgs args)
	    {
	        args.IsValid = rcbSpool.SelectedValue.SafeIntParse() > 0;
	    }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("/Ingenieria/DetSpool.aspx?ID={0}",rcbSpool.SelectedValue.SafeIntParse()),false);
        }
	}
}