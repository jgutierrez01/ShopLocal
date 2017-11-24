<%@ Page  Language="C#" MasterPageFile="~/Masters/Calidad.Master" AutoEventWireup="true" CodeBehind="SeguimientoJuntas.aspx.cs" Inherits="SAM.Web.Calidad.SeguimientoJuntas" %>
<%@ MasterType VirtualPath="~/Masters/Calidad.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" tagname="Filtro" tagprefix="uc2" %>

<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" tagname="BarraTituloPagina" tagprefix="uc3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">

    <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>            
            <telerik:AjaxSetting AjaxControlID="RadFilter1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadFilter1" />
                    <telerik:AjaxUpdatedControl ControlID="grdSegJunta" LoadingPanelID="grdPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>            
            <telerik:AjaxSetting AjaxControlID="grdSegJunta">
                <UpdatedControls>                    
                    <telerik:AjaxUpdatedControl ControlID="grdSegJunta" LoadingPanelID="grdPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>            
        </AjaxSettings>
    </telerik:RadAjaxManager> 

    <uc3:BarraTituloPagina ID="barraTitulo" runat="server" NavigateUrl="~/Calidad/FiltrosSeguimientoJunta.aspx" meta:resourcekey="lblTitulo" />
    
    <div class="contenedorCentral">               
        
        <uc1:Header ID="proyHeader" runat="server" />  

        <div class="filterDiv cajaFiltros">
            <asp:Label meta:resourcekey="lblExpresionFiltro" ID="lblExpresionFiltro" runat="server"></asp:Label>
            <telerik:RadFilter runat="server" ID="RadFilter1" FilterContainerID="grdSegJunta" ShowApplyButton="true" OnPreRender="RadFilter1_PreRender" />
        </div>
        <p></p>
        
        <telerik:RadAjaxLoadingPanel runat="server" ID="grdPanel" />
            
        <mimo:MimossRadGrid ID="grdSegJunta" runat="server" OnNeedDataSource="grdSegJunta_OnNeedDataSource" OnItemCreated="grdSegJunta_ItemCreated"
            OnItemCommand="grdSegJunta_ItemCommand" OnItemDataBound="grdSegJunta_ItemDataBound" CssClass="RadGrid RadGrid_SAMOrange segJunta">
            <ClientSettings>
                <ClientEvents OnGridCreated="Sam.Calidad.GridCreated" />
            </ClientSettings>
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false" AllowFilteringByColumn="false">   
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <asp:HyperLink ID="hlExportarJunta" runat="server" meta:resourcekey="hlExportarJunta"></asp:HyperLink>
                        <asp:HyperLink runat="server" ID="hlExportaImagenJunta" ImageUrl="~/Imagenes/Iconos/excel.png"  AlternateText="ExportarExcelJunta" meta:resourcekey="imgExportarExcel"/>                        
                        <asp:LinkButton runat="server" ID="lnkActualizar" OnClick="lnkActualizar_onClick" CausesValidation="false"
                            meta:resourcekey="lnkActualizar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgActualizar" ImageUrl="~/Imagenes/IconoS/actualizar.png" CausesValidation="false"
                            OnClick="lnkActualizar_onClick" AlternateText="Actualizar" CssClass="imgEncabezado" meta:resourcekey="imgActualizar"/>
                    </div>                                        
                </CommandItemTemplate>             
                <Columns>        
                     <telerik:GridTemplateColumn AllowFiltering="false" Reorderable="false" Resizable="false"
                        UniqueName="linkPopUp" HeaderStyle-Width="30">
                        <ItemTemplate>
                            <asp:HyperLink ImageUrl="~/Imagenes/Iconos/info.png" runat="server" ID="hlPopUp"
                                meta:resourcekey="imgInventarios" NavigateUrl="#"></asp:HyperLink>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn UniqueName="General" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderGeneral" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoGeneral" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn UniqueName="Armado" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderArmado" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoArmado" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="Soldadura" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderSoldadura" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoSoldadura" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                        
                    <telerik:GridTemplateColumn UniqueName="InspeccionVisual" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderInspeccionVisual" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoInspeccionVisual" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="InspeccionDimensional" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderInspeccionDimensional" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoInspeccionDimensional" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                        
                    <telerik:GridTemplateColumn UniqueName="InspeccionEspesores" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderInspeccionEspesores" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoInspeccionEspesores" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="PruebaRT" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderPruebaRT" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoPruebaRT" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn UniqueName="PruebaPT" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderPruebaPT" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoPruebaPT" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn UniqueName="PruebaRTPostTT" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderPruebaRTPostTT" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoPruebaRTPostTT" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="PruebaPTPostTT" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderPruebaPTPostTT" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoPruebaPTPostTT" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                                        
                    <telerik:GridTemplateColumn UniqueName="PruebaUT" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderPruebaUT" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoPruebaUT" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
    
                    <telerik:GridTemplateColumn UniqueName="TratamientoPWHT" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderTratamientoPWHT" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoTratamientoPWHT" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                    <telerik:GridTemplateColumn UniqueName="TratamientoDurezas" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderTratamientoDurezas" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoTratamientoDurezas" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="TratamientoPreheat" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderTratamientoPreheat" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoTratamientoPreheat" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="Pintura" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderPintura" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoPintura" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn UniqueName="Embarque" AllowFiltering="false" Visible="false" Groupable="false" HeaderStyle-CssClass="rgHeader segJuntaHeader">
                        <HeaderTemplate>
                            <asp:Repeater ID="repHeaderEmbarque" runat="server" OnItemDataBound="repHeader_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <asp:Literal ID="litHeaderTitulo" runat="server" />
                                        </tr>
                                </HeaderTemplate>
                                
                                <ItemTemplate>
                                    <asp:Literal ID="litHeaderNombre" runat="server" />
                                </ItemTemplate>
                                
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>                                                        
                        </HeaderTemplate>

                        <ItemTemplate>
                            <asp:Repeater ID="repCampoEmbarque" runat="server" OnItemDataBound="repCampo_ItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Literal ID="litCampo" runat="server" />                                    
                                </ItemTemplate>

                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    
                </Columns>                
            </MasterTableView>
        </mimo:MimossRadGrid> 
               
    </div>

</asp:Content>
