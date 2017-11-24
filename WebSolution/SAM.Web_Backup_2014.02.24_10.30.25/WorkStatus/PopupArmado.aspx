<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopupArmado.aspx.cs" Inherits="SAM.Web.WorkStatus.PopupArmado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal meta:resourcekey="lblArmado" runat="server" ID="lblArmado" />
    </h4>
    <div class="contenedorCentral">
    <input type="hidden" runat="server" id="hdnProyectoID" />
    <div class="divIzquierdo ancho70">
        <div class="divIzquierdo ancho50 boldElements">
            <p>
                <asp:Label runat="server" ID="ltlNumControl" meta:resourcekey="ltlNumControl" />
                <asp:Literal runat="server" ID="NumControl" />
            </p>
            <p>
                <asp:Label runat="server" ID="lblNombreSpool" meta:resourcekey="ltlNombreSpool" />
                <asp:Literal runat="server" ID="NombreSpool" />
            </p>
            <p>
                <asp:Label runat="server" ID="ltlLocalizacion" meta:resourcekey="ltlLocalizacion" />
                <asp:Literal runat="server" ID="Localizacion" />
            </p>
            <p>
                <asp:Label runat="server" ID="ltlTipo" meta:resourcekey="ltlTipo" />
                <asp:Literal runat="server" ID="Tipo" />
            </p>
            <div class="separador">
                <asp:Label runat="server" ID="lblFechaArmado" meta:resourcekey="lblFechaArmado" />
                <br />
                <mimo:MappableDatePicker ID="mdpFechaArmado" runat="server" Style="width: 209px" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="valFechaArmado" meta:resourcekey="valFechaArmado"
                    runat="server" ControlToValidate="mdpFechaArmado" Display="None"></asp:RequiredFieldValidator>
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblNumUnico1" meta:resourcekey="lblNumUnico1" />
                <br />
                <asp:DropDownList runat="server" ID="ddlNumUnico1" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="rvNumUnico1" ControlToValidate="ddlNumUnico1"
                    InitialValue="" meta:resourcekey="rvNumUnico1" Display="None" />
            </div>
            <div class="listaCheck checkBold">
                    <mimo:MappableCheckBox runat="server" ID="chbNumUnico1Pendiente" AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chbNumUnicoPendiente_OnCheckedChanged" meta:resourcekey="chbNumUnico1Pendiente" />
            </div>
            <div class="separador" style="margin-top: 15px">
                <asp:Label runat="server" ID="lblTaller" meta:resourcekey="lblTaller" />
                <br />
                <asp:DropDownList runat="server" ID="ddlTaller" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="rvTaller" ControlToValidate="ddlTaller"
                    InitialValue="" meta:resourcekey="rvTaller" Display="None" />
            </div>
             <br />
            <asp:Button meta:resourcekey="btnArmar" runat="server" ID="btnArmar" Text="Armar"
                CssClass="boton" OnClick="btnArmar_OnClick" />
        </div>
        <div class="divDerecho ancho45 boldElements">
            <p>
                <asp:Label runat="server" ID="ltlJunta" meta:resourcekey="ltlJunta" />
                <asp:Literal runat="server" ID="Junta" />
            </p>
            <p>
                <asp:Label runat="server" ID="ltlCedula" meta:resourcekey="ltlCedula" />
                <asp:Literal runat="server" ID="Cedula" />
            </p>
            <p>
                <asp:Label runat="server" ID="ltlMaterial1" meta:resourcekey="ltlMaterial1" />
                <asp:Literal runat="server" ID="Material1" />
            </p>
            <p>&nbsp;</p>
            <div class="separador">
                <asp:Label runat="server" ID="lblFechaReporte" meta:resourcekey="lblFechaReporte" />
                <br />
                <mimo:MappableDatePicker runat="server" ID="mdpFechaReporte" Style="width: 209px" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="valFechaReporte" meta:resourcekey="valFechaReporte"
                    runat="server" ControlToValidate="mdpFechaReporte" Display="None"></asp:RequiredFieldValidator>
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblNumUnico2" meta:resourcekey="lblNumUnico2" />
                <br />
                <asp:DropDownList runat="server" ID="ddlNumUnico2" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="rvNumUnico2" ControlToValidate="ddlNumUnico2"
                    InitialValue="" meta:resourcekey="rvNumUnico2" Display="None" />
            </div>
            <div class="listaCheck checkBold">
                <mimo:MappableCheckBox runat="server" ID="chbNumUnico2Pendiente" AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chbNumUnicoPendiente_OnCheckedChanged" meta:resourcekey="chbNumUnico2Pendiente" />
            </div>
            <div class="separador" style="margin-top: 15px">
                <asp:Label runat="server" ID="lblTubero" meta:resourcekey="lblTubero" />
                <div id="templateItemCode" class="sys-template">
                    <table class="rcbGenerico" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="codigo">
                                {{Codigo}}
                            </td>
                            <td>
                                {{NombreCompleto}}
                            </td>
                        </tr>
                    </table>
                </div>
                <telerik:RadComboBox ID="rcbTubero" runat="server" Width="200px" Height="150px" OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                    EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true" AutoPostBack="true"
                    CausesValidation="false" OnClientItemDataBound="Sam.WebService.TuberoTablaDataBound"
                    OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                    DropDownCssClass="liGenerico" DropDownWidth="400px">
                    <WebServiceSettings Method="ListaTuberosPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                            <tr>
                                <th class="codigo">
                                    <asp:Literal ID="litCodigo" runat="server" meta:resourcekey="litCodigo"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="litNombre" runat="server" meta:resourcekey="litNombre"></asp:Literal>
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                </telerik:RadComboBox>
                <span class="required">*</span>
                <asp:CustomValidator    
                    meta:resourcekey="valTubero"
                    runat="server" 
                    ID="valTubero" 
                    Display="None" 
                    ControlToValidate="rcbTubero" 
                    ValidateEmptyText="true"                    
                    ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                    OnServerValidate="cusRcbTubero_ServerValidate" />
                
            </div>
            <br />
                <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                    TextMode="MultiLine" Rows="3" MaxLength="500">
                </mimo:LabeledTextBox>
        </div>
    </div>
    <div class="divDerecho ancho25">
        <div class="validacionesRecuadro" style="margin-top: 20px;">
            <div class="validacionesHeader">
                &nbsp;</div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary"
                    Width="120" />
            </div>
        </div>
    </div>
    <p></p>
    </div>
</asp:Content>
