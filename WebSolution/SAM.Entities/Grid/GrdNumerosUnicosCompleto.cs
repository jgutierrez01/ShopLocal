using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdNumerosUnicosCompleto
    {
        [DataMember]
        public DateTime FechaRecepcion { get; set; }

        [DataMember]
        public int NumeroUnicoID { get; set; }

        [DataMember]
        public string NumeroUnico { get; set; }

        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public string TipoMaterial { get; set; }

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
        public string Transportista { get; set; }

        [DataMember]
        public decimal? TotalRecibida { get; set; }

        [DataMember]
        public decimal? TotalInventarioFisico { get; set; }

        [DataMember]
        public decimal? TotalOtrasEntradas { get; set; }

        [DataMember]
        public decimal? TotalDanada { get; set; }

        [DataMember]
        public decimal? TotalCondicionada { get; set; }

        [DataMember]
        public decimal? TotalRechazada { get; set; }

        [DataMember]
        public decimal? TotalDespachada { get; set; }

        [DataMember]
        public decimal? TotalCorteSinDespachada { get; set; }

        [DataMember]
        public decimal? TotalMerma { get; set; }

        [DataMember]
        public decimal? TotalEnTransferencia { get; set; }

        [DataMember]
        public decimal? InventarioCongelado { get; set; }

        [DataMember]
        public decimal? InventarioDisponibleCruce { get; set; }
        
        [DataMember]
        public decimal? TotalInventarioActual { get; set; }
        
        [DataMember]
        public bool MarcadoAsme { get; set; }

        [DataMember]
        public bool MarcadoGolpe { get; set; }

        [DataMember]
        public bool MarcadoPintura { get; set; }

        [DataMember]
        public string PruebasHidrostaticas { get; set; }

        [DataMember]
        public string Estatus { get; set; }

        [DataMember]
        public decimal? TotalOtrasSalidas { get; set; }

        [DataMember]
        public decimal? TotalRecibidoNeto { get; set; }

        [DataMember]
        public decimal? TotalSalidasTemporales { get; set; }

        [DataMember]
        public decimal? TotalDespachadaICE { get; set; }

        [DataMember]
        public string RackDisplay { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        [DataMember]
        public string CampoLibre1 { get; set; }

        [DataMember]
        public string CampoLibre2 { get; set; }

        [DataMember]
        public string CampoLibre3 { get; set; }

        [DataMember]
        public string CampoLibre4 { get; set; }

        [DataMember]
        public string CampoLibre5 { get; set; }

        [DataMember]
        public string CampoLibre6 { get; set; }

        [DataMember]
        public string CampoLibre7 { get; set; }

        [DataMember]
        public string CampoLibre8 { get; set; }

        [DataMember]
        public string CampoLibre9 { get; set; }

        [DataMember]
        public string CampoLibre10 { get; set; }
    }
}
