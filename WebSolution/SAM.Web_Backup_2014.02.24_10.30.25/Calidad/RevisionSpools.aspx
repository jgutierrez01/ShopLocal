<%@ Page  Language="C#" MasterPageFile="~/Masters/Calidad.Master" AutoEventWireup="true"
    CodeBehind="RevisionSpools.aspx.cs" Inherits="SAM.Web.Calidad.RevisionSpools" %>
<%@ MasterType VirtualPath="~/Masters/Calidad.Master" %>

<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/JuntaRO.ascx" TagName="DetJunta" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/MaterialRO.ascx" TagName="DetMaterial" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/CorteRO.ascx" TagName="DetCorte" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnlSuperior" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="grdPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnMostrar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="grdPanel" />
                    <telerik:AjaxUpdatedControl ControlID="pnlSuperior" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblIngenieria" CssClass="Titulo" meta:resourcekey="lblRevisionSpool" />
    </div>
    <div class="contenedorCentral">
        <asp:Panel runat="server" ID="pnlSuperior">
            <div class="cajaFiltros">
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Label ID="lblProyecto" runat="server" CssClass="bold" meta:resourcekey="lblProyecto" />
                        <br />
                        <mimo:MappableDropDown ID="ddlProyecto" runat="server" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged"
                            EntityPropertyName="ProyectoID" AutoPostBack="true" CausesValidation="true" />
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
            <telerik:RadAjaxLoadingPanel runat="server" ID="grdPanel">
            </telerik:RadAjaxLoadingPanel>
            <mimo:MimossRadGrid runat="server" ID="grdSpools" OnNeedDataSource="grdSpools_OnNeedDataSource"
                OnItemCommand="grdSpools_OnItemCommand" OnItemDataBound="grdSpools_ItemDataBound"
                Visible="false">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand"
                    DataKeyNames="SpoolID">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar"
                                OnClick="lnkActualizar_OnClick" CssClass="link" CausesValidation="false" />
                            <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar"
                                ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_OnClick" CssClass="imgEncabezado"
                                CausesValidation="false" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridBoundColumn UniqueName="SpoolID" DataField="SpoolID" Visible="false" />
                        <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                            UniqueName="hlkHold_h" HeaderStyle-Width="50" meta:resourcekey="gbcHold">
                            <ItemTemplate>
                                <asp:HyperLink ImageUrl="~/Imagenes/Iconos/ico_holdB.png" runat="server" ID="hypPonerHold"
                                    Visible="false" meta:resourcekey="hypPonerHold" />
                                <asp:HyperLink ImageUrl="~/Imagenes/Iconos/ico_quitarholdB.png" runat="server" ID="hypQuitarHold"
                                    Visible="false" meta:resourcekey="hypQuitarHold" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridCheckBoxColumn UniqueName="TieneHoldCalidad" DataField="TieneHoldCalidad"
                            Visible="false" />
                        <telerik:GridCheckBoxColumn UniqueName="TieneHoldIngenieria" DataField="TieneHoldIngenieria"
                            HeaderStyle-Width="80" FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcHoldIngenieria" />
                            <telerik:GridCheckBoxColumn UniqueName="Confinado" DataField="Confinado" HeaderStyle-Width="80"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcConfinado" />
                        <telerik:GridBoundColumn UniqueName="Spool" DataField="Nombre" HeaderStyle-Width="150"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcSpool" />
                        <telerik:GridBoundColumn UniqueName="Prioridad" DataField="Prioridad" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcPrioridad" />
                        <telerik:GridBoundColumn UniqueName="Dibujo" DataField="Dibujo" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDibujo" />
                        <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" HeaderStyle-Width="100"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcNumControl" />
                        <telerik:GridBoundColumn UniqueName="RevisionCliente" DataField="RevisionCliente"
                            HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcRevCliente" />
                        <telerik:GridBoundColumn UniqueName="Pdis" DataField="Pdis" HeaderStyle-Width="80" DataFormatString="{0:N1}"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcPdi" />
                        <telerik:GridBoundColumn UniqueName="Peso" DataField="Peso" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcKgs" />
                        <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcCedula" />
                        <telerik:GridBoundColumn UniqueName="Area" DataField="Area" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcArea" DataFormatString="{0:N2}" />
                        <telerik:GridBoundColumn UniqueName="Especificacion" DataField="Especificacion" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcEspecificacion" />
                        <telerik:GridBoundColumn UniqueName="DiametroPlano" DataField="DiametroPlano" DataFormatString="{0:N3}" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcDiametroPlano" />
                        <telerik:GridCheckBoxColumn UniqueName="PendienteDocumental" DataField="PendienteDocumental"
                            HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcRevIngenieria" />
                        <telerik:GridBoundColumn UniqueName="Revision" DataField="RevisionSteelgo" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcRevSteelGo" />
                        <telerik:GridBoundColumn UniqueName="PorcentajePnd" DataField="PorcentajePnd" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcPnd" />
                        <telerik:GridCheckBoxColumn UniqueName="RequierePwht" DataField="RequierePwht" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcPwht" />                        
                        <telerik:GridCheckBoxColumn UniqueName="AprobadoParaCruce" DataField="AprobadoParaCruce"
                            HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcAprobadoCruce" />
                        <telerik:GridBoundColumn UniqueName="Material" DataField="FamiliasAcero" HeaderStyle-Width="80"
                            FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcMaterial" />                        
                        <telerik:GridBoundColumn UniqueName="Segmento1" DataField="Segmento1" HeaderStyle-Width="100" DataFormatString="{0:N0}"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcSegmento1" />
                        <telerik:GridBoundColumn UniqueName="Segmento2" DataField="Segmento2" HeaderStyle-Width="100"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcSegmento2" />
                        <telerik:GridBoundColumn UniqueName="Segmento3" DataField="Segmento3" HeaderStyle-Width="100"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcSegmento3" />
                        <telerik:GridBoundColumn UniqueName="Segmento4" DataField="Segmento4" HeaderStyle-Width="100" DataFormatString="{0:N0}"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcSegmento4" />
                        <telerik:GridBoundColumn UniqueName="Segmento5" DataField="Segmento5" HeaderStyle-Width="100"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcSegmento5" />
                        <telerik:GridBoundColumn UniqueName="Segmento6" DataField="Segmento6" HeaderStyle-Width="100"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcSegmento6" />
                        <telerik:GridBoundColumn UniqueName="Segmento7" DataField="Segmento7" HeaderStyle-Width="100"
                            FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcSegmento7" />
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
                                <telerik:RadTab runat="server" meta:resourcekey="rdMateriales" Selected="True" />
                                <telerik:RadTab runat="server" meta:resourcekey="rdJuntas" />
                                <telerik:RadTab runat="server" meta:resourcekey="rdCortes" />
                            </Tabs>
                        </telerik:RadTabStrip>
                        <telerik:RadMultiPage ID="rmpDetalle" runat="server" SelectedIndex="0">
                            <telerik:RadPageView ID="rpvMateriales" runat="server" Selected="True">
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
            </mimo:MimossRadGrid>
        </div>
        <div id="bntActualiza">
            <asp:Button runat="server" CausesValidation="false" ID="btnActualiza" CssClass="oculto"
                OnClick="btnActualiza_Click" />
        </div>
    </div>
</asp:Content>
