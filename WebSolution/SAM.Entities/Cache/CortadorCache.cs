using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class CortadorCache : EntidadBase
    {

        [DataMember]
        public int TallerID { get; set; }

        [DataMember]
        public int PatioID { get; set; }


    }
}
