using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionFechas : BaseValidationException
    {
        public ExcepcionFechas(List<string> details) : base(details) { }

        public ExcepcionFechas(string detail)
            : base(detail)
        {
        }
    }
}
