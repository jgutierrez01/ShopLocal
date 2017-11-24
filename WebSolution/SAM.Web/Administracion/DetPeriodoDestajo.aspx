<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="DetPeriodoDestajo.aspx.cs" Inherits="SAM.Web.Administracion.DetPeriodoDestajo" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>

<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
      <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdPersonasDestajo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdPersonasDestajo" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblTitulo" NavigateUrl="~/Administracion/LstPeriodosDestajo.aspx" />
    <div class="contenedorCentral">
        <div class="cajaAzul">
            <div class="divIzquierdo" style="margin-right:15px;">
                <div class="separadorDashboard">
                    <asp:Label meta:resourcekey="lblAnioTexto" ID="lblAnioTexto" runat="server" CssClass="bold"/>
                    <asp:Label ID="lblAnio" runat="server" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label meta:resourcekey="lblSemanaTexto" ID="lblSemanaTexto" runat="server" CssClass="bold" />
                    <asp:Label ID="lblSemana" runat="server" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label meta:resourcekey="lblDiasFestivosTexto" ID="lblDiasFestivosTexto" runat="server" CssClass="bold" />
                    <asp:Label ID="lblDiasFestivos" runat="server" />
                </div>
            </div>
            <div class="divIzquierdo" style="margin-right:15px;">
                <div class="separadorDashboard">
                    <asp:Label meta:resourcekey="lblFechaIniTexto" ID="lblFechaIniTexto" runat="server" CssClass="bold"/>
                    <asp:Label ID="lblFechaIni" runat="server" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label meta:resourcekey="lblFechaFinTexto" ID="lblFechaFinTexto" runat="server" CssClass="bold" />
                    <asp:Label ID="lblFechaFin" runat="server" />
                </div>
                <div class="separadorDashboard">
                    <asp:Label meta:resourcekey="lblEstatusTexto" ID="lblEstatusTexto" runat="server" CssClass="bold" />
                    <asp:Label ID="lblEstatus" runat="server" />
                </div>
            </div>
            <div class="divIzquierdo" style="padding-top:37px;">
                <asp:Button runat="server" ID="btnCerrar" meta:resourcekey="btnCerrar" CssClass="boton" OnClick="btnCerrar_OnClick" />
            </div>
            <p></p>
        </div>
        <div style="margin-top:10px;">
            <asp:ValidationSummary runat="server" ID="valSummary" meta:resourcekey="valSummary" CssClass="summaryList" />
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdPersonasDestajo" OnNeedDataSource="grdPersonasDestajo_OnNeedDataSource" OnItemCommand="grdPersonasDestajo_OnItemCommand" OnItemDataBound="grdPersonasDestajo_OnItemDataBound">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar" OnClick="lnkActualizar_onClick" CssClass="link" />
                            <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" CssClass="imgEncabezado" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="ver_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <samweb:AuthenticatedHyperLink meta:resourcekey="hlVer" runat="server" ID="hlVer" ImageUrl="~/Imagenes/Iconos/info.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="eliminar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="imgBorrar" meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandName="borrar" OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="aprobar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="imgAprobar" meta:resourcekey="imgAprobar" ImageUrl="~/Imagenes/Iconos/ico_aprobar.png" CommandName="aprobar" OnClientClick="return Sam.Confirma(12);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="recalcular_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <asp:ImageButton runat="server" ID="imgRecalcular" meta:resourcekey="imgRecalcular" ImageUrl="~/Imagenes/Iconos/ico_recalcular.png" CommandName="recalcular" OnClientClick="return Sam.Confirma(13);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="Codigo" DataField="Codigo" meta:resourcekey="grdCodigo" HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="NumEmpleado" DataField="NumEmpleado" meta:resourcekey="grdNumEmpleado" HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="NombreCompleto" DataField="NombreCompleto" meta:resourcekey="grdNombre" HeaderStyle-Width="140" FilterControlWidth="100" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="CategoriaPuestoTexto" DataField="CategoriaPuestoTexto" meta:resourcekey="grdCategoria" HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="SumaPdis" DataField="SumaPdis" meta:resourcekey="grdPdis" HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="TotalAPagar" DataField="TotalAPagar" meta:resourcekey="grdTotalAPagar" DataFormatString="{0:C}" HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="EstatusTexto" DataField="EstatusTexto" meta:resourcekey="grdEstatus" HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </mimo:MimossRadGrid>            
        </div>
    </div>
</asp:Content>
