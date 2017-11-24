<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Localizaciones.ascx.cs"
    Inherits="SAM.Web.Controles.Patios.Localizaciones" %>
<div>
    <asp:PlaceHolder runat="server" ID="phUbicacion">
        <div class="clear">
            <div class="divIzquierdo ancho70">
                <div class="separador">
                    <div class="divIzquierdo ancho50">
                        <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre"
                            MaxLength="50" ValidationGroup="vgLocalizacion" meta:resourcekey="lblNombre" />
                    </div>
                    <div class="divIzquierdo ancho40" style="padding-top: 15px;">
                        <asp:CheckBox runat="server" ID="chkAreacorte" meta:resourcekey="lblAreaCorte" CssClass="checkYTexto" />
                    </div>
                </div>
                <div class="separador">
                    <asp:HiddenField runat="server" ID="hdnUbicacionID" />
                    <asp:Button runat="server" ID="btnAgregar" CssClass="boton" OnClick="btnAgregar_Click"
                        ValidationGroup="vgLocalizacion" meta:resourcekey="btnAgregar" />
                    <asp:Button runat="server" ID="btnActualizar" CssClass="boton" OnClick="btnAgregar_Click"
                        ValidationGroup="vgLocalizacion" Visible="false" meta:resourcekey="btnActualizar" />
                </div>
                <asp:PlaceHolder runat="server" ID="phUbicaciones">
                    <div class="clear">
                        <mimo:MimossRadGrid ID="grdUbicaciones" runat="server" OnNeedDataSource="grdUbicaciones_OnNeedDataSource"
                            OnItemCommand="grdUbicaciones_ItemCommand" AllowFilteringByColumn="false" AllowPaging="false"
                            Height="200px" Width="500px">
                            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false">
                                <CommandItemTemplate>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="btnEditar_h" AllowFiltering="false" HeaderStyle-Width="30"
                                        Groupable="false">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkEditar" CommandName="Editar" CausesValidation="false"
                                                CommandArgument='<%#Eval("UbicacionFisicaID") %>'>
                                                <asp:Image ImageUrl="~/Imagenes/Iconos/editar.png" ID="imgEditar" runat="server"
                                                    meta:resourcekey="imgEditar" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                                        Groupable="false">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false"
                                                CommandArgument='<%#Eval("UbicacionFisicaID") %>' OnClientClick="return Sam.Confirma(1);">
                                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                    meta:resourcekey="imgBorrar" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="UbicacionFisicaID" DataField="UbicacionFisicaID"
                                        Visible="false" />
                                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderStyle-Width="120"
                                        Groupable="false" meta:resourcekey="gbcNombre" />
                                    <telerik:GridCheckBoxColumn UniqueName="EsAreaCorte" DataField="EsAreaCorte" HeaderStyle-Width="90"
                                        ItemStyle-HorizontalAlign="Center" Groupable="false" meta:resourcekey="gbcAreaCorte" />
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
            <div class="divIzquierdo ancho30">
                <div class="validacionesRecuadro" style="margin-top: 23px;">
                    <div class="validacionesHeader">
                    </div>
                    <div class="validacionesMain">
                        <asp:ValidationSummary runat="server" ID="valSummary" EnableClienteScript="true"
                            DisplayMode="BulletList" CssClass="summary" ValidationGroup="vgLocalizacion"
                            meta:resourcekey="valSummary" />
                        <asp:ValidationSummary runat="server" ID="valSummaryGenerico" EnableClienteScript="true"
                            DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</div>
