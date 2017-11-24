using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Reportes
{
    [Serializable]
    public class CondensadoItemCode
    {
        [DataMember]
        public int ItemCodeID { get; set; }
        [DataMember]
        public string CodigoItemCode { get; set; }
        [DataMember]
        public string DescripcionItemCode { get; set; }
        [DataMember]
        public decimal D1 { get; set; }
        [DataMember]
        public decimal D2 { get; set; }
        [DataMember]
        public int? DisponibleCruceOriginal { get; set; }
        [DataMember]
        public int? CantidadRecibidaOriginal { get; set; }
        [DataMember]
        public int? CantidadCondicionadosOriginal { get; set; }
        [DataMember]
        public int? CantidadRechazadosOriginal { get; set; }
        [DataMember]
        public int? CongeladoEnEsteCruceOriginal { get; set; }
        [DataMember]
        public int? CongeladoEnEsteCruceEquivalencia { get; set; }
        [DataMember]
        public int? DisponiblePorEquivalencia { get; set; }
        [DataMember]
        public int? RequeridaParaFabricacion { get; set; }
        [DataMember]
        public int Prioridad { get; set; }
        [DataMember]
        public int SumaPrioridad { get; set; }
        [DataMember]
        public int RequeridaTotalIngenieria { get; set; }
    }
}
