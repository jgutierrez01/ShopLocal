<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true"
    CodeBehind="PopUpFijarRevIngSeleccionados.aspx.cs" Inherits="SAM.Web.Ingenieria.PopUpFijarRevIngSeleccionados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="fijarDocumento">
                    <h4>
                        <asp:Literal runat="server" ID="litDocumento" meta:resourcekey="litDocumento" /></h4>
                    <div>
                        <div class="divIzquierdo ancho50">
                            <div class="separador">
                                 <asp:Label ID="lblDocumentoAprobado" runat="server" CssClass="bold" meta:resourcekey="lblDocumentoAprobado" />
                                 <asp:CheckBox ID="chkDocumentoAprobado" runat="server" meta:resourcekey="chkDocumentoAprobado" />
                            </div>
                        </div>
                        <div class="divIzquierdo ancho40">
                            <div class="validacionesRecuadro">
                                <div class="validacionesHeader">
                                    &nbsp;</div>
                                <div class="validacionesMain">
                                    <asp:ValidationSummary runat="server" ID="ValidationSummary1" ValidationGroup="vgPrioridad"
                                        DisplayMode="BulletList" CssClass="summary" meta:resourcekey="valPrioridad" Width="160" />
                                </div>
                            </div>
                        </div>
                        <p>
                        </p>
                    </div>
                    <div class="separador">
                        <asp:Button ID="btnDocumentoAprobado" runat="server" OnClick="btnDocumentoAprobado_Click"
                            CssClass="boton" meta:resourcekey="btnDocumentoAprobado"   />
                    </div>
                </div>
</asp:Content>
