using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using Mimo.Framework.Common;
using SAM.Common;
using SAM.Entities;
using SAM.BusinessObjects.Administracion;
using System.Net;
using SAM.Web.Classes;
using SAM.BusinessObjects.Utilerias;
using System.ServiceModel.DomainServices.Server;
using SAM.Web.RIAServices;

namespace SAM.Web
{
    public class Global : HttpApplication
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            //Vamos a revisar si ya tenemos al menos al usurio administrador creado
            if (Membership.GetUser(Configuracion.UsernameDefaultAdmin) == null)
            {
                UsuarioBO.Instance.CreaAdministrador();
            }

            //Lo hacemos para inicializar el contexto del EF4
            CacheCatalogos.Instance.ObtenerAceros();

            log4net.Config.XmlConfigurator.Configure();

            if (!(DomainService.Factory is DomainServiceFactory))
            {
                DomainService.Factory = new DomainServiceFactory(DomainService.Factory);
            }

            _logger.Debug("Starting...");
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

            //Para que el formato de moneda tenga el signo negativo en lugar de los paréntesis
            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 1;
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

    internal sealed class DomainServiceFactory : IDomainServiceFactory
    {

        private IDomainServiceFactory _defaultFactory;

        public DomainServiceFactory(IDomainServiceFactory defaultFactory)
        {
            _defaultFactory = defaultFactory;
        }

        public DomainService CreateDomainService(Type domainServiceType, DomainServiceContext context)
        {
            if(domainServiceType == typeof(DashboardService))
            {
                DomainServiceContext authServiceContext = new DomainServiceContext(context, DomainOperationType.Query);
                DomainService domainService = (DomainService)Activator.CreateInstance(domainServiceType, SessionFacade.UserId);
                domainService.Initialize(context);

                return domainService;
            }

            return _defaultFactory.CreateDomainService(domainServiceType, context);
        }

        public void ReleaseDomainService(DomainService domainService)
        {
            if (domainService is DashboardService)
            {
                domainService.Dispose();
            }
            else
            {
                _defaultFactory.ReleaseDomainService(domainService);
            }
        }

    }
}