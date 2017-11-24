<%@ Page Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true"
    CodeBehind="DetOdt.aspx.cs" Inherits="SAM.Web.Produccion.DetOdt" %>

<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="cntHead" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="radMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="valSpools" />
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <sam:BarraTituloPagina ID="titulo" runat="server" NavigateUrl="~/Produccion/LstOrdenTrabajo.aspx"
        meta:resourcekey="lblTitulo" />
    <div class="contenedorCentral">
        <sam:Header ID="proyEncabezado" runat="server" />
        <asp:ValidationSummary runat="server" ID="ValidationSummary1" ValidationGroup="vgOdt"
            CssClass="summaryList" meta:resourcekey="valOdt" />


        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblOdtTexto" ID="lblOdtTexto" runat="server" AssociatedControlID="lblOdt" />
                    <asp:Label ID="lblOdt" runat="server" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblFecha" ID="lblFecha" runat="server" AssociatedControlID="dtpFecha" />
                    <mimo:MappableDatePicker runat="server" ID="dtpFecha" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ValidationGroup="vgOdt" runat="server" ID="reqFecha"
                        meta:resourcekey="reqFecha" ErrorMessage="La fecha es requerida" ControlToValidate="dtpFecha"
                        Display="None" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblTaller" ID="lblTaller" runat="server" AssociatedControlID="ddlTaller" />
                    <mimo:MappableDropDown runat="server" ID="ddlTaller" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ValidationGroup="vgOdt" runat="server" ID="reqTaller"
                        meta:resourcekey="reqTaller" ErrorMessage="El taller es requerido" ControlToValidate="ddlTaller"
                        Display="None" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblEstatus" ID="lblEstatus" runat="server" AssociatedControlID="ddlEstatus" />
                    <mimo:MappableDropDown runat="server" ID="ddlEstatus">
                        <asp:ListItem Text="" Value="" />
                        <asp:ListItem meta:resourcekey="lstActiva" Text="Activa" Value="1" />
                        <asp:ListItem meta:resourcekey="lstCancelada" Text="Cancelada" Value="2" />
                    </mimo:MappableDropDown>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ValidationGroup="vgOdt" runat="server" ID="reqEstatus"
                        meta:resourcekey="reqEstatus" ErrorMessage="El estatus es requerido" ControlToValidate="ddlEstatus"
                        Display="None" />
                </div>
            </div>
            <div class="divIzquierdo">
                <asp:Button CssClass="boton" ValidationGroup="vgOdt" meta:resourcekey="btnGuardar"
                    runat="server" ID="btnGuardar" Text="Guardar" OnClick="btnGuardar_Click" />
            </div>
            <p>
            </p>
        </div>
        <div class="clear">
            <p>
            </p>
            <asp:ValidationSummary runat="server" ID="valSpools" ValidationGroup="vgSpools" CssClass="summaryList"
                meta:resourcekey="valSpools" />
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdSpools" OnUpdateCommand="grdSpools_OnUpdateCommand" OnNeedDataSource="grdSpools_OnNeedDataSource" OnItemCommand="grdSpools_ItemCommand" OnItemDataBound="grdSpools_ItemDataBound" OnItemCreated="grdSpools_ItemCreated" OnDetailTableDataBind="grdSpools_OnDetailTableDataBind">
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="SpoolID" HierarchyDefaultExpanded="false" HierarchyLoadMode="ServerBind">
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                        <asp:LinkButton meta:resourcekey="lnkNuevaVersion" runat="server" ID="lnkNuevaVersion"
                                CssClass="link" OnClick="lnkNuevaVersion_Click" CausesValidation="false" />
                            <asp:ImageButton meta:resourcekey="imgNuevaVersion" runat="server" ID="imgNuevaVersion"
                                ImageUrl="~/Imagenes/Iconos/icono_generarequisicion.png" CssClass="imgEncabezado" OnClick="lnkNuevaVersion_Click"
                                CausesValidation="false" />
                            <samweb:AuthenticatedHyperLink meta:resourcekey="lnkAgregar" runat="server" ID="lnkAgregar"
                                CssClass="link" />
                            <samweb:AuthenticatedHyperLink meta:resourcekey="imgAgregar" runat="server" ID="imgAgregar"
                                ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado" />
                            <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar"
                                CssClass="link" OnClick="lnkActualizar_Click" CausesValidation="false" />
                            <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar"
                                ImageUrl="~/Imagenes/Iconos/actualizar.png" CssClass="imgEncabezado" OnClick="lnkActualizar_Click"
                                CausesValidation="false" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="borrar_h" AllowFiltering="false" Groupable="false"
                            HeaderStyle-Width="30">
                            <ItemTemplate>
                                <asp:ImageButton meta:resourcekey="imgBorrar" ID="imgBorrar" runat="server" CommandName="borrar"
                                    ImageUrl="~/Imagenes/Iconos/borrar.png" CausesValidation="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="DifiereDeIngenieria" HeaderStyle-Width="50"
                            DataField="DifiereOReingenieria" AllowFiltering="true" DataType="System.Boolean">
                            <ItemTemplate>
                                <asp:ImageButton meta:resourcekey="imgAdvertencia" runat="server" ID="imgAdvertencia"
                                    Visible="false" ImageUrl="~/Imagenes/Iconos/ico_reingenieria.png" CommandName="reing"
                                    CausesValidation="false" />
                                <asp:Image meta:resourcekey="imgFueReingenieria" runat="server" ID="imgFueReingenieria"
                                    Visible="false" ImageUrl="~/Imagenes/Iconos/ico_reingenieria_verde.png" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="partida" meta:resourcekey="grdPartidaCol" HeaderStyle-Width="100"
                            FilterControlWidth="60" DataField="Partida" />
                        <telerik:GridBoundColumn UniqueName="numControl" meta:resourcekey="grdNumControlCol"
                            HeaderStyle-Width="100" FilterControlWidth="60" DataField="NumeroControl" />
                        <telerik:GridTemplateColumn meta:resourcekey="grdSpoolCol" AllowFiltering="true"
                            DataField="NombreSpool" HeaderStyle-Width="180" FilterControlWidth="120" UniqueName="nombreSpool">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlSpool" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="pdis" meta:resourcekey="grdPdisCol" HeaderStyle-Width="100"
                            FilterControlWidth="60" DataField="Pdis" DataFormatString="{0:#0.000}" />
                        <telerik:GridBoundColumn UniqueName="estatus" meta:resourcekey="grdEstatusDespacho"
                            HeaderStyle-Width="172" FilterControlWidth="112" DataField="EstatusDespachoTexto" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                            Reorderable="false" ShowSortIcon="false" HeaderStyle-Width="100%">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <DetailTables>
                        <telerik:GridTableView Name="grdBastones" AllowAutomaticUpdates="true" EditMode="InPlace" AllowFilteringByColumn="false" AllowSorting="false" AllowPaging="false" EnableHeaderContextFilterMenu="false" EnableHeaderContextMenu="false" AutoGenerateColumns="false" Width="730">
                            <Columns>
                                <telerik:GridEditCommandColumn UniqueName="EditColumn" meta:resourcekey="grdEditCol" HeaderStyle-Width="78" ButtonType="ImageButton" EditImageUrl="../Imagenes/Iconos/editar.png" UpdateImageUrl="../Imagenes/Iconos/ico_aceptar.png" CancelImageUrl="../Imagenes/Iconos/ico_confinar.png" />
                                <telerik:GridBoundColumn ReadOnly="true" UniqueName="LetraBaston" DataField="LetraBaston" HeaderStyle-Width="380" Groupable="false" meta:resourcekey="htLetraBaston" />
                                <telerik:GridBoundColumn ReadOnly="true" UniqueName="PDI" DataField="PDI" DataFormatString="{0:#0.000}" HeaderStyle-Width="100" Groupable="false" meta:resourcekey="htPDI" />
                                <telerik:GridTemplateColumn UniqueName="EstacionEdit" HeaderText="Estacion" HeaderStyle-Width="173" Groupable="false" meta:resourcekey="gtcEstacion">
                                    <ItemTemplate>
                                        <asp:Literal runat="server" ID="litEstacion"  />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="EstacionCombo" runat="server" Height="100px" Width="150px"></telerik:RadComboBox>
                                        <asp:HiddenField runat="server" ID="hfNombreBaston" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </telerik:GridTableView>
                    </DetailTables>
                </MasterTableView>
            </mimo:MimossRadGrid>
            <div id="btnWrapper" class="oculto">
                <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="false"
                    OnClick="lnkActualizar_Click" />
            </div>
        </div>
    </div>
</asp:Content>
