using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SAM.Entities.Parciales;

namespace SAM.Entities
{
    public partial class NumeroUnico
    {
        private NumeroUnicoCruceInfo _infoCruce;

        [DataMember]
        public NumeroUnicoCruceInfo InfoCruce
        {
            get
            {
                if (_infoCruce == null)
                {
                    _infoCruce = new NumeroUnicoCruceInfo();
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
