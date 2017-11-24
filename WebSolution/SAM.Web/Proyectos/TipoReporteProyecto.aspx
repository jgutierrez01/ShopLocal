<%@ Page  Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="TipoReporteProyecto.aspx.cs" Inherits="SAM.Web.Proyectos.TipoReporteProyecto" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>

<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="lblTitulo" meta:resourcekey="lblTitulo" />
    <div class="cntCentralForma">
    <div class="valGroupArriba">
        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" meta:resourcekey="valSummary" />
    </div>
    <br />
    <div>
        <table class="repSam ancho100" cellpadding="0" cellspacing="0">
            <colgroup>
                <col width="20%" />
                <col width="20%" />
                <col width="20%" />
                <col width="20%" />
            </colgroup>
            <thead>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="litVacio" meta:resourcekey="litVacio" /></th>
                    <th><asp:Literal runat="server" ID="litReportePerzonalizado" meta:resourcekey="litReportePerzonalizado" /></th>
                    <th><asp:Literal runat="server" ID="litReporteEspanol" meta:resourcekey="litReporteEspanol" /></th>
                    <th><asp:Literal runat="server" ID="litReporteIngles" meta:resourcekey="litReporteIngles" /></th>
                </tr>
            </thead>
            <asp:Repeater runat="server" ID="repTiposReporte" OnItemDataBound="repTiposReporte_ItemDataBound">
            <ItemTemplate>
                <tr class="repFila">
                    <td><asp:Label runat="server" ID="lblNombre" Text='<%# Eval("NombreInt")%>' />
                    <asp:HiddenField runat="server" ID="hdnTipoReporteID" Value='<%# Eval("TipoReporteProyectoID") %>' />
                    <asp:HiddenField runat="server" ID="hdnProyectoReporteID" Value='<%# Eval("ProyectoReporteID") %>' />
                    </td>                                
                    <td><asp:CheckBox runat="server" ID="chkReportePerzonalizado" /></td> 
                    <td><asp:TextBox runat="server" ID="txtRutaEspanol" Text='<%# Eval("RutaEspaniol") %>' /></td> 
                    <td><asp:TextBox runat="server" ID="txtRutaIngles" Text='<%# Eval("RutaIngles")%>' /></td> 
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td><asp:Label runat="server" ID="lblNombre" Text='<%# Eval("NombreInt")%>' />
                    <asp:HiddenField runat="server" ID="hdnTipoReporteID" Value='<%# Eval("TipoReporteProyectoID") %>' />  
                    <asp:HiddenField runat="server" ID="hdnProyectoReporteID" Value='<%# Eval("ProyectoReporteID") %>' />   
                    </td>                         
                    <td><asp:CheckBox runat="server" ID="chkReportePerzonalizado" /></td> 
                    <td><asp:TextBox runat="server" ID="txtRutaEspanol" Text='<%# Eval("RutaEspaniol") %>' /></td> 
                    <td><asp:TextBox runat="server" ID="txtRutaIngles" Text='<%# Eval("RutaIngles")%>' /></td> 
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        </table>
        </div>
        <p></p>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true" CssClass="boton" meta:resourcekey="btnGuardar" />
    </div>
</asp:Content>
