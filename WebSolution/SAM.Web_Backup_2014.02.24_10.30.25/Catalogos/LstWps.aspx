<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="LstWps.aspx.cs" Inherits="SAM.Web.Catalogos.LstWps" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdWps">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdWps" LoadingPanelID="lpGrid" />
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
        <mimo:MimossRadGrid ID="grdWps" runat="server" OnNeedDataSource="grdWps_OnNeedDataSource" OnItemCommand="grdWps_ItemCommand" OnItemDataBound="grdWps_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblLstWps" runat="server" ID="lblLstWps" />
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" NavigateUrl="~/Catalogos/DetWps.aspx" meta:resourcekey="lnkAgregar" CssClass="link" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetWps.aspx" meta:resourcekey="imgAgregar" />
                        <asp:LinkButton runat="server" ID="lnkActualizar" OnClick="lnkActualizar_onClick" meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" CssClass="imgEncabezado" meta:resourcekey="imgActualizar" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false" UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink ImageUrl="~/Imagenes/Iconos/editar.png" runat="server" ID="hypEditar" meta:resourcekey="imgEditar" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30" Groupable="false">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("WpsID") %>' OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server" meta:resourcekey="imgBorrar" />
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="WpsID" DataField="WpsID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderStyle-Width="150" FilterControlWidth="80" Groupable="false" meta:resourcekey="gbcNombre" />
                    <telerik:GridBoundColumn UniqueName="Material1" DataField="FamiliaAcero.Nombre" HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcMaterial1" />
                    <telerik:GridBoundColumn UniqueName="Material2" DataField="FamiliaAcero1.Nombre" HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcMaterial2" />
                    <telerik:GridBoundColumn UniqueName="ProcesoRaiz" DataField="ProcesoRaiz.Nombre" HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcProcesoRaiz" />
                    <telerik:GridBoundColumn UniqueName="ProcesoRelleno" DataField="ProcesoRelleno.Nombre" HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcProcesoRelleno" />
                    <telerik:GridBoundColumn UniqueName="EspesorRaizMaximo" DataField="EspesorRaizMaximo" HeaderStyle-Width="80" FilterControlWidth="40" DataFormatString="{0:N3}" Groupable="false" meta:resourcekey="gbcEspesorRaizMaximo" />
                    <telerik:GridBoundColumn UniqueName="EspesorRellenoMaximo" DataField="EspesorRellenoMaximo" HeaderStyle-Width="80" FilterControlWidth="40" DataFormatString="{0:N3}" Groupable="false" meta:resourcekey="gbcEspesorRellenoMaximo" />
                    <telerik:GridCheckBoxColumn UniqueName="RequierePwht" DataField="RequierePwht" HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcRequierePwht" />
                    <telerik:GridCheckBoxColumn UniqueName="RequierePreheat" DataField="RequierePreheat" HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcRequierePreheat" />
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
