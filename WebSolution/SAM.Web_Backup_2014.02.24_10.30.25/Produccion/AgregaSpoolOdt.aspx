<%@ Page Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="AgregaSpoolOdt.aspx.cs" Inherits="SAM.Web.Produccion.AgregaSpoolOdt" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHead" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:BarraTituloPagina ID="titulo" runat="server" meta:resourcekey="lblTitulo" />
    <div class="cntCentralForma"> &nbsp;
    <sam:Header ID="proyEncabezado" runat="server" />
       
        <div class="cajaAzul" style="width:465px;">
            <div class="divIzquierdo" style="margin-right:20px;">
                <div class="separador">
                 <asp:HiddenField runat="server" ID="hdnProyectoID" ClientIDMode="Static" />
                    <asp:Label meta:resourcekey="lblOdtTexto" runat="server" ID="lblOdtTexto" AssociatedControlID="lblOdt" />
                    <asp:Label runat="server" ID="lblOdt" />
                </div>
                </div>
                 <div class="divIzquierdo" style="margin-right:20px;">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblTallerTexto" runat="server" ID="lblTallerTexto" AssociatedControlID="lblTaller" />
                    <asp:Label runat="server" ID="lblTaller" />
                </div>           
                </div>
                 <div class="divIzquierdo" style="margin-right:20px;">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblEstatusTexto" runat="server" ID="lblEstatusTexto" AssociatedControlID="lblEstatus" />
                    <asp:Label runat="server" ID="lblEstatus" />
                </div>
                </div>
                 <div class="divIzquierdo" style="margin-right:20px;">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblFechaTexto" runat="server" ID="lblFechaTexto" AssociatedControlID="lblFecha" />
                    <asp:Label runat="server" ID="lblFecha" />
                </div>
            </div>
            <p></p>
        </div>
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
                                            IsCaseSensitive="false">
                        <WebServiceSettings Method="SpoolsCandidatosParaOdt" Path="~/WebServices/ComboboxWebService.asmx" />
                    </telerik:RadComboBox>
                    <span class="required">*</span>
                    <asp:CustomValidator    meta:resourcekey="cusCombo"
                                            runat="server" 
                                            ID="cusCombo" 
                                            ControlToValidate="rcbSpools" 
                                            ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor" 
                                            OnServerValidate="cusCombo_ServerValidate"
                                            ValidateEmptyText="true" 
                                            Display="None" />
                </div>
                <div class="separador">
                    <asp:Label meta:resourcekey="lblNumControl" runat="server" ID="lblNumControl" AssociatedControlID="txtNumControl" />
                    <asp:Literal runat="server" ID="litNumOdt" />
                    <asp:TextBox runat="server" ID="txtNumControl" MaxLength="3" CssClass="required" Width="150" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator meta:resourcekey="reqNumControl" runat="server" ID="reqNumControl" ControlToValidate="txtNumControl" Display="None" />
                    <asp:RangeValidator meta:resourcekey="rngNumControl" runat="server" ID="rngNumControl" ControlToValidate="txtNumControl" Type="Integer" MinimumValue="1" MaximumValue="999" Display="None" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="validacionesRecuadro" style="margin-top:20px; width:250px;">
                    <div class="validacionesHeader">&nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBotonLarga">
        <asp:Button CssClass="boton" meta:resourcekey="btnAgregar" runat="server" ID="btnAgregar" OnClick="btnAgregar_Click" />    
    </div>
</asp:Content>
