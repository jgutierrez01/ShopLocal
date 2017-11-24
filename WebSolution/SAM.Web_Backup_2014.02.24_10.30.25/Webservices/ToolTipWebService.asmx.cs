using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Workstatus;
using SAM.Entities;
using SAM.Web.Controles.ToolTips;

namespace SAM.Web.Webservices
{
    /// <summary>
    /// Summary description for ToolTipWebService
    /// </summary>
    [ScriptService]
    [WebService(Namespace = "http://tooltip.sam.ws/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ToolTipWebService : System.Web.Services.WebService
    {

        /// <summary>
        /// Aqui cae cuando se manda llamar el webservice
        /// </summary>
        /// <param name="context">diccionario con los datos necesarios para construir la respuesta</param>
        /// <returns></returns>
        [WebMethod]
        public string GetToolTipData(object context)
        {
            IDictionary<string, object> contextDictionary = (IDictionary<string, object>)context;

            int reportePndID = contextDictionary["Value"].SafeIntParse();
            JuntaReportePnd juntaReportePnd = JuntaReportePndBO.Instance.ObtenerJuntaReportePndConReportes(reportePndID);
            return ViewManager.RenderView("~/Controles/ToolTips/DetalleRepPndToolTip.ascx", juntaReportePnd);
        }

    }

    public class ViewManager
    {
        public static string RenderView(string path)
        {
            return RenderView(path, null);
        }

        public static string RenderView(string path, object data)
        {
            Page pageHolder = new Page();
            SamToolTip viewControl = (SamToolTip)pageHolder.LoadControl(path);
            viewControl.BindInfo(data);
            pageHolder.Controls.Add(viewControl);

            StringWriter output = new StringWriter();
            HttpContext.Current.Server.Execute(pageHolder, output, false);

            return output.ToString();
        }
    }
}
