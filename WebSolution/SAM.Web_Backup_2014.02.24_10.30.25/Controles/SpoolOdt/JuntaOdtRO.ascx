<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JuntaOdtRO.ascx.cs" Inherits="SAM.Web.Controles.SpoolOdt.JuntaOdtRO" %>
<%@ Import Namespace="Mimo.Framework.Extensions" %>
<div class="infoJuntas">
    <asp:ValidationSummary ID="valSummary" runat="server" ValidationGroup="vgBaston" CssClass="summaryList" meta:resourcekey="valSummary" />
    <div class="icoReingEncabezado divIzquierdo" style="width:550px;">
        <asp:Image runat="server" ID="imgReingenieria" ImageUrl="~/Imagenes/Iconos/ico_reingenieria.png" />
        <asp:Label runat="server" ID="lblReing" meta:resourcekey="lblReing" />
    </div>
    <div class="divIzquierdo" style="margin-bottom: 30px">
        <table class="repSam" cellpadding="0" cellspacing="0">
        <colgroup>
            <col width="30" />
            <col width="60" />
            <col width="60" />
            <col width="50" />
            <col width="70" />
            <col width="80" />
            <col width="90" />
            <col width="80" />
            <col width="60" />
        </colgroup>
        <thead>
            <tr class="repEncabezado">
                <th colspan="9"><asp:Literal runat="server" ID="litJuntas" meta:resourcekey="litJuntas" /></th>
            </tr>
            <tr class="repTitulos">
                <th>&nbsp;</th>
                <th><asp:Literal runat="server" ID="litEtiqueta" meta:resourcekey="litEtiqueta" /></th>
                <th><asp:Literal runat="server" ID="litDiametro" meta:resourcekey="litDiametro" /></th>
                <th><asp:Literal runat="server" ID="litTipoJunta" meta:resourcekey="litTipoJunta" /></th>
                <th><asp:Literal runat="server" ID="litCedula" meta:resourcekey="litCedula" /></th>
                <th><asp:Literal runat="server" ID="litLocalizacion" meta:resourcekey="litLocalizacion" /></th>
                <th><asp:Literal runat="server" ID="litFamiliaAcero" meta:resourcekey="litFamiliaAcero" /></th>
                <th><asp:Literal runat="server" ID="litFabArea" meta:resourcekey="litFabArea" /></th>
                <th><asp:Literal runat="server" ID="LitBaston" meta:resourcekey="litBaston" /></th>
            </tr>
        </thead>
        <asp:Repeater runat="server" ID="repJuntas" OnItemDataBound="repJuntas_OnItemDataBound">
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="hdJuntaSpoolID" Value='<%#Eval("JuntaSpoolID")%>' />
                <tr class="repFila">
                    <td>
                        <asp:Image runat="server" ID="imgReing" Visible='<%#!Eval("ExisteEnLaOdt").SafeBoolParse()%>' ImageUrl="~/Imagenes/Iconos/ico_reingenieria.png" meta:resourcekey="imgReing" />
                        <asp:Image runat="server" ID="imgFueReing" Visible='<%#Eval("FueReingenieria").SafeBoolParse()%>' ImageUrl="~/Imagenes/Iconos/ico_reingenieria_verde.png" meta:resourcekey="imgFueReing" />
                    </td>
                    <td><%#Eval("Etiqueta")%></td>
                    <td><%#String.Format("{0:#0.000}",Eval("Diametro"))%></td>
                    <td><%#Eval("TipoJunta")%></td>
                    <td><%#Eval("Cedula")%></td>
                    <td><%#Eval("Localizacion")%></td>
                    <td><%#Eval("FamiliasAcero")%></td>
                    <td><%#Eval("FabArea")%></td>
                    <td><asp:CheckBox runat="server" ID="chkBaston" Checked='<%#Eval("TieneBaston")%>'  /></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <asp:HiddenField runat="server" ID="hdJuntaSpoolID" Value='<%#Eval("JuntaSpoolID")%>' />
                <tr class="repFilaPar">
                    <td>
                        <asp:Image runat="server" ID="imgReing" Visible='<%#!Eval("ExisteEnLaOdt").SafeBoolParse()%>' ImageUrl="~/Imagenes/Iconos/ico_reingenieria.png" meta:resourcekey="imgReing" />
                        <asp:Image runat="server" ID="imgFueReing" Visible='<%#Eval("FueReingenieria").SafeBoolParse()%>' ImageUrl="~/Imagenes/Iconos/ico_reingenieria_verde.png" meta:resourcekey="imgFueReing" />
                    </td>
                    <td><%#Eval("Etiqueta")%></td>
                    <td><%#String.Format("{0:#0.000}",Eval("Diametro"))%></td>
                    <td><%#Eval("TipoJunta")%></td>
                    <td><%#Eval("Cedula")%></td>
                    <td><%#Eval("Localizacion")%></td>
                    <td><%#Eval("FamiliasAcero")%></td>
                    <td><%#Eval("FabArea")%></td>
                    <td><asp:CheckBox runat="server" ID="chkBaston" Checked='<%#Eval("TieneBaston")%>' /></td>
                </tr>
            </AlternatingItemTemplate>
        </asp:Repeater>
        <tfoot>
            <tr class="repPie">
                <td colspan="9">&nbsp;</td>
            </tr>
        </tfoot>
    </table>
    </div>
    <div class="divIzquierdo" style="margin-left:25px">
        <fieldset>
            <legend><asp:Literal runat="server" ID="litBastones" meta:resourcekey="litBastones" /></legend>

            <div class="separador">
                <mimo:LabeledTextBox runat="server" ID="txtLetraBaston" meta:resourcekey="txtLetraBaston" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="rfvLetraBaston" ValidationGroup="vgBaston" ControlToValidate="txtLetraBaston" Display="None" meta:resourcekey="rfvLetraBaston" />
            </div>
            <div class="separador">
                <asp:Label runat="server" ID="lblTaller" CssClass="bold" meta:resourcekey="lblTaller" />
                <br />
                <asp:DropDownList runat="server" ID="ddlTaller" AutoPostBack="true" OnSelectedIndexChanged="ddlTaller_OnSelectedIndexChanged" />
                <span class="required">*</span>
                <asp:RequiredFieldValidator runat="server" ID="rfvTaller" ValidationGroup="vgBaston" ControlToValidate="ddlTaller" Display="None" InitialValue="" meta:resourcekey="rfvTaller" />
            </div>
            <div class="separador">
                <asp:Label ID="lblEstacion" runat="server" CssClass="bold" Text="Estación:" meta:resourcekey="lblEstacion" />
                <br />
                <asp:DropDownList runat="server" ID="ddlEstacion" />
            </div>

            <div class="separador">
                <div class="divIzquierdo">
                    <asp:Button runat="server" ID="btnAgrupar" CssClass="boton" CausesValidation="true" ValidationGroup="vgBaston" OnClick="btnAgrupar_OnClick" meta:resourcekey="btnAgrupar" />
                </div>
                <div class="divDerecho">
                    <asp:Button runat="server" ID="btnTerminar" CssClass="boton" CausesValidation="false" OnClick="btnTerminar_OnClick" meta:resourcekey="btnTerminar" />
                </div>
            </div>
        </fieldset>
    </div>
    <p class="clear"></p>

    <asp:Repeater ID="grdBastones" runat="server" OnItemCommand="grdBastones_ItemCommand" Visible="false">
        <HeaderTemplate>
            <table class="repSam" cellpadding="0" cellspacing="0" width="40%">
                <colgroup>
                    <col width="15" />
                    <col width="25" />
                    <col width="60" />
                    <col width="60" />
                </colgroup>
                <thead>
                    <tr class="repEncabezado">
                        <th colspan="4">
                            Bastones
                        </th>
                    </tr>
                    <tr class="repTitulos">
                        <th class="accion">
                            &nbsp;
                        </th>
                        <th>
                            <asp:Literal ID="litLetraBaston" runat="server" meta:resourcekey="litLetraBaston" ></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="litEstacion" runat="server" meta:resourcekey="litEstacion" ></asp:Literal>
                        </th>
                        <th>
                            <asp:Literal ID="litJuntas" runat="server" meta:resourcekey="litJuntas" ></asp:Literal>
                        </th>
                    </tr>
                </thead>
        </HeaderTemplate>
        <ItemTemplate>
            <tr class="repFila">
                <td class="accion" style="width: 20px">
                    <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server" CommandArgument='<%#Eval("BastonSpoolID") %>'
                        OnClientClick="return Sam.Confirma(1);">
                        <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server" meta:resourcekey="imgBorrar" />
                    </asp:LinkButton>
                </td>
                <td>
                    <%# Eval("LetraBaston")%>
                </td>
                <td>
                    <%# Eval("Estacion")%>
                </td>
                <td>
                    <%# Eval("Etiquetas")%>
                </td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr class="repFilaPar">
                <td style="width: 20px">
                    <asp:LinkButton CommandName="Borrar" ID="lnkBorrar" runat="server"  CommandArgument='<%#Eval("BastonSpoolID") %>'
                        OnClientClick="return Sam.Confirma(1);">
                        <asp:Image ImageUrl="~/Imagenes/Iconos/borrar.png" ID="imgBorrar" runat="server" meta:resourcekey="imgBorrar" />
                    </asp:LinkButton>
                </td>
                <td>
                    <%# Eval("LetraBaston")%>
                </td>
                <td>
                    <%# Eval("Estacion")%>
                </td>
                <td>
                    <%# Eval("Etiquetas")%>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <FooterTemplate>
            <tfoot>
                <tr class="repPie">
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
            </tfoot>
            </table></FooterTemplate>
    </asp:Repeater>
</div>