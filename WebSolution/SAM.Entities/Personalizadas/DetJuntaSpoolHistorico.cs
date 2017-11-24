using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SAM.Entities.Cache;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetJuntaSpoolHistorico
    {
        public DetJuntaSpoolHistorico() { }

        public DetJuntaSpoolHistorico(JuntaSpoolHistorico junta, Dictionary<int, string> tiposJunta, Dictionary<int, string> fabAreas, Dictionary<int, string> familiasAcero)
        {
            JuntaSpoolID = junta.JuntaSpoolID;
            Etiqueta = junta.Etiqueta;
            Diametro = junta.Diametro;
            TipoJuntaID = junta.TipoJuntaID;
            Cedula = junta.Cedula;
            EtiquetaMaterial1 = junta.EtiquetaMaterial1;
            EtiquetaMaterial2 = junta.EtiquetaMaterial2;
            FabAreaID = junta.FabAreaID;
            TipoJunta = tiposJunta[junta.TipoJuntaID];
            FabArea = fabAreas[junta.FabAreaID];
            FamiliaAceroMaterial1 = familiasAcero[junta.FamiliaAceroMaterial1ID];
            FamiliaAceroMaterial2 = junta.FamiliaAceroMaterial2ID != null ? familiasAcero[junta.FamiliaAceroMaterial1ID] : string.Empty;
            Espesor = junta.Espesor != null ? (Decimal)junta.Espesor : 0;
            Peqs = junta.Peqs != null ? junta.Peqs : 0;
        }

        [DataMember]
        public int JuntaSpoolID { get; set; }
        [DataMember]
        public string Etiqueta { get; set; }
        [DataMember]
        public decimal Espesor { get; set; }
        [DataMember]
        public decimal Diametro { get; set; }
        [DataMember]
        public int TipoJuntaID { get; set; }
        [DataMember]
        public string Cedula { get; set; }
        [DataMember]
        public string EtiquetaMaterial1 { get; set; }
        [DataMember]
        public string EtiquetaMaterial2 { get; set; }
        [DataMember]
        public string Localizacion
        {
            get
            {
                return string.Concat(EtiquetaMaterial1, '-', EtiquetaMaterial2);
            }
        }
        [DataMember]
        public int FabAreaID { get; set; }
        [DataMember]
        public string TipoJunta { get; set; }
        [DataMember]
        public string FabArea { get; set; }
        [DataMember]
        public string FamiliaAceroMaterial1 { get; set; }
        [DataMember]
        public string FamiliaAceroMaterial2 { get; set; }
        [DataMember]
        public string FamiliasAcero
        {
            get
            {
                if (!string.IsNullOrEmpty(FamiliaAceroMaterial2))
                {
                    return string.Concat(FamiliaAceroMaterial1, '/', FamiliaAceroMaterial2);
                }
                return FamiliaAceroMaterial1;
            }
        }
        [DataMember]
        public decimal? Peqs { get; set; }
    }
}