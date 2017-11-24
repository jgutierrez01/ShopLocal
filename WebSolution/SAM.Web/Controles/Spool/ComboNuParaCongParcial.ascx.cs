using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Extensions;
using System.ComponentModel;
using Resources;

namespace SAM.Web.Controles.Spool
{
    public partial class ComboNuParaCongParcial : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public int MaterialSpoolID
        {
            get
            {
                return hdnMaterialSpoolID.Value.SafeIntParse();
            }
            set
            {
                hdnMaterialSpoolID.Value = value.ToString();
            }
        }

        [Browsable(true)]
        public int Cantidad
        {
            get
            {
                return hdnCantidad.Value.SafeIntParse();
            }
            set
            {
                hdnCantidad.Value = value.ToString();
            }
        }  

        [Browsable(false)]
        public int NumeroUnicoSeleccionadoID
        {
            get
            {
                return rcbNumeroUnico.SelectedValue.Split('-')[0].SafeIntParse();
            }
        }


        [Browsable(false)]
        public string CodigoSegmento
        {
            get
            {
                return rcbNumeroUnico.Text;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected void cusCombo_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = !string.IsNullOrWhiteSpace(rcbNumeroUnico.SelectedValue);
        }
    }
}