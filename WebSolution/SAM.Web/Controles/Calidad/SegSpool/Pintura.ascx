<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pintura.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegSpool.Pintura" %>
<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaRequisicion" />
            <asp:TextBox ID="PinturaFechaRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho60">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroRequisicion" />
            <asp:TextBox ID="PinturaNumeroRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="sistema" />
            <asp:TextBox ID="PinturaSistema" runat="server" ></asp:TextBox>
        </div>
    </div>
    
    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="color" />
            <asp:TextBox ID="PinturaColor" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho30">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="codigo" />
            <asp:TextBox ID="PinturaCodigo" runat="server" ></asp:TextBox>
        </div>
    </div>

    <p> </p>
    
    <br />

    <div class="divIzquierdo ancho30">
        <div class="bold textoCentrado ancho80">
            <asp:Literal ID="litSandBlast" runat="server" meta:resourcekey="litSandBlast"/>
        </div>
        
        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fecha" />
                <asp:TextBox ID="PinturaFechaSandBlast" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="PinturaReporteSandBlast" runat="server" ></asp:TextBox>
            </div>
        </div>

    </div>

    <div class="divIzquierdo ancho30">
        <div class="bold textoCentrado ancho80">
            <asp:Literal ID="litAcabadoVisual" runat="server" meta:resourcekey="litAcabadoVisual" />
        </div>
        
        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fecha" />
                <asp:TextBox ID="PinturaFechaAcabadoVisual" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="PinturaReporteAcabadoVisual" runat="server" ></asp:TextBox>
            </div>
        </div>

    </div>

    <div class="divIzquierdo ancho30">
        <div class="bold textoCentrado ancho80">
            <asp:Literal ID="litIntermedio" runat="server" meta:resourcekey="litIntermedio" />
        </div>
        
        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fecha" />
                <asp:TextBox ID="PinturaFechaIntermedios" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="PinturaReporteIntermedios" runat="server" ></asp:TextBox>
            </div>
        </div>

    </div>

    <p></p>
    <br />

    <div class="divIzquierdo ancho30">
        <div class="bold textoCentrado ancho80">
            <asp:Literal ID="litPrimario" runat="server" meta:resourcekey="litPrimario" />
        </div>
        
        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fecha" />
                <asp:TextBox ID="PinturaFechaPrimarios" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="PinturaReportePrimarios" runat="server" ></asp:TextBox>
            </div>
        </div>

    </div>
    <div class="divIzquierdo ancho30">
        <div class="bold textoCentrado ancho80">
            <asp:Literal ID="litAdherencias" runat="server" meta:resourcekey="litAdherencias" />
        </div>
        
        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fecha" />
                <asp:TextBox ID="PinturaFechaAdherencia" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="PinturaReporteAdherencia" runat="server" ></asp:TextBox>
            </div>
        </div>

    </div>
    <div class="divIzquierdo ancho30">
        <div class="bold textoCentrado ancho80">
            <asp:Literal ID="litPulloff" runat="server" meta:resourcekey="litPulloff" />
        </div>
        
        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fecha" />
                <asp:TextBox ID="PinturaFechaPullOff" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="PinturaReportePullOff" runat="server" ></asp:TextBox>
            </div>
        </div>

    </div>
     

    <p> </p> 

</div>