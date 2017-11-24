<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="RepRequisiciones.aspx.cs" Inherits="SAM.Web.WorkStatus.RepRequisiciones" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlPatio">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlProyecto" />
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdRequisiciones">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdRequisiciones" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" meta:resourcekey="lblProyecto" CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedItemChanged" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvProyecto" runat="server" ControlToValidate="ddlProyecto"
                        InitialValue="" meta:resourcekey="rfvProyecto" Display="None" CssClass="bold" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblDesde" runat="server" meta:resourcekey="lblDesde" CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpDesde" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblHasta" runat="server" meta:resourcekey="lblHasta" CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpHasta" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblNumeroRequisicion" runat="server" meta:resourcekey="lblNumeroRequisicion"
                        CssClass="bold" />
                    <br />
                    <asp:TextBox ID="txtNumeroRequisicion" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblTipoPrueba" runat="server" meta:resourcekey="lblTipoPrueba" CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlTipoPrueba" EntityPropertyName="TipoPruebaID"
                        AutoPostBack="true" />
                     <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvTipoPrueba" runat="server" ControlToValidate="ddlTipoPrueba"
                        InitialValue="" meta:resourcekey="valTipoPrueba" Display="None" CssClass="bold" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" meta:resourcekey="btnMostrar" CssClass="boton"
                        OnClick="btnMostrar_OnClick" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="summaryList" meta:resourcekey="valSummary" />
        <div class="separador">
            <sam:Header ID="proyEncabezado" runat="server" Visible="false" />
        </div>
        <p>
        </p>
        <asp:PlaceHolder runat="server" ID="phGrid" Visible="false">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid ID="grdRequisiciones" runat="server" OnNeedDataSource="grdRequisiciones_OnNeedDataSource"
                OnItemDataBound="grdRequisiciones_ItemDataBound" OnItemCommand="grdRequisiciones_ItemCommand">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:LinkButton runat="server" ID="lnkActualizar" CausesValidation="false" meta:resourcekey="lnkActualizar"
                                CssClass="link" OnClick="lnkActualizar_onClick" />
                            <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png"
                                CausesValidation="false" AlternateText="Actualizar" CssClass="imgEncabezado"
                                OnClick="lnkActualizar_onClick" meta:resourcekey="imgActualizar" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="btnDetalle_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlDetalle" ImageUrl="~/Imagenes/Iconos/info.png"
                                    NavigateUrl='<%#Eval("RequisicionID","~/WorkStatus/DetRequisicion.aspx?ID={0}") %>'
                                    meta:resourcekey="hlDetalle" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                            UniqueName="inventario_h" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <samweb:LinkVisorReportes ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" runat="server"
                                    ID="hdReporte" meta:resourcekey="hdReporte" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Descargar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlDescargar" ImageUrl="~/Imagenes/Iconos/ico_descargar.png"
                                    Visible="false" meta:resourcekey="hlDescargar" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" ID="btnBorrar" runat="server" meta:resourcekey="imgBorrar"
                                    ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("RequisicionID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="NumeroRequisicion" DataField="NumeroRequisicion"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNumeroRequisicion" />
                        <telerik:GridBoundColumn UniqueName="Fecha" DataField="Fecha" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" DataFormatString="{0:d}" meta:resourcekey="gbcFecha" />
                        <telerik:GridBoundColumn UniqueName="Tipo" DataField="Tipo" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcTipo" />
                        <telerik:GridBoundColumn UniqueName="JuntasTotales" DataField="JuntasTotales" HeaderStyle-Width="180" DataFormatString="{0:N0}"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcJuntasTotales" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                            Reorderable="false" ShowSortIcon="false">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </mimo:MimossRadGrid>
        </asp:PlaceHolder>
        <p>
        </p>
    </div>
</asp:Content>
