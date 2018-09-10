using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Extensions;
using SAM.Entities.Busqueda;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.BusinessObjects.Utilerias;
using SAM.BusinessObjects.Excepciones;
using SAM.Entities.Personalizadas;
using System.Data.Objects;
using SAM.BusinessObjects.Ingenieria;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using SAM.BusinessObjects.Sql;
using SAM.Entities.Personalizadas.Shop;

namespace SAM.BusinessObjects.Produccion
{
    public class OrdenTrabajoSpoolBO
    {
        private static readonly object _mutex = new object();
        private static OrdenTrabajoSpoolBO _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private OrdenTrabajoSpoolBO()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase OrdenTrabajoSpoolBO
        /// </summary>
        public static OrdenTrabajoSpoolBO Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new OrdenTrabajoSpoolBO();
                    }
                }
                return _instance;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <returns></returns>
        public List<GrdMaterialesDespacho> ObtenerMaterialesParaDespacho(int ordenTrabajoSpoolID)
        {
            List<MaterialSpool> materiales;

            using (SamContext ctx = new SamContext())
            {
                //ID del spool al que corresponde
                int spoolID = ctx.OrdenTrabajoSpool
                                 .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                                 .Select(x => x.SpoolID).Single();

                IQueryable<MaterialSpool> query = ctx.MaterialSpool.Where(x => x.SpoolID == spoolID);

                materiales = query.ToList();

                //Traerme al contexto los materiales que ya están en la ODT
                ctx.OrdenTrabajoMaterial
                   .Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID)
                   .ToList();

                //Traerme al contexto los item codes
                ctx.ItemCode.Where(ic => query.Select(x => x.ItemCodeID).Contains(ic.ItemCodeID)).ToList();
            }

            List<GrdMaterialesDespacho> lista = new List<GrdMaterialesDespacho>();

            #region Iterar sobre cada uno de los materiales de ingeniería

            materiales.ForEach(material =>
            {
                GrdMaterialesDespacho despacho = new GrdMaterialesDespacho();

                despacho.MaterialSpoolID = material.MaterialSpoolID;
                despacho.CantidadRequerida = material.Cantidad;
                despacho.CodigoItemCode = material.ItemCode.Codigo;
                despacho.DescripcionItemCode = material.ItemCode.DescripcionEspanol;
                despacho.Diametro1 = material.Diametro1;
                despacho.Diametro2 = material.Diametro2;
                despacho.EsTubo = material.ItemCode.TipoMaterialID == (int)TipoMaterialEnum.Tubo;
                despacho.EtiquetaMaterial = material.Etiqueta;
                despacho.ItemCodeID = material.ItemCodeID;
                despacho.TieneHold = SpoolHoldBO.Instance.TieneHold(material.SpoolID);

                //Si está en la ODT
                if (material.OrdenTrabajoMaterial.Count > 0)
                {
                    OrdenTrabajoMaterial odtm = material.OrdenTrabajoMaterial[0];
                    despacho.OrdenTrabajoMaterialID = odtm.OrdenTrabajoMaterialID;
                    despacho.PerteneceAOdt = true;
                    despacho.TieneCorte = odtm.TieneCorte.HasValue ? odtm.TieneCorte.Value : false;
                    despacho.TieneDespacho = odtm.TieneDespacho;
                    despacho.TieneInventarioCongelado = odtm.TieneInventarioCongelado;
                }
                else
                {
                    despacho.OrdenTrabajoMaterialID = -1;
                    despacho.PerteneceAOdt = false;
                    despacho.TieneCorte = false;
                    despacho.TieneDespacho = false;
                    despacho.TieneInventarioCongelado = false;
                }

                #region Calcular el "estatus" del material en lo que se refiere al despacho

                //Calcular el estatus en base a lo que dice el spec
                if (!despacho.PerteneceAOdt)
                {
                    despacho.Estatus = EstatusMaterialDespacho.SinOdt;
                }
                else if (despacho.TieneDespacho)
                {
                    despacho.Estatus = EstatusMaterialDespacho.Despachado;
                }
                else if (!despacho.EsTubo)
                {
                    despacho.Estatus = despacho.TieneInventarioCongelado ? EstatusMaterialDespacho.AccesorioCongelado : EstatusMaterialDespacho.AccesorioNoCongelado;
                }
                else
                {
                    if (despacho.TieneCorte)
                    {
                        despacho.Estatus = EstatusMaterialDespacho.TuboConCorte;
                    }
                    else if (despacho.TieneInventarioCongelado)
                    {
                        despacho.Estatus = EstatusMaterialDespacho.TuboCongelado;
                    }
                    else
                    {
                        despacho.Estatus = EstatusMaterialDespacho.TuboNoCongelado;
                    }
                }

                despacho.EstatusTexto = TraductorEnumeraciones.TextoEstatusMaterialDespacho(despacho.Estatus);

                #endregion

                lista.Add(despacho);
            });

            #endregion

            return lista;
        }

        /// <summary>
        /// Obtiene el OrdenTabajoSpool con la orden de trabajo en base a ordenTrabajoSpoolID
        /// </summary>
        /// <param name="noControl">Orden Trabajo Spool ID</param>
        /// <returns></returns>
        public OrdenTrabajoSpool ObtenerConOrdenTrabajo(int ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Include("OrdenTrabajo.Taller").Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID).SingleOrDefault(); ;
            }
        }

        /// <summary>
        /// Obtiene el SpoolID en base a un Numero de Control
        /// </summary>
        /// <param name="noControl">Numero de Control de Orden Trabajo Spool</param>
        /// <returns></returns>
        public int? ObtenerSpoolIDPorNumeroDeControl(String noControl)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Where(x => x.NumeroControl == noControl).Select(y => y.SpoolID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene la lista de OrdenesTrabajoSpool Por Numero de Control
        /// </summary>
        /// <param name="noControl"></param>
        /// <returns></returns>
        public List<DetSpoolMobile> ObtenerListaOrdenTrabajoSpoolPorNumeroDeControl(String noControl)
        {
            List<DetSpoolMobile> detalleSpool;

            using (SamContext ctx = new SamContext())
            {
                detalleSpool = (from ots in ctx.OrdenTrabajoSpool
                                join spl in ctx.Spool on ots.SpoolID equals spl.SpoolID
                                join prh in ctx.Proyecto on spl.ProyectoID equals prh.ProyectoID
                                where ots.NumeroControl == noControl
                                select new DetSpoolMobile
                                {
                                    SpoolID = ots.SpoolID,
                                    NumeroControl = ots.NumeroControl,
                                    ProyectoID = spl.ProyectoID,
                                    PatioID = prh.PatioID,
                                    Proyecto = prh.Nombre
                                }
                               ).ToList();

            }
            return detalleSpool;
        }

        /// <summary>
        /// Obtiene OrdenTrabajoSpool en base a un Spool ID
        /// </summary>
        /// <param name="spoolID"></param>
        /// <returns></returns>
        public OrdenTrabajoSpool ObtenerOrdenTrabajoPorSpoolID(int spoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Include("OrdenTrabajo").Include("Spool").Where(x => x.SpoolID == spoolID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene el OrdenTrabajoSpoolID en base a un Numero de Control
        /// </summary>
        /// <param name="noControl">Numero de Control de Orden Trabajo Spool</param>
        /// <returns></returns>
        public int? ObtenerOrdenTrabajoSpoolIDPorNumeroDeControl(String noControl)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Where(x => x.NumeroControl == noControl).Select(y => y.OrdenTrabajoSpoolID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene el OrdenTrabajoSpoolID en base a un Numero de Control
        /// </summary>
        /// <param name="noControl">Numero de Control de Orden Trabajo Spool</param>
        /// <param name="proyectoID">Id del proyecto</param>
        /// <returns></returns>
        public int? ObtenerOrdenTrabajoSpoolIDPorNumeroDeControlYProyecto(String noControl, int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Where(x => x.NumeroControl == noControl && x.OrdenTrabajo.ProyectoID == proyectoID).Select(y => y.OrdenTrabajoSpoolID).SingleOrDefault();
            }
        }

        /// </summary>
        /// <param name="noControl">Numero de Control de Orden Trabajo Spool</param> 
        /// <param name="proyectoID">Id del proyecto</param> 
        /// <returns></returns> 
        public List<int> ObtenerOrdenTrabajoSpoolIDsPorNumeroDeControlYProyecto(List<string> noControl, int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Where(x => noControl.Contains(x.NumeroControl) && x.OrdenTrabajo.ProyectoID == proyectoID).Select(y => y.OrdenTrabajoSpoolID).ToList();
            }
        }

        /// </summary>
        /// <param name="noControl">Numero de Control de Orden Trabajo Spool</param> 
        /// <param name="proyectoID">Id del proyecto</param> 
        /// <returns></returns> 
        public List<int> ObtenerOrdenTrabajoSpoolIDsPorNumeroDeControlYProyectoSQ(List<string> noControl, int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                var datos = (from a in ctx.OrdenTrabajoSpool
                             join b in ctx.OrdenTrabajo on a.OrdenTrabajoID equals b.OrdenTrabajoID
                             select new { a.OrdenTrabajoSpoolID, b.ProyectoID, a.NumeroControl }).Where(x => noControl.Contains(x.NumeroControl)).Where(a => a.ProyectoID == proyectoID).ToList();
                return datos.Select(a => a.OrdenTrabajoSpoolID).ToList();
            }
        }

        /// <summary>
        /// Obtiene el OrdenTabajoSpool en base a ordenTrabajoSpoolID
        /// </summary>
        /// <param name="noControl">Orden Trabajo Spool ID</param>
        /// <returns></returns>
        public OrdenTrabajoSpool Obtener(int ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Where(x => x.OrdenTrabajoSpoolID == ordenTrabajoSpoolID).SingleOrDefault(); ;
            }
        }

        public OrdenTrabajoSpoolSQ ObtenerOrdenTrabajoSpoolConSQ(int ordenTrabajoSpoolID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = "SELECT " +
                                    " A.OrdenTrabajoSpoolID, " +
                                    " A.OrdenTrabajoID, " +
                                    " A.SpoolID, " +
                                    " A.Partida, " +
                                    " A.NumeroControl, " +
                                    " A.UsuarioModifica, " +
                                    " A.FechaModificacion, " +
                                    " A.VersionRegistro, " +
                                    " A.EsAsignado, " +
                                    " B.sq SqCliente, " +
                                    " B.sqinterno, " +
                                    " C.TieneHoldIngenieria, " +
                                    " CASE WHEN W.UsuarioOkPnd IS NULL THEN 0 ELSE 1 END OkPnd, " +
                                    " ISNULL(I.Incidencias, 0) Incidencias, " +
                                    " CASE WHEN B.Campo7 = 'GRANEL' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END Granel " +
                                " FROM " +
                                    " OrdenTrabajoSpool A WITH(NOLOCK) " +
                                    " LEFT JOIN Spool B WITH(NOLOCK) ON A.SpoolID = B.SpoolID " +
                                    " LEFT JOIN SpoolHold C WITH(NOLOCK) ON B.SpoolID = C.SpoolID " +
                                    " LEFT JOIN WorkstatusSpool W WITH(NOLOCK) ON A.OrdenTrabajoSpoolID = W.OrdenTrabajoSpoolID " +
                                    " LEFT JOIN " +
                                    " ( " +
                                        " SELECT SpoolID, COUNT(*) Incidencias FROM Shop_Incidencia WITH(NOLOCK) WHERE Activo = 1 GROUP BY SpoolID " +
                                    " ) I ON A.SpoolID = I.SpoolID" +
                                " WHERE " +
                                    " A.OrdenTrabajoSpoolID = " + ordenTrabajoSpoolID;

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    List<OrdenTrabajoSpoolSQ> lista = new List<OrdenTrabajoSpoolSQ>();
                    OrdenTrabajoSpoolSQ objeto;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objeto = new OrdenTrabajoSpoolSQ
                        {
                            NumeroControl = ds.Tables[0].Rows[i]["NumeroControl"].ToString(),
                            OrdenTrabajoSpoolID = int.Parse(ds.Tables[0].Rows[i]["OrdenTrabajoSpoolID"].ToString()),
                            SpoolID = int.Parse(ds.Tables[0].Rows[i]["SpoolID"].ToString()),
                            SqCliente = ds.Tables[0].Rows[i]["SqCliente"].ToString(),
                            sqinterno = ds.Tables[0].Rows[i]["sqinterno"].ToString(),
                            TieneHoldIngenieria = ds.Tables[0].Rows[i]["TieneHoldIngenieria"].ToString() == "" ? false : bool.Parse(ds.Tables[0].Rows[i]["TieneHoldIngenieria"].ToString()),
                            OkPnd = ds.Tables[0].Rows[i]["OkPnd"].ToString() == "0" ? false : true,
                            Incidencias = int.Parse(ds.Tables[0].Rows[i]["Incidencias"].ToString()),
                            Granel = bool.Parse(ds.Tables[0].Rows[i]["Granel"].ToString())
                        };
                        lista.Add(objeto);
                    }
                    return lista.Count > 0 ? lista[0] : null;
                }
            }
        }

        /// <summary>
        /// Lista Numeros de Control por ODT
        /// Y que contengan materiales afines al numero unico recibido.
        /// </summary>
        /// <param name="numeroUnicoID">ID del numero unico a comparar</param>
        /// <param name="segmento">Segmento seleccionado</param>
        /// <param name="ordenTrabajoID">ID de la orden de trabajo</param>
        /// <param name="numeroControl">Texto a comparar</param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public List<Simple> ObtenerAfinesANumeroUnico(int numeroUnicoID, string segmento, int ordenTrabajoID, string numeroControl, int skip, int take)
        {
            List<Simple> result = new List<Simple>(take * 2);

            using (SamContext ctx = new SamContext())
            {
                ctx.NumeroUnico.MergeOption = MergeOption.NoTracking;
                ctx.ItemCodeEquivalente.MergeOption = MergeOption.NoTracking;
                ctx.OrdenTrabajoSpool.MergeOption = MergeOption.NoTracking;
                ctx.OrdenTrabajoMaterial.MergeOption = MergeOption.NoTracking;
                ctx.MaterialSpool.MergeOption = MergeOption.NoTracking;

                bool esAsignacion = ctx.OrdenTrabajo
                                       .Where(x => x.OrdenTrabajoID == ordenTrabajoID)
                                       .Select(x => x.EsAsignado)
                                       .Single();

                if (!esAsignacion)
                {
                    return ObtenerAfinesSinAsignacion(numeroUnicoID, ordenTrabajoID, numeroControl, skip, take, ctx);
                }
                else
                {
                    return ObtenerAfinesConAsignacion(numeroUnicoID, segmento, ordenTrabajoID, numeroControl, skip, take, ctx);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <param name="segmento"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="numeroControl"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public List<Simple> ObtenerAfinesConAsignacion(int numeroUnicoID, string segmento, int ordenTrabajoID, string numeroControl, int skip, int take, SamContext ctx)
        {
            IEnumerable<Simple> data =
                (from nu in ctx.NumeroUnico
                 join odtm in ctx.OrdenTrabajoMaterial on nu.NumeroUnicoID equals odtm.NumeroUnicoAsignadoID
                 join odts in ctx.OrdenTrabajoSpool on odtm.OrdenTrabajoSpoolID equals odts.OrdenTrabajoSpoolID
                 where odts.OrdenTrabajoID == ordenTrabajoID
                       && nu.NumeroUnicoID == numeroUnicoID
                       && (odtm.TieneCorte == null || !odtm.TieneCorte.Value)
                       && odtm.NumeroUnicoAsignadoID == numeroUnicoID && odtm.SegmentoAsignado == segmento
                 select new Simple
                 {
                     ID = odts.OrdenTrabajoSpoolID,
                     Valor = odts.NumeroControl
                 }).Distinct().ToList();

            return data.Where(x => x.Valor.StartsWith(numeroControl, StringComparison.InvariantCultureIgnoreCase))
                       .OrderBy(x => x.Valor)
                       .Skip(skip)
                       .Take(take)
                       .ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroUnicoID"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="numeroControl"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public List<Simple> ObtenerAfinesSinAsignacion(int numeroUnicoID, int ordenTrabajoID, string numeroControl, int skip, int take, SamContext ctx)
        {
            //Obtengo el item code y diametros del numero unico 
            IQueryable<ItemCodeIntegrado> itemCodes = from nu in ctx.NumeroUnico
                                                      where nu.NumeroUnicoID == numeroUnicoID
                                                      select new ItemCodeIntegrado
                                                      {
                                                          ItemCodeID = nu.ItemCodeID.Value,
                                                          Diametro1 = nu.Diametro1,
                                                          Diametro2 = nu.Diametro2
                                                      };

            //Obtengo los itemcodes a los que el numero unico es equivalente
            IQueryable<ItemCodeIntegrado> icEquivalentes = from iceq in ctx.ItemCodeEquivalente
                                                           where itemCodes.Contains(new ItemCodeIntegrado
                                                           {
                                                               ItemCodeID = iceq.ItemEquivalenteID,
                                                               Diametro1 = iceq.DiametroEquivalente1,
                                                               Diametro2 = iceq.DiametroEquivalente2
                                                           })
                                                           select new ItemCodeIntegrado
                                                           {
                                                               ItemCodeID = iceq.ItemCodeID,
                                                               Diametro1 = iceq.Diametro1,
                                                               Diametro2 = iceq.Diametro2
                                                           };



            //Obtengo los numeros de control pertenecientes a la orden de trabajo junto con el detalle de sus materiales que aún no cuenten con corte.
            IEnumerable<Simple> data = (from otsList in ctx.OrdenTrabajoSpool
                                        join otmList in ctx.OrdenTrabajoMaterial on otsList.OrdenTrabajoSpoolID equals otmList.OrdenTrabajoSpoolID
                                        join msList in ctx.MaterialSpool on otmList.MaterialSpoolID equals msList.MaterialSpoolID
                                        where otsList.OrdenTrabajoID == ordenTrabajoID
                                        && ((!otmList.TieneCorte.HasValue) || (!otmList.TieneCorte.Value))
                                        && (itemCodes.Contains(new ItemCodeIntegrado
                                        {
                                            ItemCodeID = msList.ItemCodeID,
                                            Diametro1 = msList.Diametro1,
                                            Diametro2 = msList.Diametro2
                                        })
                                          || icEquivalentes.Contains(new ItemCodeIntegrado
                                          {
                                              ItemCodeID = msList.ItemCodeID,
                                              Diametro1 = msList.Diametro1,
                                              Diametro2 = msList.Diametro2
                                          })
                                          )
                                        select new Simple { ID = otsList.OrdenTrabajoSpoolID, Valor = otsList.NumeroControl }).Distinct();


            return data.Where(x => x.Valor.StartsWith(numeroControl, StringComparison.InvariantCultureIgnoreCase))
                       .OrderBy(x => x.Valor)
                       .Skip(skip)
                       .Take(take)
                       .ToList();
        }

        /// <summary>
        /// Obtiene la lista de numeros de control que tienen Workstatus
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="numeroControlFiltro"></param>
        /// <returns></returns>
        public IEnumerable<Simple> ObtenerNumerosDeControlLiberacionCalidad(int? proyectoID, int skip, int take, string numeroControlFiltro)
        {
            using (SamContext ctx = new SamContext())
            {
                List<Simple> lista;
                IQueryable<OrdenTrabajoSpool> ots = (from otSpool in ctx.OrdenTrabajoSpool
                                                     join ordenTrabajo in ctx.OrdenTrabajo on otSpool.OrdenTrabajoID equals ordenTrabajo.OrdenTrabajoID
                                                     join wstatus in ctx.WorkstatusSpool on otSpool.OrdenTrabajoSpoolID equals wstatus.OrdenTrabajoSpoolID
                                                     where ordenTrabajo.ProyectoID == proyectoID
                                                     select otSpool);

                lista = ots.Select(x => new Simple { ID = x.OrdenTrabajoSpoolID, Valor = x.NumeroControl })
                        .ToList();

                return lista.Where(x => x.Valor.StartsWith(numeroControlFiltro, StringComparison.InvariantCultureIgnoreCase))
                       .OrderBy(x => x.Valor)
                       .Skip(skip)
                       .Take(take);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="proyectoID"></param>
        /// <param name="ordenTrabajoID"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="numeroControlFiltro"></param>
        /// <param name="esAdministradorSistema"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IEnumerable<Simple> ObtenerNumerosDeControlPorPermiso(int? proyectoID, int? ordenTrabajoID, int skip, int take,
            string numeroControlFiltro, bool esAdministradorSistema, Guid userID)
        {
            List<Simple> lst;

            using (SamContext ctx = new SamContext())
            {
                ctx.OrdenTrabajoSpool.MergeOption = MergeOption.NoTracking;
                IQueryable<OrdenTrabajoSpool> iOts = ctx.OrdenTrabajoSpool.AsQueryable();


                if (ordenTrabajoID.HasValue && ordenTrabajoID.Value > 0)
                {
                    //asumimos que tiene permisos
                    iOts = iOts.Where(x => x.OrdenTrabajoID == ordenTrabajoID);
                }
                else if (proyectoID.HasValue && proyectoID.Value > 0)
                {
                    //aquí tmb asumimos que tiene permisos
                    iOts = iOts.Where(x => ctx.OrdenTrabajo
                                              .Where(y => y.ProyectoID == proyectoID)
                                              .Select(z => z.OrdenTrabajoID)
                                              .Contains(x.OrdenTrabajoID));
                }
                else if (!esAdministradorSistema)
                {
                    //aqui traemos unicamente por permisos
                    iOts = iOts.Where(x => ctx.OrdenTrabajo
                                              .Where(y => ctx.UsuarioProyecto
                                                             .Where(up => up.UserId == userID)
                                                             .Select(up => up.ProyectoID)
                                                             .Contains(y.ProyectoID))
                                              .Select(z => z.OrdenTrabajoID)
                                              .Contains(x.OrdenTrabajoID));
                }


                lst =
                    iOts.Select(x => new Simple { ID = x.OrdenTrabajoSpoolID, Valor = x.NumeroControl })
                        .ToList();
            }


            return lst.Where(x => x.Valor.StartsWith(numeroControlFiltro, StringComparison.InvariantCultureIgnoreCase))
                       .OrderBy(x => x.Valor)
                       .Skip(skip)
                       .Take(take);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordenTrabajoSpoolID"></param>
        /// <returns></returns>
        public int ObtenerProyectoID(int ordenTrabajoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ProyectoOrdenTrabajoSpool(ctx, ordenTrabajoSpoolID);
            }
        }

        /// <summary>
        /// Versión compilada del query para permisos de ODT spool
        /// </summary>
        private static readonly Func<SamContext, int, int> ProyectoOrdenTrabajoSpool =
        CompiledQuery.Compile<SamContext, int, int>
        (
            (ctx, id) => ctx.OrdenTrabajoSpool
                            .Where(x => x.OrdenTrabajoSpoolID == id)
                            .Select(x => x.OrdenTrabajo.ProyectoID)
                            .Single()
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombreSpool"></param>
        /// <param name="proyectoID"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="totalRows"></param>
        public List<NumeroControlBusqueda> BuscarPorNombreDeSpool(string nombreSpool, int? proyectoID, int skip, int take, out int totalRows)
        {
            using (SamContext ctx = new SamContext())
            {
                var query =
                    (from odts in ctx.OrdenTrabajoSpool
                     join c in ctx.Cuadrante on odts.Spool.CuadranteID equals c.CuadranteID into cuadranteDef
                     from t2 in cuadranteDef.DefaultIfEmpty()
                     where odts.Spool.Nombre.StartsWith(nombreSpool)
                     select new NumeroControlBusqueda
                     {
                         SpoolID = odts.SpoolID,
                         Spool = odts.Spool.Nombre,
                         OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID,
                         NumeroControl = odts.NumeroControl,
                         ProyectoID = odts.OrdenTrabajo.ProyectoID
                     });

                if (proyectoID.HasValue)
                {
                    query = query.Where(q => q.ProyectoID == proyectoID);
                }

                totalRows = query.Count();

                return query.OrderBy(sp => sp.Spool).Skip(skip).Take(take).ToList();
            }
        }



        public List<CuadranteNumeroControlSQ> ObtenerNumeroControlPorSQ(string Sq, int proyectoID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = " SELECT " +
                                    " C.CuadranteID, " +
                                    " C.Nombre, " +
                                    " OS.NumeroControl, " +
                                    " OS.OrdenTrabajoSpoolID, " +
                                    " S.sqinterno,  " +
                                    " S.SpoolID,  " +
                                    " S.sq SqCliente, " +
                                    " CASE WHEN H.TieneHoldIngenieria IS NULL THEN CAST(0 AS BIT) ELSE CAST(H.TieneHoldIngenieria AS BIT) END TieneHoldIngenieria, " +
                                    " CASE WHEN W.UsuarioOkPnd IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END OkPnd, " +
                                    " ISNULL(I.Incidencias, 0) Incidencias, " +
                                    " CASE WHEN S.Campo7 = 'GRANEL' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END Granel " +
                                " FROM " +
                                    " OrdenTrabajoSpool OS WITH(NOLOCK) " +
                                    " INNER JOIN Spool S WITH(NOLOCK) ON OS.SpoolID = S.SpoolID " +
                                    " INNER JOIN Cuadrante C WITH(NOLOCK) ON S.CuadranteID = C.CuadranteID " +
                                    " LEFT JOIN SpoolHold H WITH(NOLOCK) ON S.SpoolID = H.SpoolID " +
                                    " LEFT JOIN WorkstatusSpool W WITH(NOLOCK) ON OS.OrdenTrabajoSpoolID = W.OrdenTrabajoSpoolID " +
                                    " LEFT JOIN " +
                                    " ( " +
                                        " SELECT SpoolID, COUNT(*) Incidencias FROM Shop_Incidencia WITH(NOLOCK) WHERE Activo = 1 GROUP BY SpoolID " +
                                    " ) I ON S.SpoolID = I.SpoolID" +
                                " WHERE " +
                                    " S.sqinterno = '" + Sq + "' AND S.proyectoID = " + proyectoID +
                                " ORDER BY OS.NumeroControl ASC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    List<CuadranteNumeroControlSQ> lista = new List<CuadranteNumeroControlSQ>();
                    CuadranteNumeroControlSQ cuadranteNumeroControlSQ;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cuadranteNumeroControlSQ = new CuadranteNumeroControlSQ
                        {
                            //Accion = 1,
                            Accion = 2,
                            CuadranteID = int.Parse(ds.Tables[0].Rows[i]["CuadranteID"].ToString()),
                            Cuadrante = ds.Tables[0].Rows[i]["Nombre"].ToString(),
                            NumeroControl = ds.Tables[0].Rows[i]["NumeroControl"].ToString(),
                            SpoolID = int.Parse(ds.Tables[0].Rows[i]["SpoolID"].ToString()),
                            OrdenTrabajoSpoolID = int.Parse(ds.Tables[0].Rows[i]["OrdenTrabajoSpoolID"].ToString()),
                            SqCliente = ds.Tables[0].Rows[i]["SqCliente"].ToString(),
                            SQ = ds.Tables[0].Rows[i]["sqinterno"].ToString(),
                            TieneHoldIngenieria = bool.Parse(ds.Tables[0].Rows[i]["TieneHoldIngenieria"].ToString()),
                            OkPnd = bool.Parse(ds.Tables[0].Rows[i]["OkPnd"].ToString()),
                            Incidencias = int.Parse(ds.Tables[0].Rows[i]["Incidencias"].ToString()),
                            Granel = bool.Parse(ds.Tables[0].Rows[i]["Granel"].ToString())
                        };
                        lista.Add(cuadranteNumeroControlSQ);
                    }
                    return lista;
                }
            }
        }

        public List<AutorizarSI> ObtenerSpoolsPorSQyProyecto(string Sq, int proyectoID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = " SELECT " +
                                    " C.CuadranteID, " +
                                    " C.Nombre, " +
                                    " S.ProyectoID, " +
                                    " OS.NumeroControl, " +
                                    " OS.OrdenTrabajoSpoolID, " +
                                    " S.sqinterno,  " +
                                    " S.SpoolID,  " +
                                    " S.sq SqCliente, " +
                                    " CASE WHEN H.TieneHoldIngenieria IS NULL THEN CAST(0 AS BIT) ELSE CAST(H.TieneHoldIngenieria AS BIT) END TieneHoldIngenieria, " +
                                    " CASE WHEN W.UsuarioOkPnd IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END OkPnd, " +
                                    " CASE WHEN A.Autorizado IS NULL OR A.Autorizado = 0 THEN CAST(0 AS BIT) ELSE CAST(A.Autorizado AS BIT) END Autorizado, " +
                                    " ISNULL(I.Incidencias, 0) Incidencias, " +
                                    " CASE WHEN S.Campo7 = 'GRANEL' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END Granel " +
                                " FROM " +
                                    "   OrdenTrabajoSpool OS WITH(NOLOCK) " +
                                    " INNER JOIN Spool S WITH(NOLOCK) ON OS.SpoolID = S.SpoolID " +
                                    " INNER JOIN Cuadrante C WITH(NOLOCK) ON S.CuadranteID = C.CuadranteID " +
                                    " LEFT JOIN SpoolHold H WITH(NOLOCK) ON S.SpoolID = H.SpoolID " +
                                    " LEFT JOIN WorkstatusSpool W WITH(NOLOCK) ON OS.OrdenTrabajoSpoolID = W.OrdenTrabajoSpoolID " +
                                    " LEFT JOIN Shop_AutorizacionSI A WITH(NOLOCK)ON S.SpoolID = A.SpoolID AND A.Activo = 1 " +
                                    " LEFT JOIN " +
                                    " ( " +
                                        " SELECT SpoolID, COUNT(*) Incidencias FROM Shop_Incidencia WITH(NOLOCK) WHERE Activo = 1 GROUP BY SpoolID " +
                                    " ) I ON S.SpoolID = I.SpoolID" +
                                " WHERE " +
                                    " S.sqinterno = '" + Sq + "' AND S.proyectoID = " + proyectoID +
                                " ORDER BY A.Autorizado ASC";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    List<AutorizarSI> lista = new List<AutorizarSI>();
                    AutorizarSI Autorizar;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Autorizar = new AutorizarSI
                        {
                            Accion = 2,
                            CuadranteID = int.Parse(ds.Tables[0].Rows[i]["CuadranteID"].ToString()),
                            Cuadrante = ds.Tables[0].Rows[i]["Nombre"].ToString(),
                            NumeroControl = ds.Tables[0].Rows[i]["NumeroControl"].ToString(),
                            SpoolID = int.Parse(ds.Tables[0].Rows[i]["SpoolID"].ToString()),
                            ProyectoID = int.Parse(ds.Tables[0].Rows[i]["ProyectoID"].ToString()),
                            OrdenTrabajoSpoolID = int.Parse(ds.Tables[0].Rows[i]["OrdenTrabajoSpoolID"].ToString()),
                            SqCliente = ds.Tables[0].Rows[i]["SqCliente"].ToString(),
                            SI = ds.Tables[0].Rows[i]["sqinterno"].ToString(),
                            Hold = bool.Parse(ds.Tables[0].Rows[i]["TieneHoldIngenieria"].ToString()),
                            OkPnd = bool.Parse(ds.Tables[0].Rows[i]["OkPnd"].ToString()),
                            Autorizado = bool.Parse(ds.Tables[0].Rows[i]["Autorizado"].ToString()),
                            NoAutorizado = !bool.Parse(ds.Tables[0].Rows[i]["Autorizado"].ToString()),
                            Incidencias = int.Parse(ds.Tables[0].Rows[i]["Incidencias"].ToString()),
                            Granel = bool.Parse(ds.Tables[0].Rows[i]["Granel"].ToString())
                        };
                        lista.Add(Autorizar);
                    }
                    return lista;
                }
            }
        }

        public List<ListaIncidencia> ObtenerSpoolsNoResueltos(int proyectoID, int CuadranteID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string Operador = " OR ";
                if(proyectoID != 0 && CuadranteID != 0)
                {
                    Operador = " AND ";
                }
                string query = " SELECT " +
                                " S.SpoolID,  " +
                                " S.ProyectoID,  " +
                                " C.CuadranteID, " +
                                " C.Nombre Cuadrante, " +
                                " OT.NumeroControl, " +
                                " CASE WHEN H.TieneHoldIngenieria IS NULL THEN CAST(0 AS BIT) ELSE CAST(H.TieneHoldIngenieria AS BIT) END Hold, " +
                                " ISNULL(II.Incidencias, 0) Incidencias, " +
                                " CASE WHEN S.Campo7 = 'GRANEL' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END Granel " +
                            " FROM " +
                                " Shop_Incidencia I WITH(NOLOCK) " +
                                " INNER JOIN Spool S WITH(NOLOCK) ON I.SpoolID = S.SpoolID " +
                                " INNER JOIN OrdenTrabajoSpool OT WITH(NOLOCK) ON S.SpoolID = OT.SpoolID " +
                                " INNER JOIN Cuadrante C WITH(NOLOCK) ON S.CuadranteID = C.CuadranteID " +
                                " LEFT JOIN SpoolHold H WITH(NOLOCK) ON S.SpoolID = H.SpoolID " +
                                " LEFT JOIN " +
                                " ( " +
                                    " SELECT SpoolID, COUNT(*) Incidencias FROM Shop_Incidencia WITH(NOLOCK) WHERE Activo = 1 GROUP BY SpoolID " +
                                " ) II ON I.SpoolID = II.SpoolID " +
                            " WHERE " +
                                " (S.proyectoID = " + proyectoID + " " + Operador + " C.CuadranteID = " + CuadranteID + ") AND I.Activo = 1 AND(I.Resolucion IS NULL OR I.SI IS NULL) AND I.Inspector IS NOT NULL " +
                            " GROUP BY " +
                                " S.SpoolID, S.ProyectoID, C.CuadranteID, C.Nombre, OT.NumeroControl, H.TieneHoldIngenieria, II.Incidencias, S.Campo7 ";
                                 
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    List<ListaIncidencia> lista = new List<ListaIncidencia>();
                    ListaIncidencia Incidencias;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Incidencias = new ListaIncidencia
                        {
                            SpoolID = int.Parse(ds.Tables[0].Rows[i]["SpoolID"].ToString()),
                            ProyectoID = int.Parse(ds.Tables[0].Rows[i]["ProyectoID"].ToString()),
                            CuadranteID = int.Parse(ds.Tables[0].Rows[i]["CuadranteID"].ToString()),
                            Cuadrante = ds.Tables[0].Rows[i]["Cuadrante"].ToString(),
                            NumeroControl = ds.Tables[0].Rows[i]["NumeroControl"].ToString(),                            
                            Hold = bool.Parse(ds.Tables[0].Rows[i]["Hold"].ToString()),                            
                            Incidencias = int.Parse(ds.Tables[0].Rows[i]["Incidencias"].ToString()),
                            Granel = bool.Parse(ds.Tables[0].Rows[i]["Granel"].ToString())
                        };
                        lista.Add(Incidencias);
                    }
                    return lista;
                }
            }
        }

        public List<ListaIncidencia> ObtenerSpoolsPorNumeroControl(int proyectoID, string NumeroControl)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {                               
                string query = " SELECT " +
                                " S.SpoolID,  " +
                                " S.ProyectoID,  " +
                                " C.CuadranteID, " +
                                " C.Nombre Cuadrante, " +
                                " OT.NumeroControl, " +
                                " CASE WHEN H.TieneHoldIngenieria IS NULL THEN CAST(0 AS BIT) ELSE CAST(H.TieneHoldIngenieria AS BIT) END Hold, " +
                                " ISNULL(II.Incidencias, 0) Incidencias, " +
                                " CASE WHEN S.Campo7 = 'GRANEL' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END Granel " +
                            " FROM " +
                                " Shop_Incidencia I WITH(NOLOCK) " +
                                " INNER JOIN Spool S WITH(NOLOCK) ON I.SpoolID = S.SpoolID " +
                                " INNER JOIN OrdenTrabajoSpool OT WITH(NOLOCK) ON S.SpoolID = OT.SpoolID " +
                                " INNER JOIN Cuadrante C WITH(NOLOCK) ON S.CuadranteID = C.CuadranteID " +
                                " LEFT JOIN SpoolHold H WITH(NOLOCK) ON S.SpoolID = H.SpoolID " +
                                " LEFT JOIN " +
                                " ( " +
                                    " SELECT SpoolID, COUNT(*) Incidencias FROM Shop_Incidencia WITH(NOLOCK) WHERE Activo = 1 GROUP BY SpoolID " +
                                " ) II ON I.SpoolID = II.SpoolID " +
                            " WHERE " +
                                " (S.proyectoID = " + proyectoID + " AND OT.NumeroControl = '" + NumeroControl + "') AND I.Activo = 1 AND(I.Resolucion IS NULL OR I.SI IS NULL) AND I.Inspector IS NOT NULL " +
                            " GROUP BY " +
                                " S.SpoolID, S.ProyectoID, C.CuadranteID, C.Nombre, OT.NumeroControl, H.TieneHoldIngenieria, II.Incidencias, S.Campo7 ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    List<ListaIncidencia> lista = new List<ListaIncidencia>();
                    ListaIncidencia Incidencias;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Incidencias = new ListaIncidencia
                        {
                            SpoolID = int.Parse(ds.Tables[0].Rows[i]["SpoolID"].ToString()),
                            ProyectoID = int.Parse(ds.Tables[0].Rows[i]["ProyectoID"].ToString()),
                            CuadranteID = int.Parse(ds.Tables[0].Rows[i]["CuadranteID"].ToString()),
                            Cuadrante = ds.Tables[0].Rows[i]["Cuadrante"].ToString(),
                            NumeroControl = ds.Tables[0].Rows[i]["NumeroControl"].ToString(),
                            Hold = bool.Parse(ds.Tables[0].Rows[i]["Hold"].ToString()),
                            Incidencias = int.Parse(ds.Tables[0].Rows[i]["Incidencias"].ToString()),
                            Granel = bool.Parse(ds.Tables[0].Rows[i]["Granel"].ToString())
                        };
                        lista.Add(Incidencias);
                    }
                    return lista;
                }
            }
        }

        public List<CuadranteNumeroControlSQ> BuscarPorCuadrante(int cuadranteid, int? proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                var query =
                    (from odts in ctx.OrdenTrabajoSpool
                     join s in ctx.Spool on odts.SpoolID equals s.SpoolID
                     join c in ctx.Cuadrante on s.CuadranteID equals c.CuadranteID

                     where s.ProyectoID == proyectoID && c.CuadranteID == cuadranteid
                     select new CuadranteNumeroControlSQ
                     {
                         Accion = 1,
                         CuadranteID = c.CuadranteID,
                         Cuadrante = c.Nombre,
                         NumeroControl = odts.NumeroControl,
                         OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID
                     });




                return query.OrderBy(sp => sp.CuadranteID).ToList();
            }
        }

        public string ObtenerConsecutivoProyecto(int ProyectoID)
        {
            string consecutivo = "", query = "";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                query = "SELECT ISNULL(ConsecutivoSQ, 0) Consecutivo FROM ProyectoConsecutivo WHERE ProyectoID = " + ProyectoID;
                SqlCommand cmd = new SqlCommand(query, con);
                if (con.State == ConnectionState.Closed) con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    consecutivo = dr["Consecutivo"].ToString();
                }
                if (con.State == ConnectionState.Open) con.Close();
            }
            return consecutivo;
        }

        public List<CuadranteNumeroControlSQ> BuscarPorCuadranteSQ(int cuadranteid, int? proyectoID, int Opcion)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = "";
                if (Opcion == 1)
                {
                    query += " SELECT 1 Accion,C.Nombre Cuadrante,C.CuadranteID,c.Nombre,A.NumeroControl,A.OrdenTrabajoSpoolID, B.SpoolID, B.sq SqCliente, B.sqinterno,D.TieneHoldIngenieria, CASE WHEN W.UsuarioOkPnd IS NULL THEN 0 ELSE 1 END OkPnd, ISNULL(I.Incidencias, 0) Incidencias, CASE WHEN B.Campo7 = 'GRANEL' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END Granel ";
                }
                else
                {
                    query += " SELECT 2 Accion,C.Nombre Cuadrante,C.CuadranteID,c.Nombre,A.NumeroControl,A.OrdenTrabajoSpoolID, B.SpoolID, B.sq SqCliente, B.sqinterno,D.TieneHoldIngenieria, CASE WHEN W.UsuarioOkPnd IS NULL THEN 0 ELSE 1 END OkPnd, ISNULL(I.Incidencias, 0) Incidencias, CASE WHEN B.Campo7 = 'GRANEL' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END Granel ";
                }
                query += " FROM " +
                            " OrdenTrabajoSpool A WITH(NOLOCK) " +
                            " INNER JOIN Spool B ON A.SpoolID = B.SpoolID " +
                            " INNER JOIN Cuadrante C WITH(NOLOCK) ON B.CuadranteID = C.CuadranteID " +
                            " LEFT JOIN SpoolHold D WITH(NOLOCK) ON B.SpoolID = D.SpoolID " +
                            " LEFT JOIN WorkstatusSpool W WITH(NOLOCK) ON A.OrdenTrabajoSpoolID = W.OrdenTrabajoSpoolID " +
                            " LEFT JOIN " +
                            " ( " +
                                " SELECT SpoolID, COUNT(*) Incidencias FROM Shop_Incidencia WITH(NOLOCK) WHERE Activo = 1 GROUP BY SpoolID " +
                            " ) I ON B.SpoolID = I.SpoolID" +
                        " WHERE " +
                            " B.ProyectoID = " + proyectoID + " AND C.CuadranteID = " + cuadranteid +
                        " ORDER BY CuadranteID ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    List<CuadranteNumeroControlSQ> lista = new List<CuadranteNumeroControlSQ>();
                    CuadranteNumeroControlSQ objeto;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        objeto = new CuadranteNumeroControlSQ
                        {
                            Accion = int.Parse(ds.Tables[0].Rows[i]["Accion"].ToString()),
                            Cuadrante = ds.Tables[0].Rows[i]["Cuadrante"].ToString(),
                            CuadranteID = int.Parse(ds.Tables[0].Rows[i]["CuadranteID"].ToString()),
                            NumeroControl = ds.Tables[0].Rows[i]["NumeroControl"].ToString(),
                            SpoolID = int.Parse(ds.Tables[0].Rows[i]["SpoolID"].ToString()),
                            OrdenTrabajoSpoolID = int.Parse(ds.Tables[0].Rows[i]["OrdenTrabajoSpoolID"].ToString()),
                            SqCliente = ds.Tables[0].Rows[i]["SqCliente"].ToString(),
                            SQ = ds.Tables[0].Rows[i]["sqinterno"].ToString(),
                            TieneHoldIngenieria = ds.Tables[0].Rows[i]["TieneHoldIngenieria"].ToString() == "" ? false : bool.Parse(ds.Tables[0].Rows[i]["TieneHoldIngenieria"].ToString()),
                            OkPnd = ds.Tables[0].Rows[i]["OkPnd"].ToString() == "0" ? false : true,
                            Incidencias = int.Parse(ds.Tables[0].Rows[i]["Incidencias"].ToString()),
                            Granel = bool.Parse(ds.Tables[0].Rows[i]["Granel"].ToString())
                        };
                        lista.Add(objeto);
                    }
                    return lista;
                }
            }
        }

        public NumeroControlBusqueda BuscarPorIdSpool(int idSpool, int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                bool esgranel = EsGranel(idSpool);
                var query =
                    (from odts in ctx.OrdenTrabajoSpool                     
                     join c in ctx.Cuadrante on odts.Spool.CuadranteID equals c.CuadranteID into cuadranteDef
                     from t2 in cuadranteDef.DefaultIfEmpty()
                     join f1 in ctx.FamiliaAcero on odts.Spool.FamiliaAcero1ID equals f1.FamiliaAceroID into familiaDef1
                     from t3 in familiaDef1.DefaultIfEmpty()
                     join f1 in ctx.FamiliaAcero on odts.Spool.FamiliaAcero2ID equals f1.FamiliaAceroID into familiaDef2
                     from t4 in familiaDef2.DefaultIfEmpty()
                     where odts.Spool.SpoolID.Equals(idSpool) && odts.Spool.ProyectoID.Equals(proyectoID)
                     select new NumeroControlBusqueda
                     {
                         SpoolID = odts.SpoolID,
                         Spool = odts.Spool.Nombre,
                         OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID,
                         NumeroControl = odts.NumeroControl,
                         ProyectoID = odts.OrdenTrabajo.ProyectoID,
                         FamiliaAcero = string.IsNullOrEmpty(t3.Nombre) ? (string.IsNullOrEmpty(t4.Nombre) ? string.Empty : t4.Nombre) : t3.Nombre + (string.IsNullOrEmpty(t4.Nombre) ? string.Empty : "/" + t4.Nombre),
                         DiametroMaximo = odts.Spool.DiametroMayor,
                         Cuadrante = string.IsNullOrEmpty(t2.Nombre) ? string.Empty : t2.Nombre,
                         CuadranteId = (t2.CuadranteID == null) ? 0 : t2.CuadranteID,
                         TipoNC = TipoNumeroControlEnum.AProcesar,
                         Granel = esgranel
                     });

                return query.FirstOrDefault();
            }
        }

        public List<NumeroControlBusqueda> BuscarPorNombreDeSpool(string nombreSpool, int proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                var query =
                    (from odts in ctx.OrdenTrabajoSpool
                     join c in ctx.Cuadrante on odts.Spool.CuadranteID equals c.CuadranteID into cuadranteDef
                     from t2 in cuadranteDef.DefaultIfEmpty()
                     join f1 in ctx.FamiliaAcero on odts.Spool.FamiliaAcero1ID equals f1.FamiliaAceroID into familiaDef1
                     from t3 in familiaDef1.DefaultIfEmpty()
                     join f1 in ctx.FamiliaAcero on odts.Spool.FamiliaAcero2ID equals f1.FamiliaAceroID into familiaDef2
                     from t4 in familiaDef2.DefaultIfEmpty()
                     where odts.Spool.Nombre.StartsWith(nombreSpool) && odts.Spool.ProyectoID.Equals(proyectoID)
                     select new NumeroControlBusqueda
                     {
                         SpoolID = odts.SpoolID,
                         Spool = odts.Spool.Nombre,
                         OrdenTrabajoSpoolID = odts.OrdenTrabajoSpoolID,
                         NumeroControl = odts.NumeroControl,
                         ProyectoID = odts.OrdenTrabajo.ProyectoID,
                         FamiliaAcero = string.IsNullOrEmpty(t3.Nombre) ? (string.IsNullOrEmpty(t4.Nombre) ? string.Empty : t4.Nombre) : t3.Nombre + (string.IsNullOrEmpty(t4.Nombre) ? string.Empty : "/" + t4.Nombre),
                         DiametroMaximo = odts.Spool.DiametroMayor,
                         Cuadrante = string.IsNullOrEmpty(t2.Nombre) ? string.Empty : t2.Nombre,
                         CuadranteId = (t2.CuadranteID == null) ? 0 : t2.CuadranteID,
                         TipoNC = TipoNumeroControlEnum.AProcesar
                     });

                return query.OrderBy(sp => sp.Spool).ToList();
            }
        }

        public CuadranteSQ BuscarCuadrante(int cuadranteid, int? proyectoID)
        {
            using (SamContext ctx = new SamContext())
            {
                var query =
                    (from
                     c in ctx.Cuadrante
                     where c.CuadranteID == cuadranteid
                     select new CuadranteSQ
                     {
                         CuadranteID = c.CuadranteID,
                         Cuadrante = c.Nombre,
                     });
                return query.OrderBy(sp => sp.CuadranteID).SingleOrDefault();
            }
        }

        public List<LayoutGridSQ> ListaNumControlConSpoolID(DataTable Lista)
        {
            List<LayoutGridSQ> ListaRetorna = new List<LayoutGridSQ>();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                DataSet ds = null;
                Lista.Columns.Remove("TieneHoldIngenieria");
                Lista.Columns.Remove("OkPnd");
                Lista.Columns.Remove("Incidencias");
                Lista.Columns.Remove("Granel");
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametro = null;
                if (Lista.Rows.Count > 0)
                    ds = _SQL.Coleccion(Stords.ObtieneSpoolIDTabla, Lista, "@TablaNumeroControl", parametro);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        var result = ds.Tables[0].AsEnumerable().Select(row => new LayoutGridSQ
                        {
                            NumeroControl = row.Field<string>("NumeroControl"),
                            SpoolID = row.Field<int>("SpoolID"),
                            CuadranteID = row.Field<int>("CuadranteID"),
                            Cuadrante = row.Field<string>("Cuadrante"),
                            Accion = row.Field<int>("Accion"),
                            OrdenTrabajoSpoolID = row.Field<int>("OrdenTrabajoSpoolID"),
                            SqCliente = row.Field<string>("SqCliente"),
                            SQ = row.Field<string>("SQ"),
                            TieneHoldIngenieria = row.Field<bool>("TieneHoldIngenieria"),
                            OkPnd = row.Field<int>("OkPnd") == 0 ? false : true,
                            Incidencias = row.Field<int>("Incidencias"),
                            Granel = row.Field<bool>("Granel")
                        }).ToList();
                        ListaRetorna = result;
                    }
                }
            }
            return ListaRetorna;
        }

        public void EliminarSpool(string NumeroControl, int ProyectoID, string SQ)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametro = { { "@NumeroControl", NumeroControl }, { "@ProyectoID", ProyectoID.ToString() }, { "@SQ", SQ == null ? "" : SQ } };
                _SQL.EjecutaInsertUpdate(Stords.EliminarSpool, parametro);
            }
        }

        public string GuardarNumeroControlSQ(DataTable listaLayoutSQ, Guid userID, string inspector, int proyectoid, string sq)
        {
            try
            {
                //using (SamContext ctx = new SamContext())
                //{
                listaLayoutSQ.Columns.Remove("TieneHoldIngenieria");
                listaLayoutSQ.Columns.Remove("OkPnd");
                listaLayoutSQ.Columns.Remove("Incidencias");
                listaLayoutSQ.Columns.Remove("Granel");
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametro = { { "@Usuario", userID.ToString() }, { "@Inspector", inspector }, { "@ProyectoID", proyectoid.ToString() }, { "@SQBuscar", sq }, { "@UltimoSQInternoEncontrado", "" } };
                if (listaLayoutSQ.Rows.Count > 0)
                    //return _SQL.Ejecuta(Stords.GuardarNumeroControlSQ, listaLayoutSQ, "@tablaNumeroControlSQ", parametro).ToString();
                    return _SQL.EjecutaRetornaString(Stords.GuardarNumeroControlSQ, listaLayoutSQ, "@tablaNumeroControlSQ", parametro).ToString();
                else
                    return "0";
                //}
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        /*AutorizarSI*/
        public List<TipoIncidencia> ObtenerTipoIncidencias()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = " SELECT " +
                                    " TipoIncidenciaID," +
                                    " Incidencia " +
                                " FROM " +
                                    " Shop_TipoIncidencia WHERE Activo = 1";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    List<TipoIncidencia> lista = new List<TipoIncidencia>();
                    TipoIncidencia tipoIncidencia;
                    lista.Add(new TipoIncidencia());
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        tipoIncidencia = new TipoIncidencia
                        {
                            TipoIncidenciaID = int.Parse(ds.Tables[0].Rows[i]["TipoIncidenciaID"].ToString()),
                            Incidencia = ds.Tables[0].Rows[i]["Incidencia"].ToString()
                        };
                        lista.Add(tipoIncidencia);
                    }
                    return lista;
                }
            }
        }

        public List<IncidenciaDetalle> ObtenerDetalleIncidencias(int TipoIncidenciaID, int SpoolID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = "";
                //Cuando el tipo de incidencia es spool
                if(TipoIncidenciaID != 1 && TipoIncidenciaID != 2)
                {
                    List<IncidenciaDetalle> lista = new List<IncidenciaDetalle>();
                    IncidenciaDetalle detalle;
                    lista.Add(new IncidenciaDetalle());                   
                    detalle = new IncidenciaDetalle
                    {
                        ID = 0,
                        Etiqueta = "N/A"
                    };
                        lista.Add(detalle);                    
                    return lista;
                }
                else
                {
                    if (TipoIncidenciaID == 1) //Materiales 
                    {
                        query = " SELECT " +
                                    " MaterialSpoolID ID, " +
                                    " Etiqueta " +
                                " FROM " +
                                    " MaterialSpool " +
                                " WHERE SpoolID = " + SpoolID +
                                " ORDER BY Etiqueta ASC";
                    }
                    else
                    {
                        query = " SELECT " +
                                    " JuntaSpoolID ID, " +
                                    " Etiqueta " +
                                " FROM " +
                                    " JuntaSpool " +
                                " WHERE SpoolID = " + SpoolID + " AND FabAreaID = 1" +
                                " ORDER BY Etiqueta ASC";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        List<IncidenciaDetalle> lista = new List<IncidenciaDetalle>();
                        IncidenciaDetalle detalle;
                        lista.Add(new IncidenciaDetalle());
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            detalle = new IncidenciaDetalle
                            {
                                ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString()),
                                Etiqueta = ds.Tables[0].Rows[i]["Etiqueta"].ToString()
                            };
                            lista.Add(detalle);
                        }
                        return lista;
                    }
                }                
            }
        }

        public List<ListaErrores> ObtenerListaErrores(int TipoIncidenciaID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = " SELECT ErrorIncidenciaID ErrorID, Error FROM Shop_ErrorIncidencia WHERE TipoIncidenciaID = " + TipoIncidenciaID + " AND Activo = 1";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    List<ListaErrores> lista = new List<ListaErrores>();
                    ListaErrores detalle;
                    lista.Add(new ListaErrores());
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        detalle = new ListaErrores
                        {
                            ErrorID = int.Parse(ds.Tables[0].Rows[i]["ErrorID"].ToString()),
                            Error = ds.Tables[0].Rows[i]["Error"].ToString()
                        };
                        lista.Add(detalle);
                    }
                    return lista;
                }
            }
        }

        public List<IncidenciaC> ObtenerIncidencias(int SpoolID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = " SELECT " +
                                    " I.IncidenciaID, " +
                                    " I.SpoolID, " +
                                    " OS.NumeroControl, " +
                                    " TI.Incidencia, " +
                                    " CASE WHEN I.TipoIncidenciaID = 1 THEN MS.Etiqueta WHEN I.TipoIncidenciaID = 2 THEN JS.Etiqueta ELSE 'N/A' END MaterialJunta, " +
                                    " EI.ErrorIncidenciaID, " +
                                    " EI.Error, " +                                
                                    " I.SI, " +
                                    " I.Observacion, " +
                                    " I.Cliente, " +
                                    " CONVERT(VARCHAR(30), I.FechaIncidencia, 103) FechaIncidencia " +
                                " FROM " +
                                    " Shop_Incidencia I WITH(NOLOCK) " +
                                    " INNER JOIN Shop_TipoIncidencia TI WITH(NOLOCK) ON I.TipoIncidenciaID = TI.TipoIncidenciaID AND TI.Activo = 1 " +
                                    " LEFT JOIN OrdenTrabajoSpool OS WITH(NOLOCK) ON I.SpoolID = OS.SpoolID " +
                                    " INNER JOIN Spool S WITH(NOLOCK) ON OS.SpoolID = S.SpoolID  " +
                                    " LEFT JOIN MaterialSpool MS WITH(NOLOCK) ON I.MaterialSpoolID = MS.MaterialSpoolID AND I.MaterialSpoolID IS NOT NULL  " +
                                    " LEFT JOIN JuntaSpool JS WITH(NOLOCK) ON I.JuntaSpoolID = JS.JuntaSpoolID AND I.JuntaSpoolID IS NOT NULL " +
                                    " INNER JOIN Shop_ErrorIncidencia EI WITH(NOLOCK) ON I.ErrorIncidenciaID = EI.ErrorIncidenciaID AND EI.Activo = 1 " +
                                    " LEFT JOIN " +
                                    " ( " +
                                        " SELECT IncidenciaID, STUFF((SELECT ',' + SI FROM Shop_Incidencia WHERE Activo = 1 GROUP BY SI ORDER BY SI ASC FOR XML PATH('')), 1, 1, '') SI FROM Shop_Incidencia GROUP BY IncidenciaID " +
                                    " ) HI ON I.IncidenciaID = HI.IncidenciaID " +
                                " WHERE " +
                                    " I.SpoolID = " + SpoolID + " AND I.Activo = 1 ";


                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    List<IncidenciaC> lista = new List<IncidenciaC>();
                    IncidenciaC detalle;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        detalle = new IncidenciaC
                        {
                            Accion = 2,
                            IncidenciaID = int.Parse(ds.Tables[0].Rows[i]["IncidenciaID"].ToString()),
                            SpoolID = int.Parse(ds.Tables[0].Rows[i]["SpoolID"].ToString()),
                            NumeroControl = ds.Tables[0].Rows[i]["NumeroControl"].ToString(),
                            Incidencia = ds.Tables[0].Rows[i]["Incidencia"].ToString(),
                            MaterialJunta = ds.Tables[0].Rows[i]["MaterialJunta"].ToString(),
                            ErrorID = int.Parse(ds.Tables[0].Rows[i]["ErrorIncidenciaID"].ToString()),
                            Error = ds.Tables[0].Rows[i]["Error"].ToString(),
                            Observaciones = ds.Tables[0].Rows[i]["Observacion"].ToString(),
                            SI = ds.Tables[0].Rows[i]["SI"].ToString(),
                            Usuario = ds.Tables[0].Rows[i]["Cliente"].ToString(),
                            FechaIncidencia = ds.Tables[0].Rows[i]["FechaIncidencia"].ToString()
                        };
                        lista.Add(detalle);
                    }
                    return lista;
                }
            }
        }

        public string GuardarIncidencia(int SpoolID, int TipoIncidenciaID, int MaterialSpoolID, int JuntaSpoolID, int ErrorIncidenciaID, string Observacion, string Usuario, string SI, int TipoUsuario)
        {
            try
            {
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametro = {
                    { "@SpoolID", SpoolID.ToString() },
                    { "@TipoIncidenciaID", TipoIncidenciaID.ToString() },
                    { "@MaterialSpoolID", MaterialSpoolID.ToString() },
                    { "@JuntaSpoolID", JuntaSpoolID.ToString() },
                    { "@ErrorIncidenciaID",  ErrorIncidenciaID.ToString() },
                    { "@Observacion",  Observacion },
                    { "@Usuario",  Usuario },
                    { "@TipoUsuario", TipoUsuario.ToString() },
                    { "@SI",  SI }
                };
                return _SQL.EjecutaInsertUpdateRetornaString(Stords.GuardarIncidencia, parametro);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string GuardaAutorizacion(DataTable Detalle, string Usuario)
        {
            try
            {
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametro = { { "@Usuario", Usuario } };
                return _SQL.EjecutaInsertUpdateRetornaString(Stords.GuardaAutorizacion, Detalle, "@Captura", parametro);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GenerarSI(DataTable Detalle, Guid userID, string Inspector)
        {
            try
            {
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametro = {                    
                    { "@Usuario", userID.ToString() },
                    { "@Inspector", Inspector }                                        
                };
                return _SQL.EjecutaInsertUpdateRetornaString(Stords.GenerarSI, Detalle, "@Datos", parametro);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string ResolverEliminarIncidencia(int IncidenciaID, string Origen, string Resolucion, string Usuario, int Accion)
        {
            try
            {
                /*Origen: Ser refiere a la pantalla donde se resolvió la incidencia*/
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametro = { { "@IncidenciaID", IncidenciaID.ToString() }, { "@Accion", Accion.ToString() }, { "@Origen", Origen }, { "@Resolucion", Resolucion }, { "@Usuario", Usuario } };
                return _SQL.EjecutaInsertUpdateRetornaString(Stords.ResolverEliminarIncidencia, parametro);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public int ObtenerNumeroIncidencias(int SpoolID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = "SELECT " +
                                    " COUNT(*) Incidencias " +
                                " FROM " +
                                    " Shop_Incidencia " +
                                " WHERE " +
                                    " SpoolID = " + SpoolID + " AND Activo = 1 ";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return int.Parse(ds.Tables[0].Rows[0]["Incidencias"].ToString());
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        //public int ObtenerOrdenTrabajoSpoolID(string NumeroControl)
        //{
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
        //    {
        //        string query = " SELECT " +
        //                            " OT.OrdenTrabajoSpoolID " +
        //                        " FROM " +
        //                            " OrdenTrabajoSpool OT " +
        //                            " INNER JOIN Spool S WITH(NOLOCK) ON OT.SpoolID = S.SpoolID " +
        //                            " INNER JOIN Proyecto P WITH(NOLOCK) ON S.ProyectoID = P.ProyectoID AND P.Activo = 1 " +
        //                        " WHERE " +
        //                            " OT.NumeroControl = '" + NumeroControl + "'";
        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            DataSet ds = new DataSet();
        //            da.Fill(ds);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                return int.Parse(ds.Tables[0].Rows[0]["OrdenTrabajoSpoolID"].ToString());
        //            }
        //            else
        //            {
        //                return 0;
        //            }
        //        }
        //    }
        //}

        public string InactivarSpoolDeSI(string NumeroControl, int ProyectoID)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
                {
                    string query = " UPDATE S SET " +
                                        " S.sqinterno = NULL, " +
                                        " S.inspectorSQInterno = NULL, " +
                                        " S.fechaSQInterno = NULL " +
                                    " FROM " +
                                        " Spool S WITH(NOLOCK) " +
                                        " INNER JOIN OrdenTrabajoSpool OT WITH(NOLOCK) ON S.SpoolID = OT.SpoolID " +
                                    " WHERE " +
                                        " OT.NumeroControl = '" + NumeroControl + "' AND S.ProyectoID = " + ProyectoID;
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
                if(rowsAffected > 0)
                {
                    return "OK";
                }
                else
                {
                    return "Error";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }            
        }

        public List<AgregarSI> ObtenerSpoolAgregarSI(int SpoolID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = " SELECT " +
                                    " S.SpoolID, " +
                                    " S.ProyectoID, " +
                                    " C.CuadranteID, " +
                                    " C.Nombre Cuadrante, " +
                                    " OT.NumeroControl, " +
                                    " S.sq SqCliente, " +
                                    " S.sqinterno SI, " +
                                    " CASE WHEN H.TieneHoldIngenieria IS NULL THEN CAST(0 AS BIT) ELSE CAST(H.TieneHoldIngenieria AS BIT) END Hold, " +
                                    " CASE WHEN S.Campo7 = 'GRANEL' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END Granel " +
                                " FROM " +
                                    " Spool S WITH(NOLOCK) " +
                                    " INNER JOIN OrdenTrabajoSpool OT WITH(NOLOCK) ON S.SpoolID = OT.SpoolID " +
                                    " INNER JOIN Cuadrante C WITH(NOLOCK) ON S.CuadranteID = C.CuadranteID " +
                                    " LEFT JOIN SpoolHold H WITH(NOLOCK) ON S.SpoolID = H.SpoolID " +
                                    " LEFT JOIN " +
                                    " ( " +
                                        " SELECT SpoolID, COUNT(*) Incidencias FROM Shop_Incidencia WITH(NOLOCK) WHERE Activo = 1 GROUP BY SpoolID " +
                                    " ) I ON S.SpoolID = I.SpoolID " +
                                " WHERE " +
                                    " S.SpoolID = " + SpoolID + " AND (I.Incidencias = 0 OR I.Incidencias IS NULL) ";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    AgregarSI objeto;
                    List<AgregarSI> Lista = new List<AgregarSI>();
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        objeto = new AgregarSI{
                            SpoolID = int.Parse(ds.Tables[0].Rows[0]["SpoolID"].ToString()),
                            ProyectoID = int.Parse(ds.Tables[0].Rows[0]["ProyectoID"].ToString()),
                            CuadranteID = int.Parse(ds.Tables[0].Rows[0]["CuadranteID"].ToString()),
                            Cuadrante = ds.Tables[0].Rows[0]["Cuadrante"].ToString(),
                            NumeroControl = ds.Tables[0].Rows[0]["NumeroControl"].ToString(),
                            SqCliente = ds.Tables[0].Rows[0]["SqCliente"].ToString(),
                            SI = ds.Tables[0].Rows[0]["SI"].ToString(),
                            Hold = bool.Parse(ds.Tables[0].Rows[0]["Hold"].ToString()),
                            Granel = bool.Parse(ds.Tables[0].Rows[0]["Granel"].ToString())
                        };
                        Lista.Add(objeto);
                    }                    
                    return Lista;
                }                
            }
        }

        public bool EsGranel(int SpoolID)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlSam2"].ConnectionString))
            {
                string query = " SELECT ISNULL(Campo7, '') Campo7 FROM Spool WHERE SpoolID = " + SpoolID;
                SqlCommand cmd = new SqlCommand(query, con);
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader["Campo7"].ToString() == "GRANEL")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                reader.Close();
                con.Close();                                    
            }
            return false;
        }

        public int ObtenerDigitosODT(int proyectoID)
        {            
            using (SamContext ctx = new SamContext())
            {
                return (from a in ctx.ProyectoConfiguracion where a.ProyectoID == proyectoID select a.DigitosOrdenTrabajo).FirstOrDefault();
            }
        }

        public ObjectoSpool ObtenerDatosSpool(List<string> noControl, int ProyectoID)
        {            
            using (SamContext ctx = new SamContext())
            {
                ObjectoSpool Spool = new ObjectoSpool();
                var datos = (from a in ctx.OrdenTrabajoSpool
                             join b in ctx.OrdenTrabajo on a.OrdenTrabajoID equals b.OrdenTrabajoID
                             select new { a.OrdenTrabajoSpoolID, b.ProyectoID, a.NumeroControl, a.SpoolID }).Where(x => noControl.Contains(x.NumeroControl)).Where(a => a.ProyectoID == ProyectoID).ToList();

                Spool.SpoolID = datos.Select(a => a.SpoolID).FirstOrDefault();
                Spool.NumeroControl = datos.Select(a => a.NumeroControl).FirstOrDefault();
                return Spool;
            }
        }
    }
}
