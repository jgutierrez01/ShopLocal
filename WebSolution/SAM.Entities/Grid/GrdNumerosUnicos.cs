using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdNumerosUnicos
    {
        [DataMember]
        public int NumeroUnicoID { get; set; }

        [DataMember]
        public string NumeroUnico { get; set; }

        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public decimal? Diametro1 { get; set; }

        [DataMember]
        public decimal? Diametro2 { get; set; }

        [DataMember]
        public string Factura { get; set; }

        [DataMember]
        public string PartidaFactura { get; set; }

        [DataMember]
        public string OrdenCompra { get; set; }

        [DataMember]
        public string PartidaOrden { get; set; }

        [DataMember]
        public string NumeroColada { get; set; }

        [DataMember]
        public string Certificado { get; set; }

        [DataMember]
        public string AceroNomenclatura { get; set; }

        [DataMember]
        public int? TotalRecibida { get; set; }

        [DataMember]
        public int? TotalBuenEstado { get; set; }

        [DataMember]
        public int? TotalDanana { get; set; }

        [DataMember]
        public string Cedula { get; set; }

        [DataMember]
        public string Profile1 { get; set; }

        [DataMember]
        public string Profile2 { get; set; }

        [DataMember]
        public string Proveedor { get; set; }

        [DataMember]
        public string Fabricante { get; set; }

        [DataMember]
        public int? ColadaAceroID { get; set; }
        [DataMember]
        public int? Profile1ID { get; set; }
        [DataMember]
        public int? Profile2ID { get; set; }
        [DataMember]
        public int? ProveedorID { get; set; }
        [DataMember]
        public int? FabricanteID { get; set; }

        [DataMember]
        public string RackDisplay { get; set; }

        [DataMember]
        public string Rack { get; set; }

        [DataMember]
        public string Observaciones { get; set; }
    }
}
