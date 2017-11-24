<%@ Page Language="C#" MasterPageFile="~/Masters/PopupJuntasCampo.Master" AutoEventWireup="true"
    CodeBehind="PruebasPND.aspx.cs" Inherits="SAM.Web.Produccion.JuntasCampo.PruebasPND" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
<telerik:RadWindow runat="server" ID="rdwCambiarFechaProcesoAnterior">
    <ContentTemplate>
        <div style="margin-left: 30px; margin-top: 10px">
            <asp:HiddenField runat="server" ID="hdnCambiaFechas"/>
            <asp:HiddenField runat="server" ID="hdnProcesoAnterior2"/>
            <asp:HiddenField runat="server" ID="hdnProcesoReporteAnterior2"/>
            <asp:HiddenField runat="server" ID="hdnProcesoAnterior3"/>
            <asp:HiddenField runat="server" ID="hdnProcesoReporteAnterior3"/>
            <asp:HiddenField runat="server" ID="hdnProcesoAnterior4"/>
            <asp:HiddenField runat="server" ID="hdnProcesoReporteAnterior4"/>
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
                    <div ID="Reportes" runat="server">
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
                </div>
            <p>
                <asp:Button runat="server" ID="btnGuardarPopUp" meta:resourcekey="btnGuardarPopUp" CssClass="boton" OnClick="btnGuardarPopUp_OnClick" />
            </p>   
        </div>     
    </ContentTemplate>
