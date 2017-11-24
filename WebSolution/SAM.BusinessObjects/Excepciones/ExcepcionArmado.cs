using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionArmado : BaseValidationException
    {
        public ExcepcionArmado(List<string> details) : base(details) { }

        public ExcepcionArmado(string detail)
            : base(detail)
        {
        }
    }
}
