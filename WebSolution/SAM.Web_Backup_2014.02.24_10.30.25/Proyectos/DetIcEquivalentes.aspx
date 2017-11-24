<%@ Page Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true" CodeBehind="DetIcEquivalentes.aspx.cs" Inherits="SAM.Web.Proyectos.DetIcEquivalentes" %>
<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
    <link rel="Stylesheet" href="/Css/combos.css" type="text/css" media="all" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblDetICEquivalentes" />
    <asp:HiddenField runat="server" ID="hdnProyectoID" ClientIDMode="Static" />
    <asp:HiddenField runat="server" ID="hdnGrdClientID" ClientIDMode="Static" />
    <div id="templateItemCode" class="sys-template">
        <table class="rcbGenerico" cellpadding="0" cellspacing="0">
            <tr>
                <td class="codigo">
                    {{Codigo}}
                </td>
                <td>
                    {{Descripcion}}
                </td>
            </tr>
        </table>
    </div>
    <div class="cntCentralForma">
        <div class="dashboardCentral">
            <div class="divIzquierdo ancho70">
                <div class="divIzquierdo ancho30 bandaAzul">
                    <div class="separador">
                        <asp:Label runat="server" ID="lblItemCode" meta:resourcekey="lblItemCode" AssociatedControlID="rcbItemCode" />
                        <telerik:RadComboBox ID="rcbItemCode"
                                             runat="server" AutoPostBack="true"
                                             Width="150px"
                                             Height="150px"
                                             EnableLoadOnDemand="true"
                                             ShowMoreResultsBox="true"
                                             EnableVirtualScrolling="true"
                                             OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                                             OnClientItemDataBound="Sam.WebService.ItemCodeTablaDataBound"
                                             OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                                             DropDownCssClass="liGenerico"
                                             DropDownWidth="400px">
                            <WebServiceSettings Method="ListaItemCodesPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                            <HeaderTemplate>
                                <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                                    <tr>
                                        <th class="codigo">
                                            <asp:Literal ID="litCodigo" runat="server" meta:resourcekey="litCodigo"></asp:Literal>
                                        </th>
                                        <th>
                                            <asp:Literal ID="litDescripcion" runat="server" meta:resourcekey="litDescripcion"></asp:Literal>
                                        </th>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                        </telerik:RadComboBox>
                        <span class="required">*</span>
                        <asp:CustomValidator    meta:resourcekey="cusItemCode"
                                                runat="server" 
                                                ID="cusItemCode" 
                                                Display="None" 
                                                ControlToValidate="rcbItemCode" 
                                                ValidateEmptyText="true" 
                                                ValidationGroup="valGroupGuardar" 
                                                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                                                OnServerValidate="cusItemCode_ServerValidate" />
                        <asp:CustomValidator    meta:resourcekey="cusCuentaEquivalencias"
                                                runat="server" 
                                                ID="cusCuentaEquivalencias" 
                                                Display="None"
                                                ValidationGroup="valGroupGuardar" 
                                                ClientValidationFunction="Sam.Proyectos.Validaciones.AlMenosUnEquivalente"
                                                OnServerValidate="cusCuentaEquivalencias_ServerValidate"
                                                ControlToValidate="txtDummy"
                                                ValidateEmptyText="true" />
                        <asp:TextBox runat="server" ID="txtDummy" CssClass="oculto" />
                    </div>
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox runat="server" ID="txtDiametro1" meta:resourcekey="txtDiametro1" ValidationGroup="valGroupGuardar" MaxLength="6"  Width="145px" />
                        <asp:RangeValidator     runat="server"
                                                ID="rngD1" 
                                                meta:resourcekey="rngD1" 
                                                ValidationGroup="valGroupGuardar" 
                                                ControlToValidate="txtDiametro1" 
                                                Display="None"
                                                Type="Double"
                                                MinimumValue="0"
                                                MaximumValue="200" />
                    </div>
                    <div class="separador">
                        <mimo:RequiredLabeledTextBox runat="server" ID="txtDiametro2" meta:resourcekey="txtDiametro2" ValidationGroup="valGroupGuardar" MaxLength="6"  Width="145px" />
                        <asp:RangeValidator     runat="server"
                                                ID="rngD2" 
                                                meta:resourcekey="rngD2" 
                                                ValidationGroup="valGroupGuardar" 
                                                ControlToValidate="txtDiametro2" 
                                                Display="None"
                                                Type="Double"
                                                MinimumValue="0"
                                                MaximumValue="200" />
                    </div>
                    <div class="separador">
                       <asp:Checkbox runat="server" ID="chkUnidireccional" CssClass="divIzquierdo"/>
                       <asp:Label meta:resourcekey="lblUnidireccional" ID="lblUnidireccional" runat="server"/>
                    </div>
                </div>
                <div class="divIzquierdo" style="width:67%;margin-top:20px;">
                    <div class="cajaAzul">
                        <div class="separador">
                            <div class="divIzquierdo" style="margin-right:5px;">
                                <asp:Label runat="server" ID="lblItemCodeEquivalente" meta:resourcekey="lblItemCodeEquivalente" AssociatedControlID="rcbIcEquivalentes" />
                                <telerik:RadComboBox    ID="rcbIcEquivalentes" AutoPostBack="true"
                                                        runat="server"
                                                        Width="150px"
                                                        Height="150px"
                                                        EnableLoadOnDemand="true" 
                                                        ShowMoreResultsBox="true"
                                                        EnableVirtualScrolling="true"
                                                        OnClientItemsRequesting="Sam.WebService.AgregaProyectoID"
                                                        OnClientItemDataBound="Sam.WebService.ItemCodeTablaDataBound"
                                                        OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                                                        DropDownCssClass="liGenerico"
                                                        DropDownWidth="400px">
                                    <WebServiceSettings Method="ListaItemCodesPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                                    <HeaderTemplate>
                                        <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                                            <tr>
                                                <th class="codigo">
                                                    <asp:Literal ID="litCodigo" runat="server" meta:resourcekey="litCodigo"></asp:Literal>
                                                </th>
                                                <th>
                                                    <asp:Literal ID="litDescripcion" runat="server" meta:resourcekey="litDescripcion"></asp:Literal>
                                                </th>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                </telerik:RadComboBox>
                                <span class="required">*</span>
                                <asp:CustomValidator    meta:resourcekey="cusIcEquivalente"
                                                        runat="server" 
                                                        ID="cusIcEquivalente" 
                                                        Display="None" 
                                                        ControlToValidate="rcbIcEquivalentes" 
                                                        ValidateEmptyText="true" 
                                                        ValidationGroup="valGroupAgregar" 
                                                        ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                                                        OnServerValidate="cusIcEquivalente_ServerValidate" />
                            </div>
                            <div class="divIzquierdo" style="margin-right:5px;">
                                <mimo:RequiredLabeledTextBox runat="server" ID="txtDiametroEquivalente1" meta:resourcekey="txtDiametroEquivalente1" MaxLength="6" Width="50px" ValidationGroup="valGroupAgregar" />
                                <asp:RangeValidator     runat="server"
                                                        ID="rngEqD1" 
                                                        meta:resourcekey="rngEqD1" 
                                                        ValidationGroup="valGroupAgregar" 
                                                        ControlToValidate="txtDiametroEquivalente1" 
                                                        Display="None"
                                                        Type="Double"
                                                        MinimumValue="0"
                                                        MaximumValue="200" />
                            </div>
                            <div class="divIzquierdo" style="margin-right:5px;">
                                <mimo:RequiredLabeledTextBox runat="server" ID="txtDiametroEquivalente2" meta:resourcekey="txtDiametroEquivalente2" MaxLength="6" Width="50px" ValidationGroup="valGroupAgregar" />
                                <asp:RangeValidator     runat="server"
                                                        ID="rngEqD2" 
                                                        meta:resourcekey="rngEqD2" 
                                                        ValidationGroup="valGroupAgregar" 
                                                        ControlToValidate="txtDiametroEquivalente2" 
                                                        Display="None"
                                                        Type="Double"
                                                        MinimumValue="0"
                                                        MaximumValue="200" />
                            </div>
                            <div class="divIzquierdo" style="margin-right:5px;padding-top:7px;">
                                <asp:Button runat="server" ID="btnAgregar" CssClass="boton" meta:resourcekey="btnAgregar" ValidationGroup="valGroupAgregar" OnClick="btnAgregar_OnClick" />
                                <asp:CustomValidator    runat="server"
                                                        ID="cusDatosIzquierda"
                                                        meta:resourcekey="cusDatosIzquierda"
                                                        ControlToValidate="txtDiametro1"
                                                        ValidateEmptyText="true"
                                                        ClientValidationFunction="Sam.Proyectos.Validaciones.ItemCodePadreSeleccionado"
                                                        OnServerValidate="cusDatosIzquierda_ServerValidate"
                                                        Display="None"
                                                        ValidationGroup="valGroupAgregar" />
                            </div>  
                        </div>
                        <p></p>                   
                    </div>
                    <div style="margin-top:10px;">
                        <mimo:MimossRadGrid runat="server" ID="grdIcE" OnNeedDataSource="grdIcE_OnNeedDataSource" OnItemCommand="grdIcE_ItemCommand" AllowPaging="false" Height="250px" EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false" AllowColumnFreezing="false">
                            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="ItemCodeEquivalenteID" AllowFilteringByColumn="false">
                                <CommandItemTemplate>
                                    &nbsp;
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30" Groupable="false">
                                        <ItemTemplate>
                                            <asp:ImageButton meta:resourcekey="imgBorrar" runat="server" CommandArgument='<%#Eval("ItemCodeEquivalenteID") %>' CommandName="Borrar" ImageUrl="~/Imagenes/Iconos/borrar.png" ID="lnkBorrar" OnClientClick="return Sam.Confirma(1);" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="ItemCode" DataField="CodigoEq" meta:resourcekey="htItemCode" Groupable="false" HeaderStyle-Width="120px" />
                                    <telerik:GridBoundColumn UniqueName="Diametro1" DataField="D1Eq" DataFormatString="{0:#0.000}" meta:resourcekey="htDiametro1" Groupable="false" HeaderStyle-Width="50px"/>
                                    <telerik:GridBoundColumn UniqueName="Diametro2" DataField="D2Eq" DataFormatString="{0:#0.000}" meta:resourcekey="htDiametro2" Groupable="false" HeaderStyle-Width="50px"/>
                                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="DescripcionEq" meta:resourcekey="htItemCodeDesc" Groupable="false" />
                                </Columns>
                            </MasterTableView>
                        </mimo:MimossRadGrid>
                    </div>
                </div>
            </div>
            <div class="divIzquierdo ancho30">
                <div class="validacionesRecuadro" style="margin-top:20px;">
                    <div class="validacionesHeader"></div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summary" meta:resourcekey="valSummary" ValidationGroup="valGroupGuardar" />
                        <asp:ValidationSummary runat="server" ID="valSummaryAgregar" CssClass="summary" meta:resourcekey="valSummary" ValidationGroup="valGroupAgregar" />
                    </div>
                </div>
            </div>
            <p></p>
        </div>
    </div>
    <div class="pestanaBoton">
        <asp:Button runat="server" ID="btnGuardar" CssClass="boton" meta:resourcekey="btnGuardar" ValidationGroup="valGroupGuardar" OnClick="btnGuardar_OnClick" />
    </div>
</asp:Content>
