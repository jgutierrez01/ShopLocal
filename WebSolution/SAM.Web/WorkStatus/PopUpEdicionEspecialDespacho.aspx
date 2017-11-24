<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpEdicionEspecialDespacho.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpEdicionEspecialDespacho" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userAgentDependant" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">
    <telerik:RadAjaxManager>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnGuardar">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="phControles" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
    </h4>
    <asp:PlaceHolder ID="phControles" runat="server">
    <div>
        <div class="clear">
            <div class="divIzquierdo ancho30 col1">
                <div class="separador">
                    <asp:Label ID="lblFechaDespacho" runat="server" meta:resourcekey="lblFechaDespacho" CssClass="bold"></asp:Label>&nbsp;
                    <telerik:RadDatePicker ID="rdpFechaDespacho" runat="server" Enabled="false" EnableEmbeddedSkins="false" on></telerik:RadDatePicker>
                </div>
                <div class="separador">
                    <asp:Label ID="lblEtiqueta" runat="server" meta:resourcekey="lblEtiqueta" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtEtiqueta" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblItemCode" runat="server" meta:resourcekey="lblItemcode" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtItemCode" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblDescripcion" runat="server" meta:resourcekey="lblDescripcion" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtDescripcion" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblDiametro1" runat="server" meta:resourcekey="lblDiametro1" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtDiametro1" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblDiametro2" runat="server" meta:resourcekey="lblDiametro2" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtDiametro2" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblCantidadRequerida" runat="server" meta:resourcekey="lblCantidadRequerida" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtCantidadRequerida" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" CssClass="boton" OnClick="btnGuardar_Click" />
                </div>
            </div>

            <div class="divIzquierdo ancho30 col2">
                <div class="separador">
                    <asp:Label ID="lblEstatus" runat="server" meta:resourcekey="lblEstatus" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtEstatus" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblNumeroUnico" runat="server" meta:resourcekey="lblNumeroUnico" CssClass="bold"></asp:Label>&nbsp;
                    <telerik:RadComboBox ID="radCmbNumeroUnico" runat="server" Height="150px"
                        EnableLoadOnDemand="true"
                        ShowMoreResultsBox="true"
                        EnableVirtualScrolling="true"
                        OnSelectedIndexChanged="radCmbNumeroUnico_SelectedIndexChanged"
                        meta:resourcekey="radCmbNumeroUnico"
                        OnItemsRequested="radCmbNumeroUnico_ItemsRequested" 
                        EnableViewState="true"
                        AutoPostBack="true"
                        EnableItemCaching="true" >

                    </telerik:RadComboBox>

                    <asp:PlaceHolder ID="phNumUnicoRequerido" runat="server" Visible="true">
                        <span class="required">*</span>
                        <asp:CustomValidator
                            meta:resourcekey="valNumeroUnico"
                            runat="server"
                            ID="rfvNumUnico"
                            Display="None"
                            ControlToValidate="radCmbNumeroUnico"
                            ValidateEmptyText="true"
                            ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                            CssClass="bold"
                            OnServerValidate="rfvNumUnico_ServerValidate"/>
                    </asp:PlaceHolder>
                </div>
                <div class="separador">
                    <asp:CheckBox ID="chkEquivalente" runat="server" meta:resourcekey="chkEquivalente" CssClass="checkYTexto" Enabled="false"/>
                </div>
                <div class="separador">
                    <asp:Label ID="lblItemCodeE" runat="server" meta:resourcekey="lblItemCode" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtItemCodeE" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblDescripcionIng" runat="server" CssClass="bold" meta:resourcekey="lblDescripcion"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtDescripcionIng" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblDiametro1Despachado" runat="server" meta:resourcekey="lblDiametro1" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtDiametro1Despachado" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblDiametro2Despachado" runat="server" meta:resourcekey="lblDiametro2" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtDiametro2Despachado" runat="server" Enabled="false"></asp:TextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblCantidadDespachada" runat="server" meta:resourcekey="lblCantidadDespachada" CssClass="bold"></asp:Label>&nbsp;
                    <asp:TextBox ID="txtCantidadDespachada" runat="server" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div class="divIzquierdo ancho30 col3">
                <div class="validacionesRecuadro" style="margin-top: 20px;">
                    <div class="validacionesHeader">&nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" Width="160" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    </asp:PlaceHolder>
</asp:Content>
