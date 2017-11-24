using System;
using SAM.Web.Classes;
using System.Web.UI;
using log4net;
using System.Text;
using SAM.Common;
using System.Net;
using System.Web;

namespace SAM.Web.Errores
{
    public partial class Error403 : Page
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Error403));
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (_logger.IsDebugEnabled)
                {
                    _logger.Debug("Page Load de la página de Error 403");
                }

                string absolutePath = Request.Url.AbsolutePath;
                string sPath = Request.QueryString["aspxerrorpath"];

                if (absolutePath != "/Errores/Error403.aspx")
                {
                    sPath = Request.Url.PathAndQuery;
                }

                if (!string.IsNullOrEmpty(sPath))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine();

                    if (Request.UrlReferrer != null)
                    {
                        Uri referrer = Request.UrlReferrer;
                        sb.AppendFormat("Referrer Dns Host Name = {0}\r\n", referrer.DnsSafeHost);
                        sb.AppendFormat("Referrer Path and Query = {0}\r\n", referrer.PathAndQuery);
                    }

                    sb.AppendFormat("User host address = {0}\r\n", Request.UserHostAddress);
                    sb.AppendFormat("User host name = {0}\r\n", Request.UserHostName);
                    sb.AppendFormat("User agent = {0}\r\n", Request.UserAgent);
                    sb.AppendFormat("Recurso solicitado = {0}\r\n", sPath);

                    if (User.Identity.IsAuthenticated)
                    {
                        sb.AppendFormat("Usuario = {0}\r\n", User.Identity.Name);
                    }
                    else
                    {
                        sb.AppendFormat("ID usuario anonimo = {0}\r\n", Request.AnonymousID);
                    }

                    _logger.ErrorFormat("Acceso no autorizado, error 403, detalle: {0}", sb);

                    //Estamos en servidores de desarrollo
                    if (Request.Url.Authority.Contains(Configuracion.HostnameDesarrollo))
                    {
                        phDetalle.Visible = true;
                        litMsg.Text = sb.Replace("\r\n", "<br />").ToString();
                    }
                }

                //Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}