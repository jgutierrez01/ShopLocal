using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdMobileJuntasSeguimientoSpool
    {
        [DataMember]
        public int JuntaSpoolID { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public string Localizacion { get; set; }

        [DataMember]
        public string ArmadoTexto { get; set; }

        [DataMember]
        public string SoldaduraTexto { get; set; }

        [DataMember]
        public string InspeccionVisualTexto { get; set; }

        [DataMember]
        public string PNDTexto { get; set; }
        
        [DataMember]
        public string TTTexto { get; set; }

        [DataMember]
        public bool Armado { get; set; }

        [DataMember]
        public bool Soldadura { get; set; }

        [DataMember]
        public bool InspeccionVisual { get; set; }

        [DataMember]
        public bool PND { get; set; }

        [DataMember]
        public bool TT { get; set; }
    }
}
