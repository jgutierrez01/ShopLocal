<%@ Page  Language="C#" MasterPageFile="~/Masters/Catalogos.master" AutoEventWireup="true"
    CodeBehind="CatDefault.aspx.cs" Inherits="SAM.Web.Catalogos.CatDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadInner" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBodyInner" runat="server">
    <div class="headerDashboard">
        <asp:Label runat="server" ID="lblDashboard" meta:resourcekey="lblDashboard" Text="Catálogo" />
    </div>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
        <tr>
            <td>
                <div id="dashBoard">
                    <div class="divIzquierdo bordeDerecho">
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstAcero.aspx" meta:resourcekey="hlAceros" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstCedula.aspx" meta:resourcekey="hlCedula" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstCliente.aspx" meta:resourcekey="hlClientes" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstConsumibles.aspx" meta:resourcekey="hlConsumibles" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstDefectos.aspx" meta:resourcekey="hlDefectos" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstDiametro.aspx" meta:resourcekey="hlDiametro" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink ID="AuthenticatedHyperLink1" runat="server" NavigateUrl="TblEspesores.aspx" meta:resourcekey="hlTablaEspesores" />
                        </div>
                    </div>
                    <div class="divIzquierdo bordeDerecho">
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink ID="AuthenticatedHyperLink2" runat="server" NavigateUrl="LstFabricantes.aspx" meta:resourcekey="hlFabricantes" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstFamAcero.aspx" meta:resourcekey="hlFamAceros" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstFamiliaMaterial.aspx" meta:resourcekey="hlFamMateriales" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstProyecto.aspx" meta:resourcekey="hlItemCodes" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstProyecto.aspx" meta:resourcekey="hlItemCodesEquivalentes" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstPatio.aspx" meta:resourcekey="hlLocalizacion" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstPatio.aspx" meta:resourcekey="hlMaquinasCorte" />
                        </div>
                    </div>
                    <div class="divIzquierdo bordeDerecho">
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstPatio.aspx" meta:resourcekey="hlPatios" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="KgTeoricos.aspx" meta:resourcekey="hlTablaKgTeoricos" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstProcesoRaiz.aspx" meta:resourcekey="hlProcesoRaiz" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstProcesoRelleno.aspx" meta:resourcekey="hlProcesoRelleno" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstProveedor.aspx" meta:resourcekey="hlProveedores" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstProyecto.aspx" meta:resourcekey="hlProyectos" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstPeq.aspx" meta:resourcekey="hlTablaPeq" />
                        </div>
                    </div>
                    <div class="divIzquierdo bordeDerecho">
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink ID="AuthenticatedHyperLink4" runat="server" NavigateUrl="LstSoldador.aspx" meta:resourcekey="hlSoldadores" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstTipoCorte.aspx" meta:resourcekey="hlTiposCorte" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstTipoJunta.aspx" meta:resourcekey="hlTiposJunta" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstTransportistas.aspx" meta:resourcekey="hlTransportistas" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstTubero.aspx" meta:resourcekey="hlTuberos" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink ID="AuthenticatedHyperLink3" runat="server" NavigateUrl="LstWps.aspx" meta:resourcekey="hlWps" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstInspector.aspx" meta:resourcekey="hlInspector" />
                        </div>
                    </div>
                    <div class="divIzquierdo bordeDerecho">
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstCortador.aspx" meta:resourcekey="hlCortador" />
                        </div>
                        <div class="Elemento">
                            <samweb:AuthenticatedHyperLink runat="server" NavigateUrl="LstDespachador.aspx" meta:resourcekey="hlDespachador" />
                        </div>                                              
                    </div>
                    <p>
                    </p>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
