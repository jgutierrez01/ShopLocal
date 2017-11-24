<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true"
    CodeBehind="NuevaRecepcion.aspx.cs" Inherits="SAM.Web.Materiales.NuevaRecepcion" %>
    <%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phForma" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <proy:Encabezado ID="proyEncabezado" runat="server" Visible="false" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblNuevaRecepcion"
        NavigateUrl="~/Materiales/LstRecepcion.aspx" />
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
                <asp:PlaceHolder ID="phForma" runat="server">
                    <div class="divIzquierdo ancho50">
                        <div class="separador">
                            <asp:Label ID="lblProyecto" runat="server" meta:resourcekey="lblProyecto" CssClass="bold" />
                            <br />
                            <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID"
                                OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" AutoPostBack="true" />
                            <span class="required">*</span>
                            <asp:RequiredFieldValidator ID="valProyecto" runat="server" ControlToValidate="ddlProyecto"
                                InitialValue="" Display="None" meta:resourcekey="valProyecto"></asp:RequiredFieldValidator>
                        </div>
                        <div class="separador">
                            <asp:Label ID="lblFechaRecepcion" runat="server" meta:resourcekey="lblFechaRecepcion"
                                CssClass="bold" />
                            <br />
                            <mimo:MappableDatePicker ID="dtpFechaRecepcion" runat="server" EntityPropertyName="FechaRecepcion"
                                MinDate="01/01/1960" MaxDate="01/01/2050" CssClass="required" />
                            <span class="required">*</span>
                            <asp:RequiredFieldValidator ID="valFecha" runat="server" ControlToValidate="dtpFechaRecepcion"
                                Display="None" meta:resourcekey="valFecha"></asp:RequiredFieldValidator>
                        </div>
                        <div class="separador">
                            <asp:Label ID="lblTransportista" runat="server" meta:resourcekey="lblTransportista"
                                CssClass="bold" />
                            <br />
                            <mimo:MappableDropDown runat="server" ID="ddlTransportista" EntityPropertyName="TransportistaID" />
                            <span class="required">*</span>
                            <asp:RequiredFieldValidator ID="valTransportista" runat="server" ControlToValidate="ddlTransportista"
                                Display="None" meta:resourcekey="valTransportista" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="separador">
                            <asp:Label ID="lblProveedor" runat="server" meta:resourcekey="lblProveedor" CssClass="bold" />
                            <br />
                            <mimo:MappableDropDown runat="server" ID="ddlProveedor" EntityPropertyName="ProveedorID" />
                        </div>
                        
                        <asp:Panel runat="server" ID="pnlCamposRecepcion">
                            <h4 style="width: 80%;">
                                <asp:Label runat="server" ID="lblRecepcion" meta:resourcekey="lblRecepcion" />
                            </h4>
                            <div class="separador">
                                <mimo:LabeledTextBox ID="txtCampoRecepcion1" runat="server" EntityPropertyName="CampoRecepcion1" MaxLength="100" />
                            </div>
                            <div class="separador">
                                <mimo:LabeledTextBox ID="txtCampoRecepcion2" runat="server" EntityPropertyName="CampoRecepcion2" MaxLength="100" />
                            </div>
                            <div class="separador">
                                <mimo:LabeledTextBox ID="txtCampoRecepcion3" runat="server" EntityPropertyName="CampoRecepcion3" MaxLength="100" />
                            </div>
                            <div class="separador">
                                <mimo:LabeledTextBox ID="txtCampoRecepcion4" runat="server" EntityPropertyName="CampoRecepcion4" MaxLength="100" />
                            </div>
                            <div class="separador">
                                <mimo:LabeledTextBox ID="txtCampoRecepcion5" runat="server" EntityPropertyName="CampoRecepcion5" MaxLength="100" />
                            </div>
                        </asp:Panel>
                    </div>
                    <div class="divDerecho ancho49">
                        <div class="separador">
                            <mimo:LabeledTextBox ID="txtOrdenCompra" runat="server" meta:resourcekey="lblOrdenCompra"
                                EntityPropertyName="OrdenDeCompra" MaxLength="20" />
                        </div>
                        <div class="separador">
                            <mimo:LabeledTextBox ID="txtFactura" runat="server" meta:resourcekey="lblFactura"
                                EntityPropertyName="Factura" MaxLength="20" />
                        </div>
                        <div class="separador">
                            <mimo:RequiredLabeledTextBox runat="server" ID="txtCantidadNumUnicos" MaxLength="10"
                                meta:resourcekey="txtCantidadNumUnicos" />
                            <asp:RangeValidator ID="valCantidad" runat="server" ControlToValidate="txtCantidadNumUnicos"
                                Type="Integer" MaximumValue="2147483647" MinimumValue="1" meta:resourcekey="valCantidad"
                                Display="None"></asp:RangeValidator>
                        </div>
                        <div class="separador">
                            <asp:Label ID="lblApartirDe" runat="server" meta:resourcekey="lblApartirDe" CssClass="bold"></asp:Label><br />
                            <asp:Label ID="lblCodigo" runat="server" CssClass="bold"></asp:Label><asp:TextBox
                                runat="server" ID="txtNumeroInicial" MaxLength="10" CssClass="required"></asp:TextBox><span
                                    class="required">*</span>
                            <asp:RequiredFieldValidator ID="reqNumeroInicial" ControlToValidate="txtNumeroInicial"
                                Display="None" runat="server" meta:resourcekey="reqNumeroInicial"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="valNumeroInicial" runat="server" ControlToValidate="txtNumeroInicial"
                                Type="Integer" MaximumValue="2147483647" MinimumValue="1" meta:resourcekey="valNumeroInicial"
                                Display="None"></asp:RangeValidator>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
            <div class="divDerecho ancho30">
                <div class="validacionesRecuadro" style="margin-top: 24px;">
                    <div class="validacionesHeader">
                        &nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summary"
                            meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p>
            </p>
            <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" OnClick="btnGuardar_Click"
                CssClass="boton" />
        </div>
    </div>
</asp:Content>
