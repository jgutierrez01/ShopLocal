using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdEstimacionJunta
    {
        [DataMember]
        public int EstimacionID { get; set; }

        [DataMember]
        public int EstimadoJuntaID { get; set; }

        [DataMember]
        public string Concepto { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public string TipoJunta { get; set; }

        [DataMember]
        public decimal? Diametro { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string NombreSpool { get; set; }

        [DataMember]
        public string Material { get; set; }

        [DataMember]
        public string Cedula { get; set; }
    }
}
