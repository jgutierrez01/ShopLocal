using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetalleDestajoSoldador
    {
        [DataMember]
        public int DestajoSoldadorDetalleID { get; set; }
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
        public bool EsDePeriodoAnterior { get; set; }
        [DataMember]
        public decimal CostoUnitarioRaiz { get; set; }
        [DataMember]
        public decimal CostoUnitarioRelleno { get; set; }
        [DataMember]
        public bool RaizDividida { get; set; }
        [DataMember]
        public bool RellenoDividido { get; set; }
        [DataMember]
        public int NumeroFondeadores { get; set; }
        [DataMember]
        public int NumeroRellenadores { get; set; }
        [DataMember]
        public decimal DestajoRaiz { get; set; }
        [DataMember]
        public decimal DestajoRelleno { get; set; }
        [DataMember]
        public decimal ProrrateoCuadro { get; set; }
        [DataMember]
        public decimal ProrrateoDiasFestivos { get; set; }
        [DataMember]
        public decimal ProrrateoOtros { get; set; }
        [DataMember]
        public decimal Ajuste { get; set; }
        [DataMember]
        public decimal Total { get; set; }
        [DataMember]
        public string Spool { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string ComentariosSoldadura { get; set; }
        [DataMember]
        public string ComentariosDestajo { get; set; }
        [DataMember]
        public string EtiquetaJunta { get; set; }
        [DataMember]
        public bool CostoRellenoVacio { get; set; }
        [DataMember]
        public bool CostoRaizVacio { get; set; }
    }
}
