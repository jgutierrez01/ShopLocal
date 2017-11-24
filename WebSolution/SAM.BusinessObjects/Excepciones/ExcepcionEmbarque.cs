using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionEmbarque : BaseValidationException
    {
        public ExcepcionEmbarque(List<string> details) : base(details) { }

        public ExcepcionEmbarque(string detail)
            : base(detail)
        {
        }
    }
}
