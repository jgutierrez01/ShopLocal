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
    public partial class ComboNuParaAsignacion : System.Web.UI.UserControl
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
        public string EtiquetaMaterial
        {
            get
            {
                if (ViewState["EtiquetaMaterial"] != null)
                {
                    return ViewState["EtiquetaMaterial"].ToString();
                }

                return string.Empty;
            }
            set
            {
                ViewState["EtiquetaMaterial"] = value;
                cusCombo.ErrorMessage = string.Format(MensajesErrorWeb.Asignacion_NuDebeEspecificarse, value);
            }
        }

        [Browsable(true)]
        public string ValidationGroup
        {
            get
            {
                return cusCombo.ValidationGroup;
            }
            set
            {
                cusCombo.ValidationGroup = value;
                rcbNumeroUnico.ValidationGroup = value;
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
        public string Segmento
        {
            get
            {
                string [] arr = rcbNumeroUnico.SelectedValue.Split('-');

                if ( arr.Length > 1 )
                {
                    return arr[arr.Length - 1];
                }

                return null;
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