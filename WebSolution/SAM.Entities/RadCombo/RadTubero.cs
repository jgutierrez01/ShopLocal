﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.RadCombo
{
    [Serializable]
    public class RadTubero
    {
        [DataMember]
        public int TuberoID { get; set; }

        [DataMember]
        public string Codigo { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string ApPaterno { get; set; }

        [DataMember]
        public string ApMaterno { get; set; }

        [DataMember]
        public string NombreCompleto { get; set; }
    }
}
