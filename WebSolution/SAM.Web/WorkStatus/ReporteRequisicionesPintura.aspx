<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true" CodeBehind="ReporteRequisicionesPintura.aspx.cs" Inherits="SAM.Web.WorkStatus.ReporteRequisicionesPintura" %>
    <%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
 <telerik:RadAjaxManager runat="server" style="display:none;"  ID="rdManager">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="grdReporteReqPintura">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReporteReqPintura" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

 <div class="paginaHeader">
        <asp:Label runat="server" ID="lblHeader" CssClass="Titulo" meta:resourcekey="lblHeader" ></asp:Label>
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
                        InitialValue="" meta:resourcekey="valProyecto" Display="None" CssClass="bold" />
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
                    <asp:Label ID="lblNumeroDeRequisicion" runat="server" Text="Numero de Requisicion:" meta:resourcekey="lblNumeroDeRequisicion"
                        CssClass="bold" />
                    <br />
                 <asp:TextBox ID="txtNumeroDeRequisicion" runat="server"></asp:TextBox>
             </div>
          </div>
          <div class="divIzquierdo">
             <div class="separador">
                    <asp:Button ID="btnMostrar" runat="server" Text="Mostrar" meta:resourcekey="btnMostrar"
                         CssClass="boton" OnClick="btnMostrar_OnClick" />
             </div>
          </div>
          <p></p>
        </div>
         <p> </p>

        <div>
            <uc1:Header ID="proyHeader" runat="server" Visible="false" />
        </div>

         <asp:ValidationSummary ID="valSummary" HeaderText="Errores" runat="server" DisplayMode="BulletList" CssClass="summaryList" meta:resourcekey="valSummary" />
        <p></p>
        <asp:PlaceHolder runat="server" ID="phGrid" Visible="false">
             <telerik:RadAjaxLoadingPanel runat="server" ID="ldPanel" />
             <mimo:MimossRadGrid ID="grdReporteReqPintura" runat="server" OnNeedDataSource="grdReporteReqPintura_OnNeedDataSource"
               OnItemCommand="grdReporteReqPintura_ItemCommand" OnItemDataBound="grdReporteReqPintura_OnItemDataBound"> 
                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true">   
                    <CommandItemTemplate>
                        <div class="comandosEncabezado">                               
                              <asp:LinkButton runat="server" ID="lnkActualizar" CausesValidation="false"
                                meta:resourcekey="lnkActualizar" CssClass="link" OnClick="lnkActualizar_onClick" />
                            <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" CausesValidation="false"
                                AlternateText="Actualizar" CssClass="imgEncabezado" OnClick="lnkActualizar_onClick" meta:resourcekey="imgActualizar"/>
                        </div>
                    </CommandItemTemplate>             
                    <Columns>
                       <telerik:GridTemplateColumn UniqueName="hlVer_h" AllowFiltering="false" HeaderStyle-Width="30" Groupable="false">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="hlVer" meta:resourcekey="hlVer" ImageUrl="~/Imagenes/Iconos/info.png" NavigateUrl='<%#Eval("RequisicionPinturaID","~/WorkStatus/DetRequisicionPintura.aspx?ID={0}") %>' />
                                </ItemTemplate>
                        </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn UniqueName="hlDescargar_h" AllowFiltering="false" HeaderStyle-Width="30" Groupable="false">
                            <ItemTemplate>
                                 <asp:HyperLink runat="server" ID="hlDescargar" meta:resourcekey="hlDescargar" ImageUrl="~/Imagenes/Iconos/ico_descargar.png"/>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="btnBorrar_h" AllowFiltering="false" HeaderStyle-Width="30"
                            Groupable="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandName="Borrar" Text="Borrar" ID="btnBorrar" runat="server"  
                                meta:resourcekey="btnBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandArgument='<%#Eval("RequisicionPinturaID") %>'
                                     OnClientClick="return Sam.Confirma(1);" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="inventario_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                        <samweb:LinkVisorReportes ImageUrl="~/Imagenes/Iconos/ico_reporteB.png" runat="server" ID="hdReporte"
                                meta:resourcekey="hdReporte" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                        <telerik:GridBoundColumn UniqueName="HdNumeroDeRequisicion" DataField="NumeroDeRequisicion" HeaderText="Numero de Requisicion"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdNumeroDeRequisicion"/>
                        <telerik:GridBoundColumn UniqueName="HdFecha" DataField="Fecha" HeaderText="Fecha"
                            DataFormatString="{0:d}" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdFecha"/>
                        <telerik:GridBoundColumn UniqueName="HdSpools" DataField="Spools" HeaderText="Spools" DataFormatString="{0:N0}"
                            HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false"  meta:resourcekey="HdSpools"/>    
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
