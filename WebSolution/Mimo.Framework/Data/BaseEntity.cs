using System;
using System.Runtime.Serialization;

namespace Mimo.Framework.Data
{
    [Serializable]
    public abstract class BaseEntity
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Text { get; set; }

       
    }
}
