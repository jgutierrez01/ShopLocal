using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionSeguimiento : BaseValidationException
    {
         public ExcepcionSeguimiento(List<string> details) : base(details) { }

         public ExcepcionSeguimiento(string detail) : base(detail) { }
    }
}
