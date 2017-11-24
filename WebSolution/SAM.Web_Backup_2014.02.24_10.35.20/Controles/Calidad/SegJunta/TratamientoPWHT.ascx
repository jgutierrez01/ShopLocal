<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TratamientoPWHT.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.TratamientoPWHT" %>
<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaRequisicion" />
            <asp:TextBox ID="TratamientoPWHTFechaRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroRequisicion" />
            <asp:TextBox ID="TratamientoPWHTNumeroRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="codigo" />
            <asp:TextBox ID="TratamientoPWHTCodigoRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
            <asp:TextBox ID="TratamientoPWHTFechaReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
            <asp:TextBox ID="TratamientoPWHTNumeroReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
            <asp:TextBox ID="TratamientoPWHTHoja" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaTratamiento" />
            <asp:TextBox ID="TratamientoPWHTFechaTratamiento" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="grafica" />
            <asp:TextBox ID="TratamientoPWHTGrafica" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
            <asp:TextBox ID="TratamientoPWHTResultado" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesRequisicion" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="TratamientoPWHTObservacionesRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesReporte" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="TratamientoPWHTObservacionesReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <p> </p> 

</div>