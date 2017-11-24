using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using SAM.Entities.Personalizadas;

namespace SAM.Web.Controles.Spool
{
    /// <summary>
    /// Control que en modo sólo lectura despliega la información de los materiales de un spool
    /// </summary>
    public partial class MaterialROHistorico : UserControl, IMappable
    {
        /// <summary>
        /// El objeto enviado debe ser una lista con los materiales del spool
        /// </summary>
        /// <param name="entity">Debe ser un List[DetMaterialSpool]</param>
        public void Map(object entity)
        {
            List<DetMaterialSpoolHistorico> lst = (List<DetMaterialSpoolHistorico>)entity;
            repMateriales.DataSource = lst.OrderBy(x => x.Etiqueta);
            repMateriales.DataBind();
        }

        public void Unmap(object entity)
        {
            //no hay unmap
        }
    }
}