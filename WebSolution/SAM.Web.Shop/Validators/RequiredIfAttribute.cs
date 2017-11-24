using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SAM.Web.Shop.Validators
{
    public class RequiredIfAttribute : RequiredAttribute, IClientValidatable
    {
        private string PropertyName { get; set; }
        private object DesiredValue { get; set; }

        public RequiredIfAttribute(string properyName, object desiredValue)
        {
            PropertyName = properyName;
            DesiredValue = desiredValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();
            object propValue = type.GetProperty(PropertyName).GetValue(instance, null);

            if (propValue != null && propValue.ToString() == DesiredValue.ToString())
            {
                return base.IsValid(value, context);
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new ModelClientValidationRule
            {
                ValidationType = "requiredif",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };

            rule.ValidationParameters.Add("source", PropertyName);
            rule.ValidationParameters.Add("value", DesiredValue.ToString());

            yield return rule;
        }
    }
}