using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using Mimo.Framework.Common;
using Mimo.Framework.Data;
using SAM.BusinessLogic.Excepciones;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Utilerias;
using SAM.Common;
using SAM.Entities;
using SAM.Entities.Grid;
using Mimo.Framework.Common;

namespace SAM.BusinessLogic.Calidad
{
    public class SeguimientoSpoolBL
    {
         private static readonly object _mutex = new object();
         private static SeguimientoSpoolBL _instance;

         private SeguimientoSpoolBL()
        {
        }

        /// <summary>
        /// Patron De Singleton
        /// </summary>
         public static SeguimientoSpoolBL Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SeguimientoSpoolBL();
                    }
                }
                return _instance;
            }
        }

         /// <summary>
         /// Propiedad de conveniencia para acceder a Cache
         /// </summary>
         private static Cache cache
         {
             get
             {
                 return HttpRuntime.Cache;
             }
         }

         public DataRow[] ObtenerDataSetParaSeguimientoSpool(int proyectoID, int? ordenTrabajoID, int? ordenTrabajoSpoolID , int? spoolID, string expresionOrdenamiento)
         {

             string llaveCache = "ObtenerDataSetParaSeguimientoSpool_" + proyectoID + "_" + ordenTrabajoID + "_" +
                                 ordenTrabajoSpoolID + "_" + spoolID;

             DataRow[] registrosOrdenados =(DataRow[])cache.Get(llaveCache);
             DataSet ds = new DataSet();
             if (registrosOrdenados != null)
             {
                 return registrosOrdenados;
             }
             
             
             const string nombreProc = "ObtenerSeguimientoDeSpools";
             string nombreTabla = "SegSpool";
             using (IDbConnection connection = DataAccessFactory.CreateConnection("SamDB"))
             {
                 IDbDataParameter[] parameters = DataAccess.GetSpParameterSet("SamDB", nombreProc);
                 parameters[0].Value = proyectoID;
                 parameters[1].Value = ordenTrabajoID;
                 parameters[2].Value = ordenTrabajoSpoolID;
                 parameters[3].Value = spoolID;


                 ds = DataAccess.ExecuteDataset(connection,
                                                CommandType.StoredProcedure,
                                                nombreProc,
                                                ds,
                                                nombreTabla,
                                                parameters);

                 registrosOrdenados = ds.Tables[0].Select(string.Empty, expresionOrdenamiento);
                 cache.Insert(llaveCache, registrosOrdenados, null,
                             DateTime.Now.AddMinutes(Configuracion.CacheMuyPocosMinutos),
                             Cache.NoSlidingExpiration);
                 return registrosOrdenados;
             }
         }
        
        /// <summary>
        /// Genera una nueva personalizacion
        /// </summary>
        /// <param name="pers"></param>
        /// <returns></returns>
        public void GuardaPersonalizacionSegmentoSpool(PersonalizacionSeguimientoSpool pers)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    if (!ctx.PersonalizacionSeguimientoSpool.Where(x => x.Nombre == pers.Nombre).Any())
                    {
                        ctx.PersonalizacionSeguimientoSpool.ApplyChanges(pers);
                        ctx.SaveChanges();
                    }
                }
              
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { "Error de concurrencia" });
            }
        }

        /// <summary>
        /// trae el id de la personalizacion guardada
        /// </summary>
        /// <param name="nombre">Nombre de la personalizacion</param>
        /// <param name="ctx"></param>
        /// <returns>int = Id de la personalizacion</returns>
        public int ObtenerPersonalizacionSeguimentoSpoolID(string nombre)
        {
            using (SamContext ctx = new SamContext())
            {
                return
                    ctx.PersonalizacionSeguimientoSpool.Where(x => x.Nombre == nombre).Select(
                        x => x.PersonalizacionSeguimientoSpoolID).FirstOrDefault();
            }
        }

        /// <summary>
        /// Guara todos los campos a mostrar en seguimiento spools en la base de datos relacionado a una personalizacion
        /// </summary>
        /// <param name="campos">lista de los campos a generar</param>
        public void GuardaDetallePersonalizacionSeguimientoSpool(List<DetallePersonalizacionSeguimientoSpool> campos)
        {
            try
            {
                using (SamContext ctx = new SamContext())
                {
                    foreach (var detallePersonalizacionSeguimiento in campos)
                    {
                        ctx.DetallePersonalizacionSeguimientoSpool.ApplyChanges(detallePersonalizacionSeguimiento);
                        ctx.SaveChanges();
                    }
                }
            }
            catch (OptimisticConcurrencyException)
            {
                throw new ExcepcionConcurrencia(new List<string>() { "Error de concurrencia" });
            }
        }

        public List<PersonalizacionSeguimientoSpool> ObtenerPersonalizacion(System.Guid userID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<PersonalizacionSeguimientoSpool> persSeguimSpool =
                    ctx.PersonalizacionSeguimientoSpool.Where(x => x.UserId == userID);
                return persSeguimSpool.ToList();
            }
        }


        public List<DetallePersonalizacionSeguimientoSpool> ObtenerDetallePersonalizacion(int personalizacionSeguimientoSpoolID)
        {
            using (SamContext ctx = new SamContext())
            {
                IQueryable<DetallePersonalizacionSeguimientoSpool> detallepersSeguimSpool =
                    ctx.DetallePersonalizacionSeguimientoSpool.Where(x => x.PersonalizacionSeguimientoSpoolID == personalizacionSeguimientoSpoolID);
                return detallepersSeguimSpool.ToList();
            }
        }

        public void BorrarPersonalizacionSeguimientoSpool(int PersSegSpoolID)
        {
            List<DetallePersonalizacionSeguimientoSpool> detsegJunta = new List<DetallePersonalizacionSeguimientoSpool>();

            using (SamContext ctx = new SamContext())
            {
                detsegJunta =
                    ctx.DetallePersonalizacionSeguimientoSpool.Where(
                        x => x.PersonalizacionSeguimientoSpoolID == PersSegSpoolID).ToList();

                foreach (var detallePersSeguimientoJunta in detsegJunta)
                {
                    ctx.DeleteObject(detallePersSeguimientoJunta);
                }

                PersonalizacionSeguimientoSpool PersSeguimientoSpool =
                    ctx.PersonalizacionSeguimientoSpool.FirstOrDefault(
                        x => x.PersonalizacionSeguimientoSpoolID == PersSegSpoolID);

                ctx.DeleteObject(PersSeguimientoSpool);

                ctx.SaveChanges();
            }
        }

        public void MensajeErrorChk()
        {
            throw new ExcepcionSeguimiento(new List<string> { MensajesError.Excepcion_SegimientoSpoolChk });
        }


        public List<string> ObtenerNombreCampoSeguimientoSpool(int[] CSSId)
        {
            List<string> NombresCampoSeguimientoSpool = null;

            using (SamContext ctx = new SamContext())
            {
                NombresCampoSeguimientoSpool = ctx.CampoSeguimientoSpool.Where(x => CSSId.Contains(x.CampoSeguimientoSpoolID)).Select(y => y.NombreColumnaSp).ToList();
            }
            return NombresCampoSeguimientoSpool;
        }

        public List<string> ObtenerTodosLosCamposSeguimientoSpool(string idioma,string columna)
        {
            List<string> NombresCampoSeguimientoSpool = null;

            using (SamContext ctx = new SamContext())
            {
                if (LanguageHelper.INGLES == idioma)
                    NombresCampoSeguimientoSpool = ctx.CampoSeguimientoSpool.Where(x => !x.NombreIngles.Contains(columna)).Select(y => y.NombreColumnaSp).ToList();
                else
                    NombresCampoSeguimientoSpool = ctx.CampoSeguimientoSpool.Where(x=>!x.Nombre.Contains(columna)).Select(y => y.NombreColumnaSp).ToList();
            }
            return NombresCampoSeguimientoSpool;
        }
    }
}
