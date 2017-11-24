using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionPrioridad : BaseValidationException
    {
        public ExcepcionPrioridad(List<string> details) : base(details) { }

        public ExcepcionPrioridad(string detail): base(detail)
        {
        }
    }
}
