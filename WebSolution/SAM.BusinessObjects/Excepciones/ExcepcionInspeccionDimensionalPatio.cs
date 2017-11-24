using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionInspeccionDimensionalPatio : BaseValidationException
    {
         public ExcepcionInspeccionDimensionalPatio(List<string> details) : base(details) { }

         public ExcepcionInspeccionDimensionalPatio(string detail) : base(detail) { }         
    }
}
