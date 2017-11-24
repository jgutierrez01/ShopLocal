<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/PopupJuntasCampo.Master"
    AutoEventWireup="true" CodeBehind="Soldadura.aspx.cs" Inherits="SAM.Web.Produccion.JuntasCampo.Soldadura" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
<telerik:RadWindow runat="server" ID="rdwCambiarFechaProcesoAnterior">
    <ContentTemplate>
        <div style="margin-left: 30px; margin-top: 10px">
            <asp:HiddenField runat="server" ID="hdnCambiaFechas"/>
                <div class="divIzquierdo ancho50 boldElements">
                    
                    <asp:Label ID="lblEncabezadoFechaProcesoAnterior" runat="server" meta:resourcekey="lblEncabezadoFechaProcesoAnterior"/>
                    <asp:Label ID="lblFechaProcesoAnterior" runat="server" />        
                    <p></p>
                    <div class="separador">
                        <asp:Label ID="lblNuevaFecha" runat="server" meta:resourcekey="lblNuevaFecha"/>
                        <br />
                        <mimo:MappableDatePicker ID="mdpFechaProcesoAnterior" runat="server" Style="width: 209px" />
                        <span class="required">*</span>
                    </div>
                    <p></p>
                </div>
                <div class="divDerecho ancho50 boldElements">
                    <asp:Label ID="lblEncabezadoFechaReporteProcesoAnterior" runat="server" meta:resourcekey="lblEncabezadoFechaReporteProcesoAnterior"/>
                    <asp:Label ID="lblFechaReporteProcesoAnterior" runat="server" />        
                    <p></p>
                    <div class="separador">
                        <asp:Label ID="lblNuevaFechaReporte" runat="server" meta:resourcekey="lblNuevaFechaReporte"/>
                        <br />
                        <mimo:MappableDatePicker ID="mdpFechaReporteProcesoAnterior" runat="server" Style="width: 209px" />
                        <span class="required">*</span>
                    </div>
                    <p></p>
                </div>
            <p>
                <asp:Button runat="server" ID="btnGuardarPopUp" meta:resourcekey="btnGuardarPopUp" CssClass="boton" OnClick="btnGuardarPopUp_OnClick" />
            </p>   
        </div>     
    </ContentTemplate>
