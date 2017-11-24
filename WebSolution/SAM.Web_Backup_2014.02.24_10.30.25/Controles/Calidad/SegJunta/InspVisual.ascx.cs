using Mimo.Framework.Extensions;
using SAM.Entities.Grid;

namespace SAM.Web.Controles.Calidad.SegJunta
{
    public partial class InspVisual : ControlSegJunta
    {
        
        protected internal override void Map(GrdSeguimientoJunta detalle)
        {
            pnlDefectos.Visible = false;
            if (!string.IsNullOrEmpty(detalle.InspeccionVisualDefecto))
            {
                pnlDefectos.Visible = detalle.InspeccionVisualResultado.ToLower().Contains("no");
                //en inspección Visual defecto vienen los defectos separados por coma
                repDefecto.DataSource = detalle.InspeccionVisualDefecto.Split(',');
                repDefecto.DataBind();
                
            }
        }
    }
}