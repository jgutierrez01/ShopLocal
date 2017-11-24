using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdCorte
    {
        [DataMember]
        public int CorteID { get; set; }

        [DataMember]
        public string NumeroUnico { get; set; }

        [DataMember]
        public string ItemCode { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public decimal Diametro1 { get; set; }

        [DataMember]
        public int CantidadCortes { get; set; }

        [DataMember]
        public bool Cancelado { get; set; }

        [DataMember]
        public string Estatus { get; set; }       

        [DataMember]
        public int Sobrante { get; set; }

        [DataMember]
        public int Merma { get; set; }
        
    }
}
