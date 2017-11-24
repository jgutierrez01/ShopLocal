using System;
using System.Configuration;
using System.IO;
using System.Linq;
using log4net;
using System.Collections.Generic;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: log4net.Config.Repository()]
namespace SAM.Agente
{
    

    public  class EliminarArchivos
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(EliminarArchivos));

        
        public static void Eliminar()
        {
            DirectoryInfo dir;

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Ruta"]))
            {
                _logger.Error("No se ha especificado la ruta");
                return;
            }
                

            try
            {
                dir = new DirectoryInfo(ConfigurationManager.AppSettings["Ruta"]);
            }
            catch (Exception e)
            {
                _logger.Fatal("La ruta especificada no es un directorio válido",e);
                return;
            }

            int dias;
            DateTime date;

            try
            {
                if (int.TryParse(ConfigurationManager.AppSettings["Dias"], out dias))
                {
                    date = DateTime.Now.Subtract(new TimeSpan(dias, 0, 0, 0));
                }
                else
                {
                    _logger.Error("El campo [Dias] no es un valor entero");
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.Fatal("No se encontró el parámetro [Dias] en el archivo de configuración", e);
                return;
            }

            if (!dir.Exists)
            {
                _logger.Error("La ruta especificada no es correcta");
                return;
            }

            try
            {
                _logger.Debug("Iniciando busqueda de archivos");

                if (dir.Exists)
                {
                    IEnumerable<FileInfo> archivos = dir.GetFiles("*", SearchOption.AllDirectories).Where(x => x.CreationTime <= date);
                    int contador = 0;
                    
                    _logger.Debug("Archivos a eliminar");
                    _logger.Debug("Se encontraron " + archivos.Count() + " archivos");
                    _logger.Debug("Archivos a eliminar");

                    archivos.ToList().ForEach(x => _logger.Debug(@"Carpeta: " + x.DirectoryName + " Archivo: " + x.Name));

                    _logger.Debug("Eliminando Archivos");
                    archivos.ToList().ForEach(x =>
                                                  {

                                                      File.Delete(x.DirectoryName + @"\" + x.Name);
                                                      _logger.Debug(@"Carpeta: " + x.DirectoryName + " Archivo: " + x.Name + " Eliminado");
                                                      contador++;

                                                  });

                    _logger.Debug(contador + " Archivos eliminados");
                    _logger.Debug("Verificando carpetas a eliminar");

                    dir.GetDirectories().ToList().ForEach(x =>
                                                              {
                                                                  var archivosCarpeta = x.GetFiles("*",
                                                                                                   SearchOption.
                                                                                                       AllDirectories).
                                                                      ToList();

                                                                  if (archivosCarpeta.Count() == 0)
                                                                  {
                                                                      _logger.Debug("La carpeta " + x.Name +
                                                                                    " ha sido eliminada");
                                                                      x.Delete();
                                                                  }
                                                              });



                }
                else
                {
                    _logger.Info("No Se encontraron archivos en la ruta especificada");
                    return;
                }

                _logger.Debug("Proceso Terminado");
            }
            catch (Exception e)
            {
                _logger.Fatal("Error no esperado ", e);
                return;
            }
          
        }
    }
}
