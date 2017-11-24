using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetalleDestajoTubero
    {
        [DataMember]
        public int DestajoTuberoDetalleID { get; set; }
        [DataMember]
        public int JuntaWorkstatusID { get; set; }
        [DataMember]
        public string TipoJunta { get; set; }
        [DataMember]
        public int TipoJuntaID { get; set; }
        [DataMember]
        public string FamiliaAcero { get; set; }
        [DataMember]
        public int FamiliaAceroID { get; set; }
        [DataMember]
        public decimal Diametro { get; set; }
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
        public string Spool { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string ComentariosArmado { get; set; }
        [DataMember]
        public string ComentariosDestajo { get; set; }
        [DataMember]
        public string EtiquetaJunta { get; set; }
        [DataMember]
        public bool CostoDestajoVacio { get; set; }
    }
}
