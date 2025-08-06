using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

#if NET || NETCOREAPP
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
#else
using System.Web.Mvc;
#endif

namespace Our.Umbraco.DataAnnotations.Conditionals
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
#if NET || NETCOREAPP
    public abstract class ConditionalValidationAttribute : ValidationAttribute, IClientModelValidator
#else
    public abstract class ConditionalValidationAttribute : ValidationAttribute, IClientValidatable
#endif
    {
        protected readonly ValidationAttribute InnerAttribute;

        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }

        protected abstract string ValidationName { get; }

        protected virtual IDictionary<string, object> GetExtraValidationParameters()
        {
            return new Dictionary<string, object>();
        }

        protected ConditionalValidationAttribute(ValidationAttribute innerAttribute, string dependentProperty, object targetValue)
        {
            InnerAttribute = innerAttribute ?? throw new ArgumentNullException(nameof(innerAttribute));
            DependentProperty = dependentProperty ?? throw new ArgumentNullException(nameof(dependentProperty));
            TargetValue = targetValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(DependentProperty, BindingFlags.Public | BindingFlags.Instance);

            if (field != null)
            {
                var dependentValue = field.GetValue(validationContext.ObjectInstance, null);

                if ((dependentValue == null && TargetValue == null) ||
                    (dependentValue != null && dependentValue.Equals(TargetValue)))
                {
                    if (!InnerAttribute.IsValid(value))
                    {
                        return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }

#if NET || NETCOREAPP
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            string depProp = BuildDependentPropertyId(context);
            string targetValue = (TargetValue ?? "").ToString();

            if (TargetValue is bool)
                targetValue = targetValue.ToLower();

            var attributes = new Dictionary<string, string>
            {
                { "data-val", "true" },
                { $"data-val-{ValidationName}", FormatErrorMessage(context.ModelMetadata.GetDisplayName()) },
                { $"data-val-{ValidationName}-dependentproperty", depProp },
                { $"data-val-{ValidationName}-targetvalue", targetValue }
            };

            foreach (var param in GetExtraValidationParameters())
            {
                attributes.Add($"data-val-{ValidationName}-{param.Key}", param.Value?.ToString());
            }

            foreach (var kvp in attributes)
            {
                context.Attributes[kvp.Key] = kvp.Value;
            }
        }

        private string BuildDependentPropertyId(ClientModelValidationContext context)
        {
            // This can be customized further if needed for nested view contexts
            return context.ModelMetadata.ContainerType?.GetProperty(DependentProperty)?.Name ?? DependentProperty;
        }

#else
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = ValidationName
            };

            string depProp = BuildDependentPropertyId(metadata, context as ViewContext);
            string targetValue = (TargetValue ?? "").ToString();

            if (TargetValue is bool)
                targetValue = targetValue.ToLower();

            rule.ValidationParameters.Add("dependentproperty", depProp);
            rule.ValidationParameters.Add("targetvalue", targetValue);

            foreach (var param in GetExtraValidationParameters())
            {
                rule.ValidationParameters.Add(param.Key, param.Value);
            }

            yield return rule;
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(DependentProperty);
            var thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
            {
                depProp = depProp.Substring(thisField.Length);
            }

            return depProp;
        }
#endif
    }
}