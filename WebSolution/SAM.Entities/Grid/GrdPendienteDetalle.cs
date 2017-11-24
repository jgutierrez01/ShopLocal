using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Mimo.Framework.Common;

namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdPendienteDetalle
    {
        [DataMember]
        public int PendienteDetalleID { get; set; }
        
        [DataMember]
        public bool EsAlta { get; set; }
       
        [DataMember]
        public string Accion 
        {
            get 
            {
                if (EsAlta)
                {
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Created" : "Creado";
                }
                else
                {
                    return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Edited" : "Editado";
                }
            }
        }
        
        [DataMember]
        public string Area { get; set; }

        [DataMember]
        public string Responsable { get; set; }
        
        [DataMember]
        public string Estatus { get; set; }

        [DataMember]
        public DateTime? Fecha { get; set; }

        [DataMember]
        public string Autor { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

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
                        return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Closed" : "Cerrado" ;
                    case EstatusPendiente.Resuelto:
                        return LanguageHelper.CustomCulture == LanguageHelper.INGLES ? "Resolved" : "Resuelto";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}
