<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactoCliente.ascx.cs"
    Inherits="SAM.Web.Controles.Cliente.Contacto" %>
<%-- <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="btnAgregar">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="phContactos" />
                <telerik:AjaxUpdatedControl ControlID="phContacto" />
            </UpdatedControls>
        </telerik:AjaxSetting>
         <telerik:AjaxSetting AjaxControlID="btnActualizar">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="phContactos" />
                <telerik:AjaxUpdatedControl ControlID="phContacto" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="grdContactos">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="phContactos" />
                <telerik:AjaxUpdatedControl ControlID="phContacto" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>--%>
<asp:PlaceHolder runat="server" ID="phContacto">
    <div class="clear">
        <div class="divIzquierdo ancho70">
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtPuesto" EntityPropertyName="Puesto"
                        MaxLength="50" ValidationGroup="vgContacto" meta:resourcekey="lblPuesto" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre"
                        MaxLength="50" ValidationGroup="vgContacto" meta:resourcekey="lblNombre" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtApPaterno" EntityPropertyName="ApPaterno"
                        MaxLength="50" ValidationGroup="vgContacto" meta:resourcekey="lblApPaterno" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtApMaterno" runat="server" MaxLength="50" meta:resourcekey="lblApMaterno" />
                </div>
            </div>
            <div class="divDerecho ancho50">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtCorreo" EntityPropertyName="CorreoElectronico"
                        MaxLength="50" ValidationGroup="vgContacto" meta:resourcekey="lblCorreo" />
                    <asp:RegularExpressionValidator ID="revCorreo" runat="server" ControlToValidate="txtCorreo"
                        ValidationGroup="vgContacto" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        Display="None" meta:resourcekey="revCorreo" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtTelOficina" runat="server" EntityPropertyName="TelefonoOficina"
                        MaxLength="50" meta:resourcekey="lblTelOficina" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtTelParticular" runat="server" EntityPropertyName="TelefonoParticular"
                        MaxLength="50" meta:resourcekey="lblTelParticular" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtTelCelular" runat="server" EntityPropertyName="TelefonoCelular"
                        MaxLength="50" meta:resourcekey="lblTelCelular" />
                </div>
            </div>
            <p>
                &nbsp;</p>
            <div>
                <div class="separador">
                    <asp:HiddenField runat="server" ID="hdnContactoClienteID" />
                    <asp:Button runat="server" ID="btnAgregar" CssClass="boton" OnClick="btnAgregar_Click"
                        ValidationGroup="vgContacto" meta:resourcekey="btnAgregar" />
                    <asp:Button runat="server" ID="btnActualizar" CssClass="boton" OnClick="btnAgregar_Click"
                        ValidationGroup="vgContacto" Visible="false" meta:resourcekey="btnActualizar" />
                </div>
            </div>
        </div>
        <div class="divDerecho ancho30">
            <div class="validacionesRecuadro" style="margin-top: 23px;">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" EnableClienteScript="true"
                        DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" ValidationGroup="vgContacto" />
                    <asp:ValidationSummary runat="server" ID="valSummaryGenerico" EnableClienteScript="true"
                        DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
    </div>
</asp:PlaceHolder>
<asp:PlaceHolder runat="server" ID="phContactos">
    <div class="clear">
        <mimo:MimossRadGrid ID="grdContactos" runat="server" OnNeedDataSource="grdContactos_OnNeedDataSource"
            OnItemCommand="grdContactos_ItemCommand" AllowFilteringByColumn="false" AllowPaging="false"
            Height="250">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="btnEditar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lnkEditar" CommandName="Editar" CausesValidation="false"
                                CommandArgument='<%#Eval("ContactoClienteID") %>'>
                                <asp:Image ImageUrl="~/Imagenes/Iconos/editar.png" ID="imgEditar" runat="server"
                                    meta:resourcekey="imgEditar" /></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false"
                                CommandArgument='<%#Eval("ContactoClienteID") %>' OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderStyle-Width="120"
                        Groupable="false" meta:resourcekey="gbcNombre" />
                    <telerik:GridBoundColumn UniqueName="CorreoElectronico" DataField="CorreoElectronico"
                        HeaderStyle-Width="170" Groupable="false" meta:resourcekey="gbcCorreo" />
                    <telerik:GridBoundColumn UniqueName="TelOficina" DataField="TelefonoOficina" HeaderStyle-Width="150"
                        Groupable="false" meta:resourcekey="gbcTelOficina" />
                    <telerik:GridBoundColumn UniqueName="TelParticular" DataField="TelefonoParticular"
                        HeaderStyle-Width="150" Groupable="false" meta:resourcekey="gbcTelParticular" />
                    <telerik:GridBoundColumn UniqueName="TelCelular" DataField="TelefonoCelular" HeaderStyle-Width="150"
                        Groupable="false" meta:resourcekey="gbcTelCelular" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
    </div>
</asp:PlaceHolder>
<%--<asp:CustomValidator 
    runat="server" 
    ID="cusNombreCliente"     
    Display="None" 
    ErrorMessage="El nombre del cliente no debe ser blanco"
    Text="El nombre del cliente no debe ser blanco"
    EnableClientScript="true"
    ValidateEmptyText="true"
    ClientValidationFunction="Sam.Catalogos.RevisaNombreCliente"
    ValidationGroup="vgContacto" />--%>
