<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FiltroGenerico.ascx.cs" Inherits="SAM.Web.Controles.Proyecto.FiltroGenerico" %>

<telerik:RadAjaxManagerProxy ID="AjaxManagerProxy1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="ddlProyecto">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="phFiltroCompleto" UpdatePanelRenderMode="Inline" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="radCmbNumeroUnico">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="phFiltroCompleto" UpdatePanelRenderMode="Inline" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="radCmbNumeroControl">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="phFiltroCompleto" UpdatePanelRenderMode="Inline" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="radCmbOrdenTrabajo">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="phFiltroCompleto" UpdatePanelRenderMode="Inline" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="radComboCuadrante">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="phFiltroCompleto" UpdatePanelRenderMode="Inline" />
            </UpdatedControls>
        </telerik:AjaxSetting>
         <telerik:AjaxSetting AjaxControlID="radCmbEmbarque">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="phFiltroCompleto" UpdatePanelRenderMode="Inline" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManagerProxy>

<asp:Panel runat="server" ID="phFiltroCompleto" CssClass="filtro">
    <asp:Panel ID="phProyecto" runat="server" CssClass="divIzquierdo">
        <div class="separador">
            <asp:Label Text="Proyecto" runat="server" meta:resourcekey="lblProyecto" ID="lblProyecto" CssClass="labelHack bold"></asp:Label>
            <asp:DropDownList ID="ddlProyecto" runat="server" OnSelectedIndexChanged="ddlProyectoSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            <asp:PlaceHolder ID="phProyReq" runat="server" Visible="false">
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="rfvProyecto" runat="server" ControlToValidate="ddlProyecto"
                    InitialValue="" meta:resourcekey="valProyecto" Display="None" CssClass="bold" Enabled="false" />
            </asp:PlaceHolder>
        </div>
    </asp:Panel>

    <asp:Panel ID="phOrdenTrabajo" runat="server" CssClass="divIzquierdo">
        <div class="separador">
            <asp:Label CssClass="labelHack bold" runat="server" ID="lblOrdenTrabajo" meta:resourcekey="lblOrdenTrabajo"></asp:Label>
            <telerik:RadComboBox ID="radCmbOrdenTrabajo" runat="server" Height="150px"
                EnableLoadOnDemand="true"
                ShowMoreResultsBox="true"
                EnableVirtualScrolling="true"
                OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                OnSelectedIndexChanged="radCmbOrdenTrabajo_OnSelectedIndexChanged"
                OnClientSelectedIndexChanged="Sam.Filtro.OrdenTrabajoOnClientSelectedIndexChanged"
                AutoPostBack="true"
                meta:resourcekey="radCmbOrdenTrabajo"
                CausesValidation="false">

                <WebServiceSettings Method="ListaOrdenTrabajoPorUserScope" Path="~/WebServices/ComboboxWebService.asmx" />

            </telerik:RadComboBox>

            <asp:PlaceHolder ID="phOtReq" runat="server" Visible="false">
                <span class="required">*</span>
                <asp:CustomValidator
                    meta:resourcekey="valOrdenTrabajo"
                    runat="server"
                    ID="rfvOt"
                    Display="None"
                    ControlToValidate="radCmbOrdenTrabajo"
                    ValidateEmptyText="true"
                    ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                    CssClass="bold"
                    Enabled="false"
                    OnServerValidate="cusValOrdenTrabajo_ServerValidate" />
            </asp:PlaceHolder>
        </div>
    </asp:Panel>

    <asp:Panel ID="phNumeroControl" runat="server" CssClass="divIzquierdo">
        <div class="separador">
            <asp:Label runat="server" meta:resourcekey="lblNumeroControl" ID="lblNumeroControl" CssClass="labelHack bold"></asp:Label>
            <telerik:RadComboBox ID="radCmbNumeroControl" runat="server" Height="150px"
                EnableLoadOnDemand="true"
                ShowMoreResultsBox="true"
                EnableVirtualScrolling="true"
                OnClientItemsRequesting="Sam.Filtro.NumControlOnClientItemsRequestingEventHandler"
                OnSelectedIndexChanged="radCmbNumeroControl_OnSelectedIndexChanged"
                meta:resourcekey="radCmbNumeroControl"
                CausesValidation="false">
                <WebServiceSettings Method="ListaNumerosControlPorUserScope" Path="~/WebServices/ComboboxWebService.asmx" />
            </telerik:RadComboBox>

            <asp:PlaceHolder ID="phNumControl" runat="server" Visible="false">
                <span class="required">*</span>
                <asp:CustomValidator
                    meta:resourcekey="valNumeroControl"
                    runat="server"
                    ID="rfvNumControl"
                    Display="None"
                    ControlToValidate="radCmbNumeroControl"
                    ValidateEmptyText="true"
                    ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                    CssClass="bold"
                    Enabled="false"
                    OnServerValidate="cusValNumeroControl_ServerValidate" />
            </asp:PlaceHolder>
        </div>
    </asp:Panel>
    <asp:Panel ID="phEmbarques" runat="server" CssClass="divIzquierdo" Visible="false">
        <div class="separador">
            <asp:Label ID="lblEmbarque" meta:resourcekey="lblEmbarque" CssClass="labelHack bold" runat="server" />
            <telerik:RadComboBox ID="radCmbEmbarque" runat="server" Height="150px"
                EnableAutomaticLoadOnDemand ="true" 
                ShowMoreResultsBox="true"
                EnableVirtualScrolling="true"
                OnClientItemsRequesting="Sam.Filtro.EmbarqueOnclientItemRequesting"
                OnSelectedIndexChanged="radCmbEmbarque_SelectedIndexChanged"
                meta:resourcekey="radCmbEmbarque"
                AutoPostBack="true"   
                CausesValidation="false">
                <WebServiceSettings Method="ListaNumerosEmbarquePorProyecto" Path="~/WebServices/ComboboxWebService.asmx"></WebServiceSettings>
            </telerik:RadComboBox>

            <asp:PlaceHolder ID="phValEmbarque" runat="server" Visible="false">
                <span class="required">*</span>
                <asp:CustomValidator meta:resourcekey="valEmbarque"
                    runat="server" 
                    ID="rfvEmbarque"
                    Display="None" 
                    ControlToValidate="radCmbEmbarque"
                    ValidateEmptyText="true"
                    ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                    CssClass="bold"
                    Enabled="false"
                    OnServerValidate="rfvEmbarque_ServerValidate" />   
            </asp:PlaceHolder>     
        </div>
    </asp:Panel>

     <asp:Panel ID="phCuadrantes" runat="server" CssClass="divIzquierdo" Visible="false">
        <div class="separador">
            <asp:Label ID="lblCuadrante" meta:resourcekey="lblCuadrante" CssClass="labelHack bold" runat="server"></asp:Label>
            <telerik:RadComboBox ID="radComboCuadrante" runat="server" Height="150px"
                EnableLoadOnDemand="true"
                ShowMoreResultsBox="true"
                EnableVirtualScrolling="true"
                OnClientItemsRequesting="Sam.Filtro.CuadranteOnclientItemRequesting"
                OnSelectedIndexChanged="radComboCuadrante_SelectedIndexChanged"
                meta:resourcekey="radComboCuadrante"
                EnableItemCaching="true"
                AutoPostBack="true"
                CausesValidation="false">
                <WebServiceSettings Method="ListaCuadrantesPorProyecto" Path="~/WebServices/ComboboxWebService.asmx"></WebServiceSettings>
            </telerik:RadComboBox>
            <asp:PlaceHolder ID="phFCuadrante" runat="server" Visible="false">
                <span class="required">*</span>
                <asp:CustomValidator
                    meta:resourcekey="valCuadrante"
                    runat="server"
                    ID="rfvCuadrante"
                    Display="None"
                    ControlToValidate="radComboCuadrante"
                    ValidateEmptyText="true"
                    ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                    CssClass="bold"
                    Enabled="false"
                    OnServerValidate="rfvCuadrante_ServerValidate" />
            </asp:PlaceHolder>
        </div>
    </asp:Panel>

    <asp:Panel ID="phNumeroUnico" runat="server" CssClass="divIzquierdo">
        <div class="separador">
            <asp:Label runat="server" meta:resourcekey="lblNumeroUnico" ID="lblNumeroUnico" CssClass="labelHack bold"></asp:Label>
            <telerik:RadComboBox ID="radCmbNumeroUnico" runat="server" Height="150px"
                EnableLoadOnDemand="true"
                ShowMoreResultsBox="true"
                EnableVirtualScrolling="true"
                OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                OnSelectedIndexChanged="radCmbNumeroUnico_OnSelectedIndexChanged"
                EnableItemCaching="true"
                meta:resourcekey="radCmbNumeroUnico"
                AutoPostBack="true"
                CausesValidation="false">
                <WebServiceSettings Method="ListaNumeroUnicoPorUserScope" Path="~/WebServices/ComboboxWebService.asmx" />

            </telerik:RadComboBox>

            <asp:PlaceHolder ID="phNumUnicoRequerido" runat="server" Visible="false">
                <span class="required">*</span>
                <asp:CustomValidator
                    meta:resourcekey="valNumeroUnico"
                    runat="server"
                    ID="rfvNumUnico"
                    Display="None"
                    ControlToValidate="radCmbNumeroUnico"
                    ValidateEmptyText="true"
                    ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                    CssClass="bold"
                    Enabled="false"
                    OnServerValidate="cusValNumeroUnico_ServerValidate" />
            </asp:PlaceHolder>
        </div>
    </asp:Panel>

</asp:Panel>
