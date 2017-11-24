<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpPintura.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpPintura" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
 <h4>
         <asp:Literal runat="server" ID="lblReporte" meta:resourcekey="lblReporte" />
    </h4>
    <div>
        <div class="divIzquierdo ancho70">
            <div class="divIzquierdo ancho50">
                <div id="SandBlast">
                    <h3><asp:Literal runat="server" meta:resourcekey="litSandBlast" ID="litSandBlast" ></asp:Literal></h3>
                    <div class="separador">
                        <asp:Label ID="lblFechaReporte" runat="server" meta:resourcekey="lblFechaReporte"
                            CssClass="bold" />
                        <br />
                        <telerik:RadDatePicker ID="rdpFechaSandBlast" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                            EnableEmbeddedSkins="false"  Width="230px" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox ID="txtReporteSandBlast" runat="server" 
                            meta:resourcekey="txtNumeroReporte" MaxLength="50" >
                        </mimo:LabeledTextBox>
                    </div>               
                </div>
                <div id="Primario">
                    <h3><asp:Literal runat="server" meta:resourcekey="litPrimario" ID="litPrimario" ></asp:Literal></h3>
                    <div class="separador">
                        <asp:Label ID="lblFechaPrimario" runat="server" meta:resourcekey="lblFechaReporte"
                            CssClass="bold" />
                        <br />
                        <telerik:RadDatePicker ID="rdpFechaPrimario" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                            EnableEmbeddedSkins="false" Width="230px" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox ID="txtReportePrimario" runat="server" 
                            meta:resourcekey="txtNumeroReporte" MaxLength="50" >
                        </mimo:LabeledTextBox>
                    </div>    
                </div>    
                <div id="Intermedio">
                     <h3><asp:Literal runat="server" meta:resourcekey="litIntermedio" ID="litIntermedio"></asp:Literal></h3>
                    <div class="separador">
                        <asp:Label ID="lblFechaIntermedio" runat="server" meta:resourcekey="lblFechaReporte"
                            CssClass="bold" />
                        <br />
                        <telerik:RadDatePicker ID="rdpFechaIntermedio" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                            EnableEmbeddedSkins="false"  Width="230px" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox ID="txtReporteIntermedio" runat="server" 
                            meta:resourcekey="txtNumeroReporte" MaxLength="50" >
                        </mimo:LabeledTextBox>
                    </div> 
                </div>
                <div class="separador">
                <%--<asp:Button ID="btnGuardar" runat="server" meta:resourcekey="btnGuardar" OnClick="btnGuardar_Click" CssClass="boton" />--%>
                    <mimo:AutoDisableButton ID="btnAGuardar" runat="server" meta:resourcekey="btnGuardar" OnClick="btnGuardar_Click" CssClass="boton"  />
                </div>       
            </div>
            <div class="divDerecho ancho50">
                <div id="Acabado">
                   <h3><asp:Literal runat="server" meta:resourcekey="litAcabadoVisual" 
                           ID="litAcabadoVisual"></asp:Literal></h3>
                    <div class="separador">
                        <asp:Label ID="lblFechaAcabadoVisual" runat="server" meta:resourcekey="lblFechaReporte"
                            CssClass="bold" />
                        <br />
                        <telerik:RadDatePicker ID="rdpAcabadoVisual" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                            EnableEmbeddedSkins="false" Width="230px" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox ID="txtReporteAcabadoVisual" runat="server" 
                            meta:resourcekey="txtNumeroReporte" MaxLength="50" >
                        </mimo:LabeledTextBox>
                    </div>   
                </div>   
                <div id="Adherencia">         
                    <h3><asp:Literal runat="server" meta:resourcekey="litAdherencia" ID="litAdherencia" ></asp:Literal></h3>
                    <div class="separador">
                        <asp:Label ID="lblFechaAdherencia" runat="server" meta:resourcekey="lblFechaReporte"
                            CssClass="bold" />
                        <br />
                        <telerik:RadDatePicker ID="rdpFechaAdherencia" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                            EnableEmbeddedSkins="false"  Width="230px" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox ID="txtReporteAdherencia" runat="server" 
                            meta:resourcekey="txtNumeroReporte" MaxLength="50">
                        </mimo:LabeledTextBox>
                    </div>        
                </div>
                <div id="Pull">
                     <h3><asp:Literal runat="server" meta:resourcekey="litPullOff" 
                             ID="LilitPullOffteral3"></asp:Literal></h3>
                    <div class="separador">
                        <asp:Label ID="lblPullOff" runat="server" meta:resourcekey="lblFechaReporte"
                            CssClass="bold" />
                        <br />
                        <telerik:RadDatePicker ID="rdpPullOff" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050"
                            EnableEmbeddedSkins="false" Width="230px" />
                    </div>
                    <div class="separador">
                        <mimo:LabeledTextBox ID="txtReportePullOff" runat="server" 
                            meta:resourcekey="txtNumeroReporte" MaxLength="50" >
                        </mimo:LabeledTextBox>
                    </div>    
                </div>
                <div class="separador">
                <asp:CheckBox CssClass="checkBold" ID="chkLiberado" meta:resourcekey="chkLiberado" runat="server"/>
                </div>
            </div>
        </div>
        <div class="divDerecho ancho30">
            <div class="validacionesRecuadro">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <div class="separador">
                        <asp:ValidationSummary runat="server" ID="summaryReporte" meta:resourcekey="valSummary"
                            CssClass="summary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
