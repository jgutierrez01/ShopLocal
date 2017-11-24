using System;
using System.Collections.Generic;
using System.Web;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using System.Linq;

namespace SAM.BusinessLogic.Ingenieria
{
    public class SpoolPendienteHelper
    {

        private const string LlaveVariable = "SpoolPendientesXHomologar";
        public static List<SpoolPendientePorHomologar> SpoolsPendientesPorHomologar
        {
            get
            {
                if(HttpContext.Current.Session[LlaveVariable] == null)
                {
                    HttpContext.Current.Session[LlaveVariable] = new List<SpoolPendientePorHomologar>();
                }   
                return ((List<SpoolPendientePorHomologar>)HttpContext.Current.Session[LlaveVariable]);
            }

            set
            {
                HttpContext.Current.Session[LlaveVariable] = value;
            }
        }
        
        public static void GeneraSpoolsPendientesPorHomologar(int proyectoID)
        {
           SpoolsPendientesPorHomologar =
            IngenieriaBL.Instance.ObtenerPendientesPorHomologar(proyectoID).ToList().Select(
                x => new SpoolPendientePorHomologar {ProyectoID = proyectoID, SpoolPendienteID = x.SpoolPendienteID}).ToList();
        }

        public static void EliminaDePendientesSessionSpool(int spoolPendienteID)
        {
            List<SpoolPendientePorHomologar> pendientes = SpoolsPendientesPorHomologar;
            MaterialPendienteHelper.LimpiaPendientesSessionMaterialSpool();
            pendientes.RemoveAll(x => x.SpoolPendienteID == spoolPendienteID);
            SpoolsPendientesPorHomologar = pendientes;
        }

        public static int ObtenSiguienteEnListaPendientes(int spoolPendienteID)
        {
            try
            {
                List<SpoolPendientePorHomologar> pendientes = SpoolsPendientesPorHomologar;
                return pendientes.ElementAt(pendientes.IndexOf(pendientes.Single(x => x.SpoolPendienteID == spoolPendienteID)) + 1).SpoolPendienteID;

            }
            catch
            {
                return -1;
            }
        }
    }
}
