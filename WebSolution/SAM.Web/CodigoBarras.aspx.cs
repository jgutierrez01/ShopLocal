using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;

namespace SAM.Web
{
    public partial class CodigoBarras : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string codigo = Request.QueryString["Texto"];
            float altoBarras = -1;
            float.TryParse(Request.QueryString["Alto"], out altoBarras);

            Barcode128 bcode = new Barcode128();
            bcode.CodeType = BarcodeEAN.EAN13;
            bcode.Code = codigo;
            bcode.StartStopText = false;
            bcode.GenerateChecksum = false;
            bcode.Size = 1f;            

            if (altoBarras > -1)
            {
                bcode.BarHeight = altoBarras;
            }

            using (System.Drawing.Image img = bcode.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White))
            {
                Response.Clear();
                Response.ContentType = "image/gif";
                img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Gif);
            }
        }
    }
}