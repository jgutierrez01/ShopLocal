using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SAM.Entities.Parciales;

namespace SAM.Entities
{
    public partial class MaterialSpool
    {
        private MaterialSpoolCruceInfo _infoCruce;

        [DataMember]
        public MaterialSpoolCruceInfo InfoCruce
        {
            get
            {
                if (_infoCruce == null)
                {
                    _infoCruce = new MaterialSpoolCruceInfo();
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
