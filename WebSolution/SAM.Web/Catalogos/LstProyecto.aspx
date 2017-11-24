<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="LstProyecto.aspx.cs" Inherits="SAM.Web.Catalogos.LstProyecto" %>
    <%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdProyectos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdProyectos" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="contenedorCentral">
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <asp:ValidationSummary meta:resourcekey="valSummary" runat="server" ID="valSummary" CssClass="summaryList" />
        <mimo:MimossRadGrid runat="server" ID="grdProyectos" OnNeedDataSource="grdProyecto_OnNeedDataSource" AllowCustomPaging="false" OnItemCommand="grdProyecto_ItemCommand">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblListProyecto" runat="server" ID="lblListProyecto" />
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlAgregar" meta:resourcekey="hlAgregar" NavigateUrl="~/Catalogos/AltaProyecto.aspx" CssClass="link" />
                        <samweb:AuthenticatedHyperLink meta:resourcekey="hlAgregarImg" runat="server" ID="hlAgregarImg" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Catalogos/AltaProyecto.aspx" />
                        <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar" OnClick="lnkActualizar_OnClick" CssClass="link" />
                        <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_OnClick" CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" NavigateUrl='<%#Eval("ProyectoID", "~/Proyectos/DetProyecto.aspx?ID={0}")%>' ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30" Groupable="false">
                        <ItemTemplate>
                            <asp:ImageButton meta:resourcekey="imgBorrar" runat="server" CommandArgument='<%#Eval("ProyectoID") %>' CommandName="Borrar" ImageUrl="~/Imagenes/Iconos/borrar.png" ID="lnkBorrar" OnClientClick="return Sam.Confirma(1);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn UniqueName="copiar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlCopiar" runat="server" ID="hlCopiar" NavigateUrl='<%#Eval("ProyectoID", "~/Catalogos/ImportConfigProyecto.aspx?ID={0}")%>' ImageUrl="~/Imagenes/Iconos/ico_recalcular.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridBoundColumn UniqueName="Proyecto" DataField="Proyecto" meta:resourcekey="htNombre" FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="PrefijoNumeroUnico" DataField="PrefijoNumeroUnico" meta:resourcekey="htNu" FilterControlWidth="80" HeaderStyle-Width="120" />
                    <telerik:GridBoundColumn UniqueName="PrefijoOdt" DataField="PrefijoOdt" meta:resourcekey="htOdt" FilterControlWidth="80" HeaderStyle-Width="120" />
                    <telerik:GridBoundColumn UniqueName="Estatus" DataField="Estatus" meta:resourcekey="htEstatus" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn UniqueName="Cliente" DataField="NombreClienteCompleto" HeaderText="Cliente" meta:resourcekey="htCliente" FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="Color" DataField="NombreColor" HeaderText="Color" meta:resourcekey="htColor" FilterControlWidth="50" HeaderStyle-Width="100" />
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
