<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DetJunta.ascx.cs" Inherits="SAM.Web.Controles.Spool.DetJunta" %>
<div class="infoJuntas">
    <table class="repSam" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="40" />
            <col width="60" />
            <col width="60" />
            <col width="60" />
            <col width="80" />
            <col width="100" />
            <col width="100" />
            <col width="60" />
            <col width="60" />          
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="9"><asp:Literal runat="server" ID="litJuntas" meta:resourcekey="litJuntas" /></th>
            </tr>
            <tr>
                <th><asp:Literal runat="server" ID="litJunta" meta:resourcekey="litJunta" /></th>
                <th><asp:Literal runat="server" ID="litDiametro" meta:resourcekey="litDiametro" /></th>
                <th><asp:Literal runat="server" ID="litTipo" meta:resourcekey="litTipo" /></th>
                <th><asp:Literal runat="server" ID="litCedula" meta:resourcekey="litCedula" /></th>
                <th><asp:Literal runat="server" ID="litLocalizacion" meta:resourcekey="litLocalizacion" /></th>
                <th><asp:Literal runat="server" ID="litFamiliaAcero1" meta:resourcekey="litFamiliaAcero1" /></th>
                <th><asp:Literal runat="server" ID="litFamiliaAcero2" meta:resourcekey="litFamiliaAcero2" /></th>
                <th><asp:Literal runat="server" ID="litEspesor" meta:resourcekey="litEspesor" /></th>
                <th><asp:Literal runat="server" ID="litFabArea" meta:resourcekey="litFabArea" /></th>
            </tr>
        </thead>

        <asp:Repeater ID="repDetJuntas" runat="server" >
            <ItemTemplate>
                <tr class="repFila">
                    <td align="center"><%#Eval("JuntaSpoolID")%></td>
 	                <td align="center"><%#Eval("Diametro")%></td>
 	                <td align="center"><%#Eval("TipoJunta")%></td>
 	                <td align="center"><%#Eval("Cedula")%></td>
 	                <td align="center"><%#Eval("Localizacion")%></td>
 	                <td align="center"><%#Eval("FamiliaAceroMaterial1")%></td>
 	                <td align="center"><%#Eval("FamiliaAceroMaterial2")%></td>
 	                <td align="center"><%#Eval("Espesor")%></td>
 	                <td align="center"><%#Eval("FabArea")%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td align="center"><%#Eval("JuntaSpoolID")%></td>
                    <td align="center"><%#Eval("Diametro")%></td>
                    <td align="center"><%#Eval("TipoJunta")%></td>
                    <td align="center"><%#Eval("Cedula")%></td>
                    <td align="center"><%#Eval("Localizacion")%></td>
                    <td align="center"><%#Eval("FamiliaAceroMaterial1")%></td>
                    <td align="center"><%#Eval("FamiliaAceroMaterial2")%></td>
                    <td align="center"><%#Eval("Espesor")%></td>
 	                <td align="center"><%#Eval("FabArea")%></td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>

    </table>
</div>