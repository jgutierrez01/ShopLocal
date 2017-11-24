using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects.Modelo;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesPatio
    {

        public static bool NomenclaturaDuplicada(SamContext ctx, string nombre, int? idPatio)
        {
            return ctx.Patio.Any(x => x.Nombre == nombre && x.PatioID != idPatio);
        }
        public static bool TieneTaller(SamContext ctx, int patioID)
        {
            return ctx.Taller.Any(x => x.PatioID == patioID);
        }
        public static bool TieneMaquina(SamContext ctx, int patioID)
        {
            return ctx.Maquina.Any(x => x.PatioID == patioID);
        }
        public static bool TieneSoldador(SamContext ctx, int patioID)
        {
            return ctx.Soldador.Any(x => x.PatioID == patioID);
        }

        internal static bool TieneProyecto(SamContext ctx, int patioID)
        {
            return ctx.Proyecto.Any(x => x.PatioID == patioID);
        }

        internal static bool TieneUbicacionFisica(SamContext ctx, int patioID)
        {
            return ctx.UbicacionFisica.Any(x => x.PatioID == patioID);
        }

        internal static bool TieneTubero(SamContext ctx, int patioID)
        {
            return ctx.Tubero.Any(x => x.PatioID == patioID);
        }

        internal static bool TieneConsumible(SamContext ctx, int patioID)
        {
            return ctx.Consumible.Any(x => x.PatioID == patioID);
        }
    }
}
