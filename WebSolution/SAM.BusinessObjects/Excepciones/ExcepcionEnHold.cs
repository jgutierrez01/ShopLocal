using Mimo.Framework.Exceptions;

namespace SAM.BusinessObjects.Excepciones
{
    public class ExcepcionEnHold : BaseValidationException
    {
        public ExcepcionEnHold(string detail) :base(detail){}
    }
}
