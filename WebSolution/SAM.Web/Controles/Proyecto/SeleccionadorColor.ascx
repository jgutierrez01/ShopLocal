<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SeleccionadorColor.ascx.cs" Inherits="SAM.Web.Controles.Proyecto.SeleccionadorColor" %>
<div>
    <telerik:RadColorPicker runat="server"
                            ID="picker"
                            Preset="None"
                            Columns="5" 
                            CssClass="colorPicker"
                            ShowIcon="true"
                            OnClientColorChange="Sam.Utilerias.ColorSeleccionado"
                            ShowEmptyColor="false"
                            PreviewColor="false">
    </telerik:RadColorPicker>
    <asp:Label runat="server" ID="lblNombreColor" ClientIDMode="Static" />
    <asp:TextBox runat="server" CssClass="oculto" ID="txtColorSeleccionado" ClientIDMode="Static"/>
    <asp:CustomValidator runat="server" 
                         ID="cusColorRequerido" 
                         ControlToValidate="txtColorSeleccionado"
                         ValidateEmptyText="True"
                         ClientValidationFunction="Sam.Utilerias.ValidaColorRequerido"
                         OnServerValidate="cusColorRequerido_ServerValidate"
                         Enabled="False"
                         Display="None"
                         meta:resourcekey="cusColorRequerido" />
</div>
