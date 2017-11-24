using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Entities.Busqueda
{
    [Serializable]
    public class CuadranteNumeroControlSQ
    {
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public int CuadranteID { get; set; }
        [DataMember]
        public string Cuadrante { get; set; }
        [DataMember]
        public int Accion { get; set; }
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }
        [DataMember]
        public string SqCliente { get; set; }
        [DataMember]
        public string SQ { get; set; }
        [DataMember]
        public bool TieneHoldIngenieria { get; set; }
    }

    [Serializable]
    public class CuadranteSQ
    {
        [DataMember]
        public int CuadranteID { get; set; }
        [DataMember]
        public string Cuadrante { get; set; }
    }

    [Serializable]
    public class OrdenTrabajoSpoolSQ
    {
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string SqCliente { get; set; }
        [DataMember]
        public string sqinterno { get; set; }
        [DataMember]
        public bool TieneHoldIngenieria { get; set; }
        
    }

   

}
