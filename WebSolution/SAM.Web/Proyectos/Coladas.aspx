<%@ Page  Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="Coladas.aspx.cs" Inherits="SAM.Web.Proyectos.Coladas" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdColadas">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdColadas" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:Header ID="headerProyecto" runat="server" />
    <div class="contenedorCentral">
        <asp:ValidationSummary meta:resourcekey="valSummary" runat="server" ID="valSummary" CssClass="summaryList" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdColadas" OnNeedDataSource="grdColadas_OnNeedDataSource" OnItemCommand="grdColadas_ItemCommand" OnItemCreated="grdColadas_ItemCreated">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblColadas" runat="server" ID="lblColadas" />
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlAgregar" meta:resourcekey="hlAgregar" CssClass="link" />
                        <samweb:AuthenticatedHyperLink runat="server" meta:resourcekey="imgAgregar" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" />
                        <asp:LinkButton runat="server" ID="hlActualizar"  meta:resourcekey="hlActualizar" CssClass="link" OnClick="lnkActualizar_OnClick" />
                        <asp:ImageButton runat="server" meta:resourcekey="imgActualizarImg" ID="imgActualizarImg" ImageUrl="~/Imagenes/Iconos/actualizar.png" CssClass="imgEncabezado" OnClick="lnkActualizar_OnClick" />
                        <asp:HyperLink runat="server" ID="hlRegTxt" CssClass="link" meta:resourcekey="hlRegTxt" />
                        <asp:HyperLink runat="server" ID="hlRegresar" ImageUrl="~/Imagenes/Iconos/btn_regresar.png" CssClass="imgEncabezado" meta:resourcekey="hlRegresar" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" NavigateUrl='<%#Eval("ColadaID", "~/Proyectos/DetColadas.aspx?ID={0}")%>' ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30" Groupable="false">
                        <ItemTemplate>
                            <asp:ImageButton meta:resourcekey="imgBorrar" runat="server" CommandArgument='<%#Eval("ColadaID") %>' CommandName="Borrar" ImageUrl="~/Imagenes/Iconos/borrar.png" ID="lnkBorrar" OnClientClick="return Sam.Confirma(1);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="NumeroColada" DataField="NumeroColada" meta:resourcekey="htNumColada" HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Fabricante" DataField="Fabricante.Nombre"  meta:resourcekey="htFabricante" HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Acero" DataField="Acero.Nomenclatura" meta:resourcekey="htAcero" HeaderStyle-Width="120" FilterControlWidth="70" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Certificado" DataField="NumeroCertificado" meta:resourcekey="htCertificado" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridCheckBoxColumn UniqueName="HoldCalidad" DataField="HoldCalidad" meta:resourcekey="htHoldCalidad" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
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
