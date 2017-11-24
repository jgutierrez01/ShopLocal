<%@ Page  Language="C#" MasterPageFile="~/Masters/Materiales.master" AutoEventWireup="true"
    CodeBehind="LstPinturaNumUnico.aspx.cs" Inherits="SAM.Web.Materiales.LstPinturaNumUnico" %>
<%@ MasterType VirtualPath="~/Masters/Materiales.master" %>
<%@ Import Namespace="Mimo.Framework.Extensions" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>

<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdPintura" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros" style="margin-bottom: 5px;">
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label meta:resourcekey="lblProyecto" runat="server" ID="lblProyecto" CssClass="bold"
                        AssociatedControlID="ddlProyecto" />
                    <mimo:MappableDropDown runat="server" ID="ddlProyecto" AutoPostBack="true" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" />
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator meta:resourcekey="rfvProyecto" runat="server" ID="rfvProyecto"
                        ControlToValidate="ddlProyecto" Display="None" ValidationGroup="" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" ValidationGroup=""
                        OnClick="btnMostrar_Click" CssClass="boton" />
                </div>
            </div>
            <p>
            </p>
        </div>
        <asp:ValidationSummary ID="valSummary" runat="server" ValidationGroup="" CssClass="summaryList"
            meta:resourcekey="valSummary" />
        <div class="separador">
            <sam:Header ID="proyEncabezado" runat="server" Visible="false" />
        </div>
        <p>
        </p>
        <%--RadGrid--%>
        <mimo:MimossRadGrid runat="server" ID="grdPintura" Visible="false" OnNeedDataSource="grdPintura_OnNeedDataSource"
            OnItemCreated="grdPintura_OnItemCreated" AllowMultiRowSelection="true" CssClass="RadGrid RadGrid_SAMOrange segJunta">
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="NumeroUnicoID" ClientDataKeyNames="NumeroUnicoID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink meta:resourcekey="lnkPintar" runat="server" ID="lnkPintar" CssClass="link" />
                        <asp:HyperLink meta:resourcekey="imgPintar" runat="server" ID="imgPintar" ImageUrl="~/Imagenes/Iconos/icono_pintar.png" CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn UniqueName="chkSelect_h" Display="true" HeaderStyle-Width="35" />
                    <telerik:GridBoundColumn UniqueName="NumeroRequisicion" DataField="NumeroRequisicion" meta:resourcekey="gbcNumeroRequisicion" FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn UniqueName="FechaRequisicion" DataField="FechaRequisicion"
                        DataFormatString="{0:d}" meta:resourcekey="gbcFechaRequisicion" FilterControlWidth="100"
                        HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn UniqueName="NumeroUnico" DataField="NumeroUnico" meta:resourcekey="gbcNumeroUnico"
                        FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn UniqueName="ItemCode" DataField="ItemCode" meta:resourcekey="gbcItemCode"
                        FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" meta:resourcekey="gbcDescripcion"
                        FilterControlWidth="150" HeaderStyle-Width="350" />
                    <telerik:GridCheckBoxColumn UniqueName="Liberado" DataField="Liberado" meta:resourcekey="gbcLiberado"
                        FilterControlWidth="100" HeaderStyle-Width="130" />
                    <telerik:GridTemplateColumn UniqueName="Primario" AllowFiltering="false" Groupable="false" FilterControlWidth="100" ItemStyle-Width="150" HeaderStyle-Width="300" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <table id="Table1"  width="300">
                                <tr>
                                    <th colspan="2" ><asp:Literal ID="litPrimario" runat="server" meta:resourcekey="litPrimario" /></th>
                                </tr>
                                <tr>                                    
                                    <td><asp:Literal ID="litFechaPrimarios" runat="server" meta:resourcekey="litFechaPrimarios" /></td> 
                                    <td><asp:Literal ID="litReportePrimarios" runat="server" meta:resourcekey="litReportePrimarios" /></td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table id="Table2"  width="300" >
                                 <tr>                                    
                                    <td><%# DataBinder.Eval(Container.DataItem, "FechaPrimarios").SafeDateAsStringParse()%></td> 
                                    <td><%# DataBinder.Eval(Container.DataItem, "ReportePrimarios")%></td>
                                </tr>                                
                            </table>
                        </ItemTemplate>                                                
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Intermedio" AllowFiltering="false" FilterControlWidth="100" Groupable="false" ItemStyle-Width="150" HeaderStyle-Width="300" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <table id="Table1"  width="300" >
                                <tr class="">
                                    <th colspan="2" ><asp:Literal ID="litIntermedio" runat="server" meta:resourcekey="litIntermedio" /></th>
                                </tr>
                                <tr class="">                                    
                                    <td><asp:Literal ID="litFechaIntermedio" runat="server" meta:resourcekey="litFechaIntermedio" /></td> 
                                    <td><asp:Literal ID="litReporteIntermedio" runat="server" meta:resourcekey="litReporteIntermedio" /></td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table id="Table2"  width="300" >
                                 <tr>                                    
                                    <td><%# DataBinder.Eval(Container.DataItem, "FechaIntermedio").SafeDateAsStringParse()%></td> 
                                    <td><%# DataBinder.Eval(Container.DataItem, "ReporteIntermedio")%></td>
                                </tr>                                
                            </table>
                        </ItemTemplate>                                                
                    </telerik:GridTemplateColumn>  
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
         <div id="bntActualiza">
            <asp:Button runat="server" CausesValidation="false" ID="btnActualiza" CssClass="oculto"
                OnClick="btnActualiza_Click" />
        </div>
        <p>
        </p>
    </div>
</asp:Content>
