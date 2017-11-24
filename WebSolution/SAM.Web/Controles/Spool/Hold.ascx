<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Hold.ascx.cs" Inherits="SAM.Web.Controles.Spool.Hold" EnableViewState="false" %>
<%@ Import Namespace="SAM.BusinessObjects.Utilerias" %>
<script src="../../Scripts/WebUIValidation.js">
</script>
<div class="infoHold soloLectura">
    <div class="clear listaCheck">
        <div class="ancho30">
            <mimo:MappableCheckBox meta:resourcekey="chkHoldCalidad" runat="server" ID="chkHoldCalidad" EntityPropertyName="TieneHoldCalidad" Enabled="False" />
        </div>
        <div class="ancho30">
            <mimo:MappableCheckBox meta:resourcekey="chkHoldIngenieria" runat="server" ID="chkHoldIngenieria" AutoPostBack="true" 
                EntityPropertyName="TieneHoldIngenieria" Enabled="true" onclick="checkMotivo();"/>
        </div>
        
        <div class="ancho30">
            <mimo:MappableCheckBox meta:resourcekey="chkConfinado" runat="server" ID="chkConfinado" EntityPropertyName="Confinado" Enabled="False" />
        </div>
        <p>&nbsp;</p>
    </div>
    <div>
            <asp:label id="LblMotivo" runat="server" meta:resourcekey="txtMotivoHold" CssClass="bold"></asp:label>
        <textarea id="txtMotivoH" runat="server" EnableViewState="true" ></textarea>
            <%--<asp:TextBox id="txtMotivoH" type="text" runat="server" EnableViewState="true" />--%>
            <asp:RequiredFieldValidator ID="validaTxtMotivo" runat="server" meta:resourcekey="validaMotivo" Enabled="false"
                 ControlToValidate="txtMotivoH"></asp:RequiredFieldValidator>
    </div>
    <div class="clear">
        <table class="repSam" cellpadding="0" cellspacing="0">
            <colgroup>
                <col width="120" />
                <col width="100" />
                <col width="400" />
            </colgroup>
            <thead>
                <tr class="repEncabezado">
                    <th colspan="3"><asp:Literal runat="server" ID="litHolds" meta:resourcekey="litHolds" /></th>
                </tr>
                <tr class="repTitulos">
                    <th><asp:Literal runat="server" ID="litFecha" meta:resourcekey="litFecha" /></th>
                    <th><asp:Literal runat="server" ID="litTipoHold" meta:resourcekey="litTipoHold" /></th>
                    <th><asp:Literal runat="server" ID="litDescripcion" meta:resourcekey="litDescripcion" /></th>
                </tr>
            </thead>
            <asp:Repeater runat="server" ID="repHolds">
                <ItemTemplate>
                    <tr class="repFila">
                        <td><%#Eval("FechaHold","{0:d}")%></td>
                        <td><%#TraductorEnumeraciones.TextoTipoHold(Eval("TipoHold").ToString())%></td>
                        <td><%#Eval("Observaciones")%></td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="repFilaPar">
                        <td><%#Eval("FechaHold", "{0:d}")%></td>
                        <td><%#TraductorEnumeraciones.TextoTipoHold(Eval("TipoHold").ToString())%></td>
                        <td><%#Eval("Observaciones")%></td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <tfoot>
                <tr class="repPie">
                    <td colspan="3">&nbsp;</td>
                </tr>
            </tfoot>
        </table>
    </div>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="chkHoldIngenieria">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="LblMotivo" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="txtMotivoH" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="lblAviso" UpdatePanelCssClass="" />
                    <telerik:AjaxUpdatedControl ControlID="repHolds" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <script type="text/javascript">
        var check = document.getElementById("cphBody_cphBodyInner_ctrlHold_chkHoldIngenieria").checked;
        //debugger;
        function checkMotivo() {
            var valida = document.getElementById("cphBody_cphBodyInner_ctrlHold_validaTxtMotivo");
            if (document.getElementById("cphBody_cphBodyInner_ctrlHold_chkHoldIngenieria").checked != check) {
                ValidatorEnable(valida, true);
            }
            else {
                ValidatorEnable(valida, false);
            }
        }
</script>
</div>
