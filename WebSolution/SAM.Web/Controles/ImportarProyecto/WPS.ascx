<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WPS.ascx.cs" Inherits="SAM.Web.Controles.ImportarProyecto.WPS" %>
<div class="contenedorCentral">
    <asp:PlaceHolder runat="server" ID="phWPS">
    <div class="cajaFiltros">
        <div class="divIzquierdo">
            <div class="separador">
                <span class="required">*</span>
                <asp:Label runat="server" ID="lblProyecto" Text="Proyecto:" CssClass="bold" meta:resourcekey="lblProyecto" />
                <mimo:MappableDropDown runat="server" ID="ddlProyecto3" EntityPropertyName="ProyectoID" CssClass="labelHack" />
                <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto3" InitialValue="" Display="None"
                        ErrorMessage="El Proyecto es requerido" ValidationGroup="vgWps" meta:resourcekey="rfvProyecto" />
            </div>
        </div>
        <div class="divIzquierdo">
            <div class="separador">
                <asp:Button runat="server" ID="btnMostrar" Text="Mostrar" CssClass="boton" OnClick="btnMostrar_Click" ValidationGroup="vgWps" meta:resourcekey="btnMostrar" />
            </div>
        </div>
        <p></p>
    </div>

    <asp:HiddenField ID="hdnProyectoID" runat="server" />
    <br />
    <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summaryList" ValidationGroup="vgWps" meta:resourcekey="valSummary" />
    
    <br />
    <mimo:MimossRadGrid ID="grdWpsProyecto" runat="server" OnNeedDataSource="grdWpsProyecto_OnNeedDataSource" Visible="false" AllowMultiRowSelection="true" >
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" DataKeyNames="WpsID" ClientDataKeyNames="WpsID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblWpsProyecto" runat="server" ID="lblWpsProyecto" />
                        </div>
                        <asp:LinkButton runat="server" ID="lnkImportar" OnClick="lnkImportar_OnClick" meta:resourcekey="lnkImportar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgImportar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkImportar_OnClick" AlternateText="Importar" CssClass="imgEncabezado" meta:resourcekey="imgImportar"/>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="seleccion_h" HeaderStyle-Width="30" />
                    <telerik:GridBoundColumn UniqueName="Nombre" DataField="Nombre" HeaderText="Nombre" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcNombre" />
                    <telerik:GridBoundColumn UniqueName="Material1" DataField="FamiliaAcero.Nombre" HeaderText="Fam. Acero1" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcFamiliaAcero" />
                    <telerik:GridBoundColumn UniqueName="Material2" DataField="FamiliaAcero.Nombre" HeaderText="Fam. Acero2" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcFamiliaAcero1" />
                    <telerik:GridBoundColumn UniqueName="ProcesoRaiz" DataField="ProcesoRaiz.Codigo" HeaderText="Proceso Raiz" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcProcesoRaizID" />
                    <telerik:GridBoundColumn UniqueName="ProcesoRelleno" DataField="ProcesoRelleno.Codigo" HeaderText="Proceso Relleno" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcProcesoRellenoID" />
                    <telerik:GridBoundColumn UniqueName="EspesorRaizMaximo" DataField="EspesorRaizMaximo" HeaderText="Espesor Raiz" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcEspesorRaizMaximo" />
                    <telerik:GridBoundColumn UniqueName="EspesorRellenoMaximo" DataField="EspesorRellenoMaximo" HeaderText="Espesor Relleno Maximo" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcEspesorRellenoMaximo" />
                    <telerik:GridCheckBoxColumn UniqueName="RequierePwht" DataField="RequierePwht" HeaderText="Requiere Pwht?" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcRequierePwht" />
                    <telerik:GridCheckBoxColumn UniqueName="RequierePreheat" DataField="RequierePreheat" HeaderText="Requiere Pre-heat?" HeaderStyle-Width="180" FilterControlWidth="100" Groupable="false" meta:resourcekey="gbcRequierePreheat" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>&nbsp;</ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </mimo:MimossRadGrid>
        </asp:PlaceHolder>  
</div>