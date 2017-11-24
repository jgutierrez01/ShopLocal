using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdTipoReporte
    {
        [DataMember]
        public int TipoReporteProyectoID { get; set; }

        [DataMember]
        public int ProyectoReporteID { get; set; }

        [DataMember]
        public string NombreInt { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string NombreIngles { get; set; }

        [DataMember]
        public string RutaEspaniol { get; set; }

        [DataMember]
        public string RutaIngles { get; set; }
    }
}
