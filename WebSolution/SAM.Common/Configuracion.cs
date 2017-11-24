using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Mimo.Framework.Extensions;

namespace SAM.Common
{
    public static class Configuracion
    {

        public static string CalidadRutaDossier
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.CalidadRutaDossier"];
            }
        }

        public static int CacheMaximoHoras
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.CacheMaximoHoras"].SafeIntParse();
            }
        }

        public static int CacheMuyPocosMinutos
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.CacheMuyPocosMinutos"].SafeIntParse();
            }
        }

        public static int CacheMediaHoras
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.CacheMediaHoras"].SafeIntParse();
            }
        }

        public static int CacheMinimoHoras
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.CacheMinimoHoras"].SafeIntParse();
            }
        }

        public static string UsernameDefaultAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.UsernameDefaultAdmin"];
            }
        }

        public static string CorreoDefaultAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.CorreoDefaultAdmin"];
            }
        }

        public static string PasswordDefaultAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.PasswordDefaultAdmin"];
            }
        }

        public static string NombreDefaultAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.NombreDefaultAdmin"];
            }
        }

        public static string ApellidoDefaultAdmin
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.ApellidoDefaultAdmin"];
            }
        }

        public static string RutaParaAlmacenarArchivos
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Ingenieria.UploadedFilesDirectory"];
            }
        }

        public static int IngenieriaArchivosEsperados
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Ingenieria.ArchivosEsperados"].SafeIntParse();
            }
        }

        public static string CuentaCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Correos.Cuenta"];
            }        
        }

        public static string UsuarioCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Correos.Usuario"];
            }        
        }

        public static string PasswordCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Correos.Password"];
            }        
        }

        public static string RutaArchivosCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Correos.RutaArchivos"];
            }
        }

        public static int PuertoCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Correos.Puerto"].SafeIntParse();
            }        
        }

        public static string HostCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Correos.Host"];
            } 
        }

        public static string UrlActivacion
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Web.UrlActivacion"];
            } 
        }

        public static string ImagenHeaderCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Correos.ImagenHeader"];
            }
        }

        public static string ImagenFooterCorreo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Correos.ImagenFooter"];
            }
        }

        public static string HostnameDesarrollo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.HostnameDesarrollo"];
            }
        }

        public static bool SSLEnabled
        {
            get
            {
                return Convert.ToBoolean(ConfigurationManager.AppSettings["Sam.SSLEnabled"]);
            }
        }

        public static int CoresProcesador
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.CoresProcesador"].SafeIntParse();
            }
        }


        public static string LineaDtsx
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Ingenieria.LineaDtsx"];
            }
        }

        public static string UbicacionDtsx
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Ingenieria.UbicacionDtsx"];
            }
        }

        public static string ValidaCatalogosDtsx
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Ingenieria.ValidaCatalogosDtsx"];
            }
        }

        public static string ValidaIntegridadDtsx
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Ingenieria.ValidaIntegridadDtsx"];
            }
        }

        public static string DtsxDBConnString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DtsxSamDB"].ConnectionString;
            }
        }

        public static string DBServerRawFilesDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Ingenieria.DBServerRawFilesDirectory"];
            }
        }

        public static string DBServerUploadedFilesDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Ingenieria.DBServerUploadedFilesDirectory"];
            }
        }

        public static string DBWorkStatusReports
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.Ingenieria.WorkStatusReports"];
            }
        }

        #region Ligas al sitio de Steelgo

        public static string LigaQuienesSomos
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.LigaQuienesSomos"];
            }
        }

        public static string LigaPoliticasPrivacidad
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.LigaPoliticasPrivacidad"];
            }
        }

        public static string LigaPoliticasUso
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.LigaPoliticasUso"];
            }
        }

        public static string LigaPrincipalSteelgo
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.LigaPrincipalSteelgo"];
            }
        }

        public static string LigaServicios
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.LigaServicios"];
            }
        }

        public static string LigaContacto
        {
            get
            {
                return ConfigurationManager.AppSettings["Sam.LigaContacto"];
            }
        }

        #endregion
    }
}
