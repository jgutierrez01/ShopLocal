using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdItemCode
    {
        [DataMember]
        public int ItemCodeID { get; set; }
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string Descripcion { get; set; }
        [DataMember]
        public string DescripcionIngles { get; set; }
        [DataMember]
        public int TipoMaterialID { get; set; }
        [DataMember]
        public string TipoMaterial { get; set; }
        [DataMember]
        public decimal Peso { get; set; }
        [DataMember]
        public string DescripcionInterna { get; set; }
        [DataMember]
        public decimal? Diametro1 { get; set; }
        [DataMember]
        public decimal? Diametro2 { get; set; }
        public FamiliaAcero FamiliaAcero { get; set; }
    }
}
