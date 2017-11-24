<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="LstDiametro.aspx.cs" Inherits="SAM.Web.Catalogos.LstDiametro" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
      <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdDiametro">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDiametro" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="contenedorCentral">
        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel"></telerik:RadAjaxLoadingPanel>

        <mimo:MimossRadGrid runat="server" ID="grdDiametro" OnNeedDataSource="grdDiametro_OnNeedDataSource" OnItemCommand="grdDiametro_ItemCommand">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label Text="LISTADO DE DIÁMETROS" runat="server" ID="lblLstDiametro" CssClass="Titulo" meta:resourcekey="lblLstDiametro"></asp:Label>
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" Text="Agregar" NavigateUrl="~/Catalogos/DetDiametro.aspx" meta:resourcekey="lnkAgregar" CssClass="link" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" Text="Agregar" CssClass="imgEncabezado" NavigateUrl="~/Catalogos/DetDiametro.aspx" meta:resourcekey="imgAgregar" />
                        <asp:LinkButton runat="server" ID="lnkActualizar" Text="Actualizar" OnClick="lnkActualizar_onClick" meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_onClick" AlternateText="Actualizar" CssClass="imgEncabezado" meta:resourcekey="imgActualizar" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" NavigateUrl='<%#Eval("DiametroID", "~/Catalogos/DetDiametro.aspx?ID={0}")%>' ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="eliminar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgBorrar" meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandName="borrar" CommandArgument='<%#Eval("DiametroID") %>' OnClientClick="return Sam.Confirma(1);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Valor" DataFormatString="{0:N3}" Reorderable="false" Resizable="false" HeaderStyle-Width="200" FilterControlWidth="150" Groupable="false" meta:resourcekey="gbcDiametro" />
                    <telerik:GridCheckBoxColumn UniqueName="FamiliaDiametro" DataField="VerificadoPorCalidad" Resizable="false" Reorderable="false" HeaderText="¿Verificado por Calidad?" HeaderStyle-Width="200" FilterControlWidth="150" Groupable="false" meta:resourcekey="gbcVerifica" />
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
