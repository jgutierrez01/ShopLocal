<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PruebaPMI.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.PruebaPMI" %>
<%@ Register src="RepsPnd.ascx" tagname="RepsPnd" tagprefix="uc1" %>

<div class="contenedorCentral ancho100">
    
    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label ID="Label1"  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaRequisicion" />
            <asp:TextBox ID="PruebaPMIFechaRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label ID="Label2"  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroRequisicion" />
            <asp:TextBox ID="PruebaPMINumeroRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label ID="Label3"  CssClass="labelHack bold" runat="server" meta:resourcekey="codigo" />
            <asp:TextBox ID="PruebaPMICodigoRequisicion" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label ID="Label4"  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaPrueba" />
            <asp:TextBox ID="PruebaPMIFechaPrueba" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label ID="Label5"  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
            <asp:TextBox ID="PruebaPMIFechaReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label ID="Label6"  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
            <asp:TextBox ID="PruebaPMINumeroReporte" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label ID="Label7"  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
            <asp:TextBox ID="PruebaPMIHoja" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho25">
        <div class="separador">
            <asp:Label ID="Label8"  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
            <asp:TextBox ID="PruebaPMIResultado" runat="server" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label ID="Label9"  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesRequisicion" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaPMIObservacionesRequisicion" runat="server" CssClass="textAreaSegJunta" ></asp:TextBox>
        </div>
    </div>

    <div class="divIzquierdo ancho50 altoMin">
        <div class="separador">
            <asp:Label ID="Label10"  CssClass="labelHack bold" runat="server" meta:resourcekey="observacionesReporte" />
            <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="PruebaPMIObservacionesReporte" runat="server" CssClass="textAreaSegJunta" ></asp:TextBox>
        </div>
    </div>

    <p> </p>

    <uc1:RepsPnd ID="repsPnd" runat="server" />

</div>

