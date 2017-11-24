using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAM.Entities
{
    public class ArchivosWorkstatus
    {
        public byte[] WorkstatusSpoolEspanol { get; set; }        
        public byte[] WorkstatusSpoolIngles { get; set; }
        public byte[] WorkstatusJuntaEspanol { get; set; }
        public byte[] WorkstatusJuntaIngles { get; set; }
        public string NombreArchivoWksSpool { get; set; }
        public string NombreArchivoWksJunta { get; set; }                
    }
}
