<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="LstFabricantes.aspx.cs" Inherits="SAM.Web.Catalogos.LstFabricantes" %>
    <%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdFabricantes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdFabricantes" LoadingPanelID="lpGrid"/>
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="contenedorCentral">
        <div class="valGroupArriba">
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" meta:resourcekey="valSummary"/>
        </div>
        <telerik:RadAjaxLoadingPanel runat="server" ID="lpGrid" />
        <mimo:MimossRadGrid ID="grdFabricantes" runat="server" OnNeedDataSource="grdFabricantes_OnNeedDataSource"
            OnItemCommand="grdFabricantes_ItemCommand" OnItemDataBound="grdFabricantes_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblLstFabricantes" runat="server" ID="lblLstFabricantes" />
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" Text="Agregar" NavigateUrl="~/Catalogos/DetFabricante.aspx" CssClass="link" meta:resourcekey="lnkAgregar" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetFabricante.aspx" meta:resourcekey="imgAgregar"/>
                        <asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_onClick" CssClass="link" meta:resourcekey="lnkActualizar" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" CssClass="imgEncabezado" meta:resourcekey="imgActualizar" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink ImageUrl="~/Imagenes/Iconos/editar.png" runat="server" ID="hypEditar" meta:resourcekey="imgEditar" NavigateUrl="~/Catalogos/DetFabricante.aspx" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="borrar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("FabricanteID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="FabricanteID" DataField="FabricanteID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderText="Nombre"
                        HeaderStyle-Width="480" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" />
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
