<%@ Page Language="C#" MasterPageFile="~/Masters/PopupJuntasCampo.Master" AutoEventWireup="true" CodeBehind="Armado.aspx.cs" Inherits="SAM.Web.Produccion.JuntasCampo.Armado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
    <telerik:RadAjaxManager runat="server" ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnGuardar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlControles" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnEliminar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlControles" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radCmbSpool2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlControles" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlEtiquetaMaterial">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlControles" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:Panel runat="server" ID="pnlControles">
        <h4>
            <asp:Literal meta:resourcekey="lblArmado" runat="server" ID="lblArmado" />
        </h4>
        <div class="cajaAzul">
            <div class="divIzquierdo ancho50">
                <p>
                    <asp:Label runat="server" ID="lblSpool" meta:resourcekey="lblSpool" CssClass="bold" />
                    <asp:Literal runat="server" ID="litSpool" />
                </p>
                <p>
                    <asp:Label runat="server" ID="lblJunta" meta:resourcekey="lblJunta" CssClass="bold" />
                    <asp:Literal runat="server" ID="litJunta" />
                </p>
                <p>
                    <asp:Label runat="server" ID="lblTipoJunta" meta:resourcekey="lblTipoJunta" CssClass="bold" />
                    <asp:Literal runat="server" ID="litTipoJunta" />
                </p>
            </div>
            <div class="divIzquierdo">
                <p>
                    <asp:Label runat="server" ID="lblNumeroControl" meta:resourcekey="lblNumeroControl" CssClass="bold" />
                    <asp:Literal runat="server" ID="litNumeroControl"></asp:Literal>
                </p>
                <p>
                    <asp:Label runat="server" ID="lblLocalizacion" meta:resourcekey="lblLocalizacion" CssClass="bold" />
                    <asp:Literal runat="server" ID="litLocalizacion" />
                </p>
                <p>
                    <asp:Label runat="server" ID="lblEspesor" meta:resourcekey="lblEspesor" CssClass="bold" />
                    <asp:Literal runat="server" ID="litEspesor" />
                </p>
            </div>
            <p></p>
        </div>
        <div>
            <div class="divIzquierdo ancho70">
                <div class="divIzquierdo ancho50 boldElements">
                    <div class="separador">
                        <asp:Label runat="server" ID="lblFechaArmado" meta:resourcekey="lblFechaArmado" AssociatedControlID="mdpFechaArmado" />
                        <mimo:MappableDatePicker ID="mdpFechaArmado" runat="server" Style="width: 209px" />
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator ID="valFechaArmado" meta:resourcekey="valFechaArmado" runat="server" ControlToValidate="mdpFechaArmado" Display="None"></asp:RequiredFieldValidator>
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtSpool1" meta:resourcekey="txtSpool1" ReadOnly="true" CssTextBox="soloLectura" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtEtiquetaMaterial1" meta:resourcekey="txtEtiquetaMaterial1" ReadOnly="true" CssTextBox="soloLectura" />
                    </div>
                    <div class="separador">
                        <asp:Label runat="server" meta:resourcekey="lblNumeroUnico1" ID="lblNumeroUnico1" Text="Numero Unico 1:" AssociatedControlID="ddlNumeroUnico1" />
                        <asp:DropDownList runat="server" ID="ddlNumeroUnico1" />
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator runat="server" ID="reqNumUnico1" ControlToValidate="ddlNumeroUnico1" ErrorMessage="Debe especificar un número único 1" Display="None" />
                    </div>
                    <div class="separador">
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
                        <telerik:RadComboBox    ID="rcbTubero" runat="server" Width="200px" Height="150px" OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                                                EnableLoadOnDemand="true" AllowCustomText="false" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                                CausesValidation="false" OnClientItemDataBound="Sam.WebService.TuberoTablaDataBound"
                                                OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged" AutoPostBack="true"
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
                        <asp:CustomValidator meta:resourcekey="valTubero" runat="server" ID="valTubero" Display="None" ControlToValidate="rcbTubero" ValidateEmptyText="true" ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor" OnServerValidate="cusRcbTubero_ServerValidate" />
                    </div>                
                    <p></p>
                </div>
                <div class="divIzquierdo boldElements">
                    <div class="separador">
                        <asp:Label runat="server" ID="lblFechaReporte" meta:resourcekey="lblFechaReporte" AssociatedControlID="mdpFechaReporte" />
                        <mimo:MappableDatePicker ID="mdpFechaReporte" runat="server" Style="width: 209px" />
                        <asp:RequiredFieldValidator ID="valFechaReporte" meta:resourcekey="valFechaReporte" runat="server" ControlToValidate="mdpFechaReporte" Display="None" />
                        <span class="required">*</span>
                    </div>
                    <div class="separador">
                        <asp:Label CssClass="labelHack bold" runat="server" ID="lblSpool2" meta:resourcekey="lblSpool2"></asp:Label>
                        <telerik:RadComboBox ID="radCmbSpool2" runat="server" Height="150px" EnableLoadOnDemand="true"
                                             ShowMoreResultsBox="true" EnableVirtualScrolling="true" OnClientItemsRequesting="Sam.WebService.AgregaDatosParaArmadoCampo"
                                             OnSelectedIndexChanged="radCmbSpool_OnSelectedIndexChanged"
                                             AutoPostBack="true" meta:resourcekey="radCmbSpool2" CausesValidation="false" Width="200">  
                            <WebServiceSettings Method="ObtenerSpoolsCandidatosParaArmadoCampo" Path="~/WebServices/ComboboxWebService.asmx" />
                        </telerik:RadComboBox>
                        <span class="required">*</span>
                        <asp:CustomValidator meta:resourcekey="valSpool2" runat="server" ID="cusSpool2" Display="None" ControlToValidate="radCmbSpool2" ValidateEmptyText="true" ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor" OnServerValidate="cusSpool2_ServerValidate" />
                    </div>
                    <div class="separador">
                        <asp:Label runat="server" meta:resourcekey="lblEtiquetaMaterial2" ID="lblEtiquetaMaterial2" CssClass="labelHack bold" />
                        <asp:DropDownList ID="ddlEtiquetaMaterial" runat="server" OnSelectedIndexChanged="ddlEtiquetaMaterialSelectedIndexChanged" AutoPostBack="true" />
                        <asp:RequiredFieldValidator ID="valEtiquetaMaterial2" meta:resourcekey="valEtiquetaMaterial2" runat="server" ControlToValidate="ddlEtiquetaMaterial" Display="None" />
                        <span class="required">*</span>
                    </div>
                    <div class="separador">
                        <asp:Label runat="server" ID="lblNumeroUnico2" meta:resourcekey="lblNumeroUnico2" AssociatedControlID="ddlNumeroUnico2" />
                        <asp:DropDownList runat="server" ID="ddlNumeroUnico2" />
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator runat="server" ID="reqNumUnico2" ControlToValidate="ddlNumeroUnico2" ErrorMessage="Debe especificar un número único 2" Display="None" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones" TextMode="MultiLine" Rows="3" MaxLength="500" />
                    </div>
                    <p></p>
                </div>
                <input type="hidden" runat="server" id="hdnProyectoID" />
                <input type="hidden" runat="server" id="hdnSpool1" />
                <input type="hidden" runat="server" id="hdnEtiquetaMaterial1" />
                <input type="hidden" runat="server" id="hdnEtiquetaMaterial2" />
                <p></p>
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro" style="margin-top: 20px;">
                    <div class="validacionesHeader">
                        &nbsp;
                    </div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server"  ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" Width="120" />
                    </div>
                </div>
                <p></p>
            </div>
            <p></p>
        </div>
        <div class="separador" style="margin-top:20px;">
            <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" CssClass="boton" OnClick="btnGuardar_OnClick" CausesValidation="true" />
            <asp:Button ID="btnEliminar" runat="server" meta:resourcekey="btnEliminar" CssClass="boton" OnClick="btnEliminar_OnClick" CausesValidation="false" />
            <p></p>
        </div>
    </asp:Panel>
</asp:Content>
