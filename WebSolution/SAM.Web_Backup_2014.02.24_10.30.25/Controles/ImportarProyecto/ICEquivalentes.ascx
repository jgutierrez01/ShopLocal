<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ICEquivalentes.ascx.cs" Inherits="SAM.Web.Controles.ImportarProyecto.ICEquivalentes" %>
<div class="contenedorCentral">
    <telerik:RadAjaxManager runat="server" style="display:none;"  ID="ajaxMgr">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="grdICEquivalentes">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="grdICEquivalentes" />
                    <telerik:AjaxUpdatedControl ControlID="valSummary" />
                    <telerik:AjaxUpdatedControl ControlID="phMensajeExito" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
     <asp:PlaceHolder runat="server" ID="phICEquivalentes">   
    <div class="cajaFiltros">
        <div class="divIzquierdo">
            <div class="separador">
                <span class="required">*</span>
                <asp:Label runat="server" ID="lblProyecto" Text="Proyecto:" CssClass="bold" meta:resourcekey="lblProyecto" />
                <mimo:MappableDropDown runat="server" ID="ddlProyecto" EntityPropertyName="ProyectoID" AutoPostBack="false" CssClass="labelHack" />
                <asp:RequiredFieldValidator runat="server" ID="rfvProyecto" ControlToValidate="ddlProyecto" InitialValue="" Display="None"
                        ErrorMessage="El Proyecto es requerido" ValidationGroup="vgEquivalentes" meta:resourcekey="rfvProyecto" />
            </div>
        </div>
        <div class="divIzquierdo">
            <div class="separador">
                <asp:Button runat="server" ID="btnMostrar" Text="Mostrar" CssClass="boton" OnClick="btnMostrar_Click" ValidationGroup="vgEquivalentes" meta:resourcekey="btnMostrar" />
            </div>
        </div>
        <p></p>
    </div>

    <asp:HiddenField ID="hdnProyectoID" runat="server" />
    <br />
    <asp:ValidationSummary runat="server" ID="valSummary" DisplayMode="BulletList" CssClass="summaryList" ValidationGroup="vgEquivalentes" meta:resourcekey="valSummary" />

    <br />
    <mimo:MimossRadGrid ID="grdICEquivalentes" runat="server" OnNeedDataSource="grdICEquivalentes_OnNeedDataSource" OnDetailTableDataBind="grdICEquivalentes_OnDetailTableDataBind" Visible="false" AllowMultiRowSelection="true">
            <ClientSettings>
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <MasterTableView AutoGenerateColumns="false" AllowMultiColumnSorting="true" HierarchyLoadMode="ServerOnDemand" DataKeyNames="MinItemCodeEquivalenteID" ClientDataKeyNames="MinItemCodeEquivalenteID">
                <CommandItemTemplate>
                    <div class="comandosEncabezado">
                        <div class="tituloGrid">
                            <asp:Label meta:resourcekey="lblgrdICEquivalentes" runat="server" ID="lblgrdICEquivalentes" />
                        </div>
                        <asp:LinkButton runat="server" ID="lnkImportar" OnClick="lnkImportar_OnClick" meta:resourcekey="lnkImportar" CssClass="link" />
                        <asp:ImageButton runat="server" ID="imgImportar" ImageUrl="~/Imagenes/IconoS/actualizar.png" OnClick="lnkImportar_OnClick" AlternateText="Importar" CssClass="imgEncabezado" meta:resourcekey="imgImportar"/>
                    </div>
                </CommandItemTemplate>
                <Columns>
                    <telerik:GridClientSelectColumn Reorderable="false" Resizable="false" UniqueName="seleccion_h" HeaderStyle-Width="30" />
                    <telerik:GridBoundColumn UniqueName="Codigo" DataField="Codigo" HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" meta:resourcekey="htCodigo" />
                    <telerik:GridBoundColumn UniqueName="Descripcion" DataField="Descripcion" HeaderStyle-Width="350" FilterControlWidth="100" Groupable="false" meta:resourcekey="htDescripcion" />
                    <telerik:GridBoundColumn UniqueName="Diametro1" DataField="Diametro1" DataFormatString="{0:#0.00}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false"  meta:resourcekey="htDiametro1" />
                    <telerik:GridBoundColumn UniqueName="Diametro2" DataField="Diametro2" DataFormatString="{0:#0.00}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htDiametro2"  />
                    <telerik:GridBoundColumn UniqueName="NumEquivalencias" DataField="NumEquivalencias" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htNumEquivalencias" />
                    <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                        <ItemTemplate>
                            &nbsp;
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <DetailTables>
                    <telerik:GridTableView AllowFilteringByColumn="false" AllowSorting="false" AllowPaging="false" EnableHeaderContextFilterMenu="false" EnableHeaderContextMenu="false" AutoGenerateColumns="false" Width="700">
                        <Columns>
                            <telerik:GridBoundColumn UniqueName="CodigoEq" DataField="CodigoEq" HeaderStyle-Width="150" FilterControlWidth="100" Groupable="false" meta:resourcekey="htCodigo" />
                            <telerik:GridBoundColumn UniqueName="DescripcionEq" DataField="DescripcionEq" HeaderStyle-Width="350" FilterControlWidth="100" Groupable="false" meta:resourcekey="htDescripcion" />
                            <telerik:GridBoundColumn UniqueName="D1Eq" DataField="D1Eq" DataFormatString="{0:#0.00}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htDiametro1" />
                            <telerik:GridBoundColumn UniqueName="D2Eq" DataField="D2Eq" DataFormatString="{0:#0.00}" HeaderStyle-Width="100" FilterControlWidth="50" Groupable="false" meta:resourcekey="htDiametro2" />
                            <telerik:GridTemplateColumn UniqueName="filler_h" AllowFiltering="false" Groupable="false" Reorderable="false" ShowSortIcon="false">
                                <ItemTemplate>
                                    &nbsp;
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </telerik:GridTableView>
                </DetailTables>
            </MasterTableView>
        </mimo:MimossRadGrid>
        </asp:PlaceHolder>
    <asp:PlaceHolder ID="phMensajeExito" runat="server" Visible="False">
        <table class="mensajeExito" cellpadding="0" cellspacing="0" style="margin: 5px auto 0 auto;">
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
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td class="ligas">
                    <div class="cuadroLigas">
                        <ul>
                            <li>
                                <asp:HyperLink ID="hlProyectos" runat="server" meta:resourcekey="hlProyectos"
                                    NavigateUrl="~/Catalogos/LstProyecto.aspx" /></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
    </asp:PlaceHolder>
</div>