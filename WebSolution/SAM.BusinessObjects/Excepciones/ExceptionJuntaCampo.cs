using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExceptionJuntaCampo : BaseValidationException
    {
        public ExceptionJuntaCampo(string detail) : base(detail)
        {
        }
    }
}
