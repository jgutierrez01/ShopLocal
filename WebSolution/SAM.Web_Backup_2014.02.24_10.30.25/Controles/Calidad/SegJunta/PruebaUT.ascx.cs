using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.Entities.Grid;

namespace SAM.Web.Controles.Calidad.SegJunta
{
    public partial class PruebaUT : ControlSegJunta
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected internal override void Map(GrdSeguimientoJunta detalle)
        {
            repsPnd.DatasourceCuad = detalle.PruebaUTPndCuad;
            repsPnd.DatasourceSect = detalle.PruebaUTPndSector;
        }
    }
}