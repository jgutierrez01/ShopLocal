using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesContacto
    {
        public static bool ContactoDuplicado(SamContext ctx, string contactoNombre)
        {
            return ctx.Contacto.Any(x => x.Nombre == contactoNombre);
        }
    }
}
