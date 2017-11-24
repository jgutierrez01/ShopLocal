<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true"
    CodeBehind="PendientesAutomaticos.aspx.cs" Inherits="SAM.Web.Proyectos.PendientesAutomaticos" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="lblTitulo" meta:resourcekey="lblTitulo" />
    <asp:HiddenField ID="hdnProyectoID" runat="server" />
    <div class="cntCentralForma">
        <div class="valGroupArriba">
            <br />
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" meta:resourcekey="valSummary" />
        </div>
        <br />
        <div>
            
            <table class="repSam ancho100" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col width="50%" />
                    <col width="50%" />
                </colgroup>
                <thead>
                    <tr class="repTitulos">
                        <th>
                            <asp:Literal runat="server" ID="litPendiente" meta:resourcekey="litPendiente" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litResponsable" meta:resourcekey="litResponsable" />
                        </th>
                    </tr>
                </thead>
                <asp:Repeater runat="server" ID="repPendientes" OnItemDataBound="repPendientes_ItemDataBound">
                    <ItemTemplate>
                        <tr class="repFila">
                            <td>
                                <asp:HiddenField ID="hdnTipoPendienteID" runat="server" />
                                <asp:Label runat="server" ID="lblPendiente" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="radResponsable" runat="server" Width="200px" Height="150px"
                                    OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" EnableLoadOnDemand="true"
                                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" CausesValidation="false" DropDownWidth="200px">
                                    <WebServiceSettings Method="ListaEmpleadosPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                                            <tr>
                                                <th>
                                                    <asp:Literal ID="litNombre" runat="server" meta:resourcekey="litNombre"></asp:Literal>
                                                </th>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </telerik:RadComboBox>
                                <span class="required">*</span>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <asp:HiddenField ID="hdnTipoPendienteID" runat="server" />
                                <asp:Label runat="server" ID="lblPendiente" />
                            </td>
                            <td>
                                <telerik:RadComboBox ID="radResponsable" runat="server" Width="200px" Height="150px"
                                    OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" EnableLoadOnDemand="true"
                                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" CausesValidation="false" DropDownWidth="200px">
                                    <WebServiceSettings Method="ListaEmpleadosPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                                            <tr>
                                                <th>
                                                    <asp:Literal ID="litNombre" runat="server" meta:resourcekey="litNombre"></asp:Literal>
                                                </th>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </telerik:RadComboBox>
                                <span class="required">*</span>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <p>
        </p>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true"
            CssClass="boton" meta:resourcekey="btnGuardar" />
    </div>
</asp:Content>
