<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="ODTEspecial.aspx.cs" Inherits="SAM.Web.Produccion.OrdenTrabajoEspecial" %>


<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
   
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager runat="server" Style="display: none;" ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnMostrar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                    <telerik:AjaxUpdatedControl ControlID="grdSpoolsCompleto" UpdatePanelHeight="300px" LoadingPanelID="ldPanel2"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="valSummary" UpdatePanelCssClass=""></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdSpoolsCompleto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                    <telerik:AjaxUpdatedControl ControlID="grdSpoolsCompleto" UpdatePanelHeight="300px" LoadingPanelID="ldPanel2"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblIngenieria" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="contenedorCentral">
        <asp:Panel runat="server" ID="pnlSuperior">
            <div class="cajaFiltros" style="margin-bottom: 5px;">
                <div class="divIzquierdo">
                    <uc2:Filtro FiltroProyecto="true" ProyectoAutoPostBack="true" ProyectoRequerido="true"
                        FiltroNumeroEmbarque="false"
                        FiltroCuadrante="false"
                        FiltroNumeroControl="false"
                        FiltroNumeroUnico="false"
                        FiltroOrdenTrabajo="false"
                        ProyectoHeaderID="headerProyecto"
                        runat="server"
                        ID="filtroGenerico"></uc2:Filtro>
                </div>
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Button runat="server" ID="btnMostrar" OnClick="btnMostrar_Click" CssClass="boton"
                            meta:resourcekey="btnMostrar" />
                    </div>
                </div>
                <p>
                </p>
            </div>
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" meta:resourcekey="valSummary" />
        </asp:Panel>
        <div class="separador">
            <sam:Header ID="headerProyecto" runat="server" Visible="False" />
        </div>
        <p>
        </p>

        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
        

        <mimo:MimossRadGrid runat="server" ID="grdSpoolsCompleto" ShowFooter="True" Visible="false" AllowMultiRowSelection="true"
            MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false"
            ClientSettings-Scrolling-ScrollHeight="150">
            <ClientSettings>
                <Scrolling EnableVirtualScrollPaging="true" />
            </ClientSettings>
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand"
                ClientDataKeyNames="SpoolID" DataKeyNames="SpoolID" AllowFilteringByColumn="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:LinkButton ID="lnkMostrarConCruce" runat="server" CssClass="link" OnClick="lnkMostrarConCruce_Click" meta:resourcekey="lnkCruzar"></asp:LinkButton>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn meta:resourcekey="grdPrioridad" UniqueName="Prioridad" DataField="Prioridad" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridTemplateColumn meta:resourcekey="grdSpoolCol" FilterControlWidth="100" HeaderStyle-Width="150" SortExpression="Nombre" DataField="Nombre" DataType="System.String">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1C" runat="server" NavigateUrl='<%#Eval("SpoolID","javascript:Sam.Produccion.AbrePopupSpoolRevision({0});")%>' Text='<%# Eval("Nombre") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn meta:resourcekey="grdPDI" UniqueName="Pdis" DataField="Pdis" FilterControlWidth="30" HeaderStyle-Width="70" DataFormatString="{0:#0.000}" Aggregate="Sum" FooterAggregateFormatString="Total:<br />{0:F}" />
                    <telerik:GridBoundColumn meta:resourcekey="grdJuntas" UniqueName="Juntas" DataField="Juntas" FilterControlWidth="30" HeaderStyle-Width="70" />
                    <telerik:GridBoundColumn meta:resourcekey="grdTotalPeqs" UniqueName="TotalPeqs" DataField="TotalPeqs" FilterControlWidth="40" HeaderStyle-Width="80" DataFormatString="{0:#0.000}" />
                    <telerik:GridBoundColumn meta:resourcekey="grdPeso" UniqueName="Peso" DataField="Peso" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn meta:resourcekey="grdArea" UniqueName="Area" DataField="Area" FilterControlWidth="30" HeaderStyle-Width="70" />
                    <telerik:GridBoundColumn meta:resourcekey="grdCedula" UniqueName="Cedula" DataField="Cedula" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn meta:resourcekey="grdFamAcero" UniqueName="FamiliasAcero" DataField="FamiliasAcero" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn meta:resourcekey="grdTotalAccesorio" UniqueName="TotalAccesorio" DataField="TotalAccesorio" FilterControlWidth="30" HeaderStyle-Width="70" Display="false" />
                    <telerik:GridBoundColumn meta:resourcekey="grdTotalTubo" UniqueName="TotalTubo" DataField="TotalTubo" FilterControlWidth="30" HeaderStyle-Width="70" Display="false" />
                    <telerik:GridBoundColumn meta:resourcekey="grdLongitudTubo" UniqueName="LongitudTubo" DataField="LongitudTubo" FilterControlWidth="30" HeaderStyle-Width="70" Display="false" />
                    <telerik:GridBoundColumn meta:resourcekey="grdDibujo" UniqueName="Dibujo" DataField="Dibujo" FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn meta:resourcekey="grdDiametroPlano" UniqueName="DiametroPlano" DataField="DiametroPlano" FilterControlWidth="100" HeaderStyle-Width="150" DataFormatString="{0:#0.000}" />
                    <telerik:GridCheckBoxColumn meta:resourcekey="grdHold" UniqueName="Hold" DataField="Hold" FilterControlWidth="40" HeaderStyle-Width="50" ReadOnly="true" />
                    <telerik:GridBoundColumn meta:resourcekey="grdObservaciones" UniqueName="ObservacionesHold" DataField="ObservacionesHold" FilterControlWidth="100" HeaderStyle-Width="250" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>

    </div>
    <div class="contenedorCentral">
        <mimo:MimossRadGrid runat="server" ID="grdSpools" OnNeedDataSource="grdSpools_NeedDataSource"
            ShowFooter="True" Visible="false" AllowMultiRowSelection="true"
            MasterTableView-CommandItemSettings-ShowAddNewRecordButton="false" OnItemCreated="grdSpools_ItemCreated"
            ClientSettings-Scrolling-ScrollHeight="300" GroupPanel-Height="300" GroupPanel-PanelStyle-Height="300"
            Height="300">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand"
                ClientDataKeyNames="SpoolID" DataKeyNames="SpoolID" AllowFilteringByColumn="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink meta:resourcekey="lnkGeneraRequisición" runat="server" ID="lnGenerarRequisición" CssClass="link" />
                        <asp:HyperLink meta:resourcekey="imgActualizar" runat="server" ID="imgAgregarOrden" ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h" HeaderStyle-Width="30" />
                    <telerik:GridBoundColumn meta:resourcekey="grdPrioridad" UniqueName="Prioridad" DataField="Prioridad" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridTemplateColumn meta:resourcekey="grdSpoolCol" FilterControlWidth="100" HeaderStyle-Width="150" SortExpression="Nombre" DataField="Nombre" DataType="System.String">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%#Eval("SpoolID","javascript:Sam.Produccion.AbrePopupSpoolRevision({0});")%>' Text='<%# Eval("Nombre") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn meta:resourcekey="grdPDI" UniqueName="Pdis" DataField="Pdis" FilterControlWidth="30" HeaderStyle-Width="70" DataFormatString="{0:#0.000}" Aggregate="Sum" FooterAggregateFormatString="Total:<br />{0:F}" />
                    <telerik:GridBoundColumn meta:resourcekey="grdJuntas" UniqueName="Juntas" DataField="Juntas" FilterControlWidth="30" HeaderStyle-Width="70" />
                    <telerik:GridBoundColumn meta:resourcekey="grdTotalPeqs" UniqueName="TotalPeqs" DataField="TotalPeqs" FilterControlWidth="40" HeaderStyle-Width="80" DataFormatString="{0:#0.000}" />
                    <telerik:GridBoundColumn meta:resourcekey="grdPeso" UniqueName="Peso" DataField="Peso" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn meta:resourcekey="grdArea" UniqueName="Area" DataField="Area" FilterControlWidth="30" HeaderStyle-Width="70" />
                    <telerik:GridBoundColumn meta:resourcekey="grdCedula" UniqueName="Cedula" DataField="Cedula" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn meta:resourcekey="grdFamAcero" UniqueName="FamiliasAcero" DataField="FamiliasAcero" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn meta:resourcekey="grdTotalAccesorio" UniqueName="TotalAccesorio" DataField="TotalAccesorio" FilterControlWidth="30" HeaderStyle-Width="70" Display="false" />
                    <telerik:GridBoundColumn meta:resourcekey="grdTotalTubo" UniqueName="TotalTubo" DataField="TotalTubo" FilterControlWidth="30" HeaderStyle-Width="70" Display="false" />
                    <telerik:GridBoundColumn meta:resourcekey="grdLongitudTubo" UniqueName="LongitudTubo" DataField="LongitudTubo" FilterControlWidth="30" HeaderStyle-Width="70" Display="false" />
                    <telerik:GridBoundColumn meta:resourcekey="grdDibujo" UniqueName="Dibujo" DataField="Dibujo" FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn meta:resourcekey="grdDiametroPlano" UniqueName="DiametroPlano" DataField="DiametroPlano" FilterControlWidth="100" HeaderStyle-Width="150" DataFormatString="{0:#0.000}" />
                    <telerik:GridCheckBoxColumn meta:resourcekey="grdHold" UniqueName="Hold" DataField="Hold" FilterControlWidth="40" HeaderStyle-Width="50" ReadOnly="true" />
                    <telerik:GridBoundColumn meta:resourcekey="grdObservaciones" UniqueName="ObservacionesHold" DataField="ObservacionesHold" FilterControlWidth="100" HeaderStyle-Width="250" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>

        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel2" Height="300">
        </telerik:RadAjaxLoadingPanel>
    </div>
    <div>

        <telerik:RadWindowManager ID="windowManager" runat="server">
            <Windows>
                <telerik:RadWindow ID="rdwTalleres" runat="server" CssClass="popups" Modal="true" ReloadOnShow="true"
                    VisibleStatusbar="false" Height="350" Width="550">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>


    </div>
    <div id="btnWrapper">
        <asp:Button runat="server" CausesValidation="false" ID="btnQuitaSeleccionados" CssClass="oculto" OnClick="btnQuitaSeleccionados_Click" />
    </div>

</asp:Content>
