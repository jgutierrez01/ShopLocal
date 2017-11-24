<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpNuevoPendiente.aspx.cs" Inherits="SAM.Web.Administracion.PopUpNuevoPendiente" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></h4>
    <div class="divIzquierdo ancho60">
        <div class="separador">
            <asp:Label runat="server" ID="lblProyecto" CssClass="bold" meta:resourcekey="lblProyecto" />
            <br />
            <asp:DropDownList runat="server" ID="ddlProyecto" />
            <span class="required">*</span>
            <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto"
                InitialValue="" Display="None" ValidationGroup="vgPendiente" meta:resourcekey="rfvProyecto" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtTitulo" MaxLength="250" ValidationGroup="vgPendiente"
                meta:resourcekey="lblTitulo" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox runat="server" ID="txtDescripcion" MaxLength="50" TextMode="MultiLine"
                meta:resourcekey="lblDescripcion" />
        </div>
        <br />
        <div class="separador">
            <asp:Label runat="server" ID="lblArea" CssClass="bold" meta:resourcekey="lblArea" />
            <br />
            <asp:DropDownList runat="server" ID="ddlArea" />
            <span class="required">*</span>
            <asp:RequiredFieldValidator runat="server" ID="rfvArea" ControlToValidate="ddlArea"
                InitialValue="" Display="None" ValidationGroup="vgPendiente" meta:resourcekey="rfvArea" />
        </div>
        <div class="separador">
            
            <asp:Label runat="server" ID="lblResponsable" AssociatedControlID="rcbResponsable"
                meta:resourcekey="lblResponsable" />
            <telerik:RadComboBox runat="server" ID="rcbResponsable" Width="200px" Height="150px"
                OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" 
                EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                CssClass="required" AllowCustomText="false" IsCaseSensitive="false">
                <WebServiceSettings Method="ListaEmpleadosPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                <HeaderTemplate>
                    <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                        <tr>
                            <th>
                                <asp:Literal ID="litNombre" runat="server" meta:resourcekey="litNombre"></asp:Literal>
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
            </telerik:RadComboBox>
            <span class="required">*</span>
            
            <asp:CustomValidator runat="server" ID="cvResponsable" ControlToValidate="rcbResponsable"
                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValorCustom"
                ValidateEmptyText="true" Display="None" ValidationGroup="vgPendiente" meta:resourcekey="cvResponsable" />
        </div>
        <div class="separador">
            <asp:Button runat="server" CssClass="boton" ID="btnGuardar" OnClick="btnGuardar_Click"
                ValidationGroup="vgPendiente" meta:resourcekey="btnGuardar" />
        </div>
    </div>
    <div class="divDerecho ancho40">
        <div class="validacionesRecuadro">
            <div class="validacionesHeader">
            </div>
            <div class="validacionesMain">
                <asp:ValidationSummary ID="valPendiente" runat="server" ValidationGroup="vgPendiente"
                    meta:resourcekey="valPendiente" CssClass="summary" />
            </div>
        </div>
    </div>
</asp:Content>
