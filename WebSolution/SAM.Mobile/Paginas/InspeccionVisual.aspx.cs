using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.MobileControls;
using SAM.Mobile.Clases;
using SAM.BusinessObjects.Produccion;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities;
using SAM.Mobile.Paginas.App_LocalResources;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities.Personalizadas;

namespace SAM.Mobile.Paginas
{
    public partial class InspeccionVisual : PaginaMovilAutenticado
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblError.Visible = false;
            }
        }

        protected void cmdOK_OnClik(object sender, EventArgs e)
        {
            if (txtSpool.Text.Trim() != String.Empty || txtNoControl.Text.Trim() != String.Empty)
            {
                int? spoolID;
                if (txtNoControl.Text.Trim() != String.Empty)
                {
                    //Lista trae ya los del patio solamente y los que tiene permisos
                    List<DetSpoolMobile> lstOrdenTrabajoSpool = OrdenTrabajoSpoolBO.Instance
                                                                .ObtenerListaOrdenTrabajoSpoolPorNumeroDeControl(txtNoControl.Text)
                                                                .Where(x => x.PatioID == SessionFacade.PatioID)
                                                                .Where(x => UserScope.MisProyectos.Select(y => y.ID).Contains(x.ProyectoID)).ToList();
                    if (lstOrdenTrabajoSpool.Count == 1)
                    {
                        spoolID = lstOrdenTrabajoSpool[0].SpoolID;
                    }
                    else if (lstOrdenTrabajoSpool.Count > 1)
                    {
                        //aqui se envia a pantalla nueva para selección de proyectos.
                        SessionFacade.CambioProyectoSigURL = WebConstants.MobileUrl.DETALLEINSPECCIONVISUAL;
                        Response.Redirect(WebConstants.MobileUrl.CAMBIOPROYECTO + "?OrdenTrabajo=" + txtNoControl.Text);
                        spoolID = null;
                    }
                    else
                    {
                        spoolID = null;
                    }
                }
                else
                {
                    List<DetSpoolMobile> lstSpool = SpoolBO.Instance.ObtenerPorNombre(txtSpool.Text)
                                                           .Where(x => x.PatioID == SessionFacade.PatioID)
                                                           .Where(x => UserScope.MisProyectos.Select(y => y.ID).Contains(x.ProyectoID)).ToList();


                    if (lstSpool.Count == 1)
                    {
                        spoolID = lstSpool[0].SpoolID;
                    }
                    else if (lstSpool.Count > 1)
                    {
                        //aqui se envia a pantalla nueva para selección de proyectos.
                        SessionFacade.CambioProyectoSigURL = WebConstants.MobileUrl.DETALLEINSPECCIONVISUAL;
                        Response.Redirect(WebConstants.MobileUrl.CAMBIOPROYECTO + "?NombreSpool=" + txtSpool.Text);
                        spoolID = null;
                    }
                    else
                    {
                        spoolID = null;
                    }
                }

                if (spoolID == null || spoolID == 0)
                {
                    lblError.Text = MensajesError.InsVisual_SpoolNoExiste;
                    lblError.Visible = true;
                    return;
                }


                int? PatioID = UserScope.ProyectosPorPatio(SessionFacade.PatioID.Value).Where(x => x.ID == SpoolBO.Instance.Obtener(spoolID.Value).ProyectoID).Select(y => y.PatioID).SingleOrDefault();
                if (PatioID == null || PatioID != SessionFacade.PatioID)
                {
                    lblError.Text = MensajesError.Armado_NoPermisos;
                    lblError.Visible = true;
                    return;
                }

                //Revisar si el spool cuenta con inspeccion visual, o si ya esta soldado y armado
                int NoJuntas = JuntaSpoolBO.Instance.ObtenerNumeroDeJuntasPorSpoolID(spoolID.Value);

                List<JuntaWorkstatus> lstJuntaWorkstatus = InspeccionVisualBO.Instance.ObtenerJuntasWorkstatusParaInspeccionVisualHH(spoolID.Value, CacheCatalogos.Instance.ShopFabAreaID, CacheCatalogos.Instance.TipoJuntaTHID, CacheCatalogos.Instance.TipoJuntaTWID);

                List<int> lstJuntaWorkstatusConInspeccionVisual = lstJuntaWorkstatus.Where(x => x.InspeccionVisualAprobada).Select(x => x.JuntaWorkstatusID).ToList();
         
           
                if (lstJuntaWorkstatusConInspeccionVisual.Count == NoJuntas )
                {
                    //Todas las juntas fueron aprobadas
                    lblError.Text = MensajesError.InsVisual_TodasAprobadas;
                    lblError.Visible = true;
                    return;
                }

                if (lstJuntaWorkstatus.Count == 0)
                {
                    //no hay juntas armadas 
                    lblError.Text = MensajesError.InsVisual_JuntaSinArmar;
                    lblError.Visible = true;
                    return;
                }

                //Revisar si el spool no esta en hold
                if (SpoolBO.Instance.SpoolTieneHold(spoolID.Value))
                {
                    lblError.Text = MensajesError.InsVisual_SpoolTieneHold;
                    lblError.Visible = true;
                    return;
                }

                //Enviar a Detalle Inspeccion Visual
                Response.Redirect(WebConstants.MobileUrl.DETALLEINSPECCIONVISUAL + "?ID=" + spoolID);
            }
            else
            {
                lblError.Text = MensajesError.InsVisual_CamposVacios;
                lblError.Visible = true;
            }
        }
    }
}
