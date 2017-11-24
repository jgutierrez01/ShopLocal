<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PruebaRTPostTT.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.PruebaRTPostTT" %>
<%@ Register src="RepsPnd.ascx" tagname="RepsPnd" tagprefix="uc1" %>

<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaRequisicion" />
            <asp:TextBox ID="PruebaRTPostTTFechaRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroRequisicion" />
            <asp:TextBox ID="PruebaRTPostTTNumeroRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="codigo" />
            <asp:TextBox ID="PruebaRTPostTTCodigoRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaPrueba" />
            <asp:TextBox ID="PruebaRTPostTTFechaPrueba" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
            <asp:TextBox ID="PruebaRTPostTTFechaReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
            <asp:TextBox ID="PruebaRTPostTTNumeroReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
            <asp:TextBox ID="PruebaRTPostTTHoja" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
            <asp:TextBox ID="PruebaRTPostTTResultado" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesRequisicion" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaRTPostTTObservacionesRequisicion" runat="server" CssClass="textAreaSegJunta" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesReporte" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaRTPostTTObservacionesReporte" runat="server" CssClass="textAreaSegJunta" ></asp:TextBox>
        </div>
    </div>

    <p> </p>

    <uc1:RepsPnd ID="repsPnd" runat="server" />

</div>


