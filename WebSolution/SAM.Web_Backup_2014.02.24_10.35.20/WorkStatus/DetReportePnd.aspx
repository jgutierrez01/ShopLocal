<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true" CodeBehind="DetReportePnd.aspx.cs" Inherits="SAM.Web.WorkStatus.DetReportePnd" %>
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
                    <telerik:AjaxUpdatedControl ControlID="ToolTip" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
         <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblHeader"
        NavigateUrl="~/WorkStatus/ReportePND.aspx" /> 

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
                    
                       <asp:Label ID="lblTotalJuntasLabel" runat="server" Text="Total de Juntas:" meta:resourcekey="lblTotalJuntasLabel"
                            CssClass="bold" />
                       <asp:Label ID="lblTotalJuntas" runat="server" Text="[TotalJuntas]" meta:resourcekey="lblTotalJuntas"/>
                    <p> </p>
                        <asp:Label ID="lblJuntasAprobadaslabel" runat="server" Text="Juntas Aprobadas:" meta:resourcekey="lblJuntasAprobadaslabel"
                            CssClass="bold" />
                        <asp:Label ID="lblJuntasAprobadas" runat="server" Text="[JuntasAprobadas]" meta:resourcekey="lblJuntasAprobadas"/>
                    <p> </p>
                        <asp:Label ID="lblJuntasRechazadasLabel" runat="server" Text="Juntas Rechazadas:" meta:resourcekey="lblJuntasRechazadasLabel"
                            CssClass="bold" />
                        <asp:Label ID="lblJuntasRechazadas" runat="server" Text="[JuntasRechazadas]" meta:resourcekey="lblJuntasRechazadas"/>
             
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
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="JuntaReportePndID"> 
             <CommandItemTemplate>
                <div class="comandosEncabezado">                               
                             
                </div>
             </CommandItemTemplate>  
                <Columns>
                  <telerik:GridBoundColumn CurrentFilterFunction="NoFilter" DataField="JuntaReportePndID" Display="false"
                    DataType="System.Int32" FilterListOptions="VaryByDataType" ForceExtractValue="None" 
                    HeaderText="JuntaReportePndID" ReadOnly="True" SortExpression="JuntaReportePndID" UniqueName="JuntaReportePndID">
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
                                meta:resourcekey="btnBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("JuntaReportePndID") %>'
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
                    <telerik:GridBoundColumn UniqueName="HdJunta" DataField="Junta" HeaderText="Junta"
                        HeaderStyle-Width="70" FilterControlWidth="30" Groupable="false"  meta:resourcekey="HdJunta"/>
                    <telerik:GridBoundColumn UniqueName="HdLocalizacion" DataField="Localizacion" HeaderText="Localizacion"
                        HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false"  meta:resourcekey="HdLocalizacion"/>
                    <telerik:GridBoundColumn UniqueName="HdTipo" DataField="Tipo" HeaderText="Tipo"
                        HeaderStyle-Width="70" FilterControlWidth="30" Groupable="false"  meta:resourcekey="HdTipo"/>
                    <telerik:GridBoundColumn UniqueName="HdCedula" DataField="Cedula" HeaderText="Cedula"
                        HeaderStyle-Width="70" FilterControlWidth="30" Groupable="false"  meta:resourcekey="HdCedula"/>
                    <telerik:GridBoundColumn UniqueName="HdMaterial1" DataField="FamiliaAcero" HeaderText="Familia Acero Material"
                        HeaderStyle-Width="160" FilterControlWidth="90" Groupable="false"  meta:resourcekey="HdMaterial1"/>
                    <telerik:GridBoundColumn UniqueName="HdDiametro" DataField="Diametro" DataFormatString="{0:#0.000}" HeaderText="Diametro"
                        HeaderStyle-Width="80" FilterControlWidth="40" Groupable="false"  meta:resourcekey="HdDiametro"/>                   
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
