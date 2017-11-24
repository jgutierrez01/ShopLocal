using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionDuplicados : BaseValidationException
    {
        public ExcepcionDuplicados(List<string> details) : base(details) { }

        public ExcepcionDuplicados(string detail): base(detail)
        {
        }
    }
}
