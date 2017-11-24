<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialRO.ascx.cs" Inherits="SAM.Web.Controles.Spool.MaterialRO" %>
<div class="infoJuntas">
    <table class="repSam" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="90" />
            <col width="200" />
            <col width="60" />
            <col width="60" />
            <col width="60" />
            <col width="60" />
            <col width="80" />
            <col width="100" />
            <col width="60" />
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="9"><asp:Literal runat="server" ID="litMateriales" meta:resourcekey="litMateriales" /></th>
            </tr>
            <tr class="repTitulos">
                <th><asp:Literal runat="server" ID="litIcCodigo" meta:resourcekey="litIcCodigo" /></th>
                <th><asp:Literal runat="server" ID="litIcDescripcion" meta:resourcekey="litIcDescripcion" /></th>
                <th><asp:Literal runat="server" ID="litD1" meta:resourcekey="litD1" /></th>
                <th><asp:Literal runat="server" ID="litD2" meta:resourcekey="litD2" /></th>
                <th><asp:Literal runat="server" ID="litCantidad" meta:resourcekey="litCantidad" /></th>
                <th><asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" /></th>
                <th><asp:Literal runat="server" ID="litCategoria" meta:resourcekey="litCategoria" /></th>
                <th><asp:Literal runat="server" ID="litEspecificacion" meta:resourcekey="litEspecificacion" /></th>
                <th><asp:Literal runat="server" ID="litKg" meta:resourcekey="litKg" /></th>
            </tr>
        </thead>
        <asp:Repeater runat="server" ID="repMateriales">
            <ItemTemplate>
                <tr class="repFila">
                    <td><%#Eval("CodigoItemCode")%></td>
                    <td><%#Eval("DescripcionItemCode")%></td>
                    <td><%#String.Format("{0:N3}",Eval("Diametro1"))%></td>
                    <td><%#String.Format("{0:N3}",Eval("Diametro2"))%></td>
                    <td><%#String.Format("{0:N0}",Eval("Cantidad"))%></td>
                    <td><%#Eval("Etiqueta")%></td>
                    <td><%#Eval("Categoria")%></td>
                    <td><%#Eval("Especificacion")%></td>
                    <td><%#String.Format("{0:N2}",Eval("Peso"))%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td><%#Eval("CodigoItemCode")%></td>
                    <td><%#Eval("DescripcionItemCode")%></td>
                    <td><%#String.Format("{0:#0.000}",Eval("Diametro1"))%></td>
                    <td><%#String.Format("{0:#0.000}",Eval("Diametro2"))%></td>
                    <td><%#Eval("Cantidad")%></td>
                    <td><%#Eval("Etiqueta")%></td>
                    <td><%#Eval("Categoria")%></td>
                    <td><%#Eval("Especificacion")%></td>
                    <td><%#Eval("Peso")%></td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        <tfoot>
            <tr class="repPie">
                <td colspan="9">&nbsp;</td>
            </tr>
        </tfoot>
    </table>
</div>