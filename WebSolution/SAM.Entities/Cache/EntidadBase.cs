using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Data;

namespace SAM.Entities.Cache
{
    [Serializable]
    public abstract class EntidadBase : BaseEntity
    {
        [DataMember]
        public string Nombre
        {
            get
            {
                return Text;
            }
            set
            {
                Text = value;
            }
        }

    }

}