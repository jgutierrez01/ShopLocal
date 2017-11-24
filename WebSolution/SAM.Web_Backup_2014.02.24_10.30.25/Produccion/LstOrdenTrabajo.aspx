<%@ Page Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="LstOrdenTrabajo.aspx.cs" Inherits="SAM.Web.Produccion.LstOrdenTrabajo" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlPatio">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlProyecto" />
                    <telerik:AjaxUpdatedControl ControlID="ddlTaller" />
                    <telerik:AjaxUpdatedControl ControlID="valPatio" />
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdOrdenTrabajo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdOrdenTrabajo" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label ID="lblListaOrdenTrabajo" meta:resourcekey="lblListaOrdenTrabajo" runat="server" CssClass="Titulo" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros" style="margin-bottom:5px;">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblPatio" runat="server" meta:resourcekey="lblPatio" AssociatedControlID="ddlPatio" />
                    <mimo:MappableDropDown runat="server" ID="ddlPatio" meta:resourcekey="ddlPatio" AutoPostBack="true" OnSelectedIndexChanged="ddlPatio_SelectedIndexChange" />
                    <asp:CustomValidator    meta:resourcekey="cusPatioProyecto"
                                            runat="server" 
                                            ID="cusPatioProyecto" 
                                            Display="None"
                                            ClientValidationFunction="Sam.Produccion.Validaciones.PatioProyectoRequerido"
                                            OnServerValidate="cusPatioProyecto_ServerValidate"
                                            ValidationGroup="vgPatio"
                                            ControlToValidate="ddlPatio"
                                            ValidateEmptyText="true" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label runat="server" ID="lblProyecto" meta:resourcekey="lblProyecto" AssociatedControlID="ddlProyecto" />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" meta:resourcekey="ddlProyecto" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChange" AutoPostBack="true" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblTaller" runat="server" meta:resourcekey="lblTaller" AssociatedControlID="ddlTaller" />
                    <mimo:MappableDropDown runat="server" ID="ddlTaller" meta:resourcekey="ddlTaller" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button meta:resourcekey="btnMostrar" CssClass="boton" runat="server" ID="btnMostrar" Text="Mostrar" OnClick="btnMostrar_Click" ValidationGroup="vgPatio" />
                </div>
            </div>
            <p></p>
        </div>
        <asp:ValidationSummary runat="server" ID="valPatio" ValidationGroup="vgPatio" CssClass="summaryList" meta:resourcekey="valPatio" />
        <div class="separador">
            <sam:Header ID="headerProyecto" runat="server" Visible="False" />
        </div>
        <asp:PlaceHolder ID="phLista" runat="server" Visible="false">
            <p></p>
            <asp:ValidationSummary ID="valSummary" runat="server" ValidationGroup="valLista" CssClass="summaryList" meta:resourcekey="valSummary" />
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdOrdenTrabajo" OnNeedDataSource="grdOrdenTrabajo_OnNeedDataSource" OnItemCommand="grdOrdenTrabajo_ItemCommand" OnItemDataBound="grdOrdenTrabajo_ItemDataBound">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="OrdenDeTrabajoID">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <samweb:AuthenticatedHyperLink meta:resourcekey="lnkAgregar" runat="server" ID="lnkAgregar" Text="Agregar" NavigateUrl="~/Produccion/NuevaOdt.aspx" CssClass="link" />
                            <samweb:AuthenticatedHyperLink meta:resourcekey="imgAgregar" runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Produccion/NuevaOdt.aspx" />
                            <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar" CssClass="link" OnClick="lnkActualizar_Click" />
                            <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/Iconos/actualizar.png" CssClass="imgEncabezado" OnClick="lnkActualizar_Click" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" ImageUrl="~/Imagenes/Iconos/editar.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="borrar_h" AllowFiltering="false" Groupable="false" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <asp:ImageButton meta:resourcekey="imgBorrar" ID="imgBorrar" runat="server" CommandName="borrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CausesValidation="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="imprimir_h" AllowFiltering="false" Groupable="false" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <asp:HyperLink meta:resourcekey="hlImprimir" runat="server" ID="hlImprimir" ImageUrl="~/Imagenes/Iconos/imprimirReporte.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="historial_h" AllowFiltering="false" Groupable="false" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <asp:HyperLink meta:resourcekey="hlhistorial" runat="server" ID="hlhistorial" ImageUrl="~/Imagenes/Iconos/info.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="DifiereDeIngenieria" HeaderStyle-Width="50" DataField="DifiereOReingenieria" AllowFiltering="true" DataType="System.Boolean">
                            <ItemTemplate>
                                <asp:Image meta:resourcekey="imgAdvertencia" runat="server" ID="imgAdvertencia" Visible="false" ImageUrl="~/Imagenes/Iconos/advertenciaGridB.png" />
                                <asp:Image meta:resourcekey="imgFueReingenieria" runat="server" ID="imgFueReingenieria" Visible="false" ImageUrl="~/Imagenes/Iconos/ico_reingenieria_verde.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn meta:resourcekey="grdOdtCol" HeaderStyle-Width="100" FilterControlWidth="60" DataField="NumeroOrden" />
                        <telerik:GridDateTimeColumn meta:resourcekey="grdfechaCol" HeaderStyle-Width="100" FilterControlWidth="60" DataField="Fecha" DataFormatString="{0:d}"/>
                        <telerik:GridBoundColumn meta:resourcekey="grdVersionCol" HeaderStyle-Width="80" FilterControlWidth="40" DataField="Version" DataFormatString="{0:N0}" />
                        <telerik:GridBoundColumn meta:resourcekey="grdProyectoCol" HeaderStyle-Width="180" FilterControlWidth="120" DataField="Proyecto" />
                        <telerik:GridBoundColumn meta:resourcekey="grdTallerCol" HeaderStyle-Width="100" FilterControlWidth="60" DataField="Taller" />
                        <telerik:GridBoundColumn meta:resourcekey="grdEstatusCol" HeaderStyle-Width="80" FilterControlWidth="40" DataField="Estatus" />
                        <telerik:GridBoundColumn meta:resourcekey="grdDespCol" HeaderStyle-Width="120" FilterControlWidth="80" DataField="EstatusDespachoTexto" />
                        <telerik:GridBoundColumn meta:resourcekey="grdSpoolsCol" HeaderStyle-Width="80" FilterControlWidth="40" DataField="CantidadSpools" DataFormatString="{0:N0}" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </mimo:MimossRadGrid>
        </asp:PlaceHolder>
    </div>
</asp:Content>
