using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionReportes : BaseValidationException
    {
        public ExcepcionReportes(List<string> details) : base(details) { }

        public ExcepcionReportes(string detail)
            : base(detail)
        {
        }
    }
}
