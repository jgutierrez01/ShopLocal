<%@ Page Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="ImportConfigProyecto.aspx.cs" Inherits="SAM.Web.Catalogos.ImportConfigProyecto" %>

<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/ImportarProyecto/WPS.ascx" TagName="WPS" TagPrefix="sam" %>
<%@ Register Src="~/Controles/ImportarProyecto/TablaDestajos.ascx" TagName="TablaDestajos"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/ImportarProyecto/ICEquivalentes.ascx" TagName="ICEquivalentes"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="encabezadoProyecto"
    TagPrefix="ctrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
<ctrl:encabezadoProyecto ID="proyEncabezado" runat="server" Visible="true" />
<div class="paginaHeader">
    
    <sam:BarraTituloPagina runat="server" ID="lblImportConfigProyecto" NavigateUrl="~/Catalogos/lstProyecto.aspx"
        meta:resourcekey="lblImportConfigProyecto" /> 
    </div>       
        <div class="cntCentralForma">
        <div class="ancho100">
            <telerik:RadTabStrip runat="server" ID="tabMenu" MultiPageID="rmpImportConfigProyecto"
                SelectedIndex="0" Orientation="HorizontalBottom" CausesValidation="false">
                <Tabs>
                    <telerik:RadTab Value="WPS" meta:resourcekey="rtWPS" />
                    <telerik:RadTab Value="Tablas de Destajo" meta:resourcekey="rtTablaDestajos" />
                    <telerik:RadTab Value="IC Equivalentes" meta:resourcekey="rtICEquivalentes" />
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="rmpImportConfigProyecto" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="rpvWPS" runat="server">
                    <sam:WPS ID="ctrlWPS" runat="server"></sam:WPS>
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvTablaDestajos" runat="server">
                    <sam:TablaDestajos ID="ctrlTablaDestajos" runat="server"></sam:TablaDestajos>
                </telerik:RadPageView>
                <telerik:RadPageView ID="rpvICEquivalentes" runat="server">
                    <sam:ICEquivalentes ID="ctrlICEquivalentes" runat="server"></sam:ICEquivalentes>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
        </div>
        <p>
        </p>
    </div>
</asp:Content>
