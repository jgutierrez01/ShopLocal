using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities.Parciales;
using System.Runtime.Serialization;

namespace SAM.Entities
{
    public partial class OrdenTrabajoMaterial
    {
        private OrdenTrabajoMaterialCruceInfo _infoCruce;

        [DataMember]
        public OrdenTrabajoMaterialCruceInfo InfoCruce
        {
            get
            {
                if (_infoCruce == null)
                {
                    _infoCruce = new OrdenTrabajoMaterialCruceInfo();
                }
                return _infoCruce;
            }
            set
            {
                _infoCruce = value;
            }
        }
    }
}
