<%@ Page Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="ProyDefault.aspx.cs" Inherits="SAM.Web.Proyectos.ProyDefault" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
 <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdProyectos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdProyectos" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="contenedorCentral">
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdProyectos" OnNeedDataSource="grdProyecto_OnNeedDataSource" AllowCustomPaging="false">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblListProyecto" runat="server" ID="lblListProyecto" />
                        </div>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlDetalle" runat="server" ID="hlDetalle" NavigateUrl='<%#Eval("ID", "~/Proyectos/DetProyecto.aspx?ID={0}")%>' ImageUrl="~/Imagenes/Iconos/info.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="Proyecto" DataField="Nombre" meta:resourcekey="htNombre" FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="PrefijoNumeroUnico" DataField="PrefijoNumeroUnico" meta:resourcekey="htNu" FilterControlWidth="80" HeaderStyle-Width="120" />
                    <telerik:GridBoundColumn UniqueName="PrefijoOdt" DataField="PrefijoOdt" meta:resourcekey="htOdt" FilterControlWidth="80" HeaderStyle-Width="120" />
                    <telerik:GridBoundColumn UniqueName="Estatus" DataField="Estatus" meta:resourcekey="htEstatus" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="Cliente" DataField="NombreCliente" meta:resourcekey="htCliente" FilterControlWidth="120" HeaderStyle-Width="180" />
                    <telerik:GridBoundColumn UniqueName="Color" DataField="NombreColor" meta:resourcekey="htColor" FilterControlWidth="60" HeaderStyle-Width="100" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>

</asp:Content>
