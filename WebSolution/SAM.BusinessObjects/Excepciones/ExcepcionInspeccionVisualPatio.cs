using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    class ExcepcionInspeccionVisualPatio : BaseValidationException
    {
         public ExcepcionInspeccionVisualPatio(List<string> details) : base(details) { }

         public ExcepcionInspeccionVisualPatio(string detail) : base(detail) { }
    }
}
