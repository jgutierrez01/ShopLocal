<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspDimensional.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegSpool.InspDimensional" %>
<%@ Import Namespace="Mimo.Framework.Extensions" %>
<div class="contenedorCentral">

    <div class="divIzquierdo ancho30">

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
                <asp:TextBox ID="InspeccionDimensionalFechaReporte" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="InspeccionDimensionalNumeroReporte" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label ID="Label1"  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
                <asp:TextBox ID="InspeccionDimensionalHoja" runat="server" ></asp:TextBox>
            </div>
        </div>

    </div>

    <div class="divDerecho ancho70">

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label ID="Label2"  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaInspeccion" />
                <asp:TextBox ID="InspeccionDimensionalFecha" runat="server" ></asp:TextBox>
            </div>
        </div>
        

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
                <asp:TextBox ID="InspeccionDimensionalResultado" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observaciones" />
                <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="InspeccionDimensionalObservaciones" runat="server" Width="84%" ></asp:TextBox>
            </div>
        </div>        

    </div>
    
    <p> </p>
    <br />        
    <br />

    <asp:Repeater runat="server" ID="repSector" Visible="false">
        <HeaderTemplate>
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
                    <tr class="repTitulos bold">
                        <td><asp:Literal runat="server" ID="litFecha"  meta:resourcekey="litFecha" /></td>
                        <td><asp:Literal runat="server" ID="litNumeroReporte"  meta:resourcekey="litNumeroReporte" /></td>
                        <td><asp:Literal runat="server" ID="litHoja"  meta:resourcekey="litHoja" /></td>
                        <td><asp:Literal runat="server" ID="litResultados"  meta:resourcekey="litResultados" /></td>
                        <td><asp:Literal runat="server" ID="litObservaciones"  meta:resourcekey="litObservaciones" /></td>
                    </tr>
                </thead>
        </HeaderTemplate>        
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

        <FooterTemplate>
                <tfoot>
                    <tr class="repPie">
                        <td colspan="5">&nbsp;</td>
                    </tr>
                </tfoot>
            </table>
        </FooterTemplate>  
    </asp:Repeater>
</div>

