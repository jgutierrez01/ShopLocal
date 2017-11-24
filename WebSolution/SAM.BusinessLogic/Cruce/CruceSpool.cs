using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using Mimo.Framework.Common;
using SAM.BusinessLogic.Excepciones;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Cruce
{
    public class CruceSpool
    {
        private int _proyectoID;
        private int[] _spoolIds;
        private List<NumeroUnico> _lstNumeroUnico;
        private List<Spool> _lstSpools;
        private List<ItemCode> _lstItemCode;
        private List<ItemCodeEquivalente> _lstEquivalente;
        private SamContext _ctx;
        private Guid ? _userID;
        private Dictionary<int, ItemCode> _dicItemCodes;
        private Dictionary<int, Spool> _dicSpools;
        private List<CongeladoParcial> _lstCongeladoParcial;
        private List<CruceItemCode> _lstCondensadoIC;
        private Dictionary<ItemCodeIntegrado, CruceItemCode> _dicCodensados;

        public CruceSpool(SamContext ctx, int proyectoID, int[] spoolIds, Guid ? userID)
        {
            _ctx = ctx;
            _proyectoID = proyectoID;
            _spoolIds = spoolIds;
            _userID = userID;
        }


        public List<Spool> Procesa(out List<NumeroUnico> congelados)
        {

            //spools que tienen alguna especie de hold para el proyecto
            IQueryable<int> spoolsHold = (from sph in _ctx.SpoolHold
                                          where sph.Spool.ProyectoID == _proyectoID &&
                                                (sph.TieneHoldCalidad || sph.TieneHoldIngenieria || sph.Confinado)
                                          select sph.SpoolID).AsQueryable();

            _lstSpools = _ctx.Spool
                             .Include("MaterialSpool")
                             .Include("JuntaSpool")
                             .Where(x => x.ProyectoID == _proyectoID)
                             .Where(x => _spoolIds.Contains(x.SpoolID)) // x => _spoolIds.Contains(x.SpoolID)
                             .Where(x => x.AprobadoParaCruce && x.PendienteDocumental)
                             .Where(x => !spoolsHold.Contains(x.SpoolID))
                             .ToList();

            if (_lstSpools.Count() != _spoolIds.Length)
            {
                throw new ExcepcionCruce(MensajesError.Cruce_SpoolsNoAprobados);
            }

            _lstSpools = _spoolIds.Select(x => _lstSpools.FirstOrDefault(s => s.SpoolID == x)).ToList();

            //Los I.C. ids requeridos por ingeniería
            IQueryable<int> icIdsDirectos = 
                _ctx.MaterialSpool
                    .Where(Expressions.BuildOrExpression<MaterialSpool, int>(x => x.SpoolID, _spoolIds))
                    .Select(x => x.ItemCodeID);

            //Los I.C. equivalentes
            IQueryable<ItemCodeEquivalente> qIcEq =
                _ctx.ItemCodeEquivalente.Where(x => icIdsDirectos.Contains(x.ItemCodeID));

            //Traer los Item codes tanto los de ingeniería como los equivalentes
            IQueryable<ItemCode> qIc = 
                _ctx.ItemCode.Where(x => icIdsDirectos.Contains(x.ItemCodeID) || 
                                         qIcEq.Select(y => y.ItemEquivalenteID)
                                              .Contains(x.ItemCodeID));

            //Solo los números únicos del proyecto cuyos item codes sean requeridos
            //por los spools pasados y que se puedan usar
            _lstNumeroUnico = 
                _ctx.NumeroUnico
                    .Include("NumeroUnicoInventario")
                    .Include("NumeroUnicoSegmento")
                    .Where(x => x.ProyectoID == _proyectoID && x.NumeroUnicoInventario.InventarioDisponibleCruce > 0)
                    .Where(x => qIc.Select(y => (int?)y.ItemCodeID).Contains(x.ItemCodeID))
                    .Where(x => !x.Colada.HoldCalidad)
                    .Where(x => x.Estatus.Equals(EstatusNumeroUnico.APROBADO))
                    .ToList();

            //Traer de BD todos los I.C. del proyecto requeridos
            _lstItemCode = qIc.ToList();
            //Traer de BD todos los I.C. equivalentes necesarios
            _lstEquivalente = qIcEq.ToList();

            //Congelados parciales
            _lstCongeladoParcial = (from cong in UtileriasCruce.CongParcialPorMat(_ctx, _proyectoID)
                                                               .ToList()
                                                               .AsParallel()
                                    select cong).ToList();


            //Lista con un condensado de la disponibilidad de números únicos agrupados por item code
            _lstCondensadoIC = UtileriasCruce.InventariosCondensadosPorIC(_ctx, _proyectoID).ToList();

            //Ejecutar el proceso
            estableceVariablesTemporales();
            generaDiccionarios();
            recorreSpoolsYCongela();

            //Regresar una lista con los nus congelados
            congelados = (from nus in _lstNumeroUnico
                          where nus.InfoCruce.SeUsoEnCruce
                          select nus).ToList();
            
            //Regresar los spools que se tienen que usar para la ODT
            return _lstSpools;
        }

        public List<Spool> ProcesaParaRevision(out List<NumeroUnico> congelados)
        {

            //spools que tienen alguna especie de hold para el proyecto
            IQueryable<int> spoolsHold = (from sph in _ctx.SpoolHold
                                          where sph.Spool.ProyectoID == _proyectoID &&
                                                (sph.TieneHoldCalidad || sph.TieneHoldIngenieria || sph.Confinado)
                                          select sph.SpoolID).AsQueryable();

            _lstSpools = _ctx.Spool
                             .Include("MaterialSpool")
                             .Include("JuntaSpool")
                             .Where(x => x.ProyectoID == _proyectoID)
                             .Where(x => _spoolIds.Contains(x.SpoolID)) 
                             .Where(x => !spoolsHold.Contains(x.SpoolID))
                             .ToList();


            _lstSpools = _spoolIds.Select(x => _lstSpools.FirstOrDefault(s => s.SpoolID == x)).ToList();

            //Los I.C. ids requeridos por ingeniería
            IQueryable<int> icIdsDirectos =
                _ctx.MaterialSpool
                    .Where(Expressions.BuildOrExpression<MaterialSpool, int>(x => x.SpoolID, _spoolIds))
                    .Select(x => x.ItemCodeID);

            //Los I.C. equivalentes
            IQueryable<ItemCodeEquivalente> qIcEq =
                _ctx.ItemCodeEquivalente.Where(x => icIdsDirectos.Contains(x.ItemCodeID));

            //Traer los Item codes tanto los de ingeniería como los equivalentes
            IQueryable<ItemCode> qIc =
                _ctx.ItemCode.Where(x => icIdsDirectos.Contains(x.ItemCodeID) ||
                                         qIcEq.Select(y => y.ItemEquivalenteID)
                                              .Contains(x.ItemCodeID));

            //Solo los números únicos del proyecto cuyos item codes sean requeridos
            //por los spools pasados y que se puedan usar
            _lstNumeroUnico =
                _ctx.NumeroUnico
                    .Include("NumeroUnicoInventario")
                    .Include("NumeroUnicoSegmento")
                    .Where(x => x.ProyectoID == _proyectoID && x.NumeroUnicoInventario.InventarioDisponibleCruce > 0)
                    .Where(x => qIc.Select(y => (int?)y.ItemCodeID).Contains(x.ItemCodeID))
                    .Where(x => !x.Colada.HoldCalidad)
                    .Where(x => x.Estatus.Equals(EstatusNumeroUnico.APROBADO))
                    .ToList();

            //Traer de BD todos los I.C. del proyecto requeridos
            _lstItemCode = qIc.ToList();
            //Traer de BD todos los I.C. equivalentes necesarios
            _lstEquivalente = qIcEq.ToList();

           
            //Lista con un condensado de la disponibilidad de números únicos agrupados por item code
            _lstCondensadoIC = UtileriasCruce.InventariosCondensadosPorIC(_ctx, _proyectoID).ToList();

            //Ejecutar el proceso
            estableceVariablesTemporales();
            generaDiccionarios();
            recorreSpoolsRevisionYCongela();

            //Regresar una lista con los nus congelados
            congelados = (from nus in _lstNumeroUnico
                          where nus.InfoCruce.SeUsoEnCruce
                          select nus).ToList();

            //Regresar los spools que se tienen que usar para la ODT
            return _lstSpools;
        }

        /// <summary>
        /// Método que procesa los spools importados por csv
        /// </summary>
        /// <param name="congelados"></param>
        /// <param name="errores"></param>
        /// <returns></returns>
        public List<Spool> Procesa(out List<NumeroUnico> congelados, StringBuilder errores)
        {
            bool spoolAprobadoCruce = true;

            Spool spool = _ctx.Spool
                              .Include("MaterialSpool")
                              .Include("JuntaSpool")
                              .SingleOrDefault(x => _spoolIds.Contains(x.SpoolID));

            if (!spool.AprobadoParaCruce || !spool.PendienteDocumental)
            {
                spoolAprobadoCruce = false;
                errores.Append(string.Format(MensajesError.Csv_SpoolX_AprobadoCruce, spool.Nombre));
            }

            bool spoolHold = (from sh in _ctx.SpoolHold
                              where sh.SpoolID == spool.SpoolID &&
                                    (sh.TieneHoldCalidad || sh.TieneHoldIngenieria || sh.Confinado)
                              select sh).Any();
            if (spoolHold)
            {
                spoolAprobadoCruce = false;
                errores.Append(string.Format(MensajesError.Csv_SpoolHold, spool.Nombre));
            }

            if (!spoolAprobadoCruce)
            {
                congelados = new List<NumeroUnico>();
                return new List<Spool>();
            }

            _lstSpools = new List<Spool> { spool };

            //Los I.C. ids requeridos por ingeniería
            IQueryable<int> icIdsDirectos =
                _ctx.MaterialSpool
                    .Where(Expressions.BuildOrExpression<MaterialSpool, int>(x => x.SpoolID, _spoolIds))
                    .Select(x => x.ItemCodeID);

            //Los I.C. equivalentes
            IQueryable<ItemCodeEquivalente> qIcEq =
                _ctx.ItemCodeEquivalente.Where(x => icIdsDirectos.Contains(x.ItemCodeID));

            //Traer los Item codes tanto los de ingeniería como los equivalentes
            IQueryable<ItemCode> qIc =
                _ctx.ItemCode.Where(x => icIdsDirectos.Contains(x.ItemCodeID) ||
                                         qIcEq.Select(y => y.ItemEquivalenteID)
                                              .Contains(x.ItemCodeID));

            //Solo los números únicos del proyecto cuyos item codes sean requeridos
            //por los spools pasados y que se puedan usar
            _lstNumeroUnico =
                _ctx.NumeroUnico
                    .Include("NumeroUnicoInventario")
                    .Include("NumeroUnicoSegmento")
                    .Where(x => x.ProyectoID == _proyectoID && x.NumeroUnicoInventario.InventarioDisponibleCruce > 0)
                    .Where(x => qIc.Select(y => (int?)y.ItemCodeID).Contains(x.ItemCodeID))
                    .Where(x => !x.Colada.HoldCalidad)
                    .Where(x => x.Estatus.Equals(EstatusNumeroUnico.APROBADO))
                    .ToList();

            //Traer de BD todos los I.C. del proyecto requeridos
            _lstItemCode = qIc.ToList();
            //Traer de BD todos los I.C. equivalentes necesarios
            _lstEquivalente = qIcEq.ToList();

            //Congelados parciales
            _lstCongeladoParcial = (from cong in UtileriasCruce.CongParcialPorMat(_ctx, _proyectoID)
                                                               .ToList()
                                                               .AsParallel()
                                    select cong).ToList();


            //Lista con un condensado de la disponibilidad de números únicos agrupados por item code
            _lstCondensadoIC = UtileriasCruce.InventariosCondensadosPorIC(_ctx, _proyectoID).ToList();

            //Ejecutar el proceso
            estableceVariablesTemporales();
            generaDiccionarios();
            recorreSpoolsYCongela();

            //Regresar una lista con los nus congelados
            congelados = (from nus in _lstNumeroUnico
                          where nus.InfoCruce.SeUsoEnCruce
                          select nus).ToList();

            //Regresar los spools que se tienen que usar para la ODT
            return _lstSpools;
        }

        /// <summary>
        /// Genera diccionarios de item codes y spools para acceso rápido
        /// </summary>
        private void generaDiccionarios()
        {
            _dicItemCodes = _lstItemCode.ToDictionary(x => x.ItemCodeID);
            _dicSpools = _lstSpools.ToDictionary(x => x.SpoolID);
            _dicCodensados = _lstCondensadoIC.ToDictionary(x => new ItemCodeIntegrado { ItemCodeID = x.ItemCodeID, Diametro1 = x.Diametro1, Diametro2 = x.Diametro2 });
        }



        /// <summary>
        /// Para poder afectar los inventarios spool x spool necesitamos poder llevar tracking de esos cambios
        /// en otras variables.
        /// </summary>
        private void estableceVariablesTemporales()
        {
            //Copia congelados y disponibles a temporales
            _lstNumeroUnico.ForEach(x =>
            {
                x.InfoCruce.SeUsoEnCruce = false;
                x.NumeroUnicoInventario.InfoCruce.InventarioCongeladoTemporal = x.NumeroUnicoInventario.InventarioCongelado;
                x.NumeroUnicoInventario.InfoCruce.InventarioDisponibleCruceTemporal = x.NumeroUnicoInventario.InventarioDisponibleCruce;

                foreach (NumeroUnicoSegmento nus in x.NumeroUnicoSegmento)
                {
                    nus.InfoCruce.InventarioCongeladoTemporal = nus.InventarioCongelado;
                    nus.InfoCruce.InventarioDisponibleCruceTemporal = nus.InventarioDisponibleCruce;
                }
            });


            //asumimos que no se va a poder cruzar
            _lstSpools.ForEach(x =>
            {
                x.InfoCruce.CruceExitoso = false;
                x.InfoCruce.UsoEquivalencia = false;
            });

            //copiar las cantidades de cruce y congelados a temporales
            _lstCondensadoIC.ForEach(x =>
            {
                x.InventarioDisponibleCruceTemporal = x.InventarioDisponibleCruce;
                x.InventarioCongeladoTemporal = x.InventarioCongelado;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void recorreSpoolsYCongela()
        {
            foreach (Spool spool in _lstSpools)
            {
                List<NumeroUnico> inventarios = UtileriasCruce.NumerosUnicosDisponibles(spool, _lstNumeroUnico);
                List<NumeroUnico> equivalentes = new List<NumeroUnico>();

                spool.InfoCruce.CruceExitoso = UtileriasCruce.BuscaMaterial(spool, inventarios, equivalentes, _lstCongeladoParcial, _lstEquivalente, _lstNumeroUnico,_dicCodensados, _dicItemCodes);

                if (spool.InfoCruce.CruceExitoso)
                {
                    //Congelamos pero hay utilizar la opcion de tracking del EF para
                    //luego poder ir a afectar los inventarios
                    UtileriasCruce.CongelaInventarios(inventarios, _dicCodensados, true, _userID);
                    UtileriasCruce.CongelaInventarios(equivalentes, _dicCodensados, true, _userID);
                }
                else
                {
                    UtileriasCruce.QuitaCongelados(inventarios, _dicCodensados);
                    UtileriasCruce.QuitaCongelados(equivalentes, _dicCodensados);
                }
            }
        }

        private void recorreSpoolsRevisionYCongela()
        {
            foreach (Spool spool in _lstSpools)
            {
                List<NumeroUnico> inventarios = UtileriasCruce.NumerosUnicosDisponibles(spool, _lstNumeroUnico);
                List<NumeroUnico> equivalentes = new List<NumeroUnico>();

                spool.InfoCruce.CruceExitoso = UtileriasCruce.BuscaMaterialSpoolMarcadosRevision(spool, inventarios, equivalentes, _lstEquivalente, _lstNumeroUnico, _dicCodensados, _dicItemCodes);

                if (spool.InfoCruce.CruceExitoso)
                {
                    //Congelamos pero hay utilizar la opcion de tracking del EF para
                    //luego poder ir a afectar los inventarios
                    UtileriasCruce.CongelaInventarios(inventarios, _dicCodensados, true, _userID);
                    UtileriasCruce.CongelaInventarios(equivalentes, _dicCodensados, true, _userID);
                }
                else
                {
                    UtileriasCruce.QuitaCongelados(inventarios, _dicCodensados);
                    UtileriasCruce.QuitaCongelados(equivalentes, _dicCodensados);
                }
            }
        }

    }
}
