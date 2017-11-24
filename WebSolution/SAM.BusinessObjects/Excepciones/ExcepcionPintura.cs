using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionPintura : BaseValidationException
    {
         public ExcepcionPintura(List<string> details) : base(details) { }

         public ExcepcionPintura(string detail)
            : base(detail)
        {
        }
    }
}



