using System;
using SAM.Entities.Grid;

namespace SAM.Web.Controles.Calidad.SegJunta
{
    public partial class PruebaRT : ControlSegJunta
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected internal override void Map(GrdSeguimientoJunta detalle)
        {
            repsPnd.DatasourceCuad = detalle.PruebaRTPndCuad;
            repsPnd.DatasourceSect = detalle.PruebaRTPndSector;
        }
    }
}