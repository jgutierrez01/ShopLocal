<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpSegSpool.aspx.cs" Inherits="SAM.Web.Calidad.PopUpSegSpool" meta:resourcekey="PageResource1"  %>

<%@ Register TagPrefix="calidad" Namespace="SAM.Web.Controles.Calidad.SegSpool" %>
<%@ Register Src="~/Controles/Calidad/SegSpool/InfoGeneral.ascx" tagname="InfoGeneral" tagprefix="uc1" %>
<%@ Register Src="~/Controles/Calidad/SegSpool/Pintura.ascx" tagname="Pintura" tagprefix="uc2" %>
<%@ Register Src="~/Controles/Calidad/SegSpool/Embarque.ascx" tagname="Embarque" tagprefix="uc3" %>
<%@ Register Src="~/Controles/Calidad/SegSpool/InspDimensional.ascx" tagname="InspDimensional" tagprefix="uc4" %>
<%@ Register Src="~/Controles/Calidad/SegSpool/InspEspesores.ascx" tagname="InspEspesores" tagprefix="uc5" %>


<%@ Register Src="~/Controles/Calidad/SegSpool/Certificado.ascx" tagname="Certificado" tagprefix="uc6" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <div class="headerAzul">
            <asp:Label ID="Label1" runat="server" meta:resourcekey="lblTitulo" CssClass="tituloBlanco"></asp:Label>
    </div>
    
       
    <div class="popupSegRO soloLectura">
        <telerik:RadTabStrip runat="server" ID="menuConfiguracion" MultiPageID="rmConfiguracion"
            SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="False" 
            style="margin-left:-1px;" >
            <Tabs>
                <telerik:RadTab Value="Info" meta:resourcekey="info" Selected="True" />                
                <telerik:RadTab Value="InspDimensional" meta:resourcekey="inspDimensional" />
                <telerik:RadTab Value="InspEspesores" meta:resourcekey="inspEspesores" />                
                <telerik:RadTab Value="Pintura" meta:resourcekey="pintura" />                
                <telerik:RadTab Value="Embarque" meta:resourcekey="embarque" />
            </Tabs>
        </telerik:RadTabStrip>
        <div class="dashboardCentral">
            <telerik:RadMultiPage runat="server" ID="rmConfiguracion" SelectedIndex="0" >
                
                <telerik:RadPageView ID="rpvInfo" runat="server" Selected="True">                    
                                        
                    <uc1:InfoGeneral ID="InfoGeneral1" runat="server" />
                                        
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvInspDimensional" runat="server" >
                    <uc4:InspDimensional ID="InspDimensional1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvInspEspesores" runat="server" >
                    <uc5:InspEspesores ID="InspEspesores1" runat="server" />
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvPintura" runat="server" >
                    <uc2:Pintura ID="Pintura1" runat="server" />
                </telerik:RadPageView>                
                <telerik:RadPageView ID="rpvEmbarque" runat="server">
                    <uc3:Embarque ID="Embarque1" runat="server" />
                </telerik:RadPageView>                
            </telerik:RadMultiPage>            
            <p>
            </p>            
            
        </div>
    </div>


</asp:Content>
