<%@ Page Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true"
    CodeBehind="DetReqPinturaNumUnico.aspx.cs" Inherits="SAM.Web.Materiales.DetReqPinturaNumUnico" %>

<%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdNumUnicos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdNumUnicos" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="phDatos" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina runat="server" ID="lblTitulo" NavigateUrl="~/Materiales/RepReqPinturaNumUnico.aspx"
        meta:resourcekey="lblTitulo" />
    <br />
    <sam:Header ID="proyEncabezado" runat="server" />
    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList"
        meta:resourcekey="valSummary" />
    <div class="contenedorCentral">
        <asp:PlaceHolder runat="server" ID="phDatos">
            <div class="cajaAzul">
                <p>
                    <asp:Label runat="server" ID="lblNumeroRequisicion" CssClass="bold" meta:resourcekey="lblNumeroRequisicion" />
                    <asp:Label runat="server" ID="lblNumeroRequisicionData" />
                </p>
                <p>
                    <asp:Label runat="server" ID="lblTotalNumUnicos" CssClass="bold" meta:resourcekey="lblTotalNumUnicos" />
                    <asp:Label runat="server" ID="lblTotalNumUnicosData" />
                </p>
                <p>
                    <asp:Label runat="server" ID="lblFechaRequisicion" CssClass="bold" meta:resourcekey="lblFechaRequisicion" />
                    <asp:Label runat="server" ID="lblFechaRequisicionData" />
                </p>
            </div>
            <p>
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="phGrd">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdNumUnicos" OnNeedDataSource="grdNumUnicos_OnNeedDataSource"
                OnItemCommand="grdNumUnicos_OnItemCommand" AllowMultiRowSelection="true">
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
                                    meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("RequisicionNumeroUnicoDetalleID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="NumeroUnico" DataField="NumeroUnico" meta:resourcekey="gbcNumeroUnico"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="ItemCode" DataField="ItemCode" meta:resourcekey="gbcItemCode"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" meta:resourcekey="gbcDescripcion"
                            FilterControlWidth="80" HeaderStyle-Width="350" />
                        <telerik:GridBoundColumn UniqueName="Diametro1" DataField="Diametro1" DataFormatString="{0:N3}" meta:resourcekey="gbcDiametro1"
                            FilterControlWidth="50" HeaderStyle-Width="90" />
                        <telerik:GridBoundColumn UniqueName="Diametro2" DataField="Diametro2" DataFormatString="{0:N3}" meta:resourcekey="gbcDiametro2"
                            FilterControlWidth="50" HeaderStyle-Width="90" />
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
