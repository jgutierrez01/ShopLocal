using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using SAM.BusinessLogic;
using SAM.Entities;
using SAM.BusinessObjects;
using SAM.BusinessObjects.Proyectos;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;
using SAM.Web.Common;
using Telerik.Web.UI;
using SAM.Entities.Cache;
using System.Web.Security;
using Mimo.Framework.WebControls;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Proyectos
{
    public partial class DetItemCodes : SamPaginaPrincipal
    {

        private int ProyectoID
        {
            get
            {
                return (int)ViewState["ProyectoID"];
            }
            set
            {
                ViewState["ProyectoID"] = value;
            }
        }

        private int TipoMaterialOrigen
        {
            get {
                return (int) ViewState["TipoMaterialOrigen"];
            }
            set
            {
                ViewState["TipoMaterialOrigen"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                cargaCombos();
                
                if (EntityID != null)
                {
                    if (!SeguridadQs.TieneAccesoAItemCode(EntityID.Value))
                    {
                        //Generar error 401 (Unauthorized access)
                        string mensaje = string.Format("El usuario {0} está intentando accesar un item code {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                        UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                    }

                    ItemCode itemCode = ItemCodeBO.Instance.Obtener(EntityID.Value);
                    VersionRegistro = itemCode.VersionRegistro;
                    Map(itemCode);
                    ProyectoID = itemCode.ProyectoID;
                    TipoMaterialOrigen = itemCode.TipoMaterialID;
                }
                else
                {
                    ProyectoID = Request.QueryString["PID"].SafeIntParse();

                    if (!SeguridadQs.TieneAccesoAProyecto(ProyectoID))
                    {
                        //Generar error 401 (Unauthorized access)
                        string mensaje = string.Format("El usuario {0} está intentando agregar item codes para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, ProyectoID);
                        UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                    }
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_ItemCodes, ProyectoID);
                headerProyecto.BindInfo(ProyectoID);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_ITEM_CODES, ProyectoID);
            }
        }

        private void cargaCombos()
        {
            ddlClasificacion.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerTipoMaterial());
            ddlFamiliaAcero.DataSource = FamiliaAceroBO.Instance.ObtenerTodas();
            ddlFamiliaAcero.DataValueField = "FamiliaAceroID";
            ddlFamiliaAcero.DataTextField = "Nombre";
            ddlFamiliaAcero.DataBind();
            ddlFamiliaAcero.Items.Insert(0, new ListItem { Text = string.Empty });
        }

        public void btnGuardar_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                bool cambioTipoMaterial = false;
                ItemCode itemCode = new ItemCode();
                if (EntityID != null)
                {
                    itemCode = ItemCodeBO.Instance.Obtener(EntityID.Value);
                    itemCode.VersionRegistro = VersionRegistro;
                }
                else
                {
                    itemCode.ProyectoID = ProyectoID;
                }

                itemCode.StartTracking();
                Unmap(itemCode);
                itemCode.Codigo = itemCode.Codigo.TrimEnd();
                itemCode.UsuarioModifica = SessionFacade.UserId;
                itemCode.FechaModificacion = DateTime.Now;
                itemCode.StopTracking();

                cambioTipoMaterial = TipoMaterialOrigen != itemCode.TipoMaterialID ? true : false;

                try
                {
                    //si el material original es un accesorio y cambio or un tubo
                    if (TipoMaterialOrigen == TipoMaterialEnum.Accessorio.SafeIntParse() && itemCode.TipoMaterialID == TipoMaterialEnum.Tubo.SafeIntParse())
                    {
                        ItemCodeBO.Instance.CambioTipoMaterial(itemCode, ProyectoID, SessionFacade.UserId, itemCode.TipoMaterialID);
                    }
                    
                    ItemCodeBO.Instance.Guarda(itemCode);
                    Response.Redirect(string.Format(WebConstants.ProyectoUrl.LST_ITEM_CODES, ProyectoID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

    }
}