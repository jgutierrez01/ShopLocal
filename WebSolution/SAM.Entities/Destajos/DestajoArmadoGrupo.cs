using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SAM.Entities.Destajos
{
    [Serializable]
    [XmlRoot("armado")]
    public class DestajoArmadoGrupo
    {
        public DestajoArmadoGrupo()
        {
            Detalle = new List<DestajoArmadoDetalle>();
        }

        [DataMember]
        public bool Aprobado { get; set; }
        [DataMember]
        public int CantidadDiasFestivos { get; set; }
        [DataMember]
        public decimal CostoDiaFestivo { get; set; }
        [DataMember]
        public decimal ReferenciaCuadro { get; set; }
        [DataMember]
        public decimal TotalCuadro { get; set; }
        [DataMember]
        public decimal GranTotal { get; set; }
        [DataMember]
        public decimal TotalDestajo { get; set; }
        [DataMember]
        public decimal TotalDiasFestivos { get; set; }
        [DataMember]
        public decimal TotalOtros { get; set; }
        [DataMember]
        public decimal TotalAjuste { get; set; }
        [DataMember]
        public int TuberoID { get; set; }
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        [XmlArray("juntas")]
        [XmlArrayItem("junta")]
        public List<DestajoArmadoDetalle> Detalle { get; set; }

        public void AgregaDetalle(DestajoArmadoDetalle detalle)
        {
            Detalle.Add(detalle);
        }
    }
}
