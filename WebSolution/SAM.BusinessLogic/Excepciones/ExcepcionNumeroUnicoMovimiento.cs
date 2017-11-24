using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionNumeroUnicoMovimiento : BaseValidationException
    {
        public ExcepcionNumeroUnicoMovimiento(List<string> details) : base(details) { }

        public ExcepcionNumeroUnicoMovimiento(string detail)
            : base(detail)
        {
        }
    }
}
