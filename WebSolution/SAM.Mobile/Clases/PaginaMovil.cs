using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mobile;
using System.Web.UI.MobileControls;
using Mimo.Framework.Common;

namespace SAM.Mobile.Clases
{
    public class PaginaMovil : MobilePage
    {
        protected override void InitializeCulture()
        {
            base.InitializeCulture();
            if (LanguageHelper.CustomCulture == string.Empty) return;
            Culture = LanguageHelper.CustomCulture;
            UICulture = LanguageHelper.CustomCulture;
        }
    }
}