</telerik:RadWindow>
    <div style="width: 735;">
        <h4>
            <asp:Literal meta:resourcekey="lblSoldadura" runat="server" ID="lblSoldadura" />
        </h4>
        <div class="cajaAzul">
            <div class="divIzquierdo ancho50">
                <p>
                    <asp:Label runat="server" ID="lblSpool" meta:resourcekey="lblSpool" CssClass="bold"></asp:Label>
                    <asp:Literal runat="server" ID="litSpool"></asp:Literal>
                </p>
                <p>
                    <asp:Label runat="server" ID="lblJunta" meta:resourcekey="lblJunta" CssClass="bold"></asp:Label>
                    <asp:Literal runat="server" ID="litJunta"></asp:Literal>
                </p>
                <p>
                    <asp:Label runat="server" ID="lblTipoJunta" meta:resourcekey="lblTipoJunta" CssClass="bold"></asp:Label>
                    <asp:Literal runat="server" ID="litTipoJunta"></asp:Literal>
                </p>
            </div>
            <div class="divIzquierdo">
                <p>
                    <asp:Label runat="server" ID="lblNumeroControl" meta:resourcekey="lblNumeroControl"
                        CssClass="bold"></asp:Label>
                    <asp:Literal runat="server" ID="litNumeroControl"></asp:Literal>
                </p>
                <p>
                    <asp:Label runat="server" ID="lblLocalizacion" meta:resourcekey="lblLocalizacion"
                        CssClass="bold"></asp:Label>
                    <asp:Literal runat="server" ID="litLocalizacion"></asp:Literal>
                </p>
                <p>
                    <asp:Label runat="server" ID="lblEspesor" meta:resourcekey="lblEspesor" CssClass="bold"></asp:Label>
                    <asp:Literal runat="server" ID="litEspesor"></asp:Literal>
                </p>
            </div>
            <p>
            </p>
        </div>
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
            </telerik:RadAjaxLoadingPanel>

            <telerik:RadAjaxPanel ID="pnlControles" LoadingPanelID="ldPanel">
        <div class="divIzquierdo ancho70">
        
            <div class="divIzquierdo ancho50 boldElements">
                <div class="separador">
                    <asp:Label runat="server" ID="lblFechaSoldadura" meta:resourcekey="lblFechaSoldadura" />
                    <br />
                    <mimo:MappableDatePicker ID="mdpFechaSoldadura" runat="server" Style="width: 209px" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaSoldadura" meta:resourcekey="valFechaSoldadura"
                        runat="server" ControlToValidate="mdpFechaSoldadura" Display="None"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" meta:resourcekey="valFechaSoldadura"
                        runat="server" ControlToValidate="mdpFechaSoldadura" Display="None" ValidationGroup="soldadores"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtSpool1" meta:resourcekey="txtSpool1" ReadOnly="true" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtFamiliaAcero1" meta:resourcekey="txtFamiliaAcero1"
                        ReadOnly="true" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblProcesoRaiz" meta:resourcekey="lblProcesoRaiz" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlProcesoRaiz" OnSelectedIndexChanged="ddlProcesoRaiz_SelectedIndexChanged"
                        AutoPostBack="true" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="valProcesoRaiz" Display="None" InitialValue=""
                        meta:resourcekey="valProcesoRaiz" ControlToValidate="ddlProcesoRaiz" />
                </div>
                <asp:Panel ID="pnlChecks" runat="server">
                    <div class="separador">
                        <asp:CheckBox runat="server" ID="chkWpsDiferentes" Checked="false" OnCheckedChanged="chkWpsDiferentes_OnCheckedChanged"
                            AutoPostBack="true" CssClass="divIzquierdo" />
                        <asp:Label ID="lblChkDiferentes" meta:resourcekey="lblChkDiferentes" runat="server"
                            CssClass="divIzquierdo" />
                        <br />
                    </div>
                </asp:Panel>
                <div class="separador">
                    <asp:Label runat="server" ID="lblWpsRaiz" CssClass="bold" meta:resourcekey="lblWpsRaiz" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlWpsRaiz" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="valWpsRaiz" Display="None" InitialValue=""
                        meta:resourcekey="valWpsRaiz" ControlToValidate="ddlWpsRaiz" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" Display="None"
                        InitialValue="" meta:resourcekey="valWpsRaiz" ControlToValidate="ddlWpsRaiz"
                        ValidationGroup="soldadores" />
                </div>
            </div>
            <div class="divDerecho ancho50 boldElements">
                <div class="separador">
                    <asp:Label runat="server" ID="lblFechaReporte" meta:resourcekey="lblFechaReporte" />
                    <br />
                    <mimo:MappableDatePicker ID="mdpFechaReporte" runat="server" Style="width: 209px" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaReporte" meta:resourcekey="valFechaReporte"
                        runat="server" ControlToValidate="mdpFechaReporte" Display="None"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtSpool2" meta:resourcekey="txtSpool2" ReadOnly="true" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtFamiliaAcero2" meta:resourcekey="txtFamiliaAcero2"
                        ReadOnly="true" />
                </div>
                <div class="separador">
                    <asp:Label runat="server" ID="lblProcesoRelleno" meta:resourcekey="lblProcesoRelleno" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlProcesoRelleno" OnSelectedIndexChanged="ddlProcesoRelleno_SelectedIndexChanged"
                        AutoPostBack="true" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="valProcesoRelleno" Display="None"
                        InitialValue="" meta:resourcekey="valProcesoRelleno" ControlToValidate="ddlProcesoRelleno" />
                </div>
                <asp:Panel ID="pnlChecks2" runat="server">
                    <div class="separador">
                        <br />
                    </div>
                </asp:Panel>
                <div class="separador">
                    <asp:Label runat="server" ID="lblWpsRelleno" CssClass="bold" meta:resourcekey="lblWpsRelleno" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlWpsRelleno" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="valWpsRelleno" Display="None" InitialValue=""
                        meta:resourcekey="valWpsRelleno" ControlToValidate="ddlWpsRelleno" />
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" Display="None"
                        InitialValue="" meta:resourcekey="valWpsRelleno" ControlToValidate="ddlWpsRelleno"
                        ValidationGroup="soldadores" />
                </div>
                <p>
                </p>
            </div>
        </div>
        <div class="divDerecho ancho30">
            <div class="validacionesRecuadro" style="margin-top: 20px;">
                <div class="validacionesHeader">
                    &nbsp;
                </div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary"
                        Width="120" />
                    <asp:ValidationSummary runat="server" ID="valSummarySoldadores" ValidationGroup="soldadores"
                        CssClass="summary" meta:resourcekey="valSummarySoldadores" Width="120" />
                </div>
            </div>
        </div>
        <div>
            <p>
            </p>
            <asp:Panel ID="pnlSoldador" runat="server">
                <div class="divIzquierdo ancho70">
                    <div class="divIzquierdo ancho50">
                        <div class="separador">
                            <asp:Label runat="server" ID="txtCodigoSoldador" meta:resourcekey="txtCodigoSoldador"
                                CssClass="bold" />
                            <br />
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
                            <telerik:RadComboBox ID="rcbSoldador" runat="server" Width="200px" Height="150px"
                                OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" EnableLoadOnDemand="true"
                                ShowMoreResultsBox="true" EnableVirtualScrolling="true" CausesValidation="false"
                                OnClientItemDataBound="Sam.WebService.SoldadorTablaDataBound" OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                                DropDownCssClass="liGenerico" DropDownWidth="400px">
                                <WebServiceSettings Method="ListaSoldadoresPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
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
                            <asp:CustomValidator meta:resourcekey="valSoldador" runat="server" ID="valSoldador"
                                Display="None" ControlToValidate="rcbSoldador" ValidateEmptyText="true" ValidationGroup="soldadores"
                                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor" OnServerValidate="cusRcbSoldador_ServerValidate" />
                        </div>
                    </div>
                    <div class="divIzquierdo ancho50">
                        <div class="separador">
                            <asp:Label runat="server" ID="lblColada" meta:resourcekey="lblColada" CssClass="bold" />
                            <br />
                            <telerik:RadComboBox ID="ddlConsumibles" runat="server" Width="200px" Height="150px"
                                EmptyMessage=" " EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                OnClientItemsRequesting="Sam.Workstatus.SoldaduraConsumibles" Enabled="true"
                                ValidationGroup="valRaiz">
                                <WebServiceSettings Method="ListaConsumiblesPorPatio" Path="~/WebServices/ComboboxWebService.asmx" />
                            </telerik:RadComboBox>
                            <span class="required">*</span>
                            <asp:CustomValidator meta:resourcekey="valColada" runat="server" ID="valColada" Display="None"
                                ControlToValidate="ddlConsumibles" ValidateEmptyText="true" ValidationGroup="soldadores"
                                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor" OnServerValidate="cusRcbConsumibles_ServerValidate" />
                        </div>
                    </div>
                </div>
                <div class="divIzquierdo ancho30">
                    <div class="separador">
                        <asp:Label runat="server" ID="lblProceso" CssClass="bold" meta:resourcekey="lblProceso" />
                        <br />
                        <mimo:MappableDropDown runat="server" ID="ddlProceso" />
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator runat="server" ID="valProceso" Display="None" InitialValue="-1"
                            ValidationGroup="soldadores" meta:resourcekey="valProceso" ControlToValidate="ddlProceso" />
                    </div>
                    <div class="separador">
                        <asp:Button runat="server" ID="btnAgregar" meta:resourcekey="btnAgregar" CssClass="boton"
                            OnClick="btnAgregar_OnClick" ValidationGroup="soldadores" />
                    </div>
                </div>
                <p>
                </p>
            </asp:Panel>
            <div>
                <asp:Repeater ID="grdSoldadores" runat="server" OnItemCommand="grdSoldadores_ItemCommand"
                    Visible="false">
                    <HeaderTemplate>
                        <table class="repSam" cellpadding="0" cellspacing="0" width="100%">
                            <thead>
                                <tr class="repEncabezado">
                                    <th colspan="5">
                                        &nbsp;
                                    </th>
                                </tr>
                                <tr class="repTitulos">
                                    <th class="accion">
                                        &nbsp;
                                    </th>
                                    <th>
                                        <asp:Literal ID="litCodigoSoldador" runat="server" meta:resourcekey="litCodigoSoldador"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="litNombreSoldador" runat="server" meta:resourcekey="litNombreSoldador"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="litConsumible" runat="server" meta:resourcekey="litConsumible"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="litProceso" runat="server" meta:resourcekey="litProceso"></asp:Literal>
                                    </th>
                                </tr>
                            </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="repFila">
                            <td class="accion">
                                <asp:LinkButton CommandName='<%#Eval("TipoProceso") %>' ID="lnkBorrar" runat="server"
                                    CommandArgument='<%#Eval("SoldadorID") %>' OnClientClick="return Sam.Confirma(1);"
                                    CausesValidation="false">
                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("CodigoSoldador")%>
                            </td>
                            <td>
                                <%# Eval("NombreCompleto")%>
                            </td>
                            <td>
                                <%# Eval("CodigoConsumible")%>
                            </td>
                            <td>
                                <%# Eval("Proceso")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <asp:LinkButton CommandName='<%#Eval("TipoProceso") %>' ID="lnkBorrar" runat="server"
                                    CommandArgument='<%#Eval("SoldadorID") %>' OnClientClick="return Sam.Confirma(1);"
                                    CausesValidation="false">
                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("CodigoSoldador")%>
                            </td>
                            <td>
                                <%# Eval("NombreCompleto")%>
                            </td>
                            <td>
                                <%# Eval("CodigoConsumible")%>
                            </td>
                            <td>
                                <%# Eval("Proceso")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        <tfoot>
                            <tr class="repPie">
                                <td colspan="5">
                                    &nbsp;
                                </td>
                            </tr>
                        </tfoot>
                        </table></FooterTemplate>
                </asp:Repeater>
                <asp:Repeater ID="repSoldadoresReadOnly" runat="server" OnItemCommand="grdSoldadores_ItemCommand"
                    Visible="false">
                    <HeaderTemplate>
                        <table class="repSam" cellpadding="0" cellspacing="0" width="100%">
                            <thead>
                                <tr class="repEncabezado">
                                    <th colspan="4">
                                        &nbsp;
                                    </th>
                                </tr>
                                <tr class="repTitulos">
                                    <th>
                                        <asp:Literal ID="litCodigoSoldador" runat="server" meta:resourcekey="litCodigoSoldador"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="litNombreSoldador" runat="server" meta:resourcekey="litNombreSoldador"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="litConsumible" runat="server" meta:resourcekey="litConsumible"></asp:Literal>
                                    </th>
                                    <th>
                                        <asp:Literal ID="litProceso" runat="server" meta:resourcekey="litProceso"></asp:Literal>
                                    </th>
                                </tr>
                            </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr class="repFila">
                            <td>
                                <%# Eval("CodigoSoldador")%>
                            </td>
                            <td>
                                <%# Eval("NombreCompleto")%>
                            </td>
                            <td>
                                <%# Eval("CodigoConsumible")%>
                            </td>
                            <td>
                                <%# Eval("Proceso")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <%# Eval("CodigoSoldador")%>
                            </td>
                            <td>
                                <%# Eval("NombreCompleto")%>
                            </td>
                            <td>
                                <%# Eval("CodigoConsumible")%>
                            </td>
                            <td>
                                <%# Eval("Proceso")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <FooterTemplate>
                        <tfoot>
                            <tr class="repPie">
                                <td colspan="4">
                                    &nbsp;
                                </td>
                            </tr>
                        </tfoot>
                        </table></FooterTemplate>
                </asp:Repeater>
            </div>
            <p>
            </p>
        </div>
        <div class="divIzquierdo ancho70">
            <div class="divIzquierdo ancho50">
                <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                    TextMode="MultiLine" Rows="3" MaxLength="500">
                </mimo:LabeledTextBox>
            </div>
            <div class="divIzquierdo ancho50">
                <p>
                </p>
                <br />
                <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" CssClass="boton" />
                <asp:Button ID="btnEliminar" runat="server" meta:resourcekey="btnEliminar" CssClass="boton"
                    OnClick="btnEliminar_Clici" Visible="false" />
            </div>
            <input type="hidden" runat="server" id="hdnProyectoID" />
            <input type="hidden" runat="server" id="hdnPatioID" />
            <p>
            </p>
        </div>
        <p>
        </p>
        </telerik:RadAjaxPanel>
    </div>
</asp:Content>
