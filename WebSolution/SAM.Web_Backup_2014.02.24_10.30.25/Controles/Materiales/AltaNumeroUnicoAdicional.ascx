<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AltaNumeroUnicoAdicional.ascx.cs"
    Inherits="SAM.Web.Controles.Materiales.AltaNumeroUnicoAdicional" %>
<div class="dashboardCentral">
    <div class="divIzquierdo ancho35">
       <p>
            <asp:CheckBox ID="chkMarcadoAsme" runat="server" meta:resourcekey="chkMarcadoAsme"
                CssClass="checkBold" />
        </p>
       <p>
            <asp:CheckBox ID="chkMacadoGolpe" runat="server" meta:resourcekey="chkMacadoGolpe"
                CssClass="checkBold" />
        </p>
       <p>
            <asp:CheckBox ID="chkMarcadoPintura" runat="server" meta:resourcekey="chkMarcadoPintura"
                CssClass="checkBold" />
       </p>

        <h4 style="width: 80%;">
            <asp:Label runat="server" ID="lblRecepcion" meta:resourcekey="lblRecepcion" />
        </h4>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibreRecepcion1" runat="server" EntityPropertyName="CampoLibreRecepcion1" MaxLength="100" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibreRecepcion2" runat="server" EntityPropertyName="CampoLibreRecepcion2" MaxLength="100" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibreRecepcion3" runat="server" EntityPropertyName="CampoLibreRecepcion3" MaxLength="100" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibreRecepcion4" runat="server" EntityPropertyName="CampoLibreRecepcion4" MaxLength="100" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibreRecepcion5" runat="server" EntityPropertyName="CampoLibreRecepcion5" MaxLength="100" />
        </div>
    </div>
    <div class="divIzquierdo ancho35" style="padding-top:20px;">
        <asp:PlaceHolder runat="server" ID="phAccesorio" Visible="true">
            <mimo:LabeledTextBox runat="server" ID="txtRack" meta:resourcekey="txtRack" MaxLength="5" />            
        </asp:PlaceHolder>
        <p>
            <mimo:LabeledTextBox runat="server" ID="txtPruebas" meta:resourcekey="txtPruebas"
                Enabled="True" MaxLength="100" >
            </mimo:LabeledTextBox>
        </p>
        <asp:PlaceHolder runat="server" ID="phTubo" Visible="false">
             <table class="repSam" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col width="50" />
                        <col width="200" />
                    </colgroup>
                    <thead>
                        <tr class="repEncabezado" style="font-weight:normal;">
                            <th colspan="2">
                                <span class="tituloIzquierda">
                                    <asp:Literal runat="server" ID="litRacks" meta:resourcekey="litRacks" />
                                </span>
                            </th>
                        </tr>
                        <tr class="repTitulos">
                            <th><asp:Literal runat="server" ID="litSegmento" meta:resourcekey="litSegmento" /></th>
                            <th><asp:Literal runat="server" ID="litRack" meta:resourcekey="litRack" /></th>
                        </tr>
                    </thead>
                    <asp:Repeater runat="server" ID="repRacks">
                        <ItemTemplate>
                            <tr class="repFila">
                                <td>
                                    <asp:Label runat="server" ID="lblSegmento" Text='<%#Eval("Segmento")%>' />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRack" MaxLength="5" CssClass="dentroGrid" Text='<%#Eval("Rack")%>'/>                                    
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="repFila">
                                <td>
                                    <asp:Label runat="server" ID="lblSegmento" Text='<%#Eval("Segmento")%>' />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRack" MaxLength="5" CssClass="dentroGrid" Text='<%#Eval("Rack")%>'/>                                    
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                    <tfoot>
                        <tr class="repPie">
                            <td colspan="2">&nbsp;</td>
                        </tr>
                    </tfoot>
                </table>           
        </asp:PlaceHolder>

        <h4 style="width: 80%;">
            <asp:Label runat="server" ID="lblNumeroUnico" meta:resourcekey="lblNumeroUnico" />
        </h4>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibre1" runat="server" EntityPropertyName="CampoLibre1" MaxLength="100" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibre2" runat="server" EntityPropertyName="CampoLibre2" MaxLength="100" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibre3" runat="server" EntityPropertyName="CampoLibre3" MaxLength="100" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibre4" runat="server" EntityPropertyName="CampoLibre4" MaxLength="100" />
        </div>
        <div class="separador">
            <mimo:LabeledTextBox ID="txtCampoLibre5" runat="server" EntityPropertyName="CampoLibre5" MaxLength="100" />
        </div>
    </div>
<div class="divDerecho ancho30">
     <div class="validacionesRecuadro" >
                <div class="validacionesHeader">
                    &nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
    </div>
</div> 