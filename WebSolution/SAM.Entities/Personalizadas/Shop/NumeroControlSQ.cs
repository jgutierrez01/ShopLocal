using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAM.Entities.Personalizadas.Shop
{
    public class LayoutGridSQ
    {
        public string NumeroControl { get; set; }
        public int SpoolID { get; set; }
        public int CuadranteID { get; set; }
        public string Cuadrante { get; set; }
        public int Accion { get; set; }
        public int OrdenTrabajoSpoolID { get; set; }
        public string SqCliente { get; set; }
        public string SQ { get; set; }
        public bool TieneHoldIngenieria { get; set; }
        public LayoutGridSQ()
        {
            this.NumeroControl = "";
            this.SpoolID = 0;
            this.Cuadrante = "";
            this.CuadranteID = 0;
            this.Accion = 0;
            this.OrdenTrabajoSpoolID = 0;
            this.SqCliente = null;
            this.SQ = null;
            this.TieneHoldIngenieria = false;
        }
    }

    public class MessageVM
    {
        public string CssClassName { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
