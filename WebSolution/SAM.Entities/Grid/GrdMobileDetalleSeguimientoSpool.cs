using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdMobileDetalleSeguimientoSpool
    {
        [DataMember]
        public int JuntaSpoolID { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public string Tipo { get; set; }

        [DataMember]
        public bool InspeccionVisual { get; set; }

        [DataMember]
        public string InspeccionVisualTexto { get; set; }
        
        [DataMember]
        public bool RT { get; set; }
        
        [DataMember]
        public string RTTexto { get; set; }

        [DataMember]
        public bool PT { get; set; }

        [DataMember]
        public string PTTexto { get; set; }

        [DataMember]
        public bool PWHT { get; set; }

        [DataMember]
        public string PWHTTexto { get; set; }
    }
}
