<%@ Page Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopupSpoolRO.aspx.cs" Inherits="SAM.Web.Produccion.PopupSpoolRO" EnableSessionState="ReadOnly" EnableViewState="false" %>
<%@ Register Src="~/Controles/Spool/InfoSpoolRO.ascx" tagname="InfoSpoolRO" tagprefix="sam" %>
<%@ Register Src="~/Controles/Spool/JuntaRO.ascx" tagname="JuntaRO" tagprefix="sam" %>
<%@ Register Src="~/Controles/Spool/MaterialRO.ascx" tagname="MaterialRO" tagprefix="sam" %>
<%@ Register Src="~/Controles/Spool/CorteRO.ascx" tagname="CorteRO" tagprefix="sam" %>
<%@ Register Src="~/Controles/Spool/HoldRO.ascx" tagname="HoldRO" tagprefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <div style="width: 750px;">
        <div class="headerAzul">
            <span class="tituloBlanco">
                <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
            </span>
        </div>
        <div class="popupSpoolRO">
            <telerik:RadTabStrip runat="server" ID="tab" MultiPageID="mpSpool" Orientation="HorizontalBottom">
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
                        <sam:InfoSpoolRO ID="infoSpool" runat="server" />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvJuntas" runat="server">
                        <sam:JuntaRO ID="juntas" runat="server" />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvMateriales" runat="server">
                        <sam:MaterialRO ID="materiales" runat="server" />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvCortes" runat="server">
                        <sam:CorteRO ID="cortes" runat="server" />
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="pvHolds" runat="server">
                        <sam:HoldRO runat="server" ID="holds" />
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </div>
        </div>
    </div>
</asp:Content>
