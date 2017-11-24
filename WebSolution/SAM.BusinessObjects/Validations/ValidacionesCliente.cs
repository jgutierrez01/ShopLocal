using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesCliente
    {
        public static bool TieneProyecto(SamContext ctx, int clienteID)
        {
            return ctx.Proyecto.Any(x => x.ClienteID == clienteID);
        }
    }
}
