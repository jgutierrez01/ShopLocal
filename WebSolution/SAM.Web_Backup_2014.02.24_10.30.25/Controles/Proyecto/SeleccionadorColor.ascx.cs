using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SAM.BusinessLogic;
using SAM.Entities.Cache;
using Telerik.Web.UI;
using System.Drawing;
using System.ComponentModel;
using SAM.BusinessObjects.Utilerias;

namespace SAM.Web.Controles.Proyecto
{
    public partial class SeleccionadorColor : System.Web.UI.UserControl
    {

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<ColorCache> lstColor = CacheCatalogos.Instance.ObtenerColores();

                lstColor.ForEach(x => picker.Items.Add(new ColorPickerItem
                {
                    Title = x.Nombre,
                    Value = ColorTranslator.FromHtml(x.CodigoHexadecimal)
                }));
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (!string.IsNullOrEmpty(txtColorSeleccionado.Text))
                {
                    //settear el nombre en el label
                    lblNombreColor.Text = txtColorSeleccionado.Text;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int ? obtenerColorDelPicker()
        {
            if (picker.SelectedColor == null)
            {
                return null;
            }

            string colorSeleccionado = ColorTranslator.ToHtml(picker.SelectedColor).ToLowerInvariant();

            int? colorID = CacheCatalogos.Instance
                                         .ObtenerColores()
                                         .Where(x => x.CodigoHexadecimal.Equals(colorSeleccionado, StringComparison.InvariantCultureIgnoreCase))
                                         .Select(x => x.ID)
                                         .SingleOrDefault();

            return colorID;
        }

        private void establecerColorAlPicker(int? colorID)
        {
            if (colorID.HasValue)
            {
                ColorCache color = CacheCatalogos.Instance
                                                 .ObtenerColores()
                                                 .Where(x => x.ID == colorID.Value)
                                                 .SingleOrDefault();

                if (color != null)
                {
                    picker.SelectedColor = ColorTranslator.FromHtml(color.CodigoHexadecimal);
                    lblNombreColor.Text = color.Nombre;
                    txtColorSeleccionado.Text = color.Nombre;
                }
            }
        }

        public int ? ColorID
        {
            get
            {
                return obtenerColorDelPicker();
            }
            set
            {
                establecerColorAlPicker(value);
            }
        }

        protected void cusColorRequerido_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            args.IsValid = picker.SelectedColor != null;
        }

        [Browsable(true)]
        public bool EsRequerido
        {
            get
            {
                return cusColorRequerido.Enabled;
            }
            set
            {
                cusColorRequerido.Enabled = value;
            }
        }
    }
}