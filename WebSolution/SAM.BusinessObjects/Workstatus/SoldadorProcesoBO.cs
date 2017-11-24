using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Personalizadas;
using SAM.Entities;

namespace SAM.BusinessObjects.Workstatus
{
    public class SoldadorProcesoBO
    {
        //variables de instancia
        private static readonly object _mutex = new object();
        private static SoldadorProcesoBO _instance;

        /// <summary>
        /// constructor para implementar el patron singleton.
        /// </summary>
        private SoldadorProcesoBO()
        {
        }

        /// <summary>
        /// permite la creacion de una instancia de la clase
        /// </summary>
        public static SoldadorProcesoBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SoldadorProcesoBO();
                    }
                }
                return _instance;
            }
        }       

        /// <summary>
        /// regresa el nombre que se guardara para el soldador que se está agregando a la tabla.
        /// </summary>
        /// <param name="codigoSoldador"></param>
        /// <returns></returns>
        public string ObtenerNombreSoldador(string codigoSoldador)
        {
            string nombre = null;

            try
            {
                using (SamContext ctx = new SamContext())
                {
                    Soldador s = ctx.Soldador.Where(x => x.Codigo == codigoSoldador).SingleOrDefault();
                    if (s.ApMaterno != null)
                    {
                        nombre = String.Format("{0} {1} {2}", s.Nombre, s.ApPaterno, s.ApMaterno);
                    }
                    else
                    {
                        nombre = String.Format("{0} {1}", s.Nombre, s.ApPaterno);
                    }
                }
                return nombre;
            }
            catch(NullReferenceException)
            {
                return "No existe soldador";
            }
        }

        /// <summary>
        /// permite obtener el proyecto al cual pertenece una junta spool
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <returns></returns>
        public int ObtenerProyectoPorJuntaSpoolID(int juntaSpoolID)
        {
            JuntaSpool js = null;
            using (SamContext ctx = new SamContext())
            {
                js = ctx.JuntaSpool
                                    .Include("Spool")
                                    .Where(x => x.JuntaSpoolID == juntaSpoolID).SingleOrDefault();

                return js.Spool.ProyectoID;
            }
        }

        /// <summary>
        /// permite obtener el patio al cual un proyecto pertenece.
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <returns></returns>
        public int ObtenerPatioPorProyecto(int proyectoID)
        {
            Proyecto p = null;
            using (SamContext ctx = new SamContext())
            {
                p = ctx.Proyecto.Where(x => x.ProyectoID == proyectoID).SingleOrDefault();

                return p.PatioID;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <returns></returns>
        public JuntaWorkstatus ObtenerWorkStatus(int juntaSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.JuntaWorkstatus.Where(x => x.JuntaSpoolID == juntaSpoolID && x.JuntaFinal).SingleOrDefault();
            }
        }
    }
}
