<%@ Page Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="LstCortador.aspx.cs" Inherits="SAM.Web.Catalogos.LstCortador" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdCortador">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdCortador" LoadingPanelID="lpGrid" />
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
        <mimo:MimossRadGrid ID="grdCortador" runat="server" OnNeedDataSource="grdCortador_OnNeedDataSource"
            OnItemCommand="grdCortador_ItemCommand" OnItemDataBound="grdCortador_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblLstCortador" runat="server" ID="lblLstCortador" />
                        </div>
                        <%if(PermisoDetalleCortadores){ %>
                            <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" Text="Agregar" NavigateUrl="~/Catalogos/DetCortador.aspx"
                                CssClass="link" meta:resourcekey="lnkAgregar" />
                            <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png"
                                CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetCortador.aspx" meta:resourcekey="imgAgregar" />
                        <%} %>
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
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("CortadorID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="CortadorID" DataField="CortadorID" Visible="false" />
                   
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderText="Nombre"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" />
                    <telerik:GridBoundColumn UniqueName="ApPaterno" DataField="ApPaterno" HeaderText="Apellido Paterno"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcApPaterno" />
                    <telerik:GridBoundColumn UniqueName="ApMaterno" DataField="ApMaterno" HeaderText="Apellido Materno"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcApMaterno" />
                    <telerik:GridBoundColumn UniqueName="Patio" DataField="Patio.Nombre" HeaderText="Patio"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcPatio" />
                    <telerik:GridBoundColumn UniqueName="Taller" DataField="Taller.Nombre" HeaderText="Taller"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcTaller" />
                    <telerik:GridBoundColumn UniqueName="NumeroEmpleado" DataField="NumeroEmpleado" HeaderText="Número Empleado" DataFormatString="{0:N}"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNumeroEmpleado" />
                   
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
