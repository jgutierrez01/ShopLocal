using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetMaterialSpoolOdt : DetMaterialSpool
    {
        public DetMaterialSpoolOdt() : base() { }

        public DetMaterialSpoolOdt(MaterialSpool material): base(material)
        {
            if (material.OrdenTrabajoMaterial != null && material.OrdenTrabajoMaterial.Count > 0)
            {
                OrdenTrabajoMaterial odtm = material.OrdenTrabajoMaterial[0];
                ExisteEnLaOdt = true;
                OrdenTrabajoMaterialID = odtm.OrdenTrabajoMaterialID;
                FueReingenieria = odtm.FueReingenieria;
            }
            else
            {
                ExisteEnLaOdt = false;
                OrdenTrabajoMaterialID = -1;
            }
        }

        [DataMember]
        public bool ExisteEnLaOdt { get; set; }
        [DataMember]
        public bool FueReingenieria { get; set; }
        [DataMember]
        public int OrdenTrabajoMaterialID { get; set; }
    }
}
