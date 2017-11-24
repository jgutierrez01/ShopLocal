<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="LstPerfiles.aspx.cs" Inherits="SAM.Web.Administracion.LstPerfiles" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
 <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdPerfiles">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdPerfiles" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="contenedorCentral">
        <asp:ValidationSummary meta:resourcekey="valSummary" runat="server" ID="valSummary" CssClass="summaryList" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdPerfiles" OnNeedDataSource="grdPerfiles_OnNeedDataSource" OnItemCommand="grdPerfiles_ItemCommand">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label runat="server" ID="lblPerfiles" CssClass="Titulo" meta:resourcekey="lblPerfiles" />
                        </div>
                        <samweb:AuthenticatedHyperLink meta:resourcekey="lnkAgregar" runat="server" ID="lnkAgregar" NavigateUrl="~/Administracion/DetPerfiles.aspx" CssClass="link" />
                        <samweb:AuthenticatedHyperLink meta:resourcekey="imgAgregar" runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Administracion/DetPerfiles.aspx" />
                        <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar" OnClick="lnkActualizar_onClick" CssClass="link" />
                        <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" NavigateUrl='<%#Eval("PerfilID", "~/Administracion/DetPerfiles.aspx?ID={0}")%>' ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="eliminar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgBorrar" meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandName="borrar" CommandArgument='<%#Eval("PerfilID") %>' OnClientClick="return Sam.Confirma(1);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="Perfil" DataField="Nombre" meta:resourcekey="grdColPerfil" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="PerfilIng" DataField="NombreIngles" meta:resourcekey="grdColPerfilIngles" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" meta:resourcekey="grdColDescripcion" HeaderStyle-Width="300" FilterControlWidth="100" Groupable="false" />
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
