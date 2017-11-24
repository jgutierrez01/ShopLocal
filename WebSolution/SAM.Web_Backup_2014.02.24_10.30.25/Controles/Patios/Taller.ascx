<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Taller.ascx.cs" Inherits="SAM.Web.Controles.Patios.Taller" %>
<div>
    <asp:PlaceHolder runat="server" ID="phTaller">
        <div class="clear">
            <div class="divIzquierdo ancho70">
                <div class="separador">
                    <mimo:RequiredLabeledTextBox runat="server" ID="txtNombre" EntityPropertyName="Nombre"
                        MaxLength="50" ValidationGroup="vgTaller" meta:resourcekey="lblNombre" />
                </div>
                <div class="separador">
                    <asp:HiddenField runat="server" ID="hdnTallerID" />
                    <asp:Button runat="server" ID="btnAgregar" CssClass="boton" OnClick="btnAgregar_Click"
                        ValidationGroup="vgTaller" meta:resourcekey="btnAgregar" />
                    <asp:Button runat="server" ID="btnActualizar" CssClass="boton" OnClick="btnAgregar_Click"
                        ValidationGroup="vgTaller" Visible="false" meta:resourcekey="btnActualizar" />
                </div>
                <asp:PlaceHolder runat="server" ID="phTalleres">
                    <div class="clear">
                        <mimo:MimossRadGrid ID="grdTalleres" runat="server" OnNeedDataSource="grdTalleres_OnNeedDataSource"
                            OnItemCommand="grdTalleres_ItemCommand" AllowFilteringByColumn="false" AllowPaging="false"
                            Height="200px" Width="500px">
                            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false">
                                <CommandItemTemplate>
                                </CommandItemTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="btnEditar_h" AllowFiltering="false" HeaderStyle-Width="30"
                                        Groupable="false">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkEditar" CommandName="Editar" CausesValidation="false"
                                                CommandArgument='<%#Eval("TallerID") %>'>
                                                <asp:Image ImageUrl="~/Imagenes/Iconos/editar.png" ID="imgEditar" runat="server"
                                                    meta:resourcekey="imgEditar" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                                        Groupable="false">
                                        <ItemTemplate>
                                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CausesValidation="false"
                                                CommandArgument='<%#Eval("TallerID") %>' OnClientClick="return Sam.Confirma(1);">
                                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                                    meta:resourcekey="imgBorrar" />
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderStyle-Width="120"
                                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" />
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
                            DisplayMode="BulletList" CssClass="summary" ValidationGroup="vgTaller" meta:resourcekey="valSummary" />
                        <asp:ValidationSummary runat="server" ID="valSummaryGenerico" EnableClienteScript="true"
                            DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valSummary" />
                    </div>
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</div>
