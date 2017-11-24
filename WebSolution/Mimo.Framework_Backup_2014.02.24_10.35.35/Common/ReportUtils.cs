using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Mimo.Framework.Common
{
    public class ReportUtils
    {
        public static void SendReportAsPDF(HttpResponse httpResponse, byte[] report, string reportName)
        {
            if (!reportName.EndsWith(".pdf"))
            {
                reportName += ".pdf";
            }

            string nombreAttachment = string.Format("attachment; filename=\"{0}\"", reportName);

            httpResponse.Clear();
            httpResponse.ContentType = "application/pdf";
            httpResponse.AddHeader("content-disposition", nombreAttachment);

            //Renderear bytes con el content-type adecuando para que el browser lo intente abrir con Adobe
            httpResponse.BinaryWrite(report);       
        }
    }
}
