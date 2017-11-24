<%@ Page Language="C#" MasterPageFile="~/Masters/Ingenieria.master" AutoEventWireup="true"
    CodeBehind="RevisionesIngenieria.aspx.cs" Inherits="SAM.Web.Ingenieria.RevisionesIngenieria" %>

<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/JuntaROHistorico.ascx" TagName="DetJunta" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/MaterialROHistorico.ascx" TagName="DetMaterial" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/CorteROHistorico.ascx" TagName="DetCorte" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:radajaxmanager runat="server" style="display: none;" id="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnRefresh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:radajaxmanager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblIngenieria" CssClass="Titulo" meta:resourcekey="lblIngenieria" />
    </div>
    <div class="contenedorCentral">
        <asp:Panel runat="server" ID="pnlSuperior">
            <div class="cajaFiltros" style="margin-bottom: 5px;">
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Label ID="lblProyecto" runat="server" CssClass="bold" meta:resourcekey="lblProyecto" />                       
                        <mimo:mappabledropdown id="ddlProyecto" runat="server" onselectedindexchanged="ddlProyecto_SelectedIndexChanged"
                            entitypropertyname="ProyectoID" autopostback="true" causesvalidation="false" />
                        <span class="required">*</span>
                         <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto"
                            InitialValue="" Display="None" meta:resourcekey="rfvProyecto" />
                    </div>
                </div>
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Label runat="server" ID="lblSpool" meta:resourcekey="lblSpool" CssClass="bold" />
                        <telerik:RadComboBox ID="rcbSpool" runat="server" Width="200px" Height="150px" EmptyMessage=" "
                            EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                            OnClientItemsRequesting="Sam.WebService.AgregaProyectoID">
                            <WebServiceSettings Method="ListaHistoricoSpoolsPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                        </telerik:RadComboBox>
                        <span class="required">*</span>
                        <asp:CustomValidator    
                        meta:resourcekey="valSpool"
                        runat="server" 
                        ID="valSpool" 
                        Display="None" 
                        ControlToValidate="rcbSpool" 
                        ValidateEmptyText="true"                         
                        ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                        OnServerValidate="cusRcbSpool_ServerValidate" />                         
                    </div>
                </div>
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Button runat="server" ID="btnMostrar" OnClick="btnMostrar_Click" CssClass="boton" meta:resourcekey="btnMostrar" />
                    </div>
                </div>
                <p></p>
            </div>
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" meta:resourcekey="valSummary" />
        </asp:Panel>
        <div class="separador">
            <sam:header id="proyEncabezado" runat="server" visible="False" />
        </div>
        <p>
        </p>
        <telerik:radajaxloadingpanel runat="server" id="ldPanel">
        </telerik:radajaxloadingpanel>
        <mimo:mimossradgrid runat="server" id="grdSpools" onneeddatasource="grdSpools_OnNeedDataSource"
            showfooter="True" onitemcommand="grdSpools_OnItemCommand" onitemcreated="grdSpools_OnItemCreated"
            onitemdatabound="grdSpools_ItemDataBound" visible="false" allowmultirowselection="true">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand"
                ClientDataKeyNames="SpoolID" DataKeyNames="SpoolID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn UniqueName="SpoolID" DataField="SpoolID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Spool" DataField="Nombre" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcSpool" />
                    <telerik:GridBoundColumn UniqueName="Prioridad" DataField="Prioridad" HeaderStyle-Width="100"
                        FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcPrioridad" />
                    <telerik:GridBoundColumn UniqueName="Dibujo" DataField="Dibujo" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDibujo" />
                    <telerik:GridBoundColumn UniqueName="RevisionCliente" DataField="RevisionCliente"
                        HeaderStyle-Width="120" FilterControlWidth="80" Groupable="false" meta:resourcekey="gbcRevCliente" />
                    <telerik:GridBoundColumn UniqueName="Especificacion" DataField="Especificacion" HeaderStyle-Width="100"
                        FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcEspecificacion" />
                    <telerik:GridBoundColumn UniqueName="DiametroPlano" DataField="DiametroPlano" HeaderStyle-Width="100"
                        DataFormatString="{0:N3}" FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcDiametroPlano" />
                    <telerik:GridBoundColumn UniqueName="Revision" DataField="RevisionSteelgo" HeaderStyle-Width="100"
                        FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcRevSteelGo" />
                    <telerik:GridBoundColumn UniqueName="PorcentajePnd" DataField="PorcentajePnd" HeaderStyle-Width="80"
                        FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcPnd" />
                    <telerik:GridBoundColumn UniqueName="Material" DataField="FamiliasAcero" HeaderStyle-Width="90"
                        FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcMaterial" />
                    <telerik:GridBoundColumn UniqueName="Segmento1" DataField="Segmento1" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento1" />
                    <telerik:GridBoundColumn UniqueName="Segmento2" DataField="Segmento2" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento2" />
                    <telerik:GridBoundColumn UniqueName="Segmento3" DataField="Segmento3" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento3" />
                    <telerik:GridBoundColumn UniqueName="Segmento4" DataField="Segmento4" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento4" />
                    <telerik:GridBoundColumn UniqueName="Segmento5" DataField="Segmento5" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento5" />
                    <telerik:GridBoundColumn UniqueName="Segmento6" DataField="Segmento6" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento6" />
                    <telerik:GridBoundColumn UniqueName="Segmento7" DataField="Segmento7" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento7" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NestedViewTemplate>
                    <telerik:RadTabStrip runat="server" ID="tabStrip" MultiPageID="rmpDetalle" SelectedIndex="0">
                        <Tabs>
                            <telerik:RadTab runat="server" meta:resourcekey="rdMateriales" />
                            <telerik:RadTab runat="server" meta:resourcekey="rdJuntas" />
                            <telerik:RadTab runat="server" meta:resourcekey="rdCortes" />
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage ID="rmpDetalle" runat="server" SelectedIndex="0">
                        <telerik:RadPageView ID="rpvMateriales" runat="server">
                            <sam:DetMaterial ID="materiales" runat="server" />
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="rpvJuntas" runat="server">
                            <sam:DetJunta ID="juntas" runat="server" />
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="rpvCortes" runat="server">
                            <sam:DetCorte ID="cortes" runat="server" />
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </NestedViewTemplate>
            </MasterTableView>
        </mimo:mimossradgrid>
    </div>
</asp:Content>
