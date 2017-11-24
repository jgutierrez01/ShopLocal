<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpMovimientoInventarios.aspx.cs" Inherits="SAM.Web.Materiales.PopUpMovimientoInventarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="lblMovimientosInventario" meta:resourcekey="lblMovimientosInventario"></asp:Literal>
    </h4>
    <div class="cajaAzul">
        <div class="divIzquierdo ancho50">
            <div class="separadorDashboard">
                <asp:Label ID="lblNumeroUnicoTitulo" runat="server" CssClass="bold" meta:resourcekey="lblNumeroUnicoTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblNumeroUnico" runat="server"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblDiametro1Titulo" runat="server" CssClass="bold" meta:resourcekey="lblDiametro1Titulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDiametro1" runat="server"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblProfile1Titulo" runat="server" CssClass="bold" meta:resourcekey="lblProfile1Titulo"></asp:Label>&nbsp;
                <asp:Label ID="lblProfile1" runat="server"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblCedulaTitulo" runat="server" CssClass="bold" meta:resourcekey="lblCedulaTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblCedula" runat="server"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblInventarioFisicoTitulo" runat="server" CssClass="bold" meta:resourcekey="lblTotalRecibidoTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblTotalRecibido" runat="server"></asp:Label>
            </div>
             <div class="separadorDashboard">
                <asp:Label ID="lblInventarioDanadoTitulo" runat="server" CssClass="bold" meta:resourcekey="lblTotalDanadoTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblTotalDanado" runat="server"></asp:Label>
            </div>
           
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separadorDashboard">
                <asp:Label ID="lblItemCodeTitulo" runat="server" CssClass="bold" meta:resourcekey="lblItemCodeTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblItemCode" runat="server"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblDiametro2Titulo" runat="server" CssClass="bold" meta:resourcekey="lblDiametro2Titulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDiametro2" runat="server"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblProfile2Titulo" runat="server" CssClass="bold" meta:resourcekey="lblProfile2Titulo"></asp:Label>&nbsp;
                <asp:Label ID="lblProfile2" runat="server"></asp:Label>
            </div>
             <div class="separadorDashboard">
                <asp:Label ID="lblInventarioBuenEstadoTitulo" runat="server" CssClass="bold" meta:resourcekey="lblTotalEntradasTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblTotalEntradas" runat="server"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblInventarioEnTransferenciaTitulo" runat="server" CssClass="bold" meta:resourcekey="lblTotalSalidasTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblTotalSalidas" runat="server"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblInventarioCongeladoTitulo" runat="server" CssClass="bold" meta:resourcekey="lblSaldoActualTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblSaldoActual" runat="server"></asp:Label>
            </div>            
        </div>
        <p>
        </p>
    </div>
    <p>
    </p>
    <asp:PlaceHolder ID="phAccesorio" runat="server">
        <div class="infoJuntas center">
            <table class="repSam" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col width="150" />
                    <col width="150" />
                    <col width="150" />
                    <col width="150" />
                    <col width="150" />
                    <col width="150" />
                </colgroup>
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="7">
                            <asp:Literal runat="server" ID="litMovimientosInventario" meta:resourcekey="litMovimientosInventario" />
                        </th>
                    </tr>
                    <tr class="repTitulos">
                        <th>
                            <asp:Literal runat="server" ID="litFecha" meta:resourcekey="litFecha" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litMovimiento" meta:resourcekey="litMovimiento" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litEntradaPieza" meta:resourcekey="litEntradaPieza" />
                        </th>                        
                        <th>
                            <asp:Literal runat="server" ID="litSalidaPieza" meta:resourcekey="litSalidaPieza" />
                        </th>                        
                        <th>
                            <asp:Literal runat="server" ID="litSaldoPieza" meta:resourcekey="litSaldoPieza" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litReferencia" meta:resourcekey="litReferencia" />
                        </th>
                    </tr>
                </thead>
                <asp:Repeater runat="server" ID="repAccesorio" OnItemDataBound="repAccesorio_ItemDataBound">
                    <ItemTemplate>
                        <tr class="repFila">
                            <td>
                                <%#Eval("FechaMovimiento")%>
                            </td>
                            <td>
                                <asp:Literal ID="litMov" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="ltEntrada" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="ltSalida" runat="server"></asp:Literal>
                            </td>                            
                            <td>
                                <asp:Literal ID="ltSaldo" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <%#Eval("Referencia")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <%#Eval("FechaMovimiento")%>
                            </td>
                            <td>
                                <asp:Literal ID="litMov" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="ltEntrada" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="ltSalida" runat="server"></asp:Literal>
                            </td>                            
                            <td>
                                <asp:Literal ID="ltSaldo" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <%#Eval("Referencia")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
                <tfoot>
                    <tr class="repPie">
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td> <asp:Literal ID="litEntradaTemp" runat="server" meta:resourcekey="litEntradaTemp"></asp:Literal></td>
                        <td><asp:Literal ID="litSalidaTemp" runat="server" meta:resourcekey="litSalidaTemp"></asp:Literal></td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder ID="phTubo" runat="server">
        <div class="infoJuntas center">
            <table class="repSam" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col width="50" />
                    <col width="50" />
                    <col width="100" />
                    <col width="100" />
                    <col width="100" />
                    <col width="100" />
                    <col width="100" />
                    <col width="100" />
                    <col width="100" />
                </colgroup>
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="8">
                            <asp:Literal runat="server" ID="litSegmentos" meta:resourcekey="litSemmentos" />
                        </th>
                    </tr>
                    <tr class="repTitulos">
                        <th>
                            <asp:Literal runat="server" ID="litSegmento" meta:resourcekey="litSegmento" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litRack" Text="Rack" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litInvFisico" meta:resourcekey="litRecibido" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litInvDanado" meta:resourcekey="litInvDanado" />
                        </th>
                       
                        <th>
                            <asp:Literal runat="server" ID="Literal5" meta:resourcekey="litEntradas" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="Literal6" meta:resourcekey="litSalidas" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="Literal1" meta:resourcekey="litSalidasTemporales" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litInvBuenEstado" meta:resourcekey="litInvBuenEstado" />
                        </th>
                        <%--<th>
                            <asp:Literal runat="server" ID="Literal7" meta:resourcekey="litInvDisponible" />
                        </th>--%>
                    </tr>
                </thead>
                <asp:Repeater runat="server" ID="repSegmentos" OnItemDataBound="repSegmentos_ItemDataBound">
                    <ItemTemplate>
                        <tr class="repFila">
                            <td>
                                <%#Eval("Segmento")%>
                            </td>
                            <td>
                                <%#Eval("Rack")%>
                            </td>
                            <td>
                                <asp:Literal ID="litRecibido" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <%#Eval("CantidadDanada")%>
                            </td>                            
                            <td>                                
                                <asp:Literal ID="litTotalEntradas" runat="server"></asp:Literal>
                            </td>
                             <td>                                
                                <asp:Literal ID="litTotalSalidas" runat="server"></asp:Literal>
                            </td>
                             <td>                                
                                <asp:Literal ID="litTotalSalidasTemporales" runat="server"></asp:Literal>
                            </td>
                            <td>                                
                                <asp:Literal ID="litTotalSaldos" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <%#Eval("Segmento")%>
                            </td>
                            <td>
                                <%#Eval("Rack")%>
                            </td>
                             <td>
                                <asp:Literal ID="litRecibido" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <%#Eval("CantidadDanada")%>
                            </td>                            
                            <td>                                
                                <asp:Literal ID="litTotalEntradas" runat="server"></asp:Literal>
                            </td>
                             <td>                                
                                <asp:Literal ID="litTotalSalidas" runat="server"></asp:Literal>
                            </td>
                             <td>                                
                                <asp:Literal ID="litTotalSalidasTemporales" runat="server"></asp:Literal>
                            </td>
                            <td>                                
                                <asp:Literal ID="litTotalSaldos" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
                <tfoot>
                    <tr class="repPie">
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
        <p>
        </p>
        <div class="infoJuntas center">
            <table class="repSam" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col width="150" />
                    <col width="150" />
                    <col width="50" />
                    <col width="90" />
                    <col width="90" />
                    <col width="90" />
                    <col width="90" />
                </colgroup>
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="7">
                            <asp:Literal runat="server" ID="litMovimientos" meta:resourcekey="litMovimientosInventario" />
                        </th>
                    </tr>
                    <tr class="repTitulos">
                        <th>
                            <asp:Literal runat="server" ID="litFechaTubo" meta:resourcekey="litFecha" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litMovimientoSegmento" meta:resourcekey="litMovimiento" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litSegmentoGrid" meta:resourcekey="litSegmento" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litEntradaSegmento" meta:resourcekey="litEntrada" />
                        </th>                        
                        <th>
                            <asp:Literal runat="server" ID="litSalidaSegmento" meta:resourcekey="litSalida" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litSaldoSegmento" meta:resourcekey="litSaldo" />
                        </th>
                        <th>
                            <asp:Literal runat="server" ID="litReferenciaSegmento" meta:resourcekey="litReferencia" />
                        </th>
                    </tr>
                </thead>
                <asp:Repeater runat="server" ID="repTubo" OnItemDataBound="repTubo_ItemDataBound">
                    <ItemTemplate>
                        <tr class="repFila">
                            <td>
                                <%#Eval("FechaMovimiento")%>
                            </td>
                            <td>
                                <asp:Literal ID="litMov" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <%#Eval("Segmento")%>
                            </td>
                            <td>
                                <asp:Literal ID="ltEntrada" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="ltSalida" runat="server"></asp:Literal>
                            </td>                            
                            <td>
                                <asp:Literal ID="ltSaldo" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <%#Eval("Referencia")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr class="repFilaPar">
                            <td>
                                <%#Eval("FechaMovimiento")%>
                            </td>
                            <td>
                                <asp:Literal ID="litMov" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <%#Eval("Segmento")%>
                            </td>
                            <td>
                                <asp:Literal ID="ltEntrada" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="ltSalida" runat="server"></asp:Literal>
                            </td>                            
                            <td>
                                <asp:Literal ID="ltSaldo" runat="server"></asp:Literal>
                            </td>
                            <td>
                                <%#Eval("Referencia")%>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                </asp:Repeater>
                <tfoot>
                    <tr class="repPie">
                        <td colspan="3">
                            &nbsp;
                        </td>
                         <td><asp:Literal ID="litEntradaTemp2" runat="server" meta:resourcekey="litEntradaTemp"></asp:Literal></td>
                        <td><asp:Literal ID="litSalidaTemp2" runat="server" meta:resourcekey="litSalidaTemp"></asp:Literal></td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </tfoot>
            </table>
        </div>
    </asp:PlaceHolder>
</asp:Content>
