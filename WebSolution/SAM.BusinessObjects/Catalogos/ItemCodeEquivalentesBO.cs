using System.Collections.Generic;
using System.Data;
using System.Linq;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Grid;
using System.Web.UI.WebControls;
using System.Data.Objects;
using System;
using Mimo.Framework.Extensions;

namespace SAM.BusinessObjects.Catalogos
{
    public class ItemCodeEquivalentesBO
    {
        private static readonly object _mutex = new object();
        private static ItemCodeEquivalentesBO _instance;

        /// <summary>
        /// 
        /// </summary>
        private ItemCodeEquivalentesBO()
        {
        }

        public static ItemCodeEquivalentesBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ItemCodeEquivalentesBO();
                    }
                }
                return _instance;
            }
        }

        public List<GrdIcEquivalenteAgrupado> ObtenerAgrupadosPorProyecto(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;

                List<GrdIcEquivalenteAgrupado> lstEquiv = (from ice in ctx.ItemCodeEquivalente
                                                           let ic = ice.ItemCode
                                                           where ic.ProyectoID == proyectoID
                                                           group ice by new { ice.ItemCodeID, ic.DescripcionEspanol, ic.Codigo, ice.Diametro1, ice.Diametro2 } into grupo
                                                           select new GrdIcEquivalenteAgrupado
                                                           {
                                                               MinItemCodeEquivalenteID = (int)grupo.Min(x => x.ItemCodeEquivalenteID),
                                                               NumEquivalencias = grupo.Count(),
                                                               Codigo = grupo.Key.Codigo,
                                                               ItemCodeID = grupo.Key.ItemCodeID,
                                                               Diametro1 = grupo.Key.Diametro1,
                                                               Diametro2 = grupo.Key.Diametro2,
                                                               Descripcion = grupo.Key.DescripcionEspanol
                                                           }).ToList();

                return lstEquiv;
            }
        }

        public List<GrdIcEquivalenteAgrupado> ObtenerAgrupadosPorItemCodes(List<ItemCodeEquivalente> lstEq)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;

                List<GrdIcEquivalenteAgrupado> lstEquiv = (from ice in lstEq
                                                           join ic in ctx.ItemCode on ice.ItemCodeID equals ic.ItemCodeID
                                                           group ice by new { ice.ItemCodeID, ic.DescripcionEspanol, ic.Codigo, ice.Diametro1, ice.Diametro2 } into grupo
                                                           select new GrdIcEquivalenteAgrupado
                                                           {
                                                               MinItemCodeEquivalenteID = (int)grupo.Min(x => x.ItemCodeEquivalenteID),
                                                               NumEquivalencias = grupo.Count(),
                                                               Codigo = grupo.Key.Codigo,
                                                               ItemCodeID = grupo.Key.ItemCodeID,
                                                               Diametro1 = grupo.Key.Diametro1,
                                                               Diametro2 = grupo.Key.Diametro2,
                                                               Descripcion = grupo.Key.DescripcionEspanol
                                                           }).ToList();

                return lstEquiv;
            }
        }

        public List<ItemCodeEquivalente> ObtenerAgrupadosPorProyectoID(int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;

                List<ItemCodeEquivalente> lstEquiv = (from ice in ctx.ItemCodeEquivalente
                                                      let ic = ice.ItemCode
                                                      where ic.ProyectoID == proyectoID
                                                      select ice).ToList();

                return lstEquiv;
            }
        }

        public List<GrdItemCodeEquivalente> ObtenerGrupoDeEquivalencias(int itemCodeEquivalenteID)
        {
            using (SamContext ctx = new SamContext())
            {
                ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;
                ctx.ItemCode.MergeOption = MergeOption.NoTracking;

                ItemCodeEquivalente entidad = ctx.ItemCodeEquivalente
                                                 .Where(x => x.ItemCodeEquivalenteID == itemCodeEquivalenteID)
                                                 .Single();

                List<GrdItemCodeEquivalente> lst =
                    (from ice in ctx.ItemCodeEquivalente
                     join ic in ctx.ItemCode on ice.ItemCodeID equals ic.ItemCodeID
                     join icEq in ctx.ItemCode on ice.ItemEquivalenteID equals icEq.ItemCodeID
                     where ice.ItemCodeID == entidad.ItemCodeID
                             && ice.Diametro1 == entidad.Diametro1
                             && ice.Diametro2 == entidad.Diametro2
                     select new GrdItemCodeEquivalente
                     {
                         ItemCodeEquivalenteID = ice.ItemCodeEquivalenteID,
                         ItemCodeID = ic.ItemCodeID,
                         CodigoIC = ic.Codigo,
                         DescripcionIC = ic.DescripcionEspanol,
                         D1 = ice.Diametro1,
                         D2 = ice.Diametro2,
                         ItemEquivalenteID = icEq.ItemCodeID,
                         CodigoEq = icEq.Codigo,
                         DescripcionEq = icEq.DescripcionEspanol,
                         D1Eq = ice.DiametroEquivalente1,
                         D2Eq = ice.DiametroEquivalente2,
                         ProyectoID = ic.ProyectoID
                     }).ToList();

                return lst;
            }
        }

        public int ObtenerItemCodeEquivalentePorItemCode(int itemCodeID, decimal diam1, decimal diam2)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ItemCodeEquivalente.Where(x => x.ItemCodeID == itemCodeID && x.Diametro1 == diam1 && x.Diametro2 == diam2).Select(x => x.ItemCodeEquivalenteID).Min();
            }
        }

        public bool TieneItemCodeEquivalente(int itemCodeID, decimal diam1, decimal diam2)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ItemCodeEquivalente.Where(x => x.ItemCodeID == itemCodeID && x.Diametro1 == diam1 && x.Diametro2 == diam2).Any();
            }
        }

        /// <summary>
        /// Regresa el ID del proyecto al cual pertenece el item code del item code equivalente pasado.
        /// </summary>
        /// <param name="itemCodeEquivalenteID">ID del item code equivalente</param>
        /// <returns>Id del proyecto al cual pertenece el item code del IC equivalente</returns>
        public int ObtenerProyectoID(int itemCodeEquivalenteID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoItemCodeEquivalente(ctx, itemCodeEquivalenteID);
            }
        }

        /// <summary>
        /// Regresa una lista de item codes equivalentes cuyos codigos de IC se encuentren en el proyecto para que puedan ser importadas.
        /// </summary>
        /// <param name="proyectoID">ID del proyecto al que se desean importar las equivalencias</param>
        /// <param name="pExportID">ID del proyecto del que se obtienen las equivalencias</param>
        /// <param name="proyExport">Listado de equivalencias encontrado en el proyecto del que se desea exportar</param>
        /// <returns>Listado de equivalencias con relacion entre ambos proyectos</returns>
        public List<ItemCodeEquivalente> ObtenEquivalenciasRelacionadas(int proyectoID, int pExportID, List<ItemCodeEquivalente> proyExport)
        {
            List<ItemCodeEquivalente> lstEq = new List<ItemCodeEquivalente>();

            List<ICRelacion> iCRelacion = ObtenRelacionEntreIC(proyectoID, pExportID);

            lstEq = (from ic in iCRelacion
                     join ice in proyExport on ic.ItemCodeB equals ice.ItemCodeID
                     select ice).ToList();



            if (lstEq.Count == 0)
            {
                throw new ExcepcionConcordancia(MensajesError.Exception_EquivalentesCero);
            }

            return lstEq;


        }

        /// <summary>
        /// Regresa un listado de relaciones entre itemcodes de dos proyectos obteniendo el ID del IC de un proyecto aunado al ID correspondiente al mismos IC en el segundo proyecto
        /// </summary>
        /// <param name="proyectoID">Proyecto A</param>
        /// <param name="pExportID">Proyecto B</param>
        /// <returns>Listado de ICRelacion {CodigoA (Codigo del IC), ItemCodeA (ID del IC en el proyecto A, ItemCodeB (ID del IC en el proyecto B)}</returns>
        public List<ICRelacion> ObtenRelacionEntreIC(int proyectoID, int pExportID)
        {
            List<ICRelacion> iCRelacion;

            using (SamContext ctx = new SamContext())
            {
                iCRelacion = (from ica in ctx.ItemCode
                              join icb in ctx.ItemCode on ica.Codigo equals icb.Codigo
                              where ica.ProyectoID == proyectoID && icb.ProyectoID == pExportID
                              select new ICRelacion
                              {
                                  CodigoA = ica.Codigo,
                                  ItemCodeA = ica.ItemCodeID,
                                  ItemCodeB = icb.ItemCodeID
                              }
                             ).ToList();

            }

            return iCRelacion;
        }

        public void ImportaIcEq(List<ItemCodeEquivalente> lst, int proyA, int proyB, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                List<ICRelacion> iCRelacion = ItemCodeEquivalentesBO.Instance.ObtenRelacionEntreIC(proyA, proyB);
                List<ItemCode> itemCodesProyA = ItemCodeBO.Instance.ObtenerListaPorProyectoID(proyA);
                List<ItemCode> itemCodesProyB = ItemCodeBO.Instance.ObtenerListaPorProyectoID(proyB);
                List<ItemCodeEquivalente> icDuplicados = new List<ItemCodeEquivalente>();
                string errores = string.Empty;

                foreach (ICRelacion icr in iCRelacion)
                {
                    List<ItemCodeEquivalente> listF = lst.Where(x => x.ItemCodeID == icr.ItemCodeB)
                        .Select(x => new ItemCodeEquivalente
                        {
                            ItemCodeID = icr.ItemCodeA,
                            Diametro1 = x.Diametro1,
                            Diametro2 = x.Diametro2,
                            ItemEquivalenteID = iCRelacion.Where(y => y.ItemCodeB == x.ItemEquivalenteID).Select(y => y.ItemCodeA).FirstOrDefault().SafeIntParse(),
                            DiametroEquivalente1 = x.DiametroEquivalente1,
                            DiametroEquivalente2 = x.DiametroEquivalente2,
                            UsuarioModifica = userID,
                            FechaModificacion = DateTime.Now
                        }).ToList();

                    foreach (ItemCodeEquivalente ice in listF)
                    {
                        ItemCodeEquivalente duplicado = ctx.ItemCodeEquivalente.Include("ItemCode").Where(x =>
                            x.ItemCodeID == ice.ItemCodeID && x.Diametro1 == ice.Diametro1 && x.Diametro2 == ice.Diametro2 &&
                            x.ItemEquivalenteID == ice.ItemEquivalenteID && x.DiametroEquivalente1 == ice.DiametroEquivalente1 && x.DiametroEquivalente2 == ice.DiametroEquivalente2).SingleOrDefault();

                        if (duplicado != null)
                        {
                            icDuplicados.Add(duplicado);
                        }
                    }

                    if (icDuplicados.Count > 0)
                    {
                        icDuplicados.ForEach(x => errores += x.ItemCode.Codigo + " D1:" + x.Diametro1 + " D2:" + x.Diametro2 + " - " + x.ItemCode1.Codigo + " D1:" + x.DiametroEquivalente1 + " D2:" + x.DiametroEquivalente2 + "<br/>");

                        throw new ExcepcionDuplicados(string.Format(MensajesError.Excepcion_EquivalenciaExistente, errores));
                    }

                    listF.ForEach(x => ctx.ItemCodeEquivalente.ApplyChanges(x));
                }

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Versión compilada del query para permisos de item code equivalente
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoItemCodeEquivalente =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.ItemCodeEquivalente
                            .Where(x => x.ItemCodeEquivalenteID == id)
                            .Select(x => x.ItemCode.ProyectoID)
                            .Single()
        );



    }

    /// <summary>
    /// Subclase utilizada para la relacion entre item codes de un proyecto y relaciones obtenidas de otro.
    /// </summary>
    public class ICRelacion
    {
        public int ItemCodeA;
        public int ItemCodeB;
        public string CodigoA;
    }
}
