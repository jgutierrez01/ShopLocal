<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/Popup.Master" AutoEventWireup="true" CodeBehind="PopUpDatosNoEncontrados.aspx.cs" Inherits="SAM.Web.Ingenieria.PopUpDatosNoEncontrados" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<h4>
<asp:Literal runat="server" ID="litTitulo" meta:resourcekey="litTitulo" />
</h4>
<div class="divIzquierdo">
<mimo:MimossRadGrid runat="server" ID="grdPeqs" OnNeedDataSource="grd_OnNeedDataSource" Height="200px" Width="90%">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false" AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblTituloLstPeq" runat="server" ID="lblTituloLstPeq" />
                        </div>
                    </div>
                </CommandItemTemplate>
                <Columns>                    
                    <telerik:GridBoundColumn UniqueName="TipoJunta" DataField="TipoJunta" HeaderStyle-Width="100"
                        Groupable="false" meta:resourcekey="gbcTipoJunta" />
                    <telerik:GridBoundColumn UniqueName="FamiliaAcero" DataField="FamiliaAcero"
                        HeaderStyle-Width="150" Groupable="false" meta:resourcekey="gbcFamiliaAcero" />
                    <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" DataFormatString="{0:N3}"
                        HeaderStyle-Width="100" Groupable="false" meta:resourcekey="gbcCedula" />
                    <telerik:GridBoundColumn UniqueName="Diametro" DataField="Diametro" DataFormatString="{0:N3}"
                        HeaderStyle-Width="100" Groupable="false" meta:resourcekey="gbcDiametro" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
</div>
<p></p>
<br />
<div class="divIzquierdo">
<mimo:MimossRadGrid runat="server" ID="grdEsp" OnNeedDataSource="grd_OnNeedDataSource" Height="200px" Width="90%">
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false" AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblTituloLstEsp" runat="server" ID="lblTituloLstEsp" />
                        </div>
                    </div>
                </CommandItemTemplate>
                <Columns>                    
                    <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" HeaderStyle-Width="100"
                         Groupable="false" meta:resourcekey="gbcCedula"  />
                    <telerik:GridBoundColumn UniqueName="Diametro" DataField="Diametro" DataFormatString="{0:N3}"
                        HeaderStyle-Width="100" Groupable="false" meta:resourcekey="gbcDiametro" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
                        Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
</div>
<p></p>
<br />
<div class="divIzquierdo">
<mimo:MimossRadGrid runat="server" ID="grdKgt" OnNeedDataSource="grd_OnNeedDataSource" Height="200px" Width="90%">
<MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="false" AllowPaging="false" AllowSorting="false" AllowFilteringByColumn="false">
    <CommandItemTemplate>
        <div class="comandosEncabezado">
            <div class="tituloGrid">
                <asp:Label meta:resourcekey="lblTituloLstKgt" runat="server" ID="lblTituloLstKgt" />
            </div>
        </div>
    </CommandItemTemplate>
    <Columns>    
        <telerik:GridBoundColumn UniqueName="Cedula" DataField="Cedula" HeaderStyle-Width="100"
           Groupable="false" meta:resourcekey="gbcCedula" />
        <telerik:GridBoundColumn UniqueName="Diametro" DataField="Diametro" DataFormatString="{0:N3}"
            HeaderStyle-Width="100" Groupable="false" meta:resourcekey="gbcDiametro" />
        <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false"
            Reorderable="false" ShowSortIcon="false">
            <ItemTemplate>
                &nbsp;
            </ItemTemplate>
        </telerik:GridTemplateColumn>
    </Columns>
</MasterTableView>
</mimo:MimossRadGrid>
</div>
<br />
<br />
<br />
<br />
<p>&nbsp;</p>
</asp:Content>
