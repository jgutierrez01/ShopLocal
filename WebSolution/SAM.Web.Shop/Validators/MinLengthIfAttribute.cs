using System;
using System.ComponentModel.DataAnnotations;

namespace SAM.Web.Shop.Validators
{
    public class MinLengthIfAttribute : MinLengthAttribute
    {
        private string PropertyName { get; set; }
        private object DesiredValue { get; set; }

        public MinLengthIfAttribute(int minLength, string properyName, object desiredValue) : base(minLength)
        {
            PropertyName = properyName;
            DesiredValue = desiredValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();
            object propValue = type.GetProperty(PropertyName).GetValue(instance, null);

            if (propValue.ToString() == DesiredValue.ToString())
            {
                return base.IsValid(value, context);
            }

            return ValidationResult.Success;
        }
    }
}