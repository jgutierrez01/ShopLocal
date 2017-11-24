using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class JuntaCampoArmadoInfo
    {
        [DataMember]
        public int JuntaCampoArmadoID { get; set; }
        [DataMember]
        public int JuntaCampoID { get; set; }
        [DataMember]
        public int JuntaSpoolID { get; set; }
        [DataMember]
        public string Spool1 { get; set; }
        [DataMember]
        public string Spool2 { get; set; }
        [DataMember]
        public string EtiquetaMaterial1 { get; set; }
        [DataMember]
        public string EtiquetaMaterial2 { get; set; }
        [DataMember]
        public string NumeroUnico1 { get; set; }
        [DataMember]
        public string NumeroUnico2 { get; set; }
        [DataMember]
        public string CodigoTubero { get; set; }
        [DataMember]
        public DateTime FechaArmado { get; set; }
        [DataMember]
        public DateTime FechaReporte { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
        [DataMember]
        public int Spool1ID { get; set; }
        [DataMember]
        public int Spool2ID { get; set; }
        [DataMember]
        public int TuberoID { get; set; }
        [DataMember]
        public int NumeroUnico1ID { get; set; }
        [DataMember]
        public int NumeroUnico2ID { get; set; }
        [DataMember]
        public string EtiquetaJunta { get; set; }
    }
}
