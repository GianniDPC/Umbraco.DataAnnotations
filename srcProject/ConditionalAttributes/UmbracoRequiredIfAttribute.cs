using System;
using System.ComponentModel.DataAnnotations;

namespace Umbraco.DataAnnotations.ConditionalAttributes
{
    /// <summary>
    ///     Conditional validation attribute to indicate that a property field or parameter is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public sealed class UmbracoRequiredIfAttribute : RequiredAttribute
    {
        public string DictionaryKey { get; set; }

        private string PropertyName { get; set; }
        private object DesiredValue { get; set; }

        public UmbracoRequiredIfAttribute(string propertyName, object desiredvalue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredvalue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();
            object propertyvalue = type.GetProperty(PropertyName).GetValue(instance, null);

            if (propertyvalue?.ToString() == DesiredValue.ToString())
            {
                ValidationResult result = base.IsValid(value, context);
                return result;
            }
            return ValidationResult.Success;
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
