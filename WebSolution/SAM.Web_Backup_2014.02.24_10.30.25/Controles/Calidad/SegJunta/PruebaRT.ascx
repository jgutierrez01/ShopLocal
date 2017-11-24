<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PruebaRT.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.PruebaRT" %>
<%@ Register src="RepsPnd.ascx" tagname="RepsPnd" tagprefix="uc1" %>

<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaRequisicion" />
            <asp:TextBox ID="PruebaRTFechaRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroRequisicion" />
            <asp:TextBox ID="PruebaRTNumeroRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="codigo" />
            <asp:TextBox ID="PruebaRTCodigoRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaPrueba" />
            <asp:TextBox ID="PruebaRTFechaPrueba" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
            <asp:TextBox ID="PruebaRTFechaReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
            <asp:TextBox ID="PruebaRTNumeroReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
            <asp:TextBox ID="PruebaRTHoja" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
            <asp:TextBox ID="PruebaRTResultado" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesRequisicion" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaRTObservacionesRequisicion" runat="server" CssClass="textAreaSegJunta" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesReporte" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaRTObservacionesReporte" runat="server" CssClass="textAreaSegJunta" ></asp:TextBox>
        </div>
    </div>

    <p> </p>

    <uc1:RepsPnd ID="repsPnd" runat="server" />

</div>

