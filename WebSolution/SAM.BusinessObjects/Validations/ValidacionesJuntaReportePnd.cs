using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public class ValidacionesJuntaReportePnd
    {
        public static bool TieneRelacionesJunta(int? JuntaWorkStatusId, SamContext ctx)
        {
            //Obtiene cualquier registro de la tabla JuntaReportePnd que exista en el ReportePnd
           
                if (ctx.JuntaWorkstatus.First(y => y.JuntaWorkstatusID == JuntaWorkStatusId).JuntaSoldaduraID == null)
                {
                    if (ctx.JuntaWorkstatus.First(y => y.JuntaWorkstatusID == JuntaWorkStatusId).JuntaInspeccionVisualID == null)
                    {
                        return true;
                    }
                }
                throw new ExcepcionDetReportePnd(new List<string>() { MensajesError.Excepcion_TieneRelacionSoldaduraeInspeccionVisual });
            
        }

        /// <summary>
        /// Envia una excepcion si se intenta dar un rechazo para más de una junta
        /// </summary>
        /// <param name="cantidadJuntas"></param>
        public static void ValidaCantidadJuntasParaRechazo(int cantidadJuntas)
        {
            if(cantidadJuntas > 1)
            {
                throw new ExcepcionDetReportePnd(MensajesError.Excepcion_RechazoParaMasJuntas); 
            }
        }
    }
}
