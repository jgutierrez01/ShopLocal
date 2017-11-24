using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using SAM.Common;
using System.IO;
using System.Web.Caching;
using System.Web;
using System.Threading;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;

namespace SAM.BusinessLogic.Utilerias
{
    public static class GeneradorCorreos
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(GeneradorCorreos));
        private const string CORREO_ACTIVACION = "CorreoActivacion.txt";
        private const string CORREO_OLVIDO_PASSWORD = "CorreoOlvidoPassword.txt";
        private const string CORREO_REINICIO_PASSWORD = "CorreoReinicioPassword.txt";
        private const string CORREO_TEMPLATE_PRINCIPAL = "EmailTemplate.txt";
        private const string CORREO_PENDIENTES = "CorreoPendientes.txt";
        private const string CORREO_DATOS_NO_ENCONTRADOS = "CorreoDatosNoEncontradosPeqkgtEsp.txt";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usu"></param>
        /// <param name="urlActivacion"></param>
        /// <returns></returns>
        internal static string ObtenCorreoDeActivacion(Usuario usu, string urlActivacion)
        {
            string correo = obtenerTemplateCorreo(CORREO_ACTIVACION, usu.Idioma);
            return string.Format(correo, usu.Nombre, usu.Username, urlActivacion);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usu"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        internal static string ObtenCorreoOlvidoPassword(Usuario usu, string password)
        {
            string correo = obtenerTemplateCorreo(CORREO_OLVIDO_PASSWORD, usu.Idioma);
            return string.Format(correo, usu.Nombre, usu.Username, password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usu"></param>
        /// <param name="passwordNuevo"></param>
        /// <returns></returns>
        internal static string ObtenCorreoPasswordReiniciado(Usuario usu, string passwordNuevo)
        {
            string correo = obtenerTemplateCorreo(CORREO_REINICIO_PASSWORD, usu.Idioma);
            return string.Format(correo, usu.Nombre, usu.Username, passwordNuevo);
        }

        internal static string ObtenCorreoPendientes(Usuario usu, string proyecto, string pendiente, string detalle)
        {
            string correo = obtenerTemplateCorreo(CORREO_PENDIENTES, usu.Idioma);
            return string.Format(correo, usu.Nombre, proyecto, pendiente, detalle);
        }

        /// <summary>
        /// 
        /// </summary>      
        /// <returns></returns>
        internal static string ObtenerCorreoDatosNoEncontradosPeqKgtEsp(Usuario usu,string nombreProyecto)
        {
            string correo = obtenerTemplateCorreo(CORREO_DATOS_NO_ENCONTRADOS, usu.Idioma);
            return string.Format(correo, nombreProyecto);
        }
       


        /// <summary>
        /// Obtiene el template del correo
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo base a buscar.</param>
        /// <returns>Contenido de la plantilla de correo</returns>
        private static string obtenerTemplateCorreo(string nombreArchivo, string cultura)
        {
            string principal = string.Empty;
            string imgHeader = Configuracion.ImagenHeaderCorreo;
            string imgFooter = Configuracion.ImagenFooterCorreo;

            object templatePrincipal = HttpRuntime.Cache[CORREO_TEMPLATE_PRINCIPAL];

            if (templatePrincipal != null)
            {
                principal = (string)templatePrincipal;
            }
            else
            {
                principal = getEmailTemplateFromFileSystem();
            }

            string key = string.Format("Sam.Correos.GeneradorCorreos_{0}_{1}", nombreArchivo, cultura);

            object templateCorreo = HttpRuntime.Cache[key];


            if (templateCorreo != null)
            {
                return string.Format(@principal, imgHeader, (string)templateCorreo, imgFooter);
            }
            else
            {
                return string.Format(@principal, imgHeader, getEmailFromFileSystem(nombreArchivo, key, cultura), imgFooter);
            }
        }


        /// <summary>
        /// Obtiene el template del correo electrónico desde file system y luego lo almacena en Cache
        /// para recuperarlo posteriormente.
        /// </summary>
        /// <param name="nombreArchivo">Nombre del archivo a buscar.</param>
        /// <param name="llaveCache">Llave de cache con la cual se guardará el archivo</param>
        /// <param name="cultura">Idioma para el correo</param>
        /// <returns>String con el contenido del template del correo</returns>
        private static string getEmailFromFileSystem(string nombreArchivo, string llaveCache, string cultura)
        {
            string templateCorreo = string.Empty;
            string carpeta = Configuracion.RutaArchivosCorreo;
            string[] archivo = nombreArchivo.Split('.');
            string rutaCompleta = string.Format("{0}{1}{2}.{3}.{4}", carpeta, Path.DirectorySeparatorChar, archivo[0], cultura, archivo[1]);

            try
            {
                using (FileStream archivoFisico = new FileStream(rutaCompleta, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(archivoFisico))
                    {
                        templateCorreo = sr.ReadToEnd();
                        sr.Close();
                        archivoFisico.Close();
                    }
                }

                CacheDependency dependencia = new CacheDependency(rutaCompleta);

                HttpRuntime.Cache.Add(llaveCache,
                                      templateCorreo,
                                      dependencia,
                                      DateTime.Now.AddMonths(1),
                                      Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal,
                                      null);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Error al buscar el archivo {0}.",rutaCompleta),ex);
            }

            return templateCorreo;
        }

        private static string getEmailTemplateFromFileSystem()
        {
            string templateCorreo = string.Empty;
            string carpeta = Configuracion.RutaArchivosCorreo;
            string archivo = CORREO_TEMPLATE_PRINCIPAL;
            string rutaCompleta = string.Format("{0}{1}{2}", carpeta, Path.DirectorySeparatorChar, archivo);

            try
            {
                using (FileStream archivoFisico = new FileStream(rutaCompleta, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(archivoFisico))
                    {
                        templateCorreo = sr.ReadToEnd();
                        sr.Close();
                        archivoFisico.Close();
                    }
                }

                CacheDependency dependencia = new CacheDependency(rutaCompleta);

                HttpRuntime.Cache.Add(CORREO_TEMPLATE_PRINCIPAL,
                                      templateCorreo,
                                      dependencia,
                                      DateTime.Now.AddMonths(1),
                                      Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal,
                                      null);
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Error al buscar el archivo {0}.", rutaCompleta), ex);
            }

            return templateCorreo;
        }





    }
}
