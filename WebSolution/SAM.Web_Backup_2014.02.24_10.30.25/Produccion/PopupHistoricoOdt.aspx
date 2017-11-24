<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopupHistoricoOdt.aspx.cs" Inherits="SAM.Web.Produccion.PopupHistoricoOdt" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
<div >
    <h4>
        <asp:Literal runat="server" ID="litTitulo" />
    </h4>
    <div>
        <div class="divIzquierdo ancho60" style="margin-right:10px;">
            <div id="contenedorChk" class="listaCheck clear cajaAzul">        
            <asp:table CssClass="repSam" runat="server" ID="tblListaArchivos" meta:resourcekey="tblListaArchivos" Width="100%">
                <asp:TableHeaderRow CssClass="repEncabezado">
                    <asp:TableHeaderCell ColumnSpan="3"><asp:Literal runat="server" meta:resourcekey="litOrdenTrabajo"></asp:Literal></asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableHeaderRow CssClass="repTitulos">
                    <asp:TableHeaderCell Width="70%"><asp:Literal ID="Literal1" runat="server" meta:resourcekey="litFecha"></asp:Literal></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="20%"><asp:Literal ID="Literal2" runat="server" meta:resourcekey="litVersion"></asp:Literal></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="10%"><asp:Literal ID="Literal3" runat="server" meta:resourcekey="litArchivo"></asp:Literal></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:table>
            </div>
            <p></p>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">&nbsp;</div>

                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>
