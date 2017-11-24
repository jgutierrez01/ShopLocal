using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionRelaciones : BaseValidationException
    {
        public ExcepcionRelaciones(List<string> details) : base(details) { }

        public ExcepcionRelaciones(string detail): base(detail)
        {
        }
    }
}
