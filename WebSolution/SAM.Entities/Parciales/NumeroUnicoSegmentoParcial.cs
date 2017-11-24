using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SAM.Entities.Parciales;

namespace SAM.Entities
{
    public partial class NumeroUnicoSegmento
    {
        private NumeroUnicoSegmentoCruceInfo _infoCruce;

        [DataMember]
        public NumeroUnicoSegmentoCruceInfo InfoCruce
        {
            get
            {
                if (_infoCruce == null)
                {
                    _infoCruce = new NumeroUnicoSegmentoCruceInfo();
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
