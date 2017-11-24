<%@ Page Language="C#"  MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="CongeladoParcial.aspx.cs" Inherits="SAM.Web.Produccion.CongeladoParcial" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Spool/ComboNuParaCongParcial.ascx" TagName="ComboNuParaCongParcial" TagPrefix="sam" %>
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
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
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
                    <asp:Label ID="lblSpool" runat="server" meta:resourcekey="lblSpool" AssociatedControlID="radSpool" />
                    <telerik:RadComboBox runat="server" ID="radSpool" meta:resourcekey="radSpool" 
                        Height="200px"                    
                        EnableLoadOnDemand="true"
                        ShowMoreResultsBox="true"
                        EnableVirtualScrolling="true" 
                        OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                        EnableItemCaching="true"
                        AutoPostBack="true"
                        CausesValidation="false">
                        <WebServiceSettings Method="ListaSpoolSinOdt" Path="~/WebServices/ComboboxWebService.asmx" />                        
                        </telerik:RadComboBox>
                        <span class="required">*</span>
                        <asp:CustomValidator meta:resourcekey="valNumUnic" runat="server" ID="cusNumUnic"
                    Display="None" ControlToValidate="radSpool" ValidateEmptyText="true" ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
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
    <asp:PlaceHolder runat="server" ID="phMateriales" Visible="false">
            <div class="clear" style="margin-top:20px;">
                <asp:ValidationSummary runat="server" ID="valSummaryGuardar" ValidationGroup="vgGuardar" CssClass="summaryList" meta:resourcekey="valSummary" />
                <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="50" />
                        <col width="110" />
                        <col width="200" />
                        <col width="60" />
                        <col width="60" />
                        <col width="60" />
                        <col width="60" />
                        <col width="80" />
                        <col width="100" />
                        <col width="100" />
                        <col width="200" />
                    </colgroup>
                    <thead>
                        <tr class="repEncabezado">
                            <th colspan="11">
                                <span class="tituloIzquierda">
                                    SPOOL: <asp:Literal runat="server" ID="SpoolNombre" />
                                </span>
                            </th>
                        </tr>
                        <tr class="repTitulos">
                            <th><asp:Literal runat="server" ID="litElimCong" meta:resourcekey="litElimCong" /></th>
                            <th><asp:Literal runat="server" ID="litIcCodigo" meta:resourcekey="litIcCodigo" /></th>
                            <th><asp:Literal runat="server" ID="litIcDescripcion" meta:resourcekey="litIcDescripcion" /></th>
                            <th><asp:Literal runat="server" ID="litD1" meta:resourcekey="litD1" /></th>
                            <th><asp:Literal runat="server" ID="litD2" meta:resourcekey="litD2" /></th>
                            <th><asp:Literal runat="server" ID="litCantidad" meta:resourcekey="litCantidad" /></th>
                            <th><asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" /></th>
                            <th><asp:Literal runat="server" ID="litCategoria" meta:resourcekey="litCategoria" /></th>
                            <th><asp:Literal runat="server" ID="litEspecificacion" meta:resourcekey="litEspecificacion" /></th>
                            <th><asp:Literal runat="server" ID="litCongelado" meta:resourcekey="litCongelado" /></th>
                            <th><asp:Literal runat="server" ID="litNumeroUnico" meta:resourcekey="litNumeroUnico" /></th>
                        </tr>
                    </thead>
                    <asp:Repeater runat="server" ID="repMateriales" OnItemCommand="repeater_OnItemCommand" OnItemDataBound="repeater_OnItemDataBound">
                        <ItemTemplate>
                            <tr class="repFila">
                                <td><asp:ImageButton meta:resourcekey="imgBorrar" ID="imgBorrar" runat="server" CommandName="borrar" CommandArgument='<%#Eval("MaterialSpoolID")%>' ImageUrl="~/Imagenes/Iconos/borrar.png" CausesValidation="false" /></td>
                                <td><%#Eval("ItemCode")%><asp:HiddenField runat="server" ID="hdnItemCodeID" Value='<%#Eval("ItemCodeID")%>' /></td>
                                <td><%#Eval("Descripcion")%></td>
                                <td><%#Eval("D1")%></td>
                                <td><%#Eval("D2")%></td>
                                <td><%#Eval("Cantidad")%></td>
                                <td><%#Eval("Etiqueta")%></td>
                                <td><%#Eval("Categoria")%></td>                                
                                <td><%#Eval("Especificacion")%></td>
                                <td><%#Eval("Congelado")%></td>                                
                                <td>
                                    <sam:ComboNuParaCongParcial runat="server" ID="nuCombo" ValidationGroup="vgGuardar" MaterialSpoolID='<%#Eval("MaterialSpoolID")%>' Cantidad='<%#Eval("Cantidad")%>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="repFilaPar">
                                <td><asp:ImageButton meta:resourcekey="imgBorrar" ID="imgBorrar" runat="server" CommandName="borrar" CommandArgument='<%#Eval("MaterialSpoolID")%>' ImageUrl="~/Imagenes/Iconos/borrar.png" CausesValidation="false" /></td>
                                <td><%#Eval("ItemCode")%><asp:HiddenField runat="server" ID="hdnItemCodeID" Value='<%#Eval("ItemCodeID")%>' /></td>
                                <td><%#Eval("Descripcion")%></td>
                                <td><%#Eval("D1")%></td>
                                <td><%#Eval("D2")%></td>
                                <td><%#Eval("Cantidad")%></td>
                                <td><%#Eval("Etiqueta")%></td>
                                <td><%#Eval("Categoria")%></td>                                
                                <td><%#Eval("Especificacion")%></td>
                                <td><%#Eval("Congelado")%></td>                                
                                <td>
                                    <sam:ComboNuParaCongParcial runat="server" ID="nuCombo" ValidationGroup="vgGuardar" MaterialSpoolID='<%#Eval("MaterialSpoolID")%>' Cantidad='<%#Eval("Cantidad")%>' />
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                    <tfoot>
                        <tr class="repPie">
                            <td colspan="11">&nbsp;</td>
                        </tr>
                    </tfoot>
                </table>        
            </div>
            <div>
                <div class="separador">
                    <samweb:BotonProcesando meta:resourcekey="btnCongelar" ID="btnCongelar" runat="server" OnClick="btnCongelar_Click" CssClass="boton" />
                </div>
            </div>
            <div class="oculto" id="templateNumeroUnicoParaAsignacion">
                <table cellpadding="0" cellspacing="0" class="nuDespachoAccesorio">
                    <tr>
                        <td class="cod">{{CodigoNumeroUnico}}</td>
                        <td class="cod">{{CodigoNumeroUnico}}</td>
                        <td class="inv">{{InventarioBuenEstado}}</td>
                        <td class="diam">{{Diametro1}}</td>
                        <td class="diam">{{Diametro2}}</td>
                        <td class="ind">{{IndicadorEsEquivalente}}</td>
                        <td class="codIc">{{CodigoItemCode}}</td>
                        <td class="last">{{DescripcionItemCode}}</td>
                    </tr>
                </table>
            </div>
        </asp:PlaceHolder>
    </div>           
</asp:Content>