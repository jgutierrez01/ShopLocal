<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="LstPatio.aspx.cs" Inherits="SAM.Web.Catalogos.LstPatio" %>
    <%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdPatio">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdPatio" LoadingPanelID="lpGrid"/>
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
        <mimo:MimossRadGrid ID="grdPatio" runat="server" OnNeedDataSource="grdPatio_OnNeedDataSource"
            OnItemCommand="grdPatio_ItemCommand" OnItemDataBound="grdPatio_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblLstPatios" runat="server" ID="lblLstPatios" />
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" Text="Agregar" NavigateUrl="~/Catalogos/DetPatio.aspx" meta:resourcekey="lnkAgregar" CssClass="link" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetPatio.aspx" meta:resourcekey="imgAgregar"/>
                        <asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_onClick" meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" AlternateText="Actualizar" CssClass="imgEncabezado" meta:resourcekey="imgActualizar"/>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink ImageUrl="~/Imagenes/Iconos/editar.png" runat="server" ID="hypEditar" meta:resourcekey="imgEditar" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("PatioID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="PatioID" DataField="PatioID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderText="Nombre"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" />
                    <telerik:GridBoundColumn UniqueName="Propietario" DataField="Propietario" HeaderText="Propietario"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcPropietario" />
                    <telerik:GridBoundColumn UniqueName="NoTalleres" DataField="Taller.Count" HeaderText="# de Talleres" DataFormatString="{0:N}"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" DataType="System.Int32" meta:resourcekey="gbcNoTalleres"/>
                    <telerik:GridBoundColumn UniqueName="NoMaquinas" DataField="Maquina.Count" HeaderText="# de Maquinas" DataFormatString="{0:N}"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" DataType="System.Int32" meta:resourcekey="gbcNoMaquinas" />
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>
</asp:Content>
