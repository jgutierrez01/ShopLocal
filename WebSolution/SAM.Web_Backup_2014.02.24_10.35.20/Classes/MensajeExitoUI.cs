using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SAM.Web.Classes
{
    [Serializable]
    public class MensajeExitoUI
    {
        [DataMember]
        public string Titulo { get; set; }

        [DataMember]
        public string CuerpoMensaje { get; set; }

        [DataMember]
        public List<LigaMensaje> Ligas { get; set; }
    }
}