using SAM.Entities.Grid;

namespace SAM.Web.Controles.Calidad.SegJunta
{
    public partial class Soldadura : ControlSegJunta
    {
        
        protected internal override void Map(GrdSeguimientoJunta detalle)
        {
            repSoldadura.DataSource = detalle.SoldaduraDetalle;
            repSoldadura.DataBind();
        }

    }
}