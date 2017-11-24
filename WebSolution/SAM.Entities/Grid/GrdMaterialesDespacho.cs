using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdMaterialesDespacho
    {
        [DataMember]
        public int MaterialSpoolID { get; set; }
        [DataMember]
        public int OrdenTrabajoMaterialID { get; set; }
        [DataMember]
        public string EtiquetaMaterial { get; set; }
        [DataMember]
        public int ItemCodeID { get; set; }
        [DataMember]
        public string CodigoItemCode { get; set; }
        [DataMember]
        public string DescripcionItemCode { get; set; }
        [DataMember]
        public decimal Diametro1 { get; set; }
        [DataMember]
        public decimal Diametro2 { get; set; }
        [DataMember]
        public int CantidadRequerida { get; set; }
        [DataMember]
        public bool TieneDespacho { get; set; }
        [DataMember]
        public bool TieneCorte { get; set; }
        [DataMember]
        public bool TieneInventarioCongelado { get; set; }
        [DataMember]
        public bool PerteneceAOdt { get; set; }
        [DataMember]
        public bool EsTubo { get; set; }
        [DataMember]
        public EstatusMaterialDespacho Estatus { get; set; }
        [DataMember]
        public string EstatusTexto { get; set; }
        [DataMember]
        public bool TieneHold { get; set; }
    }
}