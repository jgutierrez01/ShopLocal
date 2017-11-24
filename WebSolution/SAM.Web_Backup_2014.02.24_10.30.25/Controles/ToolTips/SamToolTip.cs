using System.Web.UI;

namespace SAM.Web.Controles.ToolTips
{
    public abstract class SamToolTip: UserControl
    {
        public abstract bool BindInfo(object data);
    }
}
