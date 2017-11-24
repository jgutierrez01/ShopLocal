<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="LstPendientes.aspx.cs" Inherits="SAM.Web.Administracion.LstPendientes" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblPatio" runat="server" Text="Patio:" meta:resourcekey="lblPatio"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlPatio" EntityPropertyName="PatioID"
                        OnSelectedIndexChanged="DdlPatioOnSelectedIndexChanged" AutoPostBack="true" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvPatio" runat="server" ControlToValidate="ddlPatio"
                        InitialValue="" meta:resourcekey="valPatio" Display="None" CssClass="bold" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label runat="server" ID="lblProyecto" CssClass="bold" meta:resourcekey="lblProyecto" />
                    <br />
                    <asp:DropDownList runat="server" ID="ddlProyecto" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador" style="padding-top: 12px">
                    <asp:CheckBox runat="server" ID="chkMostrarTodos" CausesValidation="false" />
                    <asp:Label runat="server" ID="lblVerTodos" CssClass="bold inline" meta:resourcekey="chkVerTodos" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button runat="server" ID="btnMostrar" CssClass="boton" OnClick="btnMostrar_Click"
                        meta:resourcekey="btnMostrar" />
                </div>
            </div>
            <asp:CustomValidator runat="server" ID="cvSeleccionFiltro" Display="None" OnServerValidate="cvSeleccionFiltro_OnServerValidate"
                meta:resourcekey="cvSeleccionFiltro" />
            <p>
            </p>
        </div>
        <br />
        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" meta:resourcekey="valSummary" />
        <br />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdPendientes" OnNeedDataSource="grdPendientes_OnNeedDataSource"
            OnItemCreated="grdPendientes_OnItemCreated" OnItemDataBound="grdPendientes_OnItemDataBound"
            Visible="false">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label runat="server" ID="lblTituloGrid" meta:resourcekey="lblTituloGrid" />
                        </div>
                        <asp:HyperLink runat="server" ID="lnkAgregar" CssClass="link" meta:resourcekey="lnkAgregar" />
                        <asp:HyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png"
                            CssClass="imgEncabezado" meta:resourcekey="imgAgregar" />
                        <asp:LinkButton runat="server" ID="lnkActualizar" OnClick="lnkActualizar_OnClick"
                            CausesValidation="false" meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png"
                            CausesValidation="false" AlternateText="Actualizar" CssClass="imgEncabezado"
                            meta:resourcekey="imgActualizar" />
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
                    <telerik:GridBoundColumn UniqueName="PendienteID" DataField="PendienteID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="NombreProyecto" DataField="NombreProyecto" FilterControlWidth="150"
                        HeaderStyle-Width="200" meta:resourcekey="gbcNombreProyecto" />
                    <telerik:GridBoundColumn UniqueName="Titulo" DataField="Titulo" FilterControlWidth="150"
                        HeaderStyle-Width="200" meta:resourcekey="gbcTitulo" />
                    <telerik:GridBoundColumn UniqueName="Area" DataField="Area" FilterControlWidth="150"
                        HeaderStyle-Width="200" meta:resourcekey="gbcArea" />
                    <telerik:GridBoundColumn UniqueName="Responsable" DataField="Responsable" FilterControlWidth="150"
                        HeaderStyle-Width="200" meta:resourcekey="gbcResponsable" />
                    <telerik:GridBoundColumn UniqueName="DescripcionEstatus" DataField="DescripcionEstatus"
                        FilterControlWidth="150" HeaderStyle-Width="200" meta:resourcekey="gbcEstatus" />
                    <telerik:GridBoundColumn UniqueName="Autor" DataField="Autor" FilterControlWidth="150"
                        HeaderStyle-Width="200" meta:resourcekey="gbcAutor" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
        <div id="btnWrapper" class="oculto">
            <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="False"
                OnClick="lnkActualizar_OnClick" />
        </div>
    </div>
</asp:Content>
<asp:Content ID="cntFoot" ContentPlaceHolderID="cphInnerFoot" runat="server">
</asp:Content>
