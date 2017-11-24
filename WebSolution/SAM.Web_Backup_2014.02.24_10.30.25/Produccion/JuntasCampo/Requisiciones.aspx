<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/PopupJuntasCampo.Master" AutoEventWireup="true"
    CodeBehind="Requisiciones.aspx.cs" Inherits="SAM.Web.Produccion.JuntasCampo.Requisiciones" %>

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
            <asp:Literal meta:resourcekey="lblRequisicion" runat="server" ID="lblRequisicion" />
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
                    <asp:Label runat="server" ID="lblFechaRequisicion" meta:resourcekey="lblFechaRequisicion" />
                    <br />
                    <mimo:MappableDatePicker ID="mdpFechaRequisicion" runat="server" Style="width: 209px" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valFechaRequisicion" meta:resourcekey="valFechaRequisicion"
                        runat="server" ControlToValidate="mdpFechaRequisicion" Display="None"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNumeroRequisicion" meta:resourcekey="txtNumeroRequisicion" />
                </div>
                <div class="separador">
                    <br />
                </div>
            </div>
            <div class="divDerecho ancho50 boldElements">
                <div class="separador">
                    <asp:Label runat="server" ID="lblTipoPrueba" meta:resourcekey="lblTipoPrueba" />
                    <br />
                     <mimo:MappableDropDown runat="server" ID="ddlTipoPrueba" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valTipoPrueba" meta:resourcekey="valTipoPrueba" runat="server"
                        ControlToValidate="ddlTipoPrueba" Display="None"></asp:RequiredFieldValidator>
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtCodigo" meta:resourcekey="txtCodigo" />
                </div>
                <div class="separador">
                    <asp:Button ID="btnRequisitar" runat="server" meta:resourcekey="btnRequisitar" CssClass="boton" />
                </div>
                <p>
                </p>
            </div>
            <div>
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
                </div>
            </div>
        </div>
        <p>
        </p>
        <div>

        <asp:Repeater ID="repRequi" runat="server" OnItemCommand="repRequi_ItemCommand" >
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
                                    <asp:Literal runat="server" ID="Literal1" meta:resourcekey="gcbNumeroRequisicion"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal runat="server" ID="Literal2" meta:resourcekey="gbcFecha"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal runat="server" ID="Literal3" meta:resourcekey="gbcTipoPrueba"></asp:Literal>
                                </th>
                            </tr>
                        </thead>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="repFila">
                        <td>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" CausesValidation="false" runat="server" CommandArgument='<%#Eval("RequisicionID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </td>
                        <td>
                            <%# Eval("NumeroRequisicion")%>
                        </td>
                        <td>
                            <%# Eval("Fecha","{0:d}")%>
                        </td>
                        <td>
                            <%# Eval("TipoPrueba")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="repFilaPar">
                        <td>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" CausesValidation="false" runat="server" CommandArgument='<%#Eval("RequisicionID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </td>
                        <td>
                            <%# Eval("NumeroRequisicion")%>
                        </td>
                        <td>
                            <%# Eval("Fecha", "{0:d}")%>
                        </td>
                        <td>
                            <%# Eval("TipoPrueba")%>
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
