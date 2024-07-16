#if NET || NETCOREAPP
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
#else
using System.Web.Mvc;
using System.Collections.Generic;
#endif
using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    /// Specifies that the property must be true.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
#if NET || NETCOREAPP
    public sealed class UmbracoMustBeTrueAttribute : ValidationAttribute, IUmbracoValidationAttribute, IClientModelValidator
#else
    public sealed class UmbracoMustBeTrueAttribute : ValidationAttribute, IUmbracoValidationAttribute, IClientValidatable
#endif
    {
        public string DictionaryKey { get; set; } = "MustBeTrueError";

        public UmbracoMustBeTrueAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }

#if NET || NETCOREAPP
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var errorMessage = ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);

            context.Attributes.Add("data-val-required", errorMessage);
            context.Attributes.Add("data-val-mustbetrue", errorMessage); // Add attributes for client-side validation
            context.Attributes.Add("data-val", "true");
        }
#else
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            var rule = new ModelClientValidationRule()
            {
                ValidationType = "mustbetrue",
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
            };

            yield return rule;
        }
#endif
    }
}
