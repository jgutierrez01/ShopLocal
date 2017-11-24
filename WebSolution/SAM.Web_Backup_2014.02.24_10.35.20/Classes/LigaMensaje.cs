using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SAM.Web.Classes
{
    [Serializable]
    public class LigaMensaje
    {
        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string Texto { get; set; }
    }
}