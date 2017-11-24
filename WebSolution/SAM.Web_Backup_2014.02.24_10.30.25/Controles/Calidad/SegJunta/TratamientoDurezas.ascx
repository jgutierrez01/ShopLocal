<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TratamientoDurezas.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.TratamientoDurezas" %>
<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaRequisicion" />
            <asp:TextBox ID="TratamientoDurezasFechaRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroRequisicion" />
            <asp:TextBox ID="TratamientoDurezasNumeroRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="codigo" />
            <asp:TextBox ID="TratamientoDurezasCodigoRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
            <asp:TextBox ID="TratamientoDurezasFechaReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
            <asp:TextBox ID="TratamientoDurezasNumeroReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
            <asp:TextBox ID="TratamientoDurezasHoja" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaTratamiento" />
            <asp:TextBox ID="TratamientoDurezasFechaTratamiento" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="grafica" />
            <asp:TextBox ID="TratamientoDurezasGrafica" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
            <asp:TextBox ID="TratamientoDurezasResultado" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesRequisicion" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="TratamientoDurezasObservacionesRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesReporte" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="TratamientoDurezasObservacionesReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <p> </p> 

</div>