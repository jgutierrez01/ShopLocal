<%@ Page Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="LstSoldadura.aspx.cs" Inherits="SAM.Web.WorkStatus.LstSoldadura" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager runat="server" Style="display: none;" ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdSoldadura">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSoldadura" LoadingPanelID="loadingPanel" />
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
            <uc2:Filtro ProyectoRequerido="true" OrdenTrabajoRequerido="true" FiltroNumeroUnico="false"
                ProyectoHeaderID="headerProyecto" ProyectoAutoPostBack="true" OrdenTrabajoAutoPostBack="true"
                NumeroControlAutoPostBack="true" runat="server" ID="filtroGenerico"></uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" OnClick="btnMostrar_Click"
                        CssClass="boton" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <sam:Header ID="headerProyecto" runat="server" Visible="false" />
        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="summaryList" meta:resourcekey="valSummary" />
        <asp:PlaceHolder runat="server" ID="phSoldadura" Visible="False">
            <p>
            </p>
            <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdSoldadura" OnNeedDataSource="grdSoldadura_OnNeedDataSource"
                OnItemDataBound="grdSoldadura_OnItemDataBound" OnItemCommand="grdSoldadura_ItemCommand"
                AllowMultiRowSelection="true">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" ClientDataKeyNames="OrdenTrabajoSpoolID"
                    DataKeyNames="OrdenTrabajoSpoolID">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar"
                                OnClick="lnkActualizar_Click" CssClass="link" />
                            <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar"
                                ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_Click" CssClass="imgEncabezado" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="30" Groupable="false"
                            ShowFilterIcon="false" ShowSortIcon="false" Resizable="false" Reorderable="false"
                            UniqueName="Ver_h">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlVer" meta:resourcekey="hlVer" Visible="false"
                                    ImageUrl="~/Imagenes/Iconos/info.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="30" Groupable="false"
                            ShowFilterIcon="false" ShowSortIcon="false" Resizable="false" Reorderable="false"
                            UniqueName="Soldar_h">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlSoldar" meta:resourcekey="hlSoldar" Visible="false"
                                    ImageUrl="~/Imagenes/Iconos/ico_soldarC.png" />
                                <asp:ImageButton runat="server" ID="hlCancelar" meta:resourcekey="hlCancelar" ImageUrl="~/Imagenes/Iconos/borrar.png"
                                    CommandName="cancelar" CommandArgument='<%#Eval("JuntaSoldaduraID") %>' OnClientClick="return Sam.Confirma(14);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn meta:resourcekey="numControlCol" HeaderStyle-Width="100"
                            FilterControlWidth="50" DataField="NumeroControl" />
                        <telerik:GridBoundColumn meta:resourcekey="gbcNombreSpool" HeaderStyle-Width="100"
                            FilterControlWidth="50" DataField="NombreSpool" />
                        <telerik:GridBoundColumn meta:resourcekey="juntaCol" HeaderStyle-Width="80" FilterControlWidth="40"
                            DataField="Junta" />
                        <telerik:GridBoundColumn meta:resourcekey="localizacionCol" HeaderStyle-Width="80"
                            FilterControlWidth="40" DataField="Localizacion" />
                        <telerik:GridBoundColumn meta:resourcekey="tipoCol" HeaderStyle-Width="60" FilterControlWidth="20"
                            DataField="TipoJunta" />
                        <telerik:GridBoundColumn meta:resourcekey="cedulaCol" HeaderStyle-Width="80" FilterControlWidth="40"
                            DataField="Cedula" />
                        <telerik:GridBoundColumn meta:resourcekey="famAcero1Col" HeaderStyle-Width="100"
                            FilterControlWidth="50" DataField="FamiliaAceroMaterial" />
                        <telerik:GridBoundColumn meta:resourcekey="diametroCol" HeaderStyle-Width="80" FilterControlWidth="40"
                            DataField="Diametro" DataFormatString="{0:N3}" />
                        <telerik:GridBoundColumn meta:resourcekey="Estatus" HeaderStyle-Width="100" FilterControlWidth="50"
                            DataField="Estatus" />
                        <telerik:GridCheckBoxColumn meta:resourcekey="hdArmado" HeaderStyle-Width="80" FilterControlWidth="50"
                            DataField="ArmadoAprobado">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn UniqueName="hdSoldadura" meta:resourcekey="hdSoldadura"
                            HeaderStyle-Width="80" FilterControlWidth="50" DataField="SoldaduraAprobada">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridCheckBoxColumn meta:resourcekey="hdHold" HeaderStyle-Width="80" FilterControlWidth="50"
                            DataField="Hold">
                        </telerik:GridCheckBoxColumn>
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                            Reorderable="false" ShowSortIcon="false">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </mimo:MimossRadGrid>
            <div id="btnWrapper" class="oculto">
                <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="false"
                    OnClick="lnkActualizar_Click" />
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
