using System;
using SAM.Entities.Grid;

namespace SAM.Web.Controles.Calidad.SegJunta
{
    public partial class PruebaPT : ControlSegJunta
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected internal override void Map(GrdSeguimientoJunta detalle)
        {
            repsPnd.DatasourceSect = detalle.PruebaPTPndSector;
            repsPnd.DatasourceCuad = detalle.PruebaPTPndCuad;
        }
    }
}