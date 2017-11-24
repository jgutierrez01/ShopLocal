using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Catalogos;
using SAM.Entities;
using SAM.Entities.Grid;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Cache;

namespace SAM.BusinessLogic.Utilerias
{
    public static class PivoteEstimacion
    {

        /// <summary>
        ///  se creara el dataset de la Estimacion Junta,
        ///  se creara la tabla y las columnas asi como los registros y
        ///  se hara un pivoteo hacia la tabla del campo Concepto estimacion
        /// </summary>
        /// <param name="lista">trae los registros del grid</param>
        /// <returns></returns>
        public static DataSet PivotearDatosEstimacionJunta(List<GrdEstimacionJuntaCompleta> lista, int proyectoID)
        {
            //se crea el dataset
            DataSet ds = new DataSet("Estimacion");

            //se crea la tabla
            DataTable dt = new DataTable("EstimacionTbl");

            List<ConceptoEstimacionCache> lst = CacheCatalogos.Instance.ObtenerConceptosEstimacionJunta();
            //se crean las columnas de la tabla 

            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.JuntaWorkstatusId, typeof (int)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.NumeroControl, typeof (string)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.NombreSpool, typeof(string)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.Etiqueta, typeof (string)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.TipodeJunta, typeof (string)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.Diametro, typeof (string)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.Material, typeof (string)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.Cedula, typeof(string)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.Armada, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.Soldada, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.InspeccionVisual, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.InspeccionDimensional, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.RTAprobado, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.PTAprobado, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.PWHTAprobado, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.DurezasAprobado, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.RTPostTTAprobado, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.PTPostTTAprobado, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.PreheatAprobado, typeof (bool)));
            dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionJunta.UTAprobado, typeof (bool)));

            //son 18 columnas antes
            //La lista de cache ya viene ordenada y en el idioma que la necesitamos
            lst.ForEach(x => dt.Columns.Add(new DataColumn(x.Nombre,typeof (string))));

            //se agrega la tabla al dataset
            ds.Tables.Add(dt);

            //se agrupan todos los campos necesarios para el pivoteo
            var groups = from jta in lista
                         group jta by new
                                          {
                                              jta.JuntaWorkStatusID,
                                              jta.NumeroControl,
                                              jta.NombreSpool,
                                              jta.Armada,
                                              jta.Diametro,
                                              jta.DurezasAprobado,
                                              jta.Etiqueta,
                                              jta.InspeccionDimensional,
                                              jta.InspeccionVisual,
                                              jta.Material,
                                              jta.Cedula,
                                              jta.PreHeatAprobado,
                                              jta.PtAprobado,
                                              jta.PtPostTtAprobado,
                                              jta.PwhtAprobado,
                                              jta.RtAprobado,
                                              jta.RtPostTtAprobado,
                                              jta.Soldada,
                                              jta.TipoDeJunta,
                                              jta.UtAprobado
                                          }
                         into myGroups
                         select new
                                    {
                                        myGroups.Key.JuntaWorkStatusID,
                                        myGroups.Key.NumeroControl,
                                        myGroups.Key.NombreSpool,
                                        myGroups.Key.Armada,
                                        myGroups.Key.Diametro,
                                        myGroups.Key.DurezasAprobado,
                                        myGroups.Key.Etiqueta,
                                        myGroups.Key.InspeccionDimensional,
                                        myGroups.Key.InspeccionVisual,
                                        myGroups.Key.Material,
                                        myGroups.Key.Cedula,
                                        myGroups.Key.PreHeatAprobado,
                                        myGroups.Key.PtAprobado,
                                        myGroups.Key.PtPostTtAprobado,
                                        myGroups.Key.PwhtAprobado,
                                        myGroups.Key.RtAprobado,
                                        myGroups.Key.RtPostTtAprobado,
                                        myGroups.Key.Soldada,
                                        myGroups.Key.TipoDeJunta,
                                        myGroups.Key.UtAprobado,
                                    };


            //se crea un data row para poner los registros
            DataRow row;

            var hash = lista.ToDictionary(x => new {JuntaWorkstatusID = x.JuntaWorkStatusID, ConceptoEstimacionID = x.ConceptoEstimacionID.GetValueOrDefault(-1)}, y => y.NumeroEstimacion);

            foreach (var est in groups)
            {
                //se les asignan los registros a sus respectivas columnas
                row = dt.NewRow();
                row[TitulosColumnaEstimacionJunta.JuntaWorkstatusId] = est.JuntaWorkStatusID;
                row[TitulosColumnaEstimacionJunta.NumeroControl] = est.NumeroControl;
                row[TitulosColumnaEstimacionJunta.NombreSpool] = est.NombreSpool;
                row[TitulosColumnaEstimacionJunta.Etiqueta] = est.Etiqueta;
                row[TitulosColumnaEstimacionJunta.TipodeJunta] = est.TipoDeJunta;
                row[TitulosColumnaEstimacionJunta.Diametro] = string.Format("{0:#0.000}", est.Diametro);
                row[TitulosColumnaEstimacionJunta.Material] = est.Material;
                row[TitulosColumnaEstimacionJunta.Cedula] = est.Cedula;
                row[TitulosColumnaEstimacionJunta.Armada] = est.Armada;
                row[TitulosColumnaEstimacionJunta.Soldada] = est.Soldada;
                row[TitulosColumnaEstimacionJunta.InspeccionVisual] = est.InspeccionVisual;
                row[TitulosColumnaEstimacionJunta.InspeccionDimensional] = est.InspeccionDimensional;
                row[TitulosColumnaEstimacionJunta.RTAprobado] = est.RtAprobado;
                row[TitulosColumnaEstimacionJunta.PTAprobado] = est.PtAprobado;
                row[TitulosColumnaEstimacionJunta.PWHTAprobado] = est.PwhtAprobado;
                row[TitulosColumnaEstimacionJunta.DurezasAprobado] = est.DurezasAprobado;
                row[TitulosColumnaEstimacionJunta.RTPostTTAprobado] = est.RtPostTtAprobado;
                row[TitulosColumnaEstimacionJunta.PTPostTTAprobado] = est.PtPostTtAprobado;
                row[TitulosColumnaEstimacionJunta.PreheatAprobado] = est.PreHeatAprobado;
                row[TitulosColumnaEstimacionJunta.UTAprobado] = est.UtAprobado;

                foreach (ConceptoEstimacionCache concepto in lst)
                {
                    var key = new {JuntaWorkstatusID = est.JuntaWorkStatusID, ConceptoEstimacionID = concepto.ID};
                    row[concepto.Nombre] = hash.ContainsKey(key) ? hash[key] : string.Empty;
                }

                dt.Rows.Add(row); //se agrega el renglon a la tabla
            }
            return ds; //regresa los datos
        }




        public static DataSet PivotearDatosEstimacionSpool(List<GrdEstimacionSpoolCompleta> lista, int proyectoID)
        {
            //se crea el dataset
            DataSet ds = new DataSet("Estimacion");

            //se crea la tabla
            DataTable dt = new DataTable("EstimacionTbl");

            List<ConceptoEstimacionCache> lst = CacheCatalogos.Instance.ObtenerConceptosEstimacionSpool();

            //se crean las columnas de la tabla 
             dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionSpool.WorkstatusSpoolId, typeof(int)));
             dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionSpool.NumeroControl, typeof(string)));
             dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionSpool.Spool, typeof(string)));
             dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionSpool.PDI, typeof(decimal)));
             dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionSpool.Material, typeof(string)));
             dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionSpool.Cedula, typeof(string)));
             dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionSpool.InspeccionDimensional, typeof(bool)));
             dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionSpool.Pintura, typeof(bool)));
             dt.Columns.Add(new DataColumn(TitulosColumnaEstimacionSpool.Embarcado, typeof(bool)));

             //La lista de cache ya viene ordenada y en el idioma que la necesitamos
             lst.ForEach(x => dt.Columns.Add(new DataColumn(x.Nombre, typeof(string))));

            //se agrega la tabla al dataset
            ds.Tables.Add(dt);

            //se agrupan todos los campos necesarios para el pivoteo
            var groups = from jta in lista
                         group jta by new
                         {
                             jta.WorkStatusSpoolID,
                             jta.NumeroControl,
                             jta.Spool,
                             jta.PDI,
                             jta.Material,
                             jta.Cedula,
                             jta.InspecciónDimensional,
                             jta.Pintura,
                             jta.Embarcado
                             
                         }
                             into myGroups
                             select new
                             {
                                 myGroups.Key.WorkStatusSpoolID,
                                 myGroups.Key.NumeroControl,
                                 myGroups.Key.Spool,
                                 myGroups.Key.PDI,
                                 myGroups.Key.Material,
                                 myGroups.Key.Cedula,
                                 myGroups.Key.InspecciónDimensional,
                                 myGroups.Key.Pintura,
                                 myGroups.Key.Embarcado
                             };


            //se crea un data row para poner los registros
            DataRow row;
            
                foreach (var est in groups)
                {
                    //se les asignan los registros a sus respectivas columnas
                    row = dt.NewRow();
                    row[TitulosColumnaEstimacionSpool.WorkstatusSpoolId] = est.WorkStatusSpoolID;
                    row[TitulosColumnaEstimacionSpool.NumeroControl] = est.NumeroControl;
                    row[TitulosColumnaEstimacionSpool.Spool] = est.Spool;
                    row[TitulosColumnaEstimacionSpool.PDI] = est.PDI;
                    row[TitulosColumnaEstimacionSpool.Material] = est.Material;
                    row[TitulosColumnaEstimacionSpool.Cedula] = est.Cedula;
                    row[TitulosColumnaEstimacionSpool.InspeccionDimensional] = est.InspecciónDimensional;
                    row[TitulosColumnaEstimacionSpool.Pintura] = est.Pintura;
                    row[TitulosColumnaEstimacionSpool.Embarcado] = est.Embarcado;

                    foreach (ConceptoEstimacionCache concepto in lst)
                    {
                        GrdEstimacionSpoolCompleta elemento =
                            lista.Where(x => x.WorkStatusSpoolID == est.WorkStatusSpoolID)
                                     .Where(x => x.ConceptoEstimacionID == concepto.ID)
                                     .SingleOrDefault();

                        row[concepto.Nombre] = elemento != null ? elemento.NumeroEstimacion : string.Empty;
                    }

                    dt.Rows.Add(row); //se agrega el renglon a la tabla
                }
            return ds; //regresa los datos
        }
    }
}
