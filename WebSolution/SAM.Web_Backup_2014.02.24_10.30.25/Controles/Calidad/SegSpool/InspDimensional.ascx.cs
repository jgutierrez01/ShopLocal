using System.Linq;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities.Personalizadas;

namespace SAM.Web.Controles.Calidad.SegSpool
{
    public partial class InspDimensional : ControlSegSpool
    {
        protected internal override void Map(DetGrdSeguimientoSpool detalle)
        {
            if (detalle.ReportesDimensionales != null && detalle.ReportesDimensionales.Count > 0)
            {
                DetSegSpoolRep repEspesor = detalle.ReportesDimensionales.First();
                InspeccionDimensionalHoja.Text = repEspesor.Hoja == null ? string.Empty : repEspesor.Hoja.ToString();
                InspeccionDimensionalFecha.Text = repEspesor.Fecha.SafeDateAsStringParse();
                InspeccionDimensionalFechaReporte.Text = repEspesor.Fecha.SafeDateAsStringParse();
                InspeccionDimensionalNumeroReporte.Text = repEspesor.NumeroReporte;
                InspeccionDimensionalResultado.Text = repEspesor.Resultado;
                InspeccionDimensionalObservaciones.Text = repEspesor.Observaciones;
                repSector.DataSource = detalle.ReportesDimensionales.OrderBy(x => x.Fecha);
                repSector.DataBind();
                repSector.Visible = true;
            }
        }
    }
}