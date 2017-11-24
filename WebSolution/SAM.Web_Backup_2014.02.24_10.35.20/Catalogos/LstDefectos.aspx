<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="LstDefectos.aspx.cs" Inherits="SAM.Web.Catalogos.LstDefecto" %>
    <%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="CntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdDefectos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDefectos" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="contenedorCentral">
        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
        <mimo:MimossRadGrid runat="server" ID="grdDefectos" OnNeedDataSource="grdDefectos_OnNeedDataSource"
            OnItemCommand="grdDefectos_ItemCommand">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label runat="server" ID="lblLstDefectos" CssClass="Titulo" meta:resourcekey="lblLstDefectos"></asp:Label>
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" NavigateUrl="~/Catalogos/DetDefecto.aspx" CssClass="link" meta:resourcekey="lnkAgregar"/>
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetDefecto.aspx" meta:resourcekey="imgAgregar"/>
                        <asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_onClick" CssClass="link" meta:resourcekey="lnkActualizar"/>
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" CssClass="imgEncabezado" meta:resourcekey="imgActualizar"/>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" NavigateUrl='<%#Eval("DefectoID", "~/Catalogos/DetDefecto.aspx?ID={0}")%>' ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="eliminar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgBorrar" meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandName="borrar" CommandArgument='<%#Eval("DefectoID") %>' OnClientClick="return Sam.Confirma(1);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="TipoPrueba" DataField="TipoPrueba.Nombre" HeaderText="Tipo de Prueba" Reorderable="false" Resizable="false" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcTipoPrueba" />
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderText="Nombre" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" Reorderable="false" Resizable="false" />
                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" HeaderText="Descripción" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDescripcion" Reorderable="false" Resizable="false" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Resizable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>
</asp:Content>
