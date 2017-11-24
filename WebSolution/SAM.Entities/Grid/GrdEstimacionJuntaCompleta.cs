using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdEstimacionJuntaCompleta
    {
        [DataMember]
        public int? EstimacionId { get; set; }

        [DataMember]
        public int JuntaWorkStatusID { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string NombreSpool { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public string TipoDeJunta { get; set; }

        [DataMember]
        public decimal? Diametro { get; set; }

        [DataMember]
        public string Material { get; set; }

        [DataMember]
        public string Cedula { get; set; }

        [DataMember]
        public bool Armada { get; set; }

        [DataMember]
        public bool Soldada { get; set; }

        [DataMember]
        public bool InspeccionVisual { get; set; }

        [DataMember]
        public bool InspeccionDimensional { get; set; }

        [DataMember]
        public bool RtAprobado { get; set; }

        [DataMember]
        public bool PtAprobado { get; set; }

        [DataMember]
        public bool PwhtAprobado { get; set; }

        [DataMember]
        public bool DurezasAprobado { get; set; }

        [DataMember]
        public bool RtPostTtAprobado { get; set; }

        [DataMember]
        public bool PtPostTtAprobado { get; set; }

        [DataMember]
        public bool PreHeatAprobado { get; set; }

        [DataMember]
        public bool UtAprobado { get; set; }

        [DataMember]
        public int? ConceptoEstimacionID { get; set; }

        [DataMember]
        public string NumeroEstimacion { get; set; }
    }
}
