<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="LstUsuario.aspx.cs" Inherits="SAM.Web.Administracion.LstUsuario" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdUsuarios">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdUsuarios" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="contenedorCentral">
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdUsuarios" OnNeedDataSource="grdUsuarios_OnNeedDataSource">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblListaUsuarios" runat="server" ID="lblListaUsuarios" />
                        </div>
                        <samweb:AuthenticatedHyperLink meta:resourcekey="lnkAgregar" runat="server" ID="lnkAgregar" NavigateUrl="~/Administracion/DetUsuario.aspx" CssClass="link" />
                        <samweb:AuthenticatedHyperLink meta:resourcekey="imgAgregar" runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Administracion/DetUsuario.aspx" />
                        <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar" OnClick="lnkActualizar_OnClick" CssClass="link" />
                        <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_OnClick" CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" NavigateUrl='<%#Eval("UserId", "~/Administracion/DetUsuario.aspx?UID={0}")%>' ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn meta:resourcekey="grdColUsuario" UniqueName="Usuario" DataField="Username" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn meta:resourcekey="grdColEmail" UniqueName="Email" DataField="Email" FilterControlWidth="100" HeaderStyle-Width="180"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdColNombre" UniqueName="Nombre" DataField="Nombre" FilterControlWidth="50" HeaderStyle-Width="100"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdColApPaterno" UniqueName="ApPaterno" DataField="ApPaterno" FilterControlWidth="50" HeaderStyle-Width="100"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdColApMaterno" UniqueName="ApMaterno" DataField="ApMaterno" FilterControlWidth="50" HeaderStyle-Width="100"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdColEstatus" UniqueName="Estatus" DataField="Estatus" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn meta:resourcekey="grdColPerfil" UniqueName="Perfil" DataField="Perfil.Nombre" FilterControlWidth="50" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn meta:resourcekey="grdColIdioma" UniqueName="Idioma" DataField="IdiomaTexto" FilterControlWidth="50" HeaderStyle-Width="100" />
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
