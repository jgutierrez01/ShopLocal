using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Web.Classes;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;
using SAM.BusinessObjects.Produccion;
using Telerik.Web.UI;
using SAM.Web.Webservices;
using SAM.BusinessObjects.Materiales;

namespace SAM.Web.Produccion
{
    public partial class PopupCongeladoNumeroUnico : SamPaginaPopup
    {        

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

        private int _numeroUnico
        {
            get
            {
                if (ViewState["_numeroUnico"] == null)
                {
                    ViewState["_numeroUnico"] = -1;
                }
                return ViewState["_numeroUnico"].SafeIntParse();
            }
            set
            {
                ViewState["_numeroUnico"] = value;
            }
        }

        private int _numeroUnicoTransferirID
        {
            get
            {
                if (ViewState["_numeroUnicoTransferirID"] == null)
                {
                    ViewState["_numeroUnicoTransferirID"] = -1;
                }
                return ViewState["_numeroUnicoTransferirID"].SafeIntParse();
            }
            set
            {
                ViewState["_numeroUnicoTransferirID"] = value;
            }
        }


        private int _cantidadCongelada
        {
            get
            {
                if (ViewState["_cantidadCongelada"] != null)
                {
                    return (int)ViewState["_cantidadCongelada"];
                }
                return -1;
            }
            set
            {
                ViewState["_cantidadCongelada"] = value;
            }
        }

        private string _codigo
        {
            get
            {
                if (ViewState["_codigo"] != null)
                {
                    return ViewState["_codigo"].SafeStringParse();
                }
                return null;
            }
            set
            {
                ViewState["_codigo"] = value;
            }
        }

        private int[] _spools
        {
            get
            {
                if (ViewState["_spools"] != null)
                {
                    return (int[])ViewState["_spools"];
                }
                return null;
            }
            set
            {
                ViewState["_spools"] = value;
            }
        }

        private int[] _matSpool
        {
            get
            {
                if (ViewState["_matSpool"] != null)
                {
                    return (int[])ViewState["_matSpool"];
                }
                return null;
            }
            set
            {
                ViewState["_matSpool"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _spools = Request.QueryString["IDs"].Split(',').Select(n => n.SafeIntParse()).ToArray();
            _cantidadCongelada = Request.QueryString["cantcong"].Split(',').Select(n => n.SafeIntParse()).ToArray().Sum();
            _matSpool = Request.QueryString["MatSpool"].Split(',').Select(n => n.SafeIntParse()).ToArray();
            _proyectoID = Request.QueryString["PID"].SafeIntParse();
            _numeroUnico = Request.QueryString["NUM"].SafeIntParse();
            _codigo = Request.QueryString["CODIGO"].SafeStringParse();
            
            hdnCantidadCongelada.Value = _cantidadCongelada.SafeStringParse();
            hdnNumeroUnicoID.Value = _numeroUnico.SafeStringParse();
        }

        protected void radNumeroUnico_SelectedIndexChange(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (radNumeroUnico.SelectedValue.SafeIntParse() > 0)
            {
                phLabels.Visible = true;
                _numeroUnicoTransferirID = radNumeroUnico.SelectedValue.SafeIntParse();
                CongeladosOdt congeladosodt = CongeladosBO.Instance.ObtenerInfoNumUnico(_proyectoID, _numeroUnicoTransferirID, radNumeroUnico.Text);
                ItemCodeValor.Text = congeladosodt.Codigo;
                DescripcionValor.Text = congeladosodt.Descripcion;
                Diametro1Valor.Text = congeladosodt.Diametro1.SafeStringParse();
                Diametro2Valor.Text = congeladosodt.Diametro2.SafeStringParse();
                InventarioFisicoValor.Text = congeladosodt.InvFisico.SafeStringParse();
                InventarioDañadoValor.Text = congeladosodt.InvDañado.SafeStringParse();
                InventarioCongeladoValor.Text = congeladosodt.InvCongelado.SafeStringParse();
                InventarioDisponibleValor.Text = congeladosodt.InvDisponible.SafeStringParse();
            }
            else
            {
                phLabels.Visible = false;
            }

        }

        protected void btnTransferir_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                _numeroUnicoTransferirID = radNumeroUnico.SelectedValue.SafeIntParse();
                CongeladosBO.Instance.TransferirCongelados(_spools, _matSpool, _codigo, _numeroUnico, _numeroUnicoTransferirID, radNumeroUnico.Text, _cantidadCongelada, _proyectoID, SessionFacade.UserId, DateTime.Now);
                JsUtils.RegistraScriptActualizayCierraVentana(this);
            }
        }

        protected void cusNumUnic_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = radNumeroUnico.SelectedValue.SafeIntParse() > 0;
        }
    }
}