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
using SAM.Entities.Personalizadas;

namespace SAM.Mobile.Paginas
{
    public partial class Soldadura : PaginaMovilAutenticado
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
                        SessionFacade.CambioProyectoSigURL = WebConstants.MobileUrl.DETALLESOLDADURA;
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
                        SessionFacade.CambioProyectoSigURL = WebConstants.MobileUrl.DETALLESOLDADURA;
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
                    lblError.Text = MensajesError.Soldadura_SpoolNoExiste;
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

                //Revisar si el spool cuenta con soldadura
                int FabAreaShopID = CacheCatalogos.Instance.ShopFabAreaID;
                int TipoJuntaTHID = CacheCatalogos.Instance.TipoJuntaTHID;
                int TipoJuntaTWID = CacheCatalogos.Instance.TipoJuntaTWID;
                int NoJuntas = JuntaSpoolBO.Instance.ObtenerNumeroDeJuntasPorSpoolIDyCodigoFabAreaFiltroTiposSoldables(spoolID.Value, FabAreaShopID, TipoJuntaTHID, TipoJuntaTWID);

                List<JuntaWorkstatus> lstJuntaWorkstatus = SpoolBO.Instance.ObtenerJuntaWorksatusConSpoolIDYCodigoFabAreaYFiltroTiposSoldables(spoolID.Value, FabAreaShopID, TipoJuntaTHID, TipoJuntaTWID);
                if (lstJuntaWorkstatus.Count(x => x.SoldaduraAprobada) == NoJuntas)
                {
                    //Todas las soldaduras fueron aprobadas
                    lblError.Text = MensajesError.Soldadura_TodasSoldadurasAprobadas;
                    lblError.Visible = true;
                    return;
                }

                //Revisar si el spool no esta en hold
                if (SpoolBO.Instance.SpoolTieneHold(spoolID.Value))
                {
                    lblError.Text = MensajesError.Soldadura_SpoolTieneHold;
                    lblError.Visible = true;
                    return;
                }

                //Enviar a Detalle Soldadura
                Response.Redirect(WebConstants.MobileUrl.DETALLESOLDADURA+ "?ID=" + spoolID);
            }
            else
            {
                lblError.Text = MensajesError.Soldadura_CamposVacios;
                lblError.Visible = true;
            }
        }
    }
}