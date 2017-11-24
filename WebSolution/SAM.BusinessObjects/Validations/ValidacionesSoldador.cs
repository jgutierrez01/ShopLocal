using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities;
using SAM.BusinessObjects.Catalogos;

namespace SAM.BusinessObjects.Validations
{
    public class ValidacionesSoldador
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="codigo"></param>
        /// <param name="soldadorID"></param>
        /// <returns></returns>
        public static bool CodigoDuplicado(SamContext ctx, string codigo, int? soldadorID, int patioID)
        {
            return ctx.Soldador.Any(x => x.Codigo == codigo && x.SoldadorID != soldadorID && x.PatioID == patioID);
        }

        /// <summary>
        ///  permite verificar si el soldador existe.
        /// </summary>
        /// <param name="codigoSoldador"></param>
        /// <returns></returns>
        public static bool ExisteSoldador(string codigoSoldador, int patioID)
        {
            using (SamContext ctx = new SamContext())
            {
                if (ctx.Soldador.Any(x => x.Codigo == codigoSoldador && x.PatioID == patioID))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Verifica si el listado de soldadores ya contiene el codigo enviado
        /// </summary>
        /// <param name="codigo">Codigo a verificar</param>
        /// <param name="soldadores">Listado de Soldadores (GrdSoldadorProceso)</param>        
        public static void CodigoDuplicado(string codigo, List<GrdSoldadorProceso> soldadores)
        {
            if (soldadores.Any(x => x.CodigoSoldador == codigo))
            {
                throw new ExcepcionDuplicados(MensajesError.Excepcion_SoldadorDuplicado);
            }
        }

        /// <summary>
        /// Verifica si el listado de soldadores ya contiene el codigo enviado
        /// </summary>
        /// <param name="codigo">Codigo a verificar</param>
        /// <param name="soldadores">Listado de Soldadores (GrdSoldadorProceso)</param>        
        public static void CodigoDuplicado(string codigo, int tipoProceso, List<GrdSoldadorProceso> soldadores)
        {
            if (soldadores.Any(x => x.CodigoSoldador == codigo && x.TipoProceso == tipoProceso))
            {
                throw new ExcepcionDuplicados(MensajesError.Excepcion_SoldadorDuplicado);
            }
        }

        /// <summary>
        /// Verifica si el listado de soldadores ya contiene el codigo y consumible enviados
        /// </summary>
        /// <param name="codigo">Codigo a verificar</param>
        /// <param name="consumibleID">consumible a verificar</param>
        /// <param name="soldadores">Listado de Soldadores (GrdSoldadorProceso)</param>  
        public static void CodigoYConsumibleDuplicados(string codigo, int consumibleID, List<GrdSoldadorProceso> soldadores)
        {
            if (soldadores.Any(x => x.CodigoSoldador == codigo && x.ConsumibleID == consumibleID))
            {
                throw new ExcepcionDuplicados(MensajesError.Excepcion_SoldadorDuplicado);
            }
        }

        /// <summary>
        /// Regresa una excepción si el numero recibido es menor o igual a 0.
        /// </summary>
        /// <param name="numeroSoldadores">Cantidad de soldadores dados de alta</param>
        public static void AlMenosUnSoldador(int numeroSoldadores)
        {
            if (numeroSoldadores <= 0)
            {
                throw new ExcepcionSoldadura(MensajesError.Excepcion_SoldadoresFaltantes);
            }
        }

        /// <summary>
        /// Valida que el listado de soldadores cumplan con el wps recibido y que su wpq esté vigente
        /// </summary>
        /// <param name="detalles">Listado de Soldadores</param>
        /// <param name="wps">WPS a validar</param>
        public static void ValidaWpq(IEnumerable<JuntaSoldaduraDetalle> detalles, int? wps, DateTime fechaSoldadura)
        {
            List<string> errores = new List<string>();

            using (SamContext ctx = new SamContext())
            {
                foreach (JuntaSoldaduraDetalle soldador in detalles)
                {
                    if (wps.HasValue)
                    {
                        if (!ctx.Wpq.Where(x => x.SoldadorID == soldador.SoldadorID && x.FechaVigencia >= fechaSoldadura).Select(x => x.WpsID).Contains(wps.Value))
                        {
                            Soldador sol = ctx.Soldador.Where(x => x.SoldadorID == soldador.SoldadorID).SingleOrDefault();
                            string nombre = sol.Nombre + " " + sol.ApPaterno + " " + sol.ApMaterno;
                            errores.Add(String.Format(MensajesError.Excepcion_SoldadorSinWpq, nombre));
                        }
                    }
                }
            }

            if (errores.Count > 0)
            {
                throw new ExcepcionSoldadura(errores);
            }

        }

        public static void ValidaWpq(int soldadorID, int wps, DateTime fechaSoldadura)
        {
            List<string> errores = new List<string>();

            using (SamContext ctx = new SamContext())
            {
                if (!ctx.Wpq.Where(x => x.SoldadorID == soldadorID && x.FechaVigencia >= fechaSoldadura).Select(x => x.WpsID).Contains(wps))
                {
                    Soldador sol = ctx.Soldador.Where(x => x.SoldadorID == soldadorID).SingleOrDefault();
                    string nombre = sol.Nombre + " " + sol.ApPaterno + " " + sol.ApMaterno;
                    errores.Add(String.Format(MensajesError.Excepcion_SoldadorSinWpq, nombre));
                }

            }

            if (errores.Count > 0)
            {
                throw new ExcepcionSoldadura(errores);
            }

        }

    }
}
