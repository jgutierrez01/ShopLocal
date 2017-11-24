<%@ Page Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true"
    CodeBehind="CorteJunta.aspx.cs" Inherits="SAM.Web.Produccion.CorteJunta" %>

<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdCorteJunta">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdCorteJunta" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label ID="lblTitulo" runat="server" CssClass="Titulo" meta:resourcekey="lblTitulo"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" FiltroNumeroUnico="false" FiltroNumeroControl="true"
                FiltroOrdenTrabajo="false" ProyectoHeaderID="proyHeader" ProyectoAutoPostBack="true"
                runat="server" ID="filtroGenerico" NumeroControlRequerido="true"></uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button CssClass="boton" ID="btnMostrar" OnClick="btnMostrarClick" meta:resourcekey="btnMostrar"
                        runat="server" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <uc1:Header ID="proyHeader" runat="server" Visible="false" />
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList"
            meta:resourcekey="valSummary" />
        <p>
        </p>
        <telerik:RadAjaxLoadingPanel ID="ldPanel" runat="server"></telerik:RadAjaxLoadingPanel>
        <mimo:MimossRadGrid ID="grdCorteJunta" runat="server" OnNeedDataSource="grdCorteJunta_OnNeedDataSource"
            OnItemCommand="grdCorteJunta_ItemCommand" OnItemDataBound="grdCorteJunta_ItemDataBound"
            Visible="false">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:LinkButton runat="server" ID="lnkActualizar" OnClick="lnkActualizar_onClick"
                            CausesValidation="false" meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png"
                            CausesValidation="false" OnClick="lnkActualizar_onClick" AlternateText="Actualizar"
                            CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>                 
                    <telerik:GridTemplateColumn UniqueName="btnCortar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false" Resizable="false" ShowFilterIcon="false" ShowSortIcon="false" Reorderable="false">
                        <ItemTemplate>                           
                                 <asp:ImageButton meta:resourcekey="glbCortar" ID="imgCortar" runat="server" CommandName="Cortar"
                                ImageUrl="~/Imagenes/Iconos/ico_cortarB.png" CommandArgument='<%#Eval("JuntaWorkstatusID") %>'
                                OnClientClick="return Sam.Confirma(3)" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnEliminar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false" Resizable="false"  ShowFilterIcon="false" ShowSortIcon="false" Reorderable="false">
                        <ItemTemplate>
                            <asp:ImageButton meta:resourcekey="glbEliminar" ID="imgBorrar" runat="server" CommandName="Eliminar"
                                ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("JuntaWorkstatusID") %>'
                                OnClientClick="return Sam.Confirma(1)" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="Junta" DataField="Etiqueta" HeaderText="Junta"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="grdJunta" />
                    <telerik:GridBoundColumn UniqueName="Etiqueta" DataField="Localizacion" HeaderText="Localizacion"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="grdLocalizacion" />
                    <telerik:GridBoundColumn UniqueName="Tipo" DataField="TipoJunta" HeaderText="Tipo"
                        HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="grdTipo" />
                    <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" HeaderText="Cedula"
                        HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="grdCedula" />
                    <telerik:GridBoundColumn UniqueName="Material1" DataField="Material1" HeaderText="Material 1"
                        HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="grdMaterial1" />
                    <telerik:GridBoundColumn UniqueName="Material2" DataField="Material2" HeaderText="Material2"
                        HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="grdMaterial2" />
                    <telerik:GridBoundColumn UniqueName="Diametro" DataField="Diametro" HeaderText="Diametro"
                        DataFormatString="{0:N3}" HeaderStyle-Width="80" FilterControlWidth="40"
                        Groupable="false" meta:resourcekey="grdDiametro" />
                    <telerik:GridBoundColumn UniqueName="UltimoProceso" DataField="UltimoProceso" HeaderText="Ultimo Proceso"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="grdUltimoProceso" />
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>
</asp:Content>
