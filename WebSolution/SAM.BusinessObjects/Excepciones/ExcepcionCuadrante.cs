using Mimo.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.BusinessObjects.Excepciones
{
     public class ExcepcionCuadrante: BaseValidationException
    {
        public ExcepcionCuadrante(List<string> details) : base(details) { }

        public ExcepcionCuadrante(string detail)
            : base(detail)
        {
        }
    }
}
