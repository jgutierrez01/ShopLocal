using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdRequisiciones
    {
        [DataMember]
        public int JuntaWorkstatusID { get; set; }

        [DataMember]
        public int JuntaSpoolID { get; set; }

        [DataMember]
        public int RequisicionID { get; set; }

        [DataMember]
        public string NumeroRequisicion { get; set; }

        [DataMember]
        public DateTime FechaRequisicion { get; set; }

        [DataMember]
        public string OrdenTrabajo { get; set; }

        [DataMember]
        public string NumeroControl { get; set; }

        [DataMember]
        public string EtiquetaJunta { get; set; }

        [DataMember]
        public string EtiquetaMaterial1 { get; set; }

        [DataMember]
        public string EtiquetaMaterial2 { get; set; }

        [DataMember]
        public string Localizacion
        {
            get
            {
                return EtiquetaMaterial1 + "-" + EtiquetaMaterial2;
            }
        }

        [DataMember]
        public string TipoJunta { get; set; }

        [DataMember]
        public int TipoJuntaID { get; set; }

        [DataMember]
        public string Cedula { get; set; }

        [DataMember]
        public string FamiliaAceroMaterial1 { get; set; }

        [DataMember]
        public string FamiliaAceroMaterial2 { get; set; }

        [DataMember]
        public decimal Diametro { get; set; }
       
        [DataMember]
        public bool Hold { get; set; }

        [DataMember]
        public string NombreSpool { get; set; }
    }
}
