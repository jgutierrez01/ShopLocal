using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdSoldadura
    {
        [DataMember]
        public int ProyectoID { get; set; }
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public int OrdenTrabajoID { get; set; }
        [DataMember]
        public int OrdenTrabajoSpoolID { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string Junta { get; set; }
        [DataMember]
        public string Localizacion { get; set; }
        [DataMember]
        public string EtiquetaMaterial1 { get; set; }
        [DataMember]
        public string EtiquetaMaterial2 { get; set; }
        [DataMember]
        public string TipoJunta { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public string FamiliaAceroMaterial
        {
            get
            {
                return FamiliaAceroMaterial1 + (FamiliaAceroMaterial2 != string.Empty ? "/" + FamiliaAceroMaterial2 : string.Empty);
            }
        }
        [DataMember]
        public string FamiliaAceroMaterial1 { get; set; }
        [DataMember]
        public string FamiliaAceroMaterial2 { get; set; }
        [DataMember]
        public decimal Diametro { get; set; }
        [DataMember]
        public int JuntaSpoolID { get; set; }
        [DataMember]
        public int JuntaSoldaduraID { get; set; }
        [DataMember]
        public bool ArmadoAprobado { get; set; }
        [DataMember]
        public bool SoldaduraAprobada { get; set; }
        [DataMember]
        public int JuntaWorkStatusID { get; set; }
        [DataMember]
        public int EstatusID { get; set; }
        [DataMember]
        public string Estatus { get; set; }
        [DataMember]
        public bool Hold { get; set; }
        [DataMember]
        public bool RequierePWHT { get; set; }
        [DataMember]
        public string NombreSpool { get; set; }
    }
}
