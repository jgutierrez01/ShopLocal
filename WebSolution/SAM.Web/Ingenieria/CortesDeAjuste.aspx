<%@ Page  Language="C#" MasterPageFile="~/Masters/Ingenieria.master" AutoEventWireup="true"
    CodeBehind="CortesDeAjuste.aspx.cs" Inherits="SAM.Web.Ingenieria.CortesDeAjuste" %>

<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdDetalle">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdDetalle" LoadingPanelID="lpDetalle" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="contenedorCentral">
        <asp:Panel runat="server" ID="pnlSuperior">
            <div class="cajaFiltros">
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Label ID="lblProyecto" runat="server" CssClass="bold" meta:resourcekey="lblProyecto" />
                        <br />
                        <mimo:MappableDropDown ID="ddlProyecto" runat="server" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged"
                            EntityPropertyName="ProyectoID" AutoPostBack="true" CausesValidation="false" />
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto"
                            InitialValue="" Display="None" meta:resourcekey="rfvProyecto" />
                    </div>
                </div>
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Button runat="server" ID="btnMostrar" meta:resourcekey="btnMostrar" CssClass="divDerecho boton"
                            OnClick="btnMostrar_Click" />
                    </div>
                </div>
                <div class="divIzquierdo">
                    <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" />
                </div>
                <p>
                </p>
            </div>
        </asp:Panel>
        <p>
        </p>
        <div>
            <sam:Header ID="proyEncabezado" runat="server" Visible="False" />
        </div>
        <p>
        </p>
        <div>
            <telerik:RadAjaxLoadingPanel runat="server" ID="lpDetalle" />
            <mimo:MimossRadGrid runat="server" ID="grdDetalle" OnNeedDataSource="grdDetalle_OnNeedDataSource"
                OnItemCommand="grdDetalle_OnItemCommand" OnItemCreated="grdDetalle_OnItemCreated"
                OnItemDataBound="grdDetalle_ItemDataBound" Visible="false">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridBoundColumn UniqueName="MaterialSpoolID" DataField="MaterialSpoolID"
                            Visible="false" />
                        <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                            UniqueName="hlkAjustar_h" HeaderStyle-Width="30" meta:resourcekey="gbcHold">
                            <ItemTemplate>
                                <asp:HyperLink ImageUrl="~/Imagenes/Iconos/ico_ajustar.png" runat="server" ID="hypAjustar"
                                    meta:resourcekey="hypAjustar" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%-- <telerik:GridHyperLinkColumn UniqueName="hlkAjustar_h" HeaderStyle-Width="50" AllowFiltering="false"
                            meta:resourcekey="gbcAjustar" />--%>
                        <telerik:GridBoundColumn UniqueName="Proyecto" DataField="Proyecto" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcProyecto" />
                        <telerik:GridBoundColumn UniqueName="Spool" DataField="Spool" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcSpool" />
                        <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNumeroControl" />
                        <telerik:GridBoundColumn UniqueName="EtiquetaMaterial" DataField="EtiquetaMaterial"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcEtiquetaMaterial" />
                        <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" HeaderStyle-Width="250"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDescripcion" />
                        <telerik:GridBoundColumn UniqueName="LongitudIngenieria" DataField="LongitudIngenieria"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcLongitudIngenieria" />
                        <telerik:GridBoundColumn UniqueName="LongitudCorte" DataField="LongitudCorte" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcLongitudCorte" />
                        <telerik:GridBoundColumn UniqueName="Diferencia" DataField="Diferencia" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDiferencia" />
                        <telerik:GridBoundColumn UniqueName="Tolerancia" DataField="Tolerancia" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcTolerancia" />
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
        <div id="bntActualiza">
            <asp:Button runat="server" CausesValidation="false" ID="btnActualiza" CssClass="oculto"
                OnClick="btnActualiza_Click" />
        </div>
    </div>
</asp:Content>
