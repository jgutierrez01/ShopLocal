using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Catalogos;
using Mimo.Framework.Exceptions;
using SAM.BusinessLogic;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.BusinessLogic.Proyectos;
using SAM.BusinessObjects.Utilerias;
using Resources;
using SAM.Web.Common;

namespace SAM.Web.Proyectos
{
    public partial class DetIcEquivalentes : SamPaginaPrincipal
    {
        #region Propiedades de ViewState

        private List<GrdItemCodeEquivalente> Equivalentes
        {
            get
            {
                if (ViewState["Equivalentes"] != null)
                {
                    return (List<GrdItemCodeEquivalente>)ViewState["Equivalentes"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                ViewState["Equivalentes"] = value;
            }
        }

        private int RowID
        {
            get
            {
                if (ViewState["RowID"] == null)
                {
                    ViewState["RowID"] = -1;
                }

                return (int)ViewState["RowID"];
            }
            set
            {
                ViewState["RowID"] = value;
            }
        }

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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdnGrdClientID.Value = grdIcE.ClientID;

                if (EntityID != null)
                {
                    if (!SeguridadQs.TieneAccesoAItemCodeEquivalente(EntityID.Value))
                    {
                        //Generar error 401 (Unauthorized access)
                        string mensaje = string.Format("El usuario {0} está intentando accesar un item code equivalente {1} al cual no tiene permisos", SessionFacade.UserId, EntityID.Value);
                        UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                    }

                    Equivalentes = ItemCodeEquivalentesBO.Instance.ObtenerGrupoDeEquivalencias(EntityID.Value);
                    ProyectoID = Equivalentes[0].ProyectoID;
                    hdnProyectoID.Value = Equivalentes[0].ProyectoID.ToString();
                    cargaItemCodeBase();
                }
                else
                {
                    ProyectoID = Request.QueryString["PID"].SafeIntParse();

                    if (!SeguridadQs.TieneAccesoAProyecto(ProyectoID))
                    {
                        //Generar error 401 (Unauthorized access)
                        string mensaje = string.Format("El usuario {0} está intentando agregar item codes equivalentes para un proyecto {1} al cual no tiene permisos", SessionFacade.UserId, ProyectoID);
                        UtileriaRedireccion.EnviaPaginaDeAccesoNoAutorizado(mensaje);
                    }

                    hdnProyectoID.Value = ProyectoID.ToString();
                    Equivalentes = new List<GrdItemCodeEquivalente>();
                }

                Master.EstablecerSubMenuActivo(WebConstants.SubMenuItemEnum.proy_ICEquivalentes, ProyectoID);
                headerProyecto.BindInfo(ProyectoID);
                titulo.NavigateUrl = string.Format(WebConstants.ProyectoUrl.LST_ITEM_CODES_EQUIVALENTES, ProyectoID);
                grdIcE.Rebind();
            }
        }

        private void cargaItemCodeBase()
        {
            //Cualquiera de los equivalente trae la información del "padre"
            GrdItemCodeEquivalente ic = Equivalentes[0];

            rcbItemCode.SelectedValue = ic.ItemCodeID.ToString();
            rcbItemCode.Text = ic.CodigoIC;

            txtDiametro1.Text = ic.D1.ToString();
            txtDiametro2.Text = ic.D2.ToString();

            //Como se trata de edición los deshabilitamos
            rcbItemCode.Enabled = false;
            txtDiametro1.Enabled = false;
            txtDiametro2.Enabled = false;
        }

        protected void grdIcE_OnNeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            EstablecerDataSource();
        }

