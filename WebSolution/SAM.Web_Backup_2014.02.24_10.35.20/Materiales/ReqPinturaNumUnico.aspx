<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true"
    CodeBehind="ReqPinturaNumUnico.aspx.cs" Inherits="SAM.Web.Materiales.ReqPinturaNumUnico" %>

<%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdRequisicion">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdRequisicion" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros" style="margin-bottom: 5px;">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblProyecto" runat="server" ID="lblProyecto" CssClass="bold"
                        AssociatedControlID="ddlProyecto" />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator meta:resourcekey="rfvProyecto" runat="server" ID="rfvProyecto"
                        ControlToValidate="ddlProyecto" Display="None" ValidationGroup="" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" ValidationGroup=""
                        OnClick="btnMostrar_Click" CssClass="boton" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <asp:ValidationSummary ID="valSummary" runat="server" ValidationGroup="" CssClass="summaryList"
            meta:resourcekey="valSummary" />
        <div class="separador">
            <sam:Header ID="proyEncabezado" runat="server" Visible="false" />
        </div>
        <p>
        </p>
        <%--RadGrid--%>
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdRequisicion" Visible="false" OnNeedDataSource="grdRequisicion_OnNeedDataSource" 
            OnItemCreated="grdRequisicion_OnItemCreated" AllowMultiRowSelection="true">
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="NumeroUnicoID" ClientDataKeyNames="NumeroUnicoID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink meta:resourcekey="lnkRequisicion" runat="server" ID="lnkRequisicion"
                            CssClass="link" />
                        <asp:HyperLink meta:resourcekey="imgRequisicion" runat="server" ID="imgRequisicion"
                            ImageUrl="~/Imagenes/Iconos/icono_generarequisicion.png" CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn UniqueName="chkSelect_h" Display="true" HeaderStyle-Width="35" />
                    <telerik:GridBoundColumn UniqueName="NumeroUnico" DataField="NumeroUnico" meta:resourcekey="gbcNumeroUnico"
                        FilterControlWidth="100" HeaderStyle-Width="140" />
                    <telerik:GridBoundColumn UniqueName="ItemCode" DataField="ItemCode" meta:resourcekey="gbcItemCode"
                        FilterControlWidth="100" HeaderStyle-Width="140" />
                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" meta:resourcekey="gbcDescripcion"
                        FilterControlWidth="150" HeaderStyle-Width="330" />
                    <telerik:GridBoundColumn UniqueName="Diametro1" DataField="Diametro1" DataFormatString="{0:N3}" meta:resourcekey="gbcDiametro1"
                        FilterControlWidth="60" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="Diametro2" DataField="Diametro2" DataFormatString="{0:N3}" meta:resourcekey="gbcDiametro2"
                        FilterControlWidth="60" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="Recibido" DataField="Recibido" meta:resourcekey="gbcRecibido" DataFormatString="{0:N0}"
                        FilterControlWidth="60" HeaderStyle-Width="100" />
                    <telerik:GridBoundColumn UniqueName="Fisico" DataField="Fisico" meta:resourcekey="gbcFisico" DataFormatString="{0:N0}"
                        FilterControlWidth="60" HeaderStyle-Width="100" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
         <div id="bntActualiza">
            <asp:Button runat="server" CausesValidation="false" ID="btnActualiza" CssClass="oculto"
                OnClick="btnActualiza_Click" />
        </div>
</asp:Content>
