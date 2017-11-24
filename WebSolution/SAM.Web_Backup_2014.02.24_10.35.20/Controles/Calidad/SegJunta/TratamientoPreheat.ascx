<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TratamientoPreheat.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.TratamientoPreheat" %>
<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaRequisicion" />
            <asp:TextBox ID="TratamientoPreheatFechaRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroRequisicion" />
            <asp:TextBox ID="TratamientoPreheatNumeroRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="codigo" />
            <asp:TextBox ID="TratamientoPreheatCodigoRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
            <asp:TextBox ID="TratamientoPreheatFechaReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
            <asp:TextBox ID="TratamientoPreheatNumeroReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
            <asp:TextBox ID="TratamientoPreheatHoja" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaTratamiento" />
            <asp:TextBox ID="TratamientoPreheatFechaTratamiento" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="grafica" />
            <asp:TextBox ID="TratamientoPreheatGrafica" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
            <asp:TextBox ID="TratamientoPreheatResultado" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesRequisicion" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="TratamientoPreheatObservacionesRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesReporte" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="TratamientoPreheatObservacionesReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <p> </p> 

</div>