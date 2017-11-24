<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopupSoldadura.aspx.cs" Inherits="SAM.Web.WorkStatus.PopupSoldadura" %>
<%@ Register Src="~/Controles/Workstatus/Soldadura/SoldadorRaiz.ascx" TagName="SoldadorRaiz" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Workstatus/Soldadura/SoldadorRelleno.ascx" TagName="SoldadorRelleno" TagPrefix="ctrl" %>
<%@ Register Src="~/Controles/Workstatus/Soldadura/InfoGeneral.ascx" TagName="Info" TagPrefix="ctInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<telerik:RadWindow runat="server" ID="rdwCambiarFechaArmado">
    <ContentTemplate>
        <div style="margin-left: 30px; margin-top: 10px">
            <asp:HiddenField runat="server" ID="hdnCambiaFechas"/>
                <div class="divIzquierdo ancho50 boldElements">
                    
                    <asp:Label ID="lblEncabezadoFechaProcesoAnterior" runat="server" meta:resourcekey="lblEncabezadoFechaProcesoAnterior"/>
                    <asp:Label ID="lblFechaProcesoAnterior" runat="server" />        
                    <p></p>
                    <div class="separador">
                        <asp:Label ID="lblNuevaFecha" runat="server" meta:resourcekey="lblNuevaFecha"/>
                        <br />
                        <mimo:MappableDatePicker ID="mdpFechaSoldadura" runat="server" Style="width: 209px"/>
                    </div>
                    <p></p>
                </div>
                <div class="divDerecho ancho50 boldElements">
                    <asp:Label ID="lblEncabezadoFechaReporteProcesoAnterior" runat="server" meta:resourcekey="lblEncabezadoFechaReporteProcesoAnterior"/>
                    <asp:Label ID="lblFechaReporteProcesoAnterior" runat="server" />        
                    <p></p>
                    <div class="separador">
                        <asp:Label ID="lblNuevaFechaReporte" runat="server" meta:resourcekey="lblNuevaFechaReporte"/>
                        <br />
                        <mimo:MappableDatePicker ID="mdpFechaReporteSoldadura" runat="server" Style="width: 209px" />
                    </div>
                    <p></p>
                </div>
            <p>
            </p>   
        </div>     
    </ContentTemplate>
</telerik:RadWindow>
      <div style="width: 750px;">
        <div class="headerAzul">
            <span class="tituloBlanco">
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
            </span>
        </div>


        <asp:TextBox ID="hfFechaArmado" runat="server" style="display: none" />
        <asp:TextBox ID="hfFechaIV" runat="server" style="display: none" />        
    
        <asp:CustomValidator runat="server" ID="cvFechaIV" ErrorMessage="" ControlToValidate="hfFechaIV" ClientValidationFunction="Sam.Workstatus.CVFechaReporteIVContraSoldadura" EnableClientScript="true" Display="None" ValidationGroup="valGuardar">
        </asp:CustomValidator>
        
        <div class="popupSpoolRO" >
            <telerik:RadTabStrip runat="server" ID="tab" MultiPageID="mpSoldadura" Orientation="HorizontalBottom" CausesValidation="false" >
                <Tabs>
                    <telerik:RadTab meta:resourcekey="tabInfo" Selected="true" />
                    <telerik:RadTab meta:resourcekey="tabSoldadorRaiz" />
                    <telerik:RadTab meta:resourcekey="tabSoldadorRelleno" />
                </Tabs>
            </telerik:RadTabStrip>
            <div class="controles">
                <telerik:RadMultiPage runat="server" ID="mpSoldadura">
                    <telerik:RadPageView ID="pvInfoGeneral" runat="server" Selected="true">                                            
                        <ctInfo:Info runat="server"  ID="ctrlInfo" OnWpsSeleccionado="wpsInfoSeleccionado" OnWpsRellenoSeleccionado="wpsRellenoInfoSeleccionado" 
                            OnWpsDiferenteCambio="WpsDiferenteCambio" OnLimpiarSoldaduraConRaiz="ctrlInfo_LimpiarSoldaduraConRaiz"/>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvSoldadorRaiz" runat="server">
                        <ctrl:SoldadorRaiz runat="server" ID="ctrlRaiz" OnProcesoRaizSeleccionado="procesoSeleccionado" OnWpsSeleccionado="wpsSeleccionado"
                             OnEnviarSoldadores="EnviarSoldadores"  />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvSoldadorRelleno" runat="server">
                        <ctrl:SoldadorRelleno runat="server" ID="ctrlRell" OnProcesoRellenoSeleccionado="procesoRellenoSeleccionado" OnWpsSeleccionado="wpsRellenoSeleccionado" />
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </div>
            <p></p>
        </div>
        <div class="pestanaBoton" id="divGuardar" runat="server">
             <asp:Button runat="server" ID="btnGuardar" meta:resourcekey="btnGuardar" CssClass="boton" OnClick="btnGuardarPopUp_OnClick" ValidationGroup="valGuardar" OnClientClick="return Sam.Workstatus.ValidaFechaReporteSoldadura();" />
        </div>
          <div class="pestanaBoton" id="divBotonEdicionEspecial" runat="server" visible="false">
             <asp:Button runat="server" ID="btnGuardarEdicionEspecial" meta:resourcekey="btnGuardar" CssClass="boton" OnClick="btnGuardarEdicionEspecial_Click" 
                 ValidationGroup="valGuardar" />
        </div>
        <asp:Panel CssClass="cajaNaranja" ID="pnlEspesor" runat="server" Visible="false">
            <asp:Label ID="lblEspesorCero" runat="server" meta:resourcekey="lblEspesorCero" CssClass="bold" />
        </asp:Panel>
    </div>
</asp:Content>
