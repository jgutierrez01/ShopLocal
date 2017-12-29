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
                                    " CASE WHEN W.UsuarioOkPnd IS NULL THEN 0 ELSE 1 END OkPnd" +
                                " FROM " +
                                    " OrdenTrabajoSpool A WITH(NOLOCK) " +
                                    " LEFT JOIN Spool B WITH(NOLOCK) ON A.SpoolID = B.SpoolID " +
                                    " LEFT JOIN SpoolHold C WITH(NOLOCK) ON B.SpoolID = C.SpoolID " +
                                    " LEFT JOIN WorkstatusSpool W WITH(NOLOCK) ON A.OrdenTrabajoSpoolID = W.OrdenTrabajoSpoolID " +
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
                            OkPnd = ds.Tables[0].Rows[i]["OkPnd"].ToString() == "0" ? false : true
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
                                    " CASE WHEN W.UsuarioOkPnd IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END OkPnd " +
                                " FROM " +
                                    "   OrdenTrabajoSpool OS WITH(NOLOCK) " +
                                    " INNER JOIN Spool S WITH(NOLOCK) ON OS.SpoolID = S.SpoolID " +
                                    " INNER JOIN Cuadrante C WITH(NOLOCK) ON S.CuadranteID = C.CuadranteID " +
                                    " LEFT JOIN SpoolHold H WITH(NOLOCK) ON S.SpoolID = H.SpoolID " +
                                    " LEFT JOIN WorkstatusSpool W WITH(NOLOCK) ON OS.OrdenTrabajoSpoolID = W.OrdenTrabajoSpoolID " +
                                " WHERE " +
                                    " S.sqinterno = '" + Sq + "' AND S.proyectoID = " + proyectoID +
                                " ORDER BY OS.NumeroControl ASC";
                //string query = "select c.CuadranteID,c.Nombre,odts.NumeroControl,OrdenTrabajoSpoolID,sqinterno, s.SpoolID, sq SqCliente from OrdenTrabajoSpool odts WITH(NOLOCK)";
                //query += " inner join  Spool s WITH(NOLOCK) on odts.SpoolID = s.SpoolID";
                //query += "     inner join Cuadrante c WITH(NOLOCK) on s.CuadranteID = c.CuadranteID";
                //query += " where s.sqinterno = '" + Sq + "'" + " and s.proyectoID=" + proyectoID;

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
                            OkPnd = bool.Parse(ds.Tables[0].Rows[i]["OkPnd"].ToString())                            
                        };
                        lista.Add(cuadranteNumeroControlSQ);
                    }
                    return lista;
                }
            }
        }

        public List<CuadranteNumeroControlSQ> ObtenerSpoolsPorSQyProyecto(string Sq, int proyectoID)
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
                                    " CASE WHEN W.UsuarioOkPnd IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END OkPnd " +
                                " FROM " +
                                    "   OrdenTrabajoSpool OS WITH(NOLOCK) " +
                                    " INNER JOIN Spool S WITH(NOLOCK) ON OS.SpoolID = S.SpoolID " +
                                    " INNER JOIN Cuadrante C WITH(NOLOCK) ON S.CuadranteID = C.CuadranteID " +
                                    " LEFT JOIN SpoolHold H WITH(NOLOCK) ON S.SpoolID = H.SpoolID " +
                                    " LEFT JOIN WorkstatusSpool W WITH(NOLOCK) ON OS.OrdenTrabajoSpoolID = W.OrdenTrabajoSpoolID " +
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
                            Accion = 2,
                            CuadranteID = int.Parse(ds.Tables[0].Rows[i]["CuadranteID"].ToString()),
                            Cuadrante = ds.Tables[0].Rows[i]["Nombre"].ToString(),
                            NumeroControl = ds.Tables[0].Rows[i]["NumeroControl"].ToString(),
                            SpoolID = int.Parse(ds.Tables[0].Rows[i]["SpoolID"].ToString()),
                            OrdenTrabajoSpoolID = int.Parse(ds.Tables[0].Rows[i]["OrdenTrabajoSpoolID"].ToString()),
                            SqCliente = ds.Tables[0].Rows[i]["SqCliente"].ToString(),
                            SQ = ds.Tables[0].Rows[i]["sqinterno"].ToString(),
                            TieneHoldIngenieria = bool.Parse(ds.Tables[0].Rows[i]["TieneHoldIngenieria"].ToString()),
                            OkPnd = bool.Parse(ds.Tables[0].Rows[i]["OkPnd"].ToString())
                        };
                        lista.Add(cuadranteNumeroControlSQ);
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
                if(Opcion == 1)
                {
                    query += " SELECT 1 Accion,C.Nombre Cuadrante,C.CuadranteID,c.Nombre,A.NumeroControl,A.OrdenTrabajoSpoolID, B.SpoolID, B.sq SqCliente, B.sqinterno,D.TieneHoldIngenieria, CASE WHEN W.UsuarioOkPnd IS NULL THEN 0 ELSE 1 END OkPnd ";
                }
                else
                {
                    query += " SELECT 2 Accion,C.Nombre Cuadrante,C.CuadranteID,c.Nombre,A.NumeroControl,A.OrdenTrabajoSpoolID, B.SpoolID, B.sq SqCliente, B.sqinterno,D.TieneHoldIngenieria, CASE WHEN W.UsuarioOkPnd IS NULL THEN 0 ELSE 1 END OkPnd ";
                }
                query += " FROM " +
                            " OrdenTrabajoSpool A WITH(NOLOCK) " +
                            " INNER JOIN Spool B ON A.SpoolID = B.SpoolID " +
                            " INNER JOIN Cuadrante C WITH(NOLOCK) ON B.CuadranteID = C.CuadranteID " +
                            " LEFT JOIN SpoolHold D WITH(NOLOCK) ON B.SpoolID = D.SpoolID " +
                            " LEFT JOIN WorkstatusSpool W WITH(NOLOCK) ON A.OrdenTrabajoSpoolID = W.OrdenTrabajoSpoolID " +
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
                            OkPnd = ds.Tables[0].Rows[i]["OkPnd"].ToString() == "0" ? false : true
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
                         TipoNC = TipoNumeroControlEnum.AProcesar
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
                ObjetosSQL _SQL = new ObjetosSQL();
                string[,] parametro = null;
                if (Lista.Rows.Count > 0)
                    ds = _SQL.Coleccion(Stords.ObtieneSpoolIDTabla, Lista, "@TablaNumeroControl", parametro);
                if(ds != null)
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
                            OkPnd = row.Field<int>("OkPnd") == 0 ? false : true
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
    }
}
