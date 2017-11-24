using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SAM.Web.Classes
{
    public delegate void PaginaCambioHandler(object sender, ArgumentosPaginador args);

    public class ArgumentosPaginador : EventArgs
    {
        /// <summary>
        /// Indice base cero
        /// </summary>
        public int PaginaActual { get; set; }
    }
}