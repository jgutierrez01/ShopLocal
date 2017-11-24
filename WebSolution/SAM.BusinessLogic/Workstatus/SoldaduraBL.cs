using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using Mimo.Framework.Common;
using SAM.BusinessLogic.Administracion;
using SAM.BusinessObjects.Catalogos;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Ingenieria;
using SAM.BusinessObjects.Materiales;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using SAM.Entities.Cache;

namespace SAM.BusinessLogic.Workstatus
{
    public class SoldaduraBL
    {
        private static readonly object _mutex = new object();
        private static SoldaduraBL _instance;

        /// <summary>
        /// constructro privado para implementar patron singleton
        /// </summary>
        private SoldaduraBL()
        {
        }

        /// <summary>
        /// obtiene la instancia de la clase OrdenTrabajoBL
        /// </summary>
        public static SoldaduraBL Instance
        {
            get
            {
                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new SoldaduraBL();
                    }
                }
                return _instance;
            }
        }

        public IEnumerable<WpsCache> ObtenerWpsterminadoConRaiz(int juntaSpoolID, int proyectoID, int procesoRaizID, string material1, string material2
            , decimal? espesorJunta)
        {
            int spoolID = JuntaSpoolBO.Instance.Obtener(juntaSpoolID).SpoolID;
            Spool spool = SpoolBO.Instance.Obtener(spoolID);
            
            IEnumerable<WpsCache> origen = new List<WpsCache>();

            //esto es por si la espesor viene nulo
            espesorJunta = espesorJunta.GetValueOrDefault(0);

            if (material2 == string.Empty)
            {

                origen =
                    WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                    .Where(x => x.FamiliaAcero1 == material1
                        && x.FamiliaAcero2 == material1
                        && x.ProcesoRaizID == procesoRaizID
                        && x.EspesorRellenoMaximo >= espesorJunta);

            }
            else
            {
                origen =
                 WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                 .Where(x => ((x.FamiliaAcero1 == material1
                     && x.FamiliaAcero2 == material2)
                     || (x.FamiliaAcero1 == material2
                     && x.FamiliaAcero2 == material1))
                     && x.ProcesoRaizID == procesoRaizID
                     && x.EspesorRellenoMaximo >= espesorJunta);
            }
            return spool.RequierePwht ? origen.Where(x => x.RequierePwht) : origen;
        }

        public int ObtenerProcesoRelleno(string proceso)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.ProcesoRelleno.Where(x => x.Nombre.Equals(proceso)).Select(x => x.ProcesoRellenoID).FirstOrDefault();
            }
        }

        public IEnumerable<WpsCache> ObtenerWps(int juntaSpoolID, int proyectoID, int procesoRaizID, int procesoRellenoID, string material1, string material2, decimal? espesorJunta, bool wpsDiferentes, bool regresarWpsRaiz)
        {
            int spoolID = JuntaSpoolBO.Instance.Obtener(juntaSpoolID).SpoolID;
            Spool spool = SpoolBO.Instance.Obtener(spoolID);

            IEnumerable<WpsCache> origen = new List<WpsCache>();

            //esto es por si la espesor viene nulo
            espesorJunta = espesorJunta.GetValueOrDefault(0);

            //si es un solo Wps
            if (!wpsDiferentes)
            {
                if (procesoRaizID > 0 && procesoRellenoID > 0)
                {
                    string procesoRaiz = ProcesoRaizBO.Instance.Obtener(procesoRaizID).Codigo.Trim();
                    string procesoRelleno = ProcesoRellenoBO.Instance.Obtener(procesoRellenoID).Codigo.Trim();

                    //se elimina esta logica a peticion del cliente Feb 2014
                    //si son iguales los procesos raiz y relleno
                    //if (procesoRaiz == procesoRelleno)
                    //{
                    //    //si el material1  es igual al material 2
                    //    if (material2 == string.Empty)
                    //    {

                    //        origen =
                    //            WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                    //            .Where(x => x.FamiliaAcero1 == material1
                    //                && x.FamiliaAcero2 == material1
                    //                && x.ProcesoRaizID == procesoRaizID
                    //                && x.EspesorRaizMaximo >= espesorJunta);

                    //    }
                    //    else
                    //    {
                    //        origen =
                    //         WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                    //         .Where(x => ((x.FamiliaAcero1 == material1
                    //             && x.FamiliaAcero2 == material2)
                    //             || (x.FamiliaAcero1 == material2
                    //             && x.FamiliaAcero2 == material1))
                    //             && x.ProcesoRaizID == procesoRaizID
                    //             && x.EspesorRaizMaximo >= espesorJunta);
                    //    }
                    //}
                    //else
                    //{
                        //si los procesos son diferentes
                        if (material2 == string.Empty)
                        {

                            origen =
                                WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                                .Where(x => x.FamiliaAcero1 == material1
                                    && x.FamiliaAcero2 == material1
                                    && x.ProcesoRaizID == procesoRaizID
                                    && x.ProcesoRellenoID == procesoRellenoID
                                    && x.EspesorRellenoMaximo >= espesorJunta);
                            //.Union(
                            //        WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                            //         .Where(x => x.FamiliaAcero1 == material1
                            //            && x.FamiliaAcero2 == material1
                            //            && x.ProcesoRaizID == procesoRaizID                                            
                            //            && x.EspesorRaizMaximo >= espesorJunta)
                            //).Distinct();

                        }
                        else
                        {
                            origen =
                             WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                             .Where(x => ((x.FamiliaAcero1 == material1
                                 && x.FamiliaAcero2 == material2)
                                 || (x.FamiliaAcero1 == material2
                                 && x.FamiliaAcero2 == material1))
                                 && x.ProcesoRaizID == procesoRaizID
                                 && x.ProcesoRellenoID == procesoRellenoID
                                 && x.EspesorRellenoMaximo >= espesorJunta);
                            //.Union(
                            //            WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                            //             .Where(x => ((x.FamiliaAcero1 == material1
                            //                 && x.FamiliaAcero2 == material2)
                            //                 || (x.FamiliaAcero1 == material2
                            //                 && x.FamiliaAcero2 == material1))
                            //                 && x.ProcesoRaizID == procesoRaizID
                            //                 && x.EspesorRaizMaximo >= espesorJunta)
                            //    ).Distinct();
                        }
                    //}
                }
            }
            else
            {
                //si son wps independientes
                if (regresarWpsRaiz)
                {
                    //aqui no se revisan espesores
                    //traer wps que tengan ese proceso raiz y que coincidan materiales 
                    material2 = string.IsNullOrEmpty(material2) ? material1 : material2;

                    origen = WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                             .Where(x => ((x.FamiliaAcero1 == material1
                                 && x.FamiliaAcero2 == material2)
                                 || (x.FamiliaAcero1 == material2
                                 && x.FamiliaAcero2 == material1))
                                 && x.ProcesoRaizID == procesoRaizID);


                }
                else
                {
                    //traer wps que tengan ese proceso relleno y que coincidan materiales
                    //que el espesor total maximo sea igual al de la junta
                    material2 = string.IsNullOrEmpty(material2) ? material1 : material2;

                    origen =
                             WpsBO.Instance.ObtenerCachePorProyecto(proyectoID)
                             .Where(x => ((x.FamiliaAcero1 == material1
                                 && x.FamiliaAcero2 == material2)
                                 || (x.FamiliaAcero1 == material2
                                 && x.FamiliaAcero2 == material1))
                                 && x.ProcesoRellenoID == procesoRellenoID
                                 && x.EspesorRellenoMaximo >= espesorJunta);
                }
            }
            return spool.RequierePwht ? origen.Where(x => x.RequierePwht) : origen;
        }
    }
}
