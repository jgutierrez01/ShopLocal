<%@ Page Language="C#" MasterPageFile="~/Masters/PopupJuntasCampo.Master" AutoEventWireup="true" CodeBehind="PopupJuntasCampo.aspx.cs" Inherits="SAM.Web.Produccion.PopupJuntasCampo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphBody" runat="server">   
    <div>
        <div class="headerAzul">
            <span class="tituloBlanco">
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
            </span>
        </div>
        <div class="popupJuntasCampo">
           <telerik:RadTabStrip runat="server" ID="tab" MultiPageID="mpJuntaCampo" Orientation="HorizontalBottom" OnTabClick="tab_TabClick" CausesValidation="false">
                <Tabs>
                    <telerik:RadTab Value="Armado" meta:resourcekey="tabArmado" Selected="true" />
                    <telerik:RadTab meta:resourcekey="tabSoldadura" />
                    <telerik:RadTab meta:resourcekey="tabInspeccionVisual" />
                    <telerik:RadTab meta:resourcekey="tabRequisiciones" />
                    <telerik:RadTab meta:resourcekey="tabPruebasPND" />
                    <telerik:RadTab meta:resourcekey="tabPruebasTT" />
                </Tabs>
            </telerik:RadTabStrip>
            <div class="controles">
                <telerik:RadMultiPage runat="server" ID="mpJuntaCampo">
                    <telerik:RadPageView ID="rpArmado" runat="server" ContentUrl="~/Produccion/JuntasCampo/Armado.aspx" Selected="true" Height="550px" Width="750px"></telerik:RadPageView>
                    <telerik:RadPageView ID="rpSoldadura" runat="server" ContentUrl="~/Produccion/JuntasCampo/Soldadura.aspx"  Height="800px" Width="750px"></telerik:RadPageView>
                    <telerik:RadPageView ID="rpInspeccionVisual" runat="server" ContentUrl="~/Produccion/JuntasCampo/InspeccionVisual.aspx"  Height="800px" Width="750px"></telerik:RadPageView>
                    <telerik:RadPageView ID="rpRequisiciones" runat="server" ContentUrl="~/Produccion/JuntasCampo/Requisiciones.aspx"  Height="500px" Width="750px"></telerik:RadPageView>
                    <telerik:RadPageView ID="rpPND" runat="server" ContentUrl="~/Produccion/JuntasCampo/PruebasPND.aspx"  Height="1000px" Width="750px"></telerik:RadPageView>
                    <telerik:RadPageView ID="rpTT" runat="server" ContentUrl="~/Produccion/JuntasCampo/PruebasTT.aspx"  Height="500px" Width="750px"></telerik:RadPageView>                    
                </telerik:RadMultiPage>
            </div>
        </div>   
    </div>
</asp:Content>
