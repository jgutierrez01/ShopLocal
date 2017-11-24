using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesProveedor
    {
        public static bool NombreDuplicado(SamContext ctx, string nombreProveedor, int? proveedorID)
        {
            return ctx.Proveedor.Any(x => x.Nombre == nombreProveedor && x.ProveedorID != proveedorID);
        }

        public static bool TieneProveedorProyecto(SamContext ctx, int proveedorID)
        {
            return ctx.ProveedorProyecto.Any(x => x.ProveedorID == proveedorID);
        }

        public static bool TieneNumeroUnico(SamContext ctx, int proveedorID)
        {
            return ctx.NumeroUnico.Any(x => x.ProveedorID == proveedorID);
        }
    }
}
