<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoGeneral.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegSpool.InfoGeneral" %>
<div class="contenedorCentral">

    <div class="divIzquierdo ancho70">
            
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="spool" />
                <asp:TextBox ID="Spool" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="peso" />
                <asp:TextBox ID="Peso" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="dibujo" />
                <asp:TextBox ID="Dibujo" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="area" />
                <asp:TextBox ID="Area" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="especificacion" />
                <asp:TextBox ID="Especificacion" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="revSteelgo" />
                <asp:TextBox ID="RevisionSteelGo" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="Cedula" />
                <asp:TextBox ID="Cedula" runat="server"  ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="revCliente" />
                <asp:TextBox ID="RevisionCte" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="porcPnd" />
                <asp:TextBox ID="PorcPnd" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="prioridad" />
                <asp:TextBox ID="Prioridad" runat="server"></asp:TextBox>
            </div>
        </div> 
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="material" />
                <asp:TextBox ID="Material" runat="server"  ></asp:TextBox>
            </div>
        </div>               
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label CssClass="labelHack bold" runat="server" meta:resourcekey="pdi" />
                <asp:TextBox ID="Pdis" runat="server"  ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label ID="lblDiametroMayor" CssClass="labelHack bold" runat="server" meta:resourcekey="lblDiametroMayor" />
                <asp:TextBox ID="DiametroMayor" runat="server" Enabled="false"></asp:TextBox>
            </div>
        </div>
    </div>
    
    <div class="divDerecho ancho30">

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="segmento1" />
                <asp:TextBox ID="Segmento1" runat="server" 
                     ></asp:TextBox>
            </div>
        </div>       

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label   CssClass="labelHack bold" runat="server" meta:resourcekey="segmento2" />
                <asp:TextBox ID="Segmento2" runat="server" 
                     ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label   CssClass="labelHack bold" runat="server" meta:resourcekey="segmento3" />
                <asp:TextBox ID="Segmento3" runat="server" 
                     ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label   CssClass="labelHack bold" runat="server" meta:resourcekey="segmento4" />
                <asp:TextBox ID="Segmento4" runat="server" 
                     ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label   CssClass="labelHack bold" runat="server" meta:resourcekey="segmento5" />
                <asp:TextBox ID="Segmento5" runat="server" 
                     ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label   CssClass="labelHack bold" runat="server" meta:resourcekey="segmento6" />
                <asp:TextBox ID="Segmento6" runat="server" 
                     ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label CssClass="labelHack bold" runat="server" meta:resourcekey="segmento7" />
                <asp:TextBox ID="Segmento7" runat="server" 
                     ></asp:TextBox>
            </div>
        </div>
        
    </div>
    
    <p></p>
    
    <div class="divIzquierdo ancho100">
        
        <div class="divIzquierdo ancho30">
            <div class="separador">                
                <asp:Checkbox ID="RequierePWHT" runat="server" CssClass="checkYTexto" meta:resourcekey="requierePWHT"></asp:Checkbox>
            </div>
        </div>

        <div class="divIzquierdo ancho30">
            <div class="separador">                
                <asp:Checkbox ID="PendienteDocumental" runat="server" CssClass="checkYTexto" meta:resourcekey="pendienteDocumental"></asp:Checkbox>
            </div>
        </div>

        <div class="divIzquierdo ancho30">
            <div class="separador">                
                <asp:Checkbox ID="AprobadoCruces" runat="server" CssClass="checkYTexto" meta:resourcekey="aprobadoCruces"></asp:Checkbox>
            </div>
        </div>

        <div class="divIzquierdo ancho30">
            <div class="separador">                
                <asp:Checkbox ID="HoldCalidad" runat="server" CssClass="checkYTexto" meta:resourcekey="holdCalidad"></asp:Checkbox>
            </div>
        </div>

        <div class="divIzquierdo ancho30">
            <div class="separador">
                
                <asp:Checkbox ID="HoldIngenieria" runat="server" CssClass="checkYTexto" meta:resourcekey="holdIngenieria"></asp:Checkbox>
            </div>
        </div>

        <div class="divIzquierdo ancho30">
            <div class="separador">                
                <asp:Checkbox ID="Confinado" runat="server" CssClass="checkYTexto" meta:resourcekey="confinado"></asp:Checkbox>
            </div>
        </div>

    </div>  
</div>
