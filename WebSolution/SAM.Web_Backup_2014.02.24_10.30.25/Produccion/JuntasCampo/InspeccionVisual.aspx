<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/PopupJuntasCampo.Master"
    AutoEventWireup="true" CodeBehind="InspeccionVisual.aspx.cs" Inherits="SAM.Web.Produccion.JuntasCampo.InspeccionVisual" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
<telerik:RadWindow runat="server" ID="rdwCambiarFechaProcesoAnterior">
    <ContentTemplate>
        <div style="margin-left: 30px; margin-top: 10px">
            <asp:HiddenField runat="server" ID="hdnCambiaFechas"/>
            <asp:HiddenField runat="server" ID="hdnProcesoAnterior2"/>
            <asp:HiddenField runat="server" ID="hdnProcesoReporteAnterior2"/>
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
            <asp:Literal meta:resourcekey="lblInspeccionVisual" runat="server" ID="lblInspeccionVisual" />
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
        <div class="divIzquierdo ancho70">
            <div class="divIzquierdo ancho50 boldElements">
                <div class="separador">
                    <asp:Label runat="server" ID="lblFechaInspeccionVisual" meta:resourcekey="lblFechaInspeccionVisual" />
                    <br />
                    <mimo:MappableDatePicker ID="mdpFechaInspeccionVisual" runat="server" Style="width: 209px" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaInspeccionVisual" meta:resourcekey="valFechaInspeccionVisual"
                        runat="server" ControlToValidate="mdpFechaInspeccionVisual" Display="None"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNumeroReporte" meta:resourcekey="txtNumeroReporte" />
                </div>
                <div>
                    <asp:Panel ID="pnlDefecto" runat="server" Visible="false">
                        <div class="separador">
                            <asp:Label runat="server" ID="lblDefecto" meta:resourcekey="lblDefecto" CssClass="bold" />
                            <br />
                            <mimo:MappableDropDown runat="server" ID="ddlDefecto" AutoPostBack="false" />
                            <span class="required">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="valDefecto" Display="None" InitialValue=""
                                meta:resourcekey="valDefecto" ControlToValidate="ddlDefecto" ValidationGroup="defecto" />
                        </div>
                        <p>
                        </p>
                        <div>
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
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tfoot>
                                    </table></FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </asp:Panel>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                        TextMode="MultiLine" Rows="3" MaxLength="500">
                    </mimo:LabeledTextBox>
                </div>
                <p>
                </p>
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
                    <asp:Label runat="server" ID="lblResultado" meta:resourcekey="lblResultado" />
                    <br />
                    <asp:DropDownList ID="ddlResultado" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlResultado_SelectedIndexChanged">
                        <asp:ListItem Value="-1" Text=""></asp:ListItem>
                        <asp:ListItem Value="0" meta:resourcekey="lstReprobado"></asp:ListItem>
                        <asp:ListItem Value="1" meta:resourcekey="lstAprobado"></asp:ListItem>
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="valResultado" Display="None" InitialValue="-1"
                        meta:resourcekey="valResultado" ControlToValidate="ddlResultado" />
                </div>
                <asp:Panel ID="pnlDefecto2" runat="server" Visible="false">
                    <div class="separador">
                        <asp:Button ID="btnDefecto" runat="server" CssClass="boton" meta:resourcekey="btnDefecto"
                            ValidationGroup="defecto" OnClick="btnDefecto_Click" />
                    </div>
                    <p>
                    </p>
                </asp:Panel>
                <div class="separador">
                    <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" CssClass="boton"/></div>
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
                    <asp:ValidationSummary runat="server" ID="valSummaryDefecto" ValidationGroup="defecto"
                        CssClass="summary" meta:resourcekey="valSummaryDefecto" Width="120" />
                </div>
            </div>
        </div>
        <input type="hidden" runat="server" id="hdnProyectoID" />
        <p>
        </p>
        <div class="separador">
            <br />
            <br />
            <asp:Repeater ID="repVisual" runat="server" OnItemDataBound="repVisual_OnItemDataBound" OnItemCommand="repVisual_ItemCommand" >
                <HeaderTemplate>
                    <table class="repSam" cellpadding="0" cellspacing="0">
                        <thead>
                            <tr class="repEncabezado">
                                <th colspan="5">
                                    &nbsp;
                                </th>
                            </tr>
                            <tr class="repTitulos">
                                <th>
                                    &nbsp;
                                </th>
                                <th>
                                    <asp:Literal runat="server" ID="Literal1" meta:resourcekey="gbcNumeroReporte"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal runat="server" ID="Literal2" meta:resourcekey="gbcFecha"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal runat="server" ID="Literal3" meta:resourcekey="gbcFechaInspeccion"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal runat="server" ID="Literal4" meta:resourcekey="gbcResultado"></asp:Literal>
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="repFila">
                        <td>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false" CommandArgument='<%#Eval("JuntaCampoInspeccionVisualID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </td>
                        <td>
                            <%# Eval("NumeroReporte")%>
                        </td>
                        <td>
                            <%# Eval("FechaReporte", "{0:d}")%>
                        </td>
                        <td>
                            <%# Eval("FechaInspeccion", "{0:d}")%>
                        </td>
                        <td>
                            <%# Eval("Resultado")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="repFilaPar">
                        <td>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false" CommandArgument='<%#Eval("JuntaCampoInspeccionVisualID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </td>
                        <td>
                            <%# Eval("NumeroReporte")%>
                        </td>
                        <td>
                            <%# Eval("FechaReporte", "{0:d}")%>
                        </td>
                        <td>
                            <%# Eval("FechaInspeccion", "{0:d}")%>
                        </td>
                        <td>
                            <%# Eval("Resultado")%>
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
        </div>
    </div>
</asp:Content>
