<%@ Page Language="C#" MasterPageFile="~/Masters/Proyectos.master" AutoEventWireup="true"
    CodeBehind="DetProyecto.aspx.cs" Inherits="SAM.Web.Proyectos.DetProyecto" %>

<%@ MasterType VirtualPath="~/Masters/Proyectos.master" %>
<%@ Register Src="~/Controles/Navegacion/BarraTituloPagina.ascx" TagName="BarraTituloPagina"
    TagPrefix="sam" %>
<%@ Register Src="~/Controles/Proyecto/Header.ascx" TagName="Header" TagPrefix="sam" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <sam:Header ID="headerProyecto" runat="server" />
    <sam:BarraTituloPagina runat="server" ID="titulo" meta:resourcekey="lblTitulo" NavigateUrl="~/Proyectos/ProyDefault.aspx" />
    <div class="contenedorCentral">
        <div class="dashboardCentral">
            <div class="divIzquierdo">
                <div class="headerCajaEspecial">
                    <asp:Literal ID="litInfoProyecto" runat="server" meta:resourcekey="litInfoProyecto"></asp:Literal>
                </div>
                <div class="dashboardElement cajaAzul">
                    <div class="divDerecho" style="margin-top: 2px">
                        <samweb:AuthenticatedHyperLink runat="server" ID="imgEditar" ImageUrl="~/Imagenes/Iconos/editar.png"></samweb:AuthenticatedHyperLink>
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlEditar" meta:resourcekey="hlEditar"
                            CssClass="bold" />
                    </div>
                    <p>
                    </p>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblClienteTexto" meta:resourcekey="lblClienteTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblCliente"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblFechaInicioTexto" meta:resourcekey="lblFechaInicioTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblFechaInicio"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblPatioTexto" meta:resourcekey="lblPatioTexto" CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblPatio"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblColorTexto" meta:resourcekey="lblColorTexto" CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblColor"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblCodigoNumUnicoTexto" meta:resourcekey="lblCodigoNumUnicoTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblCodigoNumUnico"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblCodigoOdtTexto" meta:resourcekey="lblCodigoOdtTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblCodigoOdt"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblDigitoNumUnicoTexto" meta:resourcekey="lblDigitoNumUnicoTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblDigitoNumUnico"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblDigitoOdtTexto" meta:resourcekey="lblDigitoOdtTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblDigitoOdt"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="divIzquierdo">
                <div class="headerCajaEspecial">
                    <asp:Literal ID="litInfoContacto" runat="server" meta:resourcekey="lblInformacionContacto"></asp:Literal>
                </div>
                <div class="dashboardElement cajaAzul">
                    <br />
                    <br />
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblNombreTexto" meta:resourcekey="lblNombreTexto" CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblNombre"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblApellidosTexto" meta:resourcekey="lblApellidosTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblApellidos"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblTelefonoTexto" meta:resourcekey="lblTelefonoTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblTelefono"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblTelefonoOficinaTexto" meta:resourcekey="lblTelefonoOficinaTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblTelefonoOficina"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblTelefonoCelularTexto" meta:resourcekey="lblTelefonoCelularTexto"
                            CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblTelefonoCelular"></asp:Label>
                    </div>
                    <div class="separadorDashboard">
                        <asp:Label runat="server" ID="lblCorreoTexto" meta:resourcekey="lblCorreoTexto" CssClass="bold"></asp:Label>
                        <asp:Label runat="server" ID="lblCorreo"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="divDerecho">
                <div class="headerCajaEspecial">
                    <asp:Literal ID="litLinks" runat="server" meta:resourcekey="litLinks"></asp:Literal>
                </div>
                <div class=" dashboardElementLeft cajaAzul">
                <p></p>
                    <div class="separadorDashboard linkDashboard">
                    <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlProveedores" meta:resourcekey="hlProveedores" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                    <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlFabricantes" meta:resourcekey="hlFabricantes" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                    <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlTransportistas" meta:resourcekey="hlTransportistas" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                    <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlItemCodes" meta:resourcekey="hlItemCodes" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                    <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlIcEquivalentes" meta:resourcekey="hlIcEquivalentes" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                    <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlColadas" meta:resourcekey="hlColadas" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                    <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlWps" meta:resourcekey="hlWps" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                    <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlDossierCalidad" meta:resourcekey="hlDossierCalidad" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                    <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlTipoReporteProyecto" meta:resourcekey="hlTipoReporteProyecto" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                        <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlPendientesAutomaticos" meta:resourcekey="hlPendientesAutomaticos" />
                    </div>
                    <div class="separadorDashboard linkDashboard">
                        <img src="../Imagenes/Menu/bltGris.png" alt="" />
                        <samweb:AuthenticatedHyperLink runat="server" ID="hlProgramacion" meta:resourcekey="hlProgramacion" />
                    </div>
                </div>
        </div>
        <p>
        </p>
    </div>
    </div>
</asp:Content>
