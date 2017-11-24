<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.Master" AutoEventWireup="true" CodeBehind="MovimientosInventario.aspx.cs" Inherits="SAM.Web.Materiales.MovimientosInventario" %>
<%@ MasterType VirtualPath="~/Masters/Materiales.Master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rbNuevo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phTxtSegmento" />
                    <telerik:AjaxUpdatedControl ControlID="phSegmento" />
                    <telerik:AjaxUpdatedControl ControlID="rbNuevo" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="rbExistente" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rbExistente">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phTxtSegmento" />
                    <telerik:AjaxUpdatedControl ControlID="phSegmento" />
                    <telerik:AjaxUpdatedControl ControlID="rbNuevo" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="rbExistente" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="rbEntrada">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlTipo" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="pnlEntrada" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="pnlSalida" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="pnlAccesorio" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="rbEntrada" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="rbSalida" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="rbSalida">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlTipo" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="pnlEntrada" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="pnlSalida" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="pnlAccesorio" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="rbEntrada" UpdatePanelRenderMode="Inline" />
                    <telerik:AjaxUpdatedControl ControlID="rbSalida" UpdatePanelRenderMode="Inline" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina runat="server" ID="titulo" NavigateUrl="~/Materiales/MatDefault.aspx"
        meta:resourcekey="lblMovimientos" />
    <div class="cntCentralForma">
        <br />
        <div class="cajaFiltros">
            <uc2:Filtro runat="server" ID="filtroGenerico" FiltroProyecto="true" FiltroNumeroUnico="true"
                FiltroNumeroControl="false" FiltroOrdenTrabajo="false" ProyectoHeaderID="proyEncabezado"
                ProyectoAutoPostBack="true" ProyectoRequerido="true" NumeroUnicoRequerido="true">
            </uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button runat="server" ID="btnMostrar" OnClick="btnMostrar_Click" CssClass="boton"
                        meta:resourcekey="btnMostrar" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <br />
        <sam:Header ID="proyEncabezado" runat="server" Visible="False" />
        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" DisplayMode="BulletList"
            meta:resourcekey="valSummary" />
        <asp:Panel runat="server" ID="pnlInformacion" Visible="false">
            <div class="cajaFiltros">
                <div class="divIzquierdo ancho50">
                    <asp:Label runat="server" ID="lblItemCode" CssClass="bold" meta:resourcekey="lblItemCode"></asp:Label>
                    <asp:Label runat="server" ID="lblItemCodeData" CssClass=""></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblDescripcion" CssClass="bold" meta:resourcekey="lblDescripcion"></asp:Label>
                    <asp:Label runat="server" ID="lblDescripcionData" CssClass=""></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblDiametro1" CssClass="bold" meta:resourcekey="lblDiametro1"></asp:Label>
                    <asp:Label runat="server" ID="lblDiametro1Data" CssClass=""></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblDiametro2" CssClass="bold" meta:resourcekey="lblDiametro2"></asp:Label>
                    <asp:Label runat="server" ID="lblDiametro2Data" CssClass=""></asp:Label>
                    <br />
                </div>
                <div class="divIzquierdo ancho45">
                    <asp:Label runat="server" ID="lblInvFisico" CssClass="bold" meta:resourcekey="lblInvFisico"></asp:Label>
                    <asp:Label runat="server" ID="lblInvFisicoData" CssClass=""></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblInvCongelado" CssClass="bold" meta:resourcekey="lblInvCongelado"></asp:Label>
                    <asp:Label runat="server" ID="lblInvCongeladoData" CssClass=""></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblInvDanado" CssClass="bold" meta:resourcekey="lblInvDanado"></asp:Label>
                    <asp:Label runat="server" ID="lblInvDanadoData" CssClass=""></asp:Label>
                    <br />
                    <asp:Label runat="server" ID="lblInvDisponible" CssClass="bold" meta:resourcekey="lblInvDisponible"></asp:Label>
                    <asp:Label runat="server" ID="lblInvDisponibleData" CssClass=""></asp:Label>
                    <br />
                </div>
                <p>
                </p>
            </div>
            <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
                <div class="divIzquierdo ancho30">
                    <div class="separador">
                        <asp:Label runat="server" ID="lblClasificacion" CssClass="bold inline" meta:resourcekey="lblClasificacion" />
                        <br />
                        <asp:RadioButton runat="server" ID="rbEntrada" CssClass="inline" GroupName="gnClasificacion"
                            OnCheckedChanged="rbEntrada_CheckedChanged" AutoPostBack="true" CausesValidation="false"
                            meta:resourcekey="rbEntrada" />
                        <asp:RadioButton runat="server" ID="rbSalida" CssClass="inline" GroupName="gnClasificacion"
                            OnCheckedChanged="rbSalida_CheckedChanged" AutoPostBack="true" CausesValidation="false"
                            meta:resourcekey="rbSalida" />
                    </div>
                </div>
                <div class="divIzquierdo ancho40">
                    <div class="separador">
                        <asp:Label runat="server" ID="lblTipo" CssClass="bold" meta:resourcekey="lblTipo" />
                        <br />
                        <asp:DropDownList ID="ddlTipo" runat="server" CausesValidation="false" CssClass="inline" />
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator ID="rfvTipo" runat="server" Display="None" ControlToValidate="ddlTipo"
                            ValidationGroup="vgMovimientos" meta:resourcekey="rfvTipo" InitialValue="" />
                    </div>
                </div>
                <p>
                </p>
                <div class="divIzquierdo ancho70">
                    <%--Tipo de panel = 1 (Lo utilizo para saber cuando vaciar sus controles--%>
                    <asp:Panel runat="server" ID="pnlAccesorio" Visible="false">
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox runat="server" ID="txtCantidadA" MaxLength="10" ValidationGroup="vgMovimientos"
                                meta:resourcekey="lblCantidad" />
                            <asp:RegularExpressionValidator runat="server" ID="revCantidadA" ControlToValidate="txtCantidadA"
                                Display="None" ValidationExpression="\d*" ValidationGroup="vgMovimientos" meta:resourcekey="revCantidad" />
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox runat="server" ID="txtReferenciaA" MaxLength="50" ValidationGroup="vgMovimientos"
                                meta:resourcekey="lblReferencia" />
                        </div>
                    </asp:Panel>
                </div>
                <p>
                </p>
                <div class="divIzquierdo ancho70">
                    <%--Tipo de panel = 2 (Lo utilizo para saber cuando vaciar sus controles--%>
                    <asp:Panel runat="server" ID="pnlSalida" Visible="false">
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox runat="server" ID="txtCantidadS" MaxLength="10" ValidationGroup="vgMovimientos"
                                meta:resourcekey="lblCantidad" />
                            <asp:RegularExpressionValidator runat="server" ID="revCantidadS" ControlToValidate="txtCantidadS"
                                Display="None" ValidationExpression="\d*" ValidationGroup="vgMovimientos" meta:resourcekey="revCantidad" />
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox runat="server" ID="txtReferenciaS" MaxLength="50" ValidationGroup="vgMovimientos"
                                meta:resourcekey="lblReferencia" />
                        </div>
                        <asp:CustomValidator runat="server" ID="cvSelectedRow" OnServerValidate="cvSelectedRow_ServerValidate"
                            Display="None" ValidationGroup="vgMovimientos" meta:resourcekey="cvSelectedRow" />
                        <mimo:MimossRadGrid ID="grdSegmentos" runat="server" AllowFilteringByColumn="false"
                            AllowPaging="false" Height="250" AllowMultiRowSelection="false" >
                            <ClientSettings>
                                <Selecting AllowRowSelect="true" />
                            </ClientSettings>
                            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false" DataKeyNames="NumeroUnicoSegmentoID">
                                <CommandItemTemplate>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="chkSelect_h" HeaderStyle-Width="30" />
                                    <telerik:GridBoundColumn UniqueName="NumeroUnicoSegmentoID" DataField="NumeroUnicoSegmentoID"
                                        Visible="false" />
                                    <telerik:GridBoundColumn UniqueName="Segmento" DataField="Segmento" HeaderStyle-Width="90"
                                        Groupable="false" meta:resourcekey="gbcSegmento" />
                                    <telerik:GridBoundColumn UniqueName="InventarioFisico" DataField="InventarioFisico"
                                        HeaderStyle-Width="90" Groupable="false" meta:resourcekey="gbcInventarioFisico" />
                                    <telerik:GridBoundColumn UniqueName="InventarioCongelado" DataField="InventarioCongelado"
                                        HeaderStyle-Width="90" Groupable="false" meta:resourcekey="gbcInventarioCongelado" />
                                    <telerik:GridBoundColumn UniqueName="InventarioDisponibleCruce" DataField="InventarioDisponibleCruce"
                                        HeaderStyle-Width="90" Groupable="false" meta:resourcekey="gbcDisponibleCruce" />
                                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                                        Reorderable="false" ShowSortIcon="false">
                                        <ItemTemplate>
                                            &nbsp;
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </mimo:MimossRadGrid>
                        <p>
                        </p>
                    </asp:Panel>
                </div>
                <p>
                </p>
                <div class="divIzquierdo ancho70">
                    <%--Tipo de panel = 3 (Lo utilizo para saber cuando vaciar sus controles--%>
                    <asp:Panel runat="server" ID="pnlEntrada" Visible="false">
                        <div class="separador">
                            <asp:Label runat="server" ID="lblSegmento" CssClass="bold inline" meta:resourcekey="lblSegmento" />
                            <br />
                            <asp:RadioButton runat="server" ID="rbNuevo" Checked="true" CssClass="inline" GroupName="gnSegmento"
                                OnCheckedChanged="rbNuevo_CheckedChanged" AutoPostBack="true" meta:resourcekey="rbNuevo" />
                            <asp:RadioButton runat="server" ID="rbExistente" CssClass="inline" GroupName="gnSegmento"
                                OnCheckedChanged="rbExistente_CheckedChanged" AutoPostBack="true" meta:resourcekey="rbExistente" />
                        </div>
                        <div class="separador">
                            <asp:Panel runat="server" ID="phTxtSegmento">
                                <div class="separador">
                                    <asp:Label runat="server" ID="lblSeg" CssClass="bold" meta:resourcekey="lblSegmento" /><br />
                                    <asp:TextBox runat="server" ID="txtSegmento" MaxLength="1" CssClass="required" />
                                    <span class="required">*</span>
                                    <asp:RegularExpressionValidator runat="server" ID="revSegmento" ControlToValidate="txtSegmento"
                                        Display="None" ValidationExpression="[A-Z]" ValidationGroup="vgMovimientos" meta:resourcekey="revSegmento" />
                                    <asp:CustomValidator runat="server" ID="cvSegmento" Display="None" OnServerValidate="cvSegmento_ServerValidate"
                                        ValidationGroup="vgMovimientos" />
                                </div>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="phSegmento" Visible="false">
                                <div class="separador">
                                    <asp:Label runat="server" ID="lblSegmento2" CssClass="bold" meta:resourcekey="lblSegmento" />
                                    <asp:DropDownList ID="ddlSegmento" runat="server" CausesValidation="false" />
                                    <span class="required">*</span>
                                </div>
                            </asp:Panel>
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox runat="server" ID="txtCantidadE" MaxLength="50" ValidationGroup="vgMovimientos"
                                meta:resourcekey="lblCantidad" />
                            <asp:RegularExpressionValidator runat="server" ID="revCantidad" ControlToValidate="txtCantidadE"
                                Display="None" ValidationExpression="\d*" ValidationGroup="vgMovimientos" meta:resourcekey="revCantidad" />
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox runat="server" ID="txtReferenciaE" MaxLength="50" ValidationGroup="vgMovimientos"
                                meta:resourcekey="lblReferencia" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro" style="margin-top: 23px;">
                    <div class="validacionesHeader">
                    </div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummaryMovimientos" EnableClienteScript="true"
                            DisplayMode="BulletList" CssClass="summary" ValidationGroup="vgMovimientos" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            </div>
        </asp:Panel>
        <p>
        </p>
        <asp:PlaceHolder ID="phAccesorio" runat="server" Visible="false">
        <div class="infoJuntas center">
            <table class="repSam" cellpadding="0" cellspacing="0">
                <colgroup>
                    <col width="30" />
                    <col width="150" />
                    <col width="150" />
                    <col width="150" />
                    <col width="150" />
                    <col width="150" />
                    <col width="150" />
                </colgroup>
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="8">
                            <asp:Literal runat="server" ID="litMovimientosInventario" meta:resourcekey="litMovimientosInventario" />
                        </th>
                    </tr>
                    <tr class="repTitulos">
                        <th>
                            &nbsp;
                        </th>
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
                                <asp:HiddenField ID="hdnNumMovimientoID" runat="server" />
                                <asp:ImageButton ID="btnEliminarMovimiento" Visible="false" runat="server" ImageUrl="~/Imagenes/Iconos/borrar.png" OnClick="btnEliminarMovimiento_OnClick" OnClientClick="return Sam.Confirma(23);"></asp:ImageButton>
                            </td>
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
                                <asp:HiddenField ID="hdnNumMovimientoID" runat="server" />
                                <asp:ImageButton ID="btnEliminarMovimiento" Visible="false" runat="server" ImageUrl="~/Imagenes/Iconos/borrar.png" OnClick="btnEliminarMovimiento_OnClick" OnClientClick="return Sam.Confirma(23);"></asp:ImageButton>
                            </td>
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
                        <td colspan="3">
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
        <asp:PlaceHolder ID="phTubo" runat="server" Visible="false">
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
                    <col width="30" />
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
                        <th colspan="8">
                            <asp:Literal runat="server" ID="litMovimientos" meta:resourcekey="litMovimientosInventario" />
                        </th>
                    </tr>
                    <tr class="repTitulos">
                        <th>
                            &nbsp;
                        </th>
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
                                <asp:HiddenField ID="hdnNumMovimientoID" runat="server" />
                                <asp:ImageButton ID="btnEliminarMovimiento" Visible="false" runat="server" ImageUrl="~/Imagenes/Iconos/borrar.png" OnClick="btnEliminarMovimiento_OnClick" OnClientClick="return Sam.Confirma(23);"></asp:ImageButton>
                            </td>
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
                                <asp:HiddenField ID="hdnNumMovimientoID" runat="server" />
                                <asp:ImageButton ID="btnEliminarMovimiento" Visible="false" runat="server" ImageUrl="~/Imagenes/Iconos/borrar.png" OnClick="btnEliminarMovimiento_OnClick" OnClientClick="return Sam.Confirma(23);"></asp:ImageButton>
                            </td>
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
                        <td colspan="4">
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
    </div>
  

    <asp:PlaceHolder runat="server" ID="phGuardar" Visible="false">
        <div class="pestanaBoton">
            <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CausesValidation="true"
                ValidationGroup="vgMovimientos" CssClass="boton" meta:resourcekey="btnGuardar" />
        </div>
    </asp:PlaceHolder>
    
</asp:Content>
