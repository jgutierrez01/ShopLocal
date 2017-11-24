<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="ReporteTT.aspx.cs" Inherits="SAM.Web.WorkStatus.ReporteTT" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdReporteTt">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReporteTt" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblReporteTt" CssClass="Titulo" meta:resourcekey="lblReporteTt"
            Text="REPORTE - TT"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" Text="Proyecto:" meta:resourcekey="lblProyecto"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedItemChanged" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvProyecto" runat="server" ControlToValidate="ddlProyecto"
                        InitialValue="" meta:resourcekey="valProyecto" Display="None" CssClass="bold" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblDesde" runat="server" Text="Desde:" meta:resourcekey="lblDesde"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpDesde" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblHasta" runat="server" Text="Hasta:" meta:resourcekey="lblHasta"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpHasta" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblNumeroDeReporte" runat="server" Text="Numero de Reporte:" meta:resourcekey="lblNumeroDeReporte"
                        CssClass="bold" />
                    <br />
                    <asp:TextBox ID="txtNumeroDeReporte" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblTipoDePrueba" runat="server" Text="Tipo de Prueba:" meta:resourcekey="lblTipoDePrueba"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlTipoDePrueba" meta:resourcekey="ddlTipoDePrueba"
                        AutoPostBack="false" />
                     <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvTipoPrueba" runat="server" ControlToValidate="ddlTipoDePrueba"
                        InitialValue="" meta:resourcekey="valTipoPrueba" Display="None" CssClass="bold" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" meta:resourcekey="btnMostrar"
                        CssClass="boton" OnClick="btnMostrar_OnClick" />
                </div>
            </div>
            <p></p>
            </div>
            <p></p>
        </div>
        <p>
        </p>
        <div>
            <uc1:Header ID="proyHeader" runat="server" Visible="false" />
        </div>
        <asp:ValidationSummary ID="valSummary" HeaderText="Errores" runat="server" DisplayMode="BulletList"
            CssClass="summaryList" meta:resourcekey="valSummary" />
        <p>
        </p>
        <asp:PlaceHolder runat="server" ID="phGrid" Visible="false">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid ID="grdReporteTt" runat="server" OnNeedDataSource="grdReporteTt_OnNeedDataSource"
                OnItemCommand="grdReporteTt_ItemCommand" OnItemDataBound="grdReporteTt_OnItemDataBound">
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
                        <telerik:GridTemplateColumn UniqueName="hlVer_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlVer" meta:resourcekey="hlVer" ImageUrl="~/Imagenes/Iconos/info.png"
                                    NavigateUrl='<%#Eval("ReporteTtID","~/WorkStatus/DetReporteTt.aspx?ID={0}") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                            UniqueName="inventario_h" HeaderStyle-Width="30">
                            <ItemTemplate>
                                <samweb:LinkVisorReportes ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" runat="server"
                                    ID="hdReporte" Visible="false" meta:resourcekey="hdReporte" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="hlDescargar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlDescargar" meta:resourcekey="hlDescargar" ImageUrl="~/Imagenes/Iconos/ico_descargar.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" Text="Borrar" ID="btnBorrar" runat="server"
                                    meta:resourcekey="btnBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("ReporteTtID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="HdNumeroDeReporte" DataField="NumeroDeReporte"
                            HeaderText="Numero de Reporte" HeaderStyle-Width="180" FilterControlWidth="100"
                            Groupable="false" meta:resourcekey="HdNumeroDeReporte" />
                        <telerik:GridBoundColumn UniqueName="HdFecha" DataField="Fecha" HeaderText="Fecha"
                            DataFormatString="{0:d}" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"
                            meta:resourcekey="HdFecha" />
                        <telerik:GridBoundColumn UniqueName="HdTipoDePrueba" DataField="TipoDePrueba" HeaderText="Tipo de Prueba"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="HdTipoDePrueba" />
                        <telerik:GridBoundColumn UniqueName="HdJuntasTotales" DataField="JuntasTotales" HeaderText="Juntas Totales" DataFormatString="{0:N0}"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="HdJuntasTotales" />
                        <telerik:GridBoundColumn UniqueName="HdJuntasAprobadas" DataField="JuntasAprobadas" DataFormatString="{0:N0}"
                            HeaderText="Juntas Aprobadas" HeaderStyle-Width="180" FilterControlWidth="100"
                            Groupable="false" meta:resourcekey="HdJuntasAprobadas" />
                        <telerik:GridBoundColumn UniqueName="HdJuntasRechazadas" DataField="JuntasRechazadas" DataFormatString="{0:N0}"
                            HeaderText="Juntas Rechazadas" HeaderStyle-Width="180" FilterControlWidth="100"
                            Groupable="false" meta:resourcekey="HdJuntasRechazadas" />
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
    </div>
</asp:Content>
