using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Cache
{
    [Serializable]
    public class ProyectoCache : EntidadBase
    {
        [DataMember]
        public int PatioID { get; set; }

        [DataMember]
        public int ClienteID { get; set; }

        [DataMember]
        public string NombreCliente { get; set; }

        [DataMember]
        public int ContactoID { get; set; }

        [DataMember]
        public string Descripcion { get; set; }

        [DataMember]
        public DateTime ? FechaInicio { get; set; }

        [DataMember]
        public int ColorID { get; set; }

        [DataMember]
        public string NombreColor { get; set; }

        [DataMember]
        public string HexadecimalColor { get; set; }

        [DataMember]
        public string NombrePatio {get; set;}

        [DataMember]
        public int ColumnasNomenclatura { get; set; }

        [DataMember]
        public string PrefijoNumeroUnico { get; set; }
        
        [DataMember]
        public string PrefijoOdt { get; set; }

        [DataMember]
        public string Estatus { get; set; }

        [DataMember]
        public bool Activo { get; set; }
        
        [DataMember]
        public List<NomenclaturaStruct> Nomenclatura { get; set; }
        
        [DataMember]
        public int DigitosOdt { get; set; }
    }
}
