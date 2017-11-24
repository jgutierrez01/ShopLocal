<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Armado.ascx.cs" Inherits="SAM.Web.Controles.JuntaCampo.Armado" %>
<div style="width: 735;">
    <h4>
        <asp:Literal meta:resourcekey="lblArmado" runat="server" ID="lblArmado" />
    </h4>
    <div class="cajaAzul">
        <div class="divIzquierdo ancho50">
        <p>
            <asp:Label runat="server" ID="lblSpool" meta:resorucekey="lblSpool"></asp:Label>
            <asp:Literal runat="server" ID="litSpool"></asp:Literal>
            </p>
            <p>
            <asp:Label runat="server" ID="lblJunta" meta:resorucekey="lblJunta"></asp:Label>
            <asp:Literal runat="server" ID="litJunta"></asp:Literal></p>
            <p>
            <asp:Label runat="server" ID="lblFamAcero" meta:resorucekey="lblFamAcero"></asp:Label>
            <asp:Literal runat="server" ID="litFamAcero"></asp:Literal>
            </p>            
        </div>
        <div class="divIzquierdo">
        <p>
            <asp:Label runat="server" ID="lblNumeroControl" meta:resorucekey="lblNumeroControl"></asp:Label>
            <asp:Literal runat="server" ID="litNumeroControl"></asp:Literal></p>
            <p>
            <asp:Label runat="server" ID="lblLocalizacion" meta:resorucekey="lblLocalizacion"></asp:Label>
            <asp:Literal runat="server" ID="litLocalizacion"></asp:Literal></p>
            <p>
            <asp:Label runat="server" ID="lblEspesor" meta:resorucekey="lblEspesor"></asp:Label>
            <asp:Literal runat="server" ID="litEspesor"></asp:Literal>
            </p>
        </div>
    </div>
    <div class="divIzquierdo ancho70">
        <div class="divIzquierdo ancho50 boldElements">
            <div class="separador">
                <asp:Label runat="server" ID="lblFechaArmado" meta:resourcekey="lblFechaArmado" />
                <br />
                <mimo:MappableDatePicker ID="mdpFechaArmado" runat="server" Style="width: 209px" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="valFechaArmado" meta:resourcekey="valFechaArmado"
                    runat="server" ControlToValidate="mdpFechaArmado" Display="None"></asp:RequiredFieldValidator>
            </div>                       
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtSpool1" meta:resourcekey="txtSpool1"
                    ReadOnly="true" />
            </div>
            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtEtiquetaMaterial1" meta:resourcekey="txtEtiquetaMaterial1" ReadOnly="true" />                
            </div>
             <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtNumUnico1" meta:resourcekey="txtNumUnico1" ReadOnly="true" />                
            </div>
             <div class="separador">
                <asp:Label runat="server" ID="lblTubero" meta:resourcekey="lblTubero" />
                <div id="templateItemCode" class="sys-template">
                    <table class="rcbGenerico" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="codigo">
                                {{Codigo}}
                            </td>
                            <td>
                                {{NombreCompleto}}
                            </td>
                        </tr>
                    </table>
                </div>
                <telerik:RadComboBox ID="rcbTubero" runat="server" Width="200px" Height="150px" OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                    EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true" AutoPostBack="true"
                    CausesValidation="false" OnClientItemDataBound="Sam.WebService.TuberoTablaDataBound"
                    OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                    DropDownCssClass="liGenerico" DropDownWidth="400px">
                    <WebServiceSettings Method="ListaTuberosPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                    <HeaderTemplate>
                        <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                            <tr>
                                <th class="codigo">
                                    <asp:Literal ID="litCodigo" runat="server" meta:resourcekey="litCodigo"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="litNombre" runat="server" meta:resourcekey="litNombre"></asp:Literal>
                                </th>
                            </tr>
                        </table>
                    </HeaderTemplate>
                </telerik:RadComboBox>
                <span class="required">*</span>
                <asp:CustomValidator    
                    meta:resourcekey="valTubero"
                    runat="server" 
                    ID="valTubero" 
                    Display="None" 
                    ControlToValidate="rcbTubero" 
                    ValidateEmptyText="true"                    
                    ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                    OnServerValidate="cusRcbTubero_ServerValidate" />
                
            </div>
            <div class="divDerecho ancho50 boldElements">
             <div class="separador">
                <asp:Label runat="server" ID="lblFechaReporte" meta:resourcekey="lblFechaReporte" />
                <br />
                <mimo:MappableDatePicker ID="mdpFechaReporte" runat="server" Style="width: 209px" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="valFechaReporte" meta:resourcekey="valFechaReporte"
                    runat="server" ControlToValidate="mdpFechaReporte" Display="None"></asp:RequiredFieldValidator>
            </div>                       
            <div class="separador">
                <asp:Label CssClass="labelHack bold" runat="server" ID="lblSpool2" meta:resourcekey="lblSpool2"></asp:Label>                    
                <telerik:RadComboBox ID="radCmbSpool2" runat="server" Height="150px"                    
                        EnableLoadOnDemand="true"
                        ShowMoreResultsBox="true"
                        EnableVirtualScrolling="true"
                        OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" 
                        OnSelectedIndexChanged="radCmbOrdenTrabajo_OnSelectedIndexChanged"
                        OnClientSelectedIndexChanged="Sam.Filtro.OrdenTrabajoOnClientSelectedIndexChanged"
                        AutoPostBack="true"                         
                        meta:resourcekey="radCmbSpool2"
                        CausesValidation="false">

                        <WebServiceSettings Method="ListaOrdenTrabajoPorUserScope" Path="~/WebServices/ComboboxWebService.asmx" />
                
                </telerik:RadComboBox>    
            </div>
            <div class="separador">
                <asp:Label runat="server" meta:resourcekey="lblEtiquetaMaterial2" ID="lblEtiquetaMaterial2" CssClass="labelHack bold"></asp:Label>            
            <asp:DropDownList ID="ddlEtiquetaMaterial" runat="server" OnSelectedIndexChanged="ddlddlEtiquetaMaterialSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
            
            </div>
             <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtNumeroUnico2" meta:resourcekey="txtNumeroUnico2" ReadOnly="true" />                
            </div>
            <div class="separador">
                <mimo:LabeledTextBox ID="txtObservaciones" runat="server" meta:resourcekey="txtObservaciones"
                    TextMode="MultiLine" Rows="3" MaxLength="500">
                </mimo:LabeledTextBox>
            </div>
            </div>
            <p>
            </p>
        </div>
       
        <input type="hidden" runat="server" id="hdnProyectoID" />
        <p>
        </p>
    </div>
    <div class="divDerecho ancho30">
        <div class="validacionesRecuadro" style="margin-top: 20px;">
            <div class="validacionesHeader">
                &nbsp;
            </div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ValidationGroup="valGuardar" ID="valSummary"
                    CssClass="summary" meta:resourcekey="valSummary" Width="120" />
            </div>
        </div>
    </div>
    <p>
    </p>
</div>
