<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true"
    CodeBehind="LstConfinarSpool.aspx.cs" Inherits="SAM.Web.Materiales.LstConfinarSpool" %>

<%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/JuntaRO.ascx" TagName="DetJunta" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/MaterialRO.ascx" TagName="DetMaterial" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/CorteRO.ascx" TagName="DetCorte" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="FiltroGenerico"
    TagPrefix="uc1" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="grdPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
           
                <uc1:FiltroGenerico ID="filtroGenerico" runat="server" FiltroNumeroUnico="false"
                    OrdenTrabajoWidth="150px" NumeroControlWidth="150" ProyectoRequerido="true" ProyectoHeaderID="proyEncabezado" OrdenTrabajoAutoPostback="true" NumeroControlAutoPostBack="true" />
            
            <div class="divIzquierdo" style="padding-top:12px" >
                <asp:CheckBox runat="server" ID="chkConfinados" Checked="true" />
                <asp:Label ID="lblConfinados" runat="server" CssClass="bold" meta:resourcekey="lblConfinados" />
            </div>
            <div class="divIzquierdo">
                <asp:Button runat="server" ID="btnMostrar" meta:resourcekey="btnMostrar" CssClass="boton"
                    OnClick="btnMostrar_Click" />
            </div>
            <p>
            </p>
        </div>
        <br />
        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" DisplayMode="BulletList"
            meta:resourcekey="valSummary" />
        <sam:Header ID="proyEncabezado" runat="server" Visible="False" />
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
                        UniqueName="hlkHold_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Imagenes/Iconos/ico_confinarB.png" runat="server" ID="hypConfinar"
                                Visible="false" meta:resourcekey="hypConfinar" />
                            <asp:HyperLink ImageUrl="~/Imagenes/Iconos/ico_desconfinarB.png" runat="server" ID="hypDesconfinar"
                                Visible="false" meta:resourcekey="hypDesconfinar" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn><telerik:GridCheckBoxColumn UniqueName="Confinado" DataField="Confinado" HeaderStyle-Width="80"
                        FilterControlWidth="30" Groupable="false" meta:resourcekey="gbcTieneConfinado" />                  
                    <telerik:GridBoundColumn UniqueName="Spool" DataField="Nombre" HeaderStyle-Width="150"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcSpool" />
                    <telerik:GridBoundColumn UniqueName="Prioridad" DataField="Prioridad" HeaderStyle-Width="80"
                        FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcPrioridad" />
                    <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" HeaderStyle-Width="100"
                        FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcNumControl" />
                    <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" HeaderStyle-Width="80"
                        FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcCedula" />
                    <telerik:GridBoundColumn UniqueName="Area" DataField="Area" HeaderStyle-Width="80" DataFormatString="{0:N2}"
                        FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcArea" />
                    <telerik:GridBoundColumn UniqueName="FamiliaAcero" DataField="FamiliasAcero" HeaderStyle-Width="100"
                        FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcFamiliaAcero" />
                    <telerik:GridBoundColumn UniqueName="PorcentajePnd" DataField="PorcentajePnd" HeaderStyle-Width="80"
                        FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcPnd" />
                    <telerik:GridCheckBoxColumn UniqueName="RequierePwht" DataField="RequierePwht" HeaderStyle-Width="80"
                        FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcPwht" />
                    <telerik:GridBoundColumn UniqueName="Dibujo" DataField="DibujoReferencia" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDibujo" />
                    <telerik:GridBoundColumn UniqueName="Especificacion" DataField="Especificacion" HeaderStyle-Width="100"
                        FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcEspecificacion" />
                    
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
        <div id="bntActualiza">
            <asp:Button runat="server" CausesValidation="false" ID="btnActualiza" CssClass="oculto"
                OnClick="lnkActualizar_OnClick" />
        </div>
    </div>
</asp:Content>
