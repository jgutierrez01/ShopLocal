<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true" CodeBehind="TblEspesores.aspx.cs" Inherits="SAM.Web.Catalogos.TblEspesores" %>
<%@ MasterType VirtualPath="~/Masters/Catalogos.master" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="perfil">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="perfil"/>
                </UpdatedControls>
            </telerik:AjaxSetting>                
        </AjaxSettings>
    </telerik:RadAjaxManager>

    <div class="contenedorCentral">
        <asp:ValidationSummary runat="server" ID="ValidationSummary1" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" />       
            
        <mimo:MimossRadGrid runat="server" ID="grdTblEspesores" Height="500px" OnNeedDataSource="grdTblEspesores_OnNeedDataSource" OnItemDataBound="grdTblEspesores_OnItemDataBound" EnableViewState="false" AllowPaging="false">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false" AllowFilteringByColumn="false" AllowPaging="false" EnableHeaderContextMenu="false" EnableHeaderContextFilterMenu="false">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label Text="ESPESORES" runat="server" ID="lblTblEspesores" CssClass="Titulo" meta:resourcekey="lblTblEspesores"></asp:Label>
                        </div>
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkAgregar" Text="Subir espesores" NavigateUrl="~/Catalogos/ImportaEspesores.aspx" meta:resourcekey="lnkAgregar" CssClass="link" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgAgregar" ImageUrl="~/Imagenes/Iconos/agregar.png" Text="Subir espesores" CssClass="imgEncabezado" NavigateUrl="~/Catalogos/ImportaEspesores.aspx" meta:resourcekey="imgAgregar" />
                    </div>
                </CommandItemTemplate>   
                <Columns>
                     <telerik:GridBoundColumn UniqueName="Diametro" DataField="Valor" DataFormatString="{0:N3}" Reorderable="false" Resizable="false" meta:resourcekey="grdDiametro" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" AllowFiltering="false" AllowSorting="false" />
                </Columns>             
            </MasterTableView>            
            <ClientSettings>
                <Scrolling FrozenColumnsCount="1" UseStaticHeaders="true" AllowScroll="true" />
            </ClientSettings>
        </mimo:MimossRadGrid>  
    </div>
</asp:Content>
