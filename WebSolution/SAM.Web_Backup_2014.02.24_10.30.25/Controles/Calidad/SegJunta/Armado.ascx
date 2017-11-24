<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Armado.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.Armado" %>
<div class="contenedorCentral">

    <div class="ancho100">
        
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaArmado" />
                <asp:TextBox ID="ArmadoFecha" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
                <asp:TextBox ID="ArmadoFechaReporte" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="taller" />
                <asp:TextBox ID="ArmadoTaller" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroUnico1" />
                <asp:TextBox ID="ArmadoNumeroUnico1" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroUnico2" />
                <asp:TextBox ID="ArmadoNumeroUnico2" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="codigoTubero" />
                <asp:TextBox ID="ArmadoTubero" runat="server" ></asp:TextBox>
            </div>
        </div>                   
    </div>  
</div>
