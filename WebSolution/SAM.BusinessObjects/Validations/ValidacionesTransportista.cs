using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesTransportista
    {
        public static bool NombreExiste(SamContext ctx, string nombre, int? transportistaID)
        {
            return ctx.Transportista.Any(x => x.Nombre == nombre && x.TransportistaID != transportistaID);
        }

        public static bool TieneTransportistaProyecto(SamContext ctx, int transportistaID)
        {
            return ctx.TransportistaProyecto.Any(x => x.TransportistaID == transportistaID);
        }

        public static bool TieneRecepcion(SamContext ctx, int transportistaID)
        {
            return ctx.Recepcion.Any(x => x.TransportistaID == transportistaID);
        }

        public static bool TieneJuntaSoldaduraDetalle(SamContext ctx, int soldadorID)
        {
            return ctx.JuntaSoldaduraDetalle.Any(x => x.SoldadorID == soldadorID);
        }

        public static bool TieneWpq(SamContext ctx, int soldadorID)
        {
            return ctx.Wpq.Any(x => x.SoldadorID == soldadorID);
        }

        public static bool TieneDestajoSoldador(SamContext ctx, int soldadorID)
        {
            return ctx.DestajoSoldador.Any(x => x.SoldadorID == soldadorID);
        }
    }
}
