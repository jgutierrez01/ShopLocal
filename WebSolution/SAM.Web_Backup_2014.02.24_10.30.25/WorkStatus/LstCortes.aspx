<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LstCortes.aspx.cs" Inherits="SAM.Web.WorkStatus.LstCortes"
    MasterPageFile="~/Masters/WorkStatus.master"  %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>      
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Encabezado" TagPrefix="proy" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdCortes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdCortes" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
             <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblCorte" CssClass="Titulo" meta:resourcekey="lblCorte"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" Text="Proyecto:" meta:resourcekey="lblProyecto"
                        CssClass="bold" /><br />
                    <asp:DropDownList runat="server" ID="ddlProyecto" 
                        OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" AutoPostBack="True" 
                        meta:resourcekey="ddlProyectoResource1" />
                        <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="valProyecto" runat="server" Display="None" 
                        ControlToValidate="ddlProyecto" meta:resourcekey="valProyecto"></asp:RequiredFieldValidator>
                </div>
            </div>
             <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" meta:resourcekey="btnMostrar"
                        OnClick="btnMostrar_Click" CssClass="boton" />
                </div>
            </div>
            <p></p>
        </div>
        <p></p>
         <div>
            <proy:Encabezado ID="proyEncabezado" runat="server" Visible="False" />
        </div>
        <asp:ValidationSummary meta:resourcekey="valSummary" runat="server" ID="valSummary"
            CssClass="summaryList" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
        <mimo:MimossRadGrid runat="server" ID="grdCortes" OnNeedDataSource="grdCortes_OnNeedDataSource"
            OnItemCommand="grdCortes_ItemCommand" Visible="false" OnItemDataBound="grdCortes_ItemDataBound">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">
                <CommandItemTemplate>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridTemplateColumn UniqueName="ver_h" AllowFiltering="false" Groupable="false"
                        ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink meta:resourcekey="hlVer" runat="server" ID="hlVer" ImageUrl="~/Imagenes/Iconos/info.png" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="cancelar_h" AllowFiltering="false" Groupable="false"
                        ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgCancelar" meta:resourcekey="imgCancelar" ImageUrl="~/Imagenes/Iconos/ico_eliminarcorteB.png"
                                CommandName="cancelar" CommandArgument='<%#Eval("CorteID") %>' OnClientClick="return Sam.Confirma(8);" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="NumeroUnico" DataField="NumeroUnico" meta:resourcekey="hdNumeroUnico"
                        HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="ItemCode" DataField="ItemCode" meta:resourcekey="hdItemCode"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" meta:resourcekey="hdDescripcion"
                        HeaderStyle-Width="300" FilterControlWidth="100" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="Diametro1" DataField="Diametro1" DataFormatString="{0:#0.000}" meta:resourcekey="hdDiametro1"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="CantidadCortes" DataField="CantidadCortes" meta:resourcekey="hdCantidadCortes"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="Estatus" DataField="Estatus" meta:resourcekey="hdEstatus"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="Sobrante" DataField="Sobrante" meta:resourcekey="hdSobrante"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
                        <telerik:GridBoundColumn UniqueName="Merma" DataField="Merma" meta:resourcekey="hdMerma"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" />
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