</telerik:RadWindow>
    <div>
        <h4>
            <asp:Literal meta:resourcekey="lblPND" runat="server" ID="lblPND" />
        </h4>
        <input type="hidden" runat="server" id="hdnProyectoID" />
        <asp:PlaceHolder ID="phPrincipal" Visible="true" runat="server">
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
            <div>
                <div class="divIzquierdo ancho70">
                    <div>
                        <div class="divIzquierdo ancho50 boldElements">
                            <div class="separador">
                                <asp:Label runat="server" ID="lblNumeroRequisicion" meta:resourcekey="lblNumeroRequisicion"
                                    AssociatedControlID="ddlRequisicion" />
                                <mimo:MappableDropDown runat="server" ID="ddlRequisicion" OnSelectedIndexChanged="ddlRequisicion_SelectedIndexChanged"
                                    AutoPostBack="true" />
                                <span class="required">*</span>
                                <asp:RequiredFieldValidator runat="server" ID="valRequisicion" Display="None" InitialValue=""
                                    meta:resourcekey="valRequisicion" ControlToValidate="ddlRequisicion" />
                            </div>
                            <div class="separador">
                                <asp:Label runat="server" ID="lblFechaPND" meta:resourcekey="lblFechaPND" AssociatedControlID="mdpFechaPND" />
                                <mimo:MappableDatePicker ID="mdpFechaPND" runat="server" Style="width: 209px" />
                                <span class="required">*</span>
                                <asp:RequiredFieldValidator ID="valFechaPND" meta:resourcekey="valFechaPND" runat="server"
                                    ControlToValidate="mdpFechaPND" Display="None"></asp:RequiredFieldValidator>
                            </div>
                            <div class="separador">
                                <mimo:RequiredLabeledTextBox runat="server" ID="txtNumeroReporte" meta:resourcekey="txtNumeroReporte" />
                            </div>
                            <p>
                            </p>
                        </div>
                        <div class="divIzquierdo boldElements">
                            <div class="separador">
                                <asp:Label runat="server" ID="lblTipoPrueba" meta:resourcekey="lblTipoPrueba" CssClass="bold"></asp:Label>
                                <br />
                                <asp:Literal runat="server" ID="litTipoPrueba"></asp:Literal>
                            </div>
                            <div class="separador">
                                <asp:Label runat="server" ID="lblFechaReporte" meta:resourcekey="lblFechaReporte"
                                    AssociatedControlID="mdpFechaReporte" />
                                <mimo:MappableDatePicker ID="mdpFechaReporte" runat="server" Style="width: 209px" />
                                <span class="required">*</span>
                                <asp:RequiredFieldValidator ID="valFechaReporte" meta:resourcekey="valFechaReporte"
                                    runat="server" ControlToValidate="mdpFechaReporte" Display="None"></asp:RequiredFieldValidator>
                            </div>
                            <div class="separador">
                                <asp:Label runat="server" ID="lblResultado" meta:resourcekey="lblResultado" AssociatedControlID="ddlResultado" />                        
                                <asp:DropDownList ID="ddlResultado" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlResultado_SelectedIndexChanged">
                                    <asp:ListItem Value="-1" Text=""></asp:ListItem>
                                    <asp:ListItem Value="0" meta:resourcekey="lstReprobado"></asp:ListItem>
                                    <asp:ListItem Value="1" meta:resourcekey="lstAprobado"></asp:ListItem>
                                </asp:DropDownList>
                                <span class="required">*</span>
                                <asp:RequiredFieldValidator runat="server" ID="valResultado" Display="None" InitialValue="-1"
                                    meta:resourcekey="valResultado" ControlToValidate="ddlResultado" />
                            </div>
                        </div>
                        <p></p>
                    </div>
                    <asp:Panel ID="pnlDefecto" runat="server" Visible="false">
                        <div>
                            <div class="divIzquierdo ancho50">
                                <div class="separador">
                                    <asp:Label runat="server" ID="lblTipo" meta:resourcekey="lblTipo" CssClass="bold"
                                        AssociatedControlID="ddlTipoDefecto" />
                                    <asp:DropDownList ID="ddlTipoDefecto" runat="server" OnSelectedIndexChanged="ddlTipoDefecto_SelectedIndexChanged"
                                        AutoPostBack="true">
                                        <asp:ListItem Value="-1" Text=""></asp:ListItem>
                                        <asp:ListItem Value="1" meta:resourcekey="lstSector"></asp:ListItem>
                                        <asp:ListItem Value="2" meta:resourcekey="lstCuadrante"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span class="required">*</span>
                                    <asp:RequiredFieldValidator runat="server" ID="valTipo" Display="None" InitialValue=""
                                        meta:resourcekey="valTipo" ControlToValidate="ddlTipoDefecto" ValidationGroup="defectoCuadrante" />
                                </div>
                            </div>
                            <div class="divIzquierdo">
                                <div class="separador">
                                    <asp:Label runat="server" ID="lblDefecto" meta:resourcekey="lblDefecto" CssClass="bold"
                                        AssociatedControlID="ddlDefecto" />
                                    <mimo:MappableDropDown runat="server" ID="ddlDefecto" AutoPostBack="false" />
                                    <span class="required">*</span>                                
                                </div>
                            </div>
                            <p></p>
                        </div>
                        <asp:Panel ID="pnlCuadrante" runat="server" Visible="false">
                            <div>
                                <div class="divIzquierdo ancho50">
                                    <div class="separador">
                                        <mimo:RequiredLabeledTextBox runat="server" ID="txtCuadrante" meta:resourcekey="txtCuadrante"
                                            ValidationGroup="defectoCuadrante" />
                                    </div>
                                    <div class="separador">
                                        <br />
                                    </div>
                                </div>
                                <div class="divIzquierdo">
                                    <div class="separador">
                                        <mimo:RequiredLabeledTextBox runat="server" ID="txtPlaca" meta:resourcekey="txtPlaca"
                                            ValidationGroup="defectoCuadrante" />
                                    </div>
                                    <p>
                                    </p>
                                    <p>
                                    </p>
                                    <div class="separado">
                                        <asp:Button ID="btnDefecto" runat="server" CssClass="boton" meta:resourcekey="btnDefecto"
                                            OnClick="btnDefectpCuadrante_Click" ValidationGroup="defectoCuadrante" />
                                    </div>
                                </div>
                                <p></p>
                            </div>
                            <div>
                                <asp:Repeater ID="repDefectos" runat="server" OnItemCommand="repDefectos_ItemCommand"
                                    OnItemDataBound="repDefectos_ItemDataBound">
                                    <HeaderTemplate>
                                        <table class="repSam" cellpadding="0" cellspacing="0">
                                            <thead>
                                                <tr class="repEncabezado">
                                                    <th colspan="4">
                                                        &nbsp;
                                                    </th>
                                                </tr>
                                                <tr class="repTitulos">
                                                    <th>
                                                        &nbsp;
                                                    </th>
                                                    <th>
                                                        <asp:Literal ID="litCuadrante" runat="server" meta:resourcekey="litCuadrante"></asp:Literal>
                                                    </th>
                                                    <th>
                                                        <asp:Literal ID="litPlaca" runat="server" meta:resourcekey="litPlaca"></asp:Literal>
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
                                                <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("JuntaCampoReportePNDCuadranteID") %>'
                                                    OnClientClick="return Sam.Confirma(1);">
                                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
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
                                                <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("JuntaCampoReportePNDCuadranteID") %>'
                                                    OnClientClick="return Sam.Confirma(1);">
                                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
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
                        </asp:Panel>
                        <asp:Panel ID="pnlSector" runat="server" Visible="false">
                            <div>
                                <div class="divIzquierdo ancho50">
                                    <div class="separador">
                                        <mimo:RequiredLabeledTextBox runat="server" ID="txtSector" meta:resourcekey="txtSector"
                                            ValidationGroup="defectoSector" />
                                    </div>
                                    <div class="separador">
                                        <mimo:RequiredLabeledTextBox runat="server" ID="txtA" meta:resourcekey="txtA" ValidationGroup="defectoSector" />
                                    </div>
                                    <p>
                                    </p>
                                </div>
                                <div class="divIzquierdo">
                                    <div class="separador">
                                        <mimo:RequiredLabeledTextBox runat="server" ID="txtDe" meta:resourcekey="txtDe" ValidationGroup="defectoSector" />
                                    </div>
                                    <div class="separador">
                                        <asp:Button ID="btnDefectoSector" runat="server" CssClass="boton" meta:resourcekey="btnDefecto"
                                            OnClick="btnDefectoSector_OnClick" ValidationGroup="defectoSector" />
                                    </div>
                                    <p></p>
                                </div>
                                <p></p>
                            </div>
                            <div>
                                <asp:Repeater ID="repDefectoSector" runat="server" OnItemCommand="repDefectoSector_ItemCommand"
                                    OnItemDataBound="repDefectoSector_ItemDataBound">
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
                                                        <asp:Literal ID="litSector" runat="server" meta:resourcekey="litSector"></asp:Literal>
                                                    </th>
                                                    <th>
                                                        <asp:Literal ID="litDe" runat="server" meta:resourcekey="litDe"></asp:Literal>
                                                    </th>
                                                    <th>
                                                        <asp:Literal ID="litA" runat="server" meta:resourcekey="litA"></asp:Literal>
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
                                                <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("JuntaCampoReportePNDSectorID") %>'
                                                    OnClientClick="return Sam.Confirma(1);">
                                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
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
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr class="repFilaPar">
                                            <td>
                                                <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("JuntaCampoReportePNDSectorID") %>'
                                                    OnClientClick="return Sam.Confirma(1);">
                                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
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
                                                <td colspan="5">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </tfoot>
                                        </table></FooterTemplate>
                                </asp:Repeater>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                </div>
                <div class="divIzquierdo">
                    <div class="validacionesRecuadro" style="margin-top: 20px;">
                        <div class="validacionesHeader">
                            &nbsp;
                        </div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary"
                                Width="120" />
                            <asp:ValidationSummary runat="server" ID="valSummaryDefecto" ValidationGroup="defectoCuadrante"
                                CssClass="summary" meta:resourcekey="valSummaryDefecto" Width="120" />
                            <asp:ValidationSummary runat="server" ID="valsummarySector" ValidationGroup="defectoSector"
                                CssClass="summary" meta:resourcekey="valSummaryDefecto" Width="120" />
                        </div>
                    </div>
                </div>
                <p></p>
            </div>
            <div>
                <div class="divIzquierdo ancho70">
                    <div class="divIzquierdo ancho50">
                    <br />
                        <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                            TextMode="MultiLine" Rows="3" MaxLength="500" />
                    </div>
                    <div class="divIzquierdo">
                        <br />
                        <br />
                        <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" CssClass="boton"/>
                    </div>
                    <p></p>
                </div>
                <p></p>
            </div>
            <div style="margin-top:20px;" >
                <asp:Repeater ID="repPruebasPND" runat="server" OnItemDataBound="repPruebasPND_OnItemDataBound"
                    OnItemCommand="repPruebasPND_ItemCommand">
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
                                        <asp:Literal runat="server" ID="Literal3" meta:resourcekey="gbcTipoPrueba"></asp:Literal>
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
                                <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false"
                                    CommandArgument='<%#Eval("JuntaCampoReporteID") %>' OnClientClick="return Sam.Confirma(1);">
                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("NumeroReporte")%>
                            </td>
                            <td>
                                <%# Eval("Fecha", "{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("TipoPrueba")%>
                            </td>
                            <td>
                                <%# Eval("Resultado")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false"
                                    CommandArgument='<%#Eval("JuntaCampoReporteID") %>' OnClientClick="return Sam.Confirma(1);">
                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
                            </td>
                            <td>
                                <%# Eval("NumeroReporte")%>
                            </td>
                            <td>
                                <%# Eval("Fecha", "{0:d}")%>
                            </td>
                            <td>
                                <%# Eval("TipoPrueba")%>
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
            <asp:PlaceHolder runat="server" ID="phHistoria" Visible="false">
                <div>
                    <div>
                        <br />
                        <asp:Literal runat="server" ID="litTextoHistoricas" meta:resourcekey="litTextoHistoricas"/>
                        <br />
                    </div>
                    <div>
                        <br />
                        <asp:Repeater runat="server" ID="repPruebasHistoricasRechazadas" OnItemCommand="repPruebasHistoricasRechazadas_OnItemCommand">
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
                                                <asp:Literal runat="server" ID="Literal3" meta:resourcekey="gbcTipoPrueba"></asp:Literal>
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
                                        <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false" CommandArgument='<%#Eval("JuntaCampoReporteID") %>' OnClientClick="return Sam.Confirma(1);">
                                            <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server" meta:resourcekey="imgBorrar" />
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("NumeroReporte")%>
                                    </td>
                                    <td>
                                        <%# Eval("Fecha", "{0:d}")%>
                                    </td>
                                    <td>
                                        <%# Eval("TipoPrueba")%>
                                    </td>
                                    <td>
                                        <%# Eval("Resultado")%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="repFilaPar">
                                    <td>
                                        <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false" CommandArgument='<%#Eval("JuntaCampoReporteID") %>' OnClientClick="return Sam.Confirma(1);">
                                            <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server" meta:resourcekey="imgBorrar" />
                                        </asp:LinkButton>
                                    </td>
                                    <td>
                                        <%# Eval("NumeroReporte")%>
                                    </td>
                                    <td>
                                        <%# Eval("Fecha", "{0:d}")%>
                                    </td>
                                    <td>
                                        <%# Eval("TipoPrueba")%>
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
                                </table>
                            </FooterTemplate>
                
                        </asp:Repeater>
                    </div>
                </div>
            </asp:PlaceHolder>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="phMensaje" Visible = "false" runat="server">
            <div class="cajaAzul">
                <p>
                </p>
                    <asp:Literal runat="server" ID="litMensajeNuevaJunta" Visible = "false" meta:resourcekey="litMensajeNuevaJunta" ></asp:Literal>
                    <asp:Literal runat="server" ID="litMensajeNuevoCorte" Visible = "false" meta:resourcekey="litMensajeNuevoCorte" ></asp:Literal>
                <p>
                </p>
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
