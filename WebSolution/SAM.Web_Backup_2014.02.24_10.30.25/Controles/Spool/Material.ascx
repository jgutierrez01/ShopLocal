<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Material.ascx.cs" Inherits="SAM.Web.Controles.Spool.Material" %>
<div class="contenedorCentral">
    <asp:HiddenField ID="hdnProyectoID" runat="server" />
    <div>
        <div class="valGroupArriba">
            <asp:CustomValidator runat="server" ID="cvDiametro1" ControlToValidate="txtDiametro1"
                OnServerValidate="cvDiametro1_ServerValidate" Enabled="true" ValidationGroup="vgMaterial"
                Display="None" meta:resourcekey="cvDiametro1"></asp:CustomValidator>
            <asp:CustomValidator runat="server" ID="cvDiametro2" ControlToValidate="txtDiametro2"
                OnServerValidate="cvDiametro2_ServerValidate" Enabled="true" ValidationGroup="vgMaterial"
                Display="None" meta:resourcekey="cvDiametro2"></asp:CustomValidator>
            <asp:ValidationSummary runat="server" ID="vsEncabezado" DisplayMode="BulletList"
                ValidationGroup="vgEncabezadoMaterial" CssClass="summaryList" meta:resourcekey="valSummary" />
        </div>
        <div class="clear">
            <mimo:MimossRadGrid ID="grdMaterial" runat="server" OnNeedDataSource="grdMaterial_OnNeedDataSource"
                OnItemCommand="grdMaterial_ItemCommand" OnItemDataBound="grdMaterial_ItemDataBound"
                AllowFilteringByColumn="false" AllowPaging="false" Height="250">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:LinkButton runat="server" ID="lnkAgregar" OnClick="lnkAgregar_OnClick" meta:resourcekey="lnkAgregar"
                                CssClass="link" />
                            <asp:ImageButton runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png"
                                OnClick="lnkAgregar_OnClick" AlternateText="Actualizar" CssClass="imgEncabezado" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderStyle-Width="30" Groupable="false"
                            ShowFilterIcon="false" ShowSortIcon="false" Resizable="false" Reorderable="false"
                            UniqueName="Armar_h">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlArmar" meta:resourcekey="hlArmar" Visible="false"
                                    ImageUrl="~/Imagenes/Iconos/ico_cortarB.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnEditar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Editar" ID="imgEditar" runat="server" meta:resourcekey="imgEditar"
                                    ImageUrl="~/Imagenes/Iconos/editar.png" CommandArgument='<%#Eval("MaterialSpoolID") %>'
                                    CausesValidation="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" ID="imgBorrar" runat="server" meta:resourcekey="imgBorrar"
                                    ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("MaterialSpoolID") %>'
                                    OnClientClick="return Sam.Confirma(1);" CausesValidation="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="Etiqueta" DataField="Etiqueta" HeaderStyle-Width="120"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcEtiqueta" />
                        <telerik:GridBoundColumn UniqueName="ItemCode" DataField="ItemCode.Codigo" HeaderStyle-Width="120"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcItemCode" />
                        <telerik:GridBoundColumn UniqueName="ItemCodeID" DataField="ItemCodeID" HeaderStyle-Width="120"
                            FilterControlWidth="100" Visible="false" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="Descripcion" DataField="DescripcionMaterial"
                            HeaderStyle-Width="250" FilterControlWidth="200" Groupable="false" meta:resourcekey="gbcDescripcion" />
                        <telerik:GridBoundColumn UniqueName="Diametro1" DataField="Diametro1" HeaderStyle-Width="120"
                            DataFormatString="{0:#0.000}" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDiametro1" />
                        <telerik:GridBoundColumn UniqueName="Diametro2" DataField="Diametro2" HeaderStyle-Width="120"
                            DataFormatString="{0:#0.000}" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDiametro2" />
                        <telerik:GridBoundColumn UniqueName="Cantidad" DataField="Cantidad" HeaderStyle-Width="120"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcCantidad" />
                        <telerik:GridBoundColumn UniqueName="Grupo" DataField="Grupo" HeaderStyle-Width="120"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcGrupo" />
                        <telerik:GridBoundColumn UniqueName="Especificacion" DataField="Especificacion" HeaderStyle-Width="120"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcEspecificacion" />
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
    </div>
    <asp:Panel runat="server" ID="panelMaterial" Visible="false">
        <br />
        <br />
        <div class="divIzquierdo ancho60">
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <asp:Label runat="server" ID="lblItemCode" meta:resourcekey="lblItemCode" CssClass="bold" />
                    <br />
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
                    <telerik:RadComboBox ID="ddlItemCode" runat="server" Width="200px" Height="150px"
                        OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" EnableLoadOnDemand="true"
                        ShowMoreResultsBox="true" EnableVirtualScrolling="true" CausesValidation="false"
                        OnClientItemDataBound="Sam.WebService.ItemCodeTablaDataBound" OnClientSelectedIndexChanged="Sam.WebService.RadComboOnSelectedIndexChanged"
                        DropDownCssClass="liGenerico" DropDownWidth="400px" AutoPostBack="true">
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
                    <asp:CustomValidator meta:resourcekey="rfvItemCode" runat="server" ID="cusItemCode"
                        Display="None" ControlToValidate="ddlItemCode" ValidateEmptyText="true" ValidationGroup="vgMaterial"
                        ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor" OnServerValidate="cusItemCode_ServerValidate" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtDescripcionMaterial" EntityPropertyName="DescripcionMaterial"
                        ValidationGroup="vgMaterial" meta:resourcekey="lblDescripcionMaterial">
                    </mimo:RequiredLabeledTextBox>
                </div>

                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtDiametro1" EntityPropertyName="Diametro1"
                        ValidationGroup="vgMaterial" meta:resourcekey="lblDiametro1">
                    </mimo:RequiredLabeledTextBox>
                    <asp:RegularExpressionValidator runat="server" ID="revDiametro1" ControlToValidate="txtDiametro1"
                        Display="None" ValidationExpression="^[0-9]{1,3}(\.[0-9]{0,4})?$" ValidationGroup="vgMaterial"
                        meta:resourcekey="revDiametro1" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtDiametro2" EntityPropertyName="Diametro2"
                        ValidationGroup="vgMaterial" meta:resourcekey="lblDiametro2">
                    </mimo:RequiredLabeledTextBox>
                    <asp:RegularExpressionValidator runat="server" ID="revDiametro2" ControlToValidate="txtDiametro2"
                        Display="None" ValidationExpression="^[0-9]{1,3}(\.[0-9]{0,4})?$" ValidationGroup="vgMaterial"
                        meta:resourcekey="revDiametro2" />
                </div>
            </div>
            <div class="divDerecho ancho50">
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtCantidad" runat="server" EntityPropertyName="Cantidad"
                        MaxLength="50" meta:resourcekey="lblCantidad" />
                    <asp:RegularExpressionValidator runat="server" ID="revCantidad" ControlToValidate="txtCantidad"
                        Display="None" ValidationExpression="\d*" ValidationGroup="vgMaterial" meta:resourcekey="revCantidad"></asp:RegularExpressionValidator>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtEtiqueta" runat="server" EntityPropertyName="Etiqueta"
                        Label="Etiqueta:" MaxLength="50" meta:resourcekey="lblEtiqueta" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtEspecificacion" runat="server" EntityPropertyName="Especificacion"
                        Label="Especificación:" MaxLength="50" meta:resourcekey="lblEspecificacion" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtGrupo" runat="server" EntityPropertyName="Grupo" Label="Grupo:"
                        MaxLength="50" meta:resourcekey="lblGrupo" />
                </div>
            </div>
            <p>
            </p>
            <div class="separador">
                <asp:HiddenField runat="server" ID="hdnMaterialID" />
                <asp:Button runat="server" ID="btnAgregar" CssClass="boton Izquierdo" OnClick="btnAgregar_Click"
                    ValidationGroup="vgMaterial" meta:resourcekey="btnAgregar" />
                <asp:Button runat="server" ID="btnEditar" CssClass="boton Izquierdo" OnClick="btnAgregar_Click"
                    ValidationGroup="vgMaterial" Visible="false" meta:resourcekey="btnEditar" />            
                <asp:Label runat="server" ID="lblAdvertencia" meta:resourcekey="lblAdvertencia" CssClass="boldUnderline" Visible="false"/>
            </div>            
        </div>
        <div class="divDerecho ancho35">
            <div class="validacionesRecuadro" style="margin-top: 23px;">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" ValidationGroup="vgMaterial"
                        CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
        <p>
        </p>
        <br />
        <br />
    </asp:Panel>
</div>
