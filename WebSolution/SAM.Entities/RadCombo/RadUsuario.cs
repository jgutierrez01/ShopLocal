using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.RadCombo
{
    [Serializable]
    public class RadUsuario
    {
        [DataMember]
        public Guid UsuarioID { get; set; }
               
        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string ApPaterno { get; set; }

        [DataMember]
        public string ApMaterno { get; set; }

        [DataMember]
        public string NombreCompleto { get; set; }
    }
}
