using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class PreguntaSecreta
    {
        [DataMember]
        public string Pregunta { get; set; }

        [DataMember]
        public string PreguntaIngles { get; set; }
    }
}
