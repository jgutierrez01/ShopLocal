using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class JuntasCampoDTO
    {
        [DataMember]
        public int ProyectoID { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public string Junta { get; set; }

        [DataMember]
        public int JuntaCampoID { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string EtiquetaMaterial1 { get; set; }

        [DataMember]
        public string EtiquetaMaterial2 { get; set; }

        [DataMember]
        public string TipoJunta { get; set; }

        [DataMember]
        public decimal Espesor { get; set; }

        [DataMember]
        public string EtiquetaIngenieria { get; set; }
    }
}
