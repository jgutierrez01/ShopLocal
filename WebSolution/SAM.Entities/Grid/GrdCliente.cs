using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
[Serializable]
    public class GrdCliente
    {
    [DataMember]
    public int ClienteID { get; set; }
    [DataMember]
    public string NombreCliente { get; set; }
    [DataMember]
    public string TelefonoOficina { get; set; }
    [DataMember]
    public string CorreoElectronico { get; set; }
    [DataMember]
    public string NombreContacto { get; set; }
    }
}
