<%@ Page  Language="C#" MasterPageFile="~/Masters/WorkStatus.master" AutoEventWireup="true"
    CodeBehind="LstReqPintura.aspx.cs" Inherits="SAM.Web.WorkStatus.LstReqPintura" %>

<%@ MasterType VirtualPath="~/Masters/WorkStatus.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" TagName="Filtro" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
     <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdReqNum">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdReqNum" LoadingPanelID="loadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>
 
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <uc2:Filtro ProyectoRequerido="true" OrdenTrabajoRequerido="false" FiltroNumeroUnico="false"
                ProyectoHeaderID="headerProyecto" ProyectoAutoPostBack="true" runat="server"
                FiltroNumeroControl="false" ID="filtroGenerico"></uc2:Filtro>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Label CssClass="labelHack bold" runat="server" ID="lblSeleccion" meta:resourcekey="lblSeleccion"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlSeleccion" meta:resourcekey="ddlSeleccion">
                        <asp:ListItem runat="server" Text=" " Selected="True" Value="-1"></asp:ListItem>
                        <asp:ListItem runat="server" Text="Especificar Sistema" Value="1" meta:resourcekey="lstSistema"></asp:ListItem>
                        <asp:ListItem runat="server" Text="Generar Requisición" Value="2" meta:resourcekey="lstRequisicion"></asp:ListItem>
                    </asp:DropDownList>
                    <span class="required">*</span>
                    <asp:RequiredFieldValidator ID="rfvOt" runat="server" ControlToValidate="ddlSeleccion"
                        InitialValue="-1" meta:resourcekey="valSeleccion" Display="None" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <asp:Button meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" OnClick="btnMostrar_Click"
                        CssClass="boton" />
                </div>
            </div>
            <p>
            </p>
        </div>

        <p>
        </p>
        <sam:Header ID="headerProyecto" runat="server" Visible="false" />
        <asp:ValidationSummary ID="valProyecto" runat="server" CssClass="summaryList" meta:resourcekey="valSummary" />
        <asp:PlaceHolder runat="server" ID="phTotalizador" Visible="false">
            <p></p>
            <div class="ancho100">
            <table class="repSam" cellpadding="0" cellspacing="0" width="100%">
                <colgroup>
                    <col width="20%" />
                    <col width="20%" />
                    <col width="20%" />
                    <col width="20%" />
                    <col width="20%" />          
                </colgroup>
                <thead>
                <tr class="repEncabezado">
                    <th colspan="20%"><asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></th>
                </tr>
                <tr class="repTitulos">
                    <th></th>
                    <th><asp:Literal runat="server" ID="litSpools" meta:resourcekey="litSpools" /></th>             
                    <th><asp:Literal runat="server" ID="litArea" meta:resourcekey="litArea" /></th>
                    <th><asp:Literal runat="server" ID="litKgs" meta:resourcekey="litKgs" /></th>                    
                    <th><asp:Literal runat="server" ID="litPeqs" meta:resourcekey="litPeqs" /></th>
                </tr>
                </thead>
                <tr class="repFila">
                    <td>
                        <asp:Label runat="server" ID="lblTotal" CssClass="bold" meta:resourcekey="lblTotal"></asp:Label>
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litSpoolsTotales" meta:resourcekey="litSpoolsTotales" />
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litAreaTotales" meta:resourcekey="litAreaTotales" />
                    </td>
                      <td>
                        <asp:Literal runat="server" ID="litKgsTotales" meta:resourcekey="litKgsTotales" />
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litPeqsTotales" meta:resourcekey="litPeqsTotales" />
                    </td>
                </tr>
                <tr class="repFilaPar">
                    <td>
                        <asp:Label runat="server" ID="lblTotalSeleccionado" CssClass="bold" meta:resourcekey="lblTotalSeleccionado"></asp:Label>
                    </td>
                    <td>
                        <span id="spSpoolsSeleccionados">
                            0
                        </span>
                    </td>      
                   
                    <td>
                        <span id="spAreaSeleccionados">
                            0.00
                        </span>
                    </td>
                     <td>
                        <span id="spKgsSeleccionados">
                            0.00
                        </span>
                    </td>
                    <td>
                        <span id="spPeqsSeleccionados">
                            0.00
                        </span>
                    </td>
                </tr>
                <tfoot>
                <tr class="repPie">
                    <td colspan="10">&nbsp;</td>
                </tr>
                </tfoot>
            </table>
        </div>
    </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="phReqNumerosUnicos" Visible="false">
            <p>
            </p>
            <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel" />

            <mimo:MimossRadGrid runat="server" ID="grdReqNum" OnNeedDataSource="grdReqNum_OnNeedDataSource" OnItemCreated="grdReqNum_OnItemCreated"  AllowMultiRowSelection="true"   OnItemDataBound="grdSpools_ItemDataBound">

                <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" ClientDataKeyNames="WorkstatusSpoolID,SpoolID,OrdenTrabajoSpoolID" DataKeyNames="WorkstatusSpoolID,SpoolID,OrdenTrabajoSpoolID" ShowFooter="true">

                    <CommandItemTemplate>
                        <div class="comandosEncabezado">
                            <asp:HyperLink meta:resourcekey="lnkEspecificar" runat="server" ID="lnkEspecificar" CssClass="link"  />
                            <asp:ImageButton runat="server" ID="imgEspecificar" ImageUrl="~/Imagenes/Iconos/icono_especificarsistema.png" CssClass="imgEncabezado" />
                            <asp:HyperLink meta:resourcekey="lnkGenerar" runat="server" ID="lnkGenerar" CssClass="link" />
                            <asp:ImageButton runat="server" ID="imgGenerar" ImageUrl="~/Imagenes/Iconos/icono_generarequisicion.png"
                               CssClass="imgEncabezado" />
                        </div>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h"  HeaderStyle-Width="30" />
                        <telerik:GridBoundColumn meta:resourcekey="spool" HeaderStyle-Width="150" FilterControlWidth="100" DataField="NombreSpool" />
                        <telerik:GridBoundColumn meta:resourcekey="numControl" HeaderStyle-Width="150" FilterControlWidth="100" DataField="NumeroControl" />
                        <telerik:GridBoundColumn meta:resourcekey="sistema" HeaderStyle-Width="150" FilterControlWidth="100" DataField="Sistema" />
                        <telerik:GridBoundColumn meta:resourcekey="color" HeaderStyle-Width="150" FilterControlWidth="100" DataField="Color" />
                        <telerik:GridBoundColumn meta:resourcekey="codigo" HeaderStyle-Width="150" FilterControlWidth="100" DataField="Codigo" />
                        <telerik:GridCheckBoxColumn meta:resourcekey="hold" HeaderStyle-Width="150" FilterControlWidth="100" DataField="Hold" />
                        <telerik:GridBoundColumn meta:resourcekey="especificacion" HeaderStyle-Width="150" FilterControlWidth="100" DataField="EspecificacionSpool" />
                        <telerik:GridBoundColumn meta:resourcekey="area" HeaderStyle-Width="150" FilterControlWidth="100" DataField="Area" />
                        <telerik:GridBoundColumn meta:resourcekey="peso" HeaderStyle-Width="150" FilterControlWidth="100" DataField="Peso" />
                        <telerik:GridBoundColumn meta:resourcekey="localizacion" HeaderStyle-Width="150" FilterControlWidth="100" DataField="Localizacion" />
                        <telerik:GridBoundColumn meta:resourcekey="peqs" HeaderStyle-Width="150" FilterControlWidth="100" DataField="Peqs" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                            <ItemTemplate>
                                &nbsp;
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                     <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                <ClientEvents OnRowSelected="Sam.Ingenieria.FilaSeleccionada" OnRowDeselected="Sam.Ingenieria.FilaSeleccionada" />
            </ClientSettings>
            </mimo:MimossRadGrid>
            <div id="btnWrapper" class="oculto">
                <asp:Button CssClass="oculto" runat="server" ID="btnRefresh" CausesValidation="False" OnClick="lnkActualizar_Click" meta:resourcekey="btnRefreshResource1" />
            </div>
        </asp:PlaceHolder>
    </div>
</asp:Content>
