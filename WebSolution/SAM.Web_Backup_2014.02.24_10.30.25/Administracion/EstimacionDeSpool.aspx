<%@ Page  Language="C#" MasterPageFile="~/Masters/Administracion.Master" AutoEventWireup="true" CodeBehind="EstimacionDeSpool.aspx.cs" Inherits="SAM.Web.Administracion.EstimacionDeSpool" %>
<%@ MasterType VirtualPath="~/Masters/Administracion.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
  <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnRefresh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdEstimacionSpool" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

 <div class="paginaHeader">
        <asp:Label runat="server" ID="lblListaEstimacion" CssClass="Titulo" meta:resourcekey="lblListaEstimacion" ></asp:Label>
 </div>

     <div class="contenedorCentral">
        <div class="cajaFiltros">
           <div class="divIzquierdo">
              <div class="separador">
                    <asp:Label ID="lblProyecto" runat="server" Text="Proyecto:" meta:resourcekey="lblProyecto"
                         CssClass="bold" />
                    <br />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID" 
                        AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedItemChanged"/>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvProyecto" runat="server" ControlToValidate="ddlProyecto"
                        InitialValue="" meta:resourcekey="valProyecto" Display="None" CssClass="bold" ErrorMessage="El proyecto es requerido"/>
              </div>
           </div>
          <div class="divIzquierdo">
             <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" meta:resourcekey="btnMostrar" OnClick="btnMostrar_OnClick"
                         CssClass="boton" />
             </div>
          </div>
          <p></p>
        </div>
         <p> </p>
        <div>
            <uc1:Header ID="proyHeader" runat="server" Visible="false" />
        </div>

         <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" />
        <p></p>
         <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel">
        </telerik:RadAjaxLoadingPanel>
         <asp:PlaceHolder runat="server" ID="phGrid" Visible="false">
             <mimo:MimossRadGrid ID="grdEstimacionSpool" 
                                 runat="server" 
                                 OnNeedDataSource="grdEstimacionSpool_OnNeedDataSource"
                                 OnColumnCreated="grdEstimacionSpool_OnColumnCreated"
                                 AutoGenerateColumns="true"
                                 AllowMultiRowSelection="true"
                                 AllowMultiColumnSorting="true"
                                 OnItemCreated="grdEstimacionSpool_ItemCreated"
                                 >
                <ClientSettings Selecting-AllowRowSelect="true" /> 
                <MasterTableView AutoGenerateColumns="true"
                                 AllowMultiColumnSorting="true"
                                 DataKeyNames="WorkstatusSpoolID"
                                 ClientDataKeyNames="WorkstatusSpoolID">   
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:HyperLink runat="server" ID="hlEstimado"  meta:resourcekey="hlEstimado" Text="Estimar" />
                            <asp:HyperLink runat="server" ID="hlEstimadoImagen"  meta:resourcekey="hlEstimadoImagen" ImageUrl="~/Imagenes/Iconos/icono_estimar.png" />    
                        </div>
                    </CommandItemTemplate>             
                    <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h" HeaderStyle-Width="30"/>
                    </Columns>
                </MasterTableView>
            </mimo:MimossRadGrid> 
         </asp:PlaceHolder>
         
     </div>

       <div id="btnWrapper" class="oculto">
        <asp:Button CssClass="oculto" runat="server" OnClick="btnWrapper_Click" ID="btnRefresh" CausesValidation="false"  />
    </div>
</asp:Content>
