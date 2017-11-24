<%@ Page  Language="C#" MasterPageFile="~/Masters/Calidad.Master" AutoEventWireup="true" CodeBehind="FiltrosSeguimientoSpool.aspx.cs" Inherits="SAM.Web.Calidad.FiltrosSeguimientoSpool" %>
<%@ MasterType VirtualPath="~/Masters/Calidad.Master" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" tagname="Header" tagprefix="uc1" %>
<%@ Register Src="~/Controles/Proyecto/FiltroGenerico.ascx" tagname="Filtro" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">

    <telerik:RadAjaxManager ID="radManager" runat="server">
        <AjaxSettings>                        
            <telerik:AjaxSetting AjaxControlID="filtroGenerico">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                    <telerik:AjaxUpdatedControl ControlID="proyHeader" />
                </UpdatedControls>
            </telerik:AjaxSetting>            
            <telerik:AjaxSetting AjaxControlID="pnlBarConfig">
                <UpdatedControls>                    
                    <telerik:AjaxUpdatedControl ControlID="pnlBarModulos"/>
                    <telerik:AjaxUpdatedControl ControlID="pnlBarConfig"/>
                    <telerik:AjaxUpdatedControl ControlID="filtroGenerico" />
                </UpdatedControls>
            </telerik:AjaxSetting>            
        </AjaxSettings>
    </telerik:RadAjaxManager> 

    <div class="paginaHeader">
        <asp:Label ID="lblTitulo" runat="server" CssClass="Titulo" meta:resourcekey="lblTitulo"></asp:Label>
    </div>

    <div class="contenedorCentral">        
        <div class="cajaFiltros">

           <uc2:Filtro          
                ProyectoRequerido="true"       
                FiltroNumeroUnico="false" 
                ProyectoHeaderID="proyHeader"                
                ProyectoAutoPostBack="true"
                NumeroControlAutoPostBack="true"                
                OrdenTrabajoAutoPostBack="true"
                FiltroCuadrante="false"
                NumeroControlWidth="200px"
                OrdenTrabajoWidth="200px"           
                runat="server"
                ID="filtroGenerico"></uc2:Filtro>
    
            <div class="divIzquierdo" style="padding-top:20px;">                                
                <asp:CheckBox ID="chkEmbarcados" meta:resourcekey="chkEmbarcados" runat="server"  CssClass="checkYTexto"/>                
            </div>
            <p> </p>
        </div>

        <p> </p>
        <uc1:Header ID="proyHeader" runat="server"  Visible="false"/>  

        <div class="ancho100">
            <asp:ValidationSummary runat="server" ID="valSummary" EnableClientScript="true" DisplayMode="BulletList" class="summaryList" meta:resourcekey="valSummary" />
        </div>

        <p> </p>


         <telerik:RadPanelBar ID="pnlBarModulos" runat="server" CssClass="calidadRadPanelBar" Width="100%" ExpandMode="MultipleExpandedItems">
            <Items>
                <telerik:RadPanelItem Value="Aceros" Text="Modulos" CssClass="ancho100" Selected="true"  meta:resourcekey="radPnlModulos">
                    <ContentTemplate> 

                        <div class="ancho100">
                            <asp:Repeater ID="repModulos"  runat="server" OnItemDataBound="repModulos_OnItemDataBound">
                                <ItemTemplate>
                                    <div class="modulo">
                                        <div class="titulo">
                                            <asp:Label runat="server" ID="lblModulo" />
                                            <asp:CheckBox runat="server" ID="chkTitulo" onclick="Sam.Calidad.ToggleLista(this);" />
                                        </div>
                                        <div class="lista">
                                            <asp:CheckBoxList runat="server" ID="chkModulos" RepeatLayout="Table" />
                                        </div>
                                    </div>
                                </ItemTemplate>                
                            </asp:Repeater>
                        </div>
                        
                    </ContentTemplate>
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelBar>

        <p> </p>

        <telerik:RadPanelBar ID="pnlBarConfig" runat="server" CssClass="calidadRadPanelBar" Width="100%" ExpandMode="MultipleExpandedItems">
            <Items>
                <telerik:RadPanelItem Value="Nada" Text="Mis Configuraciones"   Selected="true" meta:resourcekey="radPnlMisConfig">
                    <ContentTemplate> 

                        <div class="cajaFiltros">
                             <div class="divIzquierdo" style="padding-top:18px;">
                                    <asp:Label CssClass="bold" ID="lblNombre" meta:resourcekey="lblNombre" runat="server" ></asp:Label>
                             </div>
                            <div class="divIzquierdo" style="padding-top:16px;">
                                    <asp:TextBox ID="txtNombre" meta:resourcekey="txtNombre" runat="server" />
                            </div>
                            <div class="divIzquierdo">
                                <div class="separador">
                                    <asp:Button CssClass="boton" ID="btnGuardar" meta:resourcekey="btnGuardar" CausesValidation="false" OnClick="btnGuardar_onClick" runat="server" />
                                 </div>              
                            </div>
                            <p> </p>

                            <div class="divIzquierdo" style="padding-top:18px;">
                                    <asp:Label CssClass="bold" ID="lblMisReportes" meta:resourcekey="lblMisReportes" runat="server" ></asp:Label>
                             </div>
                            <div class="divIzquierdo" style="padding-top:16px;">
                                    <mimo:MappableDropDown runat="server" ID="ddlMisReportes"  AutoPostBack="false" />
                            </div>
                            <div class="divIzquierdo">
                                <div class="separador">
                                    <asp:Button CssClass="boton" ID="btnCargar" meta:resourcekey="btnCargar"  OnClick="btnCargar_onClick" CausesValidation="false" runat="server" />
                                 </div>              
                            </div>
                             <div class="divIzquierdo">
                                <div class="separador">
                                    <asp:Button CssClass="boton" ID="btnEliminar" meta:resourcekey="btnEliminar"  OnClick="btnEliminar_onClick" CausesValidation="false" runat="server" />
                                 </div>              
                            </div>
                            <p> </p>
                        </div>               
                        
                    </ContentTemplate>
                </telerik:RadPanelItem>
            </Items>
        </telerik:RadPanelBar>

        

          <div class="center">
                    <asp:Button CssClass="boton" ID="btnMostrar" OnClick="btnMostrar_onClick" meta:resourcekey="btnMostrar" runat="server"/>
           </div>
     <p> </p>
    </div>  
</asp:Content>
