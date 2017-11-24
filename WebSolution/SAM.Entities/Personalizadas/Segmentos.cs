﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class Segmentos
    {
        [DataMember]
        public int NumeroUnicoID { get; set; }
        [DataMember]
        public string Segmento { get; set; }
        [DataMember]
        public string Rack { get; set; }
    }
}
