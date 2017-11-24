using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class DetMaterialSpoolHistorico
    {
        public DetMaterialSpoolHistorico() { }

        public DetMaterialSpoolHistorico(MaterialSpoolHistorico material)
        {
            MaterialSpoolID = material.MaterialSpoolID;
            ItemCodeID = material.ItemCodeID;
            CodigoItemCode = material.ItemCodeCodigo; 
            DescripcionItemCode = material.ItemCodeDescripcionEspanol;
            Diametro1 = material.Diametro1;
            Diametro2 = material.Diametro2;
            Cantidad = material.Cantidad;
            Etiqueta = material.Etiqueta;
            Categoria = material.Grupo;
            Especificacion = material.Especificacion;
            Peso = material.Peso != null ? (Decimal)material.Peso : 0;
            Grupo = material.Grupo;
            Area = material.Area != null ? (Decimal)material.Area : 0;
        }

        [DataMember]
        public decimal Area { get; set; }
        [DataMember]
        public int MaterialSpoolID { get; set; }
        [DataMember]
        public int ItemCodeID { get; set; }
        [DataMember]
        public string CodigoItemCode { get; set; }
        [DataMember]
        public string DescripcionItemCode { get; set; }
        [DataMember]
        public decimal Diametro1 { get; set; }
        [DataMember]
        public decimal Diametro2 { get; set; }
        [DataMember]
        public int Cantidad { get; set; }
        [DataMember]
        public string Etiqueta { get; set; }
        [DataMember]
        public string Categoria { get; set; }
        [DataMember]
        public string Especificacion { get; set; }
        [DataMember]
        public decimal Peso { get; set; }
        [DataMember]
        public string Grupo { get; set; }
    }
}
