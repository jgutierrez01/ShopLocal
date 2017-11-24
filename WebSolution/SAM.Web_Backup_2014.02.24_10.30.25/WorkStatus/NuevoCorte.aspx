<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true" CodeBehind="NuevoCorte.aspx.cs" Inherits="SAM.Web.WorkStatus.NuevoCorte" %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>   
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radAjaxMng" runat="server" EnablePageHeadUpdate="true">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radCmbOrdenTrabajo">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                    <telerik:AjaxUpdatedControl ControlID="phDatos" />
                    <telerik:AjaxUpdatedControl ControlID="pnlNumeroUnico" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radNumUnico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phDatos" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="chkTramoCompleto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnMaquina" />
                    <telerik:AjaxUpdatedControl ControlID="pnCantidadReal" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radNumeroControl">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="pnEtiquetaMaterial" />
                    <telerik:AjaxUpdatedControl ControlID="pnMaquina" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radEtiquetaMaterial">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="txtCantidadRequerida" />
                    <telerik:AjaxUpdatedControl ControlID="pnMaquina" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnAgregar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdCorte" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="txtCantidadRequerida" />
                    <telerik:AjaxUpdatedControl ControlID="pnEtiquetaMaterial" />
                    <telerik:AjaxUpdatedControl ControlID="pnMaquina" />
                    <telerik:AjaxUpdatedControl ControlID="pnCantidadReal" />
                    <telerik:AjaxUpdatedControl ControlID="chkTramoCompleto" />
                    <telerik:AjaxUpdatedControl ControlID="chkCorteAjuste" />
                    <telerik:AjaxUpdatedControl ControlID="pnNumeroControl" />                    
                    <telerik:AjaxUpdatedControl ControlID="valAgregar" />                    
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdCorte">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdCorte" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblCorte" CssClass="Titulo" meta:resourcekey="lblCorte"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div id="templateNumeroUnico" class="sys-template">
            <table class="rcbGenerico" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="codigo">
                        {{CodigoSegmento}}
                    </td>
                    <td class="itemCode">
                        {{ItemCode}}
                    </td>
                    <td class="diam1">
                        {{Diametro1}}
                    </td>
                    <td class="diam2">
                        {{Diametro2}}
                    </td>
                    <td>
                        {{InventarioBuenEstado}}
                    </td>
                </tr>
            </table>
        </div>
        <div id="templateEtiquetaMaterial" class="sys-template">
            <table class="rcbGenerico" cellpadding="0" cellspacing="0">
                <tr>
                 <td class="etiqueta">
                        {{EtiquetaSeccion}}
                    </td>
                    <td class="etiqueta">
                        {{EtiquetaMaterial}}
                    </td>
                    <td class="itemCode">
                        {{ItemCode}}
                    </td>
                    <td class="descripcion">
                        {{Descripcion}}
                    </td>
                    <td>
                        {{EsEquivalente}}
                    </td>
                </tr>
            </table>
        </div>
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador" style="margin-top: 0px;">
                    <uc2:Filtro ProyectoRequerido="true" OrdenTrabajoRequerido="true" FiltroNumeroControl="false" FiltroNumeroUnico="false"
                ProyectoHeaderID="headerProyecto" ProyectoAutoPostBack="true" NumeroControlRequerido="false" FiltroOrdenTrabajo="true"
                OrdenTrabajoAutoPostBack="true" OnDdlProyecto_SelectedIndexChanged="OnDdlProyecto_SelectedIndexChanged" runat="server" ID="filtroGenerico"></uc2:Filtro>
                    <%--<asp:Label runat="server" ID="lblOrdenTrabajo" meta:resourcekey="lblOrdenTrabajo"
                        CssClass="bold" />
                    <br />
                    <telerik:RadComboBox ID="radCmbOrdenTrabajo" runat="server" Width="200px" Height="150px"
                        EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                        CausesValidation="false" CssClass="required" OnSelectedIndexChanged="radODT_SelectedIndexChanged"
                        AutoPostBack="true">
                        <WebServiceSettings Method="ListaOrdenTrabajoPorUserScope" Path="~/WebServices/ComboboxWebService.asmx" />
                    </telerik:RadComboBox>
                    <span class="required">*</span>
                    <asp:CustomValidator    
                        meta:resourcekey="valODT"
                        runat="server" 
                        ID="valODT" 
                        Display="None" 
                        ControlToValidate="radCmbOrdenTrabajo" 
                        ValidateEmptyText="true"
                        ValidationGroup="vgComenzar"                         
                        ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                        OnServerValidate="cusRadCmbOrdenTrabajo_ServerValidate" />--%>

                    
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Panel ID="pnlNumeroUnico" runat="server">
                        <asp:Label runat="server" ID="lblNumeroUnico" meta:resourcekey="lblNumeroUnico" CssClass="bold" />
                        <br />
                        <telerik:RadComboBox runat="server" ID="radNumUnico" Height="150px" Width="200px"
                            EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                            CausesValidation="false" Enabled="false" OnClientItemsRequesting="Sam.WebService.NumControlOnClientItemsRequestingEventHandler"
                            OnClientItemDataBound="Sam.WebService.NumeroUnicoTablaDataBound" OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                            DropDownCssClass="liGenerico" DropDownWidth="400px" AutoPostBack="true" OnSelectedIndexChanged="radNumUnico_SelectedIndexChanged"
                            CssClass="required">
                            <WebServiceSettings Method="ListaNumeroUnicoEnTrasferencia" Path="~/WebServices/ComboboxWebService.asmx" />
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                                    <tr>
                                        <th class="codigo">
                                            <asp:Literal ID="litCodigo" runat="server" meta:resourcekey="litCodigo"></asp:Literal>
                                        </th>
                                         <th class="itemCode">
                                            <asp:Literal ID="litItemCode" runat="server" meta:resourcekey="litItemCode"></asp:Literal>
                                        </th>
                                         <th class="diam1">
                                            <asp:Literal ID="litDiam1" runat="server" meta:resourcekey="litDiam1"></asp:Literal>
                                        </th>
                                         <th class="diam2">
                                            <asp:Literal ID="litDiam2" runat="server" meta:resourcekey="litDiam2"></asp:Literal>
                                        </th>
                                        <th>
                                            <asp:Literal ID="litCantidad" runat="server" meta:resourcekey="litCantidad"></asp:Literal>
                                        </th>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </telerik:RadComboBox>
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator ID="valNumeroUnico" runat="server" Display="None" ControlToValidate="radNumUnico"
                            meta:resourcekey="valNumeroUnico" ValidationGroup="vgComenzar" />
                    </asp:Panel>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnComenzar" meta:resourcekey="btnComenzar" runat="server" CssClass="boton"
                        ValidationGroup="vgComenzar" OnClick="btnComenzar_Click" />
                    <asp:Button ID="btnReiniciar" meta:resourcekey="btnReiniciar" runat="server" CssClass="boton"
                        OnClick="btnReiniciar_Click" Visible="false" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <div>
            <proy:Encabezado ID="headerProyecto" runat="server" Visible="False" />
        </div>
        <asp:PlaceHolder ID="phDatos" runat="server" Visible="False">
            <asp:HiddenField ID="hdnProyectoID" runat="server" />
            <asp:HiddenField ID="hdnTolerancia" runat="server" />
            <asp:HiddenField ID="hdnPatioID" runat="server" />
            <div class="cajaAzul">
            <div class="dashboardCentral">
                <div class="divIzquierdo ancho30">
                    <p>
                        <asp:Label ID="lblTallerTitulo" runat="server" CssClass="bold" meta:resourcekey="lblTallerTitulo"></asp:Label>&nbsp;
                        <asp:Label ID="lblTaller" runat="server" meta:resourcekey="lblTallerResource1"></asp:Label>
                    </p>
                    <p>
                        <asp:Label ID="lblFechaTitulo" runat="server" CssClass="bold" meta:resourcekey="lblFechaTitulo"></asp:Label>&nbsp;
                        <asp:Label ID="lblFecha" runat="server" meta:resourcekey="lblFechaResource1"></asp:Label>
                    </p>
                </div>
                <div class="divDerecho ancho70">
                    <asp:PlaceHolder ID="phDatosNumUnico" runat="server" Visible="false">
                        <div class="divIzquierdo ancho50">
                            <p>
                                <asp:Label ID="lblICNumTitulo" runat="server" CssClass="bold" meta:resourcekey="lblICNumTitulo"></asp:Label>&nbsp;
                                <asp:Label ID="lblICNum" runat="server" meta:resourcekey="lblICNumResource1"></asp:Label></p>
                            <p>
                                <asp:Label ID="lblDiam1NumTitulo" runat="server" CssClass="bold" meta:resourcekey="lblDiam1NumTitulo"></asp:Label>&nbsp;
                                <asp:Label ID="lblDiam1Num" runat="server" meta:resourcekey="lblDiam1NumResource1"></asp:Label></p>
                            <p>
                                <asp:Label ID="lblInvFisicoTitulo" runat="server" CssClass="bold" meta:resourcekey="lblInvFisicoTitulo"></asp:Label>&nbsp;
                                <asp:Label ID="lblInvFisico" runat="server" meta:resourcekey="lblInvFisicoResource1"></asp:Label></p>
                        </div>
                        <div class="divDerecho ancho45">
                            <p>
                                <asp:Label ID="lblDescripcionNumTitulo" runat="server" CssClass="bold" meta:resourcekey="lblDescripcionNumTitulo"></asp:Label>&nbsp;
                                <asp:Label ID="lblDescripcionNum" runat="server" meta:resourcekey="lblDescripcionNumResource1"></asp:Label></p>
                            <p>
                                <asp:Label ID="lblDiam2NumTitulo" runat="server" CssClass="bold" meta:resourcekey="lblDiam2NumTitulo"></asp:Label>&nbsp;
                                <asp:Label ID="lblDiam2Num" runat="server" meta:resourcekey="lblDiam2NumResource1"></asp:Label></p>
                            <p>
                                <asp:Label ID="lblInvTitulo" runat="server" CssClass="bold" meta:resourcekey="lblInvTitulo"></asp:Label>&nbsp;
                                <asp:Label ID="lblInv" runat="server" meta:resourcekey="lblInvResource1"></asp:Label>
                            </p>
                        </div>
                    </asp:PlaceHolder>
                </div>
                <p>
                </p>
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummaryResource1"
            ValidationGroup="vgComenzar" CssClass="summaryList" />
        <p>
        </p>
        <asp:PlaceHolder ID="plhCorte" runat="server" Visible="false">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">               
                    <div class="divIzquierdo ancho50">
                        <div class="separador">
                            <asp:CheckBox ID="chkTramoCompleto" runat="server" meta:resourcekey="chkTramoCompleto"
                                CssClass="checkBold" AutoPostBack="true" OnCheckedChanged="chkTramoCompleto_CheckedChanged" />
                        </div>
                        <div class="separador">
                            <asp:Label runat="server" ID="lblNumeroControl" meta:resourcekey="lblNumeroControl"
                                CssClass="bold" />
                            <br />
                            <asp:Panel ID="pnNumeroControl" runat="server">
                            <telerik:RadComboBox ID="radNumeroControl" runat="server" Width="200px" Height="150px"
                                OnClientItemsRequesting="Sam.WebService.NumControlOnClientItemsRequestingEventHandler"
                                EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                AutoPostBack="true" CausesValidation="false" CssClass="required" OnSelectedIndexChanged="radNumeroControl_SelectedIndexChanged">
                                <WebServiceSettings Method="ListaNumerosControlAfinesANumeroUnico" Path="~/WebServices/ComboboxWebService.asmx" />
                            </telerik:RadComboBox>
                            <span class="required">*</span>
                            <asp:CustomValidator    
                                meta:resourcekey="valNumControl"
                                runat="server" 
                                ID="valNumControl" 
                                Display="None" 
                                ControlToValidate="radNumeroControl" 
                                ValidateEmptyText="true"
                                ValidationGroup="vgAgregar"                         
                                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                                OnServerValidate="cusRadNumeroControl_ServerValidate" />

                            
                            </asp:Panel>
                        </div>
                        <div class="separador">
                            <mimo:LabeledTextBox runat="server" ID="txtCantidadRequerida" ReadOnly="true" meta:resourcekey="lblCantidadRequerida">
                            </mimo:LabeledTextBox>
                        </div>
                        <div class="separador">
                            <asp:Panel ID="pnMaquina" runat="server">
                                <asp:Label runat="server" ID="lblMaquina" meta:resourcekey="lblMaquina" CssClass="bold" />
                                <br />
                                <asp:DropDownList ID="ddlMaquina" runat="server" meta:resourcekey="ddlMaquinaResource1"
                                    CssClass="required">
                                </asp:DropDownList>
                                <span class="required" id="pnRequerido" runat="server">*</span>
                                <asp:RequiredFieldValidator ID="valMaquina" runat="server" ControlToValidate="ddlMaquina"
                                    meta:resourcekey="valMaquina" Display="None" ValidationGroup="vgAgregar">
                                </asp:RequiredFieldValidator>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="divDerecho ancho45">
                        <div class="separador">
                            <asp:CheckBox ID="chkCorteAjuste" runat="server" meta:resourcekey="chkCorteAjuste"
                                CssClass="checkBold" />
                        </div>
                        <div class="separador">
                            <asp:Label runat="server" ID="lblEtiquetaMaterial" meta:resourcekey="lblEtiquetaMaterial"
                                CssClass="bold" />
                            <br />
                            <asp:Panel ID="pnEtiquetaMaterial" runat="server">
                                <telerik:RadComboBox ID="radEtiquetaMaterial" runat="server" Width="200px" Height="150px"
                                    OnClientItemsRequesting="Sam.WebService.MaterialesPorNumControlNumUnico" EnableLoadOnDemand="true"
                                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" CausesValidation="false"
                                    CssClass="required" Enabled="false" OnClientItemDataBound="Sam.WebService.EtiquetaMaterialTablaDataBound"
                                    OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                                    OnSelectedIndexChanged="radEtiquetaMaterial_SelectedIndexChanged" DropDownCssClass="liGenerico"
                                    DropDownWidth="550px" AutoPostBack="true">
                                    <WebServiceSettings Method="ListaMaterialesPorNumeroControl" Path="~/WebServices/ComboboxWebService.asmx" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                                            <tr>
                                            <th class="etiqueta">
                                                    <asp:Literal ID="litEtiquetaSeccion" runat="server" meta:resourcekey="litEtiquetaSeccion"></asp:Literal>
                                                </th>
                                                <th class="etiqueta">
                                                    <asp:Literal ID="litEtiquetaMaterial" runat="server" meta:resourcekey="litEtiquetaMaterial"></asp:Literal>
                                                </th>
                                                <th class="itemCode">
                                                    <asp:Literal ID="litItemCode" runat="server" meta:resourcekey="litItemCode"></asp:Literal>
                                                </th>
                                                <th class="descripcion">
                                                    <asp:Literal ID="litDescripcion" runat="server" meta:resourcekey="litDescripcion"></asp:Literal>
                                                </th>
                                                <th>
                                                    <asp:Literal ID="litEquivalente" runat="server" meta:resourcekey="litEquivalente"></asp:Literal>
                                                </th>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </telerik:RadComboBox>
                                <span class="required">*</span>
                                
                                <asp:CustomValidator    
                                meta:resourcekey="valEtiquetaMaterial"
                                runat="server" 
                                ID="valEtiquetaMaterial" 
                                Display="None" 
                                ControlToValidate="radEtiquetaMaterial" 
                                ValidateEmptyText="true"
                                ValidationGroup="vgAgregar"                         
                                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                                OnServerValidate="cusRadEtiquetaMaterial_ServerValidate" />
                                
                            </asp:Panel>
                        </div>
                        <div class="separador">
                        <asp:Panel ID="pnCantidadReal" runat="server">
                            <mimo:RequiredLabeledTextBox runat="server" ID="txtCantidadReal" MaxLength="10" meta:resourcekey="lblCantidadReal"
                                ValidationGroup="vgAgregar">
                            </mimo:RequiredLabeledTextBox>
                            <asp:RangeValidator ID="valCantidad" Display="None" runat="server" ControlToValidate="txtCantidadReal"
                                meta:resourcekey="valCantidad" Type="Integer" MaximumValue="2147483647" MinimumValue="0"
                                ValidationGroup="vgAgregar"></asp:RangeValidator>
                                </asp:Panel>
                        </div>
                    </div>
                    <p>
                    </p>
                    <div class="separador">
                        <asp:Button ID="btnAgregar" runat="server" meta:resourcekey="btnAgregar" OnClick="btnAgregar_OnClick"
                            CssClass="boton" ValidationGroup="vgAgregar" /></div>
                    <div class="clear">
                        <telerik:RadAjaxLoadingPanel ID="ldPanel" runat="server">
                        </telerik:RadAjaxLoadingPanel>
                        <mimo:MimossRadGrid ID="grdCorte" runat="server" OnNeedDataSource="grdCorte_OnNeedDataSource"
                            OnItemCommand="grdCorte_ItemCommand" AllowFilteringByColumn="false" AllowPaging="false"
                            Height="260" OnItemDataBound="grdCorte_ItemDataBound" Visible="false">
                            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false" ShowFooter="true">
                                <CommandItemTemplate>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridBoundColumn UniqueName="CorteID" DataField="CorteID" Visible="false" />
                                    <telerik:GridBoundColumn UniqueName="MaquinaID" DataField="MaquinaID" Visible="false" />
                                    <telerik:GridBoundColumn UniqueName="MaterialSpoolID" DataField="MaterialSpoolID"
                                        Visible="false" />
                                    <telerik:GridBoundColumn UniqueName="OrdenTrabajoSpoolID" DataField="OrdenTrabajoSpoolID"
                                        Visible="false" />
                                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                                        UniqueName="borrar_h" HeaderStyle-Width="30">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("CorteID") %>'
                                                OnClientClick="return Sam.Confirma(1);">
                                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="Maquina" DataField="Maquina" HeaderStyle-Width="80"
                                       Groupable="false" meta:resourcekey="hdMaquina" Reorderable="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" HeaderStyle-Width="130"
                                        Groupable="false" meta:resourcekey="hdNumeroControl" Reorderable="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Etiqueta" DataField="Etiqueta" HeaderStyle-Width="60"
                                        Groupable="false" meta:resourcekey="hdEtiqueta" Reorderable="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="CantidadRequerida" DataField="CantidadRequerida"
                                        HeaderStyle-Width="130" Groupable="false" meta:resourcekey="hdCantidadRequerida" Reorderable="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn UniqueName="Cantidad" DataField="Cantidad" HeaderStyle-Width="120"
                                        Groupable="false" meta:resourcekey="hdCantidadReal" Aggregate="Sum" Reorderable="false"
                                        FooterAggregateFormatString="Total = {0}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridCheckBoxColumn UniqueName="EsAjuste" DataField="EsAjuste" HeaderStyle-Width="60" Reorderable="false"
                                        Groupable="false" meta:resourcekey="hdEsAjuste">
                                    </telerik:GridCheckBoxColumn>                                   
                                </Columns>
                            </MasterTableView>
                            <PagerStyle PageButtonCount="6" AlwaysVisible="True"></PagerStyle>
                            <FilterMenu EnableEmbeddedSkins="False">
                            </FilterMenu>
                            <HeaderContextMenu EnableEmbeddedSkins="False">
                            </HeaderContextMenu>
                        </mimo:MimossRadGrid>
                    </div>
                    <br />
                <div class="divIzquierdo ancho50">
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox runat="server" ID="txtSobrante" MaxLength="10" meta:resourcekey="lblSobrante"
                            ValidationGroup="vgGuardar">
                        </mimo:RequiredLabeledTextBox>
                        <asp:RangeValidator ID="valSobrante" Display="None" runat="server" ControlToValidate="txtSobrante"
                            ValidationGroup="vgGuardar" meta:resourcekey="valSobrante" Type="Integer" MaximumValue="2147483647"
                            MinimumValue="0"></asp:RangeValidator>
                    </div>
                </div>
                <div class="divDerecho ancho45">
                    <div class="separador">
                        <mimo:LabeledTextBox runat="server" ID="txtRack" meta:resourcekey="lblRack" MaxLength="10">
                        </mimo:LabeledTextBox>
                    </div>
                </div>
                <p>
                </p>
                <div class="separador">
                    <samweb:BotonProcesando ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" OnClick="btnGuardar_OnClick"
                        CssClass="boton" ValidationGroup="vgGuardar" /></div>
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro">
                    <div class="validacionesHeader">
                    </div>
                    <div class="validacionesMain">
                        <div class="separador">
                            <asp:ValidationSummary runat="server" ID="valGuardar" ValidationGroup="vgGuardar"
                                meta:resourcekey="valGuardar" CssClass="summary" />
                            <asp:ValidationSummary runat="server" ID="valAgregar" ValidationGroup="vgAgregar"
                                meta:resourcekey="valAgregar" CssClass="summary" />
                        </div>
                    </div>
                </div>
            </div>
            <p>
            </p>
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
