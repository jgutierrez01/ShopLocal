using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mimo.Framework.Common;

using SAM.BusinessObjects.Utilerias;
using Mimo.Framework.Extensions;
using SAM.Entities.Personalizadas;

namespace SAM.Web.Controles.Calidad.SegSpool
{
    public abstract class ControlSegSpool : UserControl
    {
        protected internal void MapGeneric(DetGrdSeguimientoSpool detalle)
        {
            Dictionary<string,Type> props = ReflectionUtils.Properties(detalle);
            Controls.IterateRecursively(c =>
            {
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
                            if(c is CheckBox)
                            {
                                if (tmp.HasValue)
                                {
                                    ReflectionUtils.SetValueFromBoolean(c, tmp.Value, "Checked");
                                }
                            }else 
                            {
                                if(tmp.HasValue)
                                {
                                    value = TraductorEnumeraciones.TextoSiNo(tmp.Value);
                                }    
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
                if (c is CheckBox)
                {
                    ((CheckBox)c).Enabled = false;
                }
            });
            Map(detalle);
        }

        protected internal abstract void Map(DetGrdSeguimientoSpool detalle);

       
    }
}