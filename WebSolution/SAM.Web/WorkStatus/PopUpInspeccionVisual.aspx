<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpInspeccionVisual.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpInspeccionVisual" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
            <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlResultado">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnDefectos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <h4>
        <asp:Literal runat="server" ID="lblReporte" meta:resourcekey="lblReporte" />
    </h4>
    <asp:PlaceHolder runat="server" ID="phControles">
        <div class="divIzquierdo ancho70">
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox ID="txtNumeroReporte" runat="server" meta:resourcekey="txtNumeroReporte"
                        ValidationGroup="vgGenerar">
                    </mimo:RequiredLabeledTextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblResultado" runat="server" meta:resourcekey="lblResultado" CssClass="bold"></asp:Label><br />
                    <asp:DropDownList ID="ddlResultado" runat="server" OnSelectedIndexChanged="ddlResultado_IndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Value="-1" Text=""></asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="lstReprobado"></asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="lstAprobado"></asp:ListItem>
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valResultado" InitialValue="-1" runat="server" ControlToValidate="ddlResultado"
                        Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valResultado"></asp:RequiredFieldValidator>
                </div>

                <div class="separador">
                    <asp:Label ID="lblTaller" runat="server" CssClass="bold" meta:resourcekey="lblTaller" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlTaller" EntityPropertyName="TallerID" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlTaller_SelectedIndexChanged" ClientIDMode="Static" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvTaller" runat="server" ControlToValidate="ddlTaller"
                        InitialValue="" Display="None" meta:resourcekey="rfvTaller" />
                </div>

                <div class="separador" style="margin-top: 15px">
                        <asp:Label runat="server" ID="lblInspector" meta:resourcekey="lblInspector" CssClass="bold" />
                        <br />
                        <div id="templateItemCode" class="sys-template">
                            <table class="rcbGenerico" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="codigo">{{NumeroEmpleado}}
                                    </td>
                                    <td>{{NombreCompleto}}
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <telerik:RadComboBox ID="rcbInspector" runat="server" Width="200px" Height="150px"
                            EnableLoadOnDemand="true" 
                            ShowMoreResultsBox="true" 
                            EnableVirtualScrolling="true" 
                            AutoPostBack="true"
                            CausesValidation="false"
                            OnClientItemsRequesting="Sam.WebService.AgregaTallerID"
                            OnClientItemDataBound="Sam.WebService.InspectorTablaDataBound"
                            OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                            DropDownCssClass="liGenerico" 
                            DropDownWidth="400px">
                            <WebServiceSettings Method="ListaInspectoresPorTaller" Path="~/WebServices/ComboboxWebService.asmx" />
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
                    </div>
                    
                <div class="separador">
                    <asp:Label ID="lblFechaReporte" runat="server" meta:resourcekey="lblFechaReporte"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dpFechaReporte" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                        EnableEmbeddedSkins="false" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaReporte" runat="server" ControlToValidate="dpFechaReporte"
                        Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valFechaReporte"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <asp:Label ID="lblFechaInspeccion" runat="server" meta:resourcekey="lblFechaInspeccion"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker  ID="dpFechaInspeccion" runat="server" MinDate="01/01/1960"
                        MaxDate="01/01/2050" EnableEmbeddedSkins="false" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaInspeccion" runat="server" ControlToValidate="dpFechaInspeccion"
                        Display="None" ValidationGroup="vgGenerar" meta:resourcekey="valFechaInspeccion"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                        TextMode="MultiLine" Rows="3" MaxLength="500">
                    </mimo:LabeledTextBox>
                </div>
                <p>
                </p>
                <br />
                <div class="separador">
                    <asp:Button ID="btnGenerar" runat="server" ValidationGroup="vgGenerar" CssClass="boton"
                        meta:resourcekey="btnGenerar" OnClick="btnGenerar_Click" />
                </div>
            </div>
            <asp:Panel ID="pnDefectos" runat="server" Visible="false">
                <div class="divDerecho ancho50">
                    <div class="separador">
                        <asp:Label ID="lblDefecto" runat="server" meta:resourcekey="lblDefecto" CssClass="bold"></asp:Label><br />
                        <asp:DropDownList ID="ddlDefecto" runat="server">
                        </asp:DropDownList>
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator ID="valDefecto" InitialValue="" runat="server" ControlToValidate="ddlDefecto"
                            Display="None" ValidationGroup="vgDefecto" meta:resourcekey="valDefecto"></asp:RequiredFieldValidator>
                    </div>
                    <div class="separador">
                        <asp:Button ID="btnAgregar" runat="server" ValidationGroup="vgDefecto" CssClass="boton"
                            meta:resourcekey="btnAgregar" OnClick="btnAgregar_Click" />
                    </div>
                    <div class="separador">
                        <asp:Repeater ID="repDefectos" runat="server" OnItemCommand="repDefectos_ItemCommand">
                            <HeaderTemplate>
                                <table class="repSam" cellpadding="0" cellspacing="0">
                                    <thead>
                                        <tr class="repEncabezado">
                                            <th colspan="2">
                                                &nbsp;
                                            </th>
                                        </tr>
                                        <tr class="repTitulos">
                                            <th>
                                                &nbsp;
                                            </th>
                                            <th>
                                                <asp:Literal ID="litDefecto" runat="server" meta:resourcekey="litDefecto"></asp:Literal>
                                            </th>
                                        </tr>
                                    </thead>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="repFila">
                                    <td>
                                        <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("DefectoID") %>'
                                            OnClientClick="return Sam.Confirma(1);">
                                            <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                meta:resourcekey="imgBorrar" /></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("Nombre")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="repFilaPar">
                                    <td>
                                        <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("DefectoID") %>'
                                            OnClientClick="return Sam.Confirma(1);">
                                            <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                meta:resourcekey="imgBorrar" /></asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("Nombre") %>
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
                </div>
            </asp:Panel>
        </div>
        <div class="divDerecho ancho25">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <div class="separador">
                        <asp:ValidationSummary runat="server" ID="summaryReporte" ValidationGroup="vgGenerar"
                            meta:resourcekey="valReporte" CssClass="summary" />
                        <asp:ValidationSummary runat="server" ID="summaryDefecto" ValidationGroup="vgDefecto"
                            meta:resourcekey="valDefecto" CssClass="summary" />
                    </div>
                </div>
            </div>
        </div>
        <p></p>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phMensajeExito"  Visible="false">
        <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin: 20px auto 0 auto;">
            <tr>
                <td rowspan="3" class="icono">
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
            <tr>
                <td>
                    <br />
                    <samweb:LinkVisorReportes ID="lnkReporte" runat="server" meta:resourcekey="lnkReporte"></samweb:LinkVisorReportes>
                </td>
            </tr>
        </table>
        <p></p>
    </asp:PlaceHolder>
</asp:Content>
