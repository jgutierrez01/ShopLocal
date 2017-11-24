<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomologacionSpool.aspx.cs" MasterPageFile="~/Masters/Ingenieria.Master"  Inherits="SAM.Web.Ingenieria.HomologacionSpool" ValidateRequest="false" %>
<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>

<%@ Register Src="~/Controles/Ingenieria/CorteRO.ascx" TagName="CorteRO" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Ingenieria/MaterialRO.ascx" TagName="MaterialRO" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Ingenieria/JuntaRO.ascx" TagName="JuntaRO" TagPrefix="uc3" %>
<%@ Register Src="~/Controles/Ingenieria/InfoSpoolRO.ascx" TagName="SpoolRO" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    
 <div class="headerAzul ancho100">
        <div class="divIzquierdo ancho70">
            <asp:Label ID="Label1" runat="server" meta:resourcekey="lblTitulo" CssClass="tituloBlanco"></asp:Label>
        </div>
        <p>
        </p>
    </div>
    <div class="homologacionPopUp soloLectura ancho100">
        <telerik:RadTabStrip runat="server" ID="menuConfiguracion" MultiPageID="rmConfiguracion"
            SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="false" BackColor=""
            Style="margin-left: -1px; margin-right: -1px;" CssClass="ancho40 divIzquierdo">
            <Tabs>
                <telerik:RadTab Value="InfoGeneral" meta:resourcekey="InfoGeneral" />
                <telerik:RadTab Value="Material" meta:resourcekey="Material" />
                <telerik:RadTab Value="Junta" meta:resourcekey="Junta" />
                <telerik:RadTab Value="Corte" meta:resourcekey="Corte" />
            </Tabs>
        </telerik:RadTabStrip>
        <div class="ancho40 divIzquierdo " style="margin-top:5px">
            
            
        </div>
        <p>
        </p>
        <div class="" style="min-height:370px; min-width:980px">
            <telerik:RadMultiPage runat="server" ID="rmConfiguracion" SelectedIndex="0">
                <telerik:RadPageView ID="rpvInfo" runat="server">
                    <uc4:SpoolRO runat="server" ID="SpoolRO1" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvMaterial" runat="server">
                    <uc2:MaterialRO ID="MaterialRO1" runat="server" />
                </telerik:RadPageView>                
                <telerik:RadPageView ID="rpvJunta" runat="server">
                    <uc3:JuntaRO ID="JuntaRO1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvCorte" runat="server">
                    <uc1:CorteRO ID="CorteRO1" runat="server" />
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <p>
            </p>        
        </div>

        <asp:Panel id="divError" runat="server" CssClass="summaryList" Visible="false" >
            <asp:Literal runat="server" meta:resourcekey="litTituloError" />  
            <ul>
                <li>
                    <asp:Literal ID="litError" runat="server" meta:resourcekey="litError" />        
                </li>
            </ul>    
        </asp:Panel>
        

        <asp:Panel ID="pnlBotones" runat="server" CssClass="divDerecho">            
            <asp:Button ID="btnGuardar" OnClick="btnGuardar_OnClick" OnClientClick="return Sam.Ingenieria.ValidaHomologacionCompleta()" runat="server" Text="Guardar" CssClass="boton" meta:resourcekey="btnGuardar" />
            <samweb:BotonProcesando ID="btnSiguiente" OnClick="btnSiguiente_OnClick"  runat="server" Text="Siguiente" CssClass="boton" CausesValidation="false" meta:resourcekey="btnSiguiente"/>
            <asp:Button ID="btnGuardarContinuar" OnClick="btnGuardarContinuar_OnClick" OnClientClick="return Sam.Ingenieria.ValidaHomologacionCompleta()" runat="server" Text="Guardar y Continuar" CssClass="boton" meta:resourcekey="btnGuardarContinuar"/>
            <samweb:BotonProcesando ID="btnCancelar" OnClick="btnCancelar_OnClick"  runat="server" Text="NO homologar" CssClass="boton" CausesValidation="false" meta:resourcekey="btnNoHomologar"/>
        </asp:Panel>
        
        
    </div>
</asp:Content>