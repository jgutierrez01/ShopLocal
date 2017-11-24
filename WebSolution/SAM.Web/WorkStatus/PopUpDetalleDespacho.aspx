<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpDetalleDespacho.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpDetalleDespacho"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
    </h4>
    <div class="cajaAzul">
        <div class="divIzquierdo ancho50">
            <div class="separadorDashboard">
                <asp:Label ID="lblFechaTitulo" runat="server" CssClass="bold" meta:resourcekey="lblFechaTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblFecha" runat="server" meta:resourcekey="lblFechaResource1" ></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblEtiquetaTitulo" runat="server" CssClass="bold" meta:resourcekey="lblEtiquetaTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblEtiqueta" runat="server" 
                    meta:resourcekey="lblEtiquetaResource1"></asp:Label>
            </div>
            <div class="separadorDashboard">
                &nbsp;                
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblItemCodeIngTitulo" runat="server" CssClass="bold" meta:resourcekey="lblItemCodeIngTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblItemCodeIng" runat="server" 
                    meta:resourcekey="lblItemCodeIngResource1" ></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblDescripcionIngTitulo" runat="server" CssClass="bold" meta:resourcekey="lblDescripcionIngTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDescripcionIng" runat="server" 
                    meta:resourcekey="lblDescripcionIngResource1" ></asp:Label>
            </div>
             <div class="separadorDashboard">
                <asp:Label ID="lblDiametro1IngTitulo" runat="server" CssClass="bold" meta:resourcekey="lblDiametro1IngTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDiametro1Ing" runat="server" 
                     meta:resourcekey="lblDiametro1IngResource1"></asp:Label>
            </div>
             <div class="separadorDashboard">
                <asp:Label ID="lblDiametro2IngTitulo" runat="server" CssClass="bold" meta:resourcekey="lblDiametro2IngTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDiametro2Ing" runat="server" 
                     meta:resourcekey="lblDiametro2IngResource1" ></asp:Label>
            </div>
             <div class="separadorDashboard">
                <asp:Label ID="lblCantidadRequeridaTitulo" runat="server" CssClass="bold" meta:resourcekey="lblCantidadRequeridaTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblCantidadRequerida" runat="server" 
                     meta:resourcekey="lblCantidadRequeridaResource1" ></asp:Label>
            </div>
        </div>
        <div class="divIzquierdo ancho50">
           <div class="separadorDashboard">
                <asp:Label ID="lblEstatusTitulo" runat="server" CssClass="bold" meta:resourcekey="lblEstatusTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblEstatus" runat="server" 
                    meta:resourcekey="lblEstatusResource1" ></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblNumeroUnicoTitulo" runat="server" CssClass="bold" meta:resourcekey="lblNumeroUnicoTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblNumeroUnico" runat="server" 
                    meta:resourcekey="lblNumeroUnicoResource1"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Checkbox ID="chkEsEquivalente" runat="server" CssClass="checkBold" meta:resourcekey="chkEsEquivalente" Enabled="false" />
            <div class="separadorDashboard">
                <asp:Label ID="lblItemCodeTitulo" runat="server" CssClass="bold" meta:resourcekey="lblItemCodeIngTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblItemCode" runat="server" 
                    meta:resourcekey="lblItemCodeResource1" ></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblDescripcionTitulo" runat="server" CssClass="bold" meta:resourcekey="lblDescripcionIngTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDescripcion" runat="server" 
                    meta:resourcekey="lblDescripcionResource1" ></asp:Label>
            </div>
             <div class="separadorDashboard">
                <asp:Label ID="lblDiametro1Titulo" runat="server" CssClass="bold" meta:resourcekey="lblDiametro1IngTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDiametro1" runat="server" 
                     meta:resourcekey="lblDiametro1Resource1"></asp:Label>
            </div>
             <div class="separadorDashboard">
                <asp:Label ID="lblDiametro2Titulo" runat="server" CssClass="bold" meta:resourcekey="lblDiametro2IngTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDiametro2" runat="server" 
                     meta:resourcekey="lblDiametro2Resource1" ></asp:Label>
            </div>
             <div class="separadorDashboard">
                <asp:Label ID="lblCantidadDespachadaTitulo" runat="server" CssClass="bold" meta:resourcekey="lblCantidadDespachadaTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblCantidadDespachada" runat="server" 
                     meta:resourcekey="lblCantidadDespachadaResource1" ></asp:Label>
            </div>
        </div>
        <p>
        </p>
    </div>
    <p>
    </p>
</asp:Content>

