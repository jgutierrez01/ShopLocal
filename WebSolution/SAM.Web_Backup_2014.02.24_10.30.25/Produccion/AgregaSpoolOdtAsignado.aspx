<%@ Page Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="AgregaSpoolOdtAsignado.aspx.cs" Inherits="SAM.Web.Produccion.AgregaSpoolOdtAsignado" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/ComboNuParaAsignacion.ascx" TagName="ComboNuParaAsignacion" TagPrefix="sam" %>
<asp:Content ID="cntHead" ContentPlaceHolderID="cphHeadInner" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina ID="titulo" runat="server" meta:resourcekey="lblTitulo" />
    <div class="cntCentralForma">
        &nbsp;
        <div class="cajaAzul" style="width:465px;">
            <div class="divIzquierdo" style="margin-right:20px;">
                <div class="separador">
                    <asp:HiddenField runat="server" ID="hdnProyectoID" ClientIDMode="Static" />
                    <asp:Label meta:resourcekey="lblPatioTexto" runat="server" ID="lblPatioTexto" AssociatedControlID="lblPatio" />
                    <asp:Label runat="server" ID="lblPatio" />
                </div>
                <div class="separador">
                    <asp:Label meta:resourcekey="lblProyectoTexto" runat="server" ID="lblProyectoTexto" AssociatedControlID="lblProyecto" />
                    <asp:Label runat="server" ID="lblProyecto" />
                </div>
            </div>
            <div class="divIzquierdo" style="margin-right:20px;">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblOdtTexto" runat="server" ID="lblOdtTexto" AssociatedControlID="lblOdt" />
                    <asp:Label runat="server" ID="lblOdt" />
                </div>
                <div class="separador">
                    <asp:Label meta:resourcekey="lblTallerTexto" runat="server" ID="lblTallerTexto" AssociatedControlID="lblTaller" />
                    <asp:Label runat="server" ID="lblTaller" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblEstatusTexto" runat="server" ID="lblEstatusTexto" AssociatedControlID="lblEstatus" />
                    <asp:Label runat="server" ID="lblEstatus" />
                </div>
                <div class="separador">
                    <asp:Label meta:resourcekey="lblFechaTexto" runat="server" ID="lblFechaTexto" AssociatedControlID="lblFecha" />
                    <asp:Label runat="server" ID="lblFecha" />
                </div>
            </div>
            <p></p>
        </div>
        <asp:PlaceHolder runat="server" ID="phSeleccionSpool">
            <div class="clear" style="margin-top:20px;">
                <div class="divIzquierdo" style="margin-right:20px;">
                    <div class="separador">
                        <asp:Label meta:resourcekey="lblSpool" runat="server" ID="lblSpool" AssociatedControlID="rcbSpools" />
                        <telerik:RadComboBox    ID="rcbSpools"
                                                runat="server"
                                                Width="200px"
                                                Height="150px"
                                                OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                                                EnableLoadOnDemand="true"
                                                ShowMoreResultsBox="true" 
                                                EnableVirtualScrolling="true" 
                                                CssClass="required"
                                                AllowCustomText="false"
                                                IsCaseSensitive="false"
                                                ValidationGroup="vgSeleccionar">
                            <WebServiceSettings Method="SpoolsCandidatosParaOdt" Path="~/WebServices/ComboboxWebService.asmx" />
                        </telerik:RadComboBox>
                        <span class="required">*</span>
                        <asp:Button meta:resourcekey="btnAsignar" ID="btnAsignar" runat="server" ValidationGroup="vgSeleccionar" OnClick="btnAsignar_Click" CssClass="boton" />
                        <asp:CustomValidator    meta:resourcekey="cusCombo"
                                                runat="server" 
                                                ID="cusCombo" 
                                                ControlToValidate="rcbSpools" 
                                                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor" 
                                                OnServerValidate="cusCombo_ServerValidate"
                                                ValidateEmptyText="true" 
                                                ValidationGroup="vgSeleccionar"
                                                Display="None" />
                    </div>
                </div>
                <div class="divIzquierdo">
                    <div class="validacionesRecuadro" style="margin-top:20px; width:250px;">
                        <div class="validacionesHeader">&nbsp;</div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" ValidationGroup="vgSeleccionar" />
                        </div>
                    </div>
                </div>
                <p></p>
            </div>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="phMateriales" Visible="false">
            <div class="clear" style="margin-top:20px;">
                <asp:ValidationSummary runat="server" ID="valSummaryGuardar" ValidationGroup="vgGuardar" CssClass="summaryList" meta:resourcekey="valSummary" />
                <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="90" />
                        <col width="200" />
                        <col width="60" />
                        <col width="60" />
                        <col width="60" />
                        <col width="60" />
                        <col width="80" />
                        <col width="100" />
                        <col width="200" />
                    </colgroup>
                    <thead>
                        <tr class="repEncabezado">
                            <th colspan="9">
                                <span class="tituloIzquierda">
                                    SPOOL: <asp:Literal runat="server" ID="litNombreSpool" />
                                </span>
                                <span class="iconoDerecha">
                                    <asp:LinkButton runat="server" ID="litUndo" meta:resourcekey="litUndo" OnClick="imgUndo_Click" />
                                    <asp:ImageButton runat="server" meta:resourcekey="imgUndo" ID="imgUndo" OnClick="imgUndo_Click" CausesValidation="false" ImageUrl="~/Imagenes/Iconos/icono_undo.png" />
                                </span>
                            </th>
                        </tr>
                        <tr class="repTitulos">
                            <th><asp:Literal runat="server" ID="litIcCodigo" meta:resourcekey="litIcCodigo" /></th>
                            <th><asp:Literal runat="server" ID="litIcDescripcion" meta:resourcekey="litIcDescripcion" /></th>
                            <th><asp:Literal runat="server" ID="litD1" meta:resourcekey="litD1" /></th>
                            <th><asp:Literal runat="server" ID="litD2" meta:resourcekey="litD2" /></th>
                            <th><asp:Literal runat="server" ID="litCantidad" meta:resourcekey="litCantidad" /></th>
                            <th><asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" /></th>
                            <th><asp:Literal runat="server" ID="litCategoria" meta:resourcekey="litCategoria" /></th>
                            <th><asp:Literal runat="server" ID="litEspecificacion" meta:resourcekey="litEspecificacion" /></th>
                            <th><asp:Literal runat="server" ID="litNumeroUnico" meta:resourcekey="litNumeroUnico" /></th>
                        </tr>
                    </thead>
                    <asp:Repeater runat="server" ID="repMateriales">
                        <ItemTemplate>
                            <tr class="repFila">
                                <td><%#Eval("CodigoItemCode")%></td>
                                <td><%#Eval("DescripcionItemCode")%></td>
                                <td><%#Eval("Diametro1")%></td>
                                <td><%#Eval("Diametro2")%></td>
                                <td><%#Eval("Cantidad")%></td>
                                <td><%#Eval("Etiqueta")%></td>
                                <td><%#Eval("Categoria")%></td>
                                <td><%#Eval("Especificacion")%></td>
                                <td>
                                    <sam:ComboNuParaAsignacion runat="server" ID="nuCombo" ValidationGroup="vgGuardar" MaterialSpoolID='<%#Eval("MaterialSpoolID")%>' EtiquetaMaterial='<%#Eval("Etiqueta")%>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="repFilaPar">
                                <td><%#Eval("CodigoItemCode")%></td>
                                <td><%#Eval("DescripcionItemCode")%></td>
                                <td><%#Eval("Diametro1")%></td>
                                <td><%#Eval("Diametro2")%></td>
                                <td><%#Eval("Cantidad")%></td>
                                <td><%#Eval("Etiqueta")%></td>
                                <td><%#Eval("Categoria")%></td>
                                <td><%#Eval("Especificacion")%></td>
                                <td>
                                    <sam:ComboNuParaAsignacion runat="server" ID="nuCombo" ValidationGroup="vgGuardar" MaterialSpoolID='<%#Eval("MaterialSpoolID")%>' EtiquetaMaterial='<%#Eval("Etiqueta")%>' />
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                    <tfoot>
                        <tr class="repPie">
                            <td colspan="9">&nbsp;</td>
                        </tr>
                    </tfoot>
                </table>        
            </div>
            <div>
                <div class="separador">
                    <asp:Label meta:resourcekey="lblNumControl" runat="server" ID="lblNumControl" AssociatedControlID="txtNumControl" />
                    <asp:Literal runat="server" ID="litNumOdt" />
                    <asp:TextBox runat="server" ID="txtNumControl" MaxLength="3" CssClass="required" Width="150" ValidationGroup="vgGuardar" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator meta:resourcekey="reqNumControl" runat="server" ID="reqNumControl" ControlToValidate="txtNumControl" Display="None" ValidationGroup="vgGuardar" />
                    <asp:RangeValidator meta:resourcekey="rngNumControl" runat="server" ID="rngNumControl" ControlToValidate="txtNumControl" Type="Integer" MinimumValue="1" MaximumValue="999" Display="None" ValidationGroup="vgGuardar" />
                </div>
            </div>
            <div class="oculto" id="templateNumeroUnicoParaAsignacion">
                <table cellpadding="0" cellspacing="0" class="nuDespachoAccesorio">
                    <tr>
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
    <div class="pestanaBotonLarga">
        <samweb:BotonProcesando CssClass="boton" meta:resourcekey="btnAgregar" runat="server" ID="btnAgregar" OnServerValidate="AgregarSpool" ValidationGroup="vgGuardar" OnClick="btnAgregar_Click" Enabled="false" />
        <asp:CustomValidator    meta:resourcekey="cusComboCongParcial"
                                                runat="server" 
                                                ID="cusComboCongParcial" 
                                                ControlToValidate="rcbSpools"                                                 
                                                OnServerValidate="AgregarSpool_ServerValidate"
                                                ValidateEmptyText="true" 
                                                ValidationGroup="vgSeleccionar"
                                                Display="None" />
    </div>
</asp:Content>
