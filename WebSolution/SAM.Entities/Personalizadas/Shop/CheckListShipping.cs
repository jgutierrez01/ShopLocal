using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Text; 
 using System.Threading.Tasks; 
 using System.Runtime.Serialization;


namespace SAM.Entities.Personalizadas.Shop
{
    [Serializable]
    public class CheckListShipping
    {
        [DataMember]
        public int? SpoolId { get; set; }

        [DataMember]
        public string Hold { get; set; }

        [DataMember]
        public bool? Materials { get; set; }

        [DataMember]
        public bool? Dispatch { get; set; }

        [DataMember]
        public bool? Fitting { get; set; }

        [DataMember]
        public int? Welding { get; set; }

        [DataMember]
        public bool? VisualInspection { get; set; }

        [DataMember]
        public bool? DimensionalInspection { get; set; }

        [DataMember]
        public int? PWHT { get; set; }

        [DataMember]
        public int? PND { get; set; }

        [DataMember]
        public int? Paint { get; set; }

        [DataMember]
        public string SistemaPintura { get; set; }

        [DataMember]
        public bool? OKMIL { get; set; }

        [DataMember]
        public bool? OkPaint { get; set; }

        [DataMember]
        public bool? OkQuality { get; set; }

        [DataMember]
        public bool? PreparationShipping { get; set; }
    }
}