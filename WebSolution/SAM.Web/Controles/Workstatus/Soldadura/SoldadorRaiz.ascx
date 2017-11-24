<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SoldadorRaiz.ascx.cs"
    Inherits="SAM.Web.Controles.Workstatus.Soldadura.SoldadorRaiz" %>
<div style="width: 735px;">
    <div class="divIzquierdo ancho50">
        <asp:HiddenField runat="server" ID="hdnPatioID" />
        <asp:HiddenField runat="server" ID="hdnProyectoID" />
        <div class="separador">
            <asp:Label runat="server" ID="lblProcesoRaiz" meta:resourcekey="lblProcesoRaiz" CssClass="bold" />
            <br />
            <asp:DropDownList runat="server" ID="ddlProcesoRaiz" OnSelectedIndexChanged="ddlProcesoRaiz_SelectedIndexChanged"
                AutoPostBack="true" />
            <span class="required">*</span>
            <asp:RequiredFieldValidator runat="server" ID="valProcesoRaiz" ValidationGroup="valGuardar"
                InitialValue="" Display="None" meta:resourcekey="valProcesoRaiz" ControlToValidate="ddlProcesoRaiz" />
        </div>
        <asp:Panel ID="pnlWps" runat="server">
        <div class="separador">
                <asp:Label runat="server" ID="lblWps" CssClass="bold" meta:resourcekey="lblWps" />
                <br />
                <mimo:MappableDropDown runat="server" ID="ddlWps" OnSelectedIndexChanged="ddlWps_SelectedIndexChanged"
                    AutoPostBack="true" Enabled="false"/>           
        </div>
        </asp:Panel>
        <asp:Panel ID="pnlSoldador" runat="server">
            <div class="separador">
                <asp:Label runat="server" ID="txtCodigoSoldador" meta:resourcekey="txtCodigoSoldador"
                    CssClass="bold" />
                <br />
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
                <telerik:RadComboBox ID="rcbSoldador" runat="server" Width="200px" Height="150px"
                    OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" EnableLoadOnDemand="true" AutoPostBack="true"
                    ShowMoreResultsBox="true" EnableVirtualScrolling="true" CausesValidation="false"
                    OnClientItemDataBound="Sam.WebService.SoldadorTablaDataBound" OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                    DropDownCssClass="liGenerico" DropDownWidth="400px">
                    <WebServiceSettings Method="ListaSoldadoresPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
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
                        meta:resourcekey="valSoldador"
                        runat="server" 
                        ID="valSoldador" 
                        Display="None" 
                        ControlToValidate="rcbSoldador" 
                        ValidateEmptyText="true" 
                        ValidationGroup="valRaiz" 
                        ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                        OnServerValidate="cusRcbSoldador_ServerValidate" />                    
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblColada" meta:resourcekey="lblColada" CssClass="bold" />
                <br />
                <telerik:RadComboBox ID="ddlConsumibles" runat="server" Width="200px" Height="150px"
                    EmptyMessage=" " EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                    OnClientItemsRequesting="Sam.Workstatus.SoldaduraConsumibles" Enabled="true"
                    ValidationGroup="valRaiz">
                    <WebServiceSettings Method="ListaConsumiblesPorPatio" Path="~/WebServices/ComboboxWebService.asmx" />
                </telerik:RadComboBox>
                <asp:CustomValidator    
                        meta:resourcekey="valColada"
                        runat="server" 
                        ID="valColada" 
                        Display="None" 
                        ControlToValidate="ddlConsumibles" 
                        ValidateEmptyText="true" 
                        ValidationGroup="valRaiz" />
                
            </div>
            <p>
            </p>
            <asp:Button runat="server" ID="btnAgregar" meta:resourcekey="btnAgregar" CssClass="boton"
                OnClick="btnAgregar_OnClick" ValidationGroup="valRaiz" />
        </asp:Panel>
    </div>
    <div class="divDerecho ancho45">
        <div class="validacionesRecuadro" style="margin-top: 20px;">
            <div class="validacionesHeader">
                &nbsp;
            </div>
            <div class="validacionesMain">
                <asp:ValidationSummary runat="server" ValidationGroup="valRaiz" ID="valSummary" CssClass="summary"
                    meta:resourcekey="valSummary" Width="120" />
                <asp:ValidationSummary runat="server" ValidationGroup="valGuardar" ID="valSummaryGuardar"
                    CssClass="summary" meta:resourcekey="valSummaryGuardar" Width="120" />
            </div>
        </div>
    </div>
    <p>
    </p>
    <div>
        <br />
        <asp:Repeater ID="grdSoldadores" runat="server" OnItemCommand="grdSoldadores_ItemCommand"
            Visible="false">
            <HeaderTemplate>
                <table class="repSam" cellpadding="0" cellspacing="0" width="100%">
                    <thead>
                        <tr class="repEncabezado">
                            <th colspan="4">
                                &nbsp;
                            </th>
                        </tr>
                        <tr class="repTitulos">
                            <th class="accion">
                                &nbsp;
                            </th>
                            <th>
                                <asp:Literal ID="litCodigoSoldador" runat="server" meta:resourcekey="codigoSoldador"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="litNombre" runat="server" meta:resourcekey="NombreCompleto"></asp:Literal>
                            </th>
                            <th>
                                <asp:Literal ID="litConsumible" runat="server" meta:resourcekey="codigoConsumible"></asp:Literal>
                            </th>
                        </tr>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <tr class="repFila">
                    <td class="accion">
                        <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("SoldadorID") + "::" + Eval("ConsumibleID") %>'
                            OnClientClick="return Sam.Confirma(1);">
                            <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                meta:resourcekey="imgBorrar" /></asp:LinkButton>
                    </td>
                    <td>
                        <%# Eval("CodigoSoldador")%>
                    </td>
                    <td>
                        <%# Eval("NombreCompleto")%>
                    </td>
                    <td>
                        <%# Eval("CodigoConsumible")%>
                    </td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr class="repFilaPar">
                    <td>
                        <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server"  CommandArgument='<%#Eval("SoldadorID") + "::" + Eval("ConsumibleID") %>'
                            OnClientClick="return Sam.Confirma(1);">
                            <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                meta:resourcekey="imgBorrar" /></asp:LinkButton>
                    </td>
                    <td>
                        <%# Eval("CodigoSoldador")%>
                    </td>
                    <td>
                        <%# Eval("NombreCompleto")%>
                    </td>
                    <td>
                        <%# Eval("CodigoConsumible")%>
                    </td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                <tfoot>
                    <tr class="repPie">
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </tfoot>
                </table></FooterTemplate>
        </asp:Repeater>
    </div>
    <asp:Repeater ID="repSoldadoresReadOnly" runat="server" OnItemCommand="grdSoldadores_ItemCommand"
        Visible="false">
        <HeaderTemplate>
            <table class="repSam" cellpadding="0" cellspacing="0" width="100%">
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="3">
                            &nbsp;
                        </th>
                    </tr>
                    <tr class="repTitulos">
                        <th>
                            <asp:Literal ID="litCodigoSoldador" runat="server" meta:resourcekey="codigoSoldador"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="litNombre" runat="server" meta:resourcekey="NombreCompleto"></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="litConsumible" runat="server" meta:resourcekey="codigoConsumible"></asp:Literal>
                        </th>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="repFila">
                <td>
                    <%# Eval("CodigoSoldador")%>
                </td>
                <td>
                    <%# Eval("NombreCompleto")%>
                </td>
                <td>
                    <%# Eval("CodigoConsumible")%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="repFilaPar">
                <td>
                    <%# Eval("CodigoSoldador")%>
                </td>
                <td>
                    <%# Eval("NombreCompleto")%>
                </td>
                <td>
                    <%# Eval("CodigoConsumible")%>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            <tfoot>
                <tr class="repPie">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
            </tfoot>
            </table></FooterTemplate>
    </asp:Repeater>
</div>
