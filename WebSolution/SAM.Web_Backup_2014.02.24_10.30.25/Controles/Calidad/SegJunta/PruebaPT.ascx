<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PruebaPT.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.PruebaPT" %>
<%@ Register src="RepsPnd.ascx" tagname="RepsPnd" tagprefix="uc1" %>

<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaRequisicion" />
            <asp:TextBox ID="PruebaPTFechaRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroRequisicion" />
            <asp:TextBox ID="PruebaPTNumeroRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="codigo" />
            <asp:TextBox ID="PruebaPTCodigoRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaPrueba" />
            <asp:TextBox ID="PruebaPTFechaPrueba" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
            <asp:TextBox ID="PruebaPTFechaReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
            <asp:TextBox ID="PruebaPTNumeroReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
            <asp:TextBox ID="PruebaPTHoja" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
            <asp:TextBox ID="PruebaPTResultado" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesRequisicion" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaPTObservacionesRequisicion" runat="server"  CssClass="textAreaSegJunta" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesReporte" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaPTObservacionesReporte" runat="server" CssClass="textAreaSegJunta"></asp:TextBox>
        </div>
    </div>

    <p> </p>

    <uc1:RepsPnd ID="repsPnd" runat="server" />

</div>


