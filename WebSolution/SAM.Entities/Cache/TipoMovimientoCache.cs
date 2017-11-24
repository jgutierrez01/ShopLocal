using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class TipoMovimientoCache:EntidadBase
    {
        [DataMember]
        public bool EsEntrada { get; set; }

        [DataMember]
        public bool EsTransferenciaProcesos { get; set; }
        
        [DataMember]
        public bool ApareceEnSaldos { get; set; }

        [DataMember]
        public bool DisponibleMovimientosUI { get; set; }

    }
}
