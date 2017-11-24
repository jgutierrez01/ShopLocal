using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionConcurrencia : BaseValidationException
    {
        public ExcepcionConcurrencia(List<string> details) : base(details) { }

        public ExcepcionConcurrencia(string detail): base(detail)
        {
        }
    }
}
