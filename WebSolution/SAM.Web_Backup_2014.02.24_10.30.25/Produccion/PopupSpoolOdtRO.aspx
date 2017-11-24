<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopupSpoolOdtRO.aspx.cs" Inherits="SAM.Web.Produccion.PopupSpoolOdtRO" %>
<%@ Register Src="~/Controles/SpoolOdt/InfoSpoolOdtRO.ascx" tagname="InfoSpoolOdtRO" tagprefix="sam" %>
<%@ Register Src="~/Controles/SpoolOdt/JuntaOdtRO.ascx" tagname="JuntaOdtRO" tagprefix="sam" %>
<%@ Register Src="~/Controles/SpoolOdt/MaterialOdtRO.ascx" tagname="MaterialOdtRO" tagprefix="sam" %>
<%@ Register Src="~/Controles/SpoolOdt/CorteOdtRO.ascx" tagname="CorteOdtRO" tagprefix="sam" %>
<%@ Register Src="~/Controles/SpoolOdt/HoldOdtRO.ascx" tagname="HoldOdtRO" tagprefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div style="width: 900px;">
        <div class="headerAzul">
            <span class="tituloBlanco">
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
            </span>
        </div>
        <div class="popupSpoolRO" style="height: 400px;">
            <telerik:RadTabStrip runat="server" ID="tab" MultiPageID="mpSpool" Orientation="HorizontalBottom" CssClass="mueveIzquierda">
                <Tabs>
                    <telerik:RadTab meta:resourcekey="tabSpool" Selected="true" />
                    <telerik:RadTab meta:resourcekey="tabJuntas" />
                    <telerik:RadTab meta:resourcekey="tabMateriales" />
                    <telerik:RadTab meta:resourcekey="tabCortes" />
                    <telerik:RadTab meta:resourcekey="tabHolds" />
                </Tabs>
            </telerik:RadTabStrip>
            <div class="controles">
                <telerik:RadMultiPage runat="server" ID="mpSpool">
                    <telerik:RadPageView ID="pvSpool" runat="server" Selected="true">
                        <sam:InfoSpoolOdtRO ID="infoSpool" runat="server" />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvJuntas" runat="server">
                        <sam:JuntaOdtRO ID="juntas" runat="server" />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvMateriales" runat="server">
                        <sam:MaterialOdtRO ID="materiales" runat="server" />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvCortes" runat="server">
                        <sam:CorteOdtRO ID="cortes" runat="server" />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvHolds" runat="server">
                        <sam:HoldOdtRO runat="server" ID="holds" />
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </div>
        </div>
    </div>
</asp:Content>
