using System;
using System.Runtime.Serialization;

namespace SAM.Entities.Busqueda
{
    [Serializable]
    public class NumeroControlBusqueda
    {
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public int ProyectoID { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string Cuadrante { get; set; }

        [DataMember]
        public string FamiliaAcero { get; set; }

        [DataMember]
        public int CuadranteId { get; set; }

        [DataMember]
        public decimal? DiametroMaximo { get; set; }

        //solo para version Shop
        public TipoNumeroControlEnum TipoNC { get; set; }
        public int? TieneWorkstatusSpool { get; set; }
    }
}
