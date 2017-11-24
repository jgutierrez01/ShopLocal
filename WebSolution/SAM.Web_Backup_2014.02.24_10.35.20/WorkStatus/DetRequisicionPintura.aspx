<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true" CodeBehind="DetRequisicionPintura.aspx.cs" Inherits="SAM.Web.WorkStatus.DetRequisicionPintura" %>
<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>   
 <%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
 <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdReportePnd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReportePnd" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

    
        <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblHeader"
        NavigateUrl="~/WorkStatus/ReporteRequisicionesPintura.aspx" /> 

 <div class="contenedorCentral">
  <uc1:Header ID="proyHeader" runat="server" Visible="true" />
        <div class="cajaAzul">
           <div class="divIzquierdo ancho30">
                    <asp:Label ID="lblNumeroDeRequisicionLabel" runat="server" Text="Numero de Requisicion:" meta:resourcekey="lblNumeroDeRequisicionLabel"
                        CssClass="bold" />
                    <asp:Label ID="lblNumeroDeRequisicion" runat="server" Text="[NuemroRequisicion]" meta:resourcekey="lblNumeroDeRequisicion"/>
                <p></p>
                    <asp:Label ID="lblTotalSpoolsLabel" runat="server" Text="Total de Spools:" meta:resourcekey="lblTotalSpoolsLabel"
                            CssClass="bold" />
                                         
                       <asp:Label ID="lblTotalSpools" runat="server" Text="[TotalSpools]" meta:resourcekey="lblTotalSpools"/>
                <p> </p>
            </div>  
            <div class="divIzquierdo">  
                        <asp:Label ID="lblFechaRequisicionlabel" runat="server" Text="Fecha Requisicion:" meta:resourcekey="lblFechaRequisicionlabel"
                            CssClass="bold" />
                        <asp:Label ID="lblFechaRequisicion" runat="server" Text="[FechaRequisicion]" meta:resourcekey="lblFechaRequisicion"/>
                 <p></p>
             </div>

              
          <p></p>

         </div>

         <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" HeaderText="Errores" />
         <p> </p>
         
         <asp:PlaceHolder runat="server" ID="phGrid" Visible="true">
         <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
    
         <mimo:MimossRadGrid 
         ID="grdDetRequisicionPintura" 
         runat="server" 
         OnNeedDataSource="grdDetRequisicionPintura_OnNeedDataSource" 
         OnItemCommand="grdDetRequisicionPintura_OnItemCommand"
         AllowMultiRowSelection="true">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="RequisicionPinturaDetalleID"> 
             <CommandItemTemplate>
                <div class="comandosEncabezado">                               
                             
                </div>
             </CommandItemTemplate>  
                <Columns>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" Text="Borrar" ID="btnBorrar" runat="server"  
                                meta:resourcekey="btnBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("RequisicionPinturaDetalleID") %>'
                                     OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn UniqueName="HdNumeroDeControl" DataField="NumeroDeControl" HeaderText="Numero de Control" 
                        HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdNumeroDeControl"/>
                    <telerik:GridBoundColumn UniqueName="HdSpool" DataField="Spool" HeaderText="Spool"
                        HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdSpool"/>  
                    <telerik:GridBoundColumn UniqueName="HdSistema" DataField="Sistema" HeaderText="Sistema"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false"  meta:resourcekey="HdSistema"/>  
                    <telerik:GridBoundColumn UniqueName="HdCodigo" DataField="Codigo" HeaderText="Codigo"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false"  meta:resourcekey="HdCodigo"/>
                    <telerik:GridBoundColumn UniqueName="HdColor" DataField="Color" HeaderText="Color"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false"  meta:resourcekey="HdColor"/>
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
