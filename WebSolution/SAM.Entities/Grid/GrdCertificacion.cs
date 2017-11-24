using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdCertificacion
    {
        [DataMember]
        public int SpoolID { get; set; }
        [DataMember]
        public string Spool { get; set; }
        [DataMember]
        public string NumeroControl { get; set; }
        [DataMember]
        public string Dibujo { get; set; }
        [DataMember]
        public int JuntaID { get; set; }
        [DataMember]
        public int? JuntaArmadoID { get; set; }
        [DataMember]
        public int? JuntaWorkStatusID { get; set; }
        [DataMember]
        public int? JuntaSoldaduraID { get; set; }
        [DataMember]
        public bool ArmadoCompletas { get; set; }
        [DataMember]
        public bool SoladoCompleto { get; set; }
        [DataMember]
        public bool InspVisAprobada { get; set; }
        [DataMember]
        public bool InspVisImprimir { get; set; }
        [DataMember]
        public bool InspDimAprobada { get; set; }
        [DataMember]
        public bool InspDimImprimir { get; set; }
        [DataMember]
        public bool PinturaCompleta { get; set; }
        [DataMember]
        public bool PinturaImprimir { get; set; }
        [DataMember]
        public bool InspEspesoresAprobada { get; set; }
        [DataMember]
        public bool InspEspImprimir { get; set; }
        [DataMember]
        public bool RtCompleto { get; set; }
        [DataMember]
        public bool RtImprimir { get; set; }
        [DataMember]
        public bool PtCompleto { get; set; }
        [DataMember]
        public bool PtImprimir { get; set; }
        [DataMember]
        public bool PwhtCompleto { get; set; }
        [DataMember]
        public bool PwhtImprimir { get; set; }
        [DataMember]
        public bool DurezasCompleto { get; set; }
        [DataMember]
        public bool DurezasImprimir { get; set; }
        [DataMember]
        public bool RtPostImprimir { get; set; }
        [DataMember]
        public bool RtPostCompleto { get; set; }
        [DataMember]
        public bool PtPostCompleto { get; set; }
        [DataMember]
        public bool PtPostImprimir { get; set; }
        [DataMember]
        public bool PreheatImprimir { get; set; }
        [DataMember]
        public bool PreheatCompleto { get; set; }
        [DataMember]
        public bool EmbarqueCompleto { get; set; }
        [DataMember]
        public bool EmbarqueImprimir { get; set; }
        [DataMember]
        public bool EmbarqueEscaneado { get; set; }
        [DataMember]
        public bool TrazabilidadEscaneado { get; set; }
        [DataMember]
        public bool WpsCompleto { get; set; }
        [DataMember]
        public bool MtrCompleto { get; set; }
        [DataMember]
        public bool MtrSoldCompleto { get; set; }
        [DataMember]
        public bool WpsImprimir { get; set; }
        [DataMember]
        public bool MTRImprimir { get; set; }
        [DataMember]
        public bool MTRSoldImprimir { get; set; }
        [DataMember]
        public bool UtCompleto { get; set; }
        [DataMember]
        public bool UtImprimir { get; set; }
        [DataMember]
        public bool PMICompleto { get; set; }
        [DataMember]
        public bool PMIImprimir { get; set; }
        [DataMember]
        public bool PruebaHidroAprobada { get; set; }
        [DataMember]
        public bool PruebaHidroImprimir { get; set; }
        [DataMember]
        public string NumeroEmbarque { get; set; }
        [DataMember]
        public bool DibujoImprimir { get; set; }

        [DataMember] public List<string> InspDimReportes;
        [DataMember] public List<string> PinturaReportes;
        [DataMember] public List<string> InspEspReportes;
        [DataMember] public List<string> RtReportes;
        [DataMember] public List<string> PtReportes;
        [DataMember] public List<string> PwhtReportes;
        [DataMember] public List<string> DurezasReportes;
        [DataMember] public List<string> PreheatReportes;
        [DataMember] public List<string> UtReportes;
        [DataMember] public List<string> PMIReportes;
        [DataMember] public List<string> PruebaHidroReportes;
        [DataMember] public List<string> RtPostReportes;
        [DataMember] public List<string> InspVisReportes;
        [DataMember] public List<string> PtPostReportes;
        [DataMember] public List<string> WpsReportes;
        [DataMember] public List<string> MTRReportes;
        [DataMember] public List<string> MTRSoldReportes;
    }
}
