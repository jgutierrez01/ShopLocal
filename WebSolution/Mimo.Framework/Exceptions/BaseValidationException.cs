using System.Collections.Generic;

namespace Mimo.Framework.Exceptions
{
    public class BaseValidationException : System.Exception
    {
        public List<string> Details;

        public BaseValidationException() : base() { }

        public BaseValidationException(List<string> details)
        {
            Details = details;
        }

        public BaseValidationException(string detail)
        {
            Details = new List<string>{detail};
        }
       
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public override System.Exception GetBaseException()
        {
            return base.GetBaseException();
        }

        public override string StackTrace
        {
            get
            {
                return base.StackTrace;
            }
        }

        public override string Source
        {
            get
            {
                return base.Source;
            }
            set
            {
                base.Source = value;
            }
        }

        public override string HelpLink
        {
            get
            {
                return base.HelpLink;
            }
            set
            {
                base.HelpLink = value;
            }
        }

        public override System.Collections.IDictionary Data
        {
            get
            {
                return base.Data;
            }
        }

        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
}
