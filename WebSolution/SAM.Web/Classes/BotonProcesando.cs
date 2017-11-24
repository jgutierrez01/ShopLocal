using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;

namespace SAM.Web.Classes
{
    [DefaultProperty("ID")]
    [ToolboxData("<{0}:BotonProcesando runat=server></{0}:BotonProcesando>")]
    public class BotonProcesando : Button
    {
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            OnClientClick = OnClientClick + string.Format(  ";Sam.Utilerias.PostbackProcesando({0},'{1}');",
                                                            CausesValidation.ToString().ToLowerInvariant(), 
                                                            ValidationGroup);
        }
    }
}