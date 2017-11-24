using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mimo.Framework.WebControls;
using System.Threading;
using Mimo.Framework.Extensions;
using System.Globalization;
using Telerik.Web.UI;
using Mimo.Framework.Common;

namespace SAM.Web.Classes
{
    public class SamPaginaBase : MappablePage
    {

        /// <summary>
        /// Indicates if the page should use https or not
        /// </summary>
        private bool _isSecure = false;

        /// <summary>
        /// Indicates if the page could either use https or http
        /// </summary>
        private bool _allowBoth = false;

        /// <summary>
        /// Allows to control whether the pages shuld use https or not.
        /// </summary>
        public bool IsSecure
        {
            get { return _isSecure; }
            set { _isSecure = value; }
        }

        public bool AllowBothProtocols
        {
            get { return _allowBoth; }
            set { _allowBoth = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!_allowBoth)
            {
                if (!_isSecure)
                {
                    UtileriaRedireccion.PushSSL(false, HttpContext.Current);
                }
                else
                {
                    UtileriaRedireccion.PushSSL(true, HttpContext.Current);
                }

            }        
            
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            configurarCulturaControlesTelerik();
            base.OnLoad(e);
        }

        private void configurarCulturaControlesTelerik()
        {
            Controls.IterateRecursively(x =>
            {
                if (x is MimossRadGrid)
                {
                    ((MimossRadGrid)x).Culture = new CultureInfo(LanguageHelper.CustomCulture);
                }
                else if (x is RadDatePicker)
                {
                    ((RadDatePicker)x).Culture = new CultureInfo(LanguageHelper.CustomCulture);
                }
            });
        }

        /// <summary>
        /// Cultura actual de la página
        /// </summary>
        protected string Cultura
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name;
            }
        }
    }
}