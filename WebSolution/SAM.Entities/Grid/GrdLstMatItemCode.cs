using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdLstMatItemCode
    {

        [DataMember]
        public int ItemCodeID { get; set; }

        [DataMember]
        public string CodigoItemCode { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public string TipoMaterial { get; set; }
        
        [DataMember]
        public decimal? Diametro1 { get; set; }

        [DataMember]
        public decimal? Diametro2 { get; set; }

        [DataMember]
        public string Estatus { get; set; }

        [DataMember]
        public int? TotalInventarioFisico { get; set; }

        [DataMember]
        public int? TotalRecibida { get; set; }

        [DataMember]
        public int? TotalCondicionada { get; set; }

        [DataMember]
        public int? TotalRechazada { get; set; }
        
        [DataMember]
        public int? TotalInventarioActual { get; set; }

        [DataMember]
        public int? InventarioCongelado { get; set; }

        [DataMember]
        public int? InventarioDisponibleCruce { get; set; } 

        [DataMember]
        public int? TotalDanada { get; set; }

        [DataMember]
        public int? TotalDespachada { get; set; }

        [DataMember]
        public int? TotalMerma { get; set; }

        [DataMember]
        public int? TotalOtrasSalidas { get; set; }

        [DataMember]
        public int? TotalOrdenTrabajo { get; set; }

        [DataMember]
        public int? TotalDespachadaEquivalente { get; set; }

        [DataMember]
        public int? InventarioCongeladoEquivalente { get; set; }
        
        [DataMember]
        public int? InventarioDisponibleEquivalente { get; set; }

        [DataMember]
        public int? InventarioBuenEstadoEquivalente { get; set; }
    }
}
