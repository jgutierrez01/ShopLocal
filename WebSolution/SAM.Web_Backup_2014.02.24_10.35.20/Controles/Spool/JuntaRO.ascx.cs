using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;

namespace SAM.Web.Controles.Spool
{
    /// <summary>
    /// Control que en modo sólo lectura despliega la información de las juntas de un spool
    /// </summary>
    public partial class JuntaRO : UserControl, IMappable
    {
        /// <summary>
        /// El objeto enviado debe ser una lista con las juntas del spool
        /// </summary>
        /// <param name="entity">Debe ser un List[DetJuntaSpool]</param>
        public void Map(object entity)
        {
            List<DetJuntaSpool> lst = (List<DetJuntaSpool>)entity;
            repJuntas.DataSource = lst.OrderBy(x => x.Etiqueta);
            repJuntas.DataBind();
        }

        public void Unmap(object entity)
        {
            //No hay unmap
        }
    }
}