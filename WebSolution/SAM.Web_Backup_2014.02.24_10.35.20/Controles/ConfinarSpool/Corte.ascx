<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Corte.ascx.cs" Inherits="SAM.Web.Controles.ConfinarSpool.Corte" %>
<table class="repSam" cellpadding="0" cellspacing="0">
    <colgroup>
        <col width="100" />
        <col width="90" />
        <col width="90" />
        <col width="100" />
        <col width="90" />
        <col width="90" />
        <col width="90" />
    </colgroup>
    <thead>
        <tr class="repEncabezado">
            <th colspan="8"><asp:Literal runat="server" ID="litCortes" meta:resourcekey="litCortes" /></th>
        </tr>
        <tr>
            <th><asp:Literal runat="server" ID="litEtiquetaMaterial" meta:resourcekey="litEtiquetaMaterial" /></th>
            <th><asp:Literal runat="server" ID="litItemCode" meta:resourcekey="litItemCode" /></th>
            <th><asp:Literal runat="server" ID="litInicioFin" meta:resourcekey="litInicioFin" /></th>
            <th><asp:Literal runat="server" ID="litEtiquetaSeccion" meta:resourcekey="litEtiquetaSeccion" /></th>
            <th><asp:Literal runat="server" ID="litCantidad" meta:resourcekey="litCantidad" /></th>
            <th><asp:Literal runat="server" ID="litProfile1" meta:resourcekey="litProfile1" /></th>
            <th><asp:Literal runat="server" ID="litProfile2" meta:resourcekey="litProfile2" /></th>
            <th><asp:Literal runat="server" ID="litDiametro1" meta:resourcekey="litDiametro1" /></th>
        </tr>
    </thead>

    <asp:Repeater ID="repDetCortes" runat="server" >
        <ItemTemplate>
            <tr>
                <td align="center"><%#Eval("EtiquetaMaterial")%></td>
 	            <td align="center"><%#Eval("CodigoItemCode")%></td>
 	            <td align="center"><%#Eval("InicioFin")%></td>
 	            <td align="center"><%#Eval("EtiquetaSegmento")%></td>
 	            <td align="center"><%#Eval("Cantidad")%></td>
 	            <td align="center"><%#Eval("TipoCorte1")%></td>
 	            <td align="center"><%#Eval("TipoCorte1")%></td>
 	            <td align="center"><%#Eval("Diametro")%></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
                <td align="center"><%#Eval("EtiquetaMaterial")%></td>
 	            <td align="center"><%#Eval("CodigoItemCode")%></td>
 	            <td align="center"><%#Eval("InicioFin")%></td>
 	            <td align="center"><%#Eval("EtiquetaSegmento")%></td>
 	            <td align="center"><%#Eval("Cantidad")%></td>
 	            <td align="center"><%#Eval("TipoCorte1")%></td>
 	            <td align="center"><%#Eval("TipoCorte1")%></td>
 	            <td align="center"><%#Eval("Diametro")%></td>
        </AlternatingItemTemplate>
    </asp:Repeater>

</table>
