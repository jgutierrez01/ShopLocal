using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using SAM.Entities.Personalizadas;

namespace SAM.Web.Controles.SpoolOdt
{
    /// <summary>
    /// Versión solo lectura de los cortes de un spool en particular
    /// </summary>
    public partial class CorteOdtRO : UserControl, IMappable
    {
        /// <summary>
        /// El objeto enviado debe ser una lista con los cortes del spool
        /// </summary>
        /// <param name="entity">Debe ser un List[DetCorteSpool]</param>
        public void Map(object entity)
        {
            List<DetCorteSpool> lst = (List<DetCorteSpool>)entity;
            repCortes.DataSource = lst.OrderBy(x => x.EtiquetaMaterial).ThenBy(x => x.EtiquetaSegmento);
            repCortes.DataBind();
        }

        /// <summary>
        /// no necesario
        /// </summary>
        /// <param name="entity"></param>
        public void Unmap(object entity)
        {
            //no se usa
        }
    }
}