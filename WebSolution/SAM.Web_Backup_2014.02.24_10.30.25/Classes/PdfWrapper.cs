using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAM.Web.Classes
{
    public enum OrientacionPagina
    {
        Vertical = 1,
        Horizontal = 2
    }

    public class PdfWrapper
    {
        public byte[] Contenido { get; set; }
        public OrientacionPagina Orientacion { get; set; }

        public PdfWrapper(byte[] arr, OrientacionPagina orientacion)
        {
            Contenido = arr;
            Orientacion = orientacion;
        }

        public PdfWrapper(byte [] arr)
        {
            Contenido = arr;
            Orientacion = OrientacionPagina.Vertical;
        }
    }
}