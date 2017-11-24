using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.Web.Classes;
using SAM.BusinessObjects.Produccion;
using SAM.Entities.Grid;
using SAM.Web.Common;
using SAM.Web.Controles.Spool;
using Mimo.Framework.Exceptions;

namespace SAM.Web.Produccion
{
    public partial class CongeladoParcial : SamPaginaPrincipal
    {
        /// <summary>
        /// JS para confirmar que se desea eliminar una congelado parcial.
        /// </summary>
        private const string JS_BORRAR = "return Sam.Confirma(29,'{0}');";

        private int _proyectoID
        {
            get
            {
                if (ViewState["_proyectoID"] == null)
                {
                    ViewState["_proyectoID"] = -1;
                }
                return ViewState["_proyectoID"].SafeIntParse();
            }
            set
            {
                ViewState["_proyectoID"] = value;
            }
        }

        private int _spoolID
        {
            get
            {
                if (ViewState["_spoolID"] == null)
                {
                    ViewState["_spoolID"] = -1;
                }
                return ViewState["_spoolID"].SafeIntParse();
            }
            set
            {
                ViewState["_spoolID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.prod_CongeladoParcial);
                ddlProyecto.BindToEntiesWithEmptyRow(UserScope.MisProyectos);
            }
        }

        protected void btnMostrar_Click(object sender, EventArgs e)
        {
            _proyectoID = ddlProyecto.SelectedValue.SafeIntParse();
            _spoolID = radSpool.SelectedValue.SafeIntParse();
            SpoolNombre.Text = radSpool.Text;
            MuestraGrid();
        }

        protected void MuestraGrid()
        {
            EstablecerDataSource();
            phMateriales.Visible = true;
            repMateriales.DataBind();
        }

        protected void EstablecerDataSource()
        {
            List<GrdCongeladoParcial> datasource = CongeladosBO.Instance.obtenerListadoCongeladoParcial(_spoolID);
            repMateriales.DataSource = datasource;
        }

        /// <summary>
        /// Se dispara cuando el proyecto seleccionado cambia.  En caso de seleccionar
        /// un proyecto válido se muestra el project header control con sus datos, de lo contrario
        /// se oculta el control.
        /// </summary>
        protected void ddlProyecto_SelectedIndexChange(object sender, EventArgs e)
        {
            int proyectoID = ddlProyecto.SelectedValue.SafeIntParse();

            if (proyectoID <= 0)
            {
                radSpool.Items.Clear();
            }
            else
            {
                headerProyecto.Visible = true;
                headerProyecto.BindInfo(ddlProyecto.SelectedValue.SafeIntParse());                
            }
        }

        protected void btnCongelar_Click(object sender, EventArgs e)
        {
            try
            {
                ComboNuParaCongParcial cmb;
                List<GrdCongeladoParcial> lst = new List<GrdCongeladoParcial>();
                HiddenField hdnfieldCodeID;
                HiddenField hdnfieldCantidad;
                HiddenField hdnfieldMaterialSpool;
                foreach (RepeaterItem item in repMateriales.Items)
                {
                    if (item.IsItem())
                    {
                        cmb = (ComboNuParaCongParcial)item.FindControl("nuCombo");
                        hdnfieldCodeID = (HiddenField)item.FindControl("hdnItemCodeID");
                        hdnfieldCantidad = (HiddenField)item.FindControl("hdnCantidad");
                        hdnfieldMaterialSpool = (HiddenField)item.FindControl("hdnMaterialSpoolID");
                        if (cmb.NumeroUnicoSeleccionadoID > -1)
                        {
                            lst.Add(new GrdCongeladoParcial
                            {
                                ItemCodeID = hdnfieldCodeID.Value.SafeIntParse(),
                                Cantidad = cmb.Cantidad,
                                MaterialSpoolID = cmb.MaterialSpoolID,
                                NumeroUnico = cmb.NumeroUnicoSeleccionadoID,
                                //pedimos prestada la descripción
                                Descripcion = cmb.CodigoSegmento
                            }
                                );
                        }
                    }
                }

                if (CongeladosBO.Instance.validaCantidadesDisponibles(lst))
                {
                    CongeladosBO.Instance.nuevoCongeladoParcial(lst, SessionFacade.UserId);
                    MuestraGrid();
                }
            }
            catch (BaseValidationException bve)
            {
                RenderErrors(bve);
            }
        }

        protected void repeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.IsItem())
            {
                RepeaterItem item = (RepeaterItem)e.Item;
                GrdCongeladoParcial cp = (GrdCongeladoParcial)item.DataItem;
                if (cp.Congelado == null)
                    item.FindControl("imgBorrar").Visible = false;
                else
                {
                    item.FindControl("imgBorrar").Visible = true;
                    ImageButton imgBorrar = (ImageButton)item.FindControl("imgBorrar");

                    imgBorrar.OnClientClick = string.Format(JS_BORRAR, 0);
                }
            }

        }

        protected void repeater_OnItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "borrar")
            {
                int MaterialSpoolID = e.CommandArgument.SafeIntParse();
                int Cantidad = e.CommandArgument.SafeIntParse();

                    CongeladosBO.Instance.eliminarCongeladoParcial(MaterialSpoolID);

                    MuestraGrid();
            }
        }

        protected void cusNumUnic_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radSpool.SelectedValue.SafeIntParse() > 0;
        }
    }
}