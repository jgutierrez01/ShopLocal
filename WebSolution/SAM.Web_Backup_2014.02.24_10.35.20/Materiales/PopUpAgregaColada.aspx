<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpAgregaColada.aspx.cs" Inherits="SAM.Web.Materiales.PopUpAgregaColada"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
 <asp:Panel ID="pnlCampos" runat="server">
 <h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
    </h4>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <mimo:RequiredLabeledTextBox runat="server" ID="txtNumeroColada" MaxLength="20" 
                    CssClass="required" meta:resourcekey="txtNumeroColada"  > 
                </mimo:RequiredLabeledTextBox>
            </div>
             <div class="separador">
                <asp:Label runat="server" ID="lblFabricante" meta:resourcekey="lblFabricante" CssClass="bold" />
                <br />
                <asp:DropDownList ID="ddlFabricante" runat="server" >                   
                </asp:DropDownList>
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblAcero" meta:resourcekey="lblAcero" CssClass="bold" />
                <br />
                <asp:DropDownList ID="ddlAcero" runat="server"  >                   
                </asp:DropDownList>
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="reqAcero" runat="server" Display="None" ControlToValidate="ddlAcero"
                    meta:resourcekey="reqAcero" InitialValue="" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtNumeroCertificado" 
                    MaxLength="20" meta:resourcekey="txtNumeroCertificado" >  
                </mimo:LabeledTextBox>
            </div>
            <div class="separador">
                <asp:CheckBox ID="chkHold" runat="server" meta:resourcekey="chkHold" CssClass="checkBold" />
            </div>
           
            <div class="separador">
                <asp:Button runat="server" ID="btnGuardar" OnClick="btnGuardar_Click" CssClass="boton"
                    meta:resourcekey="btnGuardar" />
            </div>
        </div>
        <div class="divDerecho ancho50">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummary"
                        CssClass="summary" />
                </div>
            </div>
        </div>
       <p style="clear: both; height: 2px;">
            </p>
 </asp:Panel>
 <asp:Panel ID="pnlMensaje" runat="server" Visible="False" >
     <table class="mensajeExito small" cellpadding="0" cellspacing="0" style="margin:5px auto 0 auto;">
        <tr>
            <td rowspan="2" class="icono">
                <img src="/Imagenes/Iconos/mensajeExito.png" alt="" />
            </td>
            <td class="titulo">
                <asp:Label runat="server" ID="lblTituloExito" meta:resourcekey="lblTituloExito" />
            </td>
        </tr>
        <tr>
            <td class="cuerpo">
                 <asp:Label ID="Label1" runat="server" meta:resourcekey="lblMensaje"></asp:Label>
            </td>
        </tr>
    </table>
 </asp:Panel>

</asp:Content>
