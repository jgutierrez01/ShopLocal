using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAM.BusinessLogic.Ingenieria.Excepciones
{
    class ExcepcionInterprete : Exception
    {
        private int? Renglon;
        private int? Columna;

        public ExcepcionInterprete(Exception exception, string mensaje, int? renglon, int? columna): base(mensaje,exception)
        {
            Columna = columna;
            Renglon = renglon;
        }

        public ExcepcionInterprete(string mensaje, int? renglon, int? columna): base(mensaje)
        {
            Columna = columna;
            Renglon = renglon;
        }

    }
}
