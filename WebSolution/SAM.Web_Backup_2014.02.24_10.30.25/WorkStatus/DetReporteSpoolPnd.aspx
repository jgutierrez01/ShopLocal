<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
         CodeBehind="DetReporteSpoolPnd.aspx.cs" Inherits="SAM.Web.WorkStatus.DetReporteSpoolPnd" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina" TagPrefix="sam" %>

<asp:Content ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdReportePnd">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReportePnd" />
                    <telerik:AjaxUpdatedControl ControlID="ToolTip" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
         <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblHeader" NavigateUrl="~/WorkStatus/ReporteSpoolPND.aspx" /> 

 <div class="contenedorCentral">
    <uc1:Header ID="proyHeader" runat="server" Visible="true" />
        <div class="cajaAzul">           
           <div class="divIzquierdo ancho80">
              <div class="divIzquierdo ancho30">
                    <asp:Label ID="lblNumeroDeReporteLabel" runat="server" Text="Numero de Reporte:" meta:resourcekey="lblNumeroDeReporteLabel"
                        CssClass="bold" />
                    <asp:Label ID="lblNumeroDeReporte" runat="server" Text="[NuemroReporte]" meta:resourcekey="lblNumeroDeReporte"/>
                     <p></p>
                    <asp:Label ID="lblFechaDeReporteLabel" runat="server" Text="Fecha de Reporte:" meta:resourcekey="lblFechaDeReporteLabel"
                        CssClass="bold" />
                    <asp:Label ID="lblFechaDeReporte" runat="server" Text="[FechaReporte]" meta:resourcekey="lblFechaDeReporte"/>
                     <p></p>
                    <asp:Label ID="lblTipoPruebaLabel" runat="server" Text="Tipo Prueba:" meta:resourcekey="lblTipoPruebaLabel"
                        CssClass="bold" />
                    <asp:Label ID="lblTipoPrueba" runat="server" Text="[TipoPrueba]" meta:resourcekey="lblTipoPrueba"/>
             
              </div>

              <div class="divIzquierdo ancho30">             
                    
                       <asp:Label ID="lblTotalSpoolsLabel" runat="server" Text="Total de Spools:" meta:resourcekey="lblTotalSpoolsLabel"
                            CssClass="bold" />
                       <asp:Label ID="lblTotalSpools" runat="server" Text="[TotalSpools]" meta:resourcekey="lblTotalSpools"/>
                    <p> </p>
                        <asp:Label ID="lblSpoolsAprobadoslabel" runat="server" Text="Spools Aprobados:" meta:resourcekey="lblSpoolsAprobadoslabel"
                            CssClass="bold" />
                        <asp:Label ID="lblSpoolsAprobados" runat="server" Text="[SpoolsAprobados]" meta:resourcekey="lblSpoolsAprobados"/>
                    <p> </p>
                        <asp:Label ID="lblSpoolsRechazadosLabel" runat="server" Text="Spools Rechazados:" meta:resourcekey="lblSpoolsRechazadosLabel"
                            CssClass="bold" />
                        <asp:Label ID="lblSpoolsRechazados" runat="server" Text="[SpoolsRechazados]" meta:resourcekey="lblSpoolsRechazados"/>
             
             </div>

           </div>         

            <div class="divDerecho ancho20">
             
           
            </div>
          <p></p>

         </div>

         <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" HeaderText="Errores" />
         <p> </p>
         
         <asp:PlaceHolder runat="server" ID="phGrid" Visible="true">
         <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
    
         <telerik:RadToolTipManager 
            ID="ToolTip" 
            OffsetY="-1" 
            HideEvent="ManualClose"
            Width="400" 
            Height="250"
            runat="server" 
            EnableShadow="true"             
            RelativeTo="Element"
            Position="MiddleRight">
         <WebServiceSettings Method="GetToolTipData" Path="~/Webservices/ToolTipWebService.asmx" />
            
         </telerik:RadToolTipManager>

         <mimo:MimossRadGrid 
         ID="grdReportePnd" 
         runat="server" 
         OnNeedDataSource="grdReportePnd_OnNeedDataSource" 
         OnItemCommand="grdReportePnd_OnItemCommand"
         OnItemDataBound="grdReportePnd_OnItemDataBound"
         AllowMultiRowSelection="true">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="SpoolReportePndID"> 
             <CommandItemTemplate>
                <div class="comandosEncabezado">                               
                             
                </div>
             </CommandItemTemplate>  
                <Columns>
                  <telerik:GridBoundColumn CurrentFilterFunction="NoFilter" DataField="SpoolReportePndID" Display="false"
                    DataType="System.Int32" FilterListOptions="VaryByDataType" ForceExtractValue="None" 
                    HeaderText="JuntaReportePndID" ReadOnly="True" SortExpression="SpoolReportePndID" UniqueName="SpoolReportePndID">
                </telerik:GridBoundColumn>

                <telerik:GridTemplateColumn UniqueName="hlDetalle_h" AllowFiltering="false" HeaderStyle-Width="30" Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlDetalle" ImageUrl="~/Imagenes/Iconos/info.png" />
                            </ItemTemplate>
                             <ItemTemplate>
                            <asp:HyperLink ID="targetControl" ImageUrl="~/Imagenes/Iconos/info.png" runat="server" NavigateUrl="#" ></asp:HyperLink>
                         </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        

                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" Text="Borrar" ID="btnBorrar" runat="server"  
                                meta:resourcekey="btnBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("SpoolReportePndID") %>'
                                     OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    <telerik:GridBoundColumn UniqueName="HdNumeroDeRequisicion" DataField="NumeroDeRequisicion" HeaderText="# Requisicion" 
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false"  meta:resourcekey="HdNumeroDeRequisicion"/>
                    <telerik:GridBoundColumn UniqueName="HdNumeroDeControl" DataField="NumeroDeControl" HeaderText="Numero De Control" 
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false"  meta:resourcekey="HdNumeroDeControl"/>
                    <telerik:GridBoundColumn UniqueName="HdSpool" DataField="Spool" HeaderText="Spool" 
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false"  meta:resourcekey="HdSpool"/>
                    <telerik:GridBoundColumn UniqueName="HdResultado" DataField="Resultado" HeaderText="Resultado"
                        HeaderStyle-Width="110" FilterControlWidth="65" Groupable="false"  meta:resourcekey="HdResultado"/>
                    <telerik:GridBoundColumn UniqueName="HdHoja" DataField="Hoja" HeaderText="Hoja"
                        HeaderStyle-Width="65" FilterControlWidth="25" Groupable="false"  meta:resourcekey="HdHoja"/>
                    <telerik:GridBoundColumn UniqueName="HdFecha" DataField="Fecha" HeaderText="Fecha"
                        DataFormatString="{0:d}" HeaderStyle-Width="110" FilterControlWidth="65" Groupable="false"  meta:resourcekey="HdFecha"/>    
                    <telerik:GridBoundColumn UniqueName="HdMaterial1" DataField="FamiliaAcero" HeaderText="Familia Acero Material"
                        HeaderStyle-Width="160" FilterControlWidth="90" Groupable="false"  meta:resourcekey="HdMaterial1"/>                 
                    <telerik:GridBoundColumn UniqueName="HdObservaciones" DataField="Observaciones" HeaderText="Observaciones"
                        HeaderStyle-Width="160" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdObservaciones"/>
                    
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