<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Embarque.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegSpool.Embarque" %>
<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaEmbarque" />
            <asp:TextBox ID="EmbarqueFechaEmbarque" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho60">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroEmbarque" />
            <asp:TextBox ID="EmbarqueNumeroEmbarque" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="etiquetaEmbarque" />
            <asp:TextBox ID="EmbarqueEtiqueta" runat="server" ></asp:TextBox>
        </div>
    </div>
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaEtiqueta" />
            <asp:TextBox ID="EmbarqueFechaEtiqueta" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaPreparacion" />
            <asp:TextBox ID="EmbarqueFechaPreparacion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <p> </p>
</div>