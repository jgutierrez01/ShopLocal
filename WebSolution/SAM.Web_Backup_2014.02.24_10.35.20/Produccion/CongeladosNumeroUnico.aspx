<%@ Page Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="CongeladosNumeroUnico.aspx.cs" Inherits="SAM.Web.Produccion.CongeladosNumeroUnico" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
<div class="paginaHeader">
    <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
</div>
<div class="contenedorCentral">
<telerik:RadAjaxManager ID="radManager" runat="server">
    <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radNumeroUnico" />
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
    <div class="cajaFiltros" style="margin-bottom:5px;">
        <div class="divIzquierdo">
            <div class="separador">
                <asp:Label runat="server" ID="lblProyecto" meta:resourcekey="lblProyecto" AssociatedControlID="ddlProyecto" />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" meta:resourcekey="ddlProyecto" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChange" AutoPostBack="true" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="reqProy" ControlToValidate="ddlProyecto" Display="None" meta:resourcekey="reqProy" />
            </div>
        </div>
        <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblNumeroUnico" runat="server" meta:resourcekey="lblNumeroUnico" AssociatedControlID="radNumeroUnico" />
                    <telerik:RadComboBox runat="server" ID="radNumeroUnico" meta:resourcekey="radNumeroUnico" 
                        Height="150px"                    
                        EnableLoadOnDemand="true"
                        ShowMoreResultsBox="true"
                        EnableVirtualScrolling="true" 
                        OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                        EnableItemCaching="true"
                        AutoPostBack="true"
                        CausesValidation="false">
                        <WebServiceSettings Method="ListaNumeroUnicoPorUserScopeCongelados" Path="~/WebServices/ComboboxWebService.asmx" />                        
                        </telerik:RadComboBox>
                        <span class="required">*</span>
                        <asp:CustomValidator meta:resourcekey="valNumUnic" runat="server" ID="cusNumUnic"
                    Display="None" ControlToValidate="radNumeroUnico" ValidateEmptyText="true" ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                    OnServerValidate="cusNumUnic_ServerValidate" />
                </div>
            </div>
        <div class="divIzquierdo">
            <div class="separador">
                <samweb:BotonProcesando meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" OnClick="btnMostrar_Click" CssClass="boton" />
            </div>
        </div>
        <p></p>
    </div>
        <div class="separador">
        <sam:Header ID="headerProyecto" runat="server" Visible="false" />
    </div>
    <div class="ancho100">
            <asp:ValidationSummary runat="server" ID="valSummary" EnableClientScript="true" DisplayMode="BulletList"
                class="summaryList" meta:resourcekey="valSummary" />
    </div>
    <asp:PlaceHolder runat="server" ID="phLabels" Visible="false">
        <p></p>
        <div class="cajaAzul">
            <div class="divIzquierdo" style="margin-right:15px;">
                <p>
                    <asp:Label runat="server" ID="ItemCodeTexto" meta:resourcekey="ItemCodeTexto" CssClass="bold"/>&nbsp;
                    <asp:Label runat="server" ID="ItemCodeValor" meta:resourcekey="ItemCodeValor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="DescripcionTexto" meta:resourcekey="DescripcionTexto" CssClass="bold"/>&nbsp;
                    <asp:Label runat="server" ID="DescripcionValor" meta:resourcekey="DescripcionValor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="Diametro1Texto" meta:resourcekey="Diametro1Texto" CssClass="bold"/>&nbsp;
                    <asp:Label runat="server" ID="Diametro1Valor" meta:resourcekey="Diametro1Valor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="Diametro2Texto" meta:resourcekey="Diametro2Texto" CssClass="bold"/>&nbsp;
                    <asp:Label runat="server" ID="Diametro2Valor" meta:resourcekey="Diametro2Valor" />
                </p>
            </div>
            <div class="divIzquierdo">
                <div class="separador">                    
                    <p>
                        <asp:Label runat="server" ID="InventarioFisicoTexto" meta:resourcekey="InventarioFisicoTexto" CssClass="bold"/>&nbsp;
                        <asp:Label runat="server" ID="InventarioFisicoValor" meta:resourcekey="InventarioFisicoValor" />
                    </p>
                    <p>
                        <asp:Label runat="server" ID="InventarioDañadoTexto" meta:resourcekey="InventarioDañadoTexto" CssClass="bold"/>&nbsp;
                        <asp:Label runat="server" ID="InventarioDañadoValor" meta:resourcekey="InventarioDañadoValor" />
                    </p>
                    <p>
                        <asp:Label runat="server" ID="InventarioCongeladoTexto" meta:resourcekey="InventarioCongeladoTexto" CssClass="bold"/>&nbsp;
                        <asp:Label runat="server" ID="InventarioCongeladoValor" meta:resourcekey="InventarioCongeladoValor" />
                    </p>
                    <p>
                        <asp:Label runat="server" ID="InventarioDisponibleTexto" meta:resourcekey="InventarioDisponibleTexto" CssClass="bold"/>&nbsp;
                        <asp:Label runat="server" ID="InventarioDisponibleValor" meta:resourcekey="InventarioDisponibleValor" />
                    </p>
                </div>
                </div>
            <p></p>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phSpools" Visible="False">
        <p></p>
        <telerik:radajaxloadingpanel runat="server" id="ldPanel">
        </telerik:radajaxloadingpanel>
        <mimo:MimossRadGrid runat="server" ID="grdSpools" OnNeedDataSource="grdSpools_OnNeedDataSource" OnItemCreated="grdSpools_ItemCreated" allowmultirowselection="true" ShowFooter="true">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand" ClientDataKeyNames="SpoolID,CantCong,MaterialSpoolID" DataKeyNames="SpoolID,CantCong,MaterialSpoolID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink meta:resourcekey="lnkTransfCongelado" runat="server" ID="lnkTransfCongelado" CssClass="link" />
                        <asp:HyperLink meta:resourcekey="imgTransfCongelado" runat="server" ID="imgTransfCongelado" ImageUrl="~/Imagenes/Iconos/icono_odt.png" CssClass="imgEncabezado" />                        
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h" HeaderStyle-Width="30"/>                    
                    <telerik:GridBoundColumn meta:resourcekey="grdspool" UniqueName="Spool" DataField="Spool" FilterControlWidth="90" HeaderStyle-Width="140" />
                    <telerik:GridBoundColumn meta:resourcekey="grdNumControl" UniqueName="NumControl" DataField="NumControl" FilterControlWidth="100" HeaderStyle-Width="140"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdEtiqueta" UniqueName="Etiqueta" DataField="Etiqueta" FilterControlWidth="70" HeaderStyle-Width="110"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdCantCong" UniqueName="CantCong" DataField="CantCong" FilterControlWidth="80" HeaderStyle-Width="140" />
                    <telerik:GridCheckBoxColumn meta:resourcekey="grdEquiv" UniqueName="Equiv" DataField="Equiv" FilterControlWidth="70" HeaderStyle-Width="90" />                    
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
    <div id="btnWrapper" class="oculto">
        <asp:Button CssClass="oculto" runat="server" OnClick="btnWrapper_Click" ID="btnRefresh" CausesValidation="false"  />
    </div>
</asp:Content>