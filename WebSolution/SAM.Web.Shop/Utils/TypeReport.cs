using SAM.Entities.Personalizadas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;


namespace SAM.Web.Shop.Utils
{
    public class TypeReport
    {
        public static List<Simple> GetTypesReport()
        {
            List<Simple> types = new List<Simple>();
            types.Add(new Simple() { ID = 1, Valor = "Sand-Blast" });

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                types.Add(new Simple() { ID = 2, Valor = "Primary" });
                types.Add(new Simple() { ID = 3, Valor = "Intermediate" });
                types.Add(new Simple() { ID = 4, Valor = "Final Coat" });
                types.Add(new Simple() { ID = 5, Valor = "Adhesion" });         
            }
            else
            {
                types.Add(new Simple() { ID = 2, Valor = "Primario" });
                types.Add(new Simple() { ID = 3, Valor = "Intermedio" });
                types.Add(new Simple() { ID = 4, Valor = "Acabado visual" });
                types.Add(new Simple() { ID = 5, Valor = "Adherencia" });         
            }
            types.Add(new Simple() { ID = 6, Valor = "Pull Off" });
            return types;
        }
    }
}
