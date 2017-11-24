<%@ Page  Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpDetalleCorte.aspx.cs" Inherits="SAM.Web.WorkStatus.PopUpDetalleCorte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h4>
        <asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
    </h4>
    <div class="cajaAzul">
        <div class="divIzquierdo ancho40">
            <div class="separadorDashboard">
                <asp:Label ID="lblNumeroUnicoTitulo" runat="server" CssClass="bold" meta:resourcekey="lblNumeroUnicoTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblNumeroUnico" runat="server" meta:resourcekey="lblNumeroUnicoResource1"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblSobranteTitulo" runat="server" CssClass="bold" meta:resourcekey="lblSobranteTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblSobrante" runat="server" meta:resourcekey="lblSobranteResource1"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblMermaTitulo" runat="server" CssClass="bold" meta:resourcekey="lblMermaTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblMerma" runat="server" meta:resourcekey="lblMermaResource1"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblEstatusTitulo" runat="server" CssClass="bold" meta:resourcekey="lblEstatusTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblEstatus" runat="server" meta:resourcekey="lblEstatusResource1"></asp:Label>
            </div>
        </div>
        <div class="divIzquierdo ancho60">
            <div class="separadorDashboard">
                <asp:Label ID="lblItemCodeTitulo" runat="server" CssClass="bold" meta:resourcekey="lblItemCodeTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblItemCode" runat="server" meta:resourcekey="lblItemCodeResource1"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblDescripcionTitulo" runat="server" CssClass="bold" meta:resourcekey="lblDescripcionTitulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDescripcion" runat="server" meta:resourcekey="lblDescripcionResource1"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblDiametro1Titulo" runat="server" CssClass="bold" meta:resourcekey="lblDiametro1Titulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDiametro1" runat="server" meta:resourcekey="lblDiametro1Resource1"></asp:Label>
            </div>
            <div class="separadorDashboard">
                <asp:Label ID="lblDiametro2Titulo" runat="server" CssClass="bold" meta:resourcekey="lblDiametro2Titulo"></asp:Label>&nbsp;
                <asp:Label ID="lblDiametro2" runat="server" meta:resourcekey="lblDiametro2Resource1"></asp:Label>
            </div>
        </div>
        <p>
        </p>
    </div>
    <p>
    </p>
   <asp:ValidationSummary meta:resourcekey="valSummary" runat="server" ID="valSummary"
            CssClass="summaryList" />
    <div class="infoJuntas center">
        <table class="repSam" cellpadding="0" cellspacing="0">
            <colgroup>
                <col width="20" />
                <col width="150" />
                <col width="150" />
                <col width="150" />
                <col width="150" />
                <col width="150" />
                <col width="150" />
                <col width="150" />
            </colgroup>
            <thead>
             <tr class="repEncabezado">
                <th colspan="8"></th>
            </tr>
                <tr class="repTitulos">
                    <th>
                    </th>
                    <th>
                        <asp:Literal runat="server" ID="litFecha" meta:resourcekey="litFecha" />
                    </th>
                    <th>
                        <asp:Literal runat="server" ID="litMaquina" meta:resourcekey="litMaquina" />
                    </th>
                    <th>
                        <asp:Literal runat="server" ID="litNumeroControl" meta:resourcekey="litNumeroControl" />
                    </th>
                    <th>
                        <asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" />
                    </th>
                    <th>
                        <asp:Literal runat="server" ID="litCantidadRequerida" meta:resourcekey="litCantidadRequerida" />
                    </th>
                    <th>
                        <asp:Literal runat="server" ID="litCantidadReal" meta:resourcekey="litCantidadReal" />
                    </th>
                     <th>
                        <asp:Literal runat="server" ID="litEstatus" meta:resourcekey="litEstatus" />
                    </th>
                </tr>
            </thead>
            <asp:Repeater runat="server" ID="repCortesDetalle" 
                onitemcommand="repCortesDetalle_ItemCommand" 
                onitemdatabound="repCortesDetalle_ItemDataBound" >
                <ItemTemplate>
                    <tr class="repFila">
                        <td>
                            <asp:ImageButton runat="server" ID="imgCancelar" meta:resourcekey="imgCancelar" ImageUrl="~/Imagenes/Iconos/borrar.png"
                                CommandName="cancelar" CommandArgument='<%#Eval("CorteDetalleID") %>' OnClientClick="return Sam.Confirma(24);" />
                        </td>
                        <td>
                            <%#Eval("Fecha", "{0:d}")%>
                        </td>
                        <td>
                            <%#Eval("Maquina")%>
                        </td>
                        <td>
                            <%#Eval("NumeroControl")%>
                        </td>
                        <td>
                            <%#Eval("Etiqueta")%>
                        </td>
                        <td>
                            <%#Eval("CantidadRequerida")%>
                        </td>
                        <td>
                            <%#Eval("CantidadReal")%>
                        </td>
                         <td>
                            <%#Eval("Estatus")%>
                        </td>
                    </tr>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr class="repFilaPar">
                        <td>
                            <asp:ImageButton runat="server" ID="imgCancelar" meta:resourcekey="imgCancelar" ImageUrl="~/Imagenes/Iconos/borrar.png"
                                CommandName="cancelar" CommandArgument='<%#Eval("CorteDetalleID") %>' OnClientClick="return Sam.Confirma(24);" />
                        </td>
                        <td>
                            <%#Eval("Fecha","{0:d}")%>
                        </td>
                        <td>
                            <%#Eval("Maquina")%>
                        </td>
                        <td>
                            <%#Eval("NumeroControl")%>
                        </td>
                        <td>
                            <%#Eval("Etiqueta")%>
                        </td>
                        <td>
                            <%#Eval("CantidadRequerida")%>
                        </td>
                        <td>
                            <%#Eval("CantidadReal")%>
                        </td>
                         <td>
                            <%#Eval("Estatus")%>
                        </td>
                    </tr>
                </AlternatingItemTemplate>
            </asp:Repeater>
            <tfoot>
                <tr class="repPie">
                    <td colspan="8">
                        &nbsp;
                    </td>
                </tr>
            </tfoot>
        </table>
    </div>
    <p></p>
</asp:Content>
