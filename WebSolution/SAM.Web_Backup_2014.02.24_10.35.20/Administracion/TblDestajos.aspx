<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.master" AutoEventWireup="true" CodeBehind="TblDestajos.aspx.cs" Inherits="SAM.Web.Administracion.TblDestajos" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>

<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="encabezadoProyecto" TagPrefix="ctrl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdDestajos">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdTblDestajos" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="perfil">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="perfil" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTblDestajos" CssClass="Titulo" meta:resourcekey="lblTblDestajos" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <span class="required">*</span>
                    <asp:Label runat="server" ID="lblProyecto" Text="Proyecto:" CssClass="bold" meta:resourcekey="lblProyecto" />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID" AutoPostBack="True" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" CssClass="labelHack" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto" InitialValue="" Display="None"
                         ErrorMessage="El Proyecto es requerido" meta:resourcekey="rfvProyecto" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <span class="required">*</span>
                    <asp:Label runat="server" ID="lblFamiliaAcero" Text="Familia de Acero:" CssClass="bold" meta:resourcekey="lblFamiliaAcero" />
                    <mimo:MappableDropDown runat="server" ID="ddlFamiliaAcero" EntityPropertyName="FamiliaAceroID" CssClass="labelHack" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvFamiliaAcero" ControlToValidate="ddlFamiliaAcero" InitialValue="" Display="None"
                         ErrorMessage="La Familia de Acero es requerida" meta:resourcekey="rfvFamiliaAcero" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <span class="required">*</span>
                    <asp:Label runat="server" ID="lblTipoJunta" Text="Tipo de Junta:" CssClass="bold" meta:resourcekey="lblTipoJunta" />
                    <mimo:MappableDropDown runat="server" ID="ddlTipoJunta" EntityPropertyName="TipoJuntaID" CssClass="labelHack" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvTipoJunta" ControlToValidate="ddlTipoJunta" InitialValue="" Display="None"
                         ErrorMessage="El Tipo de Junta es requerido" meta:resourcekey="rfvTipoJunta" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <span class="required">*</span>
                    <asp:Label runat="server" ID="lblProceso" Text="Proceso:" CssClass="bold" meta:resourcekey="lblProceso" />
                    <mimo:MappableDropDown runat="server" ID="ddlProceso" CssClass="labelHack" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvProceso" ControlToValidate="ddlProceso" InitialValue="" Display="None"
                         ErrorMessage="El Proceso es requerido" meta:resourcekey="rfvProceso" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button runat="server" ID="btnMostrar" Text="Mostrar" CssClass="boton" OnClick="btnMostrar_Click" meta:resourcekey="btnMostrar" />
                </div>
            </div>
            <p></p>
        </div>

        <p></p>
        <ctrl:encabezadoProyecto ID="proyEncabezado" runat="server" Visible="false" />
        <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" />
        <p></p>

        <mimo:MimossRadGrid runat="server" ID="grdTblDestajos" Height="500px" OnNeedDataSource="grdTblDestajos_OnNeedNataSource" OnItemDataBound="grdTblDestajos_OnItemDataBound" OnItemCommand="grdTblDestajos_OnItemCommand" OnItemCreated="grdTblDestajos_OnItemCreated" AllowPaging="false" >
               <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" AllowFilteringByColumn="false" EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false" >
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <div class="tituloGrid">
                                <asp:Label runat="server" ID="lblTblDestajo" CssClass="Titulo" meta:resourcekey="lblTblDestajo" />
                            </div>
                            <asp:HyperLink ID="lnkExportar" Text="Exportar a Excel" runat="server" meta:resourcekey="lnkExportar"></asp:HyperLink>
                            <asp:HyperLink runat="server" ID="imgExportar" ImageUrl="~/Imagenes/Iconos/excel.png"  AlternateText="ExportarExcelDestajos" meta:resourcekey="imgExportar"/>
                            <span>&nbsp&nbsp&nbsp&nbsp</span>
                            <asp:LinkButton runat="server" ID="lnkAgregar" CommandName="Agregar" Text="Subir Archivo" CssClass="link" meta:resourcekey="lnkAgregar" />
                            <asp:ImageButton runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" meta:resourcekey="imgAgregar" />
                        </div>  
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridBoundColumn UniqueName="Diametro" DataField="Valor" DataFormatString="{0:#0.000}" Reorderable="false" Resizable="false" meta:resourcekey="grdDiametro"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" AllowFiltering="false" AllowSorting="false" />
                    </Columns>
               </MasterTableView>
               <ClientSettings>
                    <Scrolling FrozenColumnsCount="1" UseStaticHeaders="true" AllowScroll="true" />
               </ClientSettings>
        </mimo:MimossRadGrid>

    </div>
</asp:Content>
