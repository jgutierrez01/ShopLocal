<%@ Page Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopupArmado.aspx.cs" Inherits="SAM.Web.WorkStatus.PopupArmado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rcbNumeroUnico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbNumeroUnico" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbNumeroUnico2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbNumeroUnico2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rcbTubero">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rcbTubero" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlNumUnico1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlNumUnico1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlNumUnico2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlNumUnico2" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <%--<telerik:AjaxSetting AjaxControlID="chbNumUnico1Pendiente">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="chbNumUnico1Pendiente" />
                    <telerik:AjaxUpdatedControl ControlID="rcbNumeroUnico" />
                    <telerik:AjaxUpdatedControl ControlID="ddlNumUnico1" />
                    <telerik:AjaxUpdatedControl ControlID="NumeroUnico1Normal" />
                    <telerik:AjaxUpdatedControl ControlID="phControlesAccesorio" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chbNumUnico2Pendiente">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="chbNumUnico2Pendiente" />
                    <telerik:AjaxUpdatedControl ControlID="rcbNumeroUnico2" />
                    <telerik:AjaxUpdatedControl ControlID="ddlNumUnico2" />
                    <telerik:AjaxUpdatedControl ControlID="numeroUnico2Normal" />
                    <telerik:AjaxUpdatedControl ControlID="placeNumeroUnico2" />
                </UpdatedControls>
            </telerik:AjaxSetting>--%>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:HiddenField ID="hdnJuntaSpoolID" runat="server" ClientIDMode="Static" />
    <h4>
        <asp:Literal meta:resourcekey="lblArmado" runat="server" ID="lblArmado" />
    </h4>
    <asp:PlaceHolder ID="phControles" runat="server">
        <div class="contenedorCentral">
            <input type="hidden" runat="server" id="hdnProyectoID" />
            <asp:HiddenField runat="server" ID="hdnTextNU1Selected" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnNU1Selected" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnTextNU2Selected" ClientIDMode="Static" />
            <asp:HiddenField runat="server" ID="hdnNU2Selected" ClientIDMode="Static" />
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
                            <asp:Label runat="server" ID="lblNumUnico1" meta:resourcekey="lblNumUnico1"/>
                            <br />
                            <asp:DropDownList runat="server" ID="ddlNumUnico1" Visible="true" />

                            <telerik:RadComboBox ID="rcbNumeroUnico" 
                                AutoPostBack="true"
                                runat="server"
                                Width="200px"
                                Height="150px"
                                OnClientItemsRequesting="Sam.WebService.ComboOnNumeroUnicoDesdeArmadoRequested"
                                OnClientSelectedIndexChanged="Sam.WebService.ComboOnNumeroUnico1ArmadoIndexChanged"
                                OnClientItemDataBound="Sam.WebService.ComboOnNumeroUnicoDespachoAccesorioItemDataBound"
                                OnClientDropDownClosed="Sam.WebService.ComboOnNumeroUnico1DropClosed"
                                EnableLoadOnDemand="true"
                                ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true"
                                CssClass="required"
                                AllowCustomText="false"
                                IsCaseSensitive="false"
                                DropDownCssClass="liDespacho"
                                DropDownWidth="500px"
                                Visible="false" >
                                <HeaderTemplate>
                                    <table cellspacing="0" cellpadding="0" class="headerRcbNu">
                                        <tr>
                                            <th class="cod">
                                                <asp:Literal runat="server" ID="litCod" meta:resourcekey="litCod" /></th>
                                            <th class="inv">
                                                <asp:Literal runat="server" ID="litInv" meta:resourcekey="litInv" /></th>
                                            <th class="diam">
                                                <asp:Literal runat="server" ID="litD1" meta:resourcekey="litD1" /></th>
                                            <th class="diam">
                                                <asp:Literal runat="server" ID="litD2" meta:resourcekey="litD2" /></th>
                                            <th class="ind">&nbsp;</th>
                                            <th class="codIc">
                                                <asp:Literal runat="server" ID="litIcCod" meta:resourcekey="litIcCod" /></th>
                                            <th class="last">
                                                <asp:Literal runat="server" ID="litDesc" meta:resourcekey="litDesc" /></th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <WebServiceSettings Path="~/Webservices/ComboboxWebService.asmx" Method="NumerosUnicosParaDespachoDesdeArmado" />
                            </telerik:RadComboBox>
                            <span class="required" runat="server">*</span>
                            <%--<asp:CustomValidator runat="server"
                                ID="cusCombo"
                                ControlToValidate="rcbNumeroUnico"
                                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                                OnServerValidate="cusCombo_ServerValidate"
                                ValidateEmptyText="true"
                                Display="None"
                                meta:resourcekey="cusCombo" />--%>
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
                    <div class="listaCheck checkBold">
                        <mimo:MappableCheckBox runat="server" ID="chbNumUnico1Pendiente" AutoPostBack="true" CausesValidation="false"
                            OnCheckedChanged="chbNumUnico1Pendiente_CheckedChanged" meta:resourcekey="chbNumUnico1Pendiente" />
                    </div>
                    <div class="separador" style="margin-top: 15px">
                        <asp:Label runat="server" ID="lblTaller" meta:resourcekey="lblTaller" />
                        <br />
                        <asp:DropDownList runat="server" ID="ddlTaller" />
                        <span class="required">*</span>
                        <%--<asp:RequiredFieldValidator runat="server" ID="rvTaller" ControlToValidate="ddlTaller"
                            InitialValue="" meta:resourcekey="rvTaller" Display="None" />--%>
                    </div>
                    <br />
                    <asp:Button meta:resourcekey="btnArmar" runat="server" ID="btnArmar" Text="Armar"
                        CssClass="boton" OnClick="btnArmar_OnClick" />
                    <asp:Button ID="btnGuardarEdicion" runat="server" meta:resourcekey="btnGuardarEdicion" CssClass="boton" OnClick="btnGuardarEdicion_Click"
                        Visible="false" />
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
                            <asp:DropDownList runat="server" ID="ddlNumUnico2" Visible="true"/>

                            <telerik:RadComboBox ID="rcbNumeroUnico2" 
                                AutoPostBack="true"
                                runat="server"
                                Width="200px"
                                Height="150px"
                                OnClientItemsRequesting="Sam.WebService.ComboOnNumeroUnicoDesdeArmadoRequested"
                                OnClientSelectedIndexChanged="Sam.WebService.ComboOnNumeroUnico2ArmadoIndexChanged"
                                OnClientItemDataBound="Sam.WebService.ComboOnNumeroUnicoDespachoAccesorioItemDataBound"
                                OnClientDropDownClosed="Sam.WebService.ComboOnNumeroUnico2DropClosed" 
                                EnableLoadOnDemand="true"
                                ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true"
                                CssClass="required"
                                AllowCustomText="false"
                                IsCaseSensitive="false"
                                DropDownCssClass="liDespacho"
                                DropDownWidth="500px"
                                Visible="false" >
                                <HeaderTemplate>
                                    <table cellspacing="0" cellpadding="0" class="headerRcbNu">
                                        <tr>
                                            <th class="cod">
                                                <asp:Literal runat="server" ID="litCod" meta:resourcekey="litCod" /></th>
                                            <th class="inv">
                                                <asp:Literal runat="server" ID="litInv" meta:resourcekey="litInv" /></th>
                                            <th class="diam">
                                                <asp:Literal runat="server" ID="litD1" meta:resourcekey="litD1" /></th>
                                            <th class="diam">
                                                <asp:Literal runat="server" ID="litD2" meta:resourcekey="litD2" /></th>
                                            <th class="ind">&nbsp;</th>
                                            <th class="codIc">
                                                <asp:Literal runat="server" ID="litIcCod" meta:resourcekey="litIcCod" /></th>
                                            <th class="last">
                                                <asp:Literal runat="server" ID="litDesc" meta:resourcekey="litDesc" /></th>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <WebServiceSettings Path="~/Webservices/ComboboxWebService.asmx" Method="NumerosUnicosParaDespachoDesdeArmadoNumUnico2" />
                            </telerik:RadComboBox>
                            <span class="required">*</span>
                            <%--<asp:CustomValidator runat="server"
                                ID="ValidaNumUnico2"
                                ControlToValidate="rcbNumeroUnico2"
                                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                                OnServerValidate="ValidaNumUnico2_ServerValidate"
                                ValidateEmptyText="true"
                                Display="None"
                                meta:resourcekey="cusCombo" />--%>
                            <div class="oculto" id="Div1">
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
                    <div class="listaCheck checkBold">
                        <mimo:MappableCheckBox runat="server" ID="chbNumUnico2Pendiente" AutoPostBack="true" CausesValidation="false"
                            OnCheckedChanged="chbNumUnico2Pendiente_CheckedChanged" meta:resourcekey="chbNumUnico2Pendiente" />
                    </div>
                    <div class="separador" style="margin-top: 15px">
                        <asp:Label runat="server" ID="lblTubero" meta:resourcekey="lblTubero" />
                        <div id="templateItemCode" class="sys-template">
                            <table class="rcbGenerico" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="codigo">{{Codigo}}
                                    </td>
                                    <td>{{NombreCompleto}}
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
                        <%--<asp:CustomValidator
                            meta:resourcekey="valTubero"
                            runat="server"
                            ID="valTubero"
                            Display="None"
                            ControlToValidate="rcbTubero"
                            ValidateEmptyText="true"
                            ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                            OnServerValidate="cusRcbTubero_ServerValidate" />--%>

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
                        &nbsp;
                    </div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary"
                            Width="120" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phMensaje" runat="server" Visible="false">
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
                    <asp:Literal runat="server" ID="litMensajeExitoPt1" meta:resourcekey="litMensajeExitoPt1" />
                </td>
            </tr>
            <%--<tr>
                <td>
                    &nbsp;
                </td>
                <td class="ligas">
                    <div class="cuadroLigas">
                        <ul>
                            <li>
                                <asp:HyperLink runat="server" ID="hlReporteOdt" meta:resourcekey="hlReporteOdt" />
                            </li>
                        </ul>
                    </div>
                </td>
            </tr>--%>
        </table>
    </asp:PlaceHolder>
</asp:Content>
