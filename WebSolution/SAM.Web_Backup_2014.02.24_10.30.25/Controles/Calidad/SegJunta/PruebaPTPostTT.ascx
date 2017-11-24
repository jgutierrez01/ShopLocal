<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PruebaPTPostTT.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.PruebaPTPostTT" %>
<%@ Register src="RepsPnd.ascx" tagname="RepsPnd" tagprefix="uc1" %>

<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaRequisicion" />
            <asp:TextBox ID="PruebaPTPostTTFechaRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroRequisicion" />
            <asp:TextBox ID="PruebaPTPostTTNumeroRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="codigo" />
            <asp:TextBox ID="PruebaPTPostTTCodigoRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaPrueba" />
            <asp:TextBox ID="PruebaPTPostTTFechaPrueba" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
            <asp:TextBox ID="PruebaPTPostTTFechaReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
            <asp:TextBox ID="PruebaPTPostTTNumeroReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
            <asp:TextBox ID="PruebaPTPostTTHoja" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
            <asp:TextBox ID="PruebaPTPostTTResultado" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesRequisicion" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaPTPostTTObservacionesRequisicion" runat="server" CssClass="textAreaSegJunta" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesReporte" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaPTPostTTObservacionesReporte" runat="server" CssClass="textAreaSegJunta" ></asp:TextBox>
        </div>
    </div>

    <p> </p>

    <uc1:RepsPnd ID="repsPnd" runat="server" />

</div>


