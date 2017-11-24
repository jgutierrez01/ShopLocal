<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Soldadura.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.Soldadura" %>
<div class="contenedorCentral">

    <div class="ancho100">
        
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaSoldadura" />
                <asp:TextBox ID="SoldaduraFecha" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
                <asp:TextBox ID="SoldaduraFechaReporte" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="taller" />
                <asp:TextBox ID="SoldaduraTaller" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="wps" />
                <asp:TextBox ID="SoldaduraWPS" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="wpsRelleno" />
                <asp:TextBox ID="SoldaduraWPSRelleno" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="materialBase1" />
                <asp:TextBox ID="SoldaduraMaterialBase1" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="materialBase2" />
                <asp:TextBox ID="SoldaduraMaterialBase2" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="procesoRaiz" />
                <asp:TextBox ID="SoldaduraProcesoRaiz" runat="server" ></asp:TextBox>
            </div>
        </div> 
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="procesoRelleno" />
                <asp:TextBox ID="SoldaduraProcesoRelleno" runat="server" ></asp:TextBox>
            </div>
        </div>                   
    </div> 
    <p> </p>
    <br />
    <div class="ancho100">

        <table class="repSam" cellpadding="0" cellspacing="0">
            <colgroup>
                <col width="210px" />
                <col width="210px" />
                <col width="210px" />
                <col width="210px" />
            </colgroup>
            <thead>
                <tr class="repEncabezado">
                    <th colspan="4"><asp:Literal runat="server" ID="litDetSoldadura" meta:resourcekey="litDetSoldadura" /></th>
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="litProceso"  meta:resourcekey="litProceso" /></th>
                    <th><asp:Literal runat="server" ID="litCodigoSoldador"  meta:resourcekey="litCodigoSoldador" /></th>
                    <th><asp:Literal runat="server" ID="litNombre"  meta:resourcekey="litNombre" /></th>
                    <th><asp:Literal runat="server" ID="litConsumible"  meta:resourcekey="litConsumible" /></th>
                </tr>
            </thead>
            <asp:Repeater runat="server" ID="repSoldadura">
                <ItemTemplate>
                    <tr class="repFila">
                        <td><%# Eval("Proceso") %></td>                                
                        <td><%# Eval("CodigoSoldador") %></td> 
                        <td><%# Eval("Nombre") %></td> 
                        <td><%# Eval("Consumible") %></td> 
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="repFilaPar">
                        <td><%# Eval("Proceso") %></td>                                
                        <td><%# Eval("CodigoSoldador") %></td> 
                        <td><%# Eval("Nombre") %></td> 
                        <td><%# Eval("Consumible") %></td>                              
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <tfoot>
                <tr class="repPie">
                    <td colspan="4">&nbsp;</td>
                </tr>
            </tfoot>
        </table>  
        
    </div>
               
</div>