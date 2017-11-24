using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdEmbarque
    {
        [DataMember]
        public int WorkstatusSpoolID { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public string OrdenTrabajo { get; set; }

        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string NombreSpool { get; set; }

        [DataMember]
        public decimal Area { get; set; }

        [DataMember]
        public decimal PDI { get; set; }

        [DataMember]
        public decimal Peso { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public string VigenciaAduana { get; set; }

        [DataMember]
        public bool Preparado { get; set; }

        [DataMember]
        public string FolioPreparacion { get; set; }
                
        [DataMember]
        public string Embarque { get; set; }

        [DataMember]
        public string FechaEmbarque { get; set; }

        [DataMember]
        public string FechaEstimada { get; set; }

        [DataMember]
        public bool Hold { get; set; }
    }
}
