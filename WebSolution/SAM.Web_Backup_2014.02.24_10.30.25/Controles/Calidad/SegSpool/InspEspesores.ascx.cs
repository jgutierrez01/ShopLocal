using System.Linq;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using SAM.Entities.Personalizadas;

namespace SAM.Web.Controles.Calidad.SegSpool
{
    public partial class InspEspesores : ControlSegSpool
    {
        protected internal override void Map(DetGrdSeguimientoSpool detalle)
        {
            if (detalle.ReportesEspesores != null && detalle.ReportesEspesores.Count > 0)
            {
                DetSegSpoolRep repEspesor = detalle.ReportesEspesores.First();
                InspeccionEspesoresHoja.Text = repEspesor.Hoja == null ? string.Empty : repEspesor.Hoja.ToString();
                InspeccionEspesoresFecha.Text = repEspesor.Fecha.SafeDateAsStringParse();
                InspeccionEspesoresFechaReporte.Text = repEspesor.Fecha.SafeDateAsStringParse();
                InspeccionEspesoresNumeroReporte.Text = repEspesor.NumeroReporte;
                InspeccionEspesoresResultado.Text = repEspesor.Resultado;
                InspeccionEspesoresObservaciones.Text = repEspesor.Observaciones;
                repSector.DataSource = detalle.ReportesEspesores.OrderBy(x=> x.Fecha);
                repSector.DataBind();
            }
        }
    }
}