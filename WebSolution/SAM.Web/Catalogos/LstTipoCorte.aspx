<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="LstTipoCorte.aspx.cs" Inherits="SAM.Web.Catalogos.LstTipoCorte" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="RadAjaxManager1">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdTipoCorte">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdTipoCorte" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="contenedorCentral">
        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel"></telerik:RadAjaxLoadingPanel>

        <mimo:MimossRadGrid runat="server" ID="grdTipoCorte" OnNeedDataSource="grdTipoCorte_OnNeedDataSource" OnItemCommand="grdTipoCorte_ItemCommand">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label Text="TIPOS DE CORTE" runat="server" ID="lblLstTipoCorte" CssClass="Titulo" meta:resourcekey="lblLstTipoCorte"></asp:Label>
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" Text="Agregar" NavigateUrl="~/Catalogos/DetTipoCorte.aspx" meta:resourcekey="lnkAgregar" CssClass="link" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" Text="Agregar" CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetTipoCorte.aspx" meta:resourcekey="imgAgregar" />
                        <asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_onClick" meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" AlternateText="Actualizar" CssClass="imgEncabezado" meta:resourcekey="imgActualizar" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" NavigateUrl='<%#Eval("TipoCorteID", "~/Catalogos/DetTipoCorte.aspx?ID={0}")%>' ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="eliminar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgBorrar" meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandName="borrar" CommandArgument='<%#Eval("TipoCorteID") %>' OnClientClick="return Sam.Confirma(1);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn UniqueName="Codigo" DataField="Codigo" HeaderText="Código" Reorderable="false" Resizable="false"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcCodigo" />
                        <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderText="Nombre"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" Reorderable="false" Resizable="false" />
                        <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" HeaderText="Descripción" Reorderable="false" Resizable="false"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDescripcion" />
                        <telerik:GridCheckBoxColumn UniqueName="FamiliaTipoCorte" DataField="VerificadoPorCalidad" HeaderText="¿Verificado por Calidad?" Reorderable="false" Resizable="false"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcVerifica" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Resizable="false"
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