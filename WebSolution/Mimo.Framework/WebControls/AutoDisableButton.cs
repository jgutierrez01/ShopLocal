using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Globalization;

namespace Mimo.Framework.WebControls
{
    [ToolboxData("<{0}:AutoDisableButton runat=server></{0}:AutoDisableButton>")]
    public class AutoDisableButton : Button
    {
        private string _TextoEnviando = "Wait...";


        [Bindable(true),
        Category("Appearance"),
        DefaultValue("Enviando..."),
        Description("Espere un momento...")]
        public string TextoEnviando
        {
            get
            {
                return _TextoEnviando;
            }

            set
            {
                _TextoEnviando = value;
            }
        }

        /// <summary>
        /// Constructor, Se establece que el control no va a causar validación pues eso se hara de manera manual en esta clase
        /// </summary>
        public AutoDisableButton()
        {
            base.Text = "Enviar";
            
        }

        /// <summary>
        /// Registra el script de js
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            //Registramos la función de envío del botón
            Page.RegisterClientScriptBlock("fEnviar_" + ID, FunciónEnviarBotón());
            base.OnPreRender(e);
        }

        /// <summary>
        /// Creamos dos Divs diferentes uno para el boton antes y otro para el boton despues de pulsarlo
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            //Creamos el panel donde va el botón principal
            output.Write("<div id='div1_" + ID + "' style='display: inline'>");

            output.AddAttribute("onclick", "Enviar_" + ID + "('" + ID + "');");
            base.Render(output);

            output.Write("</div>");

            if (CultureInfo.CurrentCulture.Name == "en-US")
            {
                TextoEnviando = "Wait ...";
            }
            else
            {
                TextoEnviando = "Espere ...";
            }

            //Creamos el panel y el botón secundario de envío
            output.Write("<div id='div2_" + ID + "' style='display: none'>");
            output.Write("<input disabled type=submit CssClass=\"boton\" value='" + TextoEnviando + "' />");
            output.Write("</div>");

        }

        /// <summary>
        /// Envia el script que hace cambiar al boton para que no sea pulsado dos veces
        /// </summary>
        /// <returns></returns>
        private string FunciónEnviarBotón()
        {
            string txt = "<script language='javascript'>";

            //txt += "function Enviar_" + ID + "(id) { " +
            //        "if (typeof(Page_ClientValidate) == 'function') { " +
            //                "if (Page_ClientValidate() == true ) {" +
            //                    "document.getElementById('div1_' + id).style.display = 'none';" +
            //                    "document.getElementById('div2_' + id).style.display = " +
            //                "'inline';	return true;" +
            //                "}" +
            //                "else {" +
            //                    "return false;" +
            //                "}" +
            //            "}" +
            //            "else {" +
            //                "document.getElementById('div1_' + id).style.display = 'none';" +
            //                "document.getElementById('div2_' + id).style.display = 'inline';" +
            //                "return true;" +
            //            "}" +
            //        "}</script>;";


            txt += "function Enviar_" + ID + "(id) { " +
                    "if (typeof(Page_ClientValidate) == 'function') { " +
                           
                                "document.getElementById('div1_' + id).style.display = 'none';" +
                                "document.getElementById('div2_' + id).style.display = " +
                            "'inline';	return true;" +
                           
                        "}" +
                        "else {" +
                            "document.getElementById('div1_' + id).style.display = 'none';" +
                            "document.getElementById('div2_' + id).style.display = 'inline';" +
                            "return true;" +
                        "}" +
                    "}</script>;";

            return txt;
        }
    }
}