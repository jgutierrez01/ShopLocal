using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Extensions;

namespace SAM.Generador
{
    public static class KeyboardUtils
    {
        public static void ImprimeInline(string s)
        {
            Console.Write(s);
        }

        public static int LeeEntero()
        {
            return Console.ReadLine().SafeIntParse();
        }

        public static string LeeCadena()
        {
            return Console.ReadLine();
        }
    }
}
