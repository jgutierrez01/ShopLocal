using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdCruce
    {
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public int Prioridad { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Dibujo { get; set; }
        [DataMember]
        public decimal Pdis { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public int FamiliaAcero1ID { get; set; }
        [DataMember]
        public int FamiliaAcero2ID { get; set; }
        [DataMember]
        public decimal Peso { get; set; }
        [DataMember]
        public decimal Area { get; set; }
        [DataMember]
        public string FamiliaAcero1 { get; set; }
        [DataMember]
        public string FamiliaAcero2 { get; set; }
        [DataMember]
        public int Juntas { get; set; }
        [DataMember]
        public string FamiliasAcero
        {
            get
            {
                if ( !string.IsNullOrEmpty(FamiliaAcero2))
                {
                    return string.Concat(FamiliaAcero1, "/", FamiliaAcero2);
                }
                else
                {
                    return FamiliaAcero1;
                }
            }
        }
        [DataMember]
        public decimal TotalPeqs { get; set; }
        [DataMember]
        public int TotalTubo { get; set; }
        [DataMember]
        public int TotalAccesorio { get; set; }
        [DataMember]
        public int LongitudTubo { get; set; }
        [DataMember]
        public decimal? DiametroPlano { get; set; }
        [DataMember]
        public bool Hold { get; set; }
        [DataMember]
        public string ObservacionesHold { get; set; }
    }
}
