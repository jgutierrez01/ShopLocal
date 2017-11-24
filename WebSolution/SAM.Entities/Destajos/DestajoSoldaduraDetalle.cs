using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Destajos
{
    [Serializable]
    public class DestajoSoldaduraDetalle
    {
        [DataMember]
        public int JuntaWorkstatusID { get; set; }
        [DataMember]
        public int SoldadorID { get; set; }
        [DataMember]
        public int TecnicaSoldadorID { get; set; }
        [DataMember]
        public int ProcesoRaizID { get; set; }
        [DataMember]
        public int ProcesoRellenoID { get; set; }
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
        public int ID { get; set; }
        [DataMember]
        public int IDPadre { get; set; }
        [DataMember]
        public bool CostoRaizVacio { get; set; }
        [DataMember]
        public bool CostoRellenoVacio { get; set; }
    }
}
