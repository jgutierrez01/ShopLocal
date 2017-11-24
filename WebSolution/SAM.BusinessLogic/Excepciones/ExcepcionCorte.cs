using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionCorte : BaseValidationException
    {
         public ExcepcionCorte(List<string> details) : base(details) { }

         public ExcepcionCorte(string detail) : base(detail) { }
    }
}
