using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class UsuarioBloqueadoException : BaseValidationException
    {
        public UsuarioBloqueadoException() : base() { }
        
        public UsuarioBloqueadoException(List<string> details) : base(details) { }

        public UsuarioBloqueadoException(string detail): base(detail){}
    }
}
