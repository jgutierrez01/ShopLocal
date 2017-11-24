<%@ Page  Language="C#" MasterPageFile="~/Masters/Produccion.Master" AutoEventWireup="true" CodeBehind="CrucePorProyecto.aspx.cs" Inherits="SAM.Web.Produccion.CrucePorProyecto" %>
<%@ MasterType VirtualPath="~/Masters/Produccion.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
 <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajxMgr">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="grdSpools">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="loadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="btnQuitaSeleccionados">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdSpools" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="ddlProyecto">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="headerProyecto" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>
<div class="paginaHeader">
    <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
</div>
<div class="contenedorCentral">
    <div class="cajaFiltros" style="margin-bottom:5px;">
        <div class="divIzquierdo">
            <div class="separador">
                <asp:Label meta:resourcekey="lblProyecto" runat="server" ID="lblProyecto" AssociatedControlID="ddlProyecto" />
                <mimo:MappableDropDown runat="server" ID="ddlProyecto" OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" AutoPostBack="true"/>
                <asp:RequiredFieldValidator meta:resourcekey="reqProyecto" runat="server" ID="reqProyecto" ControlToValidate="ddlProyecto" Display="None" ValidationGroup="vgProyecto" />
            </div>
        </div>
        <div class="divIzquierdo">
            <div class="separador">
                <asp:Button meta:resourcekey="btnMostrar" ID="btnMostrar" runat="server" ValidationGroup="vgProyecto" OnClick="btnMostrar_Click" CssClass="boton" />
            </div>
        </div>
        <p></p>
    </div>
    <asp:ValidationSummary ID="valProyecto" runat="server" ValidationGroup="vgProyecto" CssClass="summaryList" meta:resourcekey="valSummary" />
    <div class="separador">
        <sam:Header ID="headerProyecto" runat="server" Visible="false" />
    </div>
    <asp:PlaceHolder runat="server" ID="phLabels" Visible="false">
        <p></p>
        <div class="cajaAzul">
            <div class="divIzquierdo" style="margin-right:15px;">
                <p>
                    <asp:Label runat="server" ID="lblTotalTexto" meta:resourcekey="lblTotalTexto" CssClass="bold"/>&nbsp;
                    <asp:Label runat="server" ID="lblTotalValor" meta:resourcekey="lblTotalValor" />
                </p>
                <p>
                    <asp:Label runat="server" ID="lblSinOdtTexto" meta:resourcekey="lblSinOdtTexto" CssClass="bold"/>&nbsp;
                    <asp:Label runat="server" ID="lblSinOdtValor" meta:resourcekey="lblSinOdtValor" />
                </p>
            </div>
            <div class="divIzquierdo" style="margin-right:15px; margin-top: 10px">
                <div style="height: 15px">
                    <mimo:MappableCheckBox runat="server" ID="chkSpoolHold" EntityPropertyName="VerificadoPorSpoolHold"
                                               meta:resourcekey="chkSpoolHold" CssClass="checkYTexto" />
                </div>
                <p></p>
                <div>
                    <mimo:MappableCheckBox runat="server" ID="chkCrucePorIsometrico" EntityPropertyName="CrucePorIsometrico"
                                               meta:resourcekey="chkCrucePorIsometrico" CssClass="checkYTexto" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    <samweb:BotonProcesando CssClass="boton" runat="server" ID="btnCruzar" OnClick="btnCruzar_Click" meta:resourcekey="btnCruzar" CausesValidation="false" />
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="separador">
                    &nbsp;&nbsp;&nbsp;<asp:Button CssClass="boton" runat="server" ID="btnPrioridad" Text="Prioridades" meta:resourcekey="btnPrioridad" OnClientClick="window.location='/Produccion/FijarPrioridad.aspx';return false;" />
                </div>
            </div>
            <p></p>
        </div>
    </asp:PlaceHolder>
    <asp:PlaceHolder runat="server" ID="phTotalizador" Visible="false">
        <p></p>
        <div class="ancho100">
            <table class="repSam" cellpadding="0" cellspacing="0" width="100%">
                <colgroup>
                    <col width="60" />
                    <col width="60" />
                    <col width="60" />
                    <col width="60" />
                    <col width="60" />
                    <col width="60" />
                    <col width="60" />
                    <col width="60" />
                    <col width="60" />
                    <col width="60" />
                </colgroup>
                <thead>
                <tr class="repEncabezado">
                    <th colspan="10"><asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" /></th>
                </tr>
                <tr class="repTitulos">
                    <th></th>
                    <th><asp:Literal runat="server" ID="litSpools" meta:resourcekey="litSpools" /></th>
                    <th><asp:Literal runat="server" ID="litJuntas" meta:resourcekey="litJuntas" /></th>
                    <th><asp:Literal runat="server" ID="litAccesorios" meta:resourcekey="litAccesorios" /></th>
                    <th><asp:Literal runat="server" ID="litTubos" meta:resourcekey="litTubos" /></th>
                    <th><asp:Literal runat="server" ID="litLongitud" meta:resourcekey="litLongitud" /></th>
                    <th><asp:Literal runat="server" ID="litPdi" meta:resourcekey="litPdi" /></th>
                    <th><asp:Literal runat="server" ID="litKgs" meta:resourcekey="litKgs" /></th>
                    <th><asp:Literal runat="server" ID="litArea" meta:resourcekey="litArea" /></th>
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
                        <asp:Literal runat="server" ID="litJuntasTotales" meta:resourcekey="litJuntasTotales" />
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litAccesoriosTotales" meta:resourcekey="litAccesoriosTotales" />
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litTubosTotales" meta:resourcekey="litTubosTotales" />
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litLongitudTotales" meta:resourcekey="litLongitudTotales" />
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litPdiTotales" meta:resourcekey="litPdiTotales" />
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litKgsTotales" meta:resourcekey="litKgsTotales" />
                    </td>
                    <td>
                        <asp:Literal runat="server" ID="litAreaTotales" meta:resourcekey="litAreaTotales" />
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
                        <span id="spJuntasSeleccionados">
                            0
                        </span>
                    </td>
                    <td>
                        <span id="spAccesoriosSeleccionados">
                            0
                        </span>
                    </td>
                    <td>
                        <span id="spTubosSeleccionados">
                            0
                        </span>
                    </td>
                    <td>
                        <span id="spLongitudSeleccionados">
                            0.00
                        </span>
                    </td>
                    <td>
                        <span id="spPdiSeleccionados">
                            0.00
                        </span>
                    </td>
                    <td>
                        <span id="spKgsSeleccionados">
                            0.00
                        </span>
                    </td>
                    <td>
                        <span id="spAreaSeleccionados">
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
    <asp:PlaceHolder runat="server" ID="phSpools" Visible="False">
        <p></p>
        <telerik:RadAjaxLoadingPanel runat="server" ID="loadingPanel" />

        <mimo:MimossRadGrid runat="server" ID="grdSpools" OnNeedDataSource="grdSpools_OnNeedDataSource" OnItemDataBound="grdSpools_ItemDataBound" OnItemCreated="grdSpools_ItemCreated" AllowMultiRowSelection="true">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" ClientDataKeyNames="SpoolID" DataKeyNames="SpoolID" ShowFooter="true">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink meta:resourcekey="lnkFaltantes" runat="server" ID="hlFaltantes" CssClass="link" />
                        <asp:HyperLink meta:resourcekey="imgFaltantes" runat="server" ID="imgFaltantes" ImageUrl="~/Imagenes/Iconos/icono_generareporte.png" CssClass="imgEncabezado" />
                        <asp:HyperLink meta:resourcekey="lnkFaltantesExcel" runat="server" ID="hlFaltantesExcel" CssClass="link" />
                        <asp:HyperLink meta:resourcekey="imgFaltantesExcel" runat="server" ID="imgFaltantesExcel" ImageUrl="~/Imagenes/Iconos/excel.png" CssClass="imgEncabezado" />
                        <asp:HyperLink meta:resourcekey="lnkOdt" runat="server" ID="lnkOdt" CssClass="link" />
                        <asp:HyperLink meta:resourcekey="imgOdt" runat="server" ID="imgOdt" ImageUrl="~/Imagenes/Iconos/icono_odt.png" CssClass="imgEncabezado" />
                        <asp:LinkButton meta:resourcekey="lnkActualizar" runat="server" ID="lnkActualizar" OnClick="lnkActualizar_OnClick" CssClass="link" />
                        <asp:ImageButton meta:resourcekey="imgActualizar" runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkActualizar_OnClick" CssClass="imgEncabezado" />
                    </div>
                </CommandItemTemplate>

                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h" HeaderStyle-Width="30"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdPrioridad" UniqueName="Prioridad" DataField="Prioridad" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridTemplateColumn meta:resourcekey="grdSpoolCol" FilterControlWidth="100" HeaderStyle-Width="150" SortExpression="Nombre" DataField="Nombre" DataType="System.String">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%#Eval("SpoolID","javascript:Sam.Produccion.AbrePopupSpoolRO({0});")%>' Text='<%# Eval("Nombre") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn meta:resourcekey="grdPDI" UniqueName="Pdis" DataField="Pdis" FilterControlWidth="30" HeaderStyle-Width="70" DataFormatString="{0:#0.000}" Aggregate="Sum" FooterAggregateFormatString="Total:<br />{0:F}"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdJuntas" UniqueName="Juntas" DataField="Juntas" FilterControlWidth="30" HeaderStyle-Width="70" />
                    <telerik:GridBoundColumn meta:resourcekey="grdTotalPeqs" UniqueName="TotalPeqs" DataField="TotalPeqs" FilterControlWidth="40" HeaderStyle-Width="80" DataFormatString="{0:#0.000}"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdPeso" UniqueName="Peso" DataField="Peso" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn meta:resourcekey="grdArea" UniqueName="Area" DataField="Area" FilterControlWidth="30" HeaderStyle-Width="70" />
                    <telerik:GridBoundColumn meta:resourcekey="grdCedula" UniqueName="Cedula" DataField="Cedula" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn meta:resourcekey="grdFamAcero" UniqueName="FamiliasAcero" DataField="FamiliasAcero" FilterControlWidth="40" HeaderStyle-Width="80" />
                    <telerik:GridBoundColumn meta:resourcekey="grdTotalAccesorio" UniqueName="TotalAccesorio" DataField="TotalAccesorio" FilterControlWidth="30" HeaderStyle-Width="70" Display="false"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdTotalTubo" UniqueName="TotalTubo" DataField="TotalTubo" FilterControlWidth="30" HeaderStyle-Width="70"  Display="false"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdLongitudTubo" UniqueName="LongitudTubo" DataField="LongitudTubo" FilterControlWidth="30" HeaderStyle-Width="70"  Display="false"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdDibujo" UniqueName="Dibujo" DataField="Dibujo" FilterControlWidth="100" HeaderStyle-Width="150" />
                    <telerik:GridBoundColumn meta:resourcekey="grdDiametroPlano" UniqueName="DiametroPlano" DataField="DiametroPlano" FilterControlWidth="100" HeaderStyle-Width="150" DataFormatString="{0:#0.000}" />
                    <telerik:GridCheckBoxColumn meta:resourcekey="grdHold" UniqueName="Hold" DataField="Hold" FilterControlWidth="40" HeaderStyle-Width="50" ReadOnly="true"  />
                    <telerik:GridBoundColumn meta:resourcekey="grdObservaciones" UniqueName="ObservacionesHold" DataField="ObservacionesHold" FilterControlWidth="100" HeaderStyle-Width="250" />
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>

            </MasterTableView>
            <ClientSettings>
                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true" />
                <ClientEvents OnRowSelected="Sam.Produccion.FilaSeleccionada" OnRowDeselected="Sam.Produccion.FilaSeleccionada" />
            </ClientSettings>
        </mimo:MimossRadGrid>
        
        <div id="btnWrapper">
            <asp:Button runat="server" CausesValidation="false" ID="btnQuitaSeleccionados" CssClass="oculto" OnClick="btnQuitaSeleccionados_Click" />
        </div>
    </asp:PlaceHolder>
</div>
</asp:Content>
