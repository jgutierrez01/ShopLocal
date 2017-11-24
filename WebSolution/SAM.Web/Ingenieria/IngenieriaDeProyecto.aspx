<%@ Page Language="C#" MasterPageFile="~/Masters/Ingenieria.master" AutoEventWireup="true"
    CodeBehind="IngenieriaDeProyecto.aspx.cs" Inherits="SAM.Web.Ingenieria.IngenieriaDeProyecto" %>

<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/JuntaRO.ascx" TagName="DetJunta" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/MaterialRO.ascx" TagName="DetMaterial" TagPrefix="sam" %>
<%@ Register Src="~/Controles/Spool/CorteRO.ascx" TagName="DetCorte" TagPrefix="sam" %>
<asp:Content ID="cntHeader" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="cntBody" ContentPlaceHolderID="cphBodyInner" runat="server">
    <telerik:radajaxmanager runat="server" style="display: none;" id="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="ddlProyecto">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="proyEncabezado" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdSpools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="btnRefresh">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdSpools" LoadingPanelID="ldPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:radajaxmanager>
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblIngenieria" CssClass="Titulo" meta:resourcekey="lblIngenieria" />
    </div>
    <div class="contenedorCentral">
        <asp:Panel runat="server" ID="pnlSuperior">
            <div class="cajaFiltros" style="margin-bottom: 5px;">
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Label ID="lblProyecto" runat="server" CssClass="bold" meta:resourcekey="lblProyecto" />
                        <br />
                        <mimo:mappabledropdown id="ddlProyecto" runat="server" onselectedindexchanged="ddlProyecto_SelectedIndexChanged"
                            entitypropertyname="ProyectoID" autopostback="true" causesvalidation="false" />
                        <span class="required">*</span>
                        <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto"
                            InitialValue="" Display="None" meta:resourcekey="rfvProyecto" />
                    </div>
                </div>
                <div class="divIzquierdo">
                    <div class="separador">
                        <asp:Button runat="server" ID="btnMostrar" OnClick="btnMostrar_Click" CssClass="boton"
                            meta:resourcekey="btnMostrar" />
                    </div>
                </div>
                <p>
                </p>
            </div>
            <asp:ValidationSummary runat="server" ID="valSummary" CssClass="summaryList" meta:resourcekey="valSummary" />
        </asp:Panel>
        <div class="separador">
            <sam:header id="proyEncabezado" runat="server" visible="False" />
        </div>
        <p>
        </p>
        <telerik:radajaxloadingpanel runat="server" id="ldPanel">
        </telerik:radajaxloadingpanel>
        <mimo:mimossradgrid runat="server" id="grdSpools" onneeddatasource="grdSpools_OnNeedDataSource"
            showfooter="True" onitemcommand="grdSpools_OnItemCommand" onitemcreated="grdSpools_OnItemCreated"
            onitemdatabound="grdSpools_ItemDataBound" visible="false" allowmultirowselection="true">
            <ClientSettings Selecting-AllowRowSelect="true" /> 
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand"
                ClientDataKeyNames="SpoolID" DataKeyNames="SpoolID" AllowFilteringByColumn="true" >

                <CommandItemTemplate>
                  
                    <div class="comandosEncabezado">
                        
                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkPlanchadoPrioridades" meta:resourcekey="lnkPlanchadoPrioridades"
                            CssClass="link" CommandName="Agregar" />
                        <samweb:AuthenticatedHyperLink runat="server" meta:resourcekey="imgPlanchadoPrioridades" ID="imgPlanchadoPrioridades"
                            ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado"  />

                        <samweb:AuthenticatedHyperLink runat="server" ID="lnkActualizaSpool" meta:resourcekey="lnkActualizaSpool"
                            CssClass="link" CommandName="Agregar" />
                        <samweb:AuthenticatedHyperLink runat="server" meta:resourcekey="imgActualizaSpool" ID="imgActualizaSpool"
                            ImageUrl="~/Imagenes/Iconos/agregar.png" CssClass="imgEncabezado"  />

                        <asp:LinkButton meta:resourcekey="lnkDocumentoAprobado" runat="server" ID="lnkDocumentoAprobado" CssClass="link" CommandName="DocumentoAprobado" />
                        <asp:ImageButton meta:resourcekey="imgDocumentoAprobado" runat="server" ID="imgDocumentoAprobado" CommandName="DocumentoAprobado" ImageUrl="~/Imagenes/Iconos/icono_certificar.png" CssClass="imgEncabezado" />
                                      
                        <asp:HyperLink meta:resourcekey="lnkAprobadoParaCruce" runat="server" ID="lnkAprobadoParaCruce" CssClass="link" />
                        <asp:HyperLink meta:resourcekey="imgAprobadoParaCruce" runat="server" ID="imgAprobadoParaCruce" ImageUrl="~/Imagenes/Iconos/icono_certificar.png" CssClass="imgEncabezado" />
                        
                        <asp:LinkButton meta:resourcekey="lnkPrioridadSeleccionados" runat="server" ID="lnkPrioridadSeleccionados" CommandName="PrioridadSeleccionados"  CssClass="link" />
                        <asp:ImageButton meta:resourcekey="imgPrioridadSeleccionados" runat="server" ID="imgPrioridadSeleccionados" CommandName="PrioridadSeleccionados"  ImageUrl="~/Imagenes/Iconos/icono_fijarprioridad.png" CssClass="imgEncabezado" />
                                            
                        <asp:HyperLink meta:resourcekey="lnkPrioridad" runat="server" ID="lnkPrioridad" CssClass="link" />
                        <asp:HyperLink meta:resourcekey="imgPrioridad" runat="server" ID="imgPrioridad" ImageUrl="~/Imagenes/Iconos/icono_fijarprioridad.png" CssClass="imgEncabezado" />      
                                
                        <asp:LinkButton runat="server" ID="lnkActualizarDatos" CommandName="ActualizarDatos" CssClass="link" OnClientClick="return Sam.Alerta(22);" meta:resourcekey="lnkActualizarDatos"  />
                        <asp:ImageButton runat="server" ID="imgActualizarDatos" CommandName="ActualizarDatos" ImageUrl="~/Imagenes/Iconos/actualizar.png" CssClass="imgEncabezado" OnClientClick="return Sam.Confirma(22);" meta:resourcekey="imgActualizarDatos"  />
            
                        <asp:LinkButton  runat="server" ID="lnkEliminacionMasiva"  CssClass="link"  meta:resourcekey="lnkEliminacionMasiva"  CommandName="EliminacionMasiva"  />
                        <asp:ImageButton  runat="server" ID="imgEliminacionMasiva" ImageUrl="~/Imagenes/Iconos/borrar.png" CommandName="EliminacionMasiva" CssClass="imgEncabezado" meta:resourcekey="imgBorrar"  />                                                        
                
                      
                    </div>
                </CommandItemTemplate>
                <Columns> 
                    <telerik:GridTemplateColumn UniqueName="chk_h" AllowFiltering="false" Groupable="false" ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                          <asp:CheckBox ID="rowChkBox" runat="server" OnCheckedChanged="ToggleRowSelection" 
                            AutoPostBack="True" />
                        </ItemTemplate>
                        <HeaderTemplate>                            
                            <asp:checkbox ID="headerChkbox" runat="server" OnCheckedChanged="ToggleSelectedState" 
                            AutoPostBack="True" />
                        </HeaderTemplate>
                    </telerik:GridTemplateColumn>
                    <%--<telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h" HeaderStyle-Width="30"/>--%>
                   <%-- <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="editar_h" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Imagenes/Iconos/editar.png" runat="server" ID="hypEditar"
                                meta:resourcekey="imgEditar" NavigateUrl="~/Materiales/DetRecepcion.aspx" ></asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn UniqueName="eliminar_h" AllowFiltering="false" Groupable="false"
                        ShowSortIcon="false" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imgBorrar" ImageUrl="~/Imagenes/Iconos/borrar.png"
                                CommandName="Borrar" CommandArgument='<%#Eval("SpoolID") %>' OnClientClick="return Sam.Confirma(1);"
                                meta:resourcekey="imgBorrar" CausesValidation="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="hlkHold_h" HeaderStyle-Width="47" meta:resourcekey="gbcHold" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Imagenes/Iconos/ico_holdB.png" runat="server" ID="hypPonerHold"
                                Visible="false" meta:resourcekey="hypPonerHold" />
                            <asp:HyperLink ImageUrl="~/Imagenes/Iconos/ico_quitarholdB.png" runat="server" ID="hypQuitarHold"
                                Visible="false" meta:resourcekey="hypQuitarHold" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn UniqueName="SpoolID" DataField="SpoolID" Visible="false" />
                    <telerik:GridBoundColumn UniqueName="Spool" DataField="Nombre" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcSpool" Visible="false" />
                    <telerik:GridTemplateColumn UniqueName="hypDetalle" meta:resourcekey="gbcSpool" AllowFiltering="true" 
                        Reorderable="false" Resizable="false" HeaderStyle-Width="150" FilterControlWidth="100" DataField="Nombre" SortExpression="Nombre">
                        <ItemTemplate>
                            <asp:HyperLink ID="hyDetalle" runat="server" Visible="true"></asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridCheckBoxColumn UniqueName="PendienteDocumental" DataField="PendienteDocumental"
                        HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDocumentoAprobado" />
                    <telerik:GridCheckBoxColumn UniqueName="AprobadoParaCruce" DataField="AprobadoParaCruce"
                        HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcAprobadoCruce" />
                    <telerik:GridBoundColumn UniqueName="Prioridad" DataField="Prioridad" HeaderStyle-Width="100"
                        FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcPrioridad" />
                    <telerik:GridBoundColumn UniqueName="Dibujo" DataField="Dibujo" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcDibujo" />
                     <telerik:GridBoundColumn UniqueName="FechaImportacion" DataField="FechaImportacion" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcFechaImportacion" />
                    <telerik:GridBoundColumn UniqueName="NumeroControl" DataField="NumeroControl" HeaderStyle-Width="130"
                        FilterControlWidth="90" Groupable="false" meta:resourcekey="gbcNumControl" />
                    <telerik:GridBoundColumn UniqueName="RevisionCliente" DataField="RevisionCliente"
                        HeaderStyle-Width="120" FilterControlWidth="80" Groupable="false" meta:resourcekey="gbcRevCliente" />
                    <telerik:GridBoundColumn UniqueName="Pdis" DataField="Pdis" HeaderStyle-Width="80" Aggregate="Sum" FooterAggregateFormatString="Total:<br />{0:F}"
                        DataFormatString="{0:N3}" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcPdi" />
                    <telerik:GridBoundColumn UniqueName="Peso" DataField="Peso" HeaderStyle-Width="80" Aggregate="Sum" FooterAggregateFormatString="Total:<br />{0:F}"
                        FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcKgs" />
                    <telerik:GridBoundColumn UniqueName="TotalPeq" DataField="TotalPeq" HeaderStyle-Width="80" Aggregate="Sum" FooterAggregateFormatString="Total:<br />{0:F}"
                        DataFormatString="{0:N3}" FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcTotalPeq" />
                    <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" HeaderStyle-Width="80"
                        FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcCedula" />
                    <telerik:GridBoundColumn UniqueName="Area" DataField="Area" HeaderStyle-Width="80" Aggregate="Sum" FooterAggregateFormatString="Total:<br />{0:F}"
                        FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcArea" />
                    <telerik:GridBoundColumn UniqueName="Especificacion" DataField="Especificacion" HeaderStyle-Width="100"
                        FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcEspecificacion" />
                    <telerik:GridBoundColumn UniqueName="DiametroPlano" DataField="DiametroPlano" HeaderStyle-Width="100"
                        DataFormatString="{0:N3}" FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcDiametroPlano" />
                    <telerik:GridBoundColumn UniqueName="DiametroMayor" DataField="DiametroMayor" HeaderStyle-Width="100"
                        DataFormatString="{0:N3}" FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcDiametroMayor" />
                    <telerik:GridBoundColumn UniqueName="Revision" DataField="RevisionSteelgo" HeaderStyle-Width="100"
                        FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcRevSteelGo" />
                    <telerik:GridBoundColumn UniqueName="PorcentajePnd" DataField="PorcentajePnd" HeaderStyle-Width="80"
                        FilterControlWidth="40" Groupable="false" meta:resourcekey="gbcPnd" />
                    <telerik:GridCheckBoxColumn UniqueName="RequierePwht" DataField="RequierePwht" HeaderStyle-Width="100"
                        FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcPwht" />
                    <telerik:GridCheckBoxColumn UniqueName="Confinado" DataField="Confinado" HeaderStyle-Width="80"
                        FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcConfinado" />
                    <telerik:GridBoundColumn UniqueName="Material" DataField="FamiliasAcero" HeaderStyle-Width="90"
                        FilterControlWidth="50" Groupable="false" meta:resourcekey="gbcMaterial" />
                    <telerik:GridCheckBoxColumn UniqueName="TieneHoldCalidad" DataField="TieneHoldCalidad"
                        HeaderStyle-Width="100" FilterControlWidth="70" Groupable="false" meta:resourcekey="gbcHoldCalidad" />
                    <telerik:GridCheckBoxColumn UniqueName="TieneHoldIngenieria" DataField="TieneHoldIngenieria"
                        HeaderStyle-Width="100" FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcHoldIngenieria" />
                    <telerik:GridBoundColumn UniqueName="ObservacionesHold" DataField="ObservacionesHold" HeaderStyle-Width="100"
                        FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcObservacionesHold" />
                    <telerik:GridBoundColumn UniqueName="FechaHold" DataField="FechaHold" HeaderStyle-Width="100"
                        FilterControlWidth="60" Groupable="false" meta:resourcekey="gbcFechaHold"  />
                    <%--<telerik:GridCheckBoxColumn UniqueName="EsRevision" DataField="EsRevision" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcEsRevision" />--%>
                    <telerik:GridBoundColumn UniqueName="Segmento1" DataField="Segmento1" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento1" />
                    <telerik:GridBoundColumn UniqueName="Segmento2" DataField="Segmento2" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento2" />
                    <telerik:GridBoundColumn UniqueName="Segmento3" DataField="Segmento3" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento3" />
                    <telerik:GridBoundColumn UniqueName="Segmento4" DataField="Segmento4" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento4" />
                    <telerik:GridBoundColumn UniqueName="Segmento5" DataField="Segmento5" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento5" />
                    <telerik:GridBoundColumn UniqueName="Segmento6" DataField="Segmento6" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento6" />
                    <telerik:GridBoundColumn UniqueName="Segmento7" DataField="Segmento7" HeaderStyle-Width="180"
                        FilterControlWidth="100" Groupable="false" Visible="false" meta:resourcekey="gbcSegmento7" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <%--<NestedViewTemplate>
                    <telerik:RadTabStrip runat="server" ID="tabStrip" MultiPageID="rmpDetalle" SelectedIndex="0">
                        <Tabs>
                            <telerik:RadTab runat="server" meta:resourcekey="rdMateriales"
                                Selected="True" />
                            <telerik:RadTab runat="server" meta:resourcekey="rdJuntas" />
                            <telerik:RadTab runat="server" meta:resourcekey="rdCortes" />
                        </Tabs>
                    </telerik:RadTabStrip>
                    <telerik:RadMultiPage ID="rmpDetalle" runat="server" SelectedIndex="0">
                        <telerik:RadPageView ID="rpvMateriales" runat="server" Selected="True">
                            <sam:DetMaterial ID="materiales" runat="server" />
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="rpvJuntas" runat="server">
                            <sam:DetJunta ID="juntas" runat="server" />
                        </telerik:RadPageView>
                        <telerik:RadPageView ID="rpvCortes" runat="server">
                            <sam:DetCorte ID="cortes" runat="server" />
                        </telerik:RadPageView>
                    </telerik:RadMultiPage>
                </NestedViewTemplate>--%>
            </MasterTableView>          
        </mimo:mimossradgrid>
        <telerik:radwindow runat="server" id="wndFijarPrioridad" modal="true" registerwithscriptmanager="true"
            visiblestatusbar="false" visibleonpageload="false">
            <ContentTemplate>
                <div class="fijarPrioridad">
                    <h4>
                        <asp:Literal runat="server" ID="litPrioridad" meta:resourcekey="litPrioridad" /></h4>
                    <div>
                        <div class="divIzquierdo ancho50">
                            <div class="separador">
                                <mimo:RequiredLabeledTextBox meta:resourcekey="lblPrioridad" runat="server" ID="txtPrioridad"
                                    MaxLength="3" ValidationGroup="vgPrioridad" />
                                <asp:RangeValidator meta:resourcekey="rngPrioridad" runat="server" ID="rngPrioridad"
                                    ControlToValidate="txtPrioridad" MinimumValue="0" MaximumValue="999" Type="Integer"
                                    ValidationGroup="vgPrioridad" Display="None" />
                            </div>
                        </div>
                        <div class="divIzquierdo ancho40">
                            <div class="validacionesRecuadro">
                                <div class="validacionesHeader">
                                    &nbsp;</div>
                                <div class="validacionesMain">
                                    <asp:ValidationSummary runat="server" ID="valPrioridad" ValidationGroup="vgPrioridad"
                                        DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valPrioridad" Width="160" />
                                </div>
                            </div>
                        </div>
                        <p>
                        </p>
                    </div>
                    <div class="separador">
                        <asp:Button meta:resourcekey="btnFijarPrioridad" runat="server" ID="btnFijarPrioridad"
                            ValidationGroup="vgPrioridad" CssClass="boton" OnClick="btnFijarPrioridad_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </telerik:radwindow>
               
        <telerik:radwindow runat="server" id="wndAprobadoParaCruce" modal="true" registerwithscriptmanager="true"
            visiblestatusbar="false" visibleonpageload="false">
                <ContentTemplate>
                    <div class="fijarPrioridad">
                        <h4>
                            <asp:Literal runat="server" ID="litAproabdoCruce" meta:resourcekey="litAproabdoCruce" /></h4>
                            <div>
                        <div class="divIzquierdo ancho50">
                            <div class="separador">
                                <asp:Label ID="lblAprobadoParaCruce" runat="server" CssClass="bold" meta:resourcekey="lblAprobadoParaCruce" />
                                <asp:CheckBox ID="chkAprobadoParaCruce" runat="server" meta:resourcekey="chkAprobadoParaCruce" />
                             </div>
                         </div>
                            <div class="divIzquierdo ancho40">
                            <div class="validacionesRecuadro">
                                <div class="validacionesHeader">
                                    &nbsp;</div>
                                <div class="validacionesMain">
                                    <asp:ValidationSummary runat="server" ID="ValidationSummary2" ValidationGroup="vgPrioridad"
                                        DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valPrioridad" Width="160" />
                                </div>
                            </div>
                        </div>
                        <p>
                        </p>
                        </div>
                        <div class="separador">
                            <asp:Button ID="btnAprobadoParaCruce" runat="server" OnClick="btnAprobadoParaCruce_Click"
                            CssClass="boton" meta:resourcekey="btnAprobadoParaCruce"   />
                        </div>
                        <div class="separador">
                        </div>
                    </div>
                </ContentTemplate>
            </telerik:radwindow>
        <div id="bntActualiza">
            <asp:Button runat="server" CausesValidation="false" ID="btnActualiza" CssClass="oculto"
                OnClick="btnActualiza_Click" />
        </div>
        <div id="btnWrapper" class="oculto">
            <asp:Button CssClass="oculto" runat="server" OnClick="lnkActualizar_OnClick" ID="btnRefresh"
                CausesValidation="false" />
        </div>
    </div>
</asp:Content>
