<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopupImpresionOdt.aspx.cs" Inherits="SAM.Web.Produccion.PopupImpresionOdt" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
<div style="width:560px;">
    <h4>
        <asp:Literal runat="server" ID="litTitulo" />
    </h4>
    <div>
        <div class="divIzquierdo ancho50" style="margin-right:10px;">
            <div id="contenedorChk" class="listaCheck clear cajaAzul">
            <div>
                <asp:label runat="server" ID="lblCaratulas" meta:resourcekey="lblCaratulas" Font-Bold="true" />
                <asp:CheckBox runat="server" ID="chkCaratulas" Checked="true" onclick="Sam.Produccion.ToggleLista(this);" />
                <br />
                <asp:CheckBox runat="server" ID="chkCaratula" meta:resourcekey="chkCaratula" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkJuntas" meta:resourcekey="chkJuntas" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkMateriales" meta:resourcekey="chkMateriales" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkResumenMateriales" meta:resourcekey="chkResumenMateriales" Checked="true"/><br />
                <asp:CheckBox runat="server" ID="chkListaCorte" meta:resourcekey="chkListaCorte" Checked="true" /><br /><br />
            </div>
            <div>
                <asp:label runat="server" ID="lblCaratulaDetalladas" meta:resourcekey="lblCaratulaDetalladas" Font-Bold="true" />
                <asp:CheckBox runat="server" ID="chkCaratulaDetalladas" Checked="true" onclick="Sam.Produccion.ToggleLista(this);" />
                <br />
                <asp:CheckBox runat="server" ID="chkCaratulaDetallada" meta:resourcekey="chkCaratulaDetallada" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkJuntasDetalle" meta:resourcekey="chkJuntasDetalle" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkMaterialesDetalle" meta:resourcekey="chkMaterialesDetalle" Checked="true" /><br /><br />
            </div>
            <div>
                <asp:label runat="server" ID="lblCaratulasEstacion" meta:resourcekey="lblCaratulasEstacion" Font-Bold="true" />
                <asp:CheckBox runat="server" ID="chkCaratulasEstacion" Checked="true" onclick="Sam.Produccion.ToggleLista(this);" />
                <br />
                <asp:CheckBox runat="server" ID="chkCaratulaPorEstacionTrabajo" meta:resourcekey="chkCaratulaPorEstacionTrabajo" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkJuntasDetalleEstacion" meta:resourcekey="chkJuntasDetalleEstacion" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkMaterialesDetalleEstacion" meta:resourcekey="chkMaterialesDetalleEstacion" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkResumenMaterialesEstacion" meta:resourcekey="chkResumenMaterialesEstacion" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkCortesEstacion" meta:resourcekey="chkCortesEstacion" Checked="true" /><br /><br />

            </div>
            <div>
                <asp:label runat="server" ID="lblCaratulasTaller" meta:resourcekey="lblCaratulasTaller" Font-Bold="true" />
                <asp:CheckBox runat="server" ID="chkCaratulasTaller" Checked="true" onclick="Sam.Produccion.ToggleLista(this);" />
                <br />
                <asp:CheckBox runat="server" ID="chkResumenMaterialesTaller" meta:resourcekey="chkResumenMaterialesTaller" Checked="true" /><br />
                <asp:CheckBox runat="server" ID="chkCorteTaller" meta:resourcekey="chkCorteTaller" Checked="true" />
            </div>
            </div>
            <p></p>
        </div>
        <div class="divIzquierdo ancho40">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">&nbsp;</div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
    </div>
    <div class="separador clear">
        <asp:Button CssClass="boton" runat="server" ID="btnImprimir" OnClick="btnImprimir_Click" meta:resourcekey="btnImprimir" />
        <asp:CustomValidator    meta:resourcekey="cusCheck"
                                runat="server"
                                ID="cusCheck" 
                                Display="None" 
                                
                                OnServerValidate="cusCheck_ServerValidate" />
    </div>
</div>
</asp:Content>
