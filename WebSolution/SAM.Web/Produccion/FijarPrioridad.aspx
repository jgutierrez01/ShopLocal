<%@ Page  Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="FijarPrioridad.aspx.cs" Inherits="SAM.Web.Produccion.FijarPrioridad" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/JuntaRO.ascx" TagName="JuntaRO" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/MaterialRO.ascx" TagName="MaterialRO" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/CorteRO.ascx" TagName="CorteRO" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="loadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnRefresh">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="loadingPanel" />
            </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina ID="titulo" runat="server" NavigateUrl="~/Produccion/CrucePorProyecto.aspx" meta:resourcekey="lblTitulo" />
    <div class="contenedorCentral">
        <div class="cajaFiltros" style="margin-bottom:5px;">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblProyecto" runat="server" ID="lblProyecto" AssociatedControlID="ddlProyecto" />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" meta:resourcekey="ddlProyecto" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" AutoPostBack="true" />
                    <asp:RequiredFieldValidator meta:resourcekey="reqProyecto" runat="server" ID="reqProyecto" ControlToValidate="ddlProyecto" Display="None" ValidationGroup="vgProyecto" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" ValidationGroup="vgProyecto" OnClick="btnMostrar_Click" CssClass="boton" />
                </div>
            </div>
            <p></p>
        </div>
        <asp:ValidationSummary ID="valProyecto" runat="server" ValidationGroup="vgProyecto" CssClass="summaryList" meta:resourcekey="valProyecto" />
        <div class="separador">
            <sam:Header ID="headerProyecto" runat="server" Visible="False" />
        </div>
        <asp:PlaceHolder runat="server" ID="phListado" Visible="False">
            <p></p>
            <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdSpools" OnNeedDataSource="grdSpools_OnNeedDataSource" OnItemCommand="grdSpools_OnItemCommand" OnItemCreated="grdSpools_OnItemCreated" AllowMultiRowSelection="true">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand" ClientDataKeyNames="SpoolID" DataKeyNames="SpoolID">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:HyperLink meta:resourcekey="lnkAprobadoParaCruce" runat="server" ID="lnkAprobadoParaCruce" CssClass="link" />
                            <asp:HyperLink meta:resourcekey="imgAprobadoParaCruce" runat="server" ID="imgAprobadoParaCruce" ImageUrl="~/Imagenes/Iconos/icono_certificar.png" CssClass="imgEncabezado" />
                            <asp:HyperLink meta:resourcekey="lnkPrioridadSeleccionados" runat="server" ID="lnkPrioridadSeleccionados" CssClass="link" />
                            <asp:HyperLink meta:resourcekey="imgPrioridadSeleccionados" runat="server" ID="imgPrioridadSeleccionados" ImageUrl="~/Imagenes/Iconos/icono_fijarprioridad.png" CssClass="imgEncabezado" />
                            <asp:HyperLink meta:resourcekey="lnkPrioridad" runat="server" ID="lnkPrioridad" CssClass="link" />
                            <asp:HyperLink meta:resourcekey="imgPrioridad" runat="server" ID="imgPrioridad" ImageUrl="~/Imagenes/Iconos/icono_fijarprioridad.png" CssClass="imgEncabezado" />
                            <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar" OnClick="lnkActualizar_OnClick" CssClass="link" />
                            <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_OnClick" CssClass="imgEncabezado" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h" HeaderStyle-Width="30"/>
                        <telerik:GridBoundColumn meta:resourcekey="grdColSpool" UniqueName="Spool" DataField="Nombre" HeaderStyle-Width="180" FilterControlWidth="100" />
                        <telerik:GridCheckBoxColumn meta:resourcekey="grdAprobadoCruce" UniqueName="AprobadoParaCruce" DataField="AprobadoParaCruce" HeaderStyle-Width="100" FilterControlWidth="40" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColPrioridad" UniqueName="Prioridad" DataField="Prioridad" HeaderStyle-Width="80" FilterControlWidth="40" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColNumeroControl" UniqueName="NumeroControl" DataField="NumeroControl" HeaderStyle-Width="100" FilterControlWidth="60" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColRevision" UniqueName="RevisionCliente" DataField="RevisionCliente" HeaderStyle-Width="80" FilterControlWidth="40" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColPdis" UniqueName="Pdis" DataField="Pdis" HeaderStyle-Width="70" FilterControlWidth="30" DataFormatString="{0:N3}" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColPeso" UniqueName="Peso" DataField="Peso" HeaderStyle-Width="80" FilterControlWidth="40" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColCedula" UniqueName="Cedula" DataField="Cedula" HeaderStyle-Width="80" FilterControlWidth="40" DataFormatString="{0:N3}" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColArea" UniqueName="Area" DataField="Area" HeaderStyle-Width="70" FilterControlWidth="30" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColMaterial" UniqueName="Material" DataField="FamiliasAcero" HeaderStyle-Width="80" FilterControlWidth="40" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColPnd" UniqueName="PorcentajePnd" DataField="PorcentajePnd" HeaderStyle-Width="80" FilterControlWidth="40" />
                        <telerik:GridCheckBoxColumn meta:resourcekey="grdColPwht" UniqueName="RequierePwht" DataField="RequierePwht" HeaderStyle-Width="80" FilterControlWidth="40" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColDibujo" UniqueName="Dibujo" DataField="Dibujo" HeaderStyle-Width="200" FilterControlWidth="150" />
                        <telerik:GridBoundColumn meta:resourcekey="grdColEspecificacion" UniqueName="Especificacion" DataField="Especificacion" HeaderStyle-Width="120" FilterControlWidth="80" />
                        <telerik:GridCheckBoxColumn meta:resourcekey="grdColHold" UniqueName="Hold" DataField="TieneHold" HeaderStyle-Width="80" FilterControlWidth="40" />
                        <telerik:GridCheckBoxColumn meta:resourcekey="grdColConfinado" UniqueName="Confinado" DataField="Confinado" HeaderStyle-Width="80" FilterControlWidth="40" />
                        <telerik:GridBoundColumn Visible="false" UniqueName="Segmento1" DataField="Segmento1" HeaderText="Segmento1" HeaderStyle-Width="180" FilterControlWidth="100" />
                        <telerik:GridBoundColumn Visible="false" UniqueName="Segmento2" DataField="Segmento2" HeaderText="Segmento2" HeaderStyle-Width="180" FilterControlWidth="100" />
                        <telerik:GridBoundColumn Visible="false" UniqueName="Segmento3" DataField="Segmento3" HeaderText="Segmento3" HeaderStyle-Width="180" FilterControlWidth="100" />
                        <telerik:GridBoundColumn Visible="false" UniqueName="Segmento4" DataField="Segmento4" HeaderText="Segmento4" HeaderStyle-Width="180" FilterControlWidth="100" />
                        <telerik:GridBoundColumn Visible="false" UniqueName="Segmento5" DataField="Segmento5" HeaderText="Segmento5" HeaderStyle-Width="180" FilterControlWidth="100" />
                        <telerik:GridBoundColumn Visible="false" UniqueName="Segmento6" DataField="Segmento6" HeaderText="Segmento6" HeaderStyle-Width="180" FilterControlWidth="100" />
                        <telerik:GridBoundColumn Visible="false" UniqueName="Segmento7" DataField="Segmento7" HeaderText="Segmento7" HeaderStyle-Width="180" FilterControlWidth="100" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false">
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
                                <sam:MaterialRO ID="materiales" runat="server" />
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="rpvJuntas" runat="server">
                                <sam:JuntaRO ID="juntas" runat="server" />
                            </telerik:RadPageView>
                            <telerik:RadPageView ID="rpvCortes" runat="server">
                                <sam:CorteRO ID="cortes" runat="server" />
                            </telerik:RadPageView>
                        </telerik:RadMultiPage>
                    </NestedViewTemplate>
                </MasterTableView>
            </mimo:MimossRadGrid>
        </asp:PlaceHolder>
        <telerik:radwindow runat="server" id="wndAprobadoParaCruce" modal="true" registerwithscriptmanager="true"
            visiblestatusbar="false" visibleonpageload="false">
                <ContentTemplate>
                    <div class="fijarPrioridad">
                        <h4>
                            <asp:Literal runat="server" ID="litAproabdoCruce" meta:resourcekey="litAproabdoCruce" /></h4>
                            <div>
                        <div class="divIzquierdo ancho50">
                            <div class="separador">
                                <asp:Label ID="lblAprobadoParaCruce" runat="server" CssClass="bold" meta:resourcekey="lblAprobadoParaCruce" />
                                <asp:CheckBox ID="chkAprobadoParaCruce" runat="server" meta:resourcekey="chkAprobadoParaCruce" />
                             </div>
                         </div>
                            <div class="divIzquierdo ancho40">
                            <div class="validacionesRecuadro">
                                <div class="validacionesHeader">
                                    &nbsp;</div>    
                                    <div class="validacionesMain">
                                    <asp:ValidationSummary runat="server" ID="ValidationSummary2" ValidationGroup="vgPrioridad"
                                        DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valPrioridad" Width="160" />
                                </div>
                            </div>
                        </div>
                        <p>
                        </p>
                        </div>
                        <div class="separador">
                            <asp:Button ID="btnAprobadoParaCruce" runat="server" OnClick="btnAprobadoParaCruce_Click"
                            CssClass="boton" meta:resourcekey="btnAprobadoParaCruce"   />
                        </div>
                        <div class="separador">
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:radwindow>

        <telerik:RadWindow runat="server" ID="wndFijarPrioridad" Modal="true" RegisterWithScriptManager="true" VisibleStatusbar="false" VisibleOnPageLoad="false">
            <ContentTemplate>
                <div class="fijarPrioridad">
                    <h4><asp:Literal runat="server" ID="litPrioridad" meta:resourcekey="litPrioridad" /></h4>
                    <div>
                        <div class="divIzquierdo ancho50">
                            <div class="separador">
                                <mimo:RequiredLabeledTextBox meta:resourcekey="txtPrioridad" runat="server" ID="txtPrioridad" MaxLength="3" ValidationGroup="vgPrioridad" />
                                <asp:RangeValidator meta:resourcekey="rngPrioridad" runat="server" ID="rngPrioridad" ControlToValidate="txtPrioridad" MinimumValue="0" MaximumValue="999" Type="Integer" ValidationGroup="vgPrioridad" Display="None" />
                            </div>
                        </div>
                        <div class="divIzquierdo ancho40">
                            <div class="validacionesRecuadro">
                                <div class="validacionesHeader">&nbsp;</div>
                                <div class="validacionesMain">
                                    <asp:ValidationSummary runat="server" ID="valPrioridad" ValidationGroup="vgPrioridad" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valPrioridad" Width="160" />
                                </div>
                            </div>
                        </div>
                        <p></p>
                    </div>
                    <div class="separador">
                        <asp:Button meta:resourcekey="btnFijarPrioridad" runat="server" ID="btnFijarPrioridad" ValidationGroup="vgPrioridad" CssClass="boton" OnClick="btnFijarPrioridad_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
        <div id="btnWrapper" class="oculto">
            <asp:Button CssClass="oculto" runat="server" OnClick="lnkActualizar_OnClick" ID="btnRefresh" CausesValidation="false"  />
        </div>
    </div>
</asp:Content>
