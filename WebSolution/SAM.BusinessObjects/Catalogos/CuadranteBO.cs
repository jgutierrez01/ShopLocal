using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Personalizadas;
using System.Globalization;

namespace SAM.BusinessObjects.Catalogos
{
   
    public class CuadranteBO
    {
        private static readonly object _mutex = new object();
        private static CuadranteBO _instance;

        /// <summary>
        /// Obtiene la instancia de la clase CuadranteBO
        /// </summary>
        /// <returns></returns>
        public static CuadranteBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new CuadranteBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Obtiene una lista de cuadrantes por proyecto
        /// </summary>
        /// <param name="_proyectoID">ID Proyecto</param>
        /// <param name="skip">Cantidad de datos a ignorar</param>
        /// <param name="take">Cantidad de datos de retorno</param>
        /// <returns>Listado de Cuadrantes (ID y Nombre)</returns>
        public IEnumerable<Simple> ObtenerCuadrantesProyecto(int? _proyectoID, string buscar, int skip, int take)
        {
            List<Simple> cu;
            using (SamContext ctx = new SamContext())
            {
                cu = (from cuadrantes in ctx.Cuadrante
                                   join proyectos in ctx.Proyecto
                                   on cuadrantes.PatioID equals proyectos.PatioID
                                   where proyectos.ProyectoID == _proyectoID
                                   select new Simple
                                   {
                                       ID = cuadrantes.CuadranteID,
                                       Valor = cuadrantes.Nombre
                                   }).ToList();
            }
            return cu.Where( x => x.Valor.StartsWith(buscar, StringComparison.InvariantCultureIgnoreCase))
                    .OrderBy( x => x.Valor)
                    .Skip(skip)
                    .Take(take);
        }

        public void GuardarCuadranteSpool(int _cuadranteID, int _ordenTrabajoSpoolID, DateTime? _fechaCuadrante, Guid userID)
        {
            if (_cuadranteID <= 0)
            {
                throw new ExcepcionCuadrante(string.Empty);
            }

            if (_ordenTrabajoSpoolID <= 0)
            {
                throw new ExcepcionCuadrante(string.Empty);
            }

            if (!_fechaCuadrante.HasValue)
            {
                throw new ExcepcionCuadrante(MensajesError.Excepcion_FechaCuadranteRequerida);
            }

            using (SamContext ctx = new SamContext())
            {
                Spool spool = (from spools in ctx.Spool
                               join orden in ctx.OrdenTrabajoSpool
                               on spools.SpoolID equals orden.SpoolID
                               where orden.OrdenTrabajoSpoolID == _ordenTrabajoSpoolID
                               select spools).Single();

                spool.CuadranteID = _cuadranteID;               
                spool.FechaLocalizacion = _fechaCuadrante;

                CuadranteHistorico cuadrantehistorico= new CuadranteHistorico();
                cuadrantehistorico.SpoolID = spool.SpoolID;
                cuadrantehistorico.CuadranteID = _cuadranteID;
                cuadrantehistorico.FechaModificacion = DateTime.Now;
                cuadrantehistorico.UsuarioModifica = userID;

                ctx.Spool.ApplyChanges(spool);
                ctx.CuadranteHistorico.ApplyChanges(cuadrantehistorico);
                ctx.SaveChanges();
            }
        }

        public void GuardarCuadranteSpool(int _cuadranteID, int _ordenTrabajoSpoolID, DateTime dateProcess, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                Spool spool = (from spools in ctx.Spool
                               join orden in ctx.OrdenTrabajoSpool
                               on spools.SpoolID equals orden.SpoolID
                               where orden.OrdenTrabajoSpoolID == _ordenTrabajoSpoolID
                               select spools).Single();


                spool.CuadranteID = _cuadranteID;
                spool.FechaLocalizacion = dateProcess;
                spool.FechaModificacion = DateTime.Now;
                spool.UsuarioModifica = userID;

                CuadranteHistorico cuadrantehistorico = new CuadranteHistorico();
                cuadrantehistorico.SpoolID = spool.SpoolID;
                cuadrantehistorico.CuadranteID = _cuadranteID;
                cuadrantehistorico.FechaModificacion = DateTime.Now;
                cuadrantehistorico.UsuarioModifica = userID;

                ctx.Spool.ApplyChanges(spool);
                ctx.CuadranteHistorico.ApplyChanges(cuadrantehistorico);
                ctx.SaveChanges();
            }
        }


        public void GuardarCuadranteSpools(int _cuadranteID, int[] _ordenTrabajoSpoolIDs, DateTime dateProcess, Guid userID)
        {
            foreach (int otsId in _ordenTrabajoSpoolIDs)
            {
                GuardarCuadranteSpool(_cuadranteID, otsId, dateProcess, userID);
            }
        }


        /// <summary>
        /// Obtiene una lista de cuadrantes por proyecto
        /// </summary>
        /// <param name="_proyectoID">ID Proyecto</param>
        /// <param name="skip">Cantidad de datos a ignorar</param>
        /// <param name="take">Cantidad de datos de retorno</param>
        /// <returns>Listado de Cuadrantes (ID y Nombre)</returns>
        public IEnumerable<Cuadrante> ObtenerCuadrantesPorPatio(int patioId)
        {
            List<Cuadrante> cu = new List<Cuadrante>();

            Cuadrante cuVacio = new Cuadrante() { CuadranteID=0, Nombre = "Seleccione un valor"};
            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                cuVacio.Nombre = "-- Select one --";
            }

            cu.Add(cuVacio);

            using (SamContext ctx = new SamContext())
            {
               cu.AddRange(ctx.Cuadrante.Where(x => x.PatioID == patioId).OrderBy(x => x.Nombre).ToList());
            }
            return cu;
        }      


    }//FIN DE LA CLASE
}
