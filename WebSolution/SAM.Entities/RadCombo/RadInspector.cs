using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.RadCombo
{
    [Serializable]
    public class RadInspector
    {
        [DataMember]
        public int InspectorID { get; set; }

        [DataMember]
        public int TallerID { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string ApPaterno { get; set; }

        [DataMember]
        public string ApMaterno { get; set; }

        [DataMember]
        public string NumeroEmpleado { get; set; }

        [DataMember]
        public string NombreCompleto { get; set; }
    }
}
