<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CorteROHistorico.ascx.cs" Inherits="SAM.Web.Controles.Spool.CorteROHistorico" %>
<div class="infoCorte soloLectura">
    <table class="repSam" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="60" />
            <col width="90" />
            <col width="150" />
            <col width="60" />
            <col width="60" />
            <col width="60" />
            <col width="70" />
            <col width="65" />
            <col width="65" />
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="9"><asp:Literal runat="server" ID="litCortes" meta:resourcekey="litCortes" /></th>
            </tr>
            <tr class="repTitulos">
                <th><asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" /></th>
                <th><asp:Literal runat="server" ID="litIcCodigo" meta:resourcekey="litIcCodigo" /></th>
                <th><asp:Literal runat="server" ID="litIcDescripcion" meta:resourcekey="litIcDescripcion" /></th>
                <th><asp:Literal runat="server" ID="litDiametro" meta:resourcekey="litDiametro" /></th>
                <th><asp:Literal runat="server" ID="litCantidad" meta:resourcekey="litCantidad" /></th>
                <th><asp:Literal runat="server" ID="litSegmento" meta:resourcekey="litSegmento" /></th>
                <th><asp:Literal runat="server" ID="litInicioFin" meta:resourcekey="litInicioFin" /></th>
                <th><asp:Literal runat="server" ID="litTipoCorte1" meta:resourcekey="litTipoCorte1" /></th>
                <th><asp:Literal runat="server" ID="litTipoCorte2" meta:resourcekey="litTipoCorte2" /></th>
            </tr>
        </thead>
        <asp:Repeater runat="server" ID="repCortes">
            <ItemTemplate>
                <tr class="repFila">
                    <td><%#Eval("EtiquetaMaterial")%></td>
                    <td><%#Eval("CodigoItemCode")%></td>
                    <td><%#Eval("DescripcionItemCode")%></td>
                    <td><%#String.Format("{0:N3}",Eval("Diametro"))%></td>
                    <td><%#String.Format("{0:N0}",Eval("Cantidad"))%></td>
                    <td><%#Eval("EtiquetaSegmento")%></td>
                    <td><%#Eval("InicioFin")%></td>
                    <td><%#Eval("TipoCorte1")%></td>
                    <td><%#Eval("TipoCorte2")%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td><%#Eval("EtiquetaMaterial")%></td>
                    <td><%#Eval("CodigoItemCode")%></td>
                    <td><%#Eval("DescripcionItemCode")%></td>
                    <td><%#String.Format("{0:N3}",Eval("Diametro"))%></td>
                    <td><%#String.Format("{0:N0}",Eval("Cantidad"))%></td>
                    <td><%#Eval("EtiquetaSegmento")%></td>
                    <td><%#Eval("InicioFin")%></td>
                    <td><%#Eval("TipoCorte1")%></td>
                    <td><%#Eval("TipoCorte2")%></td>
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