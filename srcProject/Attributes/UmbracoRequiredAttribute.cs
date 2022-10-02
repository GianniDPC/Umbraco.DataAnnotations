using System;
using System.ComponentModel.DataAnnotations;
using Umbraco.DataAnnotations.Interfaces;

namespace Umbraco.DataAnnotations.Attributes
{
    /// <summary>
    ///     Validation attribute to indicate that a property field or parameter is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
    AllowMultiple = false)]
    public sealed class UmbracoRequiredAttribute : RequiredAttribute, IUmbracoValidationAttribute
    {
        public string DictionaryKey { get; set; } = "RequiredError";

        public UmbracoRequiredAttribute()
            : base()
        {
        }

        public override string FormatErrorMessage(string name)
        {
            ErrorMessage = UmbracoDictionary.GetDictionaryValue(DictionaryKey);
            return base.FormatErrorMessage(name);
        }
    }
}
