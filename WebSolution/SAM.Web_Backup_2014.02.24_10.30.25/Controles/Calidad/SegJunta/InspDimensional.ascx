<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspDimensional.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.InspDimensional" %>
<div class="contenedorCentral">

    <div class="divIzquierdo ancho70">

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
                <asp:TextBox ID="InspeccionDimensionalFechaReporte" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaInspeccion" />
                <asp:TextBox ID="InspeccionDimensionalFecha" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="InspeccionDimensionalNumeroReporte" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
                <asp:TextBox ID="InspeccionDimensionalHoja" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observaciones" />
                <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="InspeccionDimensionalObservaciones" runat="server" Width="84%" ></asp:TextBox>
            </div>
        </div>

    </div>

    <div class="divDerecho ancho30">
        
        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
                <asp:TextBox ID="InspeccionDimensionalResultado" runat="server" ></asp:TextBox>
            </div>
        </div>

       

    </div>

    <p> </p>
</div>

