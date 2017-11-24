using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdJuntaCampo
    {
        [DataMember]
        public int JuntaSpoolID { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string Localizacion { get; set; }

        [DataMember]
        public bool ArmadoAprobado { get; set; }
        
        [DataMember]
        public bool SoldaduraAprobada { get; set; }

        [DataMember]
        public bool InspeccionVisualAprobada { get; set; }

        [DataMember]
        public bool TieneHold { get; set; }

        [DataMember]
        public string EtiquetaIngenieria { get; set; }

        [DataMember]
        public string EtiquetaProduccion { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public int OrdenTrabajoID { get; set; }

        [DataMember]
        public string NumeroOrdenTrabajo { get; set; }

        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }

        [DataMember]
        public bool TieneRechazoPnd { get; set; }
    }
}