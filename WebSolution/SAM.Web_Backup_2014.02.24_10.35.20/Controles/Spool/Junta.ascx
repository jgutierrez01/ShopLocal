﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Junta.ascx.cs" Inherits="SAM.Web.Controles.Spool.Junta" %>
<div class="contenedorCentral">
    <div class="valGroupArriba">
        <asp:CustomValidator runat="server" ID="cvDiametro" ControlToValidate="txtDiametro"
            OnServerValidate="cvDiametro_ServerValidate" Enabled="true" ValidationGroup="vgJunta"
            Display="None" meta:resourcekey="cvDiametro"></asp:CustomValidator>
        <asp:CustomValidator runat="server" ID="cvCedula" ControlToValidate="txtCedula" OnServerValidate="cvCedula_ServerValidate"
            Enabled="true" ValidationGroup="vgJunta" Display="None" meta:resourcekey="cvCedula"></asp:CustomValidator>
        <asp:CustomValidator runat="server" ID="cvEtiqueta" ControlToValidate="txtEtiqueta"
            OnServerValidate="cvEtiqueta_ServerValidate" Enabled="true" ValidationGroup="vgJunta"
            Display="None" meta:resourcekey="cvEtiqueta"></asp:CustomValidator>
        <asp:CustomValidator runat="server" ID="cvLocalizacion" ControlToValidate="txtLocalizacion"
            OnServerValidate="cvLocalizacion_ServerValidate" Enabled="true" ValidationGroup="vgJunta"
            Display="None" meta:resourcekey="cvLocalizacion"></asp:CustomValidator>
        <asp:ValidationSummary runat="server" ID="vsEncabezadoJunta" DisplayMode="BulletList"
            ValidationGroup="vgEncabezadoJunta" CssClass="summaryList" meta:resourcekey="valSummary" />
    </div>
    <div class="clear">
        <mimo:MimossRadGrid ID="grdJuntas" runat="server" OnNeedDataSource="grdJuntas_OnNeedDataSource"
            OnItemCommand="grdJuntas_ItemCommand" AllowFilteringByColumn="false" AllowPaging="false"
            OnItemDataBound="grdJuntas_ItemDataBound" Height="250">
            <mastertableview autogeneratecolumns="false" allowmulticolumnsorting="false">
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
                            <asp:ImageButton CommandName="Editar" ID="imgEditar" runat="server" meta:resourcekey="imgEditar"
                                ImageUrl="~/Imagenes/Iconos/editar.png" CommandArgument='<%#Eval("JuntaSpoolID") %>'
                                CausesValidation="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                        Groupable="false">
                        <ItemTemplate>
                            <asp:ImageButton CommandName="Borrar" ID="imgBorrar" runat="server" meta:resourcekey="imgBorrar"
                                ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("JuntaSpoolID") %>'
                                OnClientClick="return Sam.Confirma(1);" CausesValidation="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="JuntaSpoolID" DataField="JuntaSpoolID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="TipoJuntaID" DataField="TipoJuntaID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="FabAreaID" DataField="FabAreaID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="FamiliaAceroMaterial1ID" DataField="FamiliaAceroMaterial1ID"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="FamiliaAceroMaterial2ID" DataField="FamiliaAceroMaterial2ID"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="EtiquetaMaterial1" DataField="EtiquetaMaterial1"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="EtiquetaMaterial2" DataField="EtiquetaMaterial2"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Etiqueta" DataField="Etiqueta" HeaderText="Etiqueta"
                        HeaderStyle-Width="120" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcEtiqueta" />
                    <telerik:GridBoundColumn UniqueName="Diametro" DataField="Diametro" HeaderText="Diametro"
                        DataFormatString="{0:#0.000}" HeaderStyle-Width="120" FilterControlWidth="100"
                        Groupable="false" meta:resourcekey="gbcDiametro" />
                    <telerik:GridBoundColumn UniqueName="TipoJunta" HeaderText="TipoJunta" HeaderStyle-Width="120"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcTipoJunta" />
                    <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" HeaderText="Cédula"
                        HeaderStyle-Width="120" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcCedula" />
                    <telerik:GridBoundColumn UniqueName="Localizacion" HeaderText="Localización" HeaderStyle-Width="120"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcLocalizacion" />
                    <telerik:GridBoundColumn UniqueName="FamiliaAceroMaterial1" HeaderText="Fam. Acero1"
                        HeaderStyle-Width="120" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcFamMaterial1" />
                    <telerik:GridBoundColumn UniqueName="FamiliaAceroMaterial2" HeaderText="Fam. Acero2"
                        HeaderStyle-Width="120" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcFamMaterial2" />
                    <telerik:GridBoundColumn UniqueName="FabArea" HeaderText="Fab. Area" HeaderStyle-Width="120"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcFabArea" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </mastertableview>
        </mimo:MimossRadGrid>
    </div>
    <asp:Panel runat="server" ID="panelJunta" Visible="false">
        <br />
        <br />
        <div class="divIzquierdo ancho60">            
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox ID="txtEtiqueta" runat="server" EntityPropertyName="Etiqueta"
                        MaxLength="10" meta:resourcekey="lblEtiqueta" ValidationGroup="vgJunta" />
                </div>
                <div class="separador">
                    <mimo:RequiredLabeledTextBox ID="txtDiametro" runat="server" EntityPropertyName="Diametro"
                        MaxLength="10" meta:resourcekey="lblDiametro" ValidationGroup="vgJunta" />
                    <%--<mimo:LabeledTextBox ID="txtDiametro" runat="server" EntityPropertyName="Diametro"
                        MaxLength="10" meta:resourcekey="lblDiametro" />--%>
                    <asp:RegularExpressionValidator runat="server" ID="revDiametro" ControlToValidate="txtDiametro"
                        Display="None" ValidationExpression="^[0-9]{1,3}(\.[0-9]{0,4})?$" ValidationGroup="vgJunta" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtCedula" runat="server" EntityPropertyName="Cedula" MaxLength="50"
                        meta:resourcekey="lblCedula" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtLocalizacion" runat="server" EntityPropertyName="Localizacion"
                        MaxLength="10" meta:resourcekey="lblLocalizacion" />
                </div>
                <div class="separador">
                    <asp:Label ID="lblTipoJunta" runat="server" meta:resourcekey="lblTipoJunta" CssClass="bold" />
                    <mimo:MappableDropDown runat="server" ID="ddlTipoJunta" EntityPropertyName="TipoJuntaID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvTipoJunta" ControlToValidate="ddlTipoJunta"
                        InitialValue="" ValidationGroup="vgJunta" Display="None" meta:resourcekey="rfvTipoJunta" />
                </div>
                <div class="separador">
                    <asp:Label ID="lblFabArea" runat="server" meta:resourcekey="lblFabArea" CssClass="bold" />
                    <mimo:MappableDropDown runat="server" ID="ddlFabArea" EntityPropertyName="FabAreaID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvFabArea" ControlToValidate="ddlFabArea"
                        InitialValue="" ValidationGroup="vgJunta" Display="None" meta:resourcekey="rfvFabArea" />
                </div>
            </div>
            <div class="divIzquierdo ancho50">
                <div class="separador">
                    <asp:Label ID="lblFamAcero1" runat="server" meta:resourcekey="lblFamAcero1" CssClass="bold" />
                    <mimo:MappableDropDown runat="server" ID="ddlFamAcero1" EntityPropertyName="FamiliaAceroMaterial1ID" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator runat="server" ID="rfvFamAcero1" ControlToValidate="ddlFamAcero1"
                        InitialValue="" ValidationGroup="vgJunta" Display="None" meta:resourcekey="rfvFamAcero1" />
                </div>
                <div class="separador">
                    <asp:Label ID="lblFamAcero2" runat="server" meta:resourcekey="lblFamAcero2" CssClass="bold" />
                    <mimo:MappableDropDown runat="server" ID="ddlFamAcero2" EntityPropertyName="FamiliaAceroMaterial2ID" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtEspesor" runat="server" EntityPropertyName="Espesor"
                        MaxLength="50" meta:resourcekey="lblEspesor" Enabled="false" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtKgTeoricos" runat="server" Enabled="false" EntityPropertyName="KgTeoricos"
                        MaxLength="50" ReadOnly="true" meta:resourcekey="lblKgTeoricos" />
                </div>
                <div class="separador">
                    <mimo:LabeledTextBox ID="txtPulgadas" runat="server" Enabled="false" EntityPropertyName="Peqs"
                        MaxLength="50" ReadOnly="true" meta:resourcekey="lblPulgadas">
                    </mimo:LabeledTextBox>
                </div>
            </div>
            <p>
            </p>
            <div class="separador">
                <asp:Button runat="server" ID="btnEditar" CssClass="boton Izquierdo" OnClick="btnAgregar_Click"
                    ValidationGroup="vgJunta" Visible="false" meta:resourcekey="btnEditar" />
                <asp:Button runat="server" ID="btnAgregar" CssClass="boton Izquierdo" OnClick="btnAgregar_Click"
                    ValidationGroup="vgJunta" meta:resourcekey="btnAgregar" />
                <asp:Label runat="server" ID="lblAdvertencia" meta:resourcekey="lblAdvertencia" CssClass="boldUnderline" Visible="false"/>
            </div>
        </div>
        <div class="divDerecho ancho35">
            <div class="validacionesRecuadro" style="margin-top: 23px;">
                <div class="validacionesHeader">
                </div>
                <div class="validacionesMain">
                    <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" ValidationGroup="vgJunta"
                        CssClass="summary" meta:resourcekey="valSummary" />
                </div>
            </div>
        </div>
        <p>
        </p>
        <br />
        <br />
        <asp:HiddenField runat="server" ID="hdnJuntaID" />
    </asp:Panel>
</div>
