using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Reportes
{
    [Serializable]
    public class FaltanteCruce
    {
        [DataMember]
        public int ProyectoID { get; set; }

        [DataMember]
        public string Proyecto { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public string NombreSpool { get; set; }

        [DataMember]
        public int MaterialSpoolID { get; set; }

        [DataMember]
        public int Cantidad { get; set; }

        [DataMember]
        public int ItemCodeID { get; set; }

        [DataMember]
        public string CodigoItemCode { get; set; }

        [DataMember]
        public string DescripcionItemCode { get; set; }

        [DataMember]
        public string FamiliaAcero { get; set; }

        [DataMember]
        public string Etiqueta { get; set; }

        [DataMember]
        public string Especificacion { get; set;}

        [DataMember]
        public string Grupo {get; set;}

        [DataMember]
        public decimal Diametro1 { get; set; }

        [DataMember]
        public decimal Diametro2 { get; set; }

        [DataMember]
        public int Prioridad { get; set; }

        [DataMember]
        public bool Congelado { get; set; }

        [DataMember]
        public bool MaterialEquivalente { get; set; }

        [DataMember]
        public string Isometrico { get; set; }

        [DataMember]
        public string Cedula { get; set; }
        
        [DataMember]
        public string NumeroUnicoCongelado { get; set; }

        [DataMember]
        public decimal Pdis { get; set; }

        [DataMember]
        public decimal Peso { get; set; }

        [DataMember]
        public NumeroUnico NumeroUnicoUtilizado { get; set; }

        [DataMember]
        public bool SpoolEnHold { get; set; }

        [DataMember]
        public string ObservacionesSpoolHold { get; set; }

        [DataMember]
        public bool IsometricoCompleto { get; set; }

        [DataMember]
        public string Campo2 { get; set; }

        [DataMember]
        public decimal DiametroMayor { get; set; }

        [DataMember]
        public string Campo1 { get; set; }
    }
}
