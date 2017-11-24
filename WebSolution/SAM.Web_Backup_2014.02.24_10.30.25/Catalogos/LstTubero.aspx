<%@ Page Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="LstTubero.aspx.cs" Inherits="SAM.Web.Catalogos.LstTubero" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdTuberos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdTuberos" LoadingPanelID="lpGrid" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="contenedorCentral">
        <div class="valGroupArriba">
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" meta:resourcekey="valSummary" />
        </div>
        <telerik:RadAjaxLoadingPanel runat="server" ID="lpGrid" />
        <mimo:MimossRadGrid ID="grdTuberos" runat="server" OnNeedDataSource="grdTuberos_OnNeedDataSource"
            OnItemCommand="grdTuberos_ItemCommand" OnItemDataBound="grdTuberos_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblLstTuberos" runat="server" ID="lblLstTuberos" />
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" Text="Agregar" NavigateUrl="~/Catalogos/DetTubero.aspx"
                            CssClass="link" meta:resourcekey="lnkAgregar" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png"
                            CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetTubero.aspx" meta:resourcekey="imgAgregar" />
                        <asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_onClick"
                            CssClass="link" meta:resourcekey="lnkActualizar" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png"
                            OnClick="lnkActualizar_onClick" CssClass="imgEncabezado" meta:resourcekey="imgActualizar" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink ImageUrl="~/Imagenes/Iconos/editar.png" runat="server"
                                ID="hypEditar" meta:resourcekey="imgEditar" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("TuberoID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="TuberoID" DataField="TuberoID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Codigo" DataField="Codigo" HeaderText="Codigo"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcCodigo" />
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderText="Nombre"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" />
                    <telerik:GridBoundColumn UniqueName="ApPaterno" DataField="ApPaterno" HeaderText="Apellido Paterno"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcApPaterno" />
                    <telerik:GridBoundColumn UniqueName="ApMaterno" DataField="ApMaterno" HeaderText="Apellido Materno"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcApMaterno" />
                    <telerik:GridBoundColumn UniqueName="Patio" DataField="Patio.Nombre" HeaderText="Patio"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcPatio" />
                    <telerik:GridBoundColumn UniqueName="NumeroEmpleado" DataField="NumeroEmpleado" HeaderText="Número Empleado" DataFormatString="{0:N}"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNumeroEmpleado" />
                    <telerik:GridCheckBoxColumn UniqueName="Activo" DataField="Activo" HeaderStyle-Width="70"
                        FilterControlWidth="30" Groupable="false" meta:resourcekey="gbcActivo" />
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
