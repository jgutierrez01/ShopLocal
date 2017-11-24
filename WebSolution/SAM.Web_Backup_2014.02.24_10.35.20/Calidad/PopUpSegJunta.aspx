<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpSegJunta.aspx.cs" Inherits="SAM.Web.Calidad.PopUpSegJunta" %>

<%@ Register TagPrefix="calidad" Namespace="SAM.Web.Controles.Calidad" %>
<%@ Register src="~/Controles/Calidad/SegJunta/InspEspesores.ascx" tagname="InspEspesores" tagprefix="uc1" %>
<%@ Register src="~/Controles/Calidad/SegJunta/InspDimensional.ascx" tagname="InspDimensional" tagprefix="uc2" %>
<%@ Register src="~/Controles/Calidad/SegJunta/InspVisual.ascx" tagname="InspVisual" tagprefix="uc3" %>
<%@ Register src="~/Controles/Calidad/SegJunta/Soldadura.ascx" tagname="Soldadura" tagprefix="uc4" %>
<%@ Register src="~/Controles/Calidad/SegJunta/Armado.ascx" tagname="Armado" tagprefix="uc5" %>
<%@ Register src="~/Controles/Calidad/SegJunta/InfoGeneral.ascx" tagname="InfoGeneral" tagprefix="uc6" %>
<%@ Register src="~/Controles/Calidad/SegJunta/PruebaUT.ascx" tagname="PruebaUT" tagprefix="uc7" %>
<%@ Register src="~/Controles/Calidad/SegJunta/PruebaPTPostTT.ascx" tagname="PruebaPTPostTT" tagprefix="uc8" %>
<%@ Register src="~/Controles/Calidad/SegJunta/PruebaRTPostTT.ascx" tagname="PruebaRTPostTT" tagprefix="uc9" %>
<%@ Register src="~/Controles/Calidad/SegJunta/PruebaPT.ascx" tagname="PruebaPT" tagprefix="uc10" %>
<%@ Register src="~/Controles/Calidad/SegJunta/PruebaRT.ascx" tagname="PruebaRT" tagprefix="uc11" %>
<%@ Register src="~/Controles/Calidad/SegJunta/TratamientoPWHT.ascx" tagname="TratamientoPWHT" tagprefix="uc12" %>
<%@ Register src="~/Controles/Calidad/SegJunta/TratamientoDurezas.ascx" tagname="TratamientoDurezas" tagprefix="uc13" %>
<%@ Register src="~/Controles/Calidad/SegJunta/TratamientoPreheat.ascx" tagname="TratamientoPreheat" tagprefix="uc14" %>
<%@ Register src="~/Controles/Calidad/SegJunta/Pintura.ascx" tagname="Pintura" tagprefix="uc15" %>
<%@ Register src="~/Controles/Calidad/SegJunta/Embarque.ascx" tagname="Embarque" tagprefix="uc16" %>
<%@ Register src="~/Controles/Calidad/SegJunta/PruebaPMI.ascx" tagname="PruebaPMI" tagprefix="uc17" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <div class="headerAzul">
            <asp:Label runat="server" meta:resourcekey="lblTitulo" CssClass="tituloBlanco"></asp:Label>
    </div>
            
    <div class="popupSegRO soloLectura">
        <telerik:RadTabStrip runat="server" ID="menuConfiguracion" MultiPageID="rmConfiguracion"
            SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="false" BackColor="" style="margin-left:-1px;">
            <Tabs>
                <telerik:RadTab Value="Info" meta:resourcekey="info" />
                <telerik:RadTab Value="Armado" meta:resourcekey="armado" />
                <telerik:RadTab Value="Soldadura" meta:resourcekey="soldadura" />
                <telerik:RadTab Value="InspVisual" meta:resourcekey="inspVisual" />
                <telerik:RadTab Value="InspDimensional" meta:resourcekey="inspDimensional" />
                <telerik:RadTab Value="InspEspesores" meta:resourcekey="inspEspesores" />
                <telerik:RadTab Value="PruebaRT" meta:resourcekey="rt" />
                <telerik:RadTab Value="PruebaPT" meta:resourcekey="pt" />
                <telerik:RadTab Value="Tratamiento PWHT" meta:resourcekey="pwht" />
                <telerik:RadTab Value="TratamientoDurezas" meta:resourcekey="durezas" />
                <telerik:RadTab Value="PruebaRTPostTT" meta:resourcekey="rtPost" />
                <telerik:RadTab Value="PruebaPTPostTT" meta:resourcekey="ptPost" />
                <telerik:RadTab Value="TratamientoPreheat" meta:resourcekey="preheat" />
                <telerik:RadTab Value="PruebaUT" meta:resourcekey="ut" />
                <telerik:RadTab Value="PruebaPMI" meta:resourcekey="pmi" />
                <telerik:RadTab Value="Pintura" meta:resourcekey="pintura" />
                <telerik:RadTab Value="Embarque" meta:resourcekey="embarque" />
            </Tabs>
        </telerik:RadTabStrip>
        <div class="dashboardCentral">
            <telerik:RadMultiPage runat="server" ID="rmConfiguracion" SelectedIndex="0">
                <telerik:RadPageView ID="rpvInfo" runat="server">                    
                    
                    <uc6:InfoGeneral ID="InfoGeneral1" runat="server" />
                    
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvArmado" runat="server">                    
                    
                    <uc5:Armado ID="Armado1" runat="server" />
                    
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvSoldadura" runat="server">                    
                    
                    <uc4:Soldadura ID="Soldadura1" runat="server" />
                    
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvInspVisual" runat="server">
                    
                    <uc3:InspVisual ID="InspVisual1" runat="server" />
                    
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvInspDim" runat="server">
                    
                    <uc2:InspDimensional ID="InspDimensional1" runat="server" />
                    
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvInspEspesores" runat="server">
                    
                    <uc1:InspEspesores ID="InspEspesores1" runat="server" />
                    
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvPruebaRT" runat="server">
                    <uc11:PruebaRT ID="PruebaRT1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvPruebaPT" runat="server">
                    <uc10:PruebaPT ID="PruebaPT1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvTratPWHT" runat="server">
                    <uc12:TratamientoPWHT ID="TratamientoPWHT" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvTratDureza" runat="server">
                    <uc13:TratamientoDurezas ID="TratamientoDurezas" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvPruebaRTPost" runat="server">
                    <uc9:PruebaRTPostTT ID="PruebaRTPostTT1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvPruebaPTPost" runat="server">
                    <uc8:PruebaPTPostTT ID="PruebaPTPostTT1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvTratPreheat" runat="server">
                    <uc14:TratamientoPreheat ID="TratamientoPreheat" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvPruebaUT" runat="server">
                    <uc7:PruebaUT ID="PruebaUT1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvPruebaPMI" runat="server">
                    <uc17:PruebaPMI ID="PruebaPMI1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvPintura" runat="server">
                    <uc15:Pintura ID="Pintura" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvEmbarque" runat="server">
                    <uc16:Embarque ID="Embarque" runat="server" />
                </telerik:RadPageView>
            </telerik:RadMultiPage>            
            <p>
            </p>            
            
        </div>
    </div>


</asp:Content>
