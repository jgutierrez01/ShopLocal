using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.Entities;
using Mimo.Framework.Data;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Modelo;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using SAM.BusinessObjects.Validations;

namespace SAM.BusinessObjects.Catalogos
{
    public class DestinoBO
    {
        public event TableChangedHandler DestinoCambio;
        private static readonly object _mutex = new object();
        private static DestinoBO _instance;


        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private DestinoBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase AceroBO
        /// </summary>
        /// <returns></returns>
        public static DestinoBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new DestinoBO();
                    }
                }
                return _instance;
            }
        }

        public List<Destino> ObtenerDestinos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Destino.ToList();
            }
        }


    }
}
