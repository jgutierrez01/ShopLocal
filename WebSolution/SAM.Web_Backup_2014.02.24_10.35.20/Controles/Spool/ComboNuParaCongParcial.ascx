<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComboNuParaCongParcial.ascx.cs" Inherits="SAM.Web.Controles.Spool.ComboNuParaCongParcial" %>
<telerik:RadComboBox    ID="rcbNumeroUnico"
                        runat="server"
                        Width="170px"
                        OnClientItemsRequesting="Sam.WebService.ComboOnNumeroUnicoParaAsignacionRequested"
                        OnClientSelectedIndexChanged="Sam.WebService.ComboOnNumeroUnicoParaAsignacionIndexChanged"                        
                        EnableLoadOnDemand="true"
                        ShowMoreResultsBox="true" 
                        EnableVirtualScrolling="true"
                        EnableItemCaching="true" >
    <WebServiceSettings Path="~/Webservices/ComboboxWebService.asmx" Method="ListaNumeroUnicoItemCodePorMaterialSpoolID" />
</telerik:RadComboBox>
<asp:HiddenField runat="server" ID="hdnMaterialSpoolID" />
<asp:HiddenField runat="server" ID="hdnCantidad" />