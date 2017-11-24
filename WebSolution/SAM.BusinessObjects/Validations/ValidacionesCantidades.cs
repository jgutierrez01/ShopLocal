using SAM.BusinessObjects.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesCantidades
    {
        public static bool ValidaCantidadesCongeladoOdt(SamContext ctx, int numeroUnico, int cantidadSeleccionada)
        {
            return ctx.NumeroUnicoInventario.Where(x => x.NumeroUnicoID == numeroUnico).Select(x => x.InventarioDisponibleCruce).SingleOrDefault() < cantidadSeleccionada;
        }
    }
}
