using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace Mimo.Framework.Common
{
    public static class ReflectionUtils
    {

        public static string GetStringValue(object entity, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return string.Empty;

            string retVal = string.Empty;

            bool isSubproperty = propertyName.IndexOf(".") != -1;

            Type entityType = entity.GetType();
            PropertyInfo property = entityType.GetProperty(isSubproperty
                                           ? propertyName.Substring(0, propertyName.IndexOf("."))
                                           : propertyName);
            if (isSubproperty)
            {
                return GetStringValue(property.GetValue(entity, null), propertyName.Substring(propertyName.IndexOf(".") + 1));
            }

            object value = property.GetValue(entity, null);

            if ( value != null )
            {
                retVal = value.ToString();
            }

            return retVal;
        }

        public static int? GetInt32Value(object entity, string propertyName)
        {
            int? retVal = null;
            Type entityType = entity.GetType();
            PropertyInfo property = entityType.GetProperty(propertyName);
            object value = property.GetValue(entity, null);

            if (value != null)
            {
                retVal = (int)value;
            }

            return retVal;
        }

        public static DateTime? GetDateTimeValue(object entity, string propertyName)
        {
            DateTime? retVal = null;
            Type entityType = entity.GetType();
            PropertyInfo property = entityType.GetProperty(propertyName);
            object value = property.GetValue(entity, null);

            if (value != null)
            {
                retVal = (DateTime)value;
            }

            return retVal;
        }

        public static bool? GetBooleanValue(object entity, string propertyName)
        {
            bool? retVal = null;
            Type entityType = entity.GetType();
            PropertyInfo property = entityType.GetProperty(propertyName);
            object value = property.GetValue(entity, null);

            if (value != null)
            {
                retVal = (bool)value;
            }

            return retVal;
        }
        
        public static void SetValueFromBoolean(object entity, bool value, string propertyName)
        {
            Type entityType = entity.GetType();
            PropertyInfo property = entityType.GetProperty(propertyName);

            property.SetValue(entity, value, null);
        }

        public static void SetValueFromDateTime(object entity, DateTime? value, string propertyName)
        {
            Type entityType = entity.GetType();
            PropertyInfo property = entityType.GetProperty(propertyName);

            property.SetValue(entity, value, null);
        }

        public static void SetValueFromString(object entity, string value, string propertyName, bool emptyIsNull)
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            bool isSubproperty = propertyName.IndexOf(".") != -1;
            
            Type entityType = entity.GetType();
            PropertyInfo property =
                entityType.GetProperty(isSubproperty
                                           ? propertyName.Substring(0, propertyName.IndexOf("."))
                                           : propertyName);
            if(isSubproperty)
            {
                SetValueFromString(property.GetValue(entity, null), value,
                                   propertyName.Substring(propertyName.IndexOf(".") + 1), emptyIsNull);
                return;
            }

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(entity);

            PropertyDescriptor prop = props[propertyName];

            property.SetValue(entity, prop.Converter.ConvertFromInvariantString(value), null);
        }

        /// <summary>
        /// Return a dictionary made of the Properties of an entity as Keys and its Type as Value
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Dictionary<string,Type> Properties(object entity)
        {
            if(entity == null) return new Dictionary<string, Type>();
            Type entityType = entity.GetType();
            return entityType.GetProperties().ToDictionary(x=> x.Name, y=> y.PropertyType);
        }
    }
}
