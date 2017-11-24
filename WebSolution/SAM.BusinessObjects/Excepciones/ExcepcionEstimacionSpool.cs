using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    class ExcepcionEstimacionSpool : BaseValidationException
    {
         public ExcepcionEstimacionSpool(List<string> details) : base(details) { }

         public ExcepcionEstimacionSpool(string detail) : base(detail) { }
    }
}
