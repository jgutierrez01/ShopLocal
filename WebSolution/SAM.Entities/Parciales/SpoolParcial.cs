using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SAM.Entities.Parciales;

namespace SAM.Entities
{
    public partial class Spool
    {
        private SpoolCruceInfo _infoCruce;

        [DataMember]
        public SpoolCruceInfo InfoCruce
        {
            get
            {
                if (_infoCruce == null)
                {
                    _infoCruce = new SpoolCruceInfo();
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
