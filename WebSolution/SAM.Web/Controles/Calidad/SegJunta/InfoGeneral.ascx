<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoGeneral.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.InfoGeneral" %>
<div class="contenedorCentral">

    <div class="divIzquierdo ancho50">
        <div class="separador">
            <div class="divIzquierdo ancho100">
                <asp:Label  CssClass="bold" runat="server" meta:resourcekey="proyecto" />
                <asp:Label ID="Proyecto" runat="server" ></asp:Label>
            </div>            
            <br/><br/>
            <div class="divIzquierdo ancho100">
                <asp:Label  CssClass="bold" runat="server" meta:resourcekey="ordenTrabajo" />
                <asp:Label ID="OrdenDeTrabajo" runat="server" ></asp:Label>
            </div>            
            <br/><br/>
            <div class="divIzquierdo ancho100">
                <asp:Label  CssClass="bold" runat="server" meta:resourcekey="ultimoProceso" />
                <asp:Label ID="UltimoProceso" runat="server" ></asp:Label>
            </div> 
        </div>
    </div>
      
    <div class="divDerecho ancho50">
        <div class="separador">
            <div class="divIzquierdo ancho100">
                <asp:Label  CssClass="bold" runat="server" meta:resourcekey="spool" />
                <asp:Label ID="Spool" runat="server" ></asp:Label>
            </div>            
            <br/><br/>
            <div class="divIzquierdo ancho100">
                <asp:Label  CssClass="bold" runat="server" meta:resourcekey="numeroControl" />
                <asp:Label ID="NumeroDeControl" runat="server" ></asp:Label>
            </div>                        
        </div>
    </div>

    <p> </p>    
    <br />

    <div class="ancho100">
        
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="etiqueta" />
                <asp:TextBox ID="Junta" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="cedula" />
                <asp:TextBox ID="Cedula" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="tipoJunta" />
                <asp:TextBox ID="TipoJunta" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label CssClass="labelHack bold" runat="server" meta:resourcekey="itemCode1" />
                <asp:TextBox ID="ItemCode1" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho60">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="descItemCode1" />
                <asp:TextBox ID="DescItemCode1" runat="server" CssClass="segJuntaDescripcion"></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label CssClass="labelHack bold" runat="server" meta:resourcekey="itemCode2" />
                <asp:TextBox ID="ItemCode2" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho60">
            <div class="separador">
                <asp:Label CssClass="labelHack bold" runat="server" meta:resourcekey="descItemCode2" />
                <asp:TextBox ID="DescItemCode2" runat="server" CssClass="segJuntaDescripcion" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="famAcero1" />
                <asp:TextBox ID="FamAcero1" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label ID="Label2"  CssClass="labelHack bold" runat="server" meta:resourcekey="famAcero2" />
                <asp:TextBox ID="FamAcero2" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="localizacion" />
                <asp:TextBox ID="Localizacion" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fabArea" />
                <asp:TextBox ID="FabArea" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label ID="Label1"  CssClass="labelHack bold" runat="server" meta:resourcekey="diametro" />
                <asp:TextBox ID="Diametro" runat="server" ></asp:TextBox>
            </div>
        </div>        
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="espesor" />
                <asp:TextBox ID="Espesor" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="kgTeoricos" />
                <asp:TextBox ID="KgTeoricos" runat="server" ></asp:TextBox>
            </div>
        </div> 
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="peqs" />
                <asp:TextBox ID="Peqs" runat="server" ></asp:TextBox>
            </div>
        </div>  
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label CssClass="labelHack bold" runat="server" meta:resourcekey="requierePWHT" />
                <asp:TextBox ID="RequierePWHT" runat="server" ></asp:TextBox>
            </div>
        </div>             
    </div>  
</div>
