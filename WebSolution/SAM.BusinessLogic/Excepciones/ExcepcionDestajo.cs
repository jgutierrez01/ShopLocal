using System.Collections.Generic;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionDestajo: BaseValidationException
    {
        public ExcepcionDestajo(List<string> details) : base(details) { }

        public ExcepcionDestajo(string detail) : base(detail) { }
    }
}
