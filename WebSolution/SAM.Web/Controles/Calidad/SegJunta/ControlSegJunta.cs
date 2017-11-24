using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;
using SAM.BusinessObjects.Utilerias;
using SAM.Entities.Grid;
using Mimo.Framework.Extensions;

namespace SAM.Web.Controles.Calidad.SegJunta
{
    public abstract class ControlSegJunta : UserControl
    {
        protected internal void MapGeneric(GrdSeguimientoJunta detalle)
        {
            //Obtenemos una lista de las propiedades del objeto GrdSeguimientoJunta
            Dictionary<string,Type> props = ReflectionUtils.Properties(detalle);

            //Iteramos todos los controles dentro de este Control
            Controls.IterateRecursively(c =>
            {
                //Iteramos las propiedades del objeto GrdSeguimientoJunta
                foreach (string prop in props.Keys)
                {
                    if (!string.IsNullOrEmpty(c.ID) && c.ID.Equals(prop))
                    {
                        string value="";
                        Type type = props[prop];
                        if (type.FullName.IndexOf("DateTime") != -1)
                        {
                            DateTime? date = ReflectionUtils.GetDateTimeValue(detalle, prop);
                            if (date.HasValue)
                            {
                                value = date.Value.ToShortDateString();
                            }
                        }
                        else if (type.FullName.IndexOf("Boolean") != -1)
                        {
                            bool? tmp = ReflectionUtils.GetBooleanValue(detalle, prop);
                            if(tmp.HasValue)
                            {
                                value = TraductorEnumeraciones.TextoSiNo(tmp.Value);    
                            }
                        }
                        else
                        {
                            value = ReflectionUtils.GetStringValue(detalle, prop);
                        }


                        if (!string.IsNullOrEmpty(value))
                        {
                            ReflectionUtils.SetValueFromString(c, value, "Text", false);
                        }
                    }
                }
                if(c is TextBox)
                {
                    ((TextBox) c).ReadOnly = true;
                }
            });
            Map(detalle);
        }

        protected internal abstract void Map(GrdSeguimientoJunta detalle);

       
    }
}