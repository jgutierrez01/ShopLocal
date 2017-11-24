using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAM.BusinessLogic.Excel
{
    public static class UtileriasCsv
    {
        private static string LimpiaTexto(string texto)
        {
            string textoBarrido = texto;

            textoBarrido = textoBarrido.Replace("\r", " ");
            textoBarrido = textoBarrido.Replace("\n", " ");
            textoBarrido = textoBarrido.Replace(",", "; ");

            return textoBarrido;
        }

        private static string LimpiaTextoPlano(string texto)
        {
            string textoBarrido = texto;

            textoBarrido = textoBarrido.Replace("\r", " ");
            textoBarrido = textoBarrido.Replace("\n", " ");
            textoBarrido = textoBarrido.Replace(",", "; ");

            textoBarrido = string.Format("=\"{0}\"", textoBarrido);

            return textoBarrido;
        }

        public static void InsertaComa<T>(T texto, StringBuilder stringBuilder)
        {
            string str = texto != null ? texto.ToString() : string.Empty;

            stringBuilder.Append(LimpiaTexto(str));
            stringBuilder.Append(",");
        }

        public static void InsertaComaTextoPlano<T>(T texto, StringBuilder stringBuilder)
        {
            string str = texto != null ? texto.ToString() : string.Empty;

            stringBuilder.Append(LimpiaTextoPlano(str));
            stringBuilder.Append(",");
        }


        public static void InsertaComaFecha(DateTime? texto, StringBuilder stringBuilder)
        {
            string str = "";

            str = texto.HasValue ? LimpiaTexto(texto.Value.ToString("yyyy/MM/dd")) : string.Empty;

            stringBuilder.Append(LimpiaTexto(str));
            stringBuilder.Append(",");
        }
    }
}
