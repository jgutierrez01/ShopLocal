<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopupDespacho.aspx.cs" Inherits="SAM.Web.WorkStatus.PopupDespacho" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div class="popupDespacho">
        <asp:PlaceHolder runat="server" ID="phControles">
            <h4>
                <asp:Literal meta:resourcekey="lblDespacho" runat="server" ID="lblDespacho" />
            </h4>
            <div>
                <div class="divIzquierdo soloLectura col1">
                    <div class="separador">
                        <mimo:LabeledTextBox meta:resourcekey="txtEtiqueta" runat="server" ID="txtEtiqueta" ReadOnly="true"/>
                        <asp:HiddenField runat="server" ID="hdnMatSpoolID" ClientIDMode="Static" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox meta:resourcekey="txtItemCode" runat="server" ID="txtItemCode" ReadOnly="true" />
                    </div>
                    <div class="separador" style="margin-bottom: 35px;">
                        <mimo:LabeledTextBox meta:resourcekey="txtDescIc" runat="server" ID="txtDescIc" ReadOnly="true" TextMode="MultiLine" Rows="3" Columns="1" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox meta:resourcekey="txtD1" runat="server" ID="txtD1" ReadOnly="true" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox meta:resourcekey="txtD2" runat="server" ID="txtD2" ReadOnly="true" />
                    </div>
                </div>
                <div class="divIzquierdo col2">
                    <asp:PlaceHolder runat="server" ID="phControlesAccesorio" Visible="false">
                        <div class="separador">
                            <asp:Label meta:resourcekey="lblNumeroUnico" runat="server" ID="lblNumeroUnico" AssociatedControlID="rcbNumeroUnico" />
                            <telerik:RadComboBox    ID="rcbNumeroUnico" AutoPostBack="true"
                                                    runat="server"
                                                    Width="200px"
                                                    Height="150px"
                                                    OnClientItemsRequesting="Sam.WebService.ComboOnNumeroUnicoDespachoRequested"
                                                    OnClientSelectedIndexChanged="Sam.WebService.ComboOnNumeroUnicoDespachoIndexChanged"
                                                    OnClientItemDataBound="Sam.WebService.ComboOnNumeroUnicoDespachoAccesorioItemDataBound"
                                                    EnableLoadOnDemand="true"
                                                    ShowMoreResultsBox="true" 
                                                    EnableVirtualScrolling="true" 
                                                    CssClass="required"
                                                    AllowCustomText="false"
                                                    IsCaseSensitive="false"
                                                    DropDownCssClass="liDespacho"
                                                    DropDownWidth="500px">
                                <HeaderTemplate>
                                    <table cellspacing="0" cellpadding="0" class="headerRcbNu">
                                        <tr>
                                            <th class="cod"><asp:Literal runat="server" ID="litCod" meta:resourcekey="litCod" /></th>
                                            <th class="inv"><asp:Literal runat="server" ID="litInv" meta:resourcekey="litInv" /></th>
                                            <th class="diam"><asp:Literal runat="server" ID="litD1" meta:resourcekey="litD1" /></th>
                                            <th class="diam"><asp:Literal runat="server" ID="litD2" meta:resourcekey="litD2" /></th>
                                            <th class="ind">&nbsp;</th>
                                            <th class="codIc"><asp:Literal runat="server" ID="litIcCod" meta:resourcekey="litIcCod" /></th>
                                            <th class="last"><asp:Literal runat="server" ID="litDesc" meta:resourcekey="litDesc" /></th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <WebServiceSettings Path="~/Webservices/ComboboxWebService.asmx" Method="NumerosUnicosParaDespachoDeAccesorio" />
                            </telerik:RadComboBox>
                            <span class="required">*</span>
                            <asp:CustomValidator    runat="server" 
                                                    ID="cusCombo" 
                                                    ControlToValidate="rcbNumeroUnico" 
                                                    ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor" 
                                                    OnServerValidate="cusCombo_ServerValidate"
                                                    ValidateEmptyText="true" 
                                                    Display="None"
                                                    meta:resourcekey="cusCombo" />            
                            <div class="oculto" id="templateNumeroUnicoAccesorio">
                                <table cellpadding="0" cellspacing="0" class="nuDespachoAccesorio">
                                    <tr>
                                        <td class="cod">{{CodigoNumeroUnico}}</td>
                                        <td class="inv">{{InventarioBuenEstado}}</td>
                                        <td class="diam">{{Diametro1}}</td>
                                        <td class="diam">{{Diametro2}}</td>
                                        <td class="ind">{{IndicadorEsEquivalente}}</td>
                                        <td class="codIc">{{CodigoItemCode}}</td>
                                        <td class="last">{{DescripcionItemCode}}</td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phControlesTubo" Visible="false">
                        <div class="separador soloLectura">
                            <mimo:LabeledTextBox meta:resourcekey="txtNumUnico" ID="txtNumUnico" runat="server" ReadOnly="true" />
                        </div>
                    </asp:PlaceHolder>
                    <div class="separador" style="clear:both;height:115px;">
                        <div class="cajaAzul">
                            <div class="datosDespacho">
                                <asp:Label meta:resourcekey="lblCantidadTexto" runat="server" ID="lblCantidadTexto" CssClass="etDespacho" />
                                <asp:Label ID="lblCantidad" runat="server" ClientIDMode="Static" CssClass="textoDespacho" />
                            </div>
                            <div class="datosDespacho">
                                <asp:Label meta:resourcekey="lblIcTexto" runat="server" ID="lblIcTexto" CssClass="etDespacho" />
                                <asp:Label ID="lblIc" runat="server" ClientIDMode="Static" CssClass="textoDespacho" />
                            </div>
                            <div class="datosDespacho">
                                <asp:Label meta:resourcekey="lblIcDescTexto" runat="server" ID="lblIcDescTexto" CssClass="etDespacho" />
                                <asp:Label ID="lblIcDesc" runat="server" ClientIDMode="Static" CssClass="textoDespacho" />
                            </div>
                            <div class="datosDespacho">
                                <asp:Label meta:resourcekey="lblEquivTexto" runat="server" ID="lblEquivTexto" CssClass="etDespacho" />
                                <asp:Label ID="lblEquiv" runat="server" ClientIDMode="Static" CssClass="textoDespacho" />
                                <asp:HiddenField runat="server" ID="hdnNuSeleccionado" ClientIDMode="Static" />
                            </div>
                        </div>
                    </div>
                    <div class="separador soloLectura">
                        <mimo:LabeledTextBox meta:resourcekey="txtCantRequerida" runat="server" ID="txtCantRequerida" ReadOnly="true" />
                    </div>
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox meta:resourcekey="txtCantidad" runat="server" ID="txtCantidad" MaxLength="5" />
                        <asp:RangeValidator meta:resourcekey="rngCantidad" runat="server" ID="rngCantidad" Type="Integer" MinimumValue="1" MaximumValue="99999" ControlToValidate="txtCantidad" Enabled="false" Display="None" />
                        <asp:CompareValidator meta:resourcekey="cmpCantidad" runat="server" ID="cmpCantidad" ControlToValidate="txtCantidad" ControlToCompare="txtCantRequerida" Display="None" Operator="Equal" Enabled="false" />
                        <asp:CompareValidator meta:resourcekey="cmpTubo" runat="server" ID="cmpTubo" ControlToValidate="txtCantidad" Display="None" Operator="Equal" Enabled="false" />
                    </div>
                </div>
                <div class="divIzquierdo col3">
                    <div class="validacionesRecuadro" style="margin-top:20px;">
                        <div class="validacionesHeader">&nbsp;</div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" Width="160" />
                        </div>
                    </div>
                </div>
                <p></p>
            </div>
            <div>
                <asp:Button meta:resourcekey="btnDespachar" runat="server" ID="btnDespachar" Text="Despachar" CssClass="boton" OnClick="btnDespachar_Click" />
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="phMensaje" Visible="false">
            <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin:20px auto 0 auto;">
                <tr>
                    <td rowspan="2" class="icono">
                        <img src="/Imagenes/Iconos/mensajeExito.png" />
                    </td>
                    <td class="titulo">
                        <asp:Label runat="server" ID="lblTituloExito" meta:resourcekey="lblTituloExito" />
                    </td>
                </tr>
                <tr>
                    <td class="cuerpo">
                        <asp:Label runat="server" ID="lblMensajeExito" meta:resourcekey="lblMensajeExito" />
                    </td>
                </tr>
            </table>
        </asp:PlaceHolder>
    </div>
</asp:Content>
