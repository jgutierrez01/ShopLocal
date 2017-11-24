using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAM.Web.Classes
{
    public class ExcelException:Exception
    {
        public ExcelException() : base() { }

        public ExcelException(string message) : base(message) { }
    }
}