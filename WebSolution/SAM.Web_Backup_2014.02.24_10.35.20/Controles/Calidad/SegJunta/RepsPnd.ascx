<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepsPnd.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.RepsPnd" %>
<asp:Panel runat="server" ID="pnlDefectosSector" CssClass="divIzquierdo ancho45">

    <table class="repSam ancho100" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="25%" />
            <col width="25%" />
            <col width="25%" />
            <col width="25%" />
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="4"><asp:Literal runat="server" ID="litHeaderSector"  meta:resourcekey="litHeaderSector" /></th>
            </tr>
            <tr class="repTitulos">
                <td><asp:Literal runat="server" ID="litProceso"  meta:resourcekey="litSector" /></td>
                <td><asp:Literal runat="server" ID="litCodigoSoldador"  meta:resourcekey="litDe" /></td>
                <td><asp:Literal runat="server" ID="litNombre"  meta:resourcekey="litA" /></td>
                <td><asp:Literal runat="server" ID="litConsumible"  meta:resourcekey="litDefecto" /></td>
            </tr>
        </thead>
        <asp:Repeater runat="server" ID="repSector">
            <ItemTemplate>
                <tr class="repFila">
                    <td><%# Eval("Sector") %></td>                                
                    <td><%# Eval("De") %></td> 
                    <td><%# Eval("A") %></td> 
                    <td><%# Eval("Defecto") %></td> 
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td><%# Eval("Sector")%></td>                                
                    <td><%# Eval("De") %></td> 
                    <td><%# Eval("A") %></td> 
                    <td><%# Eval("Defecto") %></td>                              
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        <tfoot>
            <tr class="repPie">
                <td colspan="4">&nbsp;</td>                
            </tr>
        </tfoot>
    </table>  
        
</asp:Panel>

<asp:Panel runat="server" ID="pnlDefectosCuadrante" CssClass="divDerecho ancho45">

    <table class="repSam ancho100" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="33%" />
            <col width="33%" />
            <col width="34%" />               
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="3"><asp:Literal runat="server" ID="litHeaderCuadrante"  meta:resourcekey="litHeaderCuadrante" /></th>
            </tr>
            <tr class="repTitulos">
                <td><asp:Literal runat="server" ID="Literal1"  meta:resourcekey="litCuadrante" /></td>
                <td><asp:Literal runat="server" ID="Literal2"  meta:resourcekey="litPlaca" /></td>
                <td><asp:Literal runat="server" ID="Literal3"  meta:resourcekey="litDefecto" /></td>                    
            </tr>
        </thead>
        <asp:Repeater runat="server" ID="repCuad">
            <ItemTemplate>
                <tr class="repFila">
                    <td><%# Eval("Cuadrante") %></td>                                
                    <td><%# Eval("Placa") %></td> 
                    <td><%# Eval("Defecto") %></td>                         
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td><%# Eval("Cuadrante") %></td>                                
                    <td><%# Eval("Placa") %></td> 
                    <td><%# Eval("Defecto") %></td>                                 
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        <tfoot>
            <tr class="repPie">
                <td colspan="3">&nbsp;</td>
            </tr>
        </tfoot>
    </table>  
        
</asp:Panel>