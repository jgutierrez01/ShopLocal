using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Text; 
 using System.Threading.Tasks; 
 using System.Runtime.Serialization;


namespace SAM.Entities.Personalizadas.Shop
{
    [Serializable]
    public class DetailJoint
    {
        [DataMember]
        public string Joint { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public string TypeJoint { get; set; }
        [DataMember]
        public decimal Diameter { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public DateTime? DateFitting { get; set; }
        [DataMember]
        public DateTime? DateWelding { get; set; }
        [DataMember]
        public string WPS { get; set; }
        [DataMember]
        public string WelderRoot { get; set; }
        [DataMember]
        public string WelderFiller { get; set; }
        [DataMember]
        public DateTime? DateVisualInspection { get; set; }
        [DataMember]
        public string ResultVisualInspection { get; set; }
        [DataMember]
        public string UniqueNumberOne { get; set; }
        [DataMember]
        public string UniqueNumberTwo { get; set; }
        [DataMember]
        public string RT { get; set; }
        [DataMember]
        public string PT { get; set; }
        [DataMember]
        public string RequiredTestNeumatic { get; set; }
        [DataMember]
        public string TestNeumatic { get; set; }
        [DataMember]
        public string RequiredPWHT { get; set; }
        [DataMember]
        public string PWHT { get; set; }
        [DataMember]
        public string PMI { get; set; }
        [DataMember]
        public string Hardness { get; set; }
        [DataMember]
        public string Fitting { get; set; }
        [DataMember]
        public string Welding { get; set; }
        [DataMember]
        public int? JuntaWorkstatusId { get; set; }
    }
}
