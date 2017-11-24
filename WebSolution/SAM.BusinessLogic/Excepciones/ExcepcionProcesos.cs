using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionProcesos : BaseValidationException
    {
        public ExcepcionProcesos(List<string> details) : base(details) { }

        public ExcepcionProcesos(string detail) : base(detail) { }
    }
}
