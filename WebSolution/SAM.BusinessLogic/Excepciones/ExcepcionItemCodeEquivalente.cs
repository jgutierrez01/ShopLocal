using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionItemCodeEquivalente: BaseValidationException
    {
        public ExcepcionItemCodeEquivalente(List<string> details) : base(details) { }

        public ExcepcionItemCodeEquivalente(string detail) : base(detail) { }
    }
}
