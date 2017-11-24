<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.master" AutoEventWireup="true"
    CodeBehind="PopupGenerarRequisicion.aspx.cs" Inherits="SAM.Web.WorkStatus.PopupGenerarRequisicion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="lblRequisicion" meta:resourcekey="lblRequisicion" />
    </h4>
    <div class="divIzquierdo ancho50">
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtNumReq" meta:resourcekey="txtNumReq" />
        </div>
        <div class="separador">
            <asp:Label runat="server" ID="lblFechaReq" meta:resourcekey="lblFechaReq" CssClass="bold" />
            <br />
            <mimo:MappableDatePicker ID="mdpFechaReq" runat="server" Style="width: 209px" />
            <span class="required">*</span>
            <asp:RequiredFieldValidator ID="valFecha" runat="server" ControlToValidate="mdpFechaReq"
                Display="None" meta:resourcekey="valFecha"></asp:RequiredFieldValidator>
        </div>
        <div class="separador">
            <mimo:AutoDisableButton ID="btnARequisitar" runat="server" CssClass="boton" meta:resourcekey="btnRequisitar"
                  OnClick="btnARequisitar_Click"  />
        </div>
        <div class="separador">
                    <telerik:RadWindow ID="radwindowPopup" runat="server" VisibleOnPageLoad="false" meta:resourcekey="tituloConfirma"
                    Width="300px" Modal="true" BackColor="#DADADA" VisibleStatusbar="false" Behaviors="None">
                        <ContentTemplate>
                            <div style="padding: 20px">
                                <asp:Label ID="lblPopup" runat="server"></asp:Label>
                                <br /><br />
                                <asp:Button ID="btnOk" runat="server" CssClass="boton" meta:resourcekey="btnOk" OnClick="btnOk_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnCancel" runat="server" CssClass="boton" meta:resourcekey="btnCancel" OnClick="btnCancel_Click" />
                            </div>
                        </ContentTemplate>
                    </telerik:RadWindow>
                </div>
    </div>
    <div class="divDerecho ancho45">
        <div class="validacionesRecuadro" style="margin-top: 20px;">
            <div class="validacionesHeader">
                &nbsp;</div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ID="ValidationSummary1"
                    CssClass="summary" meta:resourcekey="valSummary" Width="120" />
            </div>
        </div>
    </div>
    <p>
    </p>
</asp:Content>
