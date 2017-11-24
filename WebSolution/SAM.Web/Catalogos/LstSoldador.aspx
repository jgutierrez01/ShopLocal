<%@ Page Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="LstSoldador.aspx.cs" Inherits="SAM.Web.Catalogos.LstSoldador" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdSoldadores">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSoldadores" LoadingPanelID="lpGrid" />
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
        <mimo:MimossRadGrid ID="grdSoldadores" runat="server" OnNeedDataSource="grdSoldadores_OnNeedDataSource"
            OnItemCommand="grdSoldadores_ItemCommand" OnItemDataBound="grdSoldadores_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblLstSoldadores" runat="server" ID="lblLstSoldadores" />
                        </div>         

                        <%if(SAM.Web.Common.SessionFacade.EsAdministradorSistema){ %>
                            <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" Text="Agregar" NavigateUrl="~/Catalogos/DetSoldador.aspx"
                                CssClass="link" meta:resourcekey="lnkAgregar" />                        
                            <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png"
                                CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetSoldador.aspx" meta:resourcekey="imgAgregar" />
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
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("SoldadorID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="SoldadorID" DataField="SoldadorID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Codigo" DataField="Codigo" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcCodigo" />
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" />
                    <telerik:GridBoundColumn UniqueName="ApPaterno" DataField="ApPaterno" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcApPaterno" />
                    <telerik:GridBoundColumn UniqueName="ApMaterno" DataField="ApMaterno" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcApMaterno" />
                    <telerik:GridBoundColumn UniqueName="Patio" DataField="Patio.Nombre" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcPatio" />
                    <telerik:GridBoundColumn UniqueName="NumeroEmpleado" DataField="NumeroEmpleado" HeaderStyle-Width="120" DataFormatString="{0:N}"
                        FilterControlWidth="70" Groupable="false" meta:resourcekey="gbcNumeroEmpleado" />
                    <telerik:GridBoundColumn UniqueName="FechaVigencia" DataField="FechaVigencia" HeaderStyle-Width="120" DataFormatString="{0:d}"
                        FilterControlWidth="70" Groupable="false" meta:resourceKey="gbcFechaVigencia" />

                    <telerik:GridBoundColumn UniqueName="AreaTrabajo" DataField="AreaTrabajo" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcAreaTrabajo" />

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
