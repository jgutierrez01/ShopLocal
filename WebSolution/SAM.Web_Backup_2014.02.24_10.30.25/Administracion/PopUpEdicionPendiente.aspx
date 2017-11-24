<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpEdicionPendiente.aspx.cs" Inherits="SAM.Web.Administracion.PopUpEdicionPendiente" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="userAgentDependant" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></h4>
    <asp:HiddenField runat="server" ID="hdnProyectoID" />
    <div class="cajaAzul">
        <div class="divIzquierdo ancho20">
            <asp:Label runat="server" ID="lblProyecto" CssClass="bold" meta:resourcekey="lblProyecto" />
            <br />
            <asp:Label runat="server" ID="lblFechaApertura" CssClass="bold" meta:resourcekey="lblFechaApertura" />
            <br />
            <asp:Label runat="server" ID="lblTitulo" CssClass="bold" meta:resourcekey="lblTituloPendiente" />
            <br />
            <asp:Label runat="server" ID="lblDescripcion" CssClass="bold" meta:resourcekey="lblDescripcion" />
        </div>
        <div class="divIzquierdo">
            <asp:Label runat="server" ID="lblProyectoData" />
            <br />
            <asp:Label runat="server" ID="lblFechaAperturaData" />
            <br />
            <asp:Label runat="server" ID="lblTituloData" />
            <br />
            <asp:Label runat="server" ID="lblDescripcionData" />
        </div>
        <p></p>
    </div>
    <p>
    </p>
    <div class="divIzquierdo ancho60">
        <div class="separador">
            <asp:Label runat="server" ID="lblArea" CssClass="bold" meta:resourcekey="lblArea" />
            <br />
            <asp:DropDownList runat="server" ID="ddlArea" />
            <span class="required">*</span>
            <asp:RequiredFieldValidator runat="server" ID="rfvArea" ControlToValidate="ddlArea"
                InitialValue="" Display="None" ValidationGroup="vgEdicionPendiente" meta:resourcekey="rfvArea" />
        </div>
        <div class="separador">
            <asp:Label runat="server" ID="lblResponsable" AssociatedControlID="rcbResponsable"
                meta:resourcekey="lblResponsable" />
            <telerik:RadComboBox runat="server" ID="rcbResponsable" Width="200px" Height="150px"
                OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" EnableLoadOnDemand="true"
                ShowMoreResultsBox="true" EnableVirtualScrolling="true" CssClass="required" AllowCustomText="false"
                IsCaseSensitive="false">
                <WebServiceSettings Method="ListaEmpleadosPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                <HeaderTemplate>
                    <table cellpadding="0" cellspacing="0" class="headerRcbGenerico">
                        <tr>
                            <th>
                                <asp:Literal ID="litNombre" runat="server" meta:resourcekey="litNombre"></asp:Literal>
                            </th>
                        </tr>
                    </table>
                </HeaderTemplate>
            </telerik:RadComboBox>
            <span class="required">*</span>
            <asp:CustomValidator runat="server" ID="cvResponsable" ControlToValidate="rcbResponsable"
                ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValorCustom"
                ValidateEmptyText="true" Display="None" ValidationGroup="vgEdicionPendiente"
                meta:resourcekey="cvResponsable" />
        </div>
        <div class="separador">
            <asp:Label runat="server" ID="lblEstatus" CssClass="bold" meta:resourcekey="lblEstatus"></asp:Label>
            <br />
            <asp:DropDownList runat="server" ID="ddlEstatus">
                <asp:ListItem Value="" Text="" Selected="True" />
                <asp:ListItem Value="A" Text="Abierto" meta:resourcekey="liAbierto" />
                <asp:ListItem Value="R" Text="Resuelto" meta:resourcekey="liResuelto" />
                <asp:ListItem Value="C" Text="Cerrado" meta:resourcekey="liCerrado" />
            </asp:DropDownList>
            <span class="required">*</span>
            <asp:RequiredFieldValidator runat="server" ID="rfvEstatus" ControlToValidate="ddlEstatus"
                InitialValue="" Display="None" ValidationGroup="vgEdicionPendiente" meta:resourcekey="rfvEstatus" />
        </div>
        <div class="separador">
            <mimo:RequiredLabeledTextBox runat="server" ID="txtObservaciones" MaxLength="250"
                ValidationGroup="vgEdicionPendiente" TextMode="MultiLine" meta:resourcekey="lblObservaciones" />
        </div>
        <div class="separador" style="padding-top: 15px">
            <asp:Button runat="server" CssClass="boton" ID="btnGuardar" OnClick="btnGuardar_Click"
                ValidationGroup="vgEdicionPendiente" meta:resourcekey="btnGuardar" />
        </div>
    </div>
    <div class="divDerecho ancho40">
        <div class="validacionesRecuadro">
            <div class="validacionesHeader">
            </div>
            <div class="validacionesMain">
                <asp:ValidationSummary ID="valPendiente" runat="server" ValidationGroup="vgEdicionPendiente"
                    meta:resourcekey="valPendiente" CssClass="summary" />
            </div>
        </div>
    </div>
    <p>
    </p>
    <div style="padding-top: 20px">
        <mimo:MimossRadGrid runat="server" ID="grdHistorial" OnNeedDataSource="grdHistorial_OnNeedDataSource"
            Width="100%" Height="250px">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label runat="server" ID="lblTituloGrid" meta:resourcekey="lblTituloGrid" />
                        </div>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridBoundColumn UniqueName="PendienteDetalleID" DataField="PendienteDetalleID"
                        Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Fecha" DataField="Fecha" FilterControlWidth="70"
                        HeaderStyle-Width="110" DataFormatString="{0:d}" meta:resourcekey="gbcFecha" />
                    <telerik:GridBoundColumn UniqueName="Accion" DataField="Accion" FilterControlWidth="70"
                        HeaderStyle-Width="110" meta:resourcekey="gbcAccion" />
                    <telerik:GridBoundColumn UniqueName="Area" DataField="Area" FilterControlWidth="90"
                        HeaderStyle-Width="130" meta:resourcekey="gbcArea" />
                    <telerik:GridBoundColumn UniqueName="Responsable" DataField="Responsable" FilterControlWidth="150"
                        HeaderStyle-Width="200" meta:resourcekey="gbcResponsable" />
                    <telerik:GridBoundColumn UniqueName="DescripcionEstatus" DataField="DescripcionEstatus"
                        FilterControlWidth="90" HeaderStyle-Width="130" meta:resourcekey="gbcEstatus" />
                    <telerik:GridBoundColumn UniqueName="Autor" DataField="Autor" FilterControlWidth="150"
                        HeaderStyle-Width="200" meta:resourcekey="gbcAutor" />
                    <telerik:GridBoundColumn UniqueName="Observaciones" DataField="Observaciones" FilterControlWidth="150"
                        HeaderStyle-Width="200" meta:resourcekey="gbcObservaciones" />
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
