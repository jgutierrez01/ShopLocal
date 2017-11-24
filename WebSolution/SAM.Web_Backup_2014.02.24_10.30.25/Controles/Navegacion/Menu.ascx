<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="SAM.Web.Controles.Navegacion.Menu" %>
<div class="menuSuperior">
    <asp:Panel runat="server" ID="pnDashboard" CssClass="Inicial">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/Dashboard/DashDefault.aspx"
            ID="hlDashboard" meta:resourcekey="hlDashboard"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnWorkstatus" CssClass="Elemento">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/WorkStatus/WkStDefault.aspx"
            ID="hlWorkstatus" meta:resourcekey="hlWorkstatus"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnMateriales" CssClass="Elemento">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/Materiales/MatDefault.aspx"
            ID="hlMateriales" meta:resourcekey="hlMateriales"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnIngenieria" CssClass="Elemento">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/Ingenieria/IngDefault.aspx"
            ID="hlIngenieria" meta:resourcekey="hlIngenieria"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnProduccion" CssClass="Elemento">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/Produccion/ProdDefault.aspx"
            ID="hlProduccion" meta:resourcekey="hlProduccion"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnCalidad" CssClass="Elemento">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/Calidad/CalidadDefault.aspx"
            ID="hlCalidad" meta:resourcekey="hlCalidad"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnProyecto" CssClass="Elemento">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/Proyectos/ProyDefault.aspx"
            ID="hlProyecto" meta:resourcekey="hlProyecto"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnCatalogo" CssClass="Elemento">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/Catalogos/CatDefault.aspx"
            ID="hlCatalogo" meta:resourcekey="hlCatalogo"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnAdministracion" CssClass="ElementoEspecial">
        <asp:HyperLink runat="server" Target="_self" NavigateUrl="~/Administracion/AdminDefault.aspx"
            ID="hlAdministracion" meta:resourcekey="hlAdministracion"></asp:HyperLink>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnRellenado" CssClass="ElementoEspecial" Style="width: 100;">
        <span>&nbsp;&nbsp;&nbsp;</span>
    </asp:Panel>
    <%--<div class="Cierre">
    </div>--%>
    <asp:HyperLink ID="hlIconoPendiente" ImageUrl="~/Imagenes/Iconos/taskReportWithActive.png" NavigateUrl="~/Administracion/LstPendientes.aspx"
        runat="server" CssClass="ElementoFlotante" meta:resourcekey="hlIconoPendiente"></asp:HyperLink>
</div>
<p style="clear: both; height: 0px; margin: 0px; display: none;">
</p>
