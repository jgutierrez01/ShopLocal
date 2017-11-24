using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public struct ContactoStruct
    {
        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string ApPaterno { get; set; }

        [DataMember]
        public string ApMaterno { get; set; }

        [DataMember]
        public string CorreoElectronico { get; set; }

        [DataMember]
        public string TelefonoOficina { get; set; }

        [DataMember]
        public string TelefonoParticular { get; set; }

        [DataMember]
        public string TelefonoCelular { get; set; }
    }
}