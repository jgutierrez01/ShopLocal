using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Cache;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetJuntaSpoolOdt: DetJuntaSpool
    {
        public DetJuntaSpoolOdt() : base() { }

        public DetJuntaSpoolOdt(JuntaSpool junta, Dictionary<int, string> tiposJunta, Dictionary<int, string> fabAreas, Dictionary<int, string> familiasAcero): base(junta, tiposJunta, fabAreas, familiasAcero)
        {
            if (junta.OrdenTrabajoJunta != null && junta.OrdenTrabajoJunta.Count > 0)
            {
                OrdenTrabajoJunta odtj = junta.OrdenTrabajoJunta[0];
                ExisteEnLaOdt = true;
                OrdenTrabajoJuntaID = odtj.OrdenTrabajoJuntaID;
                FueReingenieria = odtj.FueReingenieria;
            }
            else
            {
                ExisteEnLaOdt = false;
            }
        }

        [DataMember]
        public bool FueReingenieria { get; set; }
        [DataMember]
        public bool ExisteEnLaOdt { get; set; }
        [DataMember]
        public int OrdenTrabajoJuntaID { get; set; }
    }
}
