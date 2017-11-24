using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionConcordancia :BaseValidationException
    {
        public ExcepcionConcordancia(List<string> details) : base(details) { }

        public ExcepcionConcordancia(string detail)
            : base(detail)
        {
        }
    }
}
