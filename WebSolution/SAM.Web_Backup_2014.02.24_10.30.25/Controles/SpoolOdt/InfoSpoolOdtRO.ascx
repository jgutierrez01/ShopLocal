<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoSpoolOdtRO.ascx.cs" Inherits="SAM.Web.Controles.SpoolOdt.InfoSpoolOdtRO" %>
<div class="infoSpool soloLectura">
    <div class="clear">
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtSpool" ID="txtSpool" EntityPropertyName="Nombre" runat="server" ReadOnly="True"/>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtDibujo" ID="txtDibujo" runat="server" EntityPropertyName="Dibujo" ReadOnly="True"/>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtExpecificacion"  ID="txtExpecificacion" runat="server" EntityPropertyName="Especificacion" ReadOnly="True" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtCedula" ID="txtCedula" runat="server" EntityPropertyName="Cedula" ReadOnly="True" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtPnd" ID="txtPnd" runat="server" EntityPropertyName="PorcentajePnd" ReadOnly="True" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtMaterial" ID="txtMaterial" EntityPropertyName="FamiliasAcero" runat="server" ReadOnly="True" />
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtPeso" ID="txtPeso" runat="server" EntityPropertyName="Peso" ReadOnly="True" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtArea" ID="txtArea" runat="server" EntityPropertyName="Area" ReadOnly="True"/>
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtRevision" ID="txtRevision" runat="server" EntityPropertyName="RevisionSteelgo" ReadOnly="True" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtRevisionCliente" ID="txtRevisionCliente" runat="server" EntityPropertyName="RevisionCliente" ReadOnly="True" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtPrioridad" ID="txtPrioridad" runat="server" EntityPropertyName="Prioridad" ReadOnly="True"  />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox meta:resourcekey="txtPdi" ID="txtPdi" runat="server" EntityPropertyName="Pdis" ReadOnly="True" />
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div style="margin-top:7px;">
                <asp:Label meta:resourcekey="lblNomenclatura" ID="lblNomenclatura" runat="server" Font-Bold="true" />
            </div>
            <div class="cajaNaranja textosChicos" style="height:245px;">
                <asp:Repeater runat="server" ID="repSegmentos">
                    <ItemTemplate>
                        <div class="separador">
                            <mimo:LabeledTextBox runat="server" ID="txtSegmento" Label='<%# Eval("NombreColumna") %>' Text='<%# Eval("ValorColumna") %>' ReadOnly="True" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <p></p>
    </div>
    <div class="clear listaCheck">
        <div class="divIzquierdo ancho30">
            <div>
                <mimo:MappableCheckBox meta:resourcekey="chkRequierePwht" runat="server" ID="chkRequierePwht" EntityPropertyName="RequierePwht" Enabled="False" />
            </div>
            <div>
                <mimo:MappableCheckBox meta:resourcekey="chkAprobadoCruces" runat="server" ID="chkAprobadoCruces" EntityPropertyName="AprobadoParaCruce" Enabled="False" />
            </div>
            <div>
                <mimo:MappableCheckBox meta:resourcekey="chkPendienteDocumental" runat="server" ID="chkPendienteDocumental" EntityPropertyName="PendienteDocumental" Enabled="False" />
            </div>
        </div>
        <div class="divIzquierdo ancho30">
            <div>
                <mimo:MappableCheckBox meta:resourcekey="chkHoldCalidad" runat="server" ID="chkHoldCalidad" EntityPropertyName="TieneHoldCalidad" Enabled="False" />
            </div>
            <div>
                <mimo:MappableCheckBox meta:resourcekey="chkHoldIngenieria" runat="server" ID="chkHoldIngenieria" EntityPropertyName="TieneHoldIngenieria" Enabled="False" />
            </div>
            <div>
                <mimo:MappableCheckBox meta:resourcekey="chkConfinado" runat="server" ID="chkConfinado" EntityPropertyName="Confinado" Enabled="False" />
            </div>
        </div>
        <p></p>
    </div>
</div>
