using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetSpoolMobile
    {
        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public int ProyectoID { get; set; }

        [DataMember]
        public int PatioID { get; set; }

        [DataMember]
        public string Proyecto { get; set; }
    }
}
