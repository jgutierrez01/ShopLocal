<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true" CodeBehind="LiberacionesDimensionalesPatio.aspx.cs" Inherits="SAM.Web.WorkStatus.LiberacionesDimensionalesPatio" %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>   
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
 <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnRefresh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdInspeccionDimencionalPatio" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>


<div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" Text="LIBERACIONES DIMENSIONALES PATIO"></asp:Label>
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
             <mimo:MimossRadGrid ID="grdInspeccionDimencionalPatio" 
                                 runat="server" 
                                 OnNeedDataSource="grdInspeccionDimencionalPatio_OnNeedDataSource"
                                 OnItemDataBound="grdInspeccionDimansionalPatio_OnItemDataBound"
                                 OnItemCreated="grdInspeccionDimansionalPatio_ItemCreated"
                                 AutoGenerateColumns="true"
                                 AllowMultiRowSelection="true"
                                 AllowMultiColumnSorting="true"
                                 >
                <ClientSettings Selecting-AllowRowSelect="true" />
                <MasterTableView AutoGenerateColumns="false"
                                 AllowMultiColumnSorting="true"
                                 DataKeyNames="InspeccionDimansionalPatioID"
                                 ClientDataKeyNames="InspeccionDimansionalPatioID">   
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:HyperLink runat="server" ID="hlReporte" Text="Reporte" meta:resourcekey="hlReporte" />
                            <asp:HyperLink runat="server" ID="hlReporteImagen" meta:resourcekey="hlReporteImagen" ImageUrl="~/Imagenes/Iconos/Icono_GeneraReporte.png" />    
                        </div>
                    </CommandItemTemplate>             
                    <Columns>
                        <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h" HeaderStyle-Width="30"/>
                        <telerik:GridBoundColumn UniqueName="HdNumeroDeControl" DataField="NumeroDeControl" HeaderText="Numero De Control"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdNumeroDeControl"/>
                        <telerik:GridBoundColumn UniqueName="HdSpool" DataField="Spool" HeaderText="Spool"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdSpool"/>
                        <telerik:GridBoundColumn UniqueName="HdResultado" DataField="Resultado" HeaderText="Resultado"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdResultado"/>
                        <telerik:GridBoundColumn UniqueName="HdFechaInspeccion" DataField="FechaInspeccion" HeaderText="Fecha Inspeccion"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdFechaInspeccion"/>
                        <telerik:GridBoundColumn UniqueName="HdObservaciones" DataField="Observaciones" HeaderText="Observaciones"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdObservaciones"/>
                        <telerik:GridCheckBoxColumn UniqueName="HdHold" DataField="Hold" HeaderText="¿Hold?"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdHold"/>
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

      <div id="btnWrapper" class="oculto">
        <asp:Button CssClass="oculto" runat="server" OnClick="btnWrapper_Click" ID="btnRefresh" CausesValidation="false"  />
    </div>

</asp:Content>
