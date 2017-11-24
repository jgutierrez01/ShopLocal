using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.Entities.Personalizadas;
using System.Linq.Expressions;
using System;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;
using System.Data.Objects;
using Mimo.Framework.Extensions;

namespace SAM.BusinessObjects.Ingenieria
{
    public class SpoolHoldBO
    {
        private static readonly object _mutex = new object();
        private static SpoolHoldBO _instance;

         /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private SpoolHoldBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase SpoolHoldBO
        /// </summary>
        /// <returns></returns>
        public static SpoolHoldBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SpoolHoldBO();
                    }
                }
                return _instance;
            }
        }

       /// <summary>
       /// Regresa un objeto de tipo SpoolHold en base a su ID.
       /// Este objeto regresa sin relaciones, únicamente las propiedades primitivas
       /// del mismo se llenan.
       /// </summary>
       /// <param name="spoolHoldID"></param>
       /// <returns></returns>
        public SpoolHold Obtener(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.SpoolHold.Where(x => x.SpoolID == spoolID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Verifica si el spool en cuestion tiene o no algun hold
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public bool TieneHold(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.SpoolHold.Where(x => x.SpoolID == spoolID && (x.TieneHoldCalidad || x.TieneHoldIngenieria || x.Confinado)).Any();
            }
        }

        public void Guarda(SpoolHold spoolHold, SpoolHoldHistorial spoolHoldHistorial)
        {
           try
           {
            using (SamContext ctx = new SamContext())
            {
                ctx.SpoolHold.ApplyChanges(spoolHold);
                ctx.SpoolHoldHistorial.ApplyChanges(spoolHoldHistorial);
                ctx.SaveChanges();

               
            }
           }
           catch (OptimisticConcurrencyException)
           {
               throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
           }
        }

        //public void SpoolsTienenHold(int[] wsSpoolsIds)
        //{
        //    using (SamContext ctx = new SamContext())
        //    {
        //        List<Spool> spoolsConHold = (from ws in ctx.WorkstatusSpool
        //                                     join odts in ctx.OrdenTrabajoSpool on ws.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
        //                                     join s in ctx.Spool on odts.SpoolID equals s.SpoolID
        //                                     join sh in ctx.SpoolHold on s.SpoolID equals sh.SpoolID
        //                                     where wsSpoolsIds.Contains(ws.WorkstatusSpoolID)
        //                                     && sh.TieneHoldIngenieria
        //                                     && sh.TieneHoldCalidad
        //                                     && sh.Confinado
        //                                     select s).ToList();
        //    }
        //}
    }
}
