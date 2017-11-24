<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Maquina.ascx.cs" Inherits="SAM.Web.Controles.Patios.Maquina" %>
<div>
    <asp:PlaceHolder runat="server" ID="phMaquina">
        <div class="clear">
            <div class="divIzquierdo ancho70">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre"
                        MaxLength="50" ValidationGroup="vgMaquina" meta:resourcekey="lblNombre" />
                </div>
                <div class="separador">
                    <asp:HiddenField runat="server" ID="hdnMaquinaID" />
                    <asp:Button runat="server" ID="btnAgregar" CssClass="boton" OnClick="btnAgregar_Click"
                        ValidationGroup="vgMaquina" meta:resourcekey="btnAgregar" />
                    <asp:Button runat="server" ID="btnActualizar" CssClass="boton" OnClick="btnAgregar_Click"
                        ValidationGroup="vgMaquina" Visible="false" meta:resourcekey="btnActualizar" />
                </div>
                <asp:PlaceHolder runat="server" ID="phMaquinas">
                    <div class="clear">
                        <mimo:MimossRadGrid ID="grdMaquinas" runat="server" OnNeedDataSource="grdMaquinas_OnNeedDataSource"
                            OnItemCommand="grdMaquinas_ItemCommand" AllowFilteringByColumn="false" AllowPaging="false"
                            Height="200px" Width="500px">
                            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false">
                                <CommandItemTemplate>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="btnEditar_h" AllowFiltering="false" HeaderStyle-Width="30"
                                        Groupable="false">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkEditar" CommandName="Editar" CausesValidation="false"
                                                CommandArgument='<%#Eval("MaquinaID") %>'>
                                                <asp:Image ImageUrl="~/Imagenes/Iconos/editar.png" ID="imgEditar" runat="server"
                                                    meta:resourcekey="imgEditar" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                                        Groupable="false">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false"
                                                CommandArgument='<%#Eval("MaquinaID") %>' OnClientClick="return Sam.Confirma(1);">
                                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                    meta:resourcekey="imgBorrar" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderStyle-Width="120"
                                        Groupable="false" meta:resourcekey="gbcNombre" />
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
                            DisplayMode="BulletList" CssClass="summary" ValidationGroup="vgMaquina" meta:resourcekey="valSummary" />
                        <asp:ValidationSummary runat="server" ID="valSummaryGenerico" EnableClienteScript="true"
                            DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</div>
