using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionDespacho : BaseValidationException
    {
        public ExcepcionDespacho(List<string> details) : base(details) { }

        public ExcepcionDespacho(string detail): base(detail)
        {
        }
    }
}
