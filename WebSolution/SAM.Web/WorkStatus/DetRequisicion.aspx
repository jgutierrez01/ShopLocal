<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="DetRequisicion.aspx.cs" Inherits="SAM.Web.WorkStatus.DetRequisicion" %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>   
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdJuntas">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdJuntas" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="phDatos" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
     <sam:BarraTituloPagina runat="server" ID="lblHeader" meta:resourcekey="lblHeader"
        NavigateUrl="/WorkStatus/RepRequisiciones.aspx" /> 

    <div class="contenedorCentral">
    <uc1:Header ID="proyHeader" runat="server" Visible="true" />
        <asp:PlaceHolder runat="server" ID="phDatos">
            <div class="cajaAzul">
                <div class="divIzquierdo ancho30">
                        <asp:Label runat="server" ID="lblNumeroRequisicion" CssClass="bold" meta:resourcekey="lblNumeroRequisicion" />
                        <asp:Label runat="server" ID="lblNumeroRequisicionData" />
                   <p></p>
                        <asp:Label runat="server" ID="lblFechaRequisicion" CssClass="bold" meta:resourcekey="lblFechaRequisicion" />
                        <asp:Label runat="server" ID="lblFechaRequisicionData" />
                   <p></p>
                        <asp:Label runat="server" ID="lblObservaciones" CssClass="bold" meta:resourcekey="lblObservaciones" />
                        <asp:Label runat="server" ID="lblObservacionesData" />
                   <p></p>
                </div>
                <div class="divIzquierdo">
                    
                        <asp:Label runat="server" ID="lblTipoPrueba" CssClass="bold" meta:resourcekey="lblTipoPrueba" />
                        <asp:Label runat="server" ID="lblTipoPruebaData" />
                    <p></p>
                        <asp:Label runat="server" ID="lblCodigo" CssClass="bold" meta:resourcekey="lblCodigo" />
                        <asp:Label runat="server" ID="lblCodigoData" />
                   <p></p>
                        <asp:Label runat="server" ID="lblTotalJuntas" CssClass="bold" meta:resourcekey="lblTotalJuntas" />
                        <asp:Label runat="server" ID="lblTotalJuntasData" />
                    <p></p>
                </div>
                <p>
                </p>
            </div>
        </asp:PlaceHolder>
        <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" HeaderText="Errores" />
         <p> </p>
        <asp:PlaceHolder runat="server" ID="phGrd">
            <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
            <mimo:MimossRadGrid runat="server" ID="grdJuntas" OnNeedDataSource="grdJuntas_OnNeedDataSource"
                OnItemCommand="grdJuntas_OnItemCommand" AllowMultiRowSelection="true" OnItemDataBound="grdJuntas_ItemDataBound">
                <ClientSettings>
                    <Selecting AllowRowSelect="true" />
                </ClientSettings>
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                    <CommandItemTemplate>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" Text="Borrar" ID="btnBorrar" runat="server"
                                    meta:resourcekey="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("JuntaRequisicionID") %>'
                                    OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn UniqueName="OrdenTrabajo" DataField="OrdenTrabajo" meta:resourcekey="gbcOrdenTrabajo"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" meta:resourcekey="gbcNumeroControl"
                            FilterControlWidth="80" HeaderStyle-Width="120" />
                        <telerik:GridBoundColumn UniqueName="Spool" DataField="Spool" meta:resourcekey="gbcSpool"
                            FilterControlWidth="100" HeaderStyle-Width="230" />
                        <telerik:GridBoundColumn UniqueName="Junta" DataField="Junta" meta:resourcekey="gbcJunta"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Localizacion" DataField="Localizacion" meta:resourcekey="gbcLocalizacion"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Tipo" DataField="Tipo" meta:resourcekey="gbcTipo"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" meta:resourcekey="gbcCedula"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Material1" DataField="Material1" meta:resourcekey="gbcMaterial1"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Material2" DataField="Material2" meta:resourcekey="gbcMaterial2"
                            FilterControlWidth="45" HeaderStyle-Width="80" />
                        <telerik:GridBoundColumn UniqueName="Diametro" DataField="Diametro" DataFormatString="{0:#0.000}" meta:resourcekey="gbcDiametro"
                            FilterControlWidth="50" HeaderStyle-Width="100" />
                        <telerik:GridCheckBoxColumn DataField="TieneHold" meta:resourcekey="TieneHold" FilterControlWidth="80" HeaderStyle-Width="80"></telerik:GridCheckBoxColumn>
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
