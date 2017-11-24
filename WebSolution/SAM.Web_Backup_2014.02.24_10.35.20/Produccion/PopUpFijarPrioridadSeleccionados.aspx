<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpFijarPrioridadSeleccionados.aspx.cs" Inherits="SAM.Web.Produccion.PopUpFijarPrioridadSeleccionados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="fijarPrioridad">
        <h4>
            <asp:Literal runat="server" ID="litPrioridad" meta:resourcekey="litPrioridad" /></h4>
        <div>
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox meta:resourcekey="txtPrioridad" runat="server" ID="txtPrioridad"
                        MaxLength="3" ValidationGroup="vgPrioridad" />
                    <asp:RangeValidator meta:resourcekey="rngPrioridad" runat="server" ID="rngPrioridad"
                        ControlToValidate="txtPrioridad" MinimumValue="0" MaximumValue="999" Type="Integer"
                        ValidationGroup="vgPrioridad" Display="None" />
                </div>
            </div>
            <div class="divIzquierdo ancho40">
                <div class="validacionesRecuadro">
                    <div class="validacionesHeader">
                        &nbsp;</div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valPrioridad" ValidationGroup="vgPrioridad"
                            DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valPrioridad" Width="160" />
                    </div>
                </div>
            </div>
            <p>
            </p>
        </div>
        <div class="separador">
            <asp:Button meta:resourcekey="btnFijarPrioridad" runat="server" ID="btnFijarPrioridad"
                ValidationGroup="vgPrioridad" CssClass="boton" OnClick="btnFijarPrioridad_Click" />
        </div>
    </div>
</asp:Content>
