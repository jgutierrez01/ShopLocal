<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspVisual.ascx.cs" Inherits="SAM.Web.Controles.Calidad.SegJunta.InspVisual" %>
<div class="contenedorCentral">

    <div class="divIzquierdo ancho70">

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaReporte" />
                <asp:TextBox ID="InspeccionVisualFechaReporte" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="fechaInspeccion" />
                <asp:TextBox ID="InspeccionVisualFecha" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="numeroReporte" />
                <asp:TextBox ID="InspeccionVisualNumeroReporte" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho50">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="hoja" />
                <asp:TextBox ID="InspeccionVisualHoja" runat="server" ></asp:TextBox>
            </div>
        </div>

        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="observaciones" />
                <asp:TextBox Rows="5" Columns="50" TextMode="MultiLine" ID="InspeccionVisualObservaciones" runat="server" Width="84%" ></asp:TextBox>
            </div>
        </div>

    </div>

    <div class="divDerecho ancho30">
        
        <div class="divIzquierdo ancho100">
            <div class="separador">
                <asp:Label  CssClass="labelHack bold" runat="server" meta:resourcekey="resultado" />
                <asp:TextBox ID="InspeccionVisualResultado" runat="server" ></asp:TextBox>
            </div>
        </div>

        <asp:Panel runat="server" ID="pnlDefectos" CssClass="divIzquierdo ancho100">
            <div class="separador">
                <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="200px" />
                    </colgroup>
                    <thead>
                        <tr class="repEncabezado">
                            <td><asp:Literal runat="server" ID="litDetDefecto"  meta:resourcekey="litDetDefecto" /></td>
                        </tr>
                        <tr class="repTitulos">
                            <td><asp:Literal runat="server" ID="litDefecto"  meta:resourcekey="litDefecto" /></td>
                        </tr>
                    </thead>
                    <asp:Repeater runat="server" ID="repDefecto">
                        <ItemTemplate>
                            <tr class="repFila">
                                <td><%# Container.DataItem %></td>                                
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="repFilaPar">
                                <td><%# Container.DataItem %></td>                                
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                    <tfoot>
                        <tr class="repPie">
                            <td>&nbsp;</td>
                        </tr>
                    </tfoot>
                </table>
                               
            </div>
        </asp:Panel>

    </div>

    <p> </p>
</div>

