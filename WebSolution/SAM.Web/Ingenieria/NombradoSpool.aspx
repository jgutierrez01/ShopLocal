<%@ Page  Language="C#" MasterPageFile="~/Masters/Ingenieria.master" AutoEventWireup="true"
    CodeBehind="NombradoSpool.aspx.cs" Inherits="SAM.Web.Ingenieria.NombradoSpool" %>
<%@ MasterType VirtualPath="~/Masters/Ingenieria.master" %>

<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="encabezadoProyecto"
    TagPrefix="ctrl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">   
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblNombradoSpool" CssClass="Titulo" meta:resourcekey="lblNombradoSpool"></asp:Label>
    </div>
    <div class="contenedorCentral">
        <div class="cajaFiltros">
            <div class="separador">
                <asp:Label runat="server" ID="lblProyecto" meta:resourcekey="lblProyecto" CssClass="bold labelHack" />
                <mimo:MappableDropDown runat="server" ID="ddlProyecto" AutoPostBack="True" meta:resourcekey="ddlProyectoResource1"
                    OnSelectedIndexChanged="ddlProyecto_SelectedIndexChanged" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator ID="reqProyecto" runat="server" ControlToValidate="ddlProyecto"
                    Display="None" InitialValue="" meta:resourcekey="reqProyecto"></asp:RequiredFieldValidator>
            </div>
            <p>
            </p>
        </div>
        <p>
        </p>
        <asp:Panel ID="pnlDatos" runat="server" Visible="false">
            <ctrl:encabezadoProyecto ID="proyEncabezado" runat="server" />
            <br />
            <div class="dashboardCentral">
                <div class="divIzquierdo ancho60">
                  
                          <div class="separador">
                            <asp:Label runat="server" meta:resourcekey="lblNumeroControl" ID="lblNumeroControl" CssClass="labelHack bold"></asp:Label>
                            <telerik:RadComboBox ID="radCmbNumeroControl" runat="server" Height="150px"
                                EnableLoadOnDemand="true"
                                ShowMoreResultsBox="true"
                                EnableVirtualScrolling="true"
                                OnClientItemsRequesting="Sam.Filtro.NumControlOnClientItemsRequestingEventHandler"
                                OnSelectedIndexChanged="radCmbNumeroControl_SelectedIndexChanged"
                                meta:resourcekey="radCmbNumeroControl"
                                CausesValidation="false"
                                AutoPostBack="true" 
                                AllowCustomText="true"
                                OnDataBound="radCmbNumeroControl_DataBound"  >
                                <WebServiceSettings Method="ListaNumerosControlPorUserScope" Path="~/WebServices/ComboboxWebService.asmx" />
                            </telerik:RadComboBox>   
                         </div>          
                        <div class="separador">
                            <asp:Label runat="server" ID="lblSpool" meta:resourcekey="lblSpool" CssClass="bold" />                         
                            <br />
                            <telerik:RadComboBox ID="rcbSpool" runat="server" Width="200px" Height="150px" EmptyMessage=" "
                                EnableLoadOnDemand="true" ShowMoreResultsBox="true" EnableVirtualScrolling="true"
                                OnClientItemsRequesting="Sam.WebService.AgregaProyectoID" OnSelectedIndexChanged="rcbSpool_SelectedIndexChanged" style="height: 16px">
                                <WebServiceSettings Method="ListaSpoolsPorProyecto" Path="~/WebServices/ComboboxWebService.asmx" />
                            </telerik:RadComboBox><span class="required">*</span>                            
                            <asp:CustomValidator    
                            meta:resourcekey="valSpool"
                            runat="server" 
                            ID="valSpool" 
                           
                            ControlToValidate="rcbSpool" 
                            ValidateEmptyText="true"                         
                            ClientValidationFunction="Sam.Utilerias.ValidacionComboTelerikTieneValor"
                            OnServerValidate="cusRcbSpool_ServerValidate" />      
                   
                        </div>
                
                        <div class="separador">
                            <asp:Label ID="lblNuevoNombre" runat="server" CssClass="bold" meta:resourcekey="lblNuevo" />
                            <br />
                           <asp:TextBox ID="txtNuevo" runat="server" meta:resourcekey="txtNombreNuevo" />
                        </div>
                        <div class="separador">
                            <asp:Button runat="server" ID="btnGuardar" Text="Guardar" meta:resourcekey="btnGuardar"
                                CssClass="divIzquierdo boton" OnClick="btnGuardar_Click" />
                                <asp:Button runat="server" ID="btnEditar" Text="Editar" meta:resourcekey="btnEditar"
                                 CssClass=" divIzquierdo boton" onclick="btnEditar_Click" />
                        </div>
                    
              </div>
               
               <div class="divDerecho ancho40">
                   <div class="validacionesRecuadro">
                        <div class="validacionesHeader">
                        </div>
                        <div class="validacionesMain">
                            <asp:ValidationSummary runat="server" ID="valSummary" meta:resourcekey="valSummaryResource1"
                                CssClass="summary" />
                        </div>
                    </div> 
               </div> 
                <p>
                </p>
            </div>
            <asp:TextBox ID="txtMensajeError" runat="server" meta:resourcekey="txtMensajeErrorN" Visible="false" />
        </asp:Panel>
    </div>
</asp:Content>
