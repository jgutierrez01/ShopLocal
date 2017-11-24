<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="NumerosUnicosEnCorte.aspx.cs" Inherits="SAM.Web.WorkStatus.NumerosUnicosEnCorte"  %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>    
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radAjaxMng" runat="server" EnablePageHeadUpdate="true">
        <AjaxSettings>
             <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdNumeroUnicos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdNumeroUnicos" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>                       
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblNumeroUnicoCorte" CssClass="Titulo" meta:resourcekey="lblNumeroUnicoCorte"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" FiltroNumeroControl="false" ProyectoHeaderID="proyEncabezado" ProyectoAutoPostBack="true" runat="server" ID="filtroGenerico"></uc2:Filtro>
           
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" meta:resourcekey="btnMostrar" runat="server" CssClass="boton"
                        OnClick="btnMostrar_Click" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <div>
            <proy:Encabezado ID="proyEncabezado" runat="server" Visible="False" />
        </div>
        <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummary" CssClass="summaryList" DisplayMode="BulletList" />
        <p>
        </p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
        <mimo:MimossRadGrid runat="server" ID="grdNumeroUnicos" OnNeedDataSource="grdNumeroUnicos_OnNeedDataSource" OnItemCommand="grdNumerosUnicos_ItemCommand"  Visible="false" >        
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
            <CommandItemTemplate>
            </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="borrar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("NumeroUnicoSegmentoID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="hdFecha" DataField="FechaTraspaso" meta:resourcekey="hdFecha"
                        FilterControlWidth="100" HeaderStyle-Width="150"  DataFormatString="{0:d}"/>
                        <telerik:GridBoundColumn UniqueName="hdOrdenTrabajo" DataField="OrdenDeTrabajo"
                        meta:resourcekey="hdOrdenTrabajo" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdNumeroUnico" DataField="NumeroUnico"
                        meta:resourcekey="hdCodigo" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdSegmento" DataField="Segmento"
                        meta:resourcekey="hdSegmento" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="hdItemCode" DataField="ItemCode"
                        meta:resourcekey="hdItemCode" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdICDescripcion" DataField="Descripcion"
                        meta:resourcekey="hdICDescripcion" FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro1" DataField="Diametro1" DataFormatString="{0:#0.000}"
                        meta:resourcekey="hdDiametro1" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdDiametro2" DataField="Diametro2" DataFormatString="{0:#0.000}"
                        meta:resourcekey="hdDiametro2" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdColada" DataField="NumeroColada"
                        meta:resourcekey="hdColada" FilterControlWidth="50" HeaderStyle-Width="135" />
                    <telerik:GridBoundColumn UniqueName="hdCedula" DataField="Cedula" meta:resourcekey="hdCedula"
                        FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="hdCantidad" DataField="Cantidad"
                        meta:resourcekey="hdCantidad" FilterControlWidth="100" HeaderStyle-Width="200" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>
</asp:Content>
