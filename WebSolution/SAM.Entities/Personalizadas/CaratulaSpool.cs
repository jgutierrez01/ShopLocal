using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    public class CaratulaSpool
    {
        [DataMember]
        public string NombreSpool { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string Dibujo { get; set; }
        [DataMember]
        public string Revision { get; set; }
        [DataMember]
        public string NumReporteDimensional { get; set; }
        [DataMember]
        public string NumReporteEspesores { get; set; }
        [DataMember]
        public string Primario { get; set; }
        [DataMember]
        public string Enlace { get; set; }
        [DataMember]
        public string Acabado { get; set; }
        [DataMember]
        public string Adeherencias { get; set; }
        [DataMember]
        public string PullOff { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public DateTime Fecha { get; set; }
        [DataMember]
        public List<CaratulaJunta> Juntas { get; set; }
        [DataMember]
        public List<CaratulaColada> Coladas { get; set; }
    }
}
