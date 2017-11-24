using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;

namespace SAM.BusinessObjects.Validations
{
    class ValidacionesEstimacionSpool
    {
        public static bool ExisteEstimacion(SamContext ctx, string numeroEstimacion,int proyectoID)
        {

            Estimacion est =
                    ctx.Estimacion
                        .Where(x => x.NumeroEstimacion == numeroEstimacion && x.ProyectoID == proyectoID)
                        .SingleOrDefault();

            return est == null ? true : false;
        }
    }
}
