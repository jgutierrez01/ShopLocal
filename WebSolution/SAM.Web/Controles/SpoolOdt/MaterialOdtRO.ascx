<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialOdtRO.ascx.cs" Inherits="SAM.Web.Controles.SpoolOdt.MaterialOdtRO" %>
<%@ Import Namespace="Mimo.Framework.Extensions" %>
<div class="infoJuntas">
    <div class="icoReingEncabezado" style="width:730px;">
        <asp:Image runat="server" ID="imgReingenieria" ImageUrl="~/Imagenes/Iconos/ico_reingenieria.png" />
        <asp:Label runat="server" ID="lblReing" meta:resourcekey="lblReing" />
    </div>
    <table class="repSam" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="30" />
            <col width="90" />
            <col width="200" />
            <col width="60" />
            <col width="60" />
            <col width="60" />
            <col width="60" />
            <col width="80" />
            <col width="100" />
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="9"><asp:Literal runat="server" ID="litMateriales" meta:resourcekey="litMateriales" /></th>
            </tr>
            <tr class="repTitulos">
                <th>&nbsp;</th>
                <th><asp:Literal runat="server" ID="litIcCodigo" meta:resourcekey="litIcCodigo" /></th>
                <th><asp:Literal runat="server" ID="litIcDescripcion" meta:resourcekey="litIcDescripcion" /></th>
                <th><asp:Literal runat="server" ID="litD1" meta:resourcekey="litD1" /></th>
                <th><asp:Literal runat="server" ID="litD2" meta:resourcekey="litD2" /></th>
                <th><asp:Literal runat="server" ID="litCantidad" meta:resourcekey="litCantidad" /></th>
                <th><asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" /></th>
                <th><asp:Literal runat="server" ID="litCategoria" meta:resourcekey="litCategoria" /></th>
                <th><asp:Literal runat="server" ID="litEspecificacion" meta:resourcekey="litEspecificacion" /></th>
            </tr>
        </thead>
        <asp:Repeater runat="server" ID="repMateriales">
            <ItemTemplate>
                <tr class="repFila">
                    <td>
                        <asp:Image runat="server" ID="imgReing" Visible='<%#!Eval("ExisteEnLaOdt").SafeBoolParse()%>' ImageUrl="~/Imagenes/Iconos/ico_reingenieria.png" meta:resourcekey="imgReing" />
                        <asp:Image runat="server" ID="imgFueReing" Visible='<%#Eval("FueReingenieria").SafeBoolParse()%>' ImageUrl="~/Imagenes/Iconos/ico_reingenieria_verde.png" meta:resourcekey="imgFueReing" />
                    </td>
                    <td><%#Eval("CodigoItemCode")%></td>
                    <td><%#Eval("DescripcionItemCode")%></td>
                    <td><%#String.Format("{0:#0.000}",Eval("Diametro1"))%></td>
                    <td><%#String.Format("{0:#0.000}",Eval("Diametro2"))%></td>
                    <td><%#Eval("Cantidad")%></td>
                    <td><%#Eval("Etiqueta")%></td>
                    <td><%#Eval("Categoria")%></td>
                    <td><%#Eval("Especificacion")%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td>
                        <asp:Image runat="server" ID="imgReing" Visible='<%#!Eval("ExisteEnLaOdt").SafeBoolParse()%>' ImageUrl="~/Imagenes/Iconos/ico_reingenieria.png" meta:resourcekey="imgReing" />
                        <asp:Image runat="server" ID="imgFueReing" Visible='<%#Eval("FueReingenieria").SafeBoolParse()%>' ImageUrl="~/Imagenes/Iconos/ico_reingenieria_verde.png" meta:resourcekey="imgFueReing" />
                    </td>
                    <td><%#Eval("CodigoItemCode")%></td>
                    <td><%#Eval("DescripcionItemCode")%></td>
                    <td><%#String.Format("{0:#0.000}",Eval("Diametro1"))%></td>
                    <td><%#String.Format("{0:#0.000}",Eval("Diametro2"))%></td>
                    <td><%#Eval("Cantidad")%></td>
                    <td><%#Eval("Etiqueta")%></td>
                    <td><%#Eval("Categoria")%></td>
                    <td><%#Eval("Especificacion")%></td>
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