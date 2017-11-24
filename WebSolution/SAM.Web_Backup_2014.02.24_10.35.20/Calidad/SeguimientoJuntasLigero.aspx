<%@ Page  Language="C#" MasterPageFile="~/Masters/Calidad.Master" AutoEventWireup="true" CodeBehind="SeguimientoJuntasLigero.aspx.cs" Inherits="SAM.Web.Calidad.SeguimientoJuntasLigero" %>
<%@ MasterType VirtualPath="~/Masters/Calidad.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" tagname="BarraTituloPagina" tagprefix="sam" %>
<%@ Register Src="~/Controles/Navegacion/Paginador.ascx" tagname="Paginador" tagprefix="pag" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
    <style type="text/css" media="screen">
        .RadMenu ul.rmActive
        {
            max-height: 260px;
            overflow-x:hidden;
            overflow-y:auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina ID="barraTitulo" runat="server" NavigateUrl="~/Calidad/FiltrosSeguimientoJunta.aspx" meta:resourcekey="lblTitulo" />

    <telerik:RadAjaxManager runat="server" ID="ajxMgr" ClientEvents-OnResponseEnd="Sam.Seguimientos.AjaxRequestEnd">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="pnlFiltros">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlFiltros" LoadingPanelID="loading"/>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="contenedorCentral" style="height:500px;">               
        <sam:Header ID="proyHeader" runat="server" />
        
        <telerik:RadContextMenu runat="server" ID="menuColumnas" OnItemClick="menuColumnas_ItemClick" OnClientItemClicked="Sam.Seguimientos.ItemMenuClicked" CausesValidation="false">
            <Items>
                <telerik:RadMenuItem runat="server" meta:resourcekey="itmOrdenarAscendente" PostBack="true" Value="ordAsc" />
                <telerik:RadMenuItem runat="server" meta:resourcekey="itmOrdenarDescendente" PostBack="true" Value="ordDesc" />
                <telerik:RadMenuItem runat="server" meta:resourcekey="itmQuitarOrdenamiento" PostBack="true" Value="remove" />
                <telerik:RadMenuItem runat="server" meta:resourcekey="itmCongelar" PostBack="true" Value="freeze" />
            </Items>
        </telerik:RadContextMenu>

        <telerik:RadWindow runat="server" ID="wndFiltros">
            <ContentTemplate>
                <div>
                    <h4><asp:Literal runat="server" ID="litPrioridad" meta:resourcekey="litFiltros" /></h4>
                    <asp:Panel runat="server" ID="pnlFiltros" CssClass="pnlFiltros">
                        <telerik:RadFilter runat="server" ID="filtro" ShowApplyButton="false" OnPreRender="filtro_PreRender"/>
                        <p></p>
                    </asp:Panel>
                    <div class="separador" style="padding-left:5px;">
                        <asp:Button runat="server" CssClass="boton" ID="btnFiltrar" OnClick="btnFiltrar_Click" CausesValidation="false" meta:resourcekey="btnFiltrar" />
                    </div>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>

        <asp:HiddenField runat="server" ID="hdnMenuColumnasID" ClientIDMode="Static" />
        <asp:HiddenField runat="server" ID="hdnColumnaSeleccionada" ClientIDMode="Static" />

        <telerik:RadAjaxLoadingPanel runat="server" ID="loading" />
        <asp:Panel runat="server" ID="panel">
            <div id="ocultoInicio">
                <table class="layout" cellpadding="0" cellspacing="0">
                    <tbody>
                    <tr>
                        <td colspan="2">
                            <table class="repSam" cellpadding="0" cellspacing="0" width="100%">
                                <thead>
                                    <tr class="repEncabezado">
                                        <th>
                                            <div class="comandosEncabezado">
                                            <span class="iconoDerecha">
                                                <asp:LinkButton runat="server" ID="lnkMostrarFiltros" meta:resourcekey="lnkFiltrar" />
                                                <asp:ImageButton runat="server" ID="btnMostrarFiltros" UseSubmitBehavior="false" CssClass="iconoFiltro" ImageUrl="~/Imagenes/Iconos/icono_certificar.png" />
                                                </span>
                                                <span class="iconoDerecha">
                                                <asp:Hyperlink runat="server" ID="lnkExportarExcel" meta:resourcekey="lnkExportar" />
                                                <asp:Hyperlink runat="server" ID="btnExportarExcel" CssClass="iconoFiltro" ImageUrl="~/Imagenes/Iconos/excel.png" />
                                                </span>
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="scrollHeaders congelados" id="topLeft">
                                <table class="repSam" cellpadding="0" cellspacing="0">
                                    <thead>
                                        <tr class="repTitulos">
                                            <asp:Repeater runat="server" ID="repGruposTitulosCongelados" EnableViewState="false">
                                                <HeaderTemplate>
                                                    <th class="icoVer">&nbsp;</th>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <th align="center" colspan='<%#Eval("Colspan")%>'><%#Eval("Nombre")%></th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                        <tr class="repTitulos altos">
                                            <asp:Repeater runat="server" ID="repTitulosCongelados" EnableViewState="false">
                                                <HeaderTemplate>
                                                    <th class="icoVer">&nbsp;</th>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <th style='width:<%#Eval("AnchoColumna")%>px;'>
                                                        <span class="spHeader" style='width:<%#Eval("AnchoColumna")%>px;' data='<%#Eval("NombreColumnaSp")%>'>
                                                            <%#Eval("NombreCampo")%>
                                                        </span>
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div class="scrollHeaders" id="tblHeaders">
                                <table class="repSam" cellpadding="0" cellspacing="0">
                                    <thead>
                                        <tr class="repTitulos">
                                            <asp:Repeater runat="server" ID="repGruposTitulos" EnableViewState="false">
                                                <ItemTemplate>
                                                    <th align="center" colspan='<%#Eval("Colspan")%>'><%#Eval("Nombre")%></th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <th style="width:15px;">&nbsp;</th>
                                        </tr>
                                        <tr class="repTitulos altos">
                                            <asp:Repeater runat="server" ID="repTitulos" EnableViewState="false">
                                                <ItemTemplate>
                                                    <th data='<%#Eval("NombreColumnaSp")%>'>
                                                        <span class="spHeader" style='width:<%#Eval("AnchoColumna")%>px;' data='<%#Eval("NombreColumnaSp")%>'>
                                                            <%#Eval("NombreCampo")%>
                                                        </span>
                                                    </th>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            <th style="width:15px;">&nbsp;</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="scrollTblVertical congelados" id="tblCongelados">
                                <table class="repSam" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <asp:Repeater ID="repJuntasCongeladas" runat="server" OnItemDataBound="repJuntasCongeladas_OnItemDataBound">
                                            <ItemTemplate>
                                                <tr class="repFila">
                                                    <asp:Repeater runat="server" ID="repColumnasCongeladas" EnableViewState="false" OnItemDataBound="repColumnasCongeladas_OnItemDataBound">
                                                        <HeaderTemplate>
                                                            <td class="icoVer">
                                                                <asp:HyperLink ImageUrl="~/Imagenes/Iconos/info.png" runat="server" ID="hlPopUp" meta:resourcekey="imgDetalle" NavigateUrl="#" />
                                                            </td>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <td style='width:<%#Eval("AnchoColumna")%>px;'>
                                                                <span class="ellipsis" style='width:<%#Eval("AnchoColumna")%>px;'>
                                                                    <asp:Literal runat="server" ID="litColumna"  EnableViewState="false" />
                                                                </span>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="repFilaPar">
                                                    <asp:Repeater runat="server" ID="repColumnasCongeladas" EnableViewState="false" OnItemDataBound="repColumnasCongeladas_OnItemDataBound">
                                                        <HeaderTemplate>
                                                            <td class="icoVer">
                                                                <asp:HyperLink ImageUrl="~/Imagenes/Iconos/info.png" runat="server" ID="hlPopUp" meta:resourcekey="imgDetalle" NavigateUrl="#" />
                                                            </td>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <td style='width:<%#Eval("AnchoColumna")%>px;'>
                                                                <span class="ellipsis" style='width:<%#Eval("AnchoColumna")%>px;'>
                                                                    <asp:Literal runat="server" ID="litColumna"  EnableViewState="false" />
                                                                </span>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <FooterTemplate>
                                                <tr class="repFila">
                                                    <td class="icoVer">&nbsp;</td>
                                                    <asp:Literal runat="server" ID="thFooter" />
                                                </tr>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div class="scrollTblVertical" id="tblBody">
                                <table class="repSam" cellpadding="0" cellspacing="0">
                                    <tbody>
                                        <asp:Repeater ID="repJuntas" runat="server" OnItemDataBound="repJuntas_OnItemDataBound">
                                            <ItemTemplate>
                                                <tr class="repFila">
                                                    <asp:Repeater runat="server" ID="repColumnas" EnableViewState="false" OnItemDataBound="repColumnas_OnItemDataBound">
                                                        <ItemTemplate>
                                                            <td>
                                                                <span class="ellipsis" style='width:<%#Eval("AnchoColumna")%>px;'>
                                                                    <asp:Literal runat="server" ID="litColumna"  EnableViewState="false" />
                                                                </span>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="repFilaPar">
                                                    <asp:Repeater runat="server" ID="repColumnas" EnableViewState="false" OnItemDataBound="repColumnas_OnItemDataBound">
                                                        <ItemTemplate>
                                                            <td>
                                                                <span class="ellipsis" style='width:<%#Eval("AnchoColumna")%>px;'>
                                                                    <asp:Literal runat="server" ID="litColumna"  EnableViewState="false" />
                                                                </span>
                                                            </td>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </tr>
                                            </AlternatingItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </td>
                    </tr>
                    </tbody>
                </table>
                <pag:Paginador runat="server" ID="pager" TamanioPagina="20" OnPaginaCambio="pager_PaginaCambio" MuestraPanelCargando="true" />
            </div>
        </asp:Panel>
    </div>
    <script type="text/javascript" language="javascript">
        $(function () {
            Sam.Seguimientos.AttachHandlers();
            Sam.Seguimientos.MuestraGrid();
        });
    </script>
</asp:Content>