<%@ Page Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true"
    CodeBehind="ItemCodes.aspx.cs" Inherits="SAM.Web.Proyectos.ItemCodes" %>

<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager runat="server" Style="display: none;" ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdItemCodes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdItemCodes" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:Header ID="headerProyecto" runat="server" />
    <div class="contenedorCentral">
        <asp:ValidationSummary meta:resourcekey="valSummary" runat="server" ID="valSummary"
            CssClass="summaryList" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdItemCodes" OnNeedDataSource="grdItemCodes_OnNeedDataSource"
            OnItemCommand="grdItemCodes_ItemCommand" OnItemCreated="grdItemCodes_ItemCreated">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblItemCodes" runat="server" ID="lblItemCodes" />
                        </div>
                        <asp:Hyperlink runat="server" ID="lnkBajaArchivo" meta:resourcekey="lnkBajaArchivo"
                            CssClass="link" />
                        <asp:Hyperlink runat="server" meta:resourcekey="imgBajarArchivo" ID="imgBajarArchivo"
                            ImageUrl="~/Imagenes/Iconos/actualizar.png" CssClass="imgEncabezado" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkActualizaPeso" meta:resourcekey="lnkActualizaPeso"
                            CssClass="link" CommandName="Agregar" />
                        <samweb:AuthenticatedHyperLink runat="server" meta:resourcekey="imgActualizaPeso" ID="imgActualizaPeso"
                            ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado"  />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlAgregar" meta:resourcekey="hlAgregar"
                            CssClass="link" />
                        <samweb:AuthenticatedHyperLink runat="server" meta:resourcekey="imgAgregar" ID="imgAgregar"
                            ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" />
                        <asp:LinkButton runat="server" ID="hlActualizar" meta:resourcekey="hlActualizar"
                            CssClass="link" OnClick="lnkActualizar_OnClick" />
                        <asp:ImageButton runat="server" meta:resourcekey="imgActualizarImg" ID="imgActualizarImg"
                            ImageUrl="~/Imagenes/Iconos/actualizar.png" CssClass="imgEncabezado" OnClick="lnkActualizar_OnClick" />
                        <asp:HyperLink runat="server" ID="hlRegTxt" CssClass="link" meta:resourcekey="hlRegTxt" />
                        <asp:HyperLink runat="server" ID="hlRegresar" ImageUrl="~/Imagenes/Iconos/btn_regresar.png"
                            CssClass="imgEncabezado" meta:resourcekey="hlRegresar" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false"
                        ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar"
                                NavigateUrl='<%#Eval("ItemCodeID", "~/Proyectos/DetItemCodes.aspx?ID={0}")%>'
                                ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false">
                        <ItemTemplate>
                            <asp:ImageButton meta:resourcekey="imgBorrar" runat="server" CommandArgument='<%#Eval("ItemCodeID") %>'
                                CommandName="Borrar" ImageUrl="~/Imagenes/Iconos/borrar.png" ID="lnkBorrar" OnClientClick="return Sam.Confirma(1);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="ItemCode" DataField="Codigo" meta:resourcekey="htItemCode"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" meta:resourcekey="htDescripcion"
                        HeaderStyle-Width="420" FilterControlWidth="150" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Clasificacion" DataField="TipoMaterial" meta:resourcekey="htClasificacion"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Peso" DataField="Peso" meta:resourcekey="htPeso"
                        HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="DescripcionInterna" DataField="DescripcionInterna"
                        meta:resourcekey="htDescripcionInterna" HeaderStyle-Width="180" FilterControlWidth="100"
                        Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Diametro1" DataField="Diametro1" meta:resourcekey="htDiametro1"
                        HeaderStyle-Width="80" FilterControlWidth="40" DataFormatString="{0:N3}" />
                    <telerik:GridBoundColumn UniqueName="Diametro2" DataField="Diametro2" meta:resourcekey="htDiametro2"
                    HeaderStyle-Width="80" FilterControlWidth="40" DataFormatString="{0:N3}" />
                    <telerik:GridBoundColumn UniqueName="FamiliaAcero" DataField="FamiliaAcero.Nombre" meta:resourcekey="htFamiliaAcero"
                        HeaderStyle-Width="180" FilterControlWidth="100" />
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
