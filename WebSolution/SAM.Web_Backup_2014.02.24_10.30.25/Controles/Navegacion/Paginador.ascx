<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Paginador.ascx.cs" Inherits="SAM.Web.Controles.Navegacion.Paginador" %>
<div class="paginador">
    <div class="botonerPaginador">
        <div class="previos">
            <asp:Button runat="server" ID="btnPrimerPagina" OnCommand="lnk_Comando" CommandName="primera" UseSubmitBehavior="False" CssClass="boton priBtn" meta:resourcekey="btnPrimerPagina" />
            <asp:Button runat="server" ID="btnPaginaAnterior" OnCommand="lnk_Comando" CommandName="anterior" UseSubmitBehavior="False" CssClass="boton prevBtn" meta:resourcekey="btnPaginaAnterior" />
        </div>
        <div class="paginas">
            <asp:LinkButton runat="server" ID="lnkPrevios" OnCommand="lnk_Comando" CommandName="bloquePrevio" CssClass="prevs" Text="..." meta:resourcekey="lnkPrevios" />
            <asp:PlaceHolder runat="server" ID="phHolder">
                <asp:LinkButton runat="server" ID="lnkPos1" OnCommand="lnk_Comando" CommandName="pos" />
                <asp:LinkButton runat="server" ID="lnkPos2" OnCommand="lnk_Comando" CommandName="pos" />
                <asp:LinkButton runat="server" ID="lnkPos3" OnCommand="lnk_Comando" CommandName="pos" />
                <asp:LinkButton runat="server" ID="lnkPos4" OnCommand="lnk_Comando" CommandName="pos" />
                <asp:LinkButton runat="server" ID="lnkPos5" OnCommand="lnk_Comando" CommandName="pos" />
            </asp:PlaceHolder>
            <asp:LinkButton runat="server" ID="lnkSiguientes" OnCommand="lnk_Comando" CommandName="bloqueSiguiente" CssClass="sigs" Text="..." meta:resourcekey="lnkSiguientes"/>
        </div>
        <div class="siguientes">
            <asp:Button runat="server" ID="btnPaginaSiguiente" OnCommand="lnk_Comando" CommandName="siguiente" UseSubmitBehavior="False" CssClass="boton sigBtn" meta:resourcekey="btnPaginaSiguiente" />
            <asp:Button runat="server" ID="btnUltimaPagina" OnCommand="lnk_Comando" CommandName="ultima" UseSubmitBehavior="False" CssClass="boton ultBtn" meta:resourcekey="btnUltimaPagina" />
        </div>
    </div>
    <div class="leyendaPaginador">
        <asp:Literal runat="server" ID="ltLeyendaPaginador" />
    </div>
</div>