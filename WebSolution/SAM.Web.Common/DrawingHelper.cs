using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using Ghostscript.NET;
using Ghostscript.NET.Rasterizer;
using SAM.Common;
using System.Drawing.Imaging;
using System;
using Image = System.Drawing.Image;

namespace SAM.Web.Common
{
    public static class DrawingHelper
    {
        private static readonly object Mutex = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="drawingName"></param>
        /// <returns></returns>
        public static DrawingPage[] GetImagePathsForDrawing(string projectName, string drawingName)
        {

            drawingName = drawingName.Replace("\"", "");
            string imagesFolder = Configuracion.CalidadRutaDossier + @"\" + projectName + @"\Reportes\Dibujo" + @"\Paginas-" + drawingName + @"\";
            string pdfFile = Configuracion.CalidadRutaDossier + @"\" + projectName + @"\Reportes\Dibujo" + @"\" + drawingName + ".pdf";

            if (Directory.Exists(imagesFolder))
            {
                return SortedImages(imagesFolder, projectName, drawingName);
            }

            if (File.Exists(pdfFile))
            {
                GenerateImages(imagesFolder, pdfFile);
                return SortedImages(imagesFolder, projectName, drawingName);
            }

            return new DrawingPage [0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagesFolder"></param>
        /// <param name="pdfFile"></param>
        private static void GenerateImages(string imagesFolder, string pdfFile)
        {
            lock (Mutex)
            {
                if (Directory.Exists(imagesFolder))
                {
                    //handle race condition
                    return;
                }

                Queue<Tuple<string, byte[]>> images = new Queue<Tuple<string, byte[]>>();
                GhostscriptRasterizer rasterizer = new GhostscriptRasterizer();
                bool allGenerated;

                try
                {
                    GhostscriptVersionInfo version = GhostscriptVersionInfo.GetLastInstalledVersion(GhostscriptLicense.GPL | GhostscriptLicense.AFPL, GhostscriptLicense.GPL);
                    rasterizer.Open(pdfFile, version, false);

                    for (int pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                    {
                        string pageFilePath = Path.Combine(imagesFolder, pageNumber.ToString(CultureInfo.CurrentCulture).PadLeft(3, '0') + ".jpg");
                        Image img = rasterizer.GetPage(96, 96, pageNumber);
                        images.Enqueue(Tuple.Create(pageFilePath, img.ToJpgArray()));
                    }

                    allGenerated = true;
                }
                catch
                {
                    allGenerated = false;
                }
                finally
                {
                    rasterizer.Close();
                    rasterizer.Dispose();
                    rasterizer = null;
                }

                if (allGenerated && images.Count > 0)
                {
                    Directory.CreateDirectory(imagesFolder);

                    while (images.Count > 0)
                    {
                        Tuple<string, byte[]> item = images.Dequeue();
                        string path = item.Item1;

                        using (MemoryStream ms = new MemoryStream(item.Item2))
                        {
                            using (Image img = Image.FromStream(ms))
                            {
                                if (img.Width < img.Height)
                                {
                                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                }

                                img.Save(path, ImageFormat.Jpeg);
                            }
                        }
                    }
                }                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static byte[] ToJpgArray(this Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagesFolder"></param>
        /// <param name="projectName"></param>
        /// <param name="drawingName"></param>
        /// <returns></returns>
        private static DrawingPage[] SortedImages(string imagesFolder, string projectName, string drawingName)
        {
            DirectoryInfo directory = new DirectoryInfo(imagesFolder);
            FileInfo [] files = directory.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);

            return 
                (from file in files
                 orderby file.Name
                 select new DrawingPage
                 {
                     DrawingName = drawingName,
                     PageName = Path.GetFileNameWithoutExtension(file.Name),
                     Extension = file.Extension,
                     ProjectName = projectName
                 }).ToArray();
        }
    }
}
