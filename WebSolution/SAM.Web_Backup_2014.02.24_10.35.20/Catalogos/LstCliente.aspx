<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="LstCliente.aspx.cs" Inherits="SAM.Web.Catalogos.LstCliente" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  id="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdClientes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdClientes" LoadingPanelID="lpGrid" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="contenedorCentral">
        <div class="valGroupArriba">
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" meta:resourcekey="valSummary" />
        </div>
        <telerik:RadAjaxLoadingPanel runat="server" id="lpGrid" />
        <mimo:MimossRadGrid runat="server" id="grdClientes" OnNeedDataSource="grdClientes_OnNeedDataSource"
            OnItemCommand="grdClientes_ItemCommand" OnItemDataBound="grdClientes_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblLstClientes" runat="server" ID="lblLstClientes" />
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" Text="Agregar" NavigateUrl="~/Catalogos/DetCliente.aspx" meta:resourcekey="lnkAgregar" CssClass="link" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetCliente.aspx" meta:resourcekey="imgAgregar" />
                        <asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_onClick" meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" AlternateText="Actualizar" CssClass="imgEncabezado" meta:resourcekey="imgActualizar" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink ImageUrl="~/Imagenes/Iconos/editar.png" runat="server" ID="hypEditar" meta:resourcekey="imgEditar" NavigateUrl="~/Catalogos/DetCliente.aspx" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("ClienteID") %>' OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server" meta:resourcekey="imgBorrar" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="ClienteID" DataField="ClienteID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="NombreCliente" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" />
                    <telerik:GridBoundColumn UniqueName="TelefonoOficina" DataField="TelefonoOficina"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcTelefonoOficina" />
                    <telerik:GridBoundColumn UniqueName="CorreoElectronico" DataField="CorreoElectronico"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcCorreo" />
                    <telerik:GridBoundColumn UniqueName="NombreContacto" DataField="NombreContacto"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombreContacto" />
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
