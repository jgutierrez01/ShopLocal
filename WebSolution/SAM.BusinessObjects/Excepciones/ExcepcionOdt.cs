using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionOdt: BaseValidationException
    {
        public ExcepcionOdt(List<string> details) : base(details) { }

        public ExcepcionOdt(string detail): base(detail)
        {
        }
    }
}
