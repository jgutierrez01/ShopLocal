<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="LstPeq.aspx.cs" Inherits="SAM.Web.Catalogos.LstPeq" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>

<asp:Content ID="CntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdLstPeq">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdLstPeq" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="perfil">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="perfil" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblLstPeq" CssClass="Titulo" meta:resourcekey="lblLstPeq" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" CssClass="bold" meta:resourcekey="lblProyecto" />
                    <br />
                    <mimo:MappableDropDown ID="ddlProyecto" runat="server" EntityPropertyName="ProyectoID" OnSelectedIndexChanged="ddlProyecto_SelectedChanged" AutoPostBack="true" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto"
                        InitialValue="" Display="None" meta:resourcekey="rfvProyecto" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label runat="server" ID="lblTipoJunta" Text="Tipo de Junta:" CssClass="bold" meta:resourcekey="lblTipoJunta" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlTipoJunta" EntityPropertyName="TipoJuntaID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="valTipoJunta" ControlToValidate="ddlTipoJunta" InitialValue=""
                        Display="None" ErrorMessage="Tipo de junta requerido" meta:resourcekey="rfvTipoJunta"/>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label runat="server" ID="lblFamiliaAcero" Text="Familia de Acero:" CssClass="bold" meta:resourcekey="lblFamiliaAcero" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlFamiliaAcero" EntityPropertyName="FamiliaAceroID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="valFamiliaAcero" ControlToValidate="ddlFamiliaAcero" InitialValue=""
                        Display="None" ErrorMessage="Familia de acero requerido" meta:resourcekey="rfvFamAcero"/>
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
        <sam:Header ID="proyHeader" runat="server" Visible="false" />
            <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" />
        <p></p>

        <mimo:MimossRadGrid runat="server" ID="grdLstPeq" Height="500px" OnNeedDataSource="grdLstPeq_OnNeedDataSource" OnItemDataBound="grdLstPeq_OnItemDataBound" OnItemCommand="grdLstPeq_ItemCommand" AllowPaging="false" Visible="false" >
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" AllowFilteringByColumn="false" EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label runat="server" ID="lblLstPeqs" CssClass="Titulo" meta:resourcekey="lblLstPeqs" />
                        </div>
                        <asp:LinkButton runat="server" ID="lnkAgregar" CommandName="Agregar" Text="Subir archivo" CssClass="link" meta:resourcekey="lnkAgregar"  />
                        <asp:ImageButton runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" meta:resourcekey="imgAgregar"  />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn UniqueName="Diametro" DataField="Valor" DataFormatString="{0:N3}" Reorderable="false" Resizable="false" meta:resourcekey="grdDiametro"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" AllowFiltering="false" AllowSorting="false" />  
                </Columns>
            </MasterTableView>
            <ClientSettings>
                <Scrolling FrozenColumnsCount="1" UseStaticHeaders="true" AllowScroll="true" />
            </ClientSettings>
        </mimo:MimossRadGrid>
    </div>
</asp:Content>
