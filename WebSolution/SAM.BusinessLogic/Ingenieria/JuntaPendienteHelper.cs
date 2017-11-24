using System;
using System.Collections.Generic;
using System.Web;
using SAM.Entities;
using SAM.Entities.Personalizadas;

namespace SAM.BusinessLogic.Ingenieria
{
    public class JuntaPendienteHelper
    {

        private const string LlaveVariable = "juntasPendientesXHomologar";
        public static List<JuntaPendientePorHomologar> JuntasPendientesPorHomologar
        {
            get
            {
                if(HttpContext.Current.Session[LlaveVariable] == null)
                {
                    HttpContext.Current.Session[LlaveVariable] = new List<MaterialPendientePorHomologar>();
                }
                return (List<JuntaPendientePorHomologar>)HttpContext.Current.Session[LlaveVariable];
            }

        }
        
        public static void AgregarJuntasPendientesXHomologar(int juntaSpoolPendienteId, int juntaSpoolId, AccionesHomologacion accion, bool pasoValidacion, String mensajeValidacion)
        {
            JuntaPendientePorHomologar nuevoRegistro = new JuntaPendientePorHomologar
                                                    {
                                                        Accion = accion,
                                                        JuntaSpoolID = juntaSpoolId,
                                                        JuntaSpoolPendienteID = juntaSpoolPendienteId,
                                                        MensajeValidacion = mensajeValidacion,
                                                        PasoValidacion = pasoValidacion
                                                    };


            JuntasPendientesPorHomologar.Add(nuevoRegistro);
        }
    }
}
