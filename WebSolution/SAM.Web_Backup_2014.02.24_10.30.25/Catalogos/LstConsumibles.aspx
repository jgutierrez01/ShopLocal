<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LstConsumibles.aspx.cs"
    Inherits="SAM.Web.Catalogos.LstConsumibles" MasterPageFile="~/Masters/Catalogos.master" %>
    <%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdConsumibles">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdConsumibles" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblConsumibles" CssClass="Titulo" meta:resourcekey="lblConsumibles" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblPatio" runat="server" CssClass="bold" meta:resourcekey="lblPatio"></asp:Label><br />
                    <asp:DropDownList ID="ddlPatio" runat="server">
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valPatio" runat="server" ControlToValidate="ddlPatio"
                        Display="None" InitialValue="" meta:resourcekey="valPatio"></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" meta:resourcekey="btnMostrar" CssClass="boton" OnClick="btnMostrar_Click"
                        runat="server" />
                </div>
            </div>
            <p></p>
        </div>
        <br />
        <asp:ValidationSummary meta:resourcekey="valSummary" runat="server" ID="valSummary"
            CssClass="summaryList" />
            
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdConsumibles" OnNeedDataSource="grdConsumibles_OnNeedDataSource"
            OnItemCommand="grdConsumibles_ItemCommand" Visible="false" OnItemCreated="grdConsumibles_ItemCreated">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblConsumible" runat="server" ID="lblConsumible" />
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlAgregar" meta:resourcekey="hlAgregar" CssClass="link"/>
                        <samweb:AuthenticatedHyperLink runat="server" meta:resourcekey="imgAgregar" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png"
                            CssClass="imgEncabezado" />
                        <asp:LinkButton runat="server" ID="hlActualizar" meta:resourcekey="hlActualizar"
                            CssClass="link" OnClick="lnkActualizar_OnClick" />
                        <asp:ImageButton runat="server" meta:resourcekey="imgActualizarImg" ID="imgActualizarImg"
                            ImageUrl="~/Imagenes/Iconos/actualizar.png" CssClass="imgEncabezado" OnClick="lnkActualizar_OnClick" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="editar_h" AllowFiltering="false" Groupable="false"
                        ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink meta:resourcekey="hlEditar" runat="server" ID="hlEditar" NavigateUrl='<%#Eval("ConsumibleID", "~/Catalogos/DetConsumibles.aspx?ID={0}")%>'
                                ImageUrl="~/Imagenes/Iconos/editar.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false">
                        <ItemTemplate>
                            <asp:ImageButton meta:resourcekey="imgBorrar" runat="server" CommandArgument='<%#Eval("ConsumibleID") %>'
                                CommandName="Borrar" ImageUrl="~/Imagenes/Iconos/borrar.png" ID="lnkBorrar" OnClientClick="return Sam.Confirma(1);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="Codigo" DataField="Codigo" meta:resourcekey="htCodigo"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Kilogramos" DataField="Kilogramos" meta:resourcekey="htKilogramos" DataFormatString="{0:N}"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" />
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
