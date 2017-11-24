<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportacionDatos.aspx.cs"
    MasterPageFile="~/Masters/Ingenieria.master" Inherits="SAM.Web.Ingenieria.ImportacionDatos" %>

<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="FiltroGenerico"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <asp:TextBox CssClass="oculto" runat="server" ID="txtDummy"></asp:TextBox>
    <asp:CustomValidator runat="server" ID="cusChecks" EnableClientScript="true" ClientValidationFunction="Sam.Ingenieria.Validaciones.Checks"
        ControlToValidate="txtDummy" Display="None" meta:resourcekey="cusChecks" ValidateEmptyText="true">
    </asp:CustomValidator>
    <asp:CustomValidator runat="server" ID="cusFamiliaAceros" EnableClientScript="true"
        ClientValidationFunction="Sam.Ingenieria.Validaciones.FamiliasAceros" ControlToValidate="txtDummy"
        Display="None" meta:resourcekey="cusFamiliaAceros" ValidateEmptyText="true">
    </asp:CustomValidator>
    <asp:CustomValidator runat="server" ID="cusHomologacion" EnableClientScript="true"
        ClientValidationFunction="Sam.Ingenieria.Validaciones.Homologacion" ControlToValidate="txtDummy"
        Display="None" meta:resourcekey="cusHomologacion" ValidateEmptyText="true">
    </asp:CustomValidator>
    <asp:CustomValidator runat="server" ID="cusValidaRadUploader" EnableClientScript="true"
        ClientValidationFunction="Sam.Ingenieria.Validaciones.CargaArchivos" ControlToValidate="txtDummy"
        Display="None" meta:resourcekey="cusValidaRadUploader" ValidateEmptyText="true">
    </asp:CustomValidator>
    <asp:CustomValidator runat="server" ID="cusExtensionArchivos" EnableClientScript="true"
        ClientValidationFunction="Sam.Ingenieria.Validaciones.ExtensionArchivos" ControlToValidate="txtDummy"
        Display="None" meta:resourcekey="cusExtensionArchivos" ValidateEmptyText="true">
    </asp:CustomValidator>
    <div class="contenedorCentral">
        <div id="paginaHeader" class="paginaHeader">
            <asp:Label ID="lblTitulo" runat="server" CssClass="Titulo" meta:resourcekey="lblTitulo"></asp:Label>
        </div>
        <div class="cajaFiltros">
            <div class="divIzquierdo ancho100">
                <uc1:FiltroGenerico ID="filtroGenerico" runat="server" FiltroNumeroControl="false"
                    FiltroNumeroUnico="false" FiltroOrdenTrabajo="false" ProyectoHeaderID="proyHeader"
                    ProyectoRequerido="true" />
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <uc2:Header ID="proyHeader" runat="server" Visible="false" />
        <div class="dashboardCentral">
            <asp:Panel ID="pnlInfoCarga" runat="server" CssClass="divIzquierdo ancho60">
                <div class="ancho100">
                    <div id="contenedorEtiquetas" class="divIzquierdo ancho10">
                        <p class="">
                            <asp:Label ID="lblCortes" runat="server" meta:resourcekey="lblCortes" CssClass="bold"></asp:Label>
                        </p>
                        <p class="">
                            <asp:Label ID="lblMaterial" runat="server" meta:resourcekey="lblMaterial" CssClass="bold"></asp:Label>
                        </p>
                        <p class="">
                            <asp:Label ID="lblSpoolInfo" runat="server" meta:resourcekey="lblSpoolInfo" CssClass="bold"></asp:Label>
                        </p>
                        <p class="">
                            <asp:Label ID="lblJuntas" runat="server" meta:resourcekey="lblJuntas" CssClass="bold"></asp:Label>
                        </p>
                    </div>
                    <div class="divIzquierdo ancho80">
                        <telerik:RadUpload ID="RadUpload1" runat="server" InitialFileInputsCount="4" AllowedFileExtensions=".csv"
                            CssClass="radUpload" />
                        <samweb:BotonProcesando ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" meta:resourcekey="btnSubmit"
                            CssClass="boton ancho20" Width="75" />
                    </div>
                </div>
            </asp:Panel>
            <asp:PlaceHolder ID="pnlInfoArchivosCargados" runat="server">
                <asp:Panel ID="pnlCargaSinErrores" runat="server" Visible="false" CssClass="divIzquierdo ancho100">
                    <div class="cajaAzul">
                        <ul>
                            <li>
                                <asp:Label ID="lblSpoolsImportados" Text="" meta:resourcekey="lblSpoolsImportados"
                                    runat="server"></asp:Label>
                            <!--</li>-->
                            <!--<li>-->
                                <asp:Label ID="lblSpoolsImportables" Text="" meta:resourcekey="lblSpoolsImportables"
                                    runat="server" Visible="false"></asp:Label>
                            </li>
                            <asp:PlaceHolder ID="phSpoolDespachos" runat="server" Visible="false">
                                <li>
                                    <asp:Label ID="lblSpoolDespachos" Text="" meta:resourcekey="lblSpoolDespachos" runat="server"></asp:Label>
                                </li>
                                <li>
                                    <asp:Label ID="lblSpoolsConflicto" Text="" meta:resourcekey="lblSpoolsConflicto"
                                        runat="server"></asp:Label>
                                </li>
                            </asp:PlaceHolder>
                        </ul>                        
                        <asp:Panel ID="pnlHomologacion" runat="server" Visible="false">
                            <div class="cajaGris soloLectura">
                                <asp:Repeater runat="server" ID="repHomologacion" OnItemDataBound="repHomologacion_OnItemDataBound">
                                    <HeaderTemplate>
                                        <table>
                                        <colgroup>
                                            <col width="120" />
                                            <col width="80" />
                                            <col width="80" />
                                            <col width="340" />
                                        </colgroup>
                                        <thead>
                                            <th>Nombre Spool</th>
                                            <th>Aceptado</th>
                                            <th>Rechazado</th>
                                            <th>Comentarios</th>
                                        </thead>                                        
                                    </HeaderTemplate>
                                    <ItemTemplate>                                        
                                        <tr>
                                            <td>
                                                <asp:HyperLink ID="hlNombreSpool" runat="server" /></td> 
                                            <td align="center">
                                                <asp:RadioButton ID="rbAceptar" runat="server" Visible="false"  />
                                            </td>
                                            <td align="center">
                                                <asp:RadioButton ID="rbRechazar" runat="server" Visible="false" />
                                            </td>
                                            <td align="center">
                                                <asp:Literal runat="server" meta:resourcekey="litResolverManualmente" ID="litResolverManualmente" /> 
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </table>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>                            
                        </asp:Panel>
                        <asp:Label ID="lblSpoolsInstrucciones" Text="" meta:resourcekey="lblSpoolsInstrucciones"
                            runat="server"></asp:Label><br />
                        <asp:Label ID="lblSpoolsInstrucciones1" Text="" meta:resourcekey="lblSpoolsInstrucciones1"
                            runat="server"></asp:Label>
                    </div>
                    <p>
                    </p>
                    <asp:PlaceHolder ID="phValidacionesSegundaPantalla" Visible="false" runat="server">
                        <div class="divIzquierdo">
                            <asp:ValidationSummary runat="server" ID="valSummary2" EnableClientScript="true"
                                DisplayMode="BulletList" class="summaryList" meta:resourcekey="valSummary" />
                        </div>
                        <p>
                        </p>
                    </asp:PlaceHolder>
                </asp:Panel>
                <div class="divIzquierdo">
                    <telerik:RadPanelBar runat="server" ID="RadPanelBar1" CssClass="ingRadPanelBar" OnClientLoad="Sam.Ingenieria.EstilizaPanelBar" ExpandMode="MultipleExpandedItems" >
                        <Items>
                            <telerik:RadPanelItem Value="Errores" meta:resourcekey="radBarErrores">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="repErrores" Visible="true">
                                        <HeaderTemplate>
                                            <table class="repSam ancho100" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="100%" />
                                                </colgroup>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="repFila">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="repFilaPar">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            <tfoot>
                                                <tr class="repPie">
                                                    <td colspan="1">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Value="Aceros" meta:resourcekey="radBarAceros">
                                <ContentTemplate>
                                    <asp:Repeater ID="repFamiliasAcero" runat="server" OnItemDataBound="rep_ItemDataBound">
                                        <HeaderTemplate>
                                            <table class="repSam ancho100" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="35%" />
                                                    <col width="65%" />
                                                </colgroup>
                                                <thead>
                                                    <tr class="repTitulos">
                                                        <td>
                                                            <asp:Literal ID="litAcero" runat="server" meta:resourcekey="litAcero"></asp:Literal>
                                                        </td>
                                                        <td>
                                                            <asp:Literal ID="litFamilia" runat="server" meta:resourcekey="litFamilia"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="repFila">
                                                <td>
                                                    <asp:Label ID="lblAcero" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlFamilia" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="repFilaPar">
                                                <td>
                                                    <asp:Label ID="lblAcero" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlFamilia" runat="server" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            <tfoot>
                                                <tr class="repPie">
                                                    <td colspan="2">
                                                        <div class="divConfirmacion">
                                                            <asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lblConfirmarAlta" /><input
                                                                type="checkbox" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Value="Cortes" meta:resourcekey="radBarCortes">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="repCortes" Visible="true">
                                        <HeaderTemplate>
                                            <table class="repSam ancho100" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="100%" />
                                                </colgroup>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="repFila">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="repFilaPar">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            <tfoot>
                                                <tr class="repPie">
                                                    <td colspan="1">
                                                        <div class="divConfirmacion">
                                                            <asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lblConfirmarAlta" /><input
                                                                type="checkbox" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>                               
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Value="Juntas" meta:resourcekey="radBarJuntas">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="repJuntas" Visible="true">
                                        <HeaderTemplate>
                                            <table class="repSam ancho100" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="100%" />
                                                </colgroup>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="repFila">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="repFilaPar">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            <tfoot>
                                                <tr class="repPie">
                                                    <td colspan="1">
                                                        <div class="divConfirmacion">
                                                            <asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lblConfirmarAlta" /><input
                                                                type="checkbox" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>                                    
                                </ContentTemplate>
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Value="FabAreas" meta:resourcekey="radBarFabAreas">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="repFabAreas" Visible="true">
                                        <HeaderTemplate>
                                            <table class="repSam ancho100" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="100%" />
                                                </colgroup>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="repFila">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="repFilaPar">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            <tfoot>
                                                <tr class="repPie">
                                                    <td colspan="1">
                                                        <div class="divConfirmacion">
                                                            <asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lblConfirmarAlta" /><input
                                                                type="checkbox" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>                               
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Value="Cedulas" meta:resourcekey="radBarCedulas">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="repCedulas" Visible="true">
                                        <HeaderTemplate>
                                            <table class="repSam ancho100" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="100%" />
                                                </colgroup>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="repFila">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="repFilaPar">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            <tfoot>
                                                <tr class="repPie">
                                                    <td colspan="1">
                                                        <div class="divConfirmacion">
                                                            <asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lblConfirmarAlta" /><input
                                                                type="checkbox" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>                                
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Value="Diametros" meta:resourcekey="radBarDiametros">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="repDiametros" Visible="true">
                                        <HeaderTemplate>
                                            <table class="repSam ancho100" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="100%" />
                                                </colgroup>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="repFila">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="repFilaPar">
                                                <td>
                                                    <%# Container.DataItem %>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            <tfoot>
                                                <tr class="repPie">
                                                    <td colspan="1">
                                                        <div class="divConfirmacion">
                                                            <asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lblConfirmarAlta" /><input
                                                                type="checkbox" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>                               
                            </telerik:RadPanelItem>
                            <telerik:RadPanelItem Text="Item Codes" Value="ItemCodes" meta:resourcekey="radBarItemCodes">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="repItemCodes" Visible="true">
                                        <HeaderTemplate>
                                            <table class="repSam ancho100" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="30%" />
                                                    <col width="70%" />
                                                </colgroup>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="repFila">
                                                <td>
                                                    <%# Eval("itemCode")%>
                                                </td>
                                                <td>
                                                    <%# Eval("descripcion")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="repFilaPar">
                                                <td>
                                                    <%# Eval("itemCode")%>
                                                </td>
                                                <td>
                                                    <%# Eval("descripcion")%>
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <FooterTemplate>
                                            <tfoot>
                                                <tr class="repPie">
                                                    <td colspan="2">
                                                        <div class="divConfirmacion">
                                                            <asp:Label ID="Label1" runat="server" Text="" meta:resourcekey="lblConfirmarAlta" /><input
                                                                type="checkbox" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tfoot>
                                            </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>                                
                            </telerik:RadPanelItem>
                        </Items>
                    </telerik:RadPanelBar>
                </div>
                <p>
                </p>
            </asp:PlaceHolder>
            <div class="divDerecho ancho35">
                <asp:PlaceHolder ID="phValidacionesIniciales" Visible="true" runat="server">
                    <div class="divIzquierdo ancho25">
                        <div class="validacionesRecuadro">
                            <div class="validacionesHeader">
                                &nbsp;</div>
                            <div class="validacionesMain">
                                <asp:ValidationSummary runat="server" ID="valSummary" EnableClientScript="true" DisplayMode="BulletList"
                                    class="summary importacionDatosSummary" meta:resourcekey="valSummary" />
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="divIzquierdo">
                <asp:PlaceHolder ID="phInstruccionesAceptar" runat="server" Visible="false">
                    <div class="cajaAzul">
                        <asp:Label ID="lblInstruccionesAceptar1" Text="" meta:resourcekey="lblInstruccionesAceptar1"
                            runat="server"></asp:Label><br />
                        <asp:Label ID="lblInstruccionesAceptar2" Text="" meta:resourcekey="lblInstruccionesAceptar2"
                            runat="server"></asp:Label><br />
                    </div>
                </asp:PlaceHolder>
                <br />
                <div class="separador">
                    <asp:Button ID="btnRevertir" runat="server" CausesValidation="false" OnClick="btnRevertirClick"
                        meta:resourcekey="btnRevertir" Visible="false" CssClass="boton" />
                    <samweb:BotonProcesando ID="btnRegistar" runat="server" OnClick="btnRegistar_OnClick"
                        meta:resourcekey="btnRegistar" Visible="false" CssClass="boton" />
                </div>
            </div>
            <p>
            </p>
        </div>
    </div>
</asp:Content>
