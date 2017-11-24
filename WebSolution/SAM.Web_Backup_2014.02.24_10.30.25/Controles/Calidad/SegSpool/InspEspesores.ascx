<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspEspesores.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegSpool.InspEspesores" %>
<%@ Import Namespace="Mimo.Framework.Extensions" %>
<div class="contenedorCentral">

    <div class="divIzquierdo ancho30">

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label ID="Label1"  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
                <asp:TextBox ID="InspeccionEspesoresFechaReporte" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label ID="Label2"  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="InspeccionEspesoresNumeroReporte" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label ID="Label3"  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
                <asp:TextBox ID="InspeccionEspesoresHoja" runat="server" ></asp:TextBox>
            </div>
        </div>

    </div>

    <div class="divDerecho ancho70">

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label ID="Label4"  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaInspeccion" />
                <asp:TextBox ID="InspeccionEspesoresFecha" runat="server" ></asp:TextBox>
            </div>
        </div>
        

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label ID="Label5" CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
                <asp:TextBox ID="InspeccionEspesoresResultado" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label ID="Label6"  CssClass="labelHack bold" runat="server" meta:resourcekey="observaciones" />
                <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="InspeccionEspesoresObservaciones" runat="server" Width="84%" ></asp:TextBox>
            </div>
        </div>        

    </div>
    
    <p> </p>

    <br />        
    <br />

    <table class="repSam ancho100" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
            <col width="20%" />
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="5"><asp:Literal runat="server" ID="litHeaderHistRep"  meta:resourcekey="litHeaderHistRep" /></th>
            </tr>
            <tr class="repTitulos bold Centrado">
                <td><asp:Literal runat="server" ID="litFecha"  meta:resourcekey="litFecha" /></td>
                <td><asp:Literal runat="server" ID="litNumeroReporte"  meta:resourcekey="litNumeroReporte" /></td>
                <td><asp:Literal runat="server" ID="litHoja"  meta:resourcekey="litHoja" /></td>
                <td><asp:Literal runat="server" ID="litResultados"  meta:resourcekey="litResultados" /></td>
                <td><asp:Literal runat="server"  ID="litObservaciones"  meta:resourcekey="litObservaciones" /></td>
            </tr>
        </thead>
        <asp:Repeater runat="server" ID="repSector">
            <ItemTemplate>
                <tr class="repFila">
                    <td><%# Eval("Fecha").SafeDateAsStringParse()%></td>                                
                    <td><%# Eval("NumeroReporte") %></td> 
                    <td><%# Eval("Hoja") %></td> 
                    <td><%# Eval("Resultado") %></td> 
                    <td><%# Eval("Observaciones") %></td> 
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td><%# Eval("Fecha").SafeDateAsStringParse()%></td>                                
                    <td><%# Eval("NumeroReporte") %></td> 
                    <td><%# Eval("Hoja") %></td> 
                    <td><%# Eval("Resultado") %></td> 
                    <td><%# Eval("Observaciones") %></td> 
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        <tfoot>
            <tr class="repPie">
                <td colspan="5">&nbsp;</td>
            </tr>
        </tfoot>
    </table>  

</div>

