using System;
using System.Collections.Generic;
using System.Web;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using System.Linq;

namespace SAM.BusinessLogic.Ingenieria
{
    public class MaterialPendienteHelper
    {

        private const string LlaveVariable = "materialesPendientesXHomologar";
        public static List<MaterialPendientePorHomologar> MaterialesPendientesPorHomologar
        {
            get
            {
                if(HttpContext.Current.Session[LlaveVariable] == null)
                {
                    HttpContext.Current.Session[LlaveVariable] = new List<MaterialPendientePorHomologar>();
                }
                return ((List<MaterialPendientePorHomologar>)HttpContext.Current.Session[LlaveVariable]).OrderBy(x=> ((int)x.Accion)).ToList();
            }

            set
            {
                HttpContext.Current.Session[LlaveVariable] = value;
            }
        }
        
        public static void AgregarMaterialesPendientesXHomologar(int materialSpoolPendienteId, int materialSpoolId, AccionesHomologacion accion, bool pasoValidacion, String mensajeValidacion)
        {
            MaterialPendientePorHomologar nuevoRegistro = new MaterialPendientePorHomologar
                                                    {
                                                        Accion = accion,
                                                        MaterialSpoolID = materialSpoolId,
                                                        MaterialSpoolPendienteID = materialSpoolPendienteId,
                                                        MensajeValidacion = mensajeValidacion,
                                                        PasoValidacion = pasoValidacion
                                                    };

            List<MaterialPendientePorHomologar> list = MaterialesPendientesPorHomologar;
            list.Add(nuevoRegistro);
            MaterialesPendientesPorHomologar = list;
        }

        public static void LimpiaPendientesSessionMaterialSpool()
        {
            List<MaterialPendientePorHomologar> pendientes = MaterialesPendientesPorHomologar;
            pendientes.Clear();
            MaterialesPendientesPorHomologar = pendientes;
        }        
    }
}
