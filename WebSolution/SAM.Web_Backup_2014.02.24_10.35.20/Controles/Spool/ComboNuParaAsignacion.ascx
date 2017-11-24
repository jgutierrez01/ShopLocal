<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComboNuParaAsignacion.ascx.cs" Inherits="SAM.Web.Controles.Spool.ComboNuParaAsignacion" %>
<telerik:RadComboBox    ID="rcbNumeroUnico" AutoPostBack="true" CausesValidation="false"
                        runat="server"
                        Width="170px"
                        Height="150px"
                        OnClientItemsRequesting="Sam.WebService.ComboOnNumeroUnicoParaAsignacionRequested"
                        OnClientSelectedIndexChanged="Sam.WebService.ComboOnNumeroUnicoParaAsignacionIndexChanged"
                        OnClientItemDataBound="Sam.WebService.ComboOnNumeroUnicoParaAsignacionItemDataBound"
                        EnableLoadOnDemand="true"
                        ShowMoreResultsBox="true" 
                        EnableVirtualScrolling="true" 
                        CssClass="required"
                        AllowCustomText="false"
                        IsCaseSensitive="false"
                        DropDownCssClass="liDespacho"
                        DropDownWidth="500px">
    <HeaderTemplate>
        <table cellspacing="0" cellpadding="0" class="headerRcbNu">
            <tr>
                <th class="cod"><asp:Literal runat="server" ID="litCod" meta:resourcekey="litCod" /></th>
                <th class="inv"><asp:Literal runat="server" ID="litInv" meta:resourcekey="litInv" /></th>
                <th class="diam"><asp:Literal runat="server" ID="litD1" meta:resourcekey="litD1" /></th>
                <th class="diam"><asp:Literal runat="server" ID="litD2" meta:resourcekey="litD2" /></th>
                <th class="ind">&nbsp;</th>
                <th class="codIc"><asp:Literal runat="server" ID="litIcCod" meta:resourcekey="litIcCod" /></th>
                <th class="last"><asp:Literal runat="server" ID="litDesc" meta:resourcekey="litDesc" /></th>
            </tr>
        </table>
    </HeaderTemplate>
    <WebServiceSettings Path="~/Webservices/ComboboxWebService.asmx" Method="NumerosUnicosParaAsignacion" />
</telerik:RadComboBox>
<span class="required">*</span>
<asp:HiddenField runat="server" ID="hdnMaterialSpoolID" />
<asp:CustomValidator    runat="server" 
                        ID="cusCombo" 
                        ControlToValidate="rcbNumeroUnico" 
                        ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValorCustom" 
                        OnServerValidate="cusCombo_ServerValidate"
                        ValidateEmptyText="true" 
                        Display="None" />