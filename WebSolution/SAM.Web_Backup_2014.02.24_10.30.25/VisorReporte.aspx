<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisorReporte.aspx.cs" Inherits="SAM.Web.VisorReporte" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        html, body, form
        {
            margin: 0;
            padding: 0;
            border: 0 none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:Literal runat="server" ID="litReporteNoEncontrado" meta:resourcekey="litReporteNoEncontrado"
                Visible="false" />
        </div>
    <div>
        <asp:ScriptManager runat="server" ID="scrMgr" LoadScriptsBeforeUI="true" EnableCdn="true" ScriptMode="Release" />
        <rsweb:ReportViewer ID="rptViewer" runat="server" Height="600px" Width="100%" />
    </div>
    </form>
</body>
</html>
