<%@ Page Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopupCongeladoNumeroUnico.aspx.cs" Inherits="SAM.Web.Produccion.PopupCongeladoNumeroUnico"
    EnableSessionState="ReadOnly" EnableViewState="false" %>

<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="divIzquierdo ancho70">
        <div class="separador">
            <asp:Label runat="server" ID="lblNumUnico" meta:resourcekey="lblNumUnico" AssociatedControlID="radNumeroUnico" />
            <telerik:RadComboBox runat="server" ID="radNumeroUnico" meta:resourcekey="radNumeroUnico"
                OnSelectedIndexChanged="radNumeroUnico_SelectedIndexChange" Height="150px" EnableLoadOnDemand="true"
                ShowMoreResultsBox="true" EnableVirtualScrolling="true" OnClientItemsRequesting="Sam.WebService.AgregaNumeroUnicoIDhdn"
                EnableItemCaching="true" AutoPostBack="true" CausesValidation="false">
                <WebServiceSettings Method="ListaNumeroUnicoItemCodePorUserScope" Path="~/WebServices/ComboboxWebService.asmx" />
            </telerik:RadComboBox>
            <span class="required">*</span>
            <asp:CustomValidator meta:resourcekey="valNumUnic" runat="server" ID="cusNumUnic"
                Display="None" ControlToValidate="radNumeroUnico" ValidateEmptyText="true" ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                OnServerValidate="cusNumUnic_ServerValidate" />
        </div>
        <asp:HiddenField runat="server" ID="hdnNumeroUnicoID" />
        <asp:HiddenField runat="server" ID="hdnCantidadCongelada" />
        <asp:PlaceHolder runat="server" ID="phLabels" Visible="false">
            <p>
            </p>
            <div class="cajaAzul ancho90">
                <p>
                    <asp:Label runat="server" ID="ItemCodeTexto" meta:resourcekey="ItemCodeTexto" CssClass="bold" />&nbsp;
                    <asp:Label runat="server" ID="ItemCodeValor" meta:resourcekey="ItemCodeValor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="DescripcionTexto" meta:resourcekey="DescripcionTexto"
                        CssClass="bold" />&nbsp;
                    <asp:Label runat="server" ID="DescripcionValor" meta:resourcekey="DescripcionValor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="Diametro1Texto" meta:resourcekey="Diametro1Texto" CssClass="bold" />&nbsp;
                    <asp:Label runat="server" ID="Diametro1Valor" meta:resourcekey="Diametro1Valor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="Diametro2Texto" meta:resourcekey="Diametro2Texto" CssClass="bold" />&nbsp;
                    <asp:Label runat="server" ID="Diametro2Valor" meta:resourcekey="Diametro2Valor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="InventarioFisicoTexto" meta:resourcekey="InventarioFisicoTexto"
                        CssClass="bold" />&nbsp;
                    <asp:Label runat="server" ID="InventarioFisicoValor" meta:resourcekey="InventarioFisicoValor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="InventarioDañadoTexto" meta:resourcekey="InventarioDañadoTexto"
                        CssClass="bold" />&nbsp;
                    <asp:Label runat="server" ID="InventarioDañadoValor" meta:resourcekey="InventarioDañadoValor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="InventarioCongeladoTexto" meta:resourcekey="InventarioCongeladoTexto"
                        CssClass="bold" />&nbsp;
                    <asp:Label runat="server" ID="InventarioCongeladoValor" meta:resourcekey="InventarioCongeladoValor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="InventarioDisponibleTexto" meta:resourcekey="InventarioDisponibleTexto"
                        CssClass="bold" />&nbsp;
                    <asp:Label runat="server" ID="InventarioDisponibleValor" meta:resourcekey="InventarioDisponibleValor" />
                </p>
            </div>
        </asp:PlaceHolder>
        <p>
        </p>
        <div class="divIzquierdo">
            <div class="separador">
                <samweb:BotonProcesando meta:resourcekey="btnTransferir" ID="btnTransferir" runat="server" OnClick="btnTransferir_Click"
                    CssClass="boton" />
            </div>
        </div>
    </div>
    <div class="divIzquierdo ancho25">
        <div class="validacionesRecuadro" style="margin-top: 20px;">
            <div class="validacionesHeader">
                &nbsp;</div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummaryResource1"
                    Width="120" />
            </div>
        </div>
    </div>
</asp:Content>
