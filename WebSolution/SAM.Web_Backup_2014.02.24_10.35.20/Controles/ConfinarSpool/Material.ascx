<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Material.ascx.cs" Inherits="SAM.Web.Controles.ConfinarSpool.Material" %>
<div class="infoJuntas">
    <table class="repSam" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="50" />
            <col width="70" />
            <col width="70" />
            <col width="70" />
            <col width="40" />
            <col width="100" />
            <col width="50" />
            <col width="270" />
            <col width="50" />
            <col width="40" />
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="10"><asp:Literal runat="server" ID="litMaterial" meta:resourcekey="litMaterial" /></th>
            </tr>
            <tr>
                <th><asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" /></th>
                <th><asp:Literal runat="server" ID="litItemCode" meta:resourcekey="litItemCode" /></th>
                <th><asp:Literal runat="server" ID="litDiametro1" meta:resourcekey="litDiametro1" /></th>
                <th><asp:Literal runat="server" ID="litDiametro2" meta:resourcekey="litDiametro2" /></th>
                <th><asp:Literal runat="server" ID="litKg" meta:resourcekey="litKg" /></th>
                <th><asp:Literal runat="server" ID="litCantLong" meta:resourcekey="litCantLong" /></th>
                <th><asp:Literal runat="server" ID="litGrupo" meta:resourcekey="litGrupo" /></th>
                <th><asp:Literal runat="server" ID="litDescripcion" meta:resourcekey="litDescripcion" /></th>
                <th><asp:Literal runat="server" ID="litEspecificacion" meta:resourcekey="litEspecificacion" /></th>
                <th><asp:Literal runat="server" ID="litArea" meta:resourcekey="litArea" /></th>
            </tr>
        </thead>

        <asp:Repeater ID="repDetMaterial" runat="server" >
            <ItemTemplate>
                <tr class="repFila">
                    <td align="center"><%#Eval("Etiqueta")%></td>
 	                <td align="center"><%#Eval("CodigoItemCode")%></td>
 	                <td align="center"><%#Eval("Diametro1")%></td>
                    <td align="center"><%#Eval("Diametro2")%></td>
 	                <td align="center"><%#Eval("Peso")%></td>
 	                <td align="center"><%#Eval("Cantidad")%></td>
 	                <td align="center"><%#Eval("Grupo")%></td>
 	                <td align="justify"><%#Eval("DescripcionItemCode")%></td>
 	                <td align="center"><%#Eval("Especificacion")%></td>
 	                <td align="center"><%#Eval("Area")%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td align="center"><%#Eval("Etiqueta")%></td>
 	                <td align="center"><%#Eval("CodigoItemCode")%></td>
 	                <td align="center"><%#Eval("Diametro1")%></td>
                    <td align="center"><%#Eval("Diametro2")%></td>
 	                <td align="center"><%#Eval("Peso")%></td>
 	                <td align="center"><%#Eval("Cantidad")%></td>
 	                <td align="center"><%#Eval("Grupo")%></td>
 	                <td align="justify"><%#Eval("DescripcionItemCode")%></td>
 	                <td align="center"><%#Eval("Especificacion")%></td>
 	                <td align="center"><%#Eval("Area")%></td>
            </AlternatingItemTemplate>
        </asp:Repeater>

    </table>
</div>
