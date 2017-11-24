using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdPendientes
    {
        [DataMember]
        public int PendienteID { get; set; }
        [DataMember]
        public string NombreProyecto { get; set; }
        [DataMember]
        public string Titulo { get; set; }
        [DataMember]
        public string Area {get; set; }
        [DataMember]
        public string Responsable {get; set;}
        [DataMember]
        public string Estatus {get; set;}
        [DataMember]
        public string Autor { get; set; }
        [DataMember]
        public int CategoriaPendienteID { get; set; }
        [DataMember]
        public DateTime? FechaModificacion { get; set; }
        [DataMember]
        public string DescripcionEstatus
        {
            get
            {
                switch (Estatus)
                {
                    case EstatusPendiente.Abierto:
                        return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Open" : "Abierto";
                    case EstatusPendiente.Cerrado:
                        return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Closed" : "Cerrado";
                    case EstatusPendiente.Resuelto:
                        return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Resolved" : "Resuelto";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
