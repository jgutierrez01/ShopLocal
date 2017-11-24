using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionNumeroUnicoDespacho : BaseValidationException
    {
        public ExcepcionNumeroUnicoDespacho(List<string> details) : base(details) { }

        public ExcepcionNumeroUnicoDespacho(string detail) : base(detail)
        {
        }
    }
}
