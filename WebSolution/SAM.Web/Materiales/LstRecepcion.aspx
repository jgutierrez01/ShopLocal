<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true"
    CodeBehind="LstRecepcion.aspx.cs" Inherits="SAM.Web.Materiales.LstRecepcion" %>
<%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlPatio">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="ddlProyecto" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdRecepcion">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdRecepcion" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblListaRecepcion" CssClass="Titulo" meta:resourcekey="lblListaRecepcion"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblPatio" runat="server" Text="Patio:" meta:resourcekey="lblPatio"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlPatio" EntityPropertyName="PatioID"
                        OnSelectedIndexChanged="DdlPatioOnSelectedIndexChanged" AutoPostBack="true" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvPatio" runat="server" ControlToValidate="ddlPatio"
                        InitialValue="" meta:resourcekey="valPatio" Display="None" CssClass="bold" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" Text="Proyecto:" meta:resourcekey="lblProyecto"
                        CssClass="bold" />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID"
                        OnSelectedIndexChanged="ddlProyectoSelectedItemChanged" AutoPostBack="true" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblDesde" runat="server" Text="Fecha de Recepción Inicial:" meta:resourcekey="lblDesde"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpDesde" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblHasta" runat="server" Text="Fecha de Recepción Final:" meta:resourcekey="lblHasta"
                        CssClass="bold" />
                    <br />
                    <mimo:MappableDatePicker ID="dtpHasta" runat="server" MinDate="01/01/1960" MaxDate="01/01/2050" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" meta:resourcekey="btnMostrar"
                        OnClick="btnMOstrar_Click" CssClass="boton" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <div>
            <proy:Encabezado ID="proyEncabezado" runat="server" Visible="false" />
        </div>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" />
        <p>
        </p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
        <mimo:MimossRadGrid runat="server" ID="grdRecepcion" OnNeedDataSource="grdRecepcion_OnNeedDataSource"
            OnItemDataBound="grdRecepcion_ItemDataBound" OnItemCommand="grdRecepcion_ItemCommand" Visible="false">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                    <samweb:AuthenticatedHyperLink ID="hypAgregar" runat="server" meta:resourcekey="lnkAgregar" CssClass="link" NavigateUrl="~/Materiales/NuevaRecepcion.aspx" />
                    <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" Text="Agregar" CssClass="imgEncabezado" NavigateUrl="~/Materiales/NuevaRecepcion.aspx" />                        
                        <asp:LinkButton runat="server" ID="lnkActualizar" OnClick="lnkActualizar_onClick" CausesValidation="false"
                            meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" CausesValidation="false"
                            AlternateText="Actualizar" CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <samweb:AuthenticatedHyperLink ImageUrl="~/Imagenes/Iconos/editar.png" runat="server" ID="hypEditar" meta:resourcekey="imgEditar" NavigateUrl="~/Materiales/DetRecepcion.aspx" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="borrar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("RecepcionID") %>'
                                OnClientClick="return Sam.Confirma(1);">
                                <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" /></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="hdProyecto" DataField="Proyecto" meta:resourcekey="hdProyecto"
                        FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="hdTransportista" DataField="Transportista" meta:resourcekey="hdTransportista"
                        FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="hdFecha" DataField="FechaRecepcion" meta:resourcekey="hdFecha"
                        DataFormatString="{0:d}" FilterControlWidth="150" HeaderStyle-Width="200" />
                    <telerik:GridBoundColumn UniqueName="hdNoUnicos" DataField="CantidadNumerosUnicos" DataFormatString="{0:N0}"
                        meta:resourcekey="hdNoUnicos" FilterControlWidth="150" HeaderStyle-Width="200" />
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
</asp:Content>
