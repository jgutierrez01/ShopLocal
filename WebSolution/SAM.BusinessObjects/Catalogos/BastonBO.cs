using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Grid;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Catalogos
{
    public class BastonBO
    {
        private static readonly object _mutex = new object();
        private static BastonBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private BastonBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase BastonBO
        /// </summary>
        /// <returns></returns>
        public static BastonBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new BastonBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        ///  Obtiene un BastonSpool en base al id
        /// </summary>
        /// <param name="bastonSpoolID"></param>
        /// <returns></returns>
        public BastonSpool Obtener(int bastonSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.BastonSpool.Include("Estacion")
                                      .Include("BastonSpoolJunta.JuntaSpool")
                                      .Where(x => x.BastonSpoolID == bastonSpoolID)
                                      .SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene una colección de BastonSpool por spool
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public List<BastonSpool> ObtenerPorSpool(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.BastonSpool.Include("BastonSpoolJunta.JuntaSpool").Where(x => x.SpoolID == spoolID).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public List<GrdBaston> ObtenerGrdBastonPorSpool(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<BastonSpool> bastonSpoolLst = ctx.BastonSpool
                                                      .Include("Estacion.Taller")
                                                      .Include("BastonSpoolJunta.JuntaSpool")
                                                      .Where(x => x.SpoolID == spoolID)
                                                      .OrderBy(x => x.LetraBaston)
                                                      .ToList();
                List<GrdBaston> bastones = null;
                bastones = (from b in bastonSpoolLst
                            let etiquetas = String.Join("-", b.BastonSpoolJunta.Select(x => x.JuntaSpool.Etiqueta).ToArray())
                            let pdis = b.BastonSpoolJunta.Select(x => x.JuntaSpool.Diametro).Sum()
                            let nombreTaller = b.Estacion != null ? b.Estacion.Taller.Nombre : string.Empty
                            let nombreEstacion = b.Estacion != null ? b.Estacion.Nombre : string.Empty
                            select new GrdBaston
                            {
                                BastonSpoolID = b.BastonSpoolID,
                                SpoolID = b.SpoolID,
                                LetraBaston = b.LetraBaston,
                                Estacion = string.Format("{0}{1}", nombreTaller, nombreEstacion),
                                Etiquetas = etiquetas,
                                PDI = pdis
                            }).ToList();

                return bastones;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bastonSpool"></param>
        public void Guarda(BastonSpool bastonSpool)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    // Guardamos BastonSpool
                    ctx.BastonSpool.ApplyChanges(bastonSpool);
                    ctx.SaveChanges();

                    bastonSpool.BastonSpoolJunta.ToList()
                               .ForEach(x => ctx.BastonSpoolJunta.ApplyChanges(x));

                    // Actualizamos las juntas spool
                    bastonSpool.BastonSpoolJunta.ToList()
                               .ForEach(x => ctx.JuntaSpool.ApplyChanges(x.JuntaSpool));
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bastonSpool"></param>
        public void Borra(BastonSpool bastonSpool)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    var baston = ctx.BastonSpool.FirstOrDefault(x => x.BastonSpoolID == bastonSpool.BastonSpoolID);
                    var bastonSpoolJuntas = ctx.BastonSpoolJunta.Where(x => x.BastonSpoolID == bastonSpool.BastonSpoolID).ToList();

                    bastonSpoolJuntas.ForEach(x => ctx.BastonSpoolJunta.DeleteObject(x));
                    ctx.SaveChanges();

                    ctx.BastonSpool.DeleteObject(baston);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public void BorraJunta(int bastonSpoolJuntaID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    BastonSpoolJunta spoolJunta = ctx.BastonSpoolJunta.FirstOrDefault(x => x.BastonSpoolJuntaID == bastonSpoolJuntaID);
                    ctx.BastonSpoolJunta.DeleteObject(spoolJunta);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }
    }
}
