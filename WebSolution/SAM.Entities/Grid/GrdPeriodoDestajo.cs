using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdPeriodoDestajo
    {
        [DataMember]
        public int PeriodoDestajoID { get; set; }
        [DataMember]
        public string Semana { get; set; }
        [DataMember]
        public int Anio { get; set; }
        [DataMember]
        public DateTime FechaInicio { get; set; }
        [DataMember]
        public DateTime FechaFin { get; set; }
        [DataMember]
        public int CantidadDiasFestivos { get; set; }
        [DataMember]
        public bool Cerrado { get; set; }
        [DataMember]
        public decimal TotalAPagar { get; set; }
        [DataMember]
        public int CantidadTuberos { get; set; }
        [DataMember]
        public int CantidadSoldadores { get; set; }
        [DataMember]
        public string EstatusTexto { get; set; }
        [DataMember]
        public EstatusPeriodoDestajo Estatus { get; set; }
        [DataMember]
        public int DestajosSoldadorAprobados { get; set; }
        [DataMember]
        public int DestajosTuberoAprobados { get; set; }
    }
}
