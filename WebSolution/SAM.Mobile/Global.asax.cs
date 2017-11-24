using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using Mimo.Framework.Common;
using log4net;

namespace SAM.Mobile
{
    public class Global : System.Web.HttpApplication
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            string culture = LanguageHelper.CustomCulture;
            if (culture != string.Empty)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            }
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            if (arg.Equals("idioma", StringComparison.InvariantCultureIgnoreCase))
            {
                return LanguageHelper.CustomCulture;
            }
            return base.GetVaryByCustomString(context, arg);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = HttpContext.Current.Server.GetLastError();
            _logger.Error("Error en la aplicación", ex);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}