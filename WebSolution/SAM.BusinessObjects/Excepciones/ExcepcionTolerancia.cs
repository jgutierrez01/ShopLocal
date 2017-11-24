using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionTolerancia : BaseValidationException
    {
        public ExcepcionTolerancia(List<string> details) : base(details) { }

        public ExcepcionTolerancia(string detail)
            : base(detail)
        {
        }
    }
}
