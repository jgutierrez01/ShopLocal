using SAM.Entities.Busqueda;
using SAM.Web.Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;
using SAM.Entities;
using SAM.Entities.Personalizadas.Shop;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: log4net.Config.Repository()]
namespace SAM.Web.Shop.Utils
{
    public class Helps
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Helps));

        public static string GetNumberControlsModelToString(WorkstatusModel model, TipoNumeroControlEnum tipo)
        {
            string spools = string.Empty;

            List<NumeroControlBusqueda> ncbs = new List<NumeroControlBusqueda>();     

            if (tipo == TipoNumeroControlEnum.ConProceso)
            {
                ncbs = model.ControlNumberWhitProcess;
            }
            else if (tipo == TipoNumeroControlEnum.AProcesar)
            {
                ncbs = model.ControlNumberToProcess;
            }
            else if (tipo == TipoNumeroControlEnum.NoCumple)
            {
                ncbs = model.ControlNumberNotConditions;
            }
            else if (tipo == TipoNumeroControlEnum.FechaInvalida)
            {
                ncbs = model.ControlNumberInvalidDate;
            }
            else if (tipo == TipoNumeroControlEnum.Procesado)
            {
                ncbs = model.ControlNumberToProcess;
            }

            if (ncbs.Count > 0)
            {
                spools = JsonConvert.SerializeObject(ncbs);
            }

            return spools;
        }

        public static string GetNumberControlsModelToString(LocationModel model, TipoNumeroControlEnum tipo)
        {
            string spools = string.Empty;

            List<NumeroControlBusqueda> ncbs = new List<NumeroControlBusqueda>();             
            
            if(tipo == TipoNumeroControlEnum.ConProceso)
            {
                ncbs = model.ControlNumberWhitProcess;
            }
            else
            {
                ncbs = model.ControlNumberToProcess;
            }

            if(ncbs.Count > 0)
            {
                spools = JsonConvert.SerializeObject(ncbs);
            }
            return spools;
        }

        public static List<NumeroControlBusqueda> GeControlNumbersStringToNCB(string spools)
        {            
            List<NumeroControlBusqueda> SpoolsAgregados = new List<NumeroControlBusqueda>();

            if (!string.IsNullOrEmpty(spools))
            {
               SpoolsAgregados = JsonConvert.DeserializeObject<List<NumeroControlBusqueda>>(spools);          
            }

            return SpoolsAgregados;
        }

        public static List<LayoutGridSQ> GetListadoCuadrantesNumeroControlSQ(string cuadranteNumeroControlSQ)
        {
            List<LayoutGridSQ> listadoCuadrantesNumeroControlSQ = new List<LayoutGridSQ>();

            if (!string.IsNullOrEmpty(cuadranteNumeroControlSQ))
            {
                listadoCuadrantesNumeroControlSQ = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(cuadranteNumeroControlSQ);
            }

            return listadoCuadrantesNumeroControlSQ;
        }
        
        public static List<LayoutGridSQ> GetListadoCuadrantesNumeroControlSQEditar(string cuadranteNumeroControlSQ)
        {
            List<LayoutGridSQ> listadoCuadrantesNumeroControlSQ = new List<LayoutGridSQ>();

            if (!string.IsNullOrEmpty(cuadranteNumeroControlSQ))
            {
                listadoCuadrantesNumeroControlSQ = JsonConvert.DeserializeObject<List<LayoutGridSQ>>(cuadranteNumeroControlSQ);
            }

            return listadoCuadrantesNumeroControlSQ;
        }

        public static string GetSpoolsCookie(List<NumeroControlBusqueda> ncbs)
        {
            string spools = string.Empty;
         
            spools = JsonConvert.SerializeObject(ncbs);         

            return spools;
        }

        public static string GetSpoolCookies(NumeroControlBusqueda ncb, List<NumeroControlBusqueda> ncbs)
        {
            string spools = string.Empty;

            string spool = JsonConvert.SerializeObject(ncb);
            ncbs.Add(ncb);     
           
            spools = JsonConvert.SerializeObject(ncbs);

            return spools;
        }

        public static string GetNumberControlsSQCookies(List<LayoutGridSQ> listaElementosYaEnGrid)
        {
            string lista = string.Empty;

            lista = JsonConvert.SerializeObject(listaElementosYaEnGrid);

            return lista;
        }

        public static string GetNumberControlsSQCookies(IEnumerable<LayoutGridSQ> listaElementosEncontrados, List<LayoutGridSQ> listaElementosYaEnGrid)
        {
            string nuevaListaSerializadaConElementos = string.Empty;

            for (int i = 0; i < listaElementosEncontrados.Count(); i++)
            {
               
                listaElementosYaEnGrid.Add(listaElementosEncontrados.ElementAt(i));
            }

            nuevaListaSerializadaConElementos = JsonConvert.SerializeObject(listaElementosYaEnGrid);

            return nuevaListaSerializadaConElementos;
        }

        public static string GetNumberControlsSQEditarCookies(IEnumerable<LayoutGridSQ> listaElementosEncontrados, List<LayoutGridSQ> listaElementosYaEnGrid)
        {
            string nuevaListaSerializadaConElementos = string.Empty;

            for (int i = 0; i < listaElementosEncontrados.Count(); i++)
            {

                listaElementosYaEnGrid.Add(listaElementosEncontrados.ElementAt(i));
            }

            nuevaListaSerializadaConElementos = JsonConvert.SerializeObject(listaElementosYaEnGrid);

            return nuevaListaSerializadaConElementos;
        }

        

        public static string GetSpoolsCookies(List<NumeroControlBusqueda> ncbsInput, List<NumeroControlBusqueda> ncbs)
        {
            string spools = string.Empty;

            foreach(NumeroControlBusqueda ncb in ncbsInput)
            {
                if (!ncbs.Where(x => x.SpoolID == ncb.SpoolID).Any())
                {
                    string spool = JsonConvert.SerializeObject(ncb);
                    ncbs.Add(ncb);
                }                
            }

            spools = JsonConvert.SerializeObject(ncbs);

            return spools;
        }

        public static string GetAllNumbersControlModelToString(LocationModel model)
        {
            string spools = string.Empty;

            List<NumeroControlBusqueda> ncbs = new List<NumeroControlBusqueda>();
          
            ncbs.AddRange( model.ControlNumberWhitProcess);
           
            ncbs.AddRange( model.ControlNumberToProcess );            

            if (ncbs.Count > 0)
            {
                spools = JsonConvert.SerializeObject(ncbs);
            }
            return spools;
        }       

        public static string GetAllNumbersControlModelToString(WorkstatusModel model)
        {
            string spools = string.Empty;

            List<NumeroControlBusqueda> ncbs = new List<NumeroControlBusqueda>();

            ncbs.AddRange(model.ControlNumberWhitProcess);
            ncbs.AddRange(model.ControlNumberToProcess);
            ncbs.AddRange(model.ControlNumberNotConditions); 
            ncbs.AddRange(model.ControlNumberInvalidDate);

            if (ncbs.Count > 0)
            {
                spools = JsonConvert.SerializeObject(ncbs);
            }
            return spools;
        }

        public static T ConvertObjects<T, J>(J o) 
        {             
             string json = JsonConvert.SerializeObject(o); 
             
            T objeto  = JsonConvert.DeserializeObject<T>(json); 
 
            return objeto; 
        }

    }
}
