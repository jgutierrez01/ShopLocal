using System.Linq;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    public static class ValidacionesEstimacionJunta
    {
        public static bool ExisteEstimacion(SamContext ctx, string numeroEstimacion, int proyectoID)
        {

            Estimacion est =
                    ctx.Estimacion
                        .Where(x => x.NumeroEstimacion == numeroEstimacion && x.ProyectoID == proyectoID)
                        .SingleOrDefault();

            return est == null ? true : false;
        }
    }
}
               
            
