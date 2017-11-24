using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdPersonaDestajo
    {
        /// <summary>
        /// En caso de ser Soldador es el DestajoSoldadorID, para Tubero es DestajoTuberoID
        /// </summary>
        [DataMember]
        public int IdRegistroDetalle { get; set; }
        /// <summary>
        /// TuberoID o SoldadorID depende del caso
        /// </summary>
        [DataMember]
        public int IdPersona { get; set; }
        [DataMember]
        public string Codigo { get; set; }
        [DataMember]
        public string NumEmpleado { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string ApPaterno { get; set; }
        [DataMember]
        public string ApMaterno { get; set; }
        [DataMember]
        public decimal SumaPdis { get; set; }
        [DataMember]
        public decimal TotalAPagar { get; set; }
        [DataMember]
        public bool Aprobado { get; set; }
        [DataMember]
        public string EstatusTexto { get; set; }
        [DataMember]
        public string CategoriaPuestoTexto { get; set; }
        [DataMember]
        public bool EsTubero { get; set; }
        
        [DataMember]
        public string AreaTrabajo { get; set; }

        [DataMember]
        public string NombreCompleto
        {
            get 
            {
                if (string.IsNullOrEmpty(ApMaterno))
                {
                    return string.Concat(Nombre, ' ', ApPaterno, ' ', ApMaterno);
                }
                return string.Concat(Nombre, ' ', ApPaterno);
            } 
        }
    }
}
