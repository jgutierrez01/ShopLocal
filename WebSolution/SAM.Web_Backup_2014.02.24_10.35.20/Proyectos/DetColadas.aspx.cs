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
using Telerik.Web.UI;
using SAM.Entities.Cache;
using System.Web.Security;
using Mimo.Framework.WebControls;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Proyectos
{
    public partial class DetColadas : SamPaginaPrincipal
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (EntityID != null)
                {
                    if (!SeguridadQs.TieneAccesoAColada(EntityID.Value))
                    {
                        //Generar error 401 (Unauthorized access)
                        string mensaje = string.Format("El usuario {0} está intentando accesar una colada {1} a la cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                        UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                    }

                    Colada colada = ColadasBO.Instance.ObtenerConFabricanteYAcero(EntityID.Value);
                    ProyectoID = colada.ProyectoID;
                    VersionRegistro = colada.VersionRegistro;
                    cargaCombos();
                    Map(colada);
                    revisaComboFabricante(colada.Fabricante);
                }
                else
                {
                    ProyectoID = Request.QueryString["PID"].SafeIntParse();

                    if (!SeguridadQs.TieneAccesoAProyecto(ProyectoID))
                    {
                        //Generar error 401 (Unauthorized access)
                        string mensaje = string.Format("El usuario {0} está intentando agregar coladas para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, ProyectoID);
                        UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                    }

                    cargaCombos();
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_Coladas, ProyectoID);
                headerProyecto.BindInfo(ProyectoID);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_COLADAS, ProyectoID);
            }
        }

        protected override void Map(object entity)
        {
            base.Map(entity);
            Colada colada = (Colada)entity;
            txtFamAcero.Text = colada.Acero.FamiliaAcero.Nombre;
            txtFamMaterial.Text = colada.Acero.FamiliaAcero.FamiliaMaterial.Nombre;
        }


        /// <summary>
        /// Revisa si se pudo hacer el binding al fabricante, en caso que no
        /// lo agregamos manualmente pues significa que se dio de baja la relación
        /// entre el fabricante y el proyecto
        /// </summary>
        /// <param name="fabricante"></param>
        private void revisaComboFabricante(Fabricante fabricante)
        {
            if (fabricante != null)
            {
                //Significa que se dio de baja la relación entre el fabricante y el proyecto
                //en este caso agregamos a mano el fabricante
                if (ddlFabricante.SelectedValue.SafeIntParse() != fabricante.FabricanteID)
                {
                    ddlFabricante.Items.Insert(1, new ListItem { Text = fabricante.Nombre, Value = fabricante.FabricanteID.ToString()});
                    ddlFabricante.SelectedValue = fabricante.FabricanteID.ToString();
                }
            }
        }

        protected void cargaCombos()
        {
            IEnumerable<Fabricante> fabricantes = FabricanteBO.Instance
                                                              .ObtenerPorProyecto(ProyectoID)
                                                              .OrderBy(x => x.Nombre);

            ddlAcero.BindToEntiesWithEmptyRow(CacheCatalogos.Instance.ObtenerAceros().OrderBy(x => x.Nombre));
            ddlFabricante.BindToEnumerableWithEmptyRow(fabricantes, "Nombre", "FabricanteID", null);
        }

        public void ddlAcero_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            txtFamAcero.Text = string.Empty;
            txtFamMaterial.Text = string.Empty;

            int aceroID = ddlAcero.SelectedValue.SafeIntParse();

            if (aceroID > 0)
            {
                Acero acero = AceroBO.Instance.ObtenerConFamilias(aceroID);

                if (acero != null)
                {
                    txtFamAcero.Text = acero.FamiliaAcero.Nombre;
                    txtFamMaterial.Text = acero.FamiliaAcero.FamiliaMaterial.Nombre;
                }
            }
        }

        public void btnGuardar_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                Colada colada = new Entities.Colada();
                
                if (EntityID != null)
                {
                    colada = ColadasBO.Instance.ObtenerConFabricanteYAcero(EntityID.Value);
                    colada.VersionRegistro = VersionRegistro;
                }
                else
                {
                    colada.ProyectoID = ProyectoID;
                }

                colada.StartTracking();
                Unmap(colada);
                colada.UsuarioModifica = SessionFacade.UserId;
                colada.FechaModificacion = DateTime.Now;
                colada.StopTracking();

                try
                {
                    ColadasBO.Instance.Guarda(colada);
                    Response.Redirect(String.Format(WebConstants.ProyectoUrl.LST_COLADAS, ProyectoID));
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }
    }
}