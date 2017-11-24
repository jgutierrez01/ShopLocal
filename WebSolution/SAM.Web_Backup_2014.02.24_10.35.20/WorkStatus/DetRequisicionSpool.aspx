<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
         CodeBehind="DetRequisicionSpool.aspx.cs" Inherits="SAM.Web.WorkStatus.DetRequisicionSpool" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>

<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="phDatos" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
     <sam:BarraTituloPagina runat="server" ID="lblHeader" meta:resourcekey="lblHeader"
                            NavigateUrl="/WorkStatus/RepRequisicionesSpool.aspx" /> 

    <div class="contenedorCentral">
    <uc1:Header ID="proyHeader" runat="server" Visible="true" />
        <asp:PlaceHolder runat="server" ID="phDatos">
            <div class="cajaAzul">
                <div class="divIzquierdo ancho30">
                        <asp:Label runat="server" ID="lblNumeroRequisicion" CssClass="bold" meta:resourcekey="lblNumeroRequisicion" />
                        <asp:Label runat="server" ID="lblNumeroRequisicionData" />
                   <p></p>
                        <asp:Label runat="server" ID="lblFechaRequisicion" CssClass="bold" meta:resourcekey="lblFechaRequisicion" />
                        <asp:Label runat="server" ID="lblFechaRequisicionData" />
                   <p></p>
                        <asp:Label runat="server" ID="lblObservaciones" CssClass="bold" meta:resourcekey="lblObservaciones" />
                        <asp:Label runat="server" ID="lblObservacionesData" />
                   <p></p>
                </div>
                <div class="divIzquierdo">
                        <asp:Label runat="server" ID="lblTipoPrueba" CssClass="bold" meta:resourcekey="lblTipoPrueba" />
                        <asp:Label runat="server" ID="lblTipoPruebaData" />
                   <p></p>
                        <asp:Label runat="server" ID="lblTotalSpools" CssClass="bold" meta:resourcekey="lblTotalSpools" />
                        <asp:Label runat="server" ID="lblTotalSpoolsData" />
                    <p></p>
                </div>
                <p>
                </p>
            </div>
        </asp:PlaceHolder>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" HeaderText="Errores" />
         <p> </p>
        <asp:PlaceHolder runat="server" ID="phGrd">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdSpools" OnNeedDataSource="grdSpools_OnNeedDataSource"
                OnItemCommand="grdSpools_OnItemCommand" AllowMultiRowSelection="true">
                <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" Text="Borrar" ID="btnBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("SpoolRequisicionID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="OrdenTrabajo" DataField="OrdenTrabajo" meta:resourcekey="gbcOrdenTrabajo"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" meta:resourcekey="gbcNumeroControl"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="Spool" DataField="Spool" meta:resourcekey="gbcSpool"
                            FilterControlWidth="100" HeaderStyle-Width="230" />
                        <telerik:GridBoundColumn UniqueName="Material1" DataField="Material1" meta:resourcekey="gbcMaterial1"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Material2" DataField="Material2" meta:resourcekey="gbcMaterial2"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                            Reorderable="false" ShowSortIcon="false">
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