<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AltaNumeroUnicoProveedor.ascx.cs"
    Inherits="SAM.Web.Controles.Materiales.AltaNumeroUnicoProveedor" %>

<div  class="dashboardCentral">   
    <div class="divIzquierdo ancho70">
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label runat="server" ID="lblProveedor" meta:resourcekey="lblProveedor" CssClass="bold" />
                <br />
                <asp:DropDownList ID="ddlProveedor" runat="server">
                </asp:DropDownList>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtFactura" meta:resourcekey="txtFactura"
                    Enabled="True" MaxLength="20">
                </mimo:LabeledTextBox>
                <div class="separador">
                    <mimo:LabeledTextBox runat="server" ID="txtOrdenCompra" meta:resourcekey="txtOrdenCompra"
                        Enabled="True" MaxLength="20">
                    </mimo:LabeledTextBox>
                </div>
            </div>
        </div>
        <div class="divDerecho ancho49">
            <div class="separador">
                <asp:Label runat="server" ID="lblFabricante" meta:resourcekey="lblFabricante" CssClass="bold" />
                <br />
                <asp:DropDownList ID="ddlFabricante" runat="server">
                </asp:DropDownList>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtPartidaFactura" meta:resourcekey="txtPartidaFactura"
                    Enabled="True" MaxLength="10">
                </mimo:LabeledTextBox>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtPartidaOrden" meta:resourcekey="txtPartidaOrden"
                    Enabled="True" MaxLength="10">
                </mimo:LabeledTextBox>
            </div>
        </div>
    </div>
   <div class="divDerecho ancho30">
     <div class="validacionesRecuadro" >
                <div class="validacionesHeader">
                    &nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummaryResource1" />
                </div>
            </div>
    </div>
</div>
