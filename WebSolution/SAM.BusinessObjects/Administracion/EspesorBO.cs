using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAM.BusinessObjects;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using System.Data;
using SAM.BusinessObjects.Excepciones;
using Mimo.Framework.Extensions;
using SAM.Entities.Grid;

namespace SAM.BusinessObjects.Administracion
{
    public class EspesorBO
    {
        private static readonly object _mutex = new object();
        private static EspesorBO _instance;

        /// <summary>
        /// Constructor privado para implementar patron Singlenton
        /// </summary>
        private EspesorBO()
        {
        }

        /// <summary>
        /// Obtiene la instancia de la clase EspesorBO
        /// </summary>
        /// <returns></returns>
        public static EspesorBO Instance
        {
            get
            {

                lock (_mutex)
                {
                    if (_instance == null)
                    {
                        _instance = new EspesorBO();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Espesor> ObtenerTodos()
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Espesor.ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="diametroID"></param>
        /// <param name="cedulaID"></param>
        /// <returns></returns>
        public Espesor ObtenerPorDiametroCedula(int diametroID, int cedulaID)
        {
            using (SamContext ctx = new SamContext())
            {
                return ctx.Espesor.Where(x => x.DiametroID == diametroID && x.CedulaID == cedulaID).SingleOrDefault();
            }
        }
    }
}
