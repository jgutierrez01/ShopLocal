using System.Collections.Generic;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionEspesor: BaseValidationException
    {
        public ExcepcionEspesor(List<string> details) : base(details) { }

        public ExcepcionEspesor(string detail) : base(detail) { }
    }
}
