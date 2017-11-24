using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;

namespace SAM.BusinessObjects.Catalogos
{
    public class ColorBO
    {
        private static readonly object _mutex = new object();
        private static ColorBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private ColorBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase ColorBO
        /// </summary>
        /// <returns></returns>
        public static ColorBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new ColorBO();
                    }
                }
                return _instance;
            }
        }

        public Color Obtener(int colorID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Color.Where(x => x.ColorID == colorID).SingleOrDefault();
            }
        }

        public List<Color> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Color.ToList();
            }
        }

        public Color ObtenerPorHex(string hexadecimal)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Color.Where(x => x.CodigoHexadecimal == hexadecimal).SingleOrDefault();
            }
        }
    }
}
