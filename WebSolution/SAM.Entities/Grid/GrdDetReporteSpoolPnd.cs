using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Mimo.Framework.Common;
namespace SAM.Entities.Grid
{
    [Serializable]
    public class GrdDetReporteSpoolPnd
    {
        [DataMember]
        public int SpoolReportePndID { get; set; }

        [DataMember]
        public string NumeroDeControl { get; set; }

        [DataMember]
        public string Spool { get; set; }

        [DataMember]
        public string NumeroDeRequisicion { get; set; }

        [DataMember]
        public int? Hoja { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }

        [DataMember]
        public int FamiliaAceroMaterial1ID { get; set; }

        [DataMember]
        public int? FamiliaAceroMaterial2ID { get; set; }

        [DataMember]
        public string FamiliaAceroMaterial1 { get; set; }

        [DataMember]
        public string FamiliaAceroMaterial2 { get; set; }

        [DataMember]
        public string FamiliaAcero
        {
            get
            {
                if (FamiliaAceroMaterial2ID > -1)
                {
                    return string.Concat(FamiliaAceroMaterial1, " / ", FamiliaAceroMaterial2);
                }

                return FamiliaAceroMaterial1;
            }
        }

        [DataMember]
        public string Resultado
        {
            get
            {
                if (Aprobado)
                {
                    if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                    {
                        return "Aprobado.en-US";
                    }
                    return "Aprobado";
                }
                if (LanguageHelper.CustomCulture == LanguageHelper.INGLES)
                {
                    return "Rechazado.en-US";
                }
                return "Rechazado";

            }
        }

        [DataMember]
        public bool Aprobado { get; set; }

        [DataMember]
        public string Observaciones { get; set; }

        [DataMember]
        public int SpoolID { get; set; }

        [DataMember]
        public bool TieneHold { get; set; }
    }
}
