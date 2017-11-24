<%@ Page  Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="IcEquivalentes.aspx.cs" Inherits="SAM.Web.Proyectos.IcEquivalentes" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdIcEquivalentes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdIcEquivalentes" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:Header ID="headerProyecto" runat="server" />
    <div class="contenedorCentral">
        <asp:ValidationSummary meta:resourcekey="valSummary" runat="server" ID="valSummary" CssClass="summaryList" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdIcEquivalentes" OnNeedDataSource="grdIcEquivalentes_OnNeedDataSource" OnItemCommand="grdIcEquivalentes_ItemCommand" OnItemCreated="grdIcEquivalentes_ItemCreated" OnDetailTableDataBind="grdIcEquivalentes_OnDetailTableDataBind">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand" DataKeyNames="MinItemCodeEquivalenteID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblItemCodes" runat="server" ID="lblItemCodes" />
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
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" NavigateUrl='<%#Eval("MinItemCodeEquivalenteID", "~/Proyectos/DetIcEquivalentes.aspx?ID={0}")%>' ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30" Groupable="false">
                        <ItemTemplate>
                            <asp:ImageButton meta:resourcekey="imgBorrar" runat="server" CommandArgument='<%#Eval("MinItemCodeEquivalenteID") %>' CommandName="Borrar" ImageUrl="~/Imagenes/Iconos/borrar.png" ID="lnkBorrar" OnClientClick="return Sam.Confirma(1);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="Codigo" DataField="Codigo" HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" meta:resourcekey="htCodigo" />
                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" HeaderStyle-Width="350" FilterControlWidth="100" Groupable="false" meta:resourcekey="htDescripcion" />
                    <telerik:GridBoundColumn UniqueName="Diametro1" DataField="Diametro1" DataFormatString="{0:#0.000}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false"  meta:resourcekey="htDiametro1" />
                    <telerik:GridBoundColumn UniqueName="Diametro2" DataField="Diametro2" DataFormatString="{0:#0.000}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htDiametro2"  />
                    <telerik:GridBoundColumn UniqueName="NumEquivalencias" DataField="NumEquivalencias" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htNumEquivalencias" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <DetailTables>
                    <telerik:GridTableView AllowFilteringByColumn="false" AllowSorting="false" AllowPaging="false" EnableHeaderContextFilterMenu="false" EnableHeaderContextMenu="false" AutoGenerateColumns="false" Width="700">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="CodigoEq" DataField="CodigoEq" HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" meta:resourcekey="htCodigo" />
                            <telerik:GridBoundColumn UniqueName="DescripcionEq" DataField="DescripcionEq" HeaderStyle-Width="350" FilterControlWidth="100" Groupable="false" meta:resourcekey="htDescripcion" />
                            <telerik:GridBoundColumn UniqueName="D1Eq" DataField="D1Eq" DataFormatString="{0:#0.000}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htDiametro1" />
                            <telerik:GridBoundColumn UniqueName="D2Eq" DataField="D2Eq" DataFormatString="{0:#0.000}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htDiametro2" />
                            <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </telerik:GridTableView>
                </DetailTables>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>
</asp:Content>
