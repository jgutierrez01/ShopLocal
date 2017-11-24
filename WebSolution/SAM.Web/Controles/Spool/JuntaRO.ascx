<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JuntaRO.ascx.cs" Inherits="SAM.Web.Controles.Spool.JuntaRO" %>
<div class="infoJuntas">
    <table class="repSam" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="60" />
            <col width="60" />
            <col width="60" />
            <col width="80" />
            <col width="80" />
            <col width="90" />
            <col width="80" />
            <col width="70" />
            <col width="70" />
            <col width="70" />
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="10"><asp:Literal runat="server" ID="litJuntas" meta:resourcekey="litJuntas" /></th>
            </tr>
            <tr class="repTitulos">
                <th><asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" /></th>
                <th><asp:Literal runat="server" ID="litDiametro" meta:resourcekey="litDiametro" /></th>
                <th><asp:Literal runat="server" ID="litTipoJunta" meta:resourcekey="litTipoJunta" /></th>
                <th><asp:Literal runat="server" ID="litCedula" meta:resourcekey="litCedula" /></th>
                <th><asp:Literal runat="server" ID="litLocalizacion" meta:resourcekey="litLocalizacion" /></th>
                <th><asp:Literal runat="server" ID="litFamiliaAcero" meta:resourcekey="litFamiliaAcero" /></th>
                <th><asp:Literal runat="server" ID="litFabArea" meta:resourcekey="litFabArea" /></th>
                <th><asp:Literal runat="server" ID="litEspesor" meta:resourcekey="litEspesor" /></th>
                <th><asp:Literal runat="server" ID="litPeqs" meta:resourcekey="litPeqs" /></th>
                <th><asp:Literal runat="server" ID="litRequierePwht" meta:resourcekey="litRequierePwht" /></th>
            </tr>
        </thead>
        <asp:Repeater runat="server" ID="repJuntas">
            <ItemTemplate>
                <tr class="repFila">
                    <td><%#Eval("Etiqueta")%></td>
                    <td><%#String.Format("{0:N3}",Eval("Diametro"))%></td>
                    <td><%#Eval("TipoJunta")%></td>
                    <td><%#Eval("Cedula")%></td>
                    <td><%#Eval("Localizacion")%></td>
                    <td><%#Eval("FamiliasAcero")%></td>
                    <td><%#Eval("FabArea")%></td>
                    <td><%#String.Format("{0:N}",Eval("Espesor"))%></td>
                    <td><%#String.Format("{0:N3}",Eval("Peqs"))%></td>
                    <td><%#Eval("RequierePwht")%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td><%#Eval("Etiqueta")%></td>
                    <td><%#String.Format("{0:N3}",Eval("Diametro"))%></td>
                    <td><%#Eval("TipoJunta")%></td>
                    <td><%#Eval("Cedula")%></td>
                    <td><%#Eval("Localizacion")%></td>
                    <td><%#Eval("FamiliasAcero")%></td>
                    <td><%#Eval("FabArea")%></td>
                    <td><%#String.Format("{0:N}",Eval("Espesor"))%></td>
                    <td><%#String.Format("{0:N3}",Eval("Peqs"))%></td>
                    <td><%#Eval("RequierePwht")%></td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        <tfoot>
            <tr class="repPie">
                <td colspan="10">&nbsp;</td>
            </tr>
        </tfoot>
    </table>
</div>