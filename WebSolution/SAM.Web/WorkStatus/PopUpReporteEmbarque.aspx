<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpReporteEmbarque.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpReporteEmbarque" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
<h4>
        <asp:Literal runat="server" ID="lblReportesEmbarque" meta:resourcekey="lblReportesEmbarque"></asp:Literal>
    </h4>
    <div class="cajaAzul">
        <div class="divIzquierdo ancho50">
            <div class="separadorDashboard">
                <asp:Label ID="lblSpoolLabel" runat="server" CssClass="bold" meta:resourcekey="lblSpoolLabel"></asp:Label>&nbsp;
                <asp:Label ID="lblSpool" runat="server"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblNumControlLabel" runat="server" CssClass="bold" meta:resourcekey="lblNumControlLabel"></asp:Label>&nbsp;
                <asp:Label ID="lblNumControl" runat="server"></asp:Label>
            </div>            
        </div>
        <div class="divIzquierdo ancho50">           
               <asp:Button ID="btnActualiza" meta:resourcekey="btnActualiza" runat="server" CssClass="boton"
                    OnClick="btnActualiza_OnClick" />
            
            
        </div>
        <p>
        </p>
    </div>
    <p>
    </p>
    <div>
        <div class="divIzquierdo ancho70">
            <table class="repSam" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col width="30" />
                    <col width="150" />
                    <col width="150" />
                    <col width="150" />
                </colgroup>
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="4">
                            <asp:Literal runat="server" ID="litReportes" meta:resourcekey="litReportes" />
                        </th>
                    </tr>
                    <tr class="repTitulos">
                        <th>
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litAccion" meta:resourcekey="litAccion" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litFecha" meta:resourcekey="litFecha" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" />
                        </th>
                    </tr>
                </thead>
                <asp:Repeater runat="server" ID="repReportes" OnItemCommand="repReportes_ItemCommand"
                    OnItemDataBound="repReportes_ItemDataBound">
                    <ItemTemplate>
                        <tr class="repFila">
                            <td>
                                <asp:ImageButton ID="lnkBorra" runat="server" ImageUrl="~/Imagenes/Iconos/borrar.png"
                                    CommandName="Borra" CommandArgument='<%#Eval("TipoAccionEmbarqueEnum")%>' OnClientClick="return Sam.Confirma(1);">
                                </asp:ImageButton>
                            </td>
                            <td>
                                <%#Eval("Accion")%>
                            </td>
                            <td>
                                <%#Eval("Fecha")%>
                            </td>
                            <td>
                                <%#Eval("Etiqueta")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <asp:ImageButton ID="lnkBorra" runat="server" ImageUrl="~/Imagenes/Iconos/borrar.png"
                                    CommandName="Borra" CommandArgument='<%#Eval("TipoAccionEmbarqueEnum")%>' OnClientClick="return Sam.Confirma(1);">
                                </asp:ImageButton>
                            </td>
                            <td>
                                <%#Eval("Accion")%>
                            </td>
                            <td>
                                <%#Eval("Fecha")%>
                            </td>
                            <td>
                                <%#Eval("Etiqueta")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
                <tfoot>
                    <tr class="repPie">
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <div class="divIzquierdo ancho25">
            <div class="validacionesRecuadro" style="margin-top: 20px;">
                <div class="validacionesHeader">
                    &nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary"
                        Width="120" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
