using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessLogic.Excepciones
{
    public class ExcepcionMaterialInsuficiente : BaseValidationException
    {
        public ExcepcionMaterialInsuficiente(List<string> details) : base(details) { }

        public ExcepcionMaterialInsuficiente(string detail): base(detail){}
    }
}
