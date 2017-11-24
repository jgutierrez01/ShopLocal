using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionKgTeorico: BaseValidationException
    {
        public ExcepcionKgTeorico(List<string> details) : base(details) { }

        public ExcepcionKgTeorico(string detail) : base(detail) { }
    }
}
