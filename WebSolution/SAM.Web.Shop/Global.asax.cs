using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using Autofac;
using Autofac.Integration.Mvc;
using SAM.Web.Shop.Utils;

namespace SAM.Web.Shop
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<HttpNavigationContext>().As<INavigationContext>();


            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            ClientDataTypeModelValidatorProvider.ResourceClassKey = "DefaultMessages";
            DefaultModelBinder.ResourceClassKey = "DefaultMessages";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpCookie langCookie = HttpContext.Current.Request.Cookies["language"];

            if (langCookie != null)
            {
                string language = langCookie.Value;

                if (IsAcceptedLanguage(language))
                {
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
                }
            }

            string currentLanguage = Thread.CurrentThread.CurrentUICulture.Name;

            if (!IsAcceptedLanguage(currentLanguage))
            {
                ForceMexicanSpanish();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ForceMexicanSpanish()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-MX");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-MX");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private static bool IsAcceptedLanguage(string language)
        {
            return language.Equals("en-US", StringComparison.OrdinalIgnoreCase) ||
                   language.Equals("es-MX", StringComparison.OrdinalIgnoreCase);
        }
    }
}