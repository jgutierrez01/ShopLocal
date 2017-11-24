using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    public class GrdNumerosUnicosEnCorte
    {
        [DataMember]
        public int NumeroUnicoSegmentoID { get; set; }

        [DataMember]
        public DateTime FechaTraspaso { get; set; }
        
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
        public string Cedula { get; set; }

        [DataMember]
        public string NumeroColada { get; set; }

        [DataMember]
        public int Cantidad { get; set; }

        [DataMember]
        public string Segmento { get; set; }

        [DataMember]
        public string OrdenDeTrabajo { get; set; }



    }
}
