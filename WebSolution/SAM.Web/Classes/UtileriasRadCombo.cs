using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Telerik.Web.UI;
using Mimo.Framework.Extensions;

namespace SAM.Web.Classes
{
    public static class UtileriasRadCombo
    {
        public static int? ObtenId(RadComboBoxContext ctx, string llave)
        {
            if (ctx != null)
            {
                if (ctx.ContainsKey(llave))
                {
                    object o = ctx[llave];

                    if (o != null)
                    {
                        return ctx[llave].SafeIntParse();
                    }
                }
            }

            return null;
        }
    }
}