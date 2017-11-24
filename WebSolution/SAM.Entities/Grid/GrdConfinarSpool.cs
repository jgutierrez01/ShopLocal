using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdConfinarSpool
    {
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public int? Prioridad { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string RevisionCliente { get; set; }
        [DataMember]
        public decimal? Pdis { get; set; }
        [DataMember]
        public decimal? Peso { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public decimal? Area { get; set; }      
        [DataMember]
        public string FamiliaAcero1 { get; set; }
        [DataMember]
        public string FamiliaAcero2 { get; set; }
        [DataMember]
        public string FamiliasAcero
        {
            get
            {
                if (!string.IsNullOrEmpty(FamiliaAcero2))
                {
                    return string.Concat(FamiliaAcero1, "/", FamiliaAcero2);
                }
                else
                {
                    return FamiliaAcero1;
                }
            }
        }
        [DataMember]
        public int? PorcentajePnd { get; set; }
        [DataMember]
        public bool RequierePwht { get; set; }
        [DataMember]
        public string DibujoReferencia { get; set; }
        [DataMember]
        public string Especificacion { get; set; }
        [DataMember]
        public bool Confinado { get; set; }
        [DataMember]
        public int? OrdenTrabajoSpoolID { get; set; }
        [DataMember]
        public int? OrdenTrabajoID { get; set; }
    }
}
