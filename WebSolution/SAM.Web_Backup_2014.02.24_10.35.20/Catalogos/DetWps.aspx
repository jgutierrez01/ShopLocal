<%@ Page Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="DetWps.aspx.cs" Inherits="SAM.Web.Catalogos.DetWps" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Catalogos/LstWps.aspx" meta:resourcekey="lblDetWps" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho35">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre" MaxLength="50" meta:resourcekey="lblNombre" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblMaterial1" AssociatedControlID="ddlMaterial1" meta:resourcekey="lblMaterial1" />
                    <mimo:MappableDropDown runat="server" ID="ddlMaterial1" EntityPropertyName="MaterialBase1ID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvMaterial1" ControlToValidate="ddlMaterial1" Display="None" meta:resourcekey="rfvMaterial1" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblMaterial2" AssociatedControlID="ddlMaterial2" meta:resourcekey="lblMaterial2" />
                    <mimo:MappableDropDown runat="server" ID="ddlMaterial2" EntityPropertyName="MaterialBase2ID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvMaterial2" ControlToValidate="ddlMaterial2" Display="None" meta:resourcekey="rfvMaterial2" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblProcesoRaiz" AssociatedControlID="ddlProcesoRaiz" meta:resourcekey="lblProcesoRaiz" />
                    <mimo:MappableDropDown runat="server" ID="ddlProcesoRaiz" EntityPropertyName="ProcesoRaizID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvProcesoRaiz" ControlToValidate="ddlProcesoRaiz" Display="None" meta:resourcekey="rfvProcesoRaiz" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblProcesoRelleno" AssociatedControlID="ddlProcesoRelleno" meta:resourcekey="lblProcesoRelleno" />
                    <mimo:MappableDropDown runat="server" ID="ddlProcesoRelleno" EntityPropertyName="ProcesoRellenoID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvProcesoRelleno" ControlToValidate="ddlProcesoRelleno" Display="None" meta:resourcekey="rfvProcesoRelleno" />
                </div>
            </div>
            <div class="divIzquierdo ancho35">
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="lblRaizMax" EntityPropertyName="EspesorRaizMaximo" MaxLength="50" meta:resourcekey="lblRaizMax" />
                    <span>mm</span>
                    <asp:RegularExpressionValidator runat="server" ID="rgvRaizMax" ValidationExpression="([0-9]+\.[0-9]{1,3})|([0-9]*\.[0-9]+)|([0-9]+)" ControlToValidate="lblRaizMax" Display="None" meta:resourcekey="rgvRaizMax" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="lblRellenoMax" EntityPropertyName="EspesorRellenoMaximo" MaxLength="50" meta:resourcekey="lblRellenoMax" />
                    <span>mm</span>
                    <asp:RegularExpressionValidator runat="server" ID="rgvRellenoMax" ValidationExpression="([0-9]+\.[0-9]{1,3})|([0-9]*\.[0-9]+)|([0-9]+)" ControlToValidate="lblRellenoMax" Display="None" meta:resourcekey="rgvRellenoMax" />
                    <asp:CompareValidator ID="cmpEspesorRaizRelleno" runat="server" ControlToValidate="lblRellenoMax" ControlToCompare="lblRaizMax" Operator="GreaterThanEqual" meta:resourcekey="cmpEspesorRaizRelleno" Display="None" Type="Double" >
                    </asp:CompareValidator>
                </div>
                <div class="separador">
                    <mimo:MappableCheckBox runat="server" ID="chkRequierePwht" EntityPropertyName="RequierePwht" meta:resourcekey="chkRequierePwht" CssClass="checkYTexto" />
                </div>
                <div class="separador">
                    <mimo:MappableCheckBox runat="server" ID="chkRequierePreheat" EntityPropertyName="RequierePreheat" meta:resourcekey="chkRequierePreheat" CssClass="checkYTexto" />
                </div>            
            </div>
            <div class="divIzquierdo ancho30">
                <div class="validacionesRecuadro" style="margin-top: 23px;">
                    <div class="validacionesHeader">
                    </div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valsummary" EnableClienteScript="true" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true" meta:resourcekey="btnGuardar" CssClass="boton" />
    </div>
</asp:Content>
