using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SAM.Entities.Cache;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetCorteSpool
    {
        public DetCorteSpool() { }
        
        public DetCorteSpool(CorteSpool corte, Dictionary<int, string> tiposCorte)
        {
            CorteSpoolID = corte.CorteSpoolID;
            Diametro = corte.Diametro;
            ItemCodeID = corte.ItemCode.ItemCodeID;
            CodigoItemCode = corte.ItemCode.Codigo;
            DescripcionItemCode = corte.ItemCode.DescripcionEspanol;
            Cantidad = corte.Cantidad ?? 0;
            EtiquetaMaterial = corte.EtiquetaMaterial != null ? corte.EtiquetaMaterial : string.Empty;
            EtiquetaSegmento = corte.EtiquetaSeccion;
            InicioFin = corte.InicioFin;
            TipoCorte1ID = corte.TipoCorte1ID;
            TipoCorte2ID = corte.TipoCorte2ID;
            TipoCorte1 = tiposCorte[corte.TipoCorte1ID];
            TipoCorte2 = tiposCorte[corte.TipoCorte2ID];
            Observaciones = corte.Observaciones;
        }

        [DataMember]
        public int CorteSpoolID { get; set; }
        [DataMember]
        public decimal Diametro { get; set; }
        [DataMember]
        public int ItemCodeID { get; set; }
        [DataMember]
        public string CodigoItemCode { get; set; }
        [DataMember]
        public string DescripcionItemCode { get; set; }
        [DataMember]
        public int Cantidad { get; set; }
        [DataMember]
        public string EtiquetaMaterial { get; set; }
        [DataMember]
        public string EtiquetaSegmento { get; set; }
        [DataMember]
        public string InicioFin { get; set; }
        [DataMember]
        public int TipoCorte1ID { get; set; }
        [DataMember]
        public int TipoCorte2ID { get; set; }
        [DataMember]
        public string TipoCorte1 { get;set; }
        [DataMember]
        public string TipoCorte2 { get; set; }
        [DataMember]
        public string Observaciones { get; set; }
    }
}
