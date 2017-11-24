using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesPerfil
    {
        public static bool TieneUsuariosRelacionados(SamContext ctx, int perfilID)
        {
            return ctx.Usuario.Any(x => x.PerfilID == perfilID);
        }
    }
}
