using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionCruce : BaseValidationException
    {
        public ExcepcionCruce(List<string> details) : base(details) { }

        public ExcepcionCruce(string detail): base(detail){}
    }
}
