<%@ Page Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpAgregaItemCode.aspx.cs" Inherits="SAM.Web.Materiales.PopUpAgregaItemCode" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlCampos" runat="server">
        <h4>
            <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
        </h4>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:RadioButton ID="rdbNuevo" runat="server" meta:resourcekey="rdbNuevo" GroupName="AltaIC"
                    CssClass="checkBold" Checked="true" OnCheckedChanged="radio_CheckedChanged" AutoPostBack="true" /><br />
                <asp:RadioButton ID="rdbImportar" runat="server" meta:resourcekey="rdbImportar" GroupName="AltaIC"
                    CssClass="checkBold" OnCheckedChanged="radio_CheckedChanged" AutoPostBack="true"/>
            </div>
            <asp:Panel ID="pnNuevo" runat="server">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtItemCode" MaxLength="50" Label="Item Code:"
                        CssClass="required" meta:resourcekey="txtItemCode" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtDescripcionEspanol" MaxLength="150"
                        meta:resourcekey="txtDescripcionEspanol" CssClass="required" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtItemCodeCliente" MaxLength="50" meta:resourcekey="txtItemCodeCliente" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblClasificacion" meta:resourcekey="lblClasificacion"
                        CssClass="bold" />
                    <br />
                    <asp:DropDownList ID="ddlClasificacion" runat="server">
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="reqClasificacion" runat="server" Display="None" ControlToValidate="ddlClasificacion"
                        meta:resourcekey="reqClasificacion" InitialValue="" />
                </div>
                <div class="separador">
                    <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" meta:resourcekey="btnGuardar"
                        CssClass="boton" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnImportar" runat="server" Visible="false">
                <div class="separador">
                    <asp:Label runat="server" ID="lblProyecto" meta:resourcekey="lblProyecto" CssClass="bold" />
                    <br />
                    <asp:DropDownList ID="ddlProyecto" runat="server">
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valProyecto" runat="server" Display="None" ControlToValidate="ddlProyecto"
                        meta:resourcekey="valProyecto" InitialValue="" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblItemCode" meta:resourcekey="lblItemCode" CssClass="bold" />
                    <br />
                    <div id="templateItemCode" class="sys-template">
                        <table class="rcbGenerico" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="codigo">
                                    {{Codigo}}
                                </td>
                                <td>
                                    {{Descripcion}}
                                </td>
                            </tr>
                        </table>
                    </div>
                    <telerik:RadComboBox ID="rcbItemCode" runat="server" Width="200px" Height="150px"
                        OnSelectedIndexChanged="rcbItemCode_SelectedIndexChanged" OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                        EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                        CausesValidation="false" AutoPostBack="true" OnClientItemDataBound="Sam.WebService.ItemCodeTablaDataBound"
                        OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                        DropDownCssClass="liGenerico" DropDownWidth="400px">
                        <WebServiceSettings Method="ListaItemCodesPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                                <tr>
                                    <th class="codigo">
                                        <asp:Literal ID="litCodigo" runat="server" meta:resourcekey="litCodigo"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="litDescripcion" runat="server" meta:resourcekey="litDescripcion"></asp:Literal>
                                    </th>
                                </tr>
                            </table>
                        </HeaderTemplate>
                    </telerik:RadComboBox>
                    <span class="required">*</span>
                    <asp:CustomValidator meta:resourcekey="valItemCode" runat="server" ID="cusItemCode"
                        Display="None" ControlToValidate="rcbItemCode" ValidateEmptyText="true" ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                        OnServerValidate="cusItemCode_ServerValidate" />
                </div>
                <asp:Panel ID="pnDatosIC" runat="server" Visible="false">
                <div class="cajaAzul">
                    <asp:Label ID="lblDescripcion" runat="server" meta:resourcekey="lblDescripcion" CssClass="bold"></asp:Label>
                    <asp:Label ID="lblDescripcionText" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblItemCodeCliente" runat="server" meta:resourcekey="lblItemCodeCliente"
                        CssClass="bold"></asp:Label>
                    <asp:Label ID="lblItemCodeClienteText" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblTipoMaterial" runat="server" meta:resourcekey="lblTipoMaterial"
                        CssClass="bold"></asp:Label>
                    <asp:Label ID="lblTipoMaterialText" runat="server"></asp:Label>
                    <br />
                    </div>
                </asp:Panel>
                <div class="separador">
                    <asp:Button runat="server" ID="btnImportar" OnClick="btnImportar_Click" meta:resourcekey="btnImportar"
                        CssClass="boton" />
                </div>
            </asp:Panel>
        </div>
        <div class="divDerecho ancho50">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummary"
                        CssClass="summary" />
                </div>
            </div>
        </div>
        <p>
        </p>
    </asp:Panel>
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false">
        <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin: 5px auto 0 auto;">
            <tr>
                <td rowspan="2" class="icono">
                    <img src="/Imagenes/Iconos/mensajeExito.png" alt="" />
                </td>
                <td class="titulo">
                    <asp:Label runat="server" ID="lblTituloExito" meta:resourcekey="lblTituloExito" />
                </td>
            </tr>
            <tr>
                <td class="cuerpo">
                    <asp:Label ID="Label1" runat="server" meta:resourcekey="lblMensaje"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
