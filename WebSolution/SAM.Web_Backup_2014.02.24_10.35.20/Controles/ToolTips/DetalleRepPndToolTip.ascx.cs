using System;
using System.Linq;
using SAM.Entities;

namespace SAM.Web.Controles.ToolTips
{
    public partial class DetalleRepPndToolTip : SamToolTip
    {
        private JuntaReportePnd _juntaReportePnd;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Establece las propiedades para poder construir el control
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public override bool BindInfo(object data)
        {
            JuntaReportePnd juntaReportePnd = data as JuntaReportePnd;
            if (juntaReportePnd == null)
            {
                return false;
            }
            _juntaReportePnd = juntaReportePnd;
            
            //asignar a los controles las propiedades necesarias para construirse
            ToolTipGrid1.DatasourceCuad = _juntaReportePnd.JuntaReportePndCuadrante.ToList()
                .Select(x => new
                                 {
                                     Placa = x.Placa,
                                     Cuadrante = x.Cuadrante,
                                     Defecto = x.Defecto != null ? x.Defecto.Nombre: string.Empty
                                 });
            ToolTipGrid1.DatasourceSect = _juntaReportePnd.JuntaReportePndSector.ToList()
                .Select(x => new
                                 {
                                     De = x.SectorInicio,
                                     A = x.SectorFin,
                                     Defecto = x.Defecto.Nombre,
                                     Sector = x.Sector
                                 });
            return true;
        }
    }
}