        protected void grdIcE_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                int icEquivalenteID = e.CommandArgument.SafeIntParse();
                //quitarlo de la lista de memoria
                Equivalentes.Remove(Equivalentes.Where(x => x.ItemCodeEquivalenteID == icEquivalenteID).Single());
                grdIcE.Rebind();
            }
        }

        protected void EstablecerDataSource()
        {
            grdIcE.DataSource = Equivalentes;
        }

        protected void btnGuardar_OnClick(object sender, EventArgs e)
        {
            Validate("valGroupGuardar");
            if (IsValid)
            {
                try
                {
                    ItemCodeEquivalenteBL.Instance.GuardaEquivalencias(EntityID, Equivalentes, SessionFacade.UserId, chkUnidireccional.Checked);
                    ViewState.Remove("Equivalentes");
                    Response.Redirect(titulo.NavigateUrl);
                }
                catch (BaseValidationException bve)
                {
                    RenderErrors(bve);
                }
            }
        }

        protected void btnAgregar_OnClick(object sender, EventArgs e)
        {
            Validate("valGroupAgregar");

            if (IsValid)
            {
                #region Validar los diámetros contra cache

                Dictionary<decimal, int> dicDiams = CacheCatalogos.Instance
                                                                  .ObtenerDiametros()
                                                                  .ToDictionary(x => x.Valor, y => y.ID);

                List<string> errores = new List<string>(10);

                validaDiametro(txtDiametro1.Text, dicDiams, errores);
                validaDiametro(txtDiametro2.Text, dicDiams, errores);
                validaDiametro(txtDiametroEquivalente1.Text, dicDiams, errores);
                validaDiametro(txtDiametroEquivalente2.Text, dicDiams, errores);

                if (errores.Count > 0)
                {
                    RenderErrors(errores, "valGroupAgregar");
                    return;
                }

                #endregion

                GrdItemCodeEquivalente icEqNuevo = new GrdItemCodeEquivalente();
                icEqNuevo.ItemCodeEquivalenteID = RowID--;

                if (Equivalentes.Count == 0)
                {
                    //Necesitamos ir a BD por la información del I.C. padre
                    ItemCode icPadre = ItemCodeBO.Instance.Obtener(rcbItemCode.SelectedValue.SafeIntParse());
                    icEqNuevo.ItemCodeID = icPadre.ItemCodeID;
                    icEqNuevo.CodigoIC = icPadre.Codigo;
                    icEqNuevo.DescripcionIC = icPadre.DescripcionEspanol;
                }
                else
                {
                    //Podemos tomar la info del I.C. padre del primer elemento de la lista de manera segura
                    GrdItemCodeEquivalente ic = Equivalentes[0];
                    icEqNuevo.ItemCodeID = ic.ItemCodeID;
                    icEqNuevo.CodigoIC = ic.CodigoIC;
                    icEqNuevo.DescripcionIC = ic.DescripcionIC;
                }

                icEqNuevo.D1 = txtDiametro1.Text.SafeDecimalParse();
                icEqNuevo.D2 = txtDiametro2.Text.SafeDecimalParse();
                icEqNuevo.ProyectoID = hdnProyectoID.Value.SafeIntParse();
                icEqNuevo.ItemEquivalenteID = rcbIcEquivalentes.SelectedValue.SafeIntParse();
                icEqNuevo.D1Eq = txtDiametroEquivalente1.Text.SafeDecimalParse();
                icEqNuevo.D2Eq = txtDiametroEquivalente2.Text.SafeDecimalParse();

                //Este par de datos siempre lo necesitamos sacar de BD
                ItemCode equivalenteSeleccionado = ItemCodeBO.Instance.Obtener(icEqNuevo.ItemEquivalenteID);
                icEqNuevo.CodigoEq = equivalenteSeleccionado.Codigo;
                icEqNuevo.DescripcionEq = equivalenteSeleccionado.DescripcionEspanol;

                //Lo agregamos sí y sólo sí no es un duplicado
                if (!Equivalentes.Any(x => x.ItemCodeID == icEqNuevo.ItemCodeID
                                           && x.ItemEquivalenteID == icEqNuevo.ItemEquivalenteID
                                           && x.D1 == icEqNuevo.D1
                                           && x.D2 == icEqNuevo.D2
                                           && x.D1Eq == icEqNuevo.D1Eq
                                           && x.D2Eq == icEqNuevo.D2Eq))
                {

                    Equivalentes.Add(icEqNuevo);
                    grdIcE.Rebind();
                }

                //Cuando sea un "alta" deshabilitamos/habilitamos la captura en cuanto se agregue el primer
                //elemento a la lista
                if (!EntityID.HasValue)
                {
                    bool congelarLadoIzquierdo = !(Equivalentes.Count > 0);
                    rcbItemCode.Enabled = congelarLadoIzquierdo;
                    txtDiametro1.Enabled = congelarLadoIzquierdo;
                    txtDiametro2.Enabled = congelarLadoIzquierdo;
                }

                txtDiametroEquivalente1.Text = string.Empty;
                txtDiametroEquivalente2.Text = string.Empty;
                rcbIcEquivalentes.Text = string.Empty;
                rcbIcEquivalentes.Items.Clear();
            }
        }

        #region Validaciones

        protected void cusItemCode_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = rcbItemCode.SelectedValue.SafeIntParse() > 0;
        }

        protected void cusCuentaEquivalencias_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = Equivalentes.Count > 0;
        }

        protected void cusIcEquivalente_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = rcbIcEquivalentes.SelectedValue.SafeIntParse() > 0;
        }

        protected void cusDatosIzquierda_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = rcbItemCode.SelectedValue.SafeIntParse() > 0 && !string.IsNullOrWhiteSpace(txtDiametro1.Text) && !string.IsNullOrWhiteSpace(txtDiametro2.Text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dicDiams"></param>
        /// <param name="errores"></param>
        private void validaDiametro(string diametro, Dictionary<decimal, int> dicDiams, List<string> errores)
        {
            decimal diam = diametro.SafeDecimalParse();

            if (diam != 0)
            {
                if (!dicDiams.ContainsKey(diam))
                {
                    errores.Add(string.Format(MensajesErrorWeb.DiametroX_NoExiste, diametro));
                }
            }
        }

        #endregion
    }
}
