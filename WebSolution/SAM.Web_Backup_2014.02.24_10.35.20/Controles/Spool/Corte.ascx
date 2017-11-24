<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Corte.ascx.cs" Inherits="SAM.Web.Controles.Spool.Corte" %>
<div class="contenedorCentral">
    <asp:HiddenField ID="hdnProyectoID" runat="server" />
    <div>
        <div class="valGroupArriba">
            <asp:CustomValidator runat="server" ID="cvDiametro" ControlToValidate="txtDiametro1"
                OnServerValidate="cvDiametro_ServerValidate" Enabled="true" ValidationGroup="vgCorte"
                Display="None" meta:resourcekey="cvDiametro" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div class="clear">
        <asp:ValidationSummary runat="server" ID="valSummaryPrincipal" DisplayMode="BulletList" CssClass="summaryList"
                    meta:resourcekey="valSummary" />
            <mimo:MimossRadGrid ID="grdCortes" runat="server" OnNeedDataSource="grdCortes_OnNeedDataSource"
                OnItemCommand="grdCortes_ItemCommand" OnItemDataBound="grdCortes_ItemDataBound"
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
                        <telerik:GridTemplateColumn UniqueName="btnEditar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lnkEditar" CommandName="Editar" CausesValidation="false"
                                    CommandArgument='<%#Eval("CorteSpoolID") %>' meta:resourcekey="glbEditar">
                                    <asp:Image ImageUrl="~/Imagenes/Iconos/editar.png" ID="imgEditar" runat="server"
                                        meta:resourcekey="imgEditar" /></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false"
                                    CommandArgument='<%#Eval("CorteSpoolID") %>' OnClientClick="return Sam.Confirma(1);">
                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="EtiquetaMaterial" DataField="EtiquetaMaterial"
                            HeaderText="Etiqueta Material" HeaderStyle-Width="120" FilterControlWidth="100"
                            Groupable="false" meta:resourcekey="gbcEtiquetaMaterial" />
                        <telerik:GridBoundColumn UniqueName="ItemCodeID" DataField="ItemCodeID" Visible="false" />
                        <telerik:GridBoundColumn UniqueName="ItemCode" HeaderText="ItemCode" HeaderStyle-Width="120"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcItemCode" />
                        <telerik:GridBoundColumn UniqueName="ItemCodeDesc" HeaderStyle-Width="350"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcItemCodeDesc" />
                        <telerik:GridBoundColumn UniqueName="Diametro1" DataField="Diametro" HeaderText="Diametro"
                            DataFormatString="{0:#0.000}" HeaderStyle-Width="120" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDiametro" />
                        <telerik:GridBoundColumn UniqueName="Cantidad" DataField="Cantidad" HeaderText="Cantidad"
                            HeaderStyle-Width="120" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcCantidad" />
                        <telerik:GridBoundColumn UniqueName="EtiquetaSeccion" DataField="EtiquetaSeccion"
                            HeaderText="Etiqueta Seccion" HeaderStyle-Width="120" FilterControlWidth="100"
                            Groupable="false" meta:resourcekey="gbcEtiquetaSeccion" />
                        <telerik:GridBoundColumn UniqueName="InicioFin" DataField="InicioFin" HeaderText="Inicio/Fin"
                            HeaderStyle-Width="120" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcInicioFin" />
                        <telerik:GridBoundColumn UniqueName="TipoCorte1ID" DataField="TipoCorte1ID" Visible="false" />
                        <telerik:GridBoundColumn UniqueName="Profile1" HeaderText="Profile1" HeaderStyle-Width="120"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcProfile1" />
                        <telerik:GridBoundColumn UniqueName="TipoCorte2ID" DataField="TipoCorte2ID" Visible="false" />
                        <telerik:GridBoundColumn UniqueName="Profile2" HeaderText="Profile2" HeaderStyle-Width="120"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcProfile2" />
                        <telerik:GridBoundColumn UniqueName="Observaciones" HeaderText="Observaciones" DataField="Observaciones" HeaderStyle-Width="120"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcObservaciones" />
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
    <asp:Panel runat="server" ID="panelCorte" Visible="false">
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
                        DropDownCssClass="liGenerico" DropDownWidth="400px">
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
                     <asp:CustomValidator    
                        meta:resourcekey="valItemCode"
                        runat="server" 
                        ID="cusItemCode" 
                        Display="None" 
                        ControlToValidate="ddlItemCode" 
                        ValidateEmptyText="true" 
                        ValidationGroup="vgCorte" 
                        ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                        OnServerValidate="cusItemCode_ServerValidate" />
                    
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtEtiquetaMaterial" runat="server" EntityPropertyName="EtiquetaMaterial"
                        MaxLength="50" meta:resourcekey="lblEtiquetaMaterial">
                    </mimo:LabeledTextBox>
                </div>
                <div class="separador">
                    <%--<mimo:LabeledTextBox ID="txtDiametro1" runat="server" EntityPropertyName="Diametro1"
                        MaxLength="50" meta:resourcekey="lblDiametro" />--%>
                        <mimo:RequiredLabeledTextBox ID="txtDiametro1" runat="server" EntityPropertyName="Diametro1"
                        MaxLength="10" meta:resourcekey="lblDiametro" ValidationGroup="vgCorte" />
                    <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txtDiametro1"
                        ErrorMessage="El formato capturado para Diámetro es incorrecto." Display="None"
                        ValidationExpression="^[0-9]{1,3}(\.[0-9]{0,4})?$" ValidationGroup="vgCorte" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtCantidad" runat="server" EntityPropertyName="Cantidad"
                        MaxLength="50" meta:resourcekey="lblCantidad">
                    </mimo:LabeledTextBox>
                    <asp:RegularExpressionValidator runat="server" ID="revCantidad" ControlToValidate="txtCantidad"
                        ErrorMessage="Capture un número entero para el valor Cantidad." Display="None"
                        ValidationExpression="\d*" ValidationGroup="vgCorte" />
                </div>
                <div class="separador">
                    <asp:Label ID="lblObservaciones" runat="server" meta:resourcekey="lblObservaciones" CssClass="bold" />
                    <asp:TextBox Rows="3" Columns="50" TextMode="MultiLine" ID="txtObservaciones" runat="server" Width="84%" ></asp:TextBox>
                </div>
                <br />
                <br />
                <br />
                <br />
            </div>
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtInicioFin" runat="server" EntityPropertyName="InicioFin"
                        MaxLength="50" meta:resourcekey="lblInicioFin">
                    </mimo:LabeledTextBox>
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtEtiquetaSeccion" runat="server" EntityPropertyName="EtiquetaSeccion"
                        MaxLength="50" meta:resourcekey="lblEtiquetaSeccion">
                    </mimo:LabeledTextBox>
                </div>
                <div class="separador">
                    <asp:Label ID="lblProfile1" runat="server" meta:resourcekey="lblProfile1" CssClass="bold" />
                    <mimo:MappableDropDown runat="server" ID="ddlProfile1" EntityPropertyName="Profile1ID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvProfile1" ControlToValidate="ddlProfile1"
                        InitialValue="" ErrorMessage="Profile1 Requerido" Display="None" ValidationGroup="vgCorte"
                        meta:resourcekey="rfvProfile1" />
                </div>
                <div class="separador">
                    <asp:Label ID="lblProfile2" runat="server" meta:resourcekey="lblProfile2" CssClass="bold" />
                    <mimo:MappableDropDown runat="server" ID="ddlProfile2" EntityPropertyName="Profile2ID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvProfile2" ControlToValidate="ddlProfile2"
                        InitialValue="" ErrorMessage="Profile2 Requerido" Display="None" ValidationGroup="vgCorte"
                        meta:resourcekey="rfvProfile2" />
                </div>                
            </div>
            <p>
            </p>
            <div class="separador">
                <asp:HiddenField runat="server" ID="hdnCorteID" />
                <asp:Button runat="server" ID="btnAgregar" CssClass="boton Izquierdo"
                    OnClick="btnAgregar_Click" ValidationGroup="vgCorte" meta:resourcekey="btnAgregar" />
                <asp:Button runat="server" ID="btnEditar" CssClass="boton Izquierdo"
                    OnClick="btnAgregar_Click" ValidationGroup="vgCorte" meta:resourcekey="btnEditar"
                    Visible="false" />
            </div>
        </div>
        <div class="divDerecho ancho35">
            <div class="validacionesRecuadro" style="margin-top: 23px;">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummarySecundario" DisplayMode="BulletList" ValidationGroup="vgCorte"
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
