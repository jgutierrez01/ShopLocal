using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
     public class ExcepcionSoldadura: BaseValidationException
    {
        public ExcepcionSoldadura(List<string> details) : base(details) { }

        public ExcepcionSoldadura(string detail)
            : base(detail)
        {
        }
    }
}
