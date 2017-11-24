using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.WebControls;
using SAM.Entities.Personalizadas;
using Mimo.Framework.Extensions;
using SAM.BusinessObjects.Ingenieria;
using System.Globalization;

namespace SAM.Web.Controles.Spool
{
    /// <summary>
    /// Control que en modo sólo lectura despliega la información de los materiales de un spool
    /// </summary>
    public partial class MaterialRO : UserControl
    {
        public bool EsRevision
        {
            get
            {
                return ViewState["DetSpoolRevisionMat"].SafeBoolParse();
            }
            set
            {
                ViewState["DetSpoolRevisionMat"] = value;
            }
        }

        /// <summary>
        /// El objeto enviado debe ser una lista con los materiales del spool
        /// </summary>
        /// <param name="entity">Debe ser un List[DetMaterialSpool]</param>
        public void Map(object entity, bool revision = false)
        {
            EsRevision = revision;
            List<DetMaterialSpool> lst = (List<DetMaterialSpool>)entity;
            repMateriales.DataSource = lst.OrderBy(x => x.Etiqueta);
            repMateriales.DataBind();
        }

        private void MarcarMaterialesOmitidos(Repeater repMateriales)
        {
            foreach (RepeaterItem item in repMateriales.Items)
            {
                string materialSpoolID = item.FindControl("MaterialSpoolID").ToString();
                
            }
        }

        public void Unmap(object entity)
        {
            //no hay unmap
        }

        protected void repMateriales_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (EsRevision)
            {
                RepeaterItem item = e.Item;
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    // Recuperamos el id  
                    int materialSpoolID = ((SAM.Entities.Personalizadas.DetMaterialSpool)(e.Item.DataItem)).MaterialSpoolID; //.Row["MaterialSpoolID"].ToString();
                    bool tieneODTM = MaterialSpoolBO.Instance.TieneODTMaterial(materialSpoolID);
                    if (!tieneODTM)
                    {
                        Image imagenObservaciones = (Image)item.FindControl("imgObservaciones");
                        imagenObservaciones.ToolTip = CultureInfo.CurrentCulture.Name == "en-US" ?
                                                        "This material was omitted for this process." : "Este material fue omitido para este proceso.";
                        imagenObservaciones.Visible = true;
                    }
                }
            }
        }
    }
}