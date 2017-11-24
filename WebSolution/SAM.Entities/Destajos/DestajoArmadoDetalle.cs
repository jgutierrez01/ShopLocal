using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Destajos
{
    [Serializable]
    public class DestajoArmadoDetalle
    {
        [DataMember]
        public int JuntaWorkstatusID { get; set; }
        [DataMember]
        public int TuberoID { get; set; }
        [DataMember]
        public int TipoJuntaID { get; set; }
        [DataMember]
        public int FamiliaAceroID { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public decimal Diametro { get; set; }
        [DataMember]
        public bool EsDePeriodoAnterior { get; set; }
        [DataMember]
        public decimal Ajuste { get; set; }
        [DataMember]
        public decimal CostoUnitario { get; set; }
        [DataMember]
        public decimal Destajo { get; set; }
        [DataMember]
        public decimal ProrrateoDiasFestivos { get; set; }
        [DataMember]
        public decimal ProrrateoOtros { get; set; }
        [DataMember]
        public decimal ProrrateoCuadro { get; set; }
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public int IDPadre { get; set; }
        [DataMember]
        public bool CostoDestajoVacio { get; set; }
    }
}
