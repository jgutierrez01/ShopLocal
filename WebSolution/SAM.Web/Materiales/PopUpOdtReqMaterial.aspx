<%@ Page Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpOdtReqMaterial.aspx.cs" Inherits="SAM.Web.Materiales.PopUpOdtReqMaterial" 
Culture="auto" meta:resourcekey="PageResource1" UICulture="auto"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div class="paginaHeader">
        <asp:Label runat="server" ID="lblTitulo" CssClass="Titulo" meta:resourcekey="lblTitulo" />
    </div>    
    <asp:PlaceHolder runat="server" ID="phSpools" Visible="true">
    <div class="divIzquierdo">
        <asp:Label runat="server" ID="lblInfo" meta:resourcekey="lblInfo"/>
    </div>
    <p></p>
        <telerik:radajaxloadingpanel runat="server" id="ldPanel">
        </telerik:radajaxloadingpanel>
        <mimo:MimossRadGrid Height="250px" runat="server" ID="grdSpools" AllowMultiRowSelection="true">
            <ClientSettings Selecting-AllowRowSelect="true">
                <Selecting AllowRowSelect="true" UseClientSelectColumnOnly="true"  />                
            </ClientSettings>
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" ShowFooter="true" ClientDataKeyNames="MaterialSpoolID,OrdenTrabajoSpool" DataKeyNames="MaterialSpoolID,OrdenTrabajoSpool">  
             <CommandItemTemplate>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="chk_h" HeaderStyle-Width="30"/>                                        
                    <telerik:GridBoundColumn meta:resourcekey="grdNumControl" UniqueName="NumeroControl" DataField="NumeroControl" FilterControlWidth="100" HeaderStyle-Width="140"/>
                    <telerik:GridBoundColumn meta:resourcekey="grdEtiq" UniqueName="EtiquetaMaterial" DataField="EtiquetaMaterial" FilterControlWidth="60" HeaderStyle-Width="120" />
                    <telerik:GridBoundColumn meta:resourcekey="grdCant" UniqueName="Cantidad" DataField="Cantidad" FilterControlWidth="60" HeaderStyle-Width="90" />                    
                        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            
        </mimo:MimossRadGrid>

            <asp:ValidationSummary ID="valSummary" runat="server" meta:resourcekey="valSummaryResource1"
            CssClass="summaryList" />
                                          
        <div class="divIzquierdo">
            <div class="separador">
                <asp:Button meta:resourcekey="btnCongelar" ID="btnCongelar" runat="server" OnClick="btnCongelar_Click" CssClass="boton" />
            </div>    
        </div>
    </asp:PlaceHolder>
 <asp:PlaceHolder ID="pnlAcciones" runat="server" Visible="false">
    <div class="cntCentralForma">     
        <table class="mensajeExito" cellpadding="0" cellspacing="0">
            <tr><td><p></p></td></tr>
            <tr>
                    <td rowspan="2" class="icono">
                        <img src="/Imagenes/Iconos/mensajeExito.png" alt="" />
                    </td>
                    <td class="titulo">
                        <asp:Label runat="server" ID="lblTituloExito" meta:resourcekey="lblTituloExito" />
                    </td>
                </tr>
                <tr>
                    <td class="cuerpo">
                        <asp:Label ID="lblMensaje" runat="server" meta:resourcekey="lblMensaje"></asp:Label>&nbsp;
                    </td>
                </tr>
        </table>
    </div>
 </asp:PlaceHolder>

</asp:Content>