using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SAM.Entities.Parciales;

namespace SAM.Entities
{
    public partial class NumeroUnicoInventario
    {
        private NumeroUnicoInventarioCruceInfo _infoCruce;

        [DataMember]
        public NumeroUnicoInventarioCruceInfo InfoCruce
        {
            get
            {
                if (_infoCruce == null)
                {
                    _infoCruce = new NumeroUnicoInventarioCruceInfo();
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
