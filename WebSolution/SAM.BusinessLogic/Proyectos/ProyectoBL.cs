using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using System.Transactions;
using SAM.BusinessObjects.Proyectos;
using SAM.Common;
using System.IO;
using SAM.BusinessObjects.Modelo;
using System.Data.Objects;

namespace SAM.BusinessLogic.Proyectos
{
    public class ProyectoBL
    {
        private static readonly object _mutex = new object();
        private static ProyectoBL _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ProyectoBL()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ProyectoBL
        /// </summary>
        /// <returns></returns>
        public static ProyectoBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ProyectoBL();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="userID"></param>
        public void AltaProyecto(Proyecto proyecto, Guid userID)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Dar proyecto de alta en BD
                ProyectoBO.Instance.AltaProyecto(proyecto, userID);

                //Generar las carpetas físicas para almacenar los PDFs
                string pathDossier = string.Concat(Configuracion.CalidadRutaDossier, proyecto.Nombre);
                string rutaCompleta;
                
                foreach (string ruta in DirectorioDossier.NodosLeafRutas)
                {
                    rutaCompleta = string.Concat(pathDossier, ruta);
                    if (!Directory.Exists(rutaCompleta))
                    {
                        Directory.CreateDirectory(rutaCompleta);
                    }
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyecto"></param>
        /// <param name="nombreOriginal"></param>
        public void GuardaProyecto(Proyecto proyecto, string nombreOriginal)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                bool nombreCambio = !nombreOriginal.Equals(proyecto.Nombre, StringComparison.InvariantCultureIgnoreCase);

                ProyectoBO.Instance.Guarda(proyecto);

                if (nombreCambio)
                {
                    string pathDossierViejo = string.Concat(Configuracion.CalidadRutaDossier, nombreOriginal);
                    string pathDossierNuevo = string.Concat(Configuracion.CalidadRutaDossier, proyecto.Nombre);

                    Directory.Move(pathDossierViejo, pathDossierNuevo);
                }

                scope.Complete();
            }
        }
    }
}
