using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public abstract class ProveedorBase : EntidadBase
    {
        [DataMember]
        public int ContactoID { get; set; }

        [DataMember]
        public ContactoStruct Contacto { get; set; }

        [DataMember]
        public string Descripcion { get; set; }


        [DataMember]
        public string Direccion { get; set; }

        [DataMember]
        public string Telefono { get; set; }
    }
}
