<%@ Page Language="C#" MasterPageFile="~/Masters/Administracion.Master" AutoEventWireup="true"
    CodeBehind="LstEstimado.aspx.cs" Inherits="SAM.Web.Administracion.LstEstimado" %>

<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:RadAjaxManager runat="server" Style="display: none;" ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdEstimacion">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdEstimacion" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblListaEstimacion" CssClass="Titulo" meta:resourcekey="lblListaEstimacion"
            Text="ESTIMACIÓNES"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" Text="Proyecto:" meta:resourcekey="lblProyecto"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedItemChanged" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvProyecto" runat="server" ControlToValidate="ddlProyecto"
                        InitialValue="" meta:resourcekey="valProyecto" Display="None" CssClass="bold" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblDesde" runat="server" Text="Desde:" meta:resourcekey="lblDesde"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpDesde" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblHasta" runat="server" Text="Hasta:" meta:resourcekey="lblHasta"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpHasta" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblNumeroEstimacion" runat="server" Text="Numero de Estimación:" meta:resourcekey="lblNumeroEstimacion"
                        CssClass="bold" />
                    <br />
                    <asp:TextBox ID="txtNumeroEstimaciones" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" meta:resourcekey="btnMostrar"
                        CssClass="boton" OnClick="btnMostrar_OnClick" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <div>
            <uc1:Header ID="proyHeader" runat="server" Visible="false" />
        </div>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList"
            meta:resourcekey="valSummary" />
        <p>
        </p>
        <asp:PlaceHolder runat="server" ID="phGrid" Visible="false">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid ID="grdEstimacion" runat="server" OnNeedDataSource="grdEstimacion_OnNeedDataSource"
                OnItemCommand="grdEstimado_ItemCommand">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:LinkButton runat="server" ID="lnkActualizar" CausesValidation="false" meta:resourcekey="lnkActualizar"
                                CssClass="link" OnClick="lnkActualizar_onClick" />
                            <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png"
                                CausesValidation="false" AlternateText="Actualizar" CssClass="imgEncabezado"
                                OnClick="lnkActualizar_onClick" meta:resourcekey="imgActualizar" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="btnDetalle_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <samweb:AuthenticatedHyperLink runat="server" ID="hlDetalle" meta:resourcekey="hlDetalle"
                                    ImageUrl="~/Imagenes/Iconos/info.png" NavigateUrl='<%#Eval("EstimadoID","~/Administracion/DetEstimacion.aspx?ID={0}") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" Text="Borrar" ID="btnBorrar" runat="server"
                                    meta:resourcekey="btnBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("EstimadoID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="HdNumeroDeEstimacion" DataField="NumeroEstimacion"
                            HeaderStyle-Width="180" FilterControlWidth="100"
                            Groupable="false" meta:resourcekey="HdNumeroDeEstimacion" />
                        <telerik:GridDateTimeColumn UniqueName="HdFechaDeEstimacion" DataField="FechaEstimacion"
                            HeaderText="Fecha Estimación" DataFormatString="{0:d}" HeaderStyle-Width="180"
                            FilterControlWidth="100" Groupable="false" meta:resourcekey="HdFechaDeEstimacion" />
                        <telerik:GridBoundColumn UniqueName="HdNumeroJunta" DataField="NumeroJunta" HeaderText="Numero Junta" DataFormatString="{0:N0}"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="HdNumeroJunta" />
                        <telerik:GridBoundColumn UniqueName="HdNumeroSpools" DataField="NumeroSpools" HeaderText="Numero Spools" DataFormatString="{0:N0}"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="HdNumeroSpools" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                            Reorderable="false" ShowSortIcon="false">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </mimo:MimossRadGrid>
        </asp:PlaceHolder>
    </div>
</asp:Content>
