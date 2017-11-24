using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;
using SAM.BusinessObjects.Workstatus;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesSpool
    {
        /// <summary>
        /// Valida si el spool se encuentra en hold
        /// </summary>
        /// <param name="spoolID">ID del spool a verificar</param>
        /// <returns>Falso si no se encuentra en hold, de lo contrario lanza una excepcion</returns>
        public static bool SpoolEnHold(int spoolID)
        {
            if (RevisionHoldsBO.Instance.SpoolTieneHold(spoolID))
            {
                throw new ExcepcionEnHold(MensajesError.Excepcion_SpoolEnHold);
            }
            else
            {
                return false;
            }

        }

        //Verifica si el spool se encuentra en ODT
        public static bool TieneODT(SamContext ctx, int spoolID)
        {
            return ctx.OrdenTrabajoSpool.Any(x => x.SpoolID == spoolID);
        }

        //Verifica si el spool se encuentra en ODT
        public static bool TieneODT(int spoolId)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.OrdenTrabajoSpool.Any(x => x.SpoolID == spoolId);
            }
        }
    }
}
