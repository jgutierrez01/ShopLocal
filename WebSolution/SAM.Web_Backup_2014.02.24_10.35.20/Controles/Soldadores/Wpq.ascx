<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Wpq.ascx.cs" Inherits="SAM.Web.Controles.Soldadores.Wpq" %>
<div>
    <asp:PlaceHolder runat="server" ID="phWpq">
        <div class="clear">
            <div class="dashboardCentral">
                <div class="divIzquierdo ancho70">
                    <div class="clear">
                        <asp:Label runat="server" ID="lblDatos"></asp:Label>
                    </div>
                    <div>
                        <div class="separador">
                            <asp:Label ID="lblWps" runat="server" CssClass="bold" meta:resourcekey="lblWps" />
                            <br />
                            <mimo:MappableDropDown runat="server" ID="ddlWps" EntityPropertyName="WpsID" />
                            <span class="required">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="rfvWps" ControlToValidate="ddlWps"
                                InitialValue="" Display="None" ValidationGroup="vgWpq" meta:resourcekey="rfvWps" />
                        </div>
                        <div class="separador">
                            <asp:Label runat="server" ID="lblFechaInicial" CssClass="bold" meta:resourcekey="lblFechaInicial" />
                            <br />
                            <mimo:MappableDatePicker runat="server" ID="dtpFechaInicial" MinDate="01/01/1960"
                                MaxDate="01/01/2050" EnableEmbeddedSkins="false" />
                            <span class="required">*</span>
                            <asp:RequiredFieldValidator runat="server" ID="rfvFechaInicio" ControlToValidate="dtpFechaInicial"
                                Display="None" ValidationGroup="vgWpq" meta:resourcekey="rfvFechaInicio" />
                        </div>
                        <div class="separador">
                            <asp:Label runat="server" ID="lblFechaVencimiento" CssClass="bold" meta:resourcekey="lblFechaVencimiento" />
                            <br />
                            <mimo:MappableDatePicker runat="server" ID="dtpFechaVencimiento" MinDate="01/01/1960"
                                MaxDate="01/01/2050" EnableEmbeddedSkins="false" />
                            <span class="required">*</span>
                            <asp:RequiredFieldValidator ID="rfvFechaVencimiento" runat="server" ControlToValidate="dtpFechaVencimiento"
                                Display="None" ValidationGroup="vgWpq" meta:resourcekey="rfvFechaVencimiento" />
                        </div>
                    </div>
                    <p>
                        &nbsp;</p>
                    <div>
                        <div class="separador">
                            <asp:HiddenField runat="server" ID="hdnWpqID" />
                            <asp:Button runat="server" ID="btnAgregar" CssClass="boton" OnClick="btnAgregar_Click"
                                ValidationGroup="vgWpq" meta:resourcekey="btnAgregar" />
                            <asp:Button runat="server" ID="btnActualizar" CssClass="boton" OnClick="btnAgregar_Click"
                                ValidationGroup="vgWpq" Visible="false" meta:resourcekey="btnActualizar" />
                        </div>
                    </div>
                </div>
                <div class="divDerecho ancho30">
                    <div class="validacionesRecuadro" style="margin-top: 23px;">
                        <div class="validacionesHeader">
                        </div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valSummary" EnableClienteScript="true"
                                DisplayMode="BulletList" CssClass="summary" ValidationGroup="vgWpq" meta:resourcekey="valSummary" />
                            <asp:ValidationSummary runat="server" ID="valSummaryGenerico" EnableClienteScript="true"
                                DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phWpqs">
        <div class="clear">
            <mimo:MimossRadGrid ID="grdWps" runat="server" OnNeedDataSource="grdWps_OnNeedDataSource"
                OnItemCommand="grdWps_ItemCommand" OnItemDataBound="grdWps_ItemDataBound" AllowFilteringByColumn="false"
                AllowPaging="false" Height="250">
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
                                    CommandArgument='<%#Eval("WpqID") %>'>
                                    <asp:Image ImageUrl="~/Imagenes/Iconos/editar.png" ID="imgEditar" runat="server"
                                        meta:resourcekey="imgEditar" /></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false"
                                    CommandArgument='<%#Eval("WpqID") %>' OnClientClick="return Sam.Confirma(1);">
                                    <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                        meta:resourcekey="imgBorrar" /></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="WpsID" DataField="WpsID" Visible="false" />
                        <telerik:GridBoundColumn UniqueName="Wps" DataField="Wps.Nombre" HeaderStyle-Width="120"
                            Groupable="false" meta:resourcekey="gbcNombre" />
                        <telerik:GridBoundColumn UniqueName="FechaInicio" DataField="FechaInicio" HeaderStyle-Width="150"
                            Groupable="false" DataFormatString="{0:d}" meta:resourcekey="gbcFechaInicio" />
                        <telerik:GridBoundColumn UniqueName="FechaVigencia" DataField="FechaVigencia" HeaderStyle-Width="150"
                            Groupable="false" DataFormatString="{0:d}" meta:resourcekey="gbcFechaVigencia" />
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
</div>
