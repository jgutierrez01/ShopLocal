using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;

namespace SAM.Web.Controles.SpoolOdt
{
    /// <summary>
    /// Control que en modo sólo lectura despliega la información de holds de un spool
    /// </summary>
    public partial class HoldOdtRO : UserControl, IMappable
    {
        /// <summary>
        /// Mappea a los controles de la página de manera recursiva (los checkboxes)
        /// y luego manualmente al repeater con el historial de holds.
        /// </summary>
        /// <param name="entity">Debe ser una instancia de DetSpoolOdt</param>
        public void Map(object entity)
        {
            Controls.IterateRecursivelyStopOnIMappableAndUserControl(c =>
            {
                if (c is IMappableField)
                {
                    ((IMappableField)c).Map(entity);
                }
            });

            List<DetHoldSpool> holds = ((DetSpoolOdt)entity).Holds;

            if (holds != null && holds.Count > 0)
            {
                repHolds.DataSource = holds.OrderBy(x => x.FechaHold);
                repHolds.DataBind();
            }
        }

        public void Unmap(object entity)
        {
            //no se necesita
        }
    }
}