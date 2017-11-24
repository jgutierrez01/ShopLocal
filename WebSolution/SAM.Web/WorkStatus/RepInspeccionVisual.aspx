<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="RepInspeccionVisual.aspx.cs" Inherits="SAM.Web.WorkStatus.RepInspeccionVisual" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlPatio">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlProyecto" />
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdVisual">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdVisual" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" meta:resourcekey="lblProyecto" CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedItemChanged" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvProyecto" runat="server" ControlToValidate="ddlProyecto"
                        InitialValue="" meta:resourcekey="rfvProyecto" Display="None" CssClass="bold" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblDesde" runat="server" meta:resourcekey="lblDesde" CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpDesde" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblHasta" runat="server" meta:resourcekey="lblHasta" CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpHasta" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblNumeroReporte" runat="server" meta:resourcekey="lblNumeroReporte"
                        CssClass="bold" />
                    <br />
                    <asp:TextBox ID="txtNumeroReporte" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" meta:resourcekey="btnMostrar" CssClass="boton"
                        OnClick="btnMostrar_OnClick" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="summaryList" meta:resourcekey="valSummary" />
        <div class="separador">
            <sam:Header ID="proyEncabezado" runat="server" Visible="false" />
        </div>
        <p>
        </p>
        <asp:PlaceHolder runat="server" ID="phGrid" Visible="false">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid ID="grdVisual" runat="server" OnNeedDataSource="grdVisual_OnNeedDataSource"
                OnItemDataBound="grdVisual_ItemDataBound" OnItemCommand="grdVisual_ItemCommand">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:LinkButton runat="server" ID="lnkActualizar" CausesValidation="false" meta:resourcekey="lnkActualizar"
                                CssClass="link" OnClick="lnkActualizar_onClick" />
                            <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png"
                                CausesValidation="false" AlternateText="Actualizar" CssClass="imgEncabezado"
                                OnClick="lnkActualizar_onClick" meta:resourcekey="imgActualizar" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="btnDetalle_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlDetalle" ImageUrl="~/Imagenes/Iconos/info.png"
                                    NavigateUrl='<%#Eval("InspeccionVisualID","~/WorkStatus/DetInspeccionVisual.aspx?ID={0}") %>'
                                    meta:resourcekey="hlDetalle" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="editarRegistro" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink ID="hlEditarVisual"
                                ImageUrl="~/Imagenes/Iconos/editar.png" meta:resourcekey="hlEditarVisual" runat="server" Visible="false"></asp:HyperLink>
                        </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="seleccion_h"
                            HeaderStyle-Width="30" />--%>
                        <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                            UniqueName="inventario_h" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <samweb:LinkVisorReportes ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" runat="server"
                                    ID="hdReporte" meta:resourcekey="hdReporte" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Descargar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlDescargar" ImageUrl="~/Imagenes/Iconos/ico_descargar.png"
                                    Visible="false" meta:resourcekey="hlDescargar" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" ID="btnBorrar" runat="server" meta:resourcekey="imgBorrar"
                                    ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("InspeccionVisualID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="NumeroReporte" DataField="NumeroReporte" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNumeroReporte" />
                        <telerik:GridBoundColumn UniqueName="Fecha" DataField="Fecha" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" DataFormatString="{0:d}" meta:resourcekey="gbcFecha" />
                        <telerik:GridBoundColumn UniqueName="JuntasTotales" DataField="JuntasTotales" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcJuntasTotales" />
                        <telerik:GridBoundColumn UniqueName="JuntasAprobadas" DataField="JuntasAprobadas"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcJuntasAprobadas" />
                        <telerik:GridBoundColumn UniqueName="JuntasRechazadas" DataField="JuntasRechazadas"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcJuntasRechazadas" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                            Reorderable="false" ShowSortIcon="false">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </mimo:MimossRadGrid>
            <div id="btnWrapper" class="oculto">
                <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="false"
                    OnClick="btnRefresh_Click" />
            </div>
        </asp:PlaceHolder>
        <p>
        </p>
    </div>
</asp:Content>
