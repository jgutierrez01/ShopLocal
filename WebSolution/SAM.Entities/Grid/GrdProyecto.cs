using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdProyecto
    {
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public string Proyecto { get; set; }
        [DataMember]
        public string NombreClienteCompleto { get; set; }
        [DataMember]
        public string NombreColor { get; set; }
        [DataMember]
        public string PrefijoNumeroUnico { get; set; }
        [DataMember]
        public string PrefijoOdt { get; set; }
        [DataMember]
        public string Estatus { get; set; }
    }
}
