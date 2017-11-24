using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Personalizadas;
using System.Data;
using System.Transactions;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Extensions;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Produccion
{
    /// <summary>
    /// 
    /// </summary>
    public class JuntaCampoArmadoBO
    {
        private static readonly object _mutex = new object();
        private static JuntaCampoArmadoBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private JuntaCampoArmadoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase JuntaCampoArmadoBO
        /// </summary>
        /// <returns></returns>
        public static JuntaCampoArmadoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new JuntaCampoArmadoBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="etiquetaDeMaterial"></param>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public List<Simple> ObtenerNumerosUnicosCandidatosParaArmadoCampo(string etiquetaDeMaterial, int spoolID)
        {
            List<Simple> lista = new List<Simple>();
            string etiquetaPadded = etiquetaDeMaterial.PadLeft(15, '0');
            int shopID = CacheCatalogos.Instance.ShopFabAreaID;

            using (SamContext ctx = new SamContext())
            {
                bool tieneJuntasShop = ctx.JuntaSpool.Any(js => js.SpoolID == spoolID && js.FabAreaID == shopID);

                //Si el spool no tiene juntas shop se trata de un spool de un solo tramo
                if (!tieneJuntasShop)
                {
                    lista = obtenNusCandidatosSpoolUnSoloMaterial(spoolID, lista, etiquetaPadded, ctx);
                }
                else
                {
                    lista = ontenNusCandidatosEnBaseAJuntas(spoolID, lista, etiquetaPadded, shopID, ctx);
                }
            }

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID"></param>
        /// <param name="lista"></param>
        /// <param name="etiquetaPadded"></param>
        /// <param name="shopID"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private List<Simple> ontenNusCandidatosEnBaseAJuntas(int spoolID, List<Simple> lista, string etiquetaPadded, int shopID, SamContext ctx)
        {
            List<JuntaSpool> js = (from j in ctx.JuntaSpool
                                   where j.SpoolID == spoolID
                                            && j.FabAreaID == shopID
                                   select j).ToList();

            IQueryable<int> iJsConEtiquetaMat1 =
                (from j in js
                 where j.EtiquetaMaterial1.PadLeft(15, '0').Equals(etiquetaPadded)
                 select j.JuntaSpoolID).AsQueryable();

            IQueryable<int> iJsConEtiquetaMat2 =
                (from j in js
                 where j.EtiquetaMaterial2.PadLeft(15, '0').Equals(etiquetaPadded)
                 select j.JuntaSpoolID).AsQueryable();


            var nuMat1 = (from nu in ctx.NumeroUnico
                          join ja in ctx.JuntaArmado on nu.NumeroUnicoID equals ja.NumeroUnico1ID
                          join jw in ctx.JuntaWorkstatus on ja.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                          where jw.ArmadoAprobado == true
                                    && jw.SoldaduraAprobada == true
                                    && jw.JuntaFinal == true
                                    && iJsConEtiquetaMat1.Contains(jw.JuntaSpoolID)
                          select new { ID = nu.NumeroUnicoID, Codigo = nu.Codigo }
                          ).ToList();

            var nuMat2 = (from nu in ctx.NumeroUnico
                          join ja in ctx.JuntaArmado on nu.NumeroUnicoID equals ja.NumeroUnico2ID
                          join jw in ctx.JuntaWorkstatus on ja.JuntaWorkstatusID equals jw.JuntaWorkstatusID
                          where jw.ArmadoAprobado == true
                                    && jw.SoldaduraAprobada == true
                                    && jw.JuntaFinal == true
                                    && iJsConEtiquetaMat2.Contains(jw.JuntaSpoolID)
                          select new { ID = nu.NumeroUnicoID, Codigo = nu.Codigo }
                          ).ToList();

            lista = nuMat1.Union(nuMat2)
                          .Distinct()
                          .Select(x => new Simple
                          {
                              ID = x.ID,
                              Valor = x.Codigo
                          })
                          .OrderBy(n => n.Valor)
                          .ToList();
            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID"></param>
        /// <param name="lista"></param>
        /// <param name="etiquetaPadded"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private List<Simple> obtenNusCandidatosSpoolUnSoloMaterial(int spoolID, List<Simple> lista, string etiquetaPadded, SamContext ctx)
        {
            var lMatSpool = (from ms in ctx.MaterialSpool
                             where ms.SpoolID == spoolID
                             select new { MaterialSpoolID = ms.MaterialSpoolID, Etiqueta = ms.Etiqueta }).ToList();

            var iqMatSpool = (from m in lMatSpool
                              where m.Etiqueta.PadLeft(15, '0').Equals(etiquetaPadded)
                              select m.MaterialSpoolID).AsQueryable();

            lista = (from nu in ctx.NumeroUnico
                     join odtm in ctx.OrdenTrabajoMaterial on nu.NumeroUnicoID equals odtm.NumeroUnicoDespachadoID
                     where iqMatSpool.Contains(odtm.MaterialSpoolID)
                            && odtm.TieneDespacho == true
                     select new Simple
                     {
                         ID = nu.NumeroUnicoID,
                         Valor = nu.Codigo
                     }).ToList();

            return lista;
        }

        /// <summary>
        /// Va por las juntas del spool seleccionado y regresa aquellas etiquetas de material que ya cuenten con armado.
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public List<string> ObtenerEtiquetasDeMaterialArmadas(int spoolID)
        {
            int shopID = CacheCatalogos.Instance.ShopFabAreaID;

            List<string> lista = new List<string>();

            using (SamContext ctx = new SamContext())
            {
                bool tieneJuntasShop = ctx.JuntaSpool.Any(js => js.SpoolID == spoolID && js.FabAreaID == shopID);

                if (tieneJuntasShop)
                {
                    lista = obtenEtiquetasDeMaterialEnBaseAJuntas(spoolID, ctx);
                }
                else
                {
                    lista = obtenEtiquetasDeMaterialSpoolUnSoloTramo(spoolID, ctx);
                }
            }

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private List<string> obtenEtiquetasDeMaterialSpoolUnSoloTramo(int spoolID, SamContext ctx)
        {
            List<string> lista = new List<string>();

            lista = (from ms in ctx.MaterialSpool
                     join odtm in ctx.OrdenTrabajoMaterial on ms.MaterialSpoolID equals odtm.MaterialSpoolID
                     join sp in ctx.Spool on ms.SpoolID equals sp.SpoolID
                     where sp.SpoolID == spoolID
                            && odtm.NumeroUnicoDespachadoID != null
                            && odtm.TieneDespacho == true
                     select ms.Etiqueta)
                     .ToList()
                     .OrderBy(x => x.PadLeft(5, '0'))
                     .ToList();

            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spoolID"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        private List<string> obtenEtiquetasDeMaterialEnBaseAJuntas(int spoolID, SamContext ctx)
        {
            List<string> lista = new List<string>();

            IQueryable<string> etiquetaMaterial1 =
                (from js in ctx.JuntaSpool
                 join jw in ctx.JuntaWorkstatus on js.JuntaSpoolID equals jw.JuntaSpoolID
                 where jw.ArmadoAprobado == true && jw.JuntaFinal == true && jw.SoldaduraAprobada == true
                       && js.SpoolID == spoolID
                 select js.EtiquetaMaterial1).AsQueryable();

            IQueryable<string> etiquetaMaterial2 =
                (from js in ctx.JuntaSpool
                 join jw in ctx.JuntaWorkstatus on js.JuntaSpoolID equals jw.JuntaSpoolID
                 where jw.ArmadoAprobado == true && jw.JuntaFinal == true && jw.SoldaduraAprobada == true
                       && js.SpoolID == spoolID
                 select js.EtiquetaMaterial2).AsQueryable();

            lista = etiquetaMaterial1.Union(etiquetaMaterial2)
                                     .Distinct()
                                     .ToList()
                                     .OrderBy(x => x.PadLeft(5, '0'))
                                     .ToList();
            return lista;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaSpoolID"></param>
        /// <param name="juntaCampoArmadoID"></param>
        /// <param name="juntaCampoID"></param>
        /// <param name="etiquetaProduccion"></param>
        /// <returns></returns>
        public bool JuntaSpoolTieneArmadoCampo(int juntaSpoolID, out int juntaCampoArmadoID, out int juntaCampoID, out string etiquetaProduccion)
        {
            juntaCampoArmadoID = -1;
            juntaCampoID = -1;
            etiquetaProduccion = string.Empty;

            using (SamContext ctx = new SamContext())
            {
                JuntaCampo jta =
                    ctx.JuntaCampo.Where(jc => jc.JuntaFinal == true && jc.JuntaSpoolID == juntaSpoolID)
                                  .Select(jc => jc)
                                  .SingleOrDefault();

                if (jta != null)
                {
                    if (jta.ArmadoAprobado.HasValue && jta.ArmadoAprobado.Value)
                    {
                        juntaCampoArmadoID = jta.JuntaCampoArmadoID.HasValue ? jta.JuntaCampoArmadoID.Value : -1;
                    }

                    etiquetaProduccion = jta.EtiquetaJunta;
                    juntaCampoID = jta.JuntaCampoID;
                }
            }

            return juntaCampoArmadoID != -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juntaCampoArmadoID"></param>
        /// <returns></returns>
        public JuntaCampoArmadoInfo DatosJuntaCampoArmado(int juntaCampoArmadoID)
        {
            JuntaCampoArmadoInfo info = null;

            using (SamContext ctx = new SamContext())
            {
                info = (from jca in ctx.JuntaCampoArmado
                        let tubero = jca.Tubero
                        let nu1 = jca.NumeroUnico
                        let nu2 = jca.NumeroUnico1
                        let spool1 = jca.Spool
                        let spool2 = jca.Spool1
                        where jca.JuntaCampoArmadoID == juntaCampoArmadoID
                        select new JuntaCampoArmadoInfo
                        {
                            JuntaCampoArmadoID = jca.JuntaCampoArmadoID,
                            JuntaCampoID = jca.JuntaCampoID,
                            Observaciones = jca.Observaciones,
                            Spool1 = spool1.Nombre,
                            Spool1ID = spool1.SpoolID,
                            Spool2 = spool2.Nombre,
                            Spool2ID = spool2.SpoolID,
                            CodigoTubero = tubero.Codigo,
                            TuberoID = tubero.TuberoID,
                            EtiquetaJunta = jca.JuntaCampo.EtiquetaJunta,
                            EtiquetaMaterial1 = jca.EtiquetaMaterial1,
                            EtiquetaMaterial2 = jca.EtiquetaMaterial2,
                            FechaArmado = jca.FechaArmado,
                            FechaReporte = jca.FechaReporte,
                            NumeroUnico1 = nu1.Codigo,
                            NumeroUnico1ID = nu1.NumeroUnicoID,
                            NumeroUnico2 = nu2.Codigo,
                            NumeroUnico2ID = nu2.NumeroUnicoID
                        }).SingleOrDefault();
            }

            return info;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="datosArmado"></param>
        /// <param name="datosJunta"></param>
        /// <param name="userId"></param>
        public void GuardaArmado(JuntaCampoArmadoInfo datosArmado, JuntaSpoolProduccion datosJunta, Guid userId)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SamContext ctx = new SamContext())
                    {
                        JuntaCampo jc;

                        if (datosArmado.JuntaCampoID > 0)
                        {
                            //En caso que la junta de campo ya exista hay que traerla
                            jc = ctx.JuntaCampo.Single(x => x.JuntaCampoID == datosArmado.JuntaCampoID);
                            jc.StartTracking();
                            jc.ArmadoAprobado = true;
                            jc.UsuarioModifica = userId;
                            jc.FechaModificacion = DateTime.Now;
                            jc.UltimoProcesoID = (int)UltimoProcesoEnum.Armado;
                        }
                        else
                        {
                            //Si la junta de campo no existe vamos a crear la primera versión de la junta
                            jc = new JuntaCampo();
                            jc.OrdenTrabajoSpoolID = datosJunta.OrdenTrabajoSpoolID;
                            jc.JuntaSpoolID = datosArmado.JuntaSpoolID;
                            jc.EtiquetaJunta = datosJunta.Etiqueta;
                            jc.ArmadoAprobado = true;
                            jc.SoldaduraAprobada = false;
                            jc.InspeccionVisualAprobada = false;
                            jc.VersionJunta = 1;
                            jc.JuntaCampoAnteriorID = null;
                            jc.JuntaFinal = true;
                            jc.JuntaCampoInspeccionVisualID = null;
                            jc.JuntaCampoSoldaduraID = null;
                            jc.JuntaCampoArmadoID = null;
                            jc.UltimoProcesoID = (int)UltimoProcesoEnum.Armado;
                            jc.UsuarioModifica = userId;
                            jc.FechaModificacion = DateTime.Now;

                            ctx.JuntaCampo.ApplyChanges(jc);
                            ctx.SaveChanges();
                        }

                        JuntaCampoArmado jca = new JuntaCampoArmado();
                        jca.Spool1ID = datosArmado.Spool1ID;
                        jca.EtiquetaMaterial1 = datosArmado.EtiquetaMaterial1;
                        jca.EtiquetaMaterial2 = datosArmado.EtiquetaMaterial2;
                        jca.NumeroUnico1ID = datosArmado.NumeroUnico1ID;
                        jca.Spool2ID = datosArmado.Spool2ID;
                        jca.NumeroUnico2ID = datosArmado.NumeroUnico2ID;
                        jca.TuberoID = datosArmado.TuberoID;
                        jca.Observaciones = datosArmado.Observaciones;
                        jca.FechaArmado = datosArmado.FechaArmado;
                        jca.FechaReporte = datosArmado.FechaReporte;
                        jca.UsuarioModifica = userId;
                        jca.FechaModificacion = DateTime.Now;
                        jca.JuntaCampoID = jc.JuntaCampoID;

                        ctx.JuntaCampoArmado.ApplyChanges(jca);
                        ctx.SaveChanges();

                        jc.JuntaCampoArmadoID = jca.JuntaCampoArmadoID;
                        ctx.SaveChanges();
                    }

                    scope.Complete();
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
        /// <param name="juntaCampoArmadoID"></param>
        /// <param name="userID"></param>
        public void BorraJuntaCampoArmado(int juntaCampoArmadoID, Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                JuntaCampo jc = ctx.JuntaCampo.Single(x => x.JuntaCampoArmadoID == juntaCampoArmadoID);
                JuntaSpool js = ctx.JuntaSpool.Single(x => x.JuntaSpoolID == jc.JuntaSpoolID);

                if ((jc.SoldaduraAprobada.HasValue && jc.SoldaduraAprobada.Value) || jc.JuntaCampoSoldaduraID.HasValue)
                {
                    throw new ExcepcionSoldadura(MensajesError.Excepcion_JuntaConSoldadura);
                }

                string parteDerechaEtiqueta = jc.EtiquetaJunta.Substring(js.Etiqueta.Length);

                //Si se trata de un rechazo no se debe poder borrar el armado de manera directa
                if (parteDerechaEtiqueta.ContainsIgnoreCase("r"))
                {
                    throw new BaseValidationException(MensajesError.Excepcion_JuntaDeRechazoArmada);
                }

                JuntaCampoArmado jca = ctx.JuntaCampoArmado.Where(x => x.JuntaCampoArmadoID == juntaCampoArmadoID).FirstOrDefault();

                jc.StartTracking();
                jc.ArmadoAprobado = false;
                jc.JuntaCampoArmadoID = null;
                jc.UsuarioModifica = userID;
                jc.FechaModificacion = DateTime.Now;
                jc.StopTracking();

                ctx.JuntaCampo.ApplyChanges(jc);

                ctx.JuntaCampoArmado.DeleteObject(jca);

                ctx.SaveChanges();
            }
        }               
    }
}
