using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionEstimacionJunta : BaseValidationException
    {
         public ExcepcionEstimacionJunta(List<string> details) : base(details) { }

         public ExcepcionEstimacionJunta(string detail) : base(detail) { }
    }
}
