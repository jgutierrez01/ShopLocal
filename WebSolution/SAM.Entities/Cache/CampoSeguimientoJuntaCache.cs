using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class CampoSeguimientoJuntaCache : EntidadBase
    {
        [DataMember]
        public int CampoSeguimientoJuntaID { get; set; }

        [DataMember]
        public int ModuloSeguimientoJuntaID { get; set; }
        
        [DataMember]
        public string NombreColumnaSp { get; set; }

        [DataMember]
        public string DataFormat { get; set; }

        [DataMember]
        public string NombreControlUI { get; set; }

        [DataMember]
        public string CssColumnaUI{ get; set; }

        [DataMember]
        public byte OrdenUI { get; set; }

        [DataMember]
        public int? AnchoUI { get; set; }

        [DataMember]
        public string TipoDeDato { get; set; }
    }
}
