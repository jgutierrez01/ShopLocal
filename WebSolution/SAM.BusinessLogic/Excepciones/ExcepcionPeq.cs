using System.Collections.Generic;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionPeq: BaseValidationException
    {
        public ExcepcionPeq(List<string> details) : base(details) { }

        public ExcepcionPeq(string detail) : base(detail) { }
    }
}
