<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AltaNumUnicoGeneral.ascx.cs"
    Inherits="SAM.Web.Controles.Materiales.AltaNumUnicoGeneral" %>
<div class="dashboardCentral">
    <asp:HiddenField ID="hdnProyectoID" runat="server" />
    <div class="divIzquierdo ancho70">
        <asp:PlaceHolder ID="plhRecepcion" runat="server">
            <div class="divIzquierdo ancho45">
                <p>
                    <asp:Label ID="lblFechaTitulo" runat="server" meta:resourcekey="lblFechaTitulo" CssClass="bold"></asp:Label>&nbsp;
                    <asp:Label ID="lblFecha" runat="server" meta:resourcekey="lblFechaResource1"></asp:Label>
                </p>
                <p>
                    <asp:Label ID="lblTransportistaTitulo" runat="server" meta:resourcekey="lblTransportistaTitulo"
                        CssClass="bold"></asp:Label>&nbsp;
                    <asp:Label ID="lblTranportista" runat="server" meta:resourcekey="lblTranportistaResource1"></asp:Label>
                </p>
            </div>
            <div class="divDerecho ancho50">
                <p>
                    &nbsp;
                </p>
                <p>
                    <asp:Label ID="lblRecibiendo" runat="server" meta:resourcekey="lblRecibiendo" CssClass="bold"></asp:Label>&nbsp;
                    <asp:Label ID="lblNumeroActual" runat="server" CssClass="subrayado"></asp:Label>&nbsp;
                    <asp:Label ID="lblDe" runat="server" meta:resourcekey="lblDe"></asp:Label>&nbsp;
                    <asp:Label ID="lblNumerosTotales" runat="server" CssClass="subrayado"></asp:Label>
                </p>
            </div>
            <p></p>
        </asp:PlaceHolder>              
        <div class="divIzquierdo ancho45">            
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtNumeroUnico" meta:resourcekey="txtNumeroUnico"
                    Enabled="True" MaxLength="10" ReadOnly="True">
                </mimo:LabeledTextBox>
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
                <telerik:RadComboBox ID="rcbItemCode" runat="server" Width="200px" Height="150px" OnSelectedIndexChanged="rcbItemCode_SelectedIndexChanged"
                    OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" EnableLoadOnDemand="true"
                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" CausesValidation="false" AutoPostBack="true"
                    OnClientItemDataBound="Sam.WebService.ItemCodeTablaDataBound" OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
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
                <asp:Button ID="btnAgregarItemCode" runat="server" Text="+" CausesValidation="false"
                    CssClass="boton" Height="20px" />
            </div>
            <asp:Panel ID="phItemCode" runat="server" Visible="false">
                    <div class="cajaAzul" style="margin-right: 5px;">
                        <asp:Label ID="lblDescripcionEspanol" runat="server" meta:resourcekey="lblDescripcionEspanol" CssClass="bold"></asp:Label>&nbsp;
                        <asp:Label ID="lblDescripcion" runat="server"></asp:Label>
                    </div>
                </asp:Panel>

            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtDiametro1" meta:resourcekey="txtDiametro1"
                    CssClass="required" Enabled="True" MaxLength="10" TextMode="SingleLine">
                </mimo:RequiredLabeledTextBox>
                <asp:RangeValidator ID="valDiametro1" runat="server" Type="Double" MaximumValue="999.9999"
                    MinimumValue="0.001" Display="None" meta:resourcekey="valDiametro1" ControlToValidate="txtDiametro1"></asp:RangeValidator>
                <asp:CompareValidator runat="server" ID="cmpDiametro" ControlToValidate="txtDiametro1"
                    ControlToCompare="txtDiametro2" Operator="GreaterThanEqual" Type="Double" Display="None"
                    meta:resourcekey="cmpDiametro" />
            </div>
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtDiametro2" meta:resourcekey="txtDiametro2"
                    CssClass="required" Enabled="True" MaxLength="10" TextMode="SingleLine">
                </mimo:RequiredLabeledTextBox>
                <asp:RangeValidator ID="valDiametro2" runat="server" Type="Double" MaximumValue="999.9999"
                    MinimumValue="0" Display="None" meta:resourcekey="valDiametro2" ControlToValidate="txtDiametro2"></asp:RangeValidator>
            </div>
            <telerik:RadAjaxLoadingPanel ID="loadPanel" runat="server" />
            <telerik:RadAjaxPanel ID="pnlColada" runat="server" LoadingPanelID="loadPanel">
                <div class="separador">
                    <asp:Label runat="server" ID="lblColada" meta:resourcekey="lblColada" CssClass="bold" />
                    <br />
                    <div id="coladaTemplate" class="sys-template">
                        <table class="rcbGenerico" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="colada">
                                    {{NumeroColada}}
                                </td>
                                <td class="certificado">
                                    {{Certificado}}
                                </td>
                                <td>
                                    {{Fabricante}}
                                </td>
                            </tr>
                        </table>
                    </div>
                    <telerik:RadComboBox ID="ddlColada" runat="server" Width="200px" Height="150px" EnableLoadOnDemand="true"
                        ShowMoreResultsBox="true" EnableVirtualScrolling="true" AutoPostBack="true" CausesValidation="false"
                        CssClass="required" OnSelectedIndexChanged="ddlColada_SelectedIndexChanged" OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                        OnClientItemDataBound="Sam.WebService.ColadaTablaDataBound" OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                        DropDownCssClass="liGenerico" DropDownWidth="400px">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                                <tr>
                                    <th class="colada">
                                        <asp:Literal ID="litColada" runat="server" meta:resourcekey="litColada"></asp:Literal>
                                    </th>
                                    <th class="certificado">
                                        <asp:Literal ID="litCertificado" runat="server" meta:resourcekey="litCertificado"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="litFabricante" runat="server" meta:resourcekey="litFabricante"></asp:Literal>
                                    </th>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <WebServiceSettings Method="ListaColadasPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                    </telerik:RadComboBox>
                    <span class="required">*</span>
                    <asp:CustomValidator meta:resourcekey="valColada" runat="server" ID="valColada" Display="None"
                        ControlToValidate="ddlColada" ValidateEmptyText="true" ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                        OnServerValidate="cusColdada_ServerValidate" />
                    <asp:Button ID="btnColada" runat="server" Text="+" CausesValidation="false" CssClass="boton"
                        Height="20px" />
                </div>
                <asp:Panel ID="phColada" runat="server" Visible="false">
                    <div class="cajaAzul" style="margin-right: 5px;">
                        <asp:Label ID="lblCertificadoTitulo" runat="server" meta:resourcekey="lblCertificadoTitulo"
                            CssClass="bold"></asp:Label>&nbsp;
                        <asp:Label ID="lblCertificado" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="lblEstatusTitulo" runat="server" meta:resourcekey="lblEstatusTitulo"
                            CssClass="bold"></asp:Label>&nbsp;
                        <asp:Label ID="lblEstatusColada" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="lblAceroTitulo" runat="server" meta:resourcekey="lblAceroTitulo" CssClass="bold"></asp:Label>&nbsp;
                        <asp:Label ID="lblAcero" runat="server" meta:resourcekey="lblAceroResource1"></asp:Label>
                        <br />
                        <asp:Label ID="lblAceroFamTitulo" runat="server" meta:resourcekey="lblAceroFamTitulo"
                            CssClass="bold"></asp:Label>&nbsp;
                        <asp:Label ID="lblAceroFam" runat="server" meta:resourcekey="lblAceroFamResource1"></asp:Label>
                        <br />
                        <asp:Label ID="lblMaterialFamTitulo" runat="server" meta:resourcekey="lblMaterialFamTitulo"
                            CssClass="bold"></asp:Label>&nbsp;
                        <asp:Label ID="lblMaterialFam" runat="server"></asp:Label>
                    </div>
                </asp:Panel>
            </telerik:RadAjaxPanel>
            <div class="separador">
                <asp:Label ID="lblObservaciones" runat="server" meta:resourcekey="lblObservaciones" CssClass="bold" />
                    <asp:TextBox Rows="3" Columns="50" TextMode="MultiLine" ID="txtObservaciones" runat="server" Width="84%" ></asp:TextBox>
            </div>
        </div>
        <div class="divDerecho ancho50">
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtNumeroUnicoCliente" MaxLength="50" meta:resourcekey="txtNumeroUnicoCliente" />
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblEstatus" meta:resourcekey="lblEstatus" CssClass="bold" />
                <br />
                <asp:DropDownList ID="ddlEstatus" runat="server">
                    <asp:ListItem Text="" Value="-1" />
                    <asp:ListItem meta:resourcekey="lstAprobado" Value="A" />
                    <asp:ListItem meta:resourcekey="lstRechazado" Value="R" />
                    <asp:ListItem meta:resourcekey="lstCondicionado" Value="C" />
                </asp:DropDownList>
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="reqEstatus" runat="server" Display="None" ControlToValidate="ddlEstatus"
                    meta:resourcekey="reqEstatus" InitialValue="-1" />
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblCedula" meta:resourcekey="lblCedula" CssClass="bold" />
                <br />
                <asp:DropDownList ID="ddlCedula" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCedula_OnSelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblProfile" meta:resourcekey="lblProfile" CssClass="bold" />
                <br />
                <asp:DropDownList ID="ddlProfile" runat="server">
                </asp:DropDownList>
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblProfile2" meta:resourcekey="lblProfile2" CssClass="bold" />
                <br />
                <asp:DropDownList ID="ddlProfile2" runat="server">
                </asp:DropDownList>
            </div>
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtCantidad" meta:resourcekey="txtCantidad"
                    CssClass="required" Enabled="True" MaxLength="10" TextMode="SingleLine">
                </mimo:RequiredLabeledTextBox>
                <asp:RangeValidator ID="valCantidad" runat="server" Type="Integer" MaximumValue="2147483647"
                    MinimumValue="0" Display="None" meta:resourcekey="valCantidad" ControlToValidate="txtCantidad"></asp:RangeValidator>
            </div>
            <telerik:RadAjaxLoadingPanel ID="chkLoadPanel" runat="server" />
            <telerik:RadAjaxPanel ID="pnlDanada" runat="server" LoadingPanelID="chkLoadPanel">
                <p>
                    <asp:CheckBox ID="chkDanada" runat="server" meta:resourcekey="chkDanada" CssClass="checkBold"
                        AutoPostBack="true" OnCheckedChanged="chkDanada_CheckedChanged" CausesValidation="false" />
                </p>
                <asp:PlaceHolder ID="phDanada" runat="server" Visible="false">
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox runat="server" ID="txtCantidadDanada" meta:resourcekey="txtCantidadDanada"
                            MaxLength="10">
                        </mimo:RequiredLabeledTextBox>
                        <asp:RangeValidator ID="valCantidad2" runat="server" Type="Integer" MaximumValue="2147483647"
                            MinimumValue="0" Display="None" meta:resourcekey="valCantidad2" ControlToValidate="txtCantidadDanada"></asp:RangeValidator>
                    </div>
                </asp:PlaceHolder>
            </telerik:RadAjaxPanel>
           
            
        </div>
    </div>
    <div class="divDerecho ancho30">
        <div class="validacionesRecuadro">
            <div class="validacionesHeader">
                &nbsp;</div>
            <div class="validacionesMain">
                <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summary"
                    meta:resourcekey="valSummary"  />
            </div>
        </div>
        <br />
        <asp:Panel runat="server" Visible="false" CssClass="cajaAzul" ID="pnlInfoCantidades"> 
                <asp:Label  runat="server" ID="lblCantidadDespachadaTitulo" meta:resourcekey="lblCantidadDespachadaTitulo" CssClass="bold">
                </asp:Label>
                <asp:Label  runat="server" ID="lblCantidadDespachada"
                    MaxLength="10" TextMode="SingleLine">
                </asp:Label>
                <br />
                <asp:Label runat="server" ID="lblCantidadCongeladaTitulo" meta:resourcekey="lblCantidadCongeladaTitulo" CssClass="bold">
                </asp:Label>
                <asp:Label  runat="server" ID="lblCantidadCongelada" 
                    MaxLength="10" TextMode="SingleLine">
                </asp:Label>
                <br />
                <asp:Label runat="server" ID="lblCantidadOtrasSalidasTitulo" meta:resourcekey="lblCantidadOtrasSalidasTitulo" CssClass="bold">
                </asp:Label>
                <asp:Label  runat="server" ID="lblCantidadOtrasSalidas" 
                    MaxLength="10" TextMode="SingleLine">
                </asp:Label>
                <br />
                <asp:Label runat="server" ID="Label1" meta:resourcekey="lblMermasCorteTitulo" CssClass="bold">
                </asp:Label>
                <asp:Label  runat="server" ID="lblMermasCorte" 
                    MaxLength="10" TextMode="SingleLine">
                </asp:Label>
                <br />
                <asp:Label runat="server" ID="lblCantidadSalidasTemporalesTitulo" meta:resourcekey="lblCantidadSalidasTemporalesTitulo" CssClass="bold">
                </asp:Label>
                <asp:Label  runat="server" ID="lblCantidadSalidasTemporales" 
                    MaxLength="10" TextMode="SingleLine">
                </asp:Label>
                <p />
            </asp:Panel>
           <p></p>
    </div>
</div>
<asp:CustomValidator
ID="cusPreSaveValidation"
runat="server"
OnServerValidate="cusPreSaveValidation_OnServerValidate"
Display="None"
meta:resourcekey="cusPreSaveValidation"
/>
