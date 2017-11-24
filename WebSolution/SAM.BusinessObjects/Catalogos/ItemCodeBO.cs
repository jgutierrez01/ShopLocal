using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Mimo.Framework.Common;
using Mimo.Framework.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Validations;
using SAM.Entities.Grid;
using System.Data.Objects;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.RadCombo;
using System;
using Mimo.Framework.Extensions;

namespace SAM.BusinessObjects.Catalogos
{
    public class ItemCodeBO
    {
        private static readonly object _mutex = new object();
        private static ItemCodeBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ItemCodeBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ItemCodeBO
        /// </summary>
        /// <returns></returns>
        public static ItemCodeBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ItemCodeBO();
                    }
                }
                return _instance;
            }
        }

        public ItemCode Obtener(int itemCodeID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ItemCode.Where(x => x.ItemCodeID == itemCodeID).SingleOrDefault();
            }
        }

        public List<ItemCode> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ItemCode.ToList();
            }
        }

        public ItemCode ObtenerConTipoMaterial(int itemCodeID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ItemCode.Include("TipoMaterial").Where(x => x.ItemCodeID == itemCodeID).SingleOrDefault();
            }
        }

        public List<ItemCode> ObtenerPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ItemCode.Where(x => x.ProyectoID == proyectoID).ToList();
            }
        }

        /// <summary>
        /// Obtiene el listado de item codes para un proyecto con su información de material
        /// </summary>
        /// <param name="proyectoID">ID del proyecto del cual se desean obtener los item codes</param>
        /// <returns></returns>
        public List<GrdItemCode> ObtenerListaPorProyecto(int proyectoID)
        {
            List<GrdItemCode> lst = null;

            using (SamContext ctx = new SamContext())
            {
                ctx.ItemCode.MergeOption = MergeOption.NoTracking;
                
                lst =
                ctx.ItemCode.Include("FamiliaAcero").Where(x => x.ProyectoID == proyectoID)
                                                    .Select(x =>
                                                    new GrdItemCode
                                                    {
                                                        ItemCodeID = x.ItemCodeID,
                                                        Codigo = x.Codigo,
                                                        Descripcion = x.DescripcionEspanol,
                                                        DescripcionIngles = x.DescripcionIngles,
                                                        ProyectoID = x.ProyectoID,
                                                        TipoMaterialID = x.TipoMaterialID,
                                                        Peso = x.Peso.HasValue ? x.Peso.Value : 0,
                                                        DescripcionInterna = x.DescripcionInterna,
                                                        Diametro1 = x.Diametro1,
                                                        Diametro2 = x.Diametro2,
                                                        FamiliaAcero = x.FamiliaAcero
                                                    }).ToList();
            }

            Dictionary<int, string> dicTm = CacheCatalogos.Instance.ObtenerTipoMaterial().ToDictionary(x => x.ID, y => y.Nombre);

            lst.ForEach(x => x.TipoMaterial = dicTm[x.TipoMaterialID]);

            return lst;
        }

        public List<ItemCode> ObtenerListaPorProyectoID(int proyectoID)
        {

            using (SamContext ctx = new SamContext())
            {

                List<ItemCode> lst = ctx.ItemCode.Include("FamiliaAcero").Where(x => x.ProyectoID == proyectoID).ToList();
                return lst;
            }
        }

        public void Guarda(ItemCode itemCode)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesItemCode.CodigoDuplicado(ctx, itemCode.Codigo, itemCode.ItemCodeID, itemCode.ProyectoID))
                    {
                        throw new ExcepcionDuplicados(MensajesError.Excepcion_ItemCodeDuplicado);
                    }

                    ctx.ItemCode.ApplyChanges(itemCode);

                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        public void CambioTipoMaterial(ItemCode itemcode, int proyectoId, Guid userId, int nuevoTipoMaterial)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    //obtener todos los numeros unicos que usan el iemcode en cuestion, junto con sus inventarios y segmentos
                    List<NumeroUnico> numerosUnicosAfectados = ctx.NumeroUnico
                                                                    .Include("NumeroUnicoInventario")
                                                                    .Where(x => x.ItemCodeID == itemcode.ItemCodeID)
                                                                    .ToList();
                    if (nuevoTipoMaterial == TipoMaterialEnum.Tubo.SafeIntParse())
                    {
                        //generar un nuevo semento por cada numero unico afectado
                        foreach (NumeroUnico nu in numerosUnicosAfectados)
                        {
                            nu.StartTracking();
                            NumeroUnicoSegmento segmento = new NumeroUnicoSegmento();
                            segmento.Segmento = "A";
                            segmento.NumeroUnicoID = nu.NumeroUnicoID;
                            segmento.ProyectoID = nu.ProyectoID;
                            segmento.CantidadDanada = nu.NumeroUnicoInventario.CantidadDanada;
                            segmento.InventarioFisico = nu.NumeroUnicoInventario.InventarioFisico;
                            segmento.InventarioBuenEstado = nu.NumeroUnicoInventario.InventarioBuenEstado;
                            segmento.InventarioCongelado = nu.NumeroUnicoInventario.InventarioCongelado;
                            segmento.InventarioDisponibleCruce = nu.NumeroUnicoInventario.InventarioDisponibleCruce;
                            segmento.Rack = null;
                            segmento.UsuarioModifica = userId;
                            segmento.FechaModificacion = DateTime.Now;
                            nu.NumeroUnicoSegmento.Add(segmento);
                            nu.StopTracking();
                            ctx.NumeroUnico.ApplyChanges(nu);

                            //obtener las ODTM de los numeros unicos afectados
                            List<OrdenTrabajoMaterial> odtms = ctx.OrdenTrabajoMaterial.Where(x => x.NumeroUnicoCongeladoID == nu.NumeroUnicoID).ToList();
                            foreach (OrdenTrabajoMaterial odtm in odtms)
                            {
                                odtm.StartTracking();
                                odtm.SegmentoCongelado = "A";
                                odtm.FechaModificacion = DateTime.Now;
                                odtm.UsuarioModifica = userId;
                                odtm.StopTracking();
                                ctx.OrdenTrabajoMaterial.ApplyChanges(odtm);
                            }
                        }
                    }
                    else if (nuevoTipoMaterial == TipoMaterialEnum.Accessorio.SafeIntParse())
                    {
                        //Borrar todos los segmentos de los numeros unicos que se comvierten de tubo a accesorio
                        List<int> nuIds = numerosUnicosAfectados.Select(x => x.NumeroUnicoID).ToList();
                        List<NumeroUnicoSegmento> segmentos = ctx.NumeroUnicoSegmento
                                                                .Where(x => nuIds.Contains(x.NumeroUnicoID)).ToList();
                        segmentos.ForEach(x => ctx.DeleteObject(x));

                        //obtener las ODTM de los numeros unicos afectados
                        List<OrdenTrabajoMaterial> materiales = ctx.OrdenTrabajoMaterial.Where(y => nuIds.Contains(y.NumeroUnicoCongeladoID.Value)).ToList();

                        foreach (OrdenTrabajoMaterial odtm in materiales)
                        {
                            odtm.StartTracking();
                            odtm.SegmentoCongelado = null;
                            odtm.FechaModificacion = DateTime.Now;
                            odtm.UsuarioModifica = userId;
                            odtm.StopTracking();
                            ctx.OrdenTrabajoMaterial.ApplyChanges(odtm);
                        }
                    }
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(MensajesError.Excepcion_ErrorConcurrencia);
            }
        }

        /// <summary>
        /// elimina una entidad de item code a partir del ID que se envía.
        /// en caso de un error de concurrencia, regresa un mensaje.
        /// </summary>
        /// <param name="itemCodeID"></param>
        public void Borra(int itemCodeID)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (ValidacionesItemCode.TieneRelaciones(ctx, itemCodeID))
                    {
                        throw new ExcepcionRelaciones(MensajesError.Excepcion_RelacionItemCode);
                    }

                    ItemCode itemCode = ctx.ItemCode.Where(x => x.ItemCodeID == itemCodeID).Single();
                    ctx.ItemCode.DeleteObject(itemCode);
                    ctx.SaveChanges();
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string> { MensajesError.Excepcion_ErrorConcurrencia });
            }
        }

        /// <summary>
        /// Obtiene el tipo de material de un item code en especifico
        /// </summary>
        /// <param name="itemCodeID"></param>
        /// <returns></returns>
        public int ObtenTipoMaterial(int itemCodeID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ItemCode.Where(x => x.ItemCodeID == itemCodeID).Single().TipoMaterialID;
            }
        }

        public List<RadItemCode> ObtenerPorProyectoParaCombo(int proyectoID, string contextText, int skip, int take)
        {
            List<RadItemCode> result = new List<RadItemCode>(take * 2);

            using (SamContext ctx = new SamContext())
            {
                ctx.ItemCode.MergeOption = MergeOption.NoTracking;

                //Item codes por proyecto
                result =
                    (from x in ctx.ItemCode
                                  .Where(y => y.ProyectoID == proyectoID)
                     select new RadItemCode
                     {
                         ItemCodeID = x.ItemCodeID,
                         Codigo = x.Codigo,
                         Descripcion = x.DescripcionEspanol
                     })
                    .ToList();

                return result.Where(x => x.Descripcion.ContainsIgnoreCase(contextText)
                                      || x.Codigo.ContainsIgnoreCase(contextText))
                             .OrderBy(x => x.Codigo)
                             .Skip(skip)
                             .Take(take)
                             .ToList();
            }
        }

        public DataSet ObtenerLstMatItemCodePorProyecto(int proyectoID)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();


            DataSet ds = new DataSet();
            const string nombreProc = "ObtenerConsultaDeItemCode";
            string nombreTablas = "ItemCode";
            using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
            {
                IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
                parameters[0].Value = proyectoID;


                ds = DataAccess.ExecuteDataset(connection,
                                               CommandType.StoredProcedure,
                                               nombreProc,
                                               ds,
                                               nombreTablas,
                                               parameters);

                DataTable ItemCode = ds.Tables["ItemCode"];

                sw.Stop();
                return ds;
            }
        }

        /// <summary>
        /// Regresa el ID del proyecto al cual pertenece el item code.
        /// </summary>
        /// <param name="itemCodeID">ID del item code</param>
        /// <returns>Id del proyecto al cual pertenece el item code</returns>
        public int ObtenerProyectoID(int itemCodeID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoItemCode(ctx, itemCodeID);
            }
        }

        /// <summary>
        /// Versión compilada del query para permisos de item code
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoItemCode =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.ItemCode
                          .Where(x => x.ItemCodeID == id)
                          .Select(x => x.ProyectoID)
                          .Single()
        );
    }
}
