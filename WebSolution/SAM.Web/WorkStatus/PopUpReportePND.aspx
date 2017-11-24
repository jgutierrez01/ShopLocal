<%@ Page Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpReportePND.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpReportePND" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlResultado">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlDefecto" />
                    <telerik:AjaxUpdatedControl ControlID="pnSeguimientoJuntas" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="pnlDefecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnDefectosSector" />
                    <telerik:AjaxUpdatedControl ControlID="pnDefectosCuadrante" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="pnDefectosSector">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="repDefectoSector" />
                    <telerik:AjaxUpdatedControl ControlID="pnDefectosSector" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="pnDefectosCuadrante">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="repCuadrante" />
                    <telerik:AjaxUpdatedControl ControlID="pnDefectosCuadrante" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <h4>
        <asp:Literal runat="server" ID="lblReporte" meta:resourcekey="lblReporte" />
    </h4>
    <asp:PlaceHolder ID="phControles" runat="server">
        <div>
            <asp:HiddenField ID="hdnProyectoID" runat="server" ClientIDMode="Static" />
            <%--<asp:HiddenField ID="hdnJunta1Text" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnJunta1Selected" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnJunta2Text" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnJunta2Selected" runat="server" ClientIDMode="Static" />--%>
            <asp:HiddenField ID="hdnJuntaWorkstatusIDs" runat="server" ClientIDMode="Static" />
            <div class="divIzquierdo ancho35">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox ID="txtNumeroReporte" runat="server" meta:resourcekey="txtNumeroReporte"
                        ValidationGroup="vgGenerar" CssClass="required">
                    </mimo:RequiredLabeledTextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblFechaReporte" runat="server" meta:resourcekey="lblFechaReporte"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="mdpFechaReporte" runat="server" EnableEmbeddedSkins="false" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaReporte" runat="server" ControlToValidate="mdpFechaReporte"
                        Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valFechaReporte"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <asp:Label ID="lblFechaPrueba" runat="server" meta:resourcekey="lblFechaPrueba" CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="mdpFechaPrueba" runat="server" EnableEmbeddedSkins="false"  />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaPrueba" runat="server" ControlToValidate="mdpFechaPrueba"
                        Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valFechaPrueba"></asp:RequiredFieldValidator>
                   
                </div>
                <div class="separador">
                    <asp:Label ID="lblResultado" runat="server" meta:resourcekey="lblResultado" CssClass="bold"></asp:Label><br />
                    <asp:DropDownList ID="ddlResultado" runat="server" OnSelectedIndexChanged="ddlResultado_IndexChanged"
                        AutoPostBack="True">
                        <asp:ListItem Value="-1" Text=""></asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="lstReprobado"></asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="lstAprobado"></asp:ListItem>
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valResultado" InitialValue="-1" runat="server" ControlToValidate="ddlResultado"
                        Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valResultado"></asp:RequiredFieldValidator>
                </div>
                <asp:Panel ID="pnlDefecto" runat="server" Visible="false">
                    <div class="separador">
                        <asp:Label ID="lblTipoDefecto" runat="server" meta:resourcekey="lblTipoDefecto" CssClass="bold"></asp:Label><br />
                        <asp:DropDownList ID="ddlTipoDefecto" runat="server" OnSelectedIndexChanged="ddlTipoDefecto_SelectedIndexChanged"
                            AutoPostBack="true">
                            <asp:ListItem Value="-1" Text=""></asp:ListItem>
                            <asp:ListItem Value="1" meta:resourcekey="lstSector"></asp:ListItem>
                            <asp:ListItem Value="2" meta:resourcekey="lstCuadrante"></asp:ListItem>
                        </asp:DropDownList>
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator ID="valTipoDefecto" runat="server" ControlToValidate="ddlTipoDefecto"
                            InitialValue="-1" Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valTipoDefecto"></asp:RequiredFieldValidator>
                    </div>
                </asp:Panel>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                        TextMode="MultiLine" Rows="3" MaxLength="500">
                    </mimo:LabeledTextBox>
                </div>
                <br />
                <br />
                <div class="separador">
                     <asp:Label ID="lblJuntaReferencia" runat="server" CssClass="bold"></asp:Label>
                </div>
                <br />
                <br />
                <div class="separador">
                    <mimo:AutoDisableButton ID="btnAGenerar" runat="server" CssClass="boton" meta:resourcekey="btnGenerar"
                        OnClick="btnAGenerar_Click" ValidationGroup="vgGenerar" />
                    <%--  <asp:Button ID="btnAGenerar" runat="server" CssClass="boton" meta:resourcekey="btnGenerar"
                        OnClick="btnAGenerar_Click" ValidationGroup="vgGenerar" />--%>
                </div>
                <div class="separador">
                    <telerik:RadWindow ID="radwindowPopup" runat="server" VisibleOnPageLoad="false"
                        Width="300px" Modal="true" BackColor="#DADADA" VisibleStatusbar="false" Behaviors="None" meta:resourcekey="tituloConfirmacion">
                        <ContentTemplate>
                            <div style="padding: 20px">
                                <asp:Label ID="PopupLabel" runat="server"></asp:Label>
                                <br />
                                <br />
                                <asp:Button ID="btnOk" CssClass="boton" runat="server" meta:resourcekey="btnOk" Width="50px" OnClick="btnOk_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" CssClass="boton" runat="server" meta:resourcekey="btnCancel" Width="50px" OnClick="btnCancel_Click" />
                            </div>
                        </ContentTemplate>
                    </telerik:RadWindow>
                </div>
            </div>
            <%--"Sam.WebService.AgregaProyectoID"--%>
            <div class="divDerecho ancho65">
                <div class="divIzquierdo ancho50">
                    <asp:Panel ID="pnSeguimientoJuntas" runat="server" Visible="false">
                        <div class="separador">
                            <asp:Label ID="lblJunta1" runat="server" CssClass="bold" meta:resourcekey="lblJunta1"></asp:Label>
                            <telerik:RadComboBox ID="rcbJuntaSeg1"
                                runat="server"
                                EnableLoadOnDemand="true"
                                ShowMoreResultsBox="true"
                                AutoPostBack="true"
                                CausesValidation="false"
                                AllowCustomText="false"
                                OnClientItemsRequesting="Sam.WebService.AgregaDatosParaSeguimientoJuntasEnRT"
                                EnableVirtualScrolling="true"
                                Width="200px"
                                Height="150px">
                                <WebServiceSettings Method="ListaNumerosControlYJunta" Path="~/Webservices/ComboboxWebService.asmx"></WebServiceSettings>
                            </telerik:RadComboBox>
                        </div>
                        <div class="separador">
                            <asp:Label ID="lblJunta2" runat="server" CssClass="bold" meta:resourcekey="lblJunta2"></asp:Label>
                            <telerik:RadComboBox ID="rcbJuntaSeg2"
                                runat="server"
                                EnableLoadOnDemand="true"
                                ShowMoreResultsBox="true"
                                AutoPostBack="true"
                                CausesValidation="false"
                                AllowCustomText="false"
                                OnClientItemsRequesting="Sam.WebService.AgregaDatosParaSeguimientoJuntasEnRT"
                                EnableVirtualScrolling="true"
                                Width="200px"
                                Height="150px">
                                <WebServiceSettings Method="ListaNumerosControlYJunta" Path="~/Webservices/ComboboxWebService.asmx"></WebServiceSettings>
                            </telerik:RadComboBox>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnDefectosSector" runat="server" Visible="False">
                        <div class="separador">
                            <asp:Label ID="lblDefectoSector" runat="server" meta:resourcekey="lblDefecto" CssClass="bold"></asp:Label><br />
                            <asp:DropDownList ID="ddlDefectoSector" runat="server">
                            </asp:DropDownList>
                            <span class="required">*</span>
                            <asp:RequiredFieldValidator ID="valDefectoSector" runat="server" ControlToValidate="ddlDefectoSector"
                                Display="None" ValidationGroup="vgDefectoSector" meta:resourcekey="valDefectoSector"></asp:RequiredFieldValidator>
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox ID="txtSector" runat="server" meta:resourcekey="txtSector"
                                ValidationGroup="vgDefectoSector" CssClass="required">
                            </mimo:RequiredLabeledTextBox>
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox ID="txtDeSector" runat="server" meta:resourcekey="txtDe"
                                ValidationGroup="vgDefectoSector" CssClass="required">
                            </mimo:RequiredLabeledTextBox>
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox ID="txtASector" runat="server" meta:resourcekey="txtA"
                                ValidationGroup="vgDefectoSector" CssClass="required">
                            </mimo:RequiredLabeledTextBox>
                        </div>
                        <div class="separador">
                            <asp:Button ID="btnAgregarSector" runat="server" ValidationGroup="vgDefectoSector"
                                CssClass="boton" meta:resourcekey="btnAgregarSector" OnClick="btnAgregarSector_Click" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnDefectosCuadrante" runat="server" Visible="False" meta:resourcekey="pnDefectosCuadranteResource1">
                        <div class="separador">
                            <asp:Label ID="lblDefectoCuadrante" runat="server" meta:resourcekey="lblDefecto"
                                CssClass="bold"></asp:Label><br />
                            <asp:DropDownList ID="ddlDefectoCuadrante" runat="server" meta:resourcekey="ddlDefectoCuadranteResource1">
                            </asp:DropDownList>
                            <span class="required">*</span>
                            <asp:RequiredFieldValidator ID="valDefectoCuadrante" runat="server" ControlToValidate="ddlDefectoCuadrante"
                                Display="None" ValidationGroup="vgDefectoCuadrante" meta:resourcekey="valDefectoCuadrante"></asp:RequiredFieldValidator>
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox ID="txtCuadrante" runat="server" meta:resourcekey="txtCuadrante"
                                ValidationGroup="vgDefectoCuadrante" CssClass="required">
                            </mimo:RequiredLabeledTextBox>
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox ID="txtPlaca" runat="server" meta:resourcekey="txtPlaca"
                                ValidationGroup="vgDefectoCuadrante" CssClass="required">
                            </mimo:RequiredLabeledTextBox>
                        </div>
                        <div class="separador">
                            <asp:Button ID="btnAgregarCuadrante" runat="server" ValidationGroup="vgDefectoCuadrante"
                                CssClass="boton" meta:resourcekey="btnAgregarCuadrante" OnClick="btnAgregarCuadrante_Click" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="divDerecho ancho50">
                    <div class="validacionesRecuadro">
                        <div class="validacionesHeader">
                        </div>
                        <div class="validacionesMain">
                            <div class="separador">
                                <asp:ValidationSummary runat="server" ID="summaryReporte" ValidationGroup="vgGenerar"
                                    meta:resourcekey="valReporte" CssClass="summary" />
                                <asp:ValidationSummary runat="server" ID="summarySector" ValidationGroup="vgDefectoSector"
                                    meta:resourcekey="valReporte" CssClass="summary" />
                                <asp:ValidationSummary runat="server" ID="summaryCuadrante" ValidationGroup="vgDefectoCuadrante"
                                    meta:resourcekey="valReporte" CssClass="summary" />
                            </div>
                        </div>
                    </div>
                </div>
                <p>
                </p>
                <div>
                    <asp:Repeater ID="repDefectoSector" runat="server" OnItemCommand="repDefectoSector_ItemCommand"
                        OnItemDataBound="repDefectoSector_ItemDataBound" Visible="false">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" class="repSam">
                                <thead>
                                    <tr class="repEncabezado">
                                        <th colspan="5">&nbsp;
                                        </th>
                                    </tr>
                                    <tr class="repTitulos">
                                        <th>&nbsp;
                                        </th>
                                        <th>
                                            <asp:Literal ID="litSector" runat="server" meta:resourceKey="litSector"></asp:Literal>
                                        </th>
                                        <th>
                                            <asp:Literal ID="litDe" runat="server" meta:resourceKey="litDe"></asp:Literal>
                                        </th>
                                        <th>
                                            <asp:Literal ID="litA" runat="server" meta:resourceKey="litA"></asp:Literal>
                                        </th>
                                        <th>
                                            <asp:Literal ID="litDefecto" runat="server" meta:resourceKey="litDefecto"></asp:Literal>
                                        </th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="repFila">
                                <td>
                                    <asp:LinkButton ID="lnkBorrar" runat="server" CommandArgument='<%# Eval("JuntaReportePndID") %>'
                                        CommandName="Borrar" meta:resourcekey="lnkBorrarResource2" OnClientClick="return Sam.Confirma(1);">
                                        <asp:Image runat="server" ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar"
                                            meta:resourceKey="imgBorrar"></asp:Image>
                                    </asp:LinkButton>
                                </td>
                                <td>
                                    <%# Eval("Sector") %>
                                </td>
                                <td>
                                    <%# Eval("SectorInicio") %>
                                </td>
                                <td>
                                    <%# Eval("SectorFin")%>
                                </td>
                                <td>
                                    <asp:Literal ID="litNombreDefecto" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="repFilaPar">
                                <td>
                                    <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%# Eval("JuntaReportePndID") %>'
                                        OnClientClick="return Sam.Confirma(1);" meta:resourcekey="lnkBorrarResource1">
                                        <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                            meta:resourcekey="imgBorrar" />
                                    </asp:LinkButton>
                                </td>
                                <td>
                                    <%# Eval("Sector")%>
                                </td>
                                <td>
                                    <%# Eval("SectorInicio")%>
                                </td>
                                <td>
                                    <%# Eval("SectorFin")%>
                                </td>
                                <td>
                                    <asp:Literal ID="litNombreDefecto" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            <tfoot>
                                <tr class="repPie">
                                    <td colspan="5">&nbsp;
                                    </td>
                                </tr>
                            </tfoot>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:Repeater ID="repCuadrante" runat="server" OnItemCommand="repCuadrante_ItemCommand"
                        OnItemDataBound="repCuadrante_ItemDataBound" Visible="false">
                        <HeaderTemplate>
                            <table cellpadding="0" cellspacing="0" class="repSam">
                                <thead>
                                    <tr class="repEncabezado">
                                        <th colspan="4">&nbsp;
                                        </th>
                                    </tr>
                                    <tr class="repTitulos">
                                        <th>&nbsp;
                                        </th>
                                        <th>
                                            <asp:Literal ID="litCuadrante" runat="server" meta:resourceKey="litCuadrante"></asp:Literal>
                                        </th>
                                        <th>
                                            <asp:Literal ID="litPlaca" runat="server" meta:resourceKey="litPlaca"></asp:Literal>
                                        </th>
                                        <th>
                                            <asp:Literal ID="litDefecto" runat="server" meta:resourceKey="litDefecto"></asp:Literal>
                                        </th>
                                    </tr>
                                </thead>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="repFila">
                                <td>
                                    <asp:LinkButton ID="lnkBorrar" runat="server" CommandArgument='<%# Eval("JuntaReportePndID") %>'
                                        CommandName="Borrar" meta:resourcekey="lnkBorrarResource4" OnClientClick="return Sam.Confirma(1);">
                                        <asp:Image runat="server" ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar"
                                            meta:resourceKey="imgBorrar"></asp:Image>
                                    </asp:LinkButton>
                                </td>
                                <td>
                                    <%# Eval("Cuadrante") %>
                                </td>
                                <td>
                                    <%# Eval("Placa") %>
                                </td>
                                <td>
                                    <asp:Literal ID="litNombreDefecto" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="repFilaPar">
                                <td>
                                    <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%# Eval("JuntaReportePndID") %>'
                                        OnClientClick="return Sam.Confirma(1);" meta:resourcekey="lnkBorrarResource3">
                                        <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                            meta:resourcekey="imgBorrar" />
                                    </asp:LinkButton>
                                </td>
                                <td>
                                    <%# Eval("Cuadrante")%>
                                </td>
                                <td>
                                    <%# Eval("Placa")%>
                                </td>
                                <td>
                                    <asp:Literal ID="litNombreDefecto" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            <tfoot>
                                <tr class="repPie">
                                    <td colspan="4">&nbsp;
                                    </td>
                                </tr>
                            </tfoot>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <p>
            </p>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phMensaje" Visible="false">
        <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin: 20px auto 0 auto;">
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
                    <asp:Label runat="server" ID="lblMensajeExitoTercerRechazo" meta:resourcekey="lblMensajeExitoTercerRechazo" Visible="false" />
                    <asp:Label runat="server" ID="lblMensajeExitoRechazo" meta:resourcekey="lblMensajeExitoRechazo" Visible="false" />
                    <asp:Label runat="server" ID="lblMensajeExitoReporte" meta:resourcekey="lblMensajeExitoReporte" Visible="false" />
                    <br />
                    <br />
                    <samweb:LinkVisorReportes ID="lnkReporte" runat="server" meta:resourcekey="lnkReporte" Visible="false"></samweb:LinkVisorReportes>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</asp:Content